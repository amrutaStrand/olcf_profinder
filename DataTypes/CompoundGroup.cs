using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class CompoundGroup : ICompoundGroup
    {
        public string Group { get; set; }
        public double RTTgt { get; set; }
        public double RTMed { get; set; }
        public int Found { get; set; }
        public int Missed { get; set; }
        //public double ScoreTgtMax { get; set; }
        public double ScoreMFEMax { get; set; }
        public double HeightMed { get; set; }
        public double MassAvg { get; set; }
        public double RTAvg { get; set; }
        public int Saturated { get; set; }
        public double AreaAvg { get; set; }
        public double HeightAvg { get; set; }
        public double VolumeAvg { get; set; }
        public IDictionary<string, ICompound> SampleWiseDataDictionary { get; set; }
        //public double TgtScorePctRSD { get; set; }
        public double TargetMass { get; set; }
        public double MassMedian { get; set; }
        public double MassPpmRSD { get; set; }
        public double RetentionTimeSpan { get; set; }
        public double RetentionTimeWidthAtBase { get; set; }
        public double RetentionTimeDifference { get; set; }
        public string TimeSegment { get; set; }
        public int SingleIonFeatures { get; set; }
        int ICompoundGroup.Saturated { get; set; }
    }
}
