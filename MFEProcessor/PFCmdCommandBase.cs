using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Linq;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;
using Agilent.MassSpectrometry.DataAnalysis;

namespace MFEProcessor
{
    /// <summary>
    /// base class for Profinder command objects
    /// </summary>
    [Obfuscation]
    public abstract class PFCmdCommandBase : QualCommandBase
    {
        /// <summary>
        /// Reference to ProfinderLogic instance
        /// </summary>
        protected ProfinderLogic m_PFLogic;

        /// <summary>
        /// Constant ".clc"
        /// </summary>
        protected const string CLC_EXTENSION = ".clc";

        /// <summary>
        /// Constant ".unfiltered.rmc"
        /// </summary>
        protected const string RMC_UNFILTERED_EXTENSION = ".unfiltered.rmc";

        /// <summary>
        /// Constant ".rmc"
        /// </summary>
        protected const string RMC_EXTENSION = ".rmc";

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="appManager"></param>
        public PFCmdCommandBase(ProfinderLogic appManager)
            : base(appManager)
        {
            m_PFLogic = appManager;
        }

        /// <summary>
        /// Allow individual commands to override the list of analyses to process
        /// (i.e. CmdPFManualFbFCpd)
        /// </summary>
        protected virtual IEnumerable<Analysis> Analyses
        {
            get
            {
                return from p in AnalysisPaths
                       join a in m_PFLogic.DataHeirarchy.AnalysisStore.Values.Cast<Analysis>()
                              on p equals a.FilePath
                       select a;
            }
        }
        /// <summary>
        /// Returns an ordered list of analysis paths
        /// (as specified in the "Add/Remove Samples" dialog)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> AnalysisPaths
        {
            get
            {
                // query the "Add/Remove Samples" dialog PSet for the user-specified file order
                var psetFileList = m_PFLogic[QualInMemoryMethod.ParamDataFileList] as PSetDataFileList;
                return psetFileList.SelectedFileName.Where(bt => bt.AutoShow).Select(bt => bt.FileName);
            }
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>actions</returns>
        public override object Do()
        {
            DateTime startTime = DateTime.Now;

            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Trace.WriteLineIf(Ts.TraceInfo, Tm.Start);
            if (Ts.TraceInfo)
                Trace.WriteLine("Executing : " + Description);

            try
            {
                var memoryManager = m_AppManager as IManageMemory;
                bool memoryLow = memoryManager.IsWeakReferencePreferred();

                if (memoryLow)
                {
                    // In case of low memory try to free up at least estimated 
                    // amount of memory 
                    int memoryEstimate = EstimatedMemoryRequiredInMB();
                    if (memoryEstimate > 0)
                    {
                        memoryManager.MakeMemoryAvailable(memoryEstimate);
                    }
                }

                // Do the actual command
                DoSpecialized();

                // Check if renormalization is needed 
                ReNormalizeResultsIfNeeded(GeneratedActions);

                PackageResults();
                if (memoryLow)
                {
                    // In case memory is low
                    // free up memory of objects which
                    // were marked hidden or deleted by this command
                    AppManager.ConvertDeletedAndHiddenDataItemsToWeakReference(GeneratedActions);
                }


                if (IsIdentificationCommand)
                {
                    ExecuteCombineAllIDResults();
                }


                return GeneratedActions;

            }
            catch (Exception e)
            {
                Trace.WriteLineIf(Ts.TraceError, Tm.Caught);
                Trace.WriteLineIf(Ts.TraceError, e);
                Trace.WriteLineIf(Ts.TraceError, e.StackTrace);
                if (e is MSDAApplicationException)
                {
                    ActOnMSDAException(e as MSDAApplicationException, GeneratedActions);
                    throw;
                }
                else
                {
                    m_AppManager.TraceApplicationLog();

                    if (e is OutOfMemoryException)
                    {
                        Trace.WriteLineIf(Ts.TraceError, "Mapping out of memory error to nice message");
                        throw new MSDAApplicationException(new QualUserMessage(QualMessage.NotEnoughMemoryAvailable));
                    }
                    else
                    {
                        // .NET 4.5 construct that preserves the stack trace
                        ExceptionDispatchInfo.Capture(e).Throw();
                        throw;
                    }
                }
            }
            finally
            {
                RaiseEventWithGeneneratedActions();
                Trace.WriteLineIf(Ts.TraceInfo, Tm.Finish);
                m_ExecutionTime = DateTime.Now - startTime;
            }

        }

        /// <summary>
        /// Derived class must override following function
        /// it will be called from the base class's Do function
        /// the derived command class should do the actions specific for that command
        /// </summary>
        protected abstract void DoSpecialized();

        /// <summary>
        /// Package results
        /// </summary>
        protected abstract void PackageResults();

        /// <summary>
        /// Override this function if the command is very memory intensive
        /// </summary>
        /// <returns></returns>
        internal virtual int EstimatedMemoryRequiredInMB()
        {
            return 0;
        }

        /// <summary>
        /// Override to get command parameters to be logged.
        /// Override here so that none of the other classes must implement this method
        /// </summary>
        /// <returns>object array</returns>
        public override object[] GetParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates output file name to export data to based on parameter settings 
        /// </summary>
        /// <param name="filename">Complete file path including filename</param>
        /// <returns>Generated file name</returns>
        public static string GetAutoIncrementedExportFile(string filename)
        {
            // we only have work to do if the file already exists
            if (!File.Exists(filename) && !Directory.Exists(filename))
                return filename;

            StringBuilder sb = new StringBuilder();
            sb.Append(Path.GetDirectoryName(filename));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(Path.GetFileNameWithoutExtension(filename));

            var tempDestinationPath = sb.ToString();
            var tempDestinationExtension = Path.GetExtension(filename);

            // starting with _1, append an autoincrementing file counter
            var fileCount = 1;
            while (File.Exists(tempDestinationPath + "_" + fileCount.ToString(CultureInfo.CurrentUICulture)
                               + tempDestinationExtension))
            {
                fileCount++;
            }

            sb.Clear();

            sb.Append(tempDestinationPath);
            sb.Append("_");
            sb.Append(fileCount.ToString(CultureInfo.CurrentCulture));
            sb.Append(tempDestinationExtension);
            return sb.ToString();
        }

        /// <summary>
        /// Get the correlation algorithm's alignment parameters (for rMFE and filters)
        /// </summary>
        /// <returns></returns>
        protected CnEngine.UserParameters GetAlignmentParameters()
        {
            var rv = new CnEngine.UserParameters();
            IPSetAlignmentInfo psetAlign = m_AppManager[QualDAMethod.ParamKeyAlignmentInformation] as IPSetAlignmentInfo;
            double[] rtTimeCorrelation = new[] { psetAlign.RTMinutes, psetAlign.RTPercent / 100 };
            double[] msTolerance = new[] { psetAlign.MassWindowDa / 1000, psetAlign.MassWindowPpm / 1000000 };

            rv.TimeToleranceFunctionForCorrelation = new FunctionLinear(rtTimeCorrelation);
            rv.MassToleranceFunction = new FunctionLinear(msTolerance);

            return rv;
        }

        /// <summary>
        /// Group pattern
        /// </summary>
        /// <param name="ionPolarity"></param>
        /// <param name="storageType"></param>
        /// <param name="fragmentorVoltage"></param>
        /// <returns></returns>
        private string GetFileName(IonPolarity ionPolarity, DesiredMSStorageType storageType, double fragmentorVoltage)
        {
            StringBuilder pFN = new StringBuilder();
            switch (storageType)
            {
                case DesiredMSStorageType.Peak:
                    pFN.Append("Cent");
                    break;
                case DesiredMSStorageType.Profile:
                    pFN.Append("Prof");
                    break;
                case DesiredMSStorageType.Unspecified:
                    // no suffix
                    break;
                default:
                    Debug.Assert(false, "Storage type not Centroid or Profile");
                    break;
            }

            switch (ionPolarity)
            {
                case IonPolarity.Positive:
                    pFN.Append("Pos");
                    break;
                case IonPolarity.Negative:
                    pFN.Append("Neg");
                    break;
                default:
                    // No polarity suffix
                    break;
            }

            if (fragmentorVoltage != 0.0)
            {
                pFN.AppendFormat("{0:F0}", fragmentorVoltage);
            }

            return pFN.ToString();
        }
    }
}
