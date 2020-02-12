using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class Spectrum : ISpectrum
    {

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private IList<IPeak> peaks;
        public IList<IPeak> Peaks {
            get { return peaks; }
            set { peaks = value; }
        }

    }
}
