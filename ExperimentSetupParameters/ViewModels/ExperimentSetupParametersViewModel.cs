namespace Agilent.OpenLab.ExperimentSetupParameters
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// ExperimentSetupParametersViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class ExperimentSetupParametersViewModel : BaseViewModel, IExperimentSetupParametersViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentSetupParametersViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public ExperimentSetupParametersViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<IExperimentSetupParametersView>();
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
        public IExperimentSetupParametersView View { get; set; }

        #endregion
    }
}