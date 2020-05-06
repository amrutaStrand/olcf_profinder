namespace Agilent.OpenLab.CompoundSpectrum
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using DataTypes;


    #endregion

    /// <summary>
    /// ICompoundSpectrumViewModel
    /// </summary>
    public interface ICompoundSpectrumViewModel : IBaseViewModel
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
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> ExportCommand { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        ICompoundSpectrumView View { get; }

        /// <summary>
        /// 
        /// </summary>
        AgtPlotControl PlotControl { get; }

        /// <summary>
        /// 
        /// </summary>
        void ExportData();

        #endregion
    }
}