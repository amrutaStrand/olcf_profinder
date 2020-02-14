using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface ICompoundGroup
    {

        string Group { get; set; }
        double RTTgt { get; set; }
        double RTMed { get; set; }

        int Found { get; set; }

        int Missed { get; set; }

        //double ScoreTgtMax { get; set; }

        double ScoreMFEMax { get; set; }

        double HeightMed { get; set; }

        double MassAvg { get; set; }

        double RTAvg { get; set; }

        int Saturated { get; set; }

        double AreaAvg { get; set; }

        double HeightAvg { get; set; }

        double VolumeAvg { get; set; }

        IDictionary<string, ICompound> SampleWiseDataDictionary { get; set; }
        
        //double TgtScorePctRSD { get; set; }
        double TargetMass { get; set; }
        double MassMedian { get; set; }
        double MassPpmRSD { get; set; }
        double RetentionTimeSpan { get; set; }
        double RetentionTimeWidthAtBase { get; set; }
        double RetentionTimeDifference { get; set; }
        string TimeSegment { get; set; }
        int SingleIonFeatures { get; set; }
    }
}
