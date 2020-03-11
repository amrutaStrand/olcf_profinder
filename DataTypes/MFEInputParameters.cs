using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agilent.MassSpectrometry.DataAnalysis;

namespace DataTypes
{
    public class MFEInputParameters
    {

        public Dictionary<String, object> AllParameters {
            get;
            private set;
        }

        public MFEInputParameters()
        {
            AllParameters = new Dictionary<string, object>();
        }

    }
}
