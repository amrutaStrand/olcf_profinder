﻿using System;
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
        /// sample type property
        /// </summary>
        public string SampleType { get; set; }
        /// <summary>
        /// group property
        /// </summary>
        public string Group { get; set; }
        #endregion
    }
}
