using Agilent.OpenLab.UI.Controls.WinFormsControls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.ComoundTable.ViewModels
{
    class CompoundTableHeadersViewModel : TableHeadersViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompoundTableHeadersViewModel" /> class.
        /// </summary>
        public CompoundTableHeadersViewModel()
        {
            this.AddHeader(
                CompoundTableHeaders.File,
                new TableHeader
                {
                    Caption = "File",
                    Description = "The file name",
                    Name = CompoundTableHeaders.File,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Mass,
                new TableHeader
                {
                    Caption = "Mass",
                    Description = "The Mass",
                    Name = CompoundTableHeaders.Mass,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.RT,
                new TableHeader
                {
                    Caption = "RT",
                    Description = "The Retention Time",
                    Name = CompoundTableHeaders.RT,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Area,
                new TableHeader
                {
                    Caption = "Area",
                    Description = "The Area",
                    Name = CompoundTableHeaders.Area,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Volume,
                new TableHeader
                {
                    Caption = "Volume",
                    Description = "The Volume",
                    Name = CompoundTableHeaders.Volume,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Width,
                new TableHeader
                {
                    Caption = "Width",
                    Description = "The Width",
                    Name = CompoundTableHeaders.Width,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Format = "F3",
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Saturated,
                new TableHeader
                {
                    Caption = "Saturated",
                    Description = "The Saturated",
                    Name = CompoundTableHeaders.Saturated,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.Ions,
                new TableHeader
                {
                    Caption = "Ions",
                    Description = "The Ions",
                    Name = CompoundTableHeaders.Ions,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
            this.AddHeader(
                CompoundTableHeaders.ZCount,
                new TableHeader
                {
                    Caption = "ZCount",
                    Description = "The ZCount",
                    Name = CompoundTableHeaders.ZCount,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false
                });
        }
        #endregion
    }
}
