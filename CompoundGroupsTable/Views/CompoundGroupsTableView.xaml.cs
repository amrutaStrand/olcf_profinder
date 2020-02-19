namespace Agilent.OpenLab.CompoundGroupsTable
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    #region

    using System.Windows;
    using Agilent.OpenLab.CompoundGroupsTable.ViewModels;
    using Agilent.OpenLab.UI.Controls.WinFormsControls;
    using DataTypes;
    using Infragistics.Win.UltraWinGrid;


    #endregion

    /// <summary>
    /// Interaction logic for CompoundGroupsTableView.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundGroupsTableView : ICompoundGroupsTableView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundGroupsTableView"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public CompoundGroupsTableView()
        {
            this.InitializeComponent();
            this.ultraGrid = new AgtBaseUltraGrid();
            this.ultraGrid.TableHeadersViewModel = new CompoundGroupsHeadersViewModel();
            this.ultraGrid.ReadOnlyGrid = this.ultraGrid.TableHeadersViewModel.AllHeadersReadOnly();
            this.ultraGrid.AfterRowActivate += this.OnAfterRowActivate;
            this.ultraGrid.InitializeLayout += this.OnInitializeLayout;
            this.ultraGrid.AfterSelectChange += this.AfterSelectChange;

            // initialize the grid host control
            this.GridControlHost.Child = this.ultraGrid;
            this.GridControlHost.GotFocus += this.OnGridControlGotFocus;
            this.GridControlHost.Margin = new Thickness(0, 0, 0, 0);

            // activate grid validation
            this.gridValidationManager = new GridValueValidationManager(this.ultraGrid);

        }

        private void OnInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            
        }


        #endregion

        #region Public Properties
        /// <summary>
        ///     The grid validation manager.
        /// </summary>
        private readonly GridValueValidationManager gridValidationManager;

        /// <summary>
        ///     The ultra grid.
        /// </summary>
        private readonly AgtBaseUltraGrid ultraGrid;


        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ICompoundGroupsTableViewModel Model
        {
            get
            {
                return this.DataContext as ICompoundGroupsTableViewModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

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
            this.ultraGrid.DataSource = this.Model.CompoundGroups;
        }

        /// <summary>
        /// The on grid control got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnGridControlGotFocus(object sender, EventArgs e)
        {
            this.GridControlHost.Focus();
        }

        /// <summary>
        /// The on after row activate.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnAfterRowActivate(object sender, EventArgs e)
        {
            if (this.ultraGrid.ActiveRow != null)
            {
                var focusedCompoundGroup = this.ultraGrid.ActiveRow.ListObject as ICompoundGroupItem;
                if (focusedCompoundGroup != null)
                {
                    this.Model.FocusedCompoundGroup = focusedCompoundGroup;
                }
            }
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
        ///     The unsubscribe event handlers.
        /// </summary>
        public void UnsubscribeEventHandlers()
        {
            this.ultraGrid.AfterRowActivate -= this.OnAfterRowActivate;
            this.ultraGrid.InitializeLayout -= this.OnInitializeLayout;
            this.GridControlHost.GotFocus -= this.OnGridControlGotFocus;
        }

        private void AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

            BindingList<ICompoundGroupItem> selectedCompounds = new BindingList<ICompoundGroupItem>();

            SelectedRowsCollection seletedRows = this.ultraGrid.Selected.Rows;
            if (seletedRows != null)
            {
                RowEnumerator enumerator = seletedRows.GetEnumerator();
                if (enumerator != null)
                {
                    while (enumerator.MoveNext())
                    {
                        UltraGridRow row = enumerator.Current;
                        ICompoundGroupItem compound = row.ListObject as ICompoundGroupItem;
                        if (compound != null)
                            selectedCompounds.Add(compound);
                    }
                }
            }
            this.Model.SelectedCompoundGroups = selectedCompounds;

        }

        /// <summary>
        ///     The update focus.
        /// </summary>
        public void UpdateFocus()
        {
            this.ultraGrid.ActiveRow = null;
            this.ultraGrid.Selected.Rows.Clear();
            var rowToSelect = this.ultraGrid.Rows.FirstOrDefault(row => row.ListObject == this.Model.FocusedCompoundGroup);
            if (rowToSelect != null)
            {
                rowToSelect.Activate();
            }
        }




        #endregion
    }
}