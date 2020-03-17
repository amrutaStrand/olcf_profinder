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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetGrouping()
        {
            return null;
        }

    }
}
