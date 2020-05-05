namespace Agilent.OpenLab.CompoundSpectrum
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// CompoundSpectrumViewModel
    /// </summary>
    partial class CompoundSpectrumViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the toggle command A.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ToggleCommand<object> ToggleCommandA { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> TriggerCommandB { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> ExportCommand { get; private set; }

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

            this.TriggerCommandB = new TriggerCommand<object>(this.OnTestCommand)
            {
                Caption = "Trigger B",
                Hint = "Test Command Trigger B",
                KeyTip = "B"
            };

            this.ExportCommand = new TriggerCommand<object>(this.ExportData)
            {
                Caption = "Export Data",
                Hint = "Export Table data to a png file.",
                KeyTip = "E"
            };
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

        #endregion
    }
}