namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// FeatureExtractionUIViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class FeatureExtractionUIViewModel : BaseViewModel, IFeatureExtractionUIViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureExtractionUIViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public FeatureExtractionUIViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<IFeatureExtractionUIView>();
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
        public IFeatureExtractionUIView View { get; set; }

        #endregion
    }
}