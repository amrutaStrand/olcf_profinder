using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface ICompound
    {

        string FileName
        {
            get; set;
        }

        double Mass
        {
            get; set;
        }

        double RT
        {
            get; set;
        }

        double Area
        {
            get; set;
        }
        
        double Volume
        {
            get; set;
        }
        
        Boolean Saturated
        {
            get; set;
        }

        double Width
        {
            get; set;
        }
        
        int Ions
        {
            get; set;
        }
        int ZCount
        {
            get; set;
        }


        ISpectrum Spectrum
        {
            get; set;
        }

        IChromatogram Chromatogram
        {
            get; set;
        }


    }
}
