using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Chromatogram : IChromatogram
    {
        public List<DataPoint> Data { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class DataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
