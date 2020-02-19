namespace Agilent.OpenLab.TICPlot
{
    #region

    using System.Windows;

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

        #endregion
    }
}