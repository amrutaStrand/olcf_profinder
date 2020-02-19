using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.CompoundGroupsTable.ViewModels
{
    /// <summary>
    /// The compound group item interface
    /// </summary>
    public interface ICompoundGroupItem
    {
        #region Properties

        /// <summary>
        /// The compound group info
        /// </summary>
        ICompoundGroup CompoundGroupInfo { get; }

        /// <summary>
        /// The group
        /// </summary>
        string Group { get; }
        /// <summary>
        /// The Target RT
        /// </summary>
        double RTTgt { get; }
        /// <summary>
        /// The median RT
        /// </summary>
        double RTMed { get; }
        /// <summary>
        /// Found
        /// </summary>
        int Found { get; }
        /// <summary>
        /// Missed
        /// </summary>
        int Missed { get; }
        /// <summary>
        /// Max MFE score
        /// </summary>
        double ScoreMFEMax { get; }
        /// <summary>
        /// The median height
        /// </summary>
        double HeightMed { get; }
        /// <summary>
        /// The average mass
        /// </summary>
        double MassAvg { get; }
        /// <summary>
        /// The average RT
        /// </summary>
        double RTAvg { get; }
        /// <summary>
        /// The median mass
        /// </summary>
        double MassMedian { get; }
        /// <summary>
        /// The target mass
        /// </summary>
        double TargetMass { get; }

        #endregion

    }
}
