using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface ISpectrum
    {
        string Name
        {
            get; set;
        }

        IList<IPeak> Peaks
        {
            get; set;
        }
    }
}
