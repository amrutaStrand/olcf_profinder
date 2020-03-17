using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Sample:ISample
    {
        #region Properties
        /// <summary>
        /// Experiment oerder property
        /// </summary>
        public int ExpOrder { get; set; }
        /// <summary>
        /// File name property
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Source of the ions in sample
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// sample type property
        /// </summary>
        public string SampleType { get; set; }
        /// <summary>
        /// group property
        /// </summary>
        public string Group { get; set; }
        public bool HideOrShow { get; set; }

        public List<string> GetDefaultGroupingTitles()
        {
            List<string> defaultList = new List<string>();
            defaultList.Add("Sample Type");
            defaultList.Add("Group");

            return defaultList;
        }

        public List<string> GetDefaultSampleGroups()
        {
            List<string> defaultGroups = new List<string>();
            defaultGroups.Add(this.SampleType);
            defaultGroups.Add(this.Group);
            return defaultGroups;
        }

        #endregion
    }
}
