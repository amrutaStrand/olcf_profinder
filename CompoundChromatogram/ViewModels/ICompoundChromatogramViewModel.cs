namespace Agilent.OpenLab.CompoundChromatogram
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;

    #endregion

    /// <summary>
    /// ICompoundChromatogramViewModel
    /// </summary>
    public interface ICompoundChromatogramViewModel : IBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the toggle command A.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> ToggleCommandA { get; }

        /// <summary>
        /// Gets the list mode toggle command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> ListModeCommand { get; }

        /// <summary>
        /// Gets the sample group mode toggle command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> SampleGroupModeCommand { get; }

        /// <summary>
        /// Gets the overlay mode toggle command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> OverlayModeCommand { get; }

        /// <summary>
        /// Gets the sample group overlay toggle command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> GroupOverlayModeCommand { get; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> TriggerCommandB { get; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> ExportCommand { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        ICompoundChromatogramView View { get; }

        /// <summary>
        ///     Gets PlotControl.
        /// </summary>
        AgtPlotControl PlotControl { get; }
        #endregion

    }
}