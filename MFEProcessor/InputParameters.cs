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

        public IPSetAlignmentInfo GetAlignmentParameters(ProfinderLogic AppLogic)
        {
            IPSetAlignmentInfo alignmentInfo = AppLogic[QualDAMethod.ParamKeyAlignmentInformation] as IPSetAlignmentInfo;
            return alignmentInfo;
        }

        public IPSetAlignmentInfo GetMFEProcessingParameters(ProfinderLogic AppLogic)
        {
            IPSetAlignmentInfo extractionInfo = AppLogic[QualDAMethod.ParamKeyMFEProcessing] as IPSetAlignmentInfo;
            return extractionInfo;
        }
        
        public IPSetAlignmentInfo GetChargeStateParameters(ProfinderLogic AppLogic)
        {
            IPSetAlignmentInfo chargeStateInfo = AppLogic[QualDAMethod.ParamKeyMSChargeStateAssignment] as IPSetAlignmentInfo;
            return chargeStateInfo;
        }

    }

}
