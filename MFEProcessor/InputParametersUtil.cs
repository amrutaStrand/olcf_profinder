using Agilent.MassSpectrometry.DataAnalysis;
using Agilent.MassSpectrometry.DataAnalysis.Qualitative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFEProcessor
{
    public class InputParametersUtil
    {
        public static void SavePSet(ProfinderLogic AppLogic, IParameterSet pset, string usageKey)
        {
            pset.UsageKey = usageKey;
            IParameterSet[] Pset = { pset as ParameterSet };
            var cmdSetParam = new CmdSetParameters(AppLogic, Pset);
            cmdSetParam.Execute();
        }

        public static IPSetAlignmentInfo GetAlignmentParameters(ProfinderLogic AppLogic)
        {
            IPSetAlignmentInfo alignmentInfo = AppLogic[QualDAMethod.ParamKeyAlignmentInformation] as IPSetAlignmentInfo;
            return alignmentInfo;
        }
        
        public static IPSetMassHunterProcessing GetMFEProcessingParameters(ProfinderLogic AppLogic)
        {
            IPSetMassHunterProcessing extractionInfo = AppLogic[QualDAMethod.ParamKeyMFEProcessing] as IPSetMassHunterProcessing;
            return extractionInfo;
        }

        public static IPSetChargeStateAssignment GetChargeStateParameters(ProfinderLogic AppLogic)
        {
            IPSetChargeStateAssignment chargeStateInfo = AppLogic[QualDAMethod.GetUsageKey(QualFunctionality.ChargeStateAssignment, QualDataType.MS)] as IPSetChargeStateAssignment;

            return chargeStateInfo;
        }

    }

}
