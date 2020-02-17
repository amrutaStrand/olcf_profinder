// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomApplicationResouces.cs" company="">
//   
// </copyright>
// <summary>
//   The custom application resouces.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Agilent.OpenLab.ProfinderApplication
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Agilent.OpenLab.Framework.UI.ApplicationContext;

    /// <summary>
    /// The custom application resouces.
    /// </summary>
    public class CustomApplicationResources : ApplicationResources
    {
        #region Public Properties

        /// <summary>
        ///     Gets the application copyright string.
        /// </summary>
        /// <remark>
        /// The application copyright shows up on the splash screen.
        /// The base implementation takes the application copyright from the 
        /// AssemblyInfo file
        /// </remark>
        public override string ApplicationCopyright
        {
            get
            {
                // TODO: remove or implement
                return base.ApplicationCopyright;
            }
        }

        /// <summary>
        /// Gets the application logo.
        /// </summary>
        public override ImageSource ApplicationLogo
        {
            get
            {
                return this.ApplicationLogoBitmapImage
                       ?? (this.ApplicationLogoBitmapImage =
                           new BitmapImage(new Uri("pack://application:,,,/Images/AppLogo.png")));
            }
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <remark>
        /// The name of the application shows up in the application title bar.
        /// The base implementation takes the application name from the assembly title as
        /// specified in the AssemblyInfo file
        /// </remark>
        public override string ApplicationName
        {
            get
            {
                // TODO: remove or implement
                //return base.ApplicationName;
                return "Agilent MassHunter Profinder";
            }
        }

        /// <summary>
        /// Gets the short name of the application.
        /// </summary>
        /// <remark>
        /// The short name of the application shows up in the splash screen and the about box.
        /// The default implementation takes the ApplicationName.
        /// </remark>
        public override string ApplicationShortName
        {
            get
            {
                // TODO: remove or implement
                return "Profinder";
            }
        }

        /// <summary>
        ///     Gets the application logo.
        /// </summary>        
        public override BitmapImage ApplicationGlyph
        {
            get
            {
                return this.ApplicationGlyphBitmapImage
                       ?? (this.ApplicationGlyphBitmapImage =
                           new BitmapImage(
                               new Uri(
                               "pack://application:,,,/Images/Glyph.png")));
            }
        }

        /// <summary>
        /// Gets the application group.
        /// </summary>
        /// <remark>
        /// The application group shows up in the about box.
        /// The default implementation returns an empty string.
        /// </remark>
        public override string ApplicationGroup
        {
            get
            {
                // TODO: remove or implement
                return "Agilent MassHunter Profinder";
            }
        }

        /// <summary>
        /// Gets the application group with version.
        /// </summary>
        /// <remark>
        /// The application group with version shows up in the splash screen.
        /// The default implementation returns an empty string.
        /// </remark>
        public override string ApplicationGroupWithVersion
        {
            get
            {
                // TODO: remove or implement
                return "Agilent MassHunter Profinder";
            }
        }

        /// <summary>
        ///     Gets the application startup message.
        /// </summary>
        /// <remark>
        /// This message shows up on the splash screen when the splash screen is initially shown.
        /// The base implementation provides a default message like 'Application starting up ...' 
        /// </remark>
        public override string ApplicationStartupMessage
        {
            get
            {
                // TODO: remove or implement
                return base.ApplicationStartupMessage;
            }
        }

        /// <summary>
        ///     Gets the application version.
        /// </summary>
        /// <remark>
        /// The application version shows up on the splash screen.
        /// The base implementation takes the application version from the 
        /// AssemblyInfo file
        /// </remark>
        public override string ApplicationVersion
        {
            get
            {
                // TODO: remove or implement
                return base.ApplicationVersion;
            }
        }

        /// <summary>
        ///     Gets the initializing modules message.
        /// </summary>                
        /// <remark>
        /// This message is used on the splash screen while the individual UI/controller modules are loaded.
        /// The base implementation provides a default message like 'Initializing modules...' 
        /// </remark>
        public override string InitializingModulesMessage
        {
            get
            {
                // TODO: remove or implement
                return base.InitializingModulesMessage;
            }
        }

        /// <summary>
        ///     Gets the path to the custom tool configuration file.
        /// </summary>
        /// <remarks>
        /// The full path to the custom tools configuration file. By default it is set to:
        /// %ProgramData%\%Company%\%Product%\CustomToolsConfiguration.xml
        /// </remarks>
        public override string PathToCustomToolsConfigurationFile
        {
            get
            {
                // TODO: remove or implement
                return base.PathToCustomToolsConfigurationFile;
            }
        }

        /// <summary>
        ///     Gets the shutdown block reason message.
        /// </summary>                
        /// <remark>
        /// This message is shown on log off if there are unsaved changes.
        /// The base implementation provides a default message like 'There are unsaved changes' 
        /// </remark>
        public override string ShutdownBlockReason
        {
            get
            {
                // TODO: remove or implement
                return base.ShutdownBlockReason;
            }
        }

        /// <summary>
        /// Gets the splash screen image.
        /// </summary>       
        public override BitmapImage SplashScreenImage
        {
            get
            {
                // TODO: remove or implement
                return base.SplashScreenImage;
            }
        }

        /// <summary>
        ///     Gets the unhandled exception title.
        /// </summary>                
        /// <remark>
        /// This message is shown when an unhandled exception was thrown.
        /// The base implementation provides a default message like 'An unhandled exception has occured' 
        /// </remark>
        public override string UnhandledExceptionTitle
        {
            get
            {
                // TODO: remove or implement
                return base.UnhandledExceptionTitle;
            }
        }

        /// <summary>
        ///     Gets the user layout key.
        /// </summary>
        /// <remarks>
        ///     This is the key under which the layout of the application (workspace configuration) is stored in the services.        
        /// </remarks>
        public override string UserLayoutKey
        {
            get
            {
                return @"ProfinderApplication_UserLayout";
            }
        }

        /// <summary>
        /// Gets the workspace configuration filename.
        /// </summary>
        /// <remarks>
        /// The name of the workspace configuration file. On startup the application if looking for this file 
        /// in the Layout directory.
        /// </remarks>
        public override string WorkspaceConfigurationFileName
        {
            get
            {
                return "ProfinderApplication.workspace.xml";
            }
        }

        #endregion
    }
}