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
    public class ExperimentContext
    {
        /// <summary>
        /// 
        /// </summary>
        List<ISample> Samples { set; get; }


        /// <summary>
        /// 
        /// </summary>
        List<ICompoundGroup> CompoundGroups { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetGrouping()
        {
            return null;
        }

    }
}
