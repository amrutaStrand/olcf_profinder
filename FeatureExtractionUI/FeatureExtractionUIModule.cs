namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using System;
    using System.Windows.Media.Imaging;

    using Agilent.OpenLab.Framework.UI.Layout.MenuInterfaces;
    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// FeatureExtractionUIModule
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class FeatureExtractionUIModule : BaseUIModule
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureExtractionUIModule"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public FeatureExtractionUIModule(IUnityContainer container)
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
            // Create menu definition with one tab which contains one group
            var menuDefinition = new MenuDefinition(this, string.Empty);
            var tabDefinition = new MenuTabDefinition(this.Caption, "T");
            menuDefinition.Add(tabDefinition);
            var groupDefinition = new MenuGroupDefinition(this, this.Caption);
            tabDefinition.Add(groupDefinition);

            // register module specific menu definition
            // and retrieve associated menu group manager
            this.MenuService.CreateUIModuleMenu(menuDefinition);
            IMenuGroupManager groupManager = this.MenuService.GetMenuGroupManager(this.ModuleId);

            // add commands to the menu group manager                        
            if (groupManager != null)
            {
                var viewModel = this.Container.Resolve<IFeatureExtractionUIViewModel>();
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override void RegisterTypes()
        {
            this.Container.RegisterType<IFeatureExtractionUIViewModel, FeatureExtractionUIViewModel>(
                new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IFeatureExtractionUIView, FeatureExtractionUIView>(
                new ContainerControlledLifetimeManager());
            this.RegisterViewModel(this.Container.Resolve<IFeatureExtractionUIViewModel>());
            this.Container.Resolve<IRegionViewRegistry>().RegisterViewWithRegion(
                this.ModuleId, () => this.Container.Resolve<IFeatureExtractionUIViewModel>().View);
        }

        #endregion
    }
}