// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomBootstrapper.cs" company="">
//   
// </copyright>
// <summary>
//   Custom Bootstrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Agilent.OpenLab.ProfinderApplication
{
    #region

    using Agilent.OpenLab.Framework.UI.ApplicationContext;

    #endregion

    /// <summary>
    ///     Custom Bootstrapper
    /// </summary>
    public class CustomBootstrapper : Bootstrapper
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBootstrapper"/> class.
        /// </summary>
        /// <param name="applicationResources">
        /// The application resources.
        /// </param>
        public CustomBootstrapper(IApplicationResources applicationResources)
            : base(applicationResources)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register application parameters.
        /// </summary>
        protected override void RegisterApplicationParameters()
        {
            // TODO: remove or implement
            base.RegisterApplicationParameters();
        }

        /// <summary>
        ///     Register shared services.
        /// </summary>
        /// <remarks>
        /// Override this method to register your own application specific shared services.
        /// The base implementation registers the default shared services.
        /// </remarks>
        protected override void RegisterSharedServices()
        {
            // TODO: remove or implement
            base.RegisterSharedServices();
        }

        /// <summary>
        ///     Register specific services.
        /// </summary>
        /// <remarks>
        /// Override this method to register additional application specific services and types,
        /// like for example a specific data module.
        /// The base implementation is just empty.
        /// </remarks>
        protected override void RegisterSpecificServices()
        {
            // TODO: remove or implement
            base.RegisterSpecificServices();
        }

        #endregion
    }
}