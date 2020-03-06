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
        private static void SavePSet(ProfinderLogic AppLogic, IParameterSet pset, string usageKey)
        {
            pset.UsageKey = usageKey;
            IParameterSet[] Pset = { pset as ParameterSet };
            var cmdSetParam = new CmdSetParameters(AppLogic, Pset);
            cmdSetParam.Execute();
        }

        public static void SetAlignmentParameters(ProfinderLogic AppLogic)
        {
            IPSetAlignmentInfo alignmentInfo = AppLogic[QualDAMethod.ParamKeyAlignmentInformation] as IPSetAlignmentInfo;
            alignmentInfo.RTMinutes = 0.8;
            SavePSet(AppLogic, alignmentInfo, QualDAMethod.ParamKeyAlignmentInformation);
        }
    }
}
