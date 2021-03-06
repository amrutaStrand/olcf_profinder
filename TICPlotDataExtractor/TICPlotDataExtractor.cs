using Agilent.MassSpectrometry.DataAnalysis;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TICPlotDataExtractor
{
    public static class TICPlotDataExtractor
    {
        public static Dictionary<string, TICData> Extract(List<string> FileList)

        {
            Dictionary<string, TICData> result = new Dictionary<string, TICData>();
            //QualFeatureConfig.InitRegistryFromAppConfig();
            AppFeatureConfig.Configuration.SetKeyState(AppFeatureConfig.Key_ProfinderApp, true);
            //IMsStreamsProvider msStreamsProvider = new MsStreamsLocalFsProvider(new RawDataReader(new System.IO.FileStream(args[0], System.IO.FileMode.Open)), args[0]);
            foreach (string filename in FileList)
            {
                TICData tICData = DOReadTICTest1(filename) as TICData;

                //Console.WriteLine("File Name: " + filename + "\nNo. of Data Points in TIC " + tICData.XArray.Length);
                result.Add(filename, tICData);
            }

            return result;
            
            //Console.ReadKey();
            
        }
        private static ITICData DOReadTICTest1(string FileName)  // data file name, such as c:\temp\mydata.d

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
               
                    TICData tICData = new TICData();
                    tICData.SetTICData(m_allRtArray, m_allTicYArray);
                return tICData;
                  

                
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
