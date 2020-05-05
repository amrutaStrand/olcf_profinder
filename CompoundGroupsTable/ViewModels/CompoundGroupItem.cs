using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agilent.OpenLab.UI.Controls.WinFormsControls;
using DataTypes;

namespace Agilent.OpenLab.CompoundGroupsTable.ViewModels
{
    /// <summary>
    ///     The compound group item.
    /// </summary>

    public class CompoundGroupItem : BasicGridItemViewModelWithValidationManager, ICompoundGroupItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundGroupItem"/> class.
        /// </summary>
        /// <param name="compoundGroup"></param>
        public CompoundGroupItem(ICompoundGroup compoundGroup)
        {
            this.CompoundGroupInfo = compoundGroup;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the compound group info
        /// </summary>
        public ICompoundGroup CompoundGroupInfo { get; private set; }

        /// <summary>
        /// The group
        /// </summary>
        public string Group { get { return CompoundGroupInfo.Group; } }

        /// <summary>
        /// The Target RT
        /// </summary>
        public double RTTgt { get { return CompoundGroupInfo.RTTgt; } }

        /// <summary>
        /// The median RT
        /// </summary>
        public double RTMed { get { return CompoundGroupInfo.RTMed; } }

        /// <summary>
        /// Found
        /// </summary>
        public int Found { get { return CompoundGroupInfo.Found; } }

        /// <summary>
        /// Missed
        /// </summary>
        public int Missed { get { return CompoundGroupInfo.Missed; } }

        /// <summary>
        /// Maximum MFE score
        /// </summary>
        public double ScoreMFEMax { get { return CompoundGroupInfo.ScoreMFEMax; } }

        /// <summary>
        /// The Median height
        /// </summary>
        public double HeightMed { get { return CompoundGroupInfo.HeightMed; } }

        /// <summary>
        /// The average mass
        /// </summary>
        public double MassAvg { get { return CompoundGroupInfo.MassAvg; } }

        /// <summary>
        /// The average RT
        /// </summary>
        public double RTAvg { get { return CompoundGroupInfo.RTAvg; } }

        /// <summary>
        /// The median mass
        /// </summary>
        public double MassMedian { get { return CompoundGroupInfo.MassMedian; } }

        /// <summary>
        /// The target mass
        /// </summary>
        public double TargetMass { get { return CompoundGroupInfo.TargetMass; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString(string delimeter)
        {
            string[] values = new string[]
            {
                Group,
                RTTgt.ToString(),
                RTMed.ToString(),
                Found.ToString(),
                Missed.ToString(),
                ScoreMFEMax.ToString(),
                HeightMed.ToString(),
                MassAvg.ToString(),
                RTAvg.ToString(),
                MassMedian.ToString(),
                TargetMass.ToString()
            };
            return String.Join(delimeter, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string GetHeader(string delimeter)
        {
            string[] values = new string[]
            {
                "Group",
                "RTTgt",
                "RTMed",
                "Found",
                "Missed",
                "ScoreMFEMax",
                "HeightMed",
                "MassAvg",
                "RTAvg",
                "MassMedian",
                "TargetMass"
            };
            return String.Join(delimeter, values);
        }

        #endregion
    }
}
