namespace Agilent.OpenLab.SampleGrouping
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;
    using DataTypes;
    using System.ComponentModel;

    #endregion

    /// <summary>
    /// ISampleGroupingViewModel
    /// </summary>
    public interface ISampleGroupingViewModel : IBaseViewModel
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
        ISampleGroupingView View { get; }


        /// <summary>
        /// Gets the sample groups list.
        /// </summary>
        BindingList<ISample> Samples { get; }

        /// <summary>
        /// Gets the selected sample groups list.
        /// </summary>
        BindingList<ISample> SelectedSamples { get; set; }

        /// <summary>
        ///     Gets or sets the focused sample.
        /// </summary>
        ISample FocusedSample { get; set; }
        #endregion
    }
}