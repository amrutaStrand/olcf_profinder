namespace Agilent.OpenLab.CompoundChromatogram
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// CompoundChromatogramViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundChromatogramViewModel : BaseViewModel, ICompoundChromatogramViewModel
    {
        /// <summary>
        ///     The plot control.
        /// </summary>
        private AgtPlotControl plotControl;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundChromatogramViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public CompoundChromatogramViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ICompoundChromatogramView>();
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
        public ICompoundChromatogramView View { get; set; }

        /// <summary>
        ///     Gets PlotControl.
        /// </summary>
        public AgtPlotControl PlotControl
        {
            get
            {
                if (this.plotControl == null)
                {
                    this.plotControl = new AgtPlotControl();
                }

                return this.plotControl;
            }
        }
        #endregion
    }
}