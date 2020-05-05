namespace Agilent.OpenLab.CompoundChromatogram
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Events;
    using System.Windows.Forms;

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
        public TriggerCommand<object> ExportCommand { get; private set; }

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
                KeyTip = "L",
                IsChecked = true
            };

            this.SampleGroupModeCommand = new ToggleCommand<object>(this.HandleColorBySampleGroupFlag)
            {
                Caption = "Color by Sample Group",
                Hint = "Activate the color by sample group mode on the plots.",
                KeyTip = "G",
                IsChecked = false
            };

            this.OverlayModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "Overlay Mode",
                Hint = "Activate the overlay mode on the plots.",
                KeyTip = "O"
            };

            this.GroupOverlayModeCommand = new ToggleCommand<object>(this.ActivateMode)
            {
                Caption = "Sample Group Overlay Mode",
                Hint = "Activate the sample group overlay mode on the plots.",
                KeyTip = "O"
            };

            this.ExportCommand = new TriggerCommand<object>(this.ExportData)
            {
                Caption = "Export Data",
                Hint = "Export Table data to a png file.",
                KeyTip = "E"
            };

            this.TriggerCommandB = new TriggerCommand<object>(this.OnTestCommand)
            {
                Caption = "Trigger B",
                Hint = "Test Command Trigger B",
                KeyTip = "B"
            };
            DisplayMode = "List";
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

        private void ExportData(object unused)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ExportToPng(filePath);
            }

        }

        /// <summary>
        /// Event handler for mode change commands.
        /// </summary>
        /// <param name="unused">
        /// The command parameter. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void HandleColorBySampleGroupFlag(object unused)
        {
            ColorBySampleGroupFlag = SampleGroupModeCommand.IsChecked;
            UpdatePlotControl();
            EventAggregator.GetEvent<ColorBySampleGroupFlagChanged>().Publish(ColorBySampleGroupFlag);
        }

        private void HandleModeCommands()
        {
            if (DisplayMode.Equals("Overlay") && OverlayModeCommand.IsChecked)
            {
                ListModeCommand.IsChecked = false;
                GroupOverlayModeCommand.IsChecked = false;
            }
            else if (DisplayMode.Equals("GroupOverlay") && GroupOverlayModeCommand.IsChecked)
            {
                ListModeCommand.IsChecked = false;
                OverlayModeCommand.IsChecked = false;
            }
            else
            {
                ListModeCommand.IsChecked = true;
                OverlayModeCommand.IsChecked = false;
                GroupOverlayModeCommand.IsChecked = false;
            }
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

            DisplayMode = mode;
            HandleModeCommands();
            UpdatePlotControl();
            EventAggregator.GetEvent<PlotDisplayModeChanged>().Publish(mode);   
        }

        #endregion
    }
}