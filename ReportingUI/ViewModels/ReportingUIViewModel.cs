namespace Agilent.OpenLab.ReportingUI
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// ReportingUIViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class ReportingUIViewModel : BaseViewModel, IReportingUIViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingUIViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public ReportingUIViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<IReportingUIView>();
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
        public IReportingUIView View { get; set; }

        #endregion
    }
}