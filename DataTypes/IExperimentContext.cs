using System.Collections.Generic;

namespace DataTypes
{
    public interface IExperimentContext
    {
        List<ICompoundGroup> CompoundGroups { get; set; }
        List<ISample> Samples { get; set; }

        MFEInputParameters MFEInputParameters { get; set; }

        Dictionary<string, string> GetGrouping();
    }
}