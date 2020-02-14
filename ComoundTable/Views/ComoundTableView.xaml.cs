namespace Agilent.OpenLab.ComoundTable
{
    #region

    using System.Windows;
    using System.Linq;
    using Agilent.OpenLab.UI.Controls.WinFormsControls;
    using Agilent.OpenLab.ComoundTable.ViewModels;
    using Infragistics.Win.UltraWinGrid;
    using System;
    using DataTypes;
    using System.ComponentModel;

    #endregion

    /// <summary>
    /// Interaction logic for ComoundTableView.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class ComoundTableView : IComoundTableView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComoundTableView"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ComoundTableView()
        {
            this.InitializeComponent();
            this.ultraGrid = new AgtBaseUltraGrid();
            this.ultraGrid.TableHeadersViewModel = new CompoundTableHeadersViewModel();
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
        public IComoundTableViewModel Model
        {
            get
            {
                return this.DataContext as IComoundTableViewModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        ///     The grid validation manager.
        /// </summary>
        private readonly GridValueValidationManager gridValidationManager;

        /// <summary>
        ///     The ultra grid.
        /// </summary>
        private readonly AgtBaseUltraGrid ultraGrid;


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
            this.ultraGrid.DataSource = this.Model.Compounds;
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

        private void OnInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

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
                var focusedCompound = this.ultraGrid.ActiveRow.ListObject as ICompound;
                if (focusedCompound != null)
                {
                    this.Model.FocusedCompound = focusedCompound;
                }
            }
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

            BindingList<ICompound> selectedCompounds = new BindingList<ICompound>();

            SelectedRowsCollection seletedRows = this.ultraGrid.Selected.Rows;
            if (seletedRows != null)
            {
                RowEnumerator enumerator = seletedRows.GetEnumerator();
                if (enumerator != null)
                {
                    while (enumerator.MoveNext())
                    {
                        UltraGridRow row = enumerator.Current;
                        ICompound compound = row.ListObject as ICompound;
                        if (compound != null)
                            selectedCompounds.Add(compound);
                    }
                }
            }
            this.Model.SelectedCompounds = selectedCompounds;

        }

        /// <summary>
        ///     The update focus.
        /// </summary>
        public void UpdateFocus()
        {
            this.ultraGrid.ActiveRow = null;
            this.ultraGrid.Selected.Rows.Clear();
            var rowToSelect = this.ultraGrid.Rows.FirstOrDefault(row => row.ListObject == this.Model.FocusedCompound);
            if (rowToSelect != null)
            {
                rowToSelect.Activate();
            }
        }


        #endregion
    }
}