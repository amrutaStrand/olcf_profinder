using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public interface IPeak
    {
        double X
        {
            get; set;
        }

        double Y
        {
            get; set;
        }

        string Name
        {
            get; set;
        }
    }
}
