namespace Agilent.OpenLab.SampleGrouping
{
    using System.ComponentModel;
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.SampleGrouping.ViewModels;
    using Microsoft.Practices.Unity;
    using DataTypes;
    #endregion

    /// <summary>
    /// SampleGroupingViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class SampleGroupingViewModel : BaseViewModel, ISampleGroupingViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleGroupingViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public SampleGroupingViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ISampleGroupingView>();
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
        public ISampleGroupingView View { get; set; }

        /// <summary>
        /// list samples to show in table
        /// </summary>
        public BindingList<ISample> Samples => throw new System.NotImplementedException();

        /// <summary>
        /// list of selected samples
        /// </summary>
        public BindingList<ISample> SelectedSamples { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <summary>
        /// focused sample
        /// </summary>
        public ISample FocusedSample { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        #endregion
    }
}