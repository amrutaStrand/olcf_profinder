namespace Agilent.OpenLab.SampleGrouping
{
    using System.ComponentModel;
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.SampleGrouping.ViewModels;
    using Microsoft.Practices.Unity;
    using DataTypes;
    using System.Collections.Generic;
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
            this.Samples = new BindingList<ISample>();
            this.FilePaths = new List<string>();
            this.View = this.UnityContainer.Resolve<ISampleGroupingView>();
            this.View.Model = this;
            this.ExperimentContext = this.UnityContainer.Resolve<IExperimentContext>();
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
        public BindingList<ISample> Samples { get; set; }

        /// <summary>
        /// list of selected samples
        /// </summary>
        public BindingList<ISample> SelectedSamples { get; set; }
        /// <summary>
        /// focused sample
        /// </summary>
        public ISample FocusedSample { get; set; }
        /// <summary>
        /// Holds List Of Files Selected
        /// </summary>
        public List<string> FilePaths { get; private set; }


        #endregion

        #region Private Properties
        private IExperimentContext ExperimentContext;
        #endregion
    }
}