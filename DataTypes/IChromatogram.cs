using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface IChromatogram
    {
        List<DataPoint> Data
        {
            get; set;
        }

        string Name
        {
            get; set;
        }

        string Title
        { 
            get; set;
        }
    }
}
