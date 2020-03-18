using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class ExperimentContext : IExperimentContext
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ISample> Samples { set; get; }


        /// <summary>
        /// 
        /// </summary>
        public List<ICompoundGroup> CompoundGroups { get; set; }
        public MFEInputParameters MFEInputParameters { get; set; }
        public Dictionary<string, TICData> SamplewiseTICData { get; set; }


        /// <summary>
        /// Returns map of samplename to sample group
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetGrouping()
        {
            if (Samples == null)
                return null;
            Dictionary<string, string> grouping = new Dictionary<string, string>();
            
            foreach(Sample sample in Samples)
            {
                grouping.Add(sample.FileName, sample.Group);
            }

            return grouping;
        }

    }
}
