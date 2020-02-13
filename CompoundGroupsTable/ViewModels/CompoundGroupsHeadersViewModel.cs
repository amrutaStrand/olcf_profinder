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
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RTgt,
                new TableHeader
                {
                    Caption = "Tgt. RT",
                    Description = "The target RT",
                    Name = CompoundGroupsTableHeaders.RTgt,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RTmed,
                new TableHeader
                {
                    Caption = "Med. RT",
                    Description = "The median RT",
                    Name = CompoundGroupsTableHeaders.RTmed,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
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
                    Hidden = true
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
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.ScoreMFEMax,
                new TableHeader
                {
                    Caption = "MFE max. score",
                    Description = "The maximum MFE score.",
                    Name = CompoundGroupsTableHeaders.ScoreMFEMax,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.HeightMed,
                new TableHeader
                {
                    Caption = "Med. Height",
                    Description = "The median height",
                    Name = CompoundGroupsTableHeaders.HeightMed,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.MassAvg,
                new TableHeader
                {
                    Caption = "Avg. Mass",
                    Description = "The average mass",
                    Name = CompoundGroupsTableHeaders.MassAvg,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.RtAvg,
                new TableHeader
                {
                    Caption = "Avg. RT",
                    Description = "The average RT",
                    Name = CompoundGroupsTableHeaders.RtAvg,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.MassMedian,
                new TableHeader
                {
                    Caption = "Med. Mass",
                    Description = "The median mass",
                    Name = CompoundGroupsTableHeaders.MassMedian,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });
            this.AddHeader(
                CompoundGroupsTableHeaders.TargetMass,
                new TableHeader
                {
                    Caption = "Tgt. Mass",
                    Description = "The target mass",
                    Name = CompoundGroupsTableHeaders.TargetMass,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = true
                });

        }
        #endregion
    }
}