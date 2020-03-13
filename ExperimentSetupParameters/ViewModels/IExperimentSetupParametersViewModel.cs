namespace Agilent.OpenLab.ExperimentSetupParameters
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;

    #endregion

    /// <summary>
    /// IExperimentSetupParametersViewModel
    /// </summary>
    public interface IExperimentSetupParametersViewModel : IBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the toggle command A.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> ToggleCommandA { get; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> TriggerCommandB { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        IExperimentSetupParametersView View { get; }

        #endregion
    }
}