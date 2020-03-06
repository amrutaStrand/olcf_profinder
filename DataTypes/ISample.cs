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


        #endregion
    }
}
