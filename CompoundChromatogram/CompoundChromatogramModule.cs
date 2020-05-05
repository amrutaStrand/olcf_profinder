namespace Agilent.OpenLab.CompoundChromatogram
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
    /// CompoundChromatogramModule
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundChromatogramModule : BaseUIModule
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundChromatogramModule"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public CompoundChromatogramModule(IUnityContainer container)
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
            var groupDefinition = new MenuGroupDefinition(this, "Display Mode");
            tabDefinition.Add(groupDefinition);

            // register module specific menu definition
            // and retrieve associated menu group manager
            this.MenuService.CreateUIModuleMenu(menuDefinition);
            IMenuGroupManager groupManager = this.MenuService.GetMenuGroupManager(this.ModuleId);

            // add commands to the menu group manager                        
            if (groupManager != null)
            {
                var viewModel = this.Container.Resolve<ICompoundChromatogramViewModel>();
                groupManager.AddCommandTool(
                    viewModel.ListModeCommand, "List",
                    this.GetImageFromImageFile("Images/ListMode.png"));
                groupManager.AddCommandTool(
                   viewModel.OverlayModeCommand, "Overlay",
                   this.GetImageFromImageFile("Images/OverlayMode.png"));
                groupManager.AddCommandTool(
                    viewModel.GroupOverlayModeCommand, "GroupOverlay",
                    this.GetImageFromImageFile("Images/SampleGroup.png"));
                groupManager.AddCommandTool(
                    viewModel.SampleGroupModeCommand, "Group",
                    this.GetImageFromImageFile("Images/ColorBySampleGroup.png"));
                groupManager.AddCommandTool(
                    viewModel.ExportCommand,
                    this.GetImageFromImageFile("Images/TestImage.png"),
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
            this.Container.RegisterType<ICompoundChromatogramViewModel, CompoundChromatogramViewModel>(
                new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ICompoundChromatogramView, CompoundChromatogramView>(
                new ContainerControlledLifetimeManager());
            this.RegisterViewModel(this.Container.Resolve<ICompoundChromatogramViewModel>());
            this.Container.Resolve<IRegionViewRegistry>().RegisterViewWithRegion(
                this.ModuleId, () => this.Container.Resolve<ICompoundChromatogramViewModel>().View);
        }

        #endregion
    }
}