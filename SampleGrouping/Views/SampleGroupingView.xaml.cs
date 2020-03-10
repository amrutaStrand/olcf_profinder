namespace Agilent.OpenLab.SampleGrouping
{
    #region

    using System.Windows;
    using Agilent.OpenLab.UI.Controls.WinFormsControls;
    using Agilent.OpenLab.SampleGrouping.ViewModels;
    using Infragistics.Win.UltraWinGrid;
    using System;
    using DataTypes;
    using System.ComponentModel;
    using System.Linq;

    #endregion

    /// <summary>
    /// Interaction logic for SampleGroupingView.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class SampleGroupingView : ISampleGroupingView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleGroupingView"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public SampleGroupingView()
        {
            this.InitializeComponent();
            this.ultraGrid = new AgtBaseUltraGrid();
            this.ultraGrid.TableHeadersViewModel = new SampleGroupingHeadersViewModel();
            
            this.ultraGrid.AfterRowActivate += this.OnAfterRowActivate;
            this.ultraGrid.InitializeLayout += this.OnInitializeLayout;
            this.ultraGrid.AfterSelectChange += this.AfterSelectChange;
            this.ultraGrid.Enabled = true;
            this.ultraGrid.TextEditor.Enabled = true;
            
            
            

            // initialize the grid host control
            this.GridControlHost.Child = this.ultraGrid;
            this.GridControlHost.GotFocus += this.OnGridControlGotFocus;
            this.GridControlHost.Margin = new Thickness(0, 0, 0, 0);

            // activate grid validation
            this.gridValidationManager = new GridValueValidationManager(this.ultraGrid);
        }

        private void OnGridControlGotFocus(object sender, RoutedEventArgs e)
        {
            this.GridControlHost.Focus();
        }

        private void AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            BindingList<ISample> selectedSamples = new BindingList<ISample>();

            SelectedRowsCollection seletedRows = this.ultraGrid.Selected.Rows;
            if (seletedRows != null)
            {
                RowEnumerator enumerator = seletedRows.GetEnumerator();
                if (enumerator != null)
                {
                    while (enumerator.MoveNext())
                    {
                        UltraGridRow row = enumerator.Current;
                        ISample sample = row.ListObject as ISample;
                        if (sample != null)
                            selectedSamples.Add(sample);
                    }
                }
            }
            this.Model.SelectedSamples = selectedSamples;
        }

        private void OnInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
          
        }

        private void OnAfterRowActivate(object sender, EventArgs e)
        {
            if (this.ultraGrid.ActiveRow != null)
            {
                var focusedSample = this.ultraGrid.ActiveRow.ListObject as ISample;
                if (focusedSample != null)
                {
                    this.Model.FocusedSample = focusedSample;
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ISampleGroupingViewModel Model
        {
            get
            {
                return this.DataContext as ISampleGroupingViewModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        #endregion

        #region Private Members
        /// <summary>
        ///     The ultra grid.
        /// </summary>
        private readonly AgtBaseUltraGrid ultraGrid;
        private GridValueValidationManager gridValidationManager;
        #endregion

        #region Methods

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ultraGrid.DataSource = this.Model.Samples;
        }

        /// <summary>
        /// Called when [unload].
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// updates foucs of the table
        /// </summary>
        public void UpdateFocus()
        {
            this.ultraGrid.ActiveRow = null;
            this.ultraGrid.Selected.Rows.Clear();
            var rowToSelect = this.ultraGrid.Rows.FirstOrDefault(row => row.ListObject == this.Model.FocusedSample);
            if (rowToSelect != null)
            {
                rowToSelect.Activate();
            }
        }

        #endregion
    }
}