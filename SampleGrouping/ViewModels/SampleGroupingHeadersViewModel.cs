using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agilent.OpenLab.UI.Controls.WinFormsControls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Agilent.OpenLab.SampleGrouping.ViewModels
{
    /// <summary>
    /// Class that sets headears and binding keys for ultraGrid
    /// </summary>
    public class SampleGroupingHeadersViewModel : TableHeadersViewModel
    {
        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleGroupingHeadersViewModel" /> class.
        /// </summary>
        public SampleGroupingHeadersViewModel()
        {
            this.AddHeader(
                SampleGroupingHeaders.HideOrShow,
                new TableHeader
                {
                    Caption = "Hide or Show ",
                    Description = "Hide or Show the sample by default true",
                    Name = "Hide/Show",
                    Width = 100,
                    Alignment = HAlign.Center,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = false

                });
            this.AddHeader(
                SampleGroupingHeaders.ExpOrder,
                new TableHeader
                {
                    Caption = "Exp. Order",
                    Description = "The Experiment Order",
                    Name = "Exp. order",
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = true
                    
                });

            this.AddHeader(
                SampleGroupingHeaders.FileName,
                new TableHeader
                {
                    Caption = "File Name",
                    Description = "The file name",
                    Name = "File Name",
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = true
                });

            this.AddHeader(
                SampleGroupingHeaders.Source,
                new TableHeader
                {
                    Caption = "Source",
                    Description = "The Ion Source",
                    Name = SampleGroupingHeaders.Source,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = true
                });

            this.AddHeader(
                SampleGroupingHeaders.SampleType,
                new TableHeader
                {
                    Caption = "Sampel Type",
                    Description = "The Sample Type",
                    Name = "Sample Type",
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = false
                });
            this.AddHeader(
                SampleGroupingHeaders.Group,
                new TableHeader
                {
                    Caption = "Group",
                    Description = "The Group",
                    Name = SampleGroupingHeaders.Group,
                    Width = 100,
                    Alignment = HAlign.Left,
                    SortOrder = SortIndicator.None,
                    Hidden = false,
                    ReadOnly = false
                }) ;
        }
        #endregion
    }
}
