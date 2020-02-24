using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataTypes
{
    [Serializable]
    [XmlRoot]
    public class TICData : ITICData
    {
        double[] m_xArray;
        float[] m_yArray;
        //[XmlArrayAttribute("XArray")]
        public double[] XArray {  get { return m_xArray; }  set { m_xArray=value; } }
        //[XmlArrayAttribute("YArray")]
        public float[] YArray { get { return m_yArray; } set { m_yArray = value; } }

        public TICData()
        {

        }    
        public void SetTICData(double[] xArray, float[] yArray)
        {
            m_xArray = xArray;
            m_yArray = yArray;
        }

        
    }
}
