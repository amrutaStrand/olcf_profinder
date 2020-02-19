using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Agilent.MassSpectrometry.CommandModel;
using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;
using MassProfiler;

namespace MFEProcessor
{
    /// <summary>
    /// Applies compound group level abundance and quality filters after molecular feature extraction
    /// </summary>
    [CommandClass("CmdFilterCompoundGroupsMFE")]
    public class CmdFilterCompoundGroupsMFE : PFCmdCommandBase
    {
        private Dictionary<string, string> m_sampleGroupDict;
        private IPSetCpdGroupFilters m_psetFilters;

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="appManager"></param>
        /// <param name="filePaths"></param>
        public CmdFilterCompoundGroupsMFE(ProfinderLogic appManager, List<string> filePaths) : base(appManager)
        {
            m_psetFilters = m_AppManager[QualDAMethod.ParamKeyCpdGroupFilters] as IPSetCpdGroupFilters;
            var psetFileList = m_PFLogic[QualInMemoryMethod.ParamDataFileList] as PSetDataFileList;
            foreach (string path in filePaths)
            {
                psetFileList.SelectedFileName.Add(new BatchExtractorFileSelect { FileName = path });
            }

            m_sampleGroupDict = psetFileList.SelectedFileName.ToDictionary(
                fs => fs.FileName,
                fs => string.Join(":", fs.SampleGroups));
        }

        /// <summary>
        /// Override to do the actual work of the command
        /// </summary>
        protected override void DoSpecialized()
        {
            var cp = GetAlignmentParameters();
            var fp = new CompoundFilterByBucket.Parameters();

            // filter by height
            fp.UseVolumeAsIntensity = false;

            // dynamic height threshold (PSet says Volume but is really a height value)
            if (m_psetFilters.VolumeRelativeEnabled)
                fp.MinRelativeIntensity = (float)m_psetFilters.VolumeRelative / 100.0f;

            // absolute volume threshold (PSet says Volume but is really a height value)
            if (m_psetFilters.VolumeAbsoluteEnabled)
                fp.MinAbsoluteIntensity = m_psetFilters.VolumeAbsolute; // should be long

            // MFE Q-Score
            if (m_psetFilters.MFEScoreMinEnabled)
                fp.MinQScore = (float)m_psetFilters.MFEScoreMin;

            // Frequency:  Number of file(s)
            if (m_psetFilters.VolumeRelativeEnabled || m_psetFilters.VolumeAbsoluteEnabled || m_psetFilters.MFEScoreMinEnabled)
            {
                switch (m_psetFilters.FrequencyFilterModeMFE)
                {
                    case CpdGroupFrequencyFilterMode.NumberOfFiles:
                        fp.MinAbsoluteFrequency = m_psetFilters.FrequencyMinMFE;
                        break;
                    case CpdGroupFrequencyFilterMode.PctOfFiles:
                        fp.MinRelativeFrequency = m_psetFilters.FrequencyMinPctMFE / 100.0f;
                        break;
                }
            }
            else
            {
                // if no filters are enabled, set the min frequency to 0
                fp.MinAbsoluteFrequency = 0;
            }

            switch (m_psetFilters.FrequencyGroupMFE)
            {
                case CpdGroupFilterMode.AllSamples:
                    // the frequency pass cutoff must be met across all samples
                    fp.RequirementScope = CompoundFilterByBucket.Parameters.FrequencyRequirementScope.AllSames;
                    break;
                case CpdGroupFilterMode.SamplesInEachGroup:
                    // the frequency pass cutoff must be met within each sample group
                    fp.RequirementScope = CompoundFilterByBucket.Parameters.FrequencyRequirementScope.EveryGroup;
                    break;
                case CpdGroupFilterMode.SamplesInOneGroup:
                    // the frequency pass cutoff must be met within at least one sample group
                    fp.RequirementScope = CompoundFilterByBucket.Parameters.FrequencyRequirementScope.AtLeastOneGroup;
                    break;
            }

            // finally, apply the limit to largest N filter
            if (m_psetFilters.LimitToLargestNEnabledMFE)
                fp.NumberOfMostAbundantFeatureToKeep = (int)m_psetFilters.LimitToLargestNMFE;

            // execute the filtering step
            var dataFiles = GetFilePaths();
            CompoundFilterByBucket.Filter(dataFiles, cp, fp);

            // delete the unfiltered .rmc files
            foreach (var rmc in dataFiles.SelectMany(dpi => dpi))
            {
                if (File.Exists(rmc.InputCompoundFilePath))
                    File.Delete(rmc.InputCompoundFilePath);
            }
        }

        /// <summary>
        /// Override to package results for DataStore (or another destination)
        /// </summary>
        protected override void PackageResults()
        {

        }

        /// <summary>
        /// Compile a 2D list of files organized by sample group for rMFE filtering
        /// This list has input and output file paths (.rmc)
        /// </summary>
        /// <returns></returns>
        private ICollection<ICollection<CompoundFilterByBucket.DataPathInfo>> GetFilePaths()
        {
            // outer list is by sample group
            var rv = new List<ICollection<CompoundFilterByBucket.DataPathInfo>>();
            foreach (var group in m_sampleGroupDict.Values.Distinct())
            {
                // inner list is by .rmc file
                var grpList = new List<CompoundFilterByBucket.DataPathInfo>();
                foreach (var file in m_sampleGroupDict.Where(sg => sg.Value == group).Select(sg => sg.Key))
                {
                    // there can be multiple .unfiltered.rmc files depending on frag voltage, polarity, etc.
                    var rmcPath = FindCpdsMassHunter.GetPersistenceFilePath(file, null);
                    foreach (var rmcFile in Directory.GetFiles(rmcPath, "*" + RMC_UNFILTERED_EXTENSION))
                    {
                        var dpi = new CompoundFilterByBucket.DataPathInfo();
                        dpi.CustomerScalingFactor = 1.0f;
                        dpi.InputCompoundFilePath = rmcFile;
                        dpi.OutputCompoundFilePah = Regex.Replace(rmcFile, RMC_UNFILTERED_EXTENSION, RMC_EXTENSION);
                        grpList.Add(dpi);
                    }
                }

                rv.Add(grpList);
            }

            return rv;
        }
    }
}