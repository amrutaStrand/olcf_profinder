using Agilent.OpenLab.UI.Controls.WinFormsControls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.CompoundGroupsTable.ViewModels
{
    class CompoundGroupsHeadersViewModel : TableHeadersViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompoundGroupsHeadersViewModel" /> class.
        /// </summary>
        public CompoundGroupsHeadersViewModel()
        {
            this.AddHeader(
                CompoundGroupsTableHeaders.Group,
                new TableHeader
                {
                    Caption = "Group",
                    Description = "The compound group name",
                    Name = CompoundGroupsTableHeaders.Group,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RTTgt,
                new TableHeader
                {
                    Caption = "RT (Tgt)",
                    Description = "The target RT",
                    Name = CompoundGroupsTableHeaders.RTTgt,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RTmed,
                new TableHeader
                {
                    Caption = "RT (med)",
                    Description = "The median RT",
                    Name = CompoundGroupsTableHeaders.RTmed,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.Found,
                new TableHeader
                {
                    Caption = "Found",
                    Description = "Found",
                    Name = CompoundGroupsTableHeaders.Found,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.Missed,
                new TableHeader
                {
                    Caption = "Missed",
                    Description = "Missed",
                    Name = CompoundGroupsTableHeaders.Missed,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.ScoreMFEMax,
                new TableHeader
                {
                    Caption = "Score (MFE, max)",
                    Description = "The maximum MFE score.",
                    Name = CompoundGroupsTableHeaders.ScoreMFEMax,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.HeightMed,
                new TableHeader
                {
                    Caption = "Height (med)",
                    Description = "The median height",
                    Name = CompoundGroupsTableHeaders.HeightMed,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.MassAvg,
                new TableHeader
                {
                    Caption = "Mass (avg)",
                    Description = "The average mass",
                    Name = CompoundGroupsTableHeaders.MassAvg,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RtAvg,
                new TableHeader
                {
                    Caption = "RT (avg)",
                    Description = "The average RT",
                    Name = CompoundGroupsTableHeaders.RtAvg,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.MassMedian,
                new TableHeader
                {
                    Caption = "Mass (med)",
                    Description = "The median mass",
                    Name = CompoundGroupsTableHeaders.MassMedian,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.TargetMass,
                new TableHeader
                {
                    Caption = "Mass (Tgt)",
                    Description = "The target mass",
                    Name = CompoundGroupsTableHeaders.TargetMass,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });

        }
        #endregion
    }
}