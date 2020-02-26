namespace Agilent.OpenLab.TICPlot
{
    #region
    using System;
    using System.Windows;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;

    #endregion

    /// <summary>
    /// Interaction logic for TICPlotView.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class TICPlotView : ITICPlotView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TICPlotView"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TICPlotView()
        {
            this.InitializeComponent();

            
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
        public ITICPlotViewModel Model
        {
            get
            {
                return this.DataContext as ITICPlotViewModel;
            }

            set
            {
                this.DataContext = value;
            }
        }

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

        /// <summary>
        ///     Occurs when any hosted win form control is focused.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler WinFormControlFocused;


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
            this.plotControlHost.Child = this.PlotControl;
            this.PlotControl.GotFocus += this.OnPlotControlGotFocus;
            this.plotControlHost.Margin = new Thickness(0, 0, 0, 0);
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
            this.PlotControl.GotFocus -= this.OnPlotControlGotFocus;
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
    }
}