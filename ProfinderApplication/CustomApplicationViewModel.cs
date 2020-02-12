// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomApplicationViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   CustomApplicationViewModel
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Agilent.OpenLab.ProfinderApplication
{
    #region

    using System.Collections.Generic;

    using Agilent.OpenLab.Framework.Common.Utilities;
    using Agilent.OpenLab.Framework.Infrastructure.Services.Interfaces;
    using Agilent.OpenLab.Framework.UI.ApplicationContext;
    using Agilent.OpenLab.Framework.UI.Common.ApplicationPath;
    using Agilent.OpenLab.Framework.UI.Common.Help;
    using Agilent.OpenLab.Framework.UI.Layout.MenuInterfaces;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// CustomApplicationViewModel
    /// </summary>
    public class CustomApplicationViewModel : ApplicationViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomApplicationViewModel"/> class.
        /// </summary>
        /// <param name="bootstrapper">
        /// The bootstrapper.
        /// </param>
        /// <param name="applicationResources">
        /// The application resources.
        /// </param>
        public CustomApplicationViewModel(Bootstrapper bootstrapper, IApplicationResources applicationResources)
            : base(bootstrapper, applicationResources)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The on after startup.
        /// </summary>
        /// <remarks>
        /// Override this method to do application specific initialization on start up.
        /// The base implementation is empty.
        /// </remarks>
        public override void OnAfterStartup()
        {
            // TODO: remove or implement
            base.OnAfterStartup();
        }

        /// <summary>
        ///     The on before exit.
        /// </summary>
        /// <remarks>
        /// Override this method to do application specific cleanup on exit.
        /// The base implementation is empty.
        /// </remarks>
        public override void OnBeforeExit()
        {
            // TODO: remove or implement
            base.OnBeforeExit();
        }

        /// <summary>
        ///     The register sub view details.
        /// </summary>
        /// <remarks>
        /// Override this method to assign a specific custom images and labels for sub-views/layouts that are defined in the workspace layout file.
        /// The sub-view/layout is identified by the tag specified in the workspaces layout file.
        /// The base implementation is empty.
        /// </remarks>
        public override void RegisterSubViewDetails()
        {
            // TODO: remove or implement
            base.RegisterSubViewDetails();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks for valid license.
        /// </summary>
        /// <returns>
        ///     The <see cref="ILicense" />.
        /// </returns>
        /// <remarks>       
        /// Override this method to do application specific license handling.
        /// If null is returned no specific license handling is done.
        /// Method should throw exception if no valid license can be retrieved.
        /// The base implementation returns null.
        /// </remarks>
        protected override ILicense AcquireLicense()
        {
            // TODO: remove or implement
            return base.AcquireLicense();
        }

        /// <summary>
        /// Checks if the application has unapplied changes in modules.
        /// </summary>
        /// <param name="unsavedDocumentStrings">
        /// The unsaved documents.
        /// </param>
        /// <remarks>
        /// Override this method to check and handle unapplied changes in UI/controller modules.
        /// Add one or multiple string(s) to the collection, so that this information can
        /// be shown to the user. 
        /// The base implementation checks if any of the UI/controller modules has unapplied changes.        
        /// The base implementation should usually be fine for all kinds of applications.
        /// </remarks>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ApplicationHasUnappliedChangesInUiModules(ICollection<string> unsavedDocumentStrings)
        {
            // TODO: remove or implement
            return base.ApplicationHasUnappliedChangesInUiModules(unsavedDocumentStrings);
        }

        /// <summary>
        /// The application has unsaved changes in data modules.
        /// </summary>
        /// <param name="unsavedDocumentStrings">
        /// The unsaved documents.
        /// </param>
        /// <remarks>
        /// Override this method to check and handle unapplied changes in any application specific
        /// data modules. Add one or multiple string(s) to the collection, so that this information can
        /// be shown to the user. 
        /// The base implementation does not do anything, as the base implementation does not know about
        /// any application specific registered data modules etc.
        /// </remarks>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ApplicationHasUnsavedChangesInDataModules(ICollection<string> unsavedDocumentStrings)
        {
            // TODO: remove or implement
            return base.ApplicationHasUnsavedChangesInDataModules(unsavedDocumentStrings);
        }

        /// <summary>
        /// Combines the unsaved document strings.
        /// </summary>
        /// <param name="unsavedDocumentStrings">
        /// The unsaved document strings.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <remarks>
        /// Override this method to specify how the unsaved document strings should be assembled to form
        /// the message shown to the user.
        /// </remarks>
        protected override string CombineUnsavedDocumentStrings(ICollection<string> unsavedDocumentStrings)
        {
            // TODO: remove or implement
            return base.CombineUnsavedDocumentStrings(unsavedDocumentStrings);
        }

        /// <summary>
        /// Registers the application paths.
        /// </summary>
        /// <param name="applicationPathRepository">
        /// The application path repository.
        /// </param>
        /// <remarks>
        /// Override this method to register application specific paths.
        /// The base implementation does not do anything
        /// </remarks>
        protected override void RegisterApplicationPaths(IApplicationPathRepository applicationPathRepository)
        {
            // TODO: remove or implement
            base.RegisterApplicationPaths(applicationPathRepository);
        }

        /// <summary>
        /// Registers application menu commands.
        /// </summary>       
        protected override void RegisterApplicationMenuCommands()
        {
            // TODO: remove or implement
            base.RegisterApplicationMenuCommands();
        }

        /// <summary>
        /// Register the help provider.
        /// </summary>        
        /// <remarks>
        /// Override this method to setup and register the help provider.
        /// The base implementation does not do anything.
		/// Currently a CHM and Html5 help provider is provided by the framework,
		/// additional help providers can be implemented by the specific application 
		/// if needed.
        /// </remarks>
        protected override void RegisterHelpProvider()
        {
            // TODO: remove or implement
            base.RegisterHelpProvider();

            // Setup CHM Help Provider (example)
            // var helpProvider = new ChmHelpProvider();
            // helpProvider.DefaultTopic = @"/First_Topic.htm";
            // helpProvider.RegisterCultureSpecificHelpFile("en-US", @".\Help\en-US.chm");
            // helpProvider.RegisterCultureSpecificHelpFile("ja-JP", @".\Help\ja-Jp.chm");
            // helpProvider.RegisterCultureSpecificHelpFile("zh-CN", @".\Help\zh-CN.chm");
            // this.Container.RegisterInstance<IHelpProvider>(helpProvider);

            // Setup Html5 Help Provider (example)
            // var helpProvider = new Html5HelpProvider();
            // helpProvider.DefaultTopic = @"/First_Topic.htm";
            // helpProvider.RegisterCultureSpecificHelpDirectory("en-US", @"/Help/en-US");
            // helpProvider.RegisterCultureSpecificHelpDirectory("ja-JP", @"/Help/ja-JP");
            // helpProvider.RegisterCultureSpecificHelpDirectory("zh-CN", @"/Help/zh-CN");
            // this.Container.RegisterInstance<IHelpProvider>(helpProvider);
        }

        /// <summary>
        /// Registers the log files.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <remarks>
        /// Override this method to register application specific log files with shared services.
        /// The base implementation does not do anything
        /// </remarks>
        protected override void RegisterLogFiles(IServices services)
        {
            // TODO: remove or implement
            base.RegisterLogFiles(services);
        }

        #endregion
    }
}