namespace Agilent.OpenLab.ProfinderController
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Microsoft.Win32;
    using System.IO;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using System.Collections.Generic;
    using System;
    using Events;
    using Agilent.OpenLab.Framework.UI.Common.Services;

    #endregion

    /// <summary>
    /// UIModuleProjectTemplateViewModel
    /// </summary>
    partial class ProfinderControllerViewModel
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
        public TriggerCommand<object> ExperimentSetupCommand { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> FeatureExtractionCommand { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> StatisticAnalysisCommand { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> IdentificationCommand { get; private set; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TriggerCommand<object> ReportCommand { get; private set; }


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

            this.ExperimentSetupCommand = new TriggerCommand<object>(this.SelectFile)
            {
                Caption = "Experiment Setup",
                Hint = "Experiment Setup",
                KeyTip = "E"
            };
            this.FeatureExtractionCommand = new ToggleCommand<object>(this.runMFEWithBusyIndicator)
            {
                Caption = "Feature Extraction",
                Hint = "Feature Extraction",
                KeyTip = "F"
            };
            this.StatisticAnalysisCommand = new TriggerCommand<object>(this.OnTestCommand)
            {
                Caption = "Statistical Analysis",
                Hint = "Statistical Analysis",
                KeyTip = "S"
            };
            this.IdentificationCommand = new TriggerCommand<object>(this.OnTestCommand)
            {
                Caption = "Identification",
                Hint = "Identification",
                KeyTip = "I"
            };
            this.ReportCommand = new SelectorCommand<object>(this.OnTestCommand)
            {
                Caption = "Report",
                Hint = "Report",
                KeyTip = "R"
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

        

        /// <summary>
        /// Event handler for test command.
        /// </summary>
        /// <param name="unused">
        /// The unused. 
        /// </param>
        /// <remarks>
        /// </remarks>
        private void SelectFile(object unused)
        {
            List<string> files = new List<string>();
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;


            
            
            //ileDialog.Filter = "*.d";

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var iter = openFileDialog.FileNames.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (iter.Current.EndsWith(".d"))
                    {
                        files.Add(iter.Current);
                    }
                    else
                    {
                        throw new Exception("Selecter is not a \".d\" folder.");
                    }
                }
            }

            if (files.Count != 0 )
            {
                if (!files.Equals(FilePaths))
                {
                    FilePaths = files;
                }
            }

        }


        #endregion
    }
}