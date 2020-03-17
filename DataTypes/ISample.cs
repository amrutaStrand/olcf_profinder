using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface ISample
    {
        #region Properties

        /// <summary>
        /// Check box to show or hide the sample
        /// </summary>
        bool HideOrShow { get; set; }

        /// <summary>
        /// Experiment order property
        /// </summary>
        int ExpOrder { get; set; }

        /// <summary>
        /// File name property
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Sample type property
        /// </summary>
        string SampleType { get; set; }

        /// <summary>
        /// Group property
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// Source of the ion in sample
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// returns list of grouping titles 
        /// </summary>
        /// <returns></returns>
        List<string> GetDefaultGroupingTitles();


        /// <summary>
        /// returns list of values for respective group titles.
        /// </summary>
        /// <returns></returns>
        List<string> GetDefaultSampleGroups();

        #endregion
    }
}
