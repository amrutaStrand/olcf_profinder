using System.Collections.Generic;
using System.Linq;
using Agilent.MassSpectrometry.CommandModel;
using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;

namespace MFEProcessor
{
    /// <summary>
    /// Applies compound group level abundance and quality filters after feature extraction
    /// </summary>
    [CommandClass("CmdFilterCompoundGroupsMFEPost")]
    public class CmdFilterCompoundGroupsMFEPost : CmdCommandBase
    {
        private readonly Dictionary<string, string> m_sampleGroupDict;
        private readonly IPSetCpdGroupFilters m_psetFilters;

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="appManager"></param>
        /// <param name="filePaths"></param>
        public CmdFilterCompoundGroupsMFEPost(ProfinderLogic appManager, List<string> filePaths)
            : base(appManager)
        {
            m_psetFilters = m_AppManager[QualDAMethod.ParamKeyCpdGroupFilters] as IPSetCpdGroupFilters;
            var psetFileList = m_AppManager[QualInMemoryMethod.ParamDataFileList] as PSetDataFileList;
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

            var ds = m_PFLogic.DataStore;
            ds.DataStoreDeletedItem += Ds_DataStoreDeletedItem;
            ds.DataStoreCreated += Ds_DataStoreCreated;

            var relHeightThreshold = 0.0;

            // calculate the dynamic volume threshold before filtering
            var abundList = ds.CompoundGroups
                              .SelectMany(cg => cg.Values)
                              .Where(c => c.HasValue(ResultAttribute.Height))
                              .Select(c => c.Height).ToList();
            if (abundList.Any())
                relHeightThreshold = abundList.Max() * m_psetFilters.VolumeRelative / 100.0;


            // build a continually growing list of rejected compound groups for one-shot deletion
            var cgRejectList = new List<ICompoundGroup>();

            foreach (var cg in ds.CompoundGroups)
            {
                // apply height level filters if they apply
                if (m_psetFilters.VolumeAbsoluteEnabled)
                {
                    if (!TestFilter(cg, ResultAttribute.Height, m_psetFilters.VolumeAbsolute))
                    {
                        cgRejectList.Add(cg);
                        continue;
                    }
                }

                if (m_psetFilters.VolumeRelativeEnabled)
                {
                    if (!TestFilter(cg, ResultAttribute.Height, relHeightThreshold))
                    {
                        cgRejectList.Add(cg);
                        continue;
                    }
                }


                // apply target score filters if they apply
                if (m_psetFilters.MFEScoreMinEnabled)
                {
                    if (!TestFilter(cg, ResultAttribute.QualityScore, m_psetFilters.MFEScoreMin))
                    {
                        cgRejectList.Add(cg);
                        continue;
                    }
                }
            }

            // remove all rejected compound groups from the DataStore
            ds.DeleteCompoundGroups(cgRejectList);
            ds.RenumberCompounds();

            // finally, apply the limit to largest N filter,
            // as it is the easiest and reduces the load on downstream filters
            //if (m_psetFilters.LimitToLargestNEnabledMFE && m_psetFilters.LimitToLargestNMFE < ds.Count)
            //{
            //    // all algorithms report height, so sort on it
            //    var cgFiltered = ds.CompoundGroups
            //        .OrderByDescending(cg => cg.HeightMedian)
            //        .Take((int)m_psetFilters.LimitToLargestNMFE);
            //    ds.AssignCompoundGroupList(cgFiltered.ToList());
            //}

            //// cache all the rMFE RT ranges for use in Find by Ion
            //m_PFLogic.CpdDimensionStore = new CpdDimensionStore(ds);

            //Update UI
            m_PFLogic.DataStore.UpdateUI();
            //m_PFLogic.DoFireEnableDisableUIEvent();
        }

        private void Ds_DataStoreCreated(object sender, System.EventArgs e)
        {
        }

        private void Ds_DataStoreDeletedItem(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Package results
        /// </summary>
        protected override void PackageResults()
        {
        }

        /// <summary>
        /// Test whether the compound group passes the threshold cutoff for a given compound ResultAttribute
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="valueAttr"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private bool TestFilter(ICompoundGroup cg, ResultAttribute valueAttr, double threshold)
        {
            // set up algorithm specific filter parameters
            var filterMode = m_psetFilters.FrequencyGroupMFE;
            var freqMode = m_psetFilters.FrequencyFilterModeMFE;
            var freqMin = m_psetFilters.FrequencyMinMFE;
            var pctMin = m_psetFilters.FrequencyMinPctMFE;

            // get a structure of compound groups grouped by sample group
            var sgGroups = cg.GroupBy(kvp => m_sampleGroupDict[kvp.Key], kvp => kvp.Value);
            switch (filterMode)
            {
                case CpdGroupFilterMode.AllSamples:
                    // the frequency pass cutoff must be met across all samples
                    if (freqMode == CpdGroupFrequencyFilterMode.PctOfFiles)
                        freqMin = m_sampleGroupDict.Count * pctMin / 100;
                    return cg.Values
                               .Where(c => c.HasValue(valueAttr))
                               .Count(c => (double)c.Value(valueAttr) >= threshold) >= freqMin;

                case CpdGroupFilterMode.SamplesInEachGroup:
                    // the frequency pass cutoff must be met within every sample group
                    foreach (var sg in sgGroups)
                    {
                        var sgFreq = sg.Where(c => c.HasValue(valueAttr))
                            .Count(c => (double)c.Value(valueAttr) >= threshold);
                        if (freqMode == CpdGroupFrequencyFilterMode.PctOfFiles)
                            freqMin = m_sampleGroupDict.Count(g => g.Value == sg.Key) * pctMin / 100;
                        if (sgFreq < freqMin)
                            return false;
                    }
                    return true;

                case CpdGroupFilterMode.SamplesInOneGroup:
                    // the frequence pass cutoff must be met within at least one sample group
                    foreach (var sg in sgGroups)
                    {
                        var sgFreq = sg.Where(c => c.HasValue(valueAttr))
                            .Count(c => (double)c.Value(valueAttr) >= threshold);
                        if (freqMode == CpdGroupFrequencyFilterMode.PctOfFiles)
                            freqMin = m_sampleGroupDict.Count(g => g.Value == sg.Key) * pctMin / 100;
                        if (sgFreq >= freqMin)
                            return true;
                    }
                    return false;

                default:
                    return true;
            }
        }
    }
}
