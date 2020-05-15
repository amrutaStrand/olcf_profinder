// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   The app.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Agilent.OpenLab.ProfinderApplication
{
    #region

    using System;
    using System.Globalization;
    using System.Security.Authentication;
    using System.Windows;
    using System.Windows.Threading;

    using Agilent.OpenLab.ProfinderApplication.Properties;
    using Agilent.OpenLab.Framework.UI.ApplicationContext;
    using Agilent.OpenLab.Framework.UI.Common;
    using Agilent.OpenLab.Framework.UI.Windows;

    #endregion

    /// <summary>
    ///     The app.
    /// </summary>
    public partial class App:Application
    {
        #region Fields

        /// <summary>
        ///     The application resources
        /// </summary>
        /// void
        private readonly IApplicationResources applicationResources = new CustomApplicationResources();

        /// <summary>
        ///     The application window.
        /// </summary>
        private IApplicationWindow applicationWindow;

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.
        /// </param>        
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += this.ApplicationDispatcherUnhandledException;

            var splashWindow = new SplashScreenWindow(this.applicationResources);
            try
            {
                // show the splash screen
                splashWindow.Show();

                // initialize bootstarpper
                var bootstrapper = new CustomBootstrapper(this.applicationResources);

                // initialize application view model
                var applicationViewModel = new CustomApplicationViewModel(bootstrapper, this.applicationResources);

                // create and show application window
                this.applicationWindow = ApplicationWindowFactory.CreateApplicationWindow(applicationViewModel, this.applicationResources);
                this.applicationWindow.OnStartup(splashWindow);
                this.applicationWindow.Show();

                // once applicationWindow is in place, close splash screen
                splashWindow.Close();

                TraceUI.Log.Info("Application successfully started");
            }
            catch (Exception ex)
            {
                splashWindow.Close();
                TraceUI.Log.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Event handler for unhandled exception.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.
        /// </param>        
        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is AuthenticationException)
            {
                CommonDialogsProvider.ShowError(
                CustomResources.AuthenticationFailedCaption,
                CustomResources.AuthenticationFailedMessage);
            }
            else
            {
                CommonDialogsProvider.ShowError(
                    this.applicationResources.UnhandledExceptionTitle, e.Exception.ToString());
                TraceUI.Log.ExceptionCritical(e.Exception);
            }

            e.Handled = true;
            e.Dispatcher.InvokeShutdown();
        }

        /// <summary>
        /// Called when application session is ending.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.SessionEndingCancelEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// Application session ending occurs for example if the user tries to log off.
        /// </remarks>
        private void OnApplicationSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            if (!this.applicationWindow.CanExit())
            {
                e.Cancel = true;
            }
            else
            {
                this.applicationWindow.UnregisterShutdownReason();
            }
        }

        /// <summary>
        /// Called when the application is about to exit.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.ExitEventArgs"/> instance containing the event data.
        /// </param>        
        private void OnExit(object sender, ExitEventArgs e)
        {
            this.applicationWindow.OnExit();
            this.DispatcherUnhandledException -= this.ApplicationDispatcherUnhandledException;
            TraceUI.Log.Close();
        }

        #endregion
    }
}