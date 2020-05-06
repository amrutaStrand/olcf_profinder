namespace Agilent.OpenLab.CompoundChromatogram
{
    #region

    using System;

    using System.Windows;
    using System.Windows.Forms;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;

    #endregion

    /// <summary>
    /// Interaction logic for CompoundChromatogramView.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundChromatogramView : ICompoundChromatogramView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundChromatogramView"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public CompoundChromatogramView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties
        private bool viewInitialized = false;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler WinFormControlFocused;


        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ICompoundChromatogramViewModel Model
        {
            get
            {
                return this.DataContext as ICompoundChromatogramViewModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets PlotControl.
        /// </summary>
        public AgtPlotControl PlotControl
        {
            get
            {
                return this.Model != null ? this.Model.PlotControl : null;
            }
        }

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
            if (!this.viewInitialized)
            {
                this.plotControlHost.Child = this.PlotControl;
                this.PlotControl.GotFocus += this.OnPlotControlGotFocus;
                this.plotControlHost.Margin = new Thickness(0, 0, 0, 0);
                this.viewInitialized = true;
                this.PlotControl.ContextMenu = this.GetContextMenu();
            }
        }

        private ContextMenu GetContextMenu()
        {
            MenuItem[] menuItems = new MenuItem[]
            {
                new MenuItem("Export Data", OnExportClick)
            };
            return new ContextMenu(menuItems);
        }

        private void OnExportClick(object sender, EventArgs e)
        {
            Model.ExportData();
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
            if (this.viewInitialized)
            {
                this.PlotControl.GotFocus -= this.OnPlotControlGotFocus;
            }

            this.viewInitialized = false;
        }

        /// <summary>
        /// The on plot control got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnPlotControlGotFocus(object sender, EventArgs e)
        {
            if (this.WinFormControlFocused != null)
            {
                this.WinFormControlFocused(this, null);
            }

            // TODO:
            // the following line will fix the issue regarding the focusing of UI modules
            // however introduces problems with key events in win forms controls
            // Keyboard.Focus(this);            
            this.plotControlHost.Focus();
        }
        #endregion

        #endregion
    }
}