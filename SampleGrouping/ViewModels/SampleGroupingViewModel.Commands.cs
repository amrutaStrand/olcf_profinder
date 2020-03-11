namespace Agilent.OpenLab.SampleGrouping
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using DataTypes;
    using Events;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// SampleGroupingViewModel
    /// </summary>
    partial class SampleGroupingViewModel
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



        private ICommand addInputFiles;
        /// <summary>
        /// adds new files to the table.
        /// </summary>
        public ICommand AddInputFiles
        {
            get
            {
                return addInputFiles ?? (addInputFiles = new CommandHandler(() => AddInputFilesHandler(), () => CanExecute));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        private ICommand sampleGroupingsCompleted;


        /// <summary>
        /// 
        /// </summary>
        public ICommand SampleGroupingCompleted
        {
            get
            {
                return sampleGroupingsCompleted ?? (sampleGroupingsCompleted = new CommandHandler(() => SamplesGroupingCompletedHandler(), () => IsSamplesAdded));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSamplesAdded
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

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

        private void SamplesGroupingCompletedHandler()
        {
            
            if (this.Samples.Count != 0)
            {
                List<ISample> result = new List<ISample>();

                foreach (Sample sample in this.Samples)
                {
                    result.Add(sample);
                }
                this.EventAggregator.GetEvent<SamplesAdded>().Publish(result);
            }
            
        }

        private void AddInputFilesHandler()
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

            if (files.Count != 0)
            {
                if(FilePaths.Count ==0)
                {
                    int i = FilePaths.Count + 1;
                    FilePaths = files;
                    
                    foreach (string file in files)
                    {
                        Samples.Add(new Sample
                        {
                            ExpOrder = i ,
                            FileName = file,
                            SampleType = "SampleType" + i,
                            Group = null
                        });
                        i++;

                    }
                }
                else
                {
                    int i = FilePaths.Count + 1;
                    foreach (string filename in files)
                    {
                        if (!FilePaths.Contains(filename))
                        {
                            FilePaths.Add(filename);
                            Samples.Add(new Sample
                            {
                                ExpOrder = i,
                                FileName = filename,
                                SampleType = "SampleType" + i,
                                Group = null
                            });


                        }
                            
                    }
                }
            }
            
            
            
        }

        #endregion
    }

    

}