using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFEProcessor
{
    public class InputParameters
    {
        private static void SavePSet(ProfinderLogic AppLogic, ParameterSet pset, string usageKey)
        {
            pset.UsageKey = usageKey;
            IParameterSet[] Pset = { pset as ParameterSet };
            var cmdSetParam = new CmdSetParameters(AppLogic, Pset);
            cmdSetParam.Execute();
        }
        public static void SetAlignmentParameters(ProfinderLogic AppLogic)
        {
            PSetAlignmentInfo alignmentInfo = new PSetAlignmentInfo();
            alignmentInfo.RTMinutes = 0.8;
            SavePSet(AppLogic, alignmentInfo, QualDAMethod.ParamKeyAlignmentInformation);
            //alignmentInfo.UsageKey = QualDAMethod.ParamKeyAlignmentInformation;
            //IParameterSet[] Pset = { alignmentInfo as ParameterSet };
            //var cmdSetParam = new CmdSetParameters(AppLogic, Pset);
            //cmdSetParam.Execute();
        }
    }
}
