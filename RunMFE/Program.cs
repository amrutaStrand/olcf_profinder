using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace RunMFE
{
    class Program
    {
        static void Main(string[] args)
        {
            MFEProcessor.MFE mFEProcessor = new MFEProcessor.MFE();
            List<string> analysisFiles = new List<string>();
            analysisFiles.Add("C:\\Users\\harika\\Desktop\\D01B.d");
            analysisFiles.Add("C:\\Users\\harika\\Desktop\\D02B.d");
            var compoundGroups = mFEProcessor.GetCompoundGroups(analysisFiles);
            //Console.WriteLine(compoundGroups);
        }
    }
}
