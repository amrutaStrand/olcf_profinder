namespace Agilent.OpenLab.TICPlot
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// TICPlotViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class TICPlotViewModel : BaseViewModel, ITICPlotViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TICPlotViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public TICPlotViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ITICPlotView>();
            this.View.Model = this;
            this.SubscribeEvents();
            this.InitializeCommands();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ITICPlotView View { get; set; }

        #endregion
    }
}