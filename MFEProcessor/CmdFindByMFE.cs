using System.Collections.Generic;
using System.Linq;
using Agilent.MassSpectrometry.CommandModel;
using System.Text;
using System.IO;
using System;
using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;

namespace MFEProcessor
{
    /// <summary>
    /// Command to execute recursive Molecular Feature Extractor (rMFE)
    /// </summary>
    [CommandClass("CmdFindByMFE")]
    public class CmdFindByMFE : PFCmdCommandBase
    {
        private List<ICompoundGroup> m_cpdGroupList;
        SortedDictionary<string, IFindCpdResults> m_cpdResults = null;
        private MFEMode m_MFEMode;
        private List<Analysis> m_analyses;
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="appManager"></param>
        /// <param name="mfeMode"></param>
        /// <param name="analysis"></param>
        public CmdFindByMFE(ProfinderLogic appManager, MFEMode mfeMode, List<Analysis> analysis)
            : base(appManager)
        {
            // if the user passes a reference to a List<ICompoundGroup>
            // use that as the data store
            m_cpdGroupList = new List<ICompoundGroup>();
            m_MFEMode = mfeMode;
            m_analyses = analysis;
            m_cpdGroupList = new List<ICompoundGroup>();
            m_cpdResults = new SortedDictionary<string, IFindCpdResults>();
        }


        /// <summary>
        /// Input parameters for mfe
        /// </summary>
        public override object[] GetParameters()
        {
            return null;
        }

        /// <summary>
        /// CompoundGroup results from MFE extraction
        /// (used by Find by Ion)
        /// </summary>
        public List<ICompoundGroup> CompoundGroups
        {
            get { return m_cpdGroupList; }
        }

        /// <summary>
        /// True since DB search can be used as an ID source
        /// </summary>
        public override bool IsIdentificationCommand
        {
            get { return true; }
        }
        /// <summary>
        /// Do MFE
        /// </summary>
        protected override void DoSpecialized()
        {
            try
            {
                IFindCompoundsParameters findParams = GetFindCompoundsParams();
                IFindCompounds findCpd = GetFindCompoundsAlgorithm();
                foreach (Analysis m_analysis in m_analyses)
                {
                    findParams.DataAccess = m_analysis.DataAccess;
                    IFindCpdResults cpdResults = findCpd.FindCompounds(findParams);
                    if (cpdResults != null)
                        m_cpdResults.Add(m_analysis.FilePath, cpdResults);
                }
            }
            catch (MSDAApplicationException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Get the MFE parameter set
        /// (used universally by MFE/rMFE extraction)
        /// </summary>
        /// <returns></returns>
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
            var findParams = GetMFEParams();

            // do not do any abundance-based filtering
            // or quality score filtering in pre-alignment
            findParams.ResultFilters.MaximumCount = int.MaxValue - 1;
            findParams.ResultFilters.PeakHeightAbsThreshold = 0.0;
            findParams.ResultFilters.PeakHeightPctThreshold = 0.0;
            findParams.ResultFilters.QualityScore = 0.0;

            // in fact, the UI has been removed entirely for the RT and Z filters
            // so disable those as well
            findParams.ResultFilters.LimitedAcqTimeRange = false;
            findParams.ResultFilters.AcqTimeRanges.SetEmpty();
            findParams.ResultFilters.LimitedChargeStates = false;
            findParams.ResultFilters.ChargeStates.SetEmpty();

            // also include all single-ion features (as they may be re-assigned)
            // when running MFE in recursive stage 1
            if (m_MFEMode == MFEMode.RECURSIVE_STAGE_1)
            {
                findParams.ResultFilters.NoMassCpds = InclusionType.Include;
                findParams.ResultFilters.NoIsotopeCpds = InclusionType.Include;
            }

            findParams.MFEMODE = m_MFEMode;

            return findParams;
        }
        /// <summary>
        /// Prepare an InterruptableAlgorithm by clearing cancel flags and
        /// binding to the AppManager's cancel indicator
        /// </summary>
        /// <param name="analysis"></param>
        /// <param name="findCpd"></param>
        /// <returns></returns>
        protected IInterruptAlgorithm GetInterruptableAlgorithm(Analysis analysis, IFindCompounds findCpd)
        {
            #region PVCS 11925 - always clear Cancel flag on DataAccess or the algorithm will terminate

            // PVCS 11925 - If a DataAccess command (e.g., extract chromatograms)
            // has been run, analysis.DataAccess gets a reference to CancelCommandIndicator.
            // If a FindCpds algorithm then run and interrupted, the DataAccess keeps that 
            // reference, which is still set to Cancel == true.  The next time a FindCpds
            // command (or any command that internally uses the dataaccess) is run, it's
            // calls to DataAccess will immediately terminate when that DataAccess checks
            // for Cancel.
            // To prevent that happening here, set the DataAccess cancel indicator to
            // one that (for sure) does not indicate Cancel.   If the find compounds 
            // algorithm wishes to reset it to something else before calling DataAccess,
            // that should still work.
            var interruptableAlgorithm = analysis.DataAccess as IInterruptAlgorithm;
            if (interruptableAlgorithm != null)
            {
                interruptableAlgorithm.CancelIndicator = new CancelIndicator();
                interruptableAlgorithm.CancelIndicator.Cancel = false;
            }

            #endregion

            // Set the cancel indicator for the FindCpds algorithm
            interruptableAlgorithm = findCpd as IInterruptAlgorithm;
            interruptableAlgorithm.CancelIndicator = CancelCommandIndicator;
            return interruptableAlgorithm;
        }
        /// <summary>
        /// Override to get an instance of the command-specific Find Compounds algorithm
        /// </summary>
        internal IFindCompounds GetFindCompoundsAlgorithm()
        {
            return new FindCpdsMassHunter(CreatePeakFinderForMassSpectrum());
        }

        /// <summary>
        /// Package results
        /// </summary>
        protected override void PackageResults()
        {
            var alignmentInfo = m_AppManager[QualDAMethod.ParamKeyAlignmentInformation] as PSetAlignmentInfo;

            if (m_analyses.Any() && m_analyses.First().DataAccess.FileInformation.MSScanFileInformation.DeviceType == DeviceType.Quadrupole)
                alignmentInfo.MsPeakAlignmentToleranceDa = 0.2; // 20180323NK - default per Don in MFE in Profinder B.08.00 SP3

            var cpdCorrEngine = new CompoundCorrelationEngine();
            var analyses = m_cpdResults.Keys.ToList();
            var cpdGroup2D = m_cpdResults.Select(cr => cr.Value.CpdResultList.ToList() as IList<ICpdResult>).ToList();
            m_cpdGroupList.AddRange(cpdCorrEngine.Run(analyses, cpdGroup2D, alignmentInfo));

            m_PFLogic.DataStore.AssignCompoundGroupList(m_cpdGroupList);
        }
    }
}
