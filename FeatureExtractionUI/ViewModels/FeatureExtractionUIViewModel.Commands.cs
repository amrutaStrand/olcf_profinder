namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Events;
    using System;
    using System.Windows;

    #endregion

    /// <summary>
    /// FeatureExtractionUIViewModel
    /// </summary>
    partial class FeatureExtractionUIViewModel
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

        public TriggerCommand<object> RunMFECommand { get; private set; }


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

            this.RunMFECommand = new TriggerCommand<object>(this.OnRunMFECommand, this.CanRunMFECommand);
        }

        private bool CanRunMFECommand(object arg)
        {
            //TODO
            //check flag - Mfe not already running
            //check flag - application is in correct state
            return true;
        }


        ///<summary>
        ///Event handler for Running MFE
        ///</summary>
        private void OnRunMFECommand(object unused)
        {

            //public event to update/save inputs

            //Validate inputs
            string invalidErrorMsg = this.ValidateAllInputs();
            if (invalidErrorMsg != null && invalidErrorMsg.Length > 0)
            {
                MessageBox.Show(invalidErrorMsg);
                return;
            }

            //publish event to run mfe
            this.ExperimentContext.MFEInputParameters = this.AllInputsParameters;
            this.EventAggregator.GetEvent<RunMFEInitiated>().Publish(true);
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

        #endregion
    }
}