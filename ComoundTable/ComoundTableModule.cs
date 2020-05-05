namespace Agilent.OpenLab.ComoundTable
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
    /// ComoundTableModule
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class ComoundTableModule : BaseUIModule
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComoundTableModule"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public ComoundTableModule(IUnityContainer container)
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
                var viewModel = this.Container.Resolve<IComoundTableViewModel>();
                groupManager.AddCommandTool(
                    viewModel.ExportCommand,
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
            this.Container.RegisterType<IComoundTableViewModel, ComoundTableViewModel>(
                new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IComoundTableView, ComoundTableView>(
                new ContainerControlledLifetimeManager());
            this.RegisterViewModel(this.Container.Resolve<IComoundTableViewModel>());
            this.Container.Resolve<IRegionViewRegistry>().RegisterViewWithRegion(
                this.ModuleId, () => this.Container.Resolve<IComoundTableViewModel>().View);
        }

        #endregion
    }
}