namespace Agilent.OpenLab.SampleGrouping
{
    using Agilent.MassSpectrometry.DataAnalysis;
    using Agilent.MassSpectrometry.DataAnalysis.Utilities;
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using DataTypes;
    using Events;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
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
                List<DataTypes.ISample> result = new List<DataTypes.ISample>();

                foreach (Sample sample in this.Samples)
                {
                    result.Add(sample);
                }
                this.ExperimentContext.Samples = result;
                this.EventAggregator.GetEvent<SamplesAdded>().Publish(true);
            }
            
        }

        private void AddInputFilesHandler()
        {
            List<string> files = new List<string>();
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.AllowNonFileSystemItems = true;




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
                        var source = GetFilePolarity(file);
                        Samples.Add(new Sample
                        {
                            HideOrShow = true,
                            ExpOrder = i,
                            FileName = file,
                            Source = source,
                            SampleType = null,
                            Group = null
                        }) ;
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
                            var source = GetFilePolarity(filename);
                            Samples.Add(new Sample
                            {
                                HideOrShow = true,
                                ExpOrder = i,
                                FileName = filename,
                                Source = source,
                                SampleType = null,
                                Group = null
                            });


                        }
                            
                    }
                }
            }
            
            
            
        }

        private string GetFilePolarity(string sFileName)
        {
            bool bMultEC = false;
            bool bEIorCI = false;
            bool bAllIon = false;
            double lowEng = double.NaN;
            double[] hiEng = null;
            IBDAMSScanFileInformation msfi;
            var isGCEI = false;
            var rv = string.Empty;
            if (!Directory.Exists(sFileName))
                return rv;

                IDataAccess dataAccessor = new DataAccess() as IDataAccess;
                dataAccessor.OpenDataFile(sFileName);
                var bda = dataAccessor.BaseDataAccess;// new BDADataAccess() as IBDADataAccess;
                                                      //  bda.OpenDataFile(, true);
                msfi = bda.FileInformation.MSScanFileInformation;
                isGCEI = bda.IsGCEIData();
                //IDataAccess dataAccessor = bda as IDataAccess;
                bool bIsAllIons = FindCpdsUtilities.HasAllIonSupport(dataAccessor, out lowEng, out hiEng, out bMultEC, out bAllIon, out bEIorCI);
                
                //bda.CloseDataFile();
                dataAccessor.CloseDataFile();
            

            // Create a string like ESI+, ESI- or EI+
            // 1. identify the ion source
            switch (msfi.IonModes)
            {
                case IonizationMode.Unspecified:
                    // ChemStation translated .D files do not specify the ionization mode
                    if (isGCEI)
                        rv = "EI";
                    break;
                case IonizationMode.Esi:
                case IonizationMode.JetStream:
                case IonizationMode.MsChip:
                case IonizationMode.NanoEsi:
                    rv = "ESI";
                    break;
                case IonizationMode.Apci:
                case IonizationMode.Appi:
                case IonizationMode.CI:
                case IonizationMode.EI:
                case IonizationMode.ICP:
                case IonizationMode.Maldi:
                    rv = msfi.IonModes.ToString().ToUpperInvariant();
                    break;
            }

            // 2. +/- polarity
            switch (msfi.IonPolarity)
            {
                case IonPolarity.Positive:
                    rv += "+";
                    break;
                case IonPolarity.Negative:
                    rv += "-";
                    break;
                case IonPolarity.Mixed:
                    rv += "\u00B1"; // +/- character
                    break;
            }

            return rv;
        }

        #endregion
    }

    

}