using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Compound : ICompound
    {
        public string FileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Mass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double RT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Area { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Saturated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Ions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ZCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISpectrum Spectrum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IChromatogram Chromatogram { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
