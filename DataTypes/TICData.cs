using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class TICData : ITICData
    {
        double[] m_xArray;
        float[] m_yArray;
        public double[] XArray =>  m_xArray;

        public float[] YArray => m_yArray;

        public TICData(double[] xArray, float[] yArray)
        {
            m_xArray = xArray;
            m_yArray = yArray;
        }

        
    }
}
