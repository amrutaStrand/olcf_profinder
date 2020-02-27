namespace Agilent.OpenLab.ProfinderController
{
    #region

    using System;
    using System.Windows.Media.Imaging;

    using Agilent.OpenLab.Framework.UI.Layout.MenuInterfaces;
    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Unity;

    #endregion

    public partial class ProfinderControllerModule : BaseControllerModule
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfinderControllerModule"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        public ProfinderControllerModule(IUnityContainer container)
            : base(container)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Extends the menu.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override void ExtendMenu()
        {
            var menuDefinition = new MenuDefinition(this, string.Empty);

            var tabDefinition = new MenuTabDefinition("Experiment Setup", "E");

            menuDefinition.Add(tabDefinition);

            var tabDefinition1 = new MenuTabDefinition("Process", "P");

            menuDefinition.Add(tabDefinition1);

            var groupDefinition1 = new MenuGroupDefinition(this, "Workflow");

            tabDefinition1.Add(groupDefinition1);

            var tabDefinition2 = new MenuTabDefinition("Export/Report", "R");

            menuDefinition.Add(tabDefinition2);

            this.MenuService.CreateControllerModuleMenu(menuDefinition);

            var viewModel = this.Container.Resolve<IProfinderControllerViewModel>();

            IMenuGroupManager groupManager = this.MenuService.GetMenuGroupManager(this.ModuleId);

            if (groupManager != null)

            {

                groupManager.AddCommandTool(

                    viewModel.ExperimentSetupCommand,

                    this.GetImageFromImageFile("Images/experiment-setup.png"), this.GetImageFromImageFile("Images/experiment-setup.png"));

                groupManager.AddCommandTool(

                    viewModel.FeatureExtractionCommand,

                    this.GetImageFromImageFile("Images/feature-extraction.png"), this.GetImageFromImageFile("Images/feature-extraction.png"));
                groupManager.AddCommandTool(

                    viewModel.StatisticAnalysisCommand,

                    this.GetImageFromImageFile("Images/statistical-analysis.png"), this.GetImageFromImageFile("Images/statistical-analysis.png"));
                groupManager.AddCommandTool(

                    viewModel.IdentificationCommand,

                    this.GetImageFromImageFile("Images/compound-identification.png"), this.GetImageFromImageFile("Images/compound-identification.png"));
                groupManager.AddCommandTool(

                    viewModel.ReportCommand,

                    this.GetImageFromImageFile("Images/report-create.png"), this.GetImageFromImageFile("Images/report-create.png"));

            }

        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override void RegisterTypes()
        {
            this.Container.RegisterType
                <IProfinderControllerViewModel, ProfinderControllerViewModel>(
                    new ContainerControlledLifetimeManager());
            this.RegisterViewModel(this.Container.Resolve<IProfinderControllerViewModel>());
        }

        #endregion
    }
}