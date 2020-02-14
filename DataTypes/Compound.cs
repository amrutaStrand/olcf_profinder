using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Compound : ICompound
    {
        public string FileName { get; set; }
        public double Mass { get; set; }
        public double RT { get; set; }
        public double Area { get; set; }
        public double Volume { get; set; }
        public bool Saturated { get; set; }
        public double Width { get; set; }
        public int Ions { get; set; }
        public int ZCount { get; set; }
        public ISpectrum Spectrum { get; set; }
        public IChromatogram Chromatogram { get; set; }
    }
}
