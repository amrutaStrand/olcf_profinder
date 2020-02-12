namespace Agilent.OpenLab.CompoundSpectrum
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
        using System.ComponentModel;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// CompoundSpectrumViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundSpectrumViewModel : BaseViewModel, ICompoundSpectrumViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     The plot control.
        /// </summary>
        private AgtPlotControl plotControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundSpectrumViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public CompoundSpectrumViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ICompoundSpectrumView>();
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
        public ICompoundSpectrumView View { get; set; }

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