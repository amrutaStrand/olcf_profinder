using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    interface ICompoundGroup
    {

        string Group { get; set; }
        float RTTgt { get; set; }
        float RTMed { get; set; }

        Boolean Found { get; set; }

        Boolean Missed { get; set; }

        float ScoreTgtMax { get; set; }

        float ScoreMFEMax { get; set; }

        float HeightMed { get; set; }

        float MassAvg { get; set; }

        float RTAvg { get; set; }

        Boolean Saturated { get; set; }

        float AreaAvg { get; set; }

        float HeightAvg { get; set; }

        float VolumeAvg { get; set; }

        IDictionary<string, ICompound> SampleWiseDataDictionary { get; set; }
    }
}
