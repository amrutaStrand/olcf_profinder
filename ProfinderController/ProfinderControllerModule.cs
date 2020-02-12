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
            // Create module menu definition with one group
            var menuDefinition = new MenuGroupDefinition(this, this.Caption);
            this.MenuService.CreateControllerModuleMenuGroup(menuDefinition);

            // register module specific menu definition
            // and retrieve associated nemu group manager
            var viewModel = this.Container.Resolve<IProfinderControllerViewModel>();
            IMenuGroupManager groupManager = this.MenuService.GetMenuGroupManager(this.ModuleId);
            if (groupManager != null)
            {
                groupManager.AddCommandTool(
                    viewModel.ToggleCommandA,
                    this.GetImageFromImageFile("Images/TestImage.png"));
                groupManager.AddCommandTool(
                    viewModel.TriggerCommandB,
                    this.GetImageFromImageFile("Images/TestImage.png"));
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