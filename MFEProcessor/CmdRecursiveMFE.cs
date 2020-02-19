//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AgtQualAutomation
//{
//    class CmdRecursiveMFE
//    {
//    }
//}
using System.IO;
using System.Text.RegularExpressions;
using Agilent.MassSpectrometry.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

using Agilent.MassSpectrometry.DataAnalysis;
using RecursiveMfe;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;

namespace MFEProcessor
{
    /// <summary>
    /// Command to execute recursive Molecular Feature Extractor (rMFE)
    /// re-alignment and shuffling algorithm
    /// </summary>
    [CommandClass("CmdRecursiveMFE")]
    public class CmdRecursiveMFE : PFCmdCommandBase
    {
        private struct clcFileItem     // to keep item has the same count. Recursive engine requires this.
        {
            public string clcPath;
            public string rmcPath;
            public string clcName;
            public float scalingfactor;
        }
        List<clcFileItem> clcfileList = new List<clcFileItem>();
        private List<Analysis> m_analyses;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="appManager"></param>
        /// <param name="analysis"></param>
        public CmdRecursiveMFE(ProfinderLogic appManager, List<Analysis> analysis) : base(appManager)
        {
            m_analyses = analysis;
        }

        /// <summary>
        /// Override to get command parameters to be logged
        /// </summary>
        /// <returns>object array</returns>
        public override object[] GetParameters()
        {
            return new object[] { };
        }

        private bool IsGCData()
        {
            bool bRet = false;
            if (m_analyses.Count() > 0)
            {
                var analysis = m_analyses[0];
                if (analysis.DataAccess != null)
                {
                    IBDAFileInformation fileInfo = analysis.DataAccess.FileInformation;
                    if (fileInfo.MSScanFileInformation.IonModes == IonizationMode.EI)
                    {
                        bRet = true;
                    }
                }

            }
            return bRet;

        }

        /// <summary>
        /// Override to do the actual work of the command
        /// </summary>
        protected override void DoSpecialized()
        {
            //BatchExtractor.CommonUtility.LogMessage("Recursive MFE", this);
            var userParameters = GetAlignmentParameters();
            clcfileList.Clear();
            if (IsGCData() == true) // To do recursive MFE for GC data
            {
                return;
            }
            //var progressTracker = new PFEProgressTracker(m_AppManager, ProgressCategory.Task);

            try
            {
                //progressTracker.ReportProgress(ProgressStage.Starting, 0, new QualUserMessage(QualMessage.ExecutingFindCompounds));

                for (int i = 0; i < m_analyses.Count; i++)
                {
                    var m_analysis = m_analyses[i];
                    IFindCompoundsParameters findParams = GetFindCompoundsParams();
                    GetAnalysisSpecificParams(ref findParams, new object[] { m_analysis });
                    List<IonPolarity> polarityList = GetScanIonPolarities(findParams.DataAccess);
                    // for Allion data, not to use fragVoltage.
                    List<double> energyVoltages = null;
                    double lowE = double.NaN;
                    double[] highEs = null;
                    bool bMultipleCEs = false;

                    bool AllIonsData = FindCpdsUtilities.IsThisAllIonsData(findParams.DataAccess, out lowE, out highEs, out bMultipleCEs);
                    if (AllIonsData)
                    {
                        energyVoltages = new List<double>(1);
                        energyVoltages.Add(lowE);
                    }
                    else
                    {
                        energyVoltages = GetFragmentorVoltages(findParams.DataAccess);
                        if (energyVoltages.Count == 1)
                        {
                            energyVoltages[0] = 0.0;
                        }
                    }
                    // multiple time segemnt support. One segment per clc file. 
                    // No MFE data segment with no clc file generated. 
                    IBDADataAccess m_BdaDa = new BDADataAccess();
                    m_BdaDa.OpenDataFile(findParams.DataAccess.DataFileName, true);
                    IRange[] tempRange = m_BdaDa.GetTimeSegmentRanges();
                    int m_totalTimeSegment = tempRange.Length;
                    m_BdaDa.CloseDataFile();
                    //
                    double fragVoltage = 0.0;
                    // From B08, MFE data reader API has changed. It doesn't use the passed in multiple voltages. The reader handle it itself.
                    if (energyVoltages.Count > 1) // for multiple frag voltageas, we take the smallest one
                    {
                        // fragVoltage = energyVoltages.Min();  // match the changes in MassHunterAlgorithm.cs and in MFE (MFE agreed to do so)
                    }
                    DesiredMSStorageType storeType = DesiredMSStorageType.PeakElseProfile;
                    for (int idxPolarity = 0; idxPolarity < polarityList.Count; idxPolarity++)
                    {
                        IonPolarity ionPolarity = polarityList[idxPolarity];
                        foreach (var framVoltage in energyVoltages)  // get one valid store type.
                        {
                            try
                            {
                                storeType = GetStorageType(findParams.DataAccess, ionPolarity, framVoltage);
                                break;
                            }
                            catch  // small framVoltage different could get there, we use a valid one (TT 268031)
                            {
                            }
                        }


                        for (int segnum = 1; segnum <= m_totalTimeSegment; segnum++)
                        {
                            var sPath = FindCpdsMassHunter.GetPersistenceFilePath(
                                m_analysis.FilePath, null);
                            var sName = FindCpdsMassHunter.GetPersistenceFileName(
                                m_analysis.FilePath, ionPolarity, storeType, fragVoltage, segnum);

                            clcFileItem clcItem;
                            clcItem.rmcPath = Path.Combine(sPath, (sName + RMC_UNFILTERED_EXTENSION));
                            clcItem.clcPath = Path.Combine(sPath, (sName + CLC_EXTENSION));
                            clcItem.clcName = Regex.Replace(sName, @"^.+\.", String.Empty);
                            clcItem.scalingfactor = 1.0f;
                            clcfileList.Add(clcItem);
                        }
                    }
                }
                if (clcfileList.Count > 0)
                {
                    var itemQuery = clcfileList.GroupBy(p => p.clcName);
                    string errMsg = string.Empty;
                    List<string> processedFilesList = new List<string>();
                    foreach (IGrouping<string, clcFileItem> queryItem in itemQuery)
                    {
                        List<string> clcFilePaths = new List<string> { };
                        List<string> rmcFilePaths = new List<string> { };
                        List<float> scalfactor = new List<float> { };
                        foreach (var clcItem in queryItem)
                        {
                            if (File.Exists(clcItem.clcPath))
                            {
                                clcFilePaths.Add(clcItem.clcPath);
                                rmcFilePaths.Add(clcItem.rmcPath);
                                scalfactor.Add(clcItem.scalingfactor);
                                processedFilesList.Add(clcItem.clcPath);
                            }
                        }


                        if (clcFilePaths.Count > 0) // Do Recursive MFE only there is .clc file.
                        {
                            // Do RecursEngine  
                            RecursiveMfeEngine recursMfeEng = new RecursiveMfeEngine(clcFilePaths, rmcFilePaths, scalfactor, userParameters);
                            for (int i = 0; i < recursMfeEng.StepCount; i++)  // must start with 0.
                            {
                                //progressTracker.PercentComplete = (int)((double)(i+1) / recursMfeEng.StepCount * 100);
                                //progressTracker.ReportProgress(ProgressStage.Progressing, (int)((double)(i + 1) / recursMfeEng.StepCount * 100), "Refinding isotope cluster assignments...");
                                try
                                {
                                    recursMfeEng.StepIt(i);
                                }
                                catch (Exception ex)
                                {
                                    errMsg = ex.Message;
                                }

                                if (this.CancelCommandIndicator.Cancel == true)
                                {
                                    throw new Exception("This operation was canceled.");
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        bool bOK = false;
                        foreach (string sF in processedFilesList)
                        {
                            string rmcFile = sF.Replace(".clc", ".rmc");   // if has rmc file be generated, at lease one of the time segment has data.
                            if (File.Exists(rmcFile))
                            {
                                bOK = true;
                                break;
                            }
                        }
                        if (!bOK)
                        {
                            throw new Exception(errMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // IUserMessage msg = new UserMessage(0, string.Empty,
                // ProgramModule.CoreDbSearch,
                // ProgramModule.MfeEngine);
                // throw new MSDAApplicationException(msg);
                //throw new Exception("CmdPFRecursiveMFECpd : " + ex.Message);
                var umsg = new QualUserMessage(QualMessage.MultipleAnalysisErrors_0, ex.Message);
                throw new MSDAApplicationException(umsg);
            }
            finally
            {
                //progressTracker.ReportProgress(ProgressStage.Finished, 100, new QualUserMessage(QualMessage.OperationComplete));
                Trace.WriteLineIf(Ts.TraceInfo, Tm.Finish);
            }
        }

        /// <summary>
        /// Override to package results for DataStore (or another destination)
        /// </summary>
        //protected override void PackageResults()
        //{
        //    // compounds are not packaged at this level - they are packaged in Stage 2 of CmdPFFindByMFE()
        //}

        /// <summary>
        /// Override to get an instance of the command-specific Find Compounds algorithm
        /// </summary>
        internal IFindCompounds GetFindCompoundsAlgorithm()
        {
            return new FindCpdsMassHunter(CreatePeakFinderForMassSpectrum());
        }

        protected FindCpdsMassHunterParameters GetMFEParams()
        {
            var findParams = new FindCpdsMassHunterParameters();

            findParams.ProcessingParameters = m_AppManager[QualDAMethod.ParamKeyMFEProcessing] as PSetMassHunterProcessing;

            // Results filters are not shown in the wizard
            // however the pset contains parameters for Mass Filters hence we require it
            findParams.ResultFilters = m_AppManager[QualDAMethod.ParamKeyMFEResultFilters] as PSetMassHunterResultFilters;

            findParams.TimeRangeFilters = new PSetTimeRangeCollection();
            findParams.TimeRangeFilters.UseRanges = false;
            findParams.MassDefectFilters = m_AppManager[QualDAMethod.ParamKeyMassDefectFilter] as IPSetMassDefectFilter;
            findParams.ChargeStateAssignment = m_AppManager[QualDAMethod.GetUsageKey(QualFunctionality.ChargeStateAssignment, QualDataType.MS)] as IPSetChargeStateAssignment;

            // TT 147479:  MFE Mass Filter throws error about invalid DB path, even when filter is disabled
            // TT #147535: MFE: Invalid Mass Filters Database location when disabled should not prevent MFE from running - Added the LimitedMassRange condition for 
            // The IPSetCpdDatabase should be retireved only if both the "Filter Mass List" option is anabled and "Database" option
            // is selected in the MFE UI
            if (findParams.ResultFilters.MassSourceIsDatabase && findParams.ResultFilters.LimitedMassRange)
            {
                findParams.CpdDbAccessor =
                    CpdDatabase.GetQualifiedAccessor(m_AppManager[QualDAMethod.ParamKeyMFEDBLocation] as IPSetCpdDatabase);
                // MFE has its own instance of PSetDBLocation
            }
            else
                findParams.CpdDbAccessor = null;

            findParams.IdentityScoring = m_AppManager[QualDAMethod.ParamKeyIdScoring] as IPSetIdentityScoring;
            findParams.MSMSPeakFilter = m_AppManager[QualDAMethod.GetUsageKey(QualFunctionality.SpectrumPeakFilters, QualDataType.MSMS)]
                        as IPSetPeakFilter;//Using the one for Spectrum(MSMS), shared with MFE tab
            findParams.TofPkFinder = m_AppManager[QualDAMethod.GetUsageKey(QualFunctionality.SpectrumPeakFinder, QualDataType.MSMS)] as
                        IPSetTofPeakFinder;//Using the one for Spectrum(MSMS)

            //Abhijit:-there was a check for factory only key in Qual's MFE which I have removed.
            findParams.SaturationThresholdPct = 10.0;

            // disable extraction of all graphics/plots
            findParams.ProcessingParameters.ExrtactMSMSSpectrum = false;
            findParams.ProcessingParameters.ExtractECC = false;
            findParams.ProcessingParameters.ExtractEIC = false;
            findParams.ProcessingParameters.ExtractMFESpectrum = false;
            findParams.ProcessingParameters.ExtractRawSpectra = false;
            findParams.ProcessingParameters.PreferProfileRawSpectra = false;

            // add MFEGC filter
            if (AppFeatureConfig.Configuration.IsApplicable(AppFeatureConfig.Key_GcSep))
            {
                IPSetPeakFilter mfeGCpeakFilter = m_AppManager[QualDAMethod.ParamKeyMSPeakFilters] as IPSetPeakFilter;
                findParams.MFEGCPeakFilter = mfeGCpeakFilter;
            }
            return findParams;

        }
        /// <summary>
        /// Override to get the Find Compounds parameter set
        /// </summary>
        internal IFindCompoundsParameters GetFindCompoundsParams()
        {
            return GetMFEParams();
        }

        /// <summary>
        /// Override to make analysis-specific changes to Find Compounds parameters
        /// (i.e. set DataAccess reference)
        /// </summary>
        /// <param name="findParams">parameter set to update</param>
        /// <param name="objects">object parameters containing analysis-specific details</param>
        /// <param name="indx"></param>
        /// <returns>true if it's okay to proceed</returns>
        internal bool GetAnalysisSpecificParams(ref IFindCompoundsParameters findParams, object[] objects, int indx = 0)
        {
            var analysis = objects[0] as Analysis;
            findParams.DataAccess = analysis.DataAccess;
            return true;
        }

        private List<IonPolarity> GetScanIonPolarities(IDataAccess dataAccess)
        {
            List<IonPolarity> rv = new List<IonPolarity>();

            IonPolarity polaritiesInFile = IonPolarity.Unassigned;
            IBDAFileInformation fileInfo = dataAccess.FileInformation;
            if (fileInfo.IsMSDataPresent())
            {
                IBDAMSScanFileInformation msInfo = fileInfo.MSScanFileInformation;
                IBDAMSScanTypeInformation scanInfo = msInfo.GetMSScanTypeInformation(MSScanType.Scan);
                if (scanInfo != null)
                {
                    polaritiesInFile = scanInfo.IonPolarities;
                }
            }
            if (polaritiesInFile == IonPolarity.Unassigned)
            {
                UserMessage msg = new UserMessage(0,
                                                   string.Empty,
                                                   ProgramModule.CoreFacades,
                                                   DACoreFacadeMessages.MSG_MFE_NoMsScanData);
                throw new MSDAApplicationException(msg as IUserMessage);
            }

            switch (polaritiesInFile)
            {
                case IonPolarity.Mixed:
                    rv.Add(IonPolarity.Positive);
                    rv.Add(IonPolarity.Negative);
                    break;
                case IonPolarity.Positive:
                    rv.Add(IonPolarity.Positive);
                    break;
                case IonPolarity.Negative:
                    rv.Add(IonPolarity.Negative);
                    break;
                default:
                    break;
            }

            return rv;
        }
        private List<double> GetFragmentorVoltages(IDataAccess dataAccess)
        {
            List<double> rv = new List<double>();

            var fileInfo = dataAccess.FileInformation;
            var scanInfo = fileInfo.MSScanFileInformation;
            var fvList = scanInfo.FragmentorVoltages;

            if (fvList != null && fvList.Any())
            {
                foreach (var dv in fvList)
                    rv.Add(dv);
            }
            else
            {
                rv.Add(0.0);
            }

            return rv;
        }
        /// <summary>
        /// group files based on three items: Ion polarity, fragmentor voltage and storage type.
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="ionPolarity"></param>
        /// <param name="fragmentorVoltage"></param>
        /// <returns></returns>
        private DesiredMSStorageType GetStorageType(IDataAccess dataAccess, IonPolarity ionPolarity, double fragmentorVoltage)
        {
            DesiredMSStorageType storageModeToUse;
            IMSChromatogram chromatogram;
            bool requiresTofData = true;
            IBDAFileInformation fileInfo = dataAccess.FileInformation;
            if (fileInfo.MSScanFileInformation.DeviceType == DeviceType.Quadrupole)
                requiresTofData = false;
            storageModeToUse = FindCpdsUtilities.GetStorageTypeToUse(dataAccess, ionPolarity, requiresTofData, MSLevel.MS, fragmentorVoltage, out chromatogram);

            return storageModeToUse;
        }

        /// <summary>
        /// Override to package results for DataStore (or another destination)
        /// </summary>
        protected override void PackageResults()
        {
            // compounds are not packaged at this level - they are packaged in Stage 2 of CmdPFFindByMFE()
        }
    }
}
