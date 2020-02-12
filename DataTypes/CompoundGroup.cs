using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class CompoundGroup : ICompoundGroup
    {
        public string Group { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float RTTgt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float RTMed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Found { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Missed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float ScoreTgtMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float ScoreMFEMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float HeightMed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float MassAvg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float RTAvg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Saturated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float AreaAvg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float HeightAvg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float VolumeAvg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, ICompound> SampleWiseDataDictionary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
