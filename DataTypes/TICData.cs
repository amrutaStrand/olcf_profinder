using Agilent.MassSpectrometry.DataAnalysis;
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

        public TICData(string FileName)
        {
            try
            {
                if (FileName != null)
                {
                    DOReadTICTest1(FileName);
                }
                else
                {
                    throw new Exception("File name is null. So cant produce TIC data.");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DOReadTICTest1(string FileName)  // data file name, such as c:\temp\mydata.d

        {

            IDataAccess dataAccessor = new DataAccess() as IDataAccess;

            string sFileName = string.Empty;

            try

            {

                double[] m_allRtArray = null;       // RT  x-value

                float[] m_allTicYArray = null;      // TIC y-values

                sFileName = FileName;

                dataAccessor.OpenDataFile(sFileName);

                var m_BDADataAccess = dataAccessor.BaseDataAccess;

                IBDAChromFilter psetfilter = new BDAChromFilter();

                psetfilter.ChromatogramType = ChromType.TotalIon;

                IBDAChromData[] chromarray = m_BDADataAccess.GetChromatogram(psetfilter);

                m_allRtArray = chromarray[0].XArray;

                m_allTicYArray = chromarray[0].YArray;

                //setting values in data members
                m_xArray = m_allRtArray;
                m_yArray = m_allTicYArray;

            }

            catch (Exception ex)

            {

                // MessageBox.Show(ex.Message);
                throw;

            }

            finally

            {

                if (!string.IsNullOrEmpty(sFileName))

                    dataAccessor.CloseDataFile();  // make sure the data file will be closed

            }

        }
    }
}
