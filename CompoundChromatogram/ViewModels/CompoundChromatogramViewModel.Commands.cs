namespace Agilent.OpenLab.CompoundChromatogram
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Events;

    #endregion

    /// <summary>
    /// CompoundChromatogramViewModel
    /// </summary>
    partial class CompoundChromatogramViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the toggle command A.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> ToggleCommandA { get; private set; }

        /// <summary>
        /// Gets the list mode command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> ListModeCommand { get; private set; }

        /// <summary>
        /// Gets the sample group mode command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> SampleGroupModeCommand { get; private set; }

        /// <summary>
        /// Gets the overlay mode command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> OverlayModeCommand { get; private set; }

        /// <summary>
        /// Gets the sample group overlay mode command.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> GroupOverlayModeCommand { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> TriggerCommandB { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void InitializeCommands()
        {
            this.ToggleCommandA = new ToggleCommand<object>(this.OnTestCommand)
            {
                Caption = "Toggle A",
                Hint = "Test Command Toggle A",
                KeyTip = "A"
            };

            this.ListModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "List Mode",
                Hint = "Activate the list mode on the plots.",
                KeyTip = "L"
            };

            this.SampleGroupModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "Sample Group Mode",
                Hint = "Activate the color by sample group mode on the plots.",
                KeyTip = "G"
            };

            this.OverlayModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "Overlay Mode",
                Hint = "Activate the overlay mode on the plots.",
                KeyTip = "O"
            };

            this.GroupOverlayModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "Overlay Mode",
                Hint = "Activate the sample group overlay mode on the plots.",
                KeyTip = "O"
            };

            this.TriggerCommandB = new TriggerCommand<object>(this.OnTestCommand)
            {
                Caption = "Trigger B",
                Hint = "Test Command Trigger B",
                KeyTip = "B"
            };

            this.ListModeCommand.IsChecked = true;
        }

        /// <summary>
        /// Event handler for test command.
        /// </summary>
        /// <param name="unused">
        /// The unused. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void OnTestCommand(object unused)
        {
        }

        /// <summary>
        /// Event handler for mode change commands.
        /// </summary>
        /// <param name="param">
        /// The command parameter. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void ActivateMode(object param)
        {
            string mode = (string)param;
            if (mode.Equals("Overlay") && OverlayModeCommand.IsChecked)
            {
                UpdatePlotControlInOverlayMode();
                ListModeCommand.IsChecked = false;
                SampleGroupModeCommand.IsChecked = false;
                EventAggregator.GetEvent<PlotDisplayModeChanged>().Publish("Overlay");
            }
            else if (mode.Equals("Group") && SampleGroupModeCommand.IsChecked)
            {
                UpdatePlotControlInSampleGroupMode();
                ListModeCommand.IsChecked = false;
                OverlayModeCommand.IsChecked = false;
                EventAggregator.GetEvent<PlotDisplayModeChanged>().Publish("Group");
            }
            else
            {
                UpdatePlotControlInListMode();
                ListModeCommand.IsChecked = true;
                SampleGroupModeCommand.IsChecked = false;
                OverlayModeCommand.IsChecked = false;
                EventAggregator.GetEvent<PlotDisplayModeChanged>().Publish("List");
            }
        }

        #endregion
    }
}