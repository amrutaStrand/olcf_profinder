using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agilent.MassSpectrometry.DataAnalysis;

namespace DataTypes
{
    public class MFEInputParameters
    {
        public IPSetAlignmentInfo pSetAlignmentInfo;

        public MFEInputParameters(IPSetAlignmentInfo pSetAlignmentInfo)
        {
            this.pSetAlignmentInfo = pSetAlignmentInfo;
        }

    }
}
