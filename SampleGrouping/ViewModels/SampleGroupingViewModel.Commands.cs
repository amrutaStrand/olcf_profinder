namespace Agilent.OpenLab.SampleGrouping
{
    using Agilent.MassSpectrometry.DataAnalysis;
    using Agilent.MassSpectrometry.DataAnalysis.Utilities;
    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using DataTypes;
    using Events;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
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
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Selected", typeof(bool));
                    dataTable.Columns.Add("Exp. Order", typeof(int));
                    dataTable.Columns.Add("File Name", typeof(string));
                    dataTable.Columns.Add("Source", typeof(string));
                    dataTable.Columns.Add("Sample Type", typeof(string));
                    dataTable.Columns.Add("Group", typeof(string));
                    
                    foreach (string file in files)
                    {
                        var source = GetFilePolarity(file);
                        dataTable.Rows.Add(true, i, file, source, null, null);
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
                        this.SampleDataTable = dataTable;
                    }
                }
                else
                {
                    int i = FilePaths.Count + 1;
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Selected", typeof(bool));
                    dataTable.Columns.Add("Exp. Order", typeof(int));
                    dataTable.Columns.Add("File Name", typeof(string));
                    dataTable.Columns.Add("Source", typeof(string));
                    dataTable.Columns.Add("Sample Type", typeof(string));
                    dataTable.Columns.Add("Group", typeof(string));

                    foreach (string filename in files)
                    {
                        if (!FilePaths.Contains(filename))
                        {
                            FilePaths.Add(filename);
                            var source = GetFilePolarity(filename);
                            dataTable.Rows.Add(true, i, filename, source, null, null);
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
                    this.SampleDataTable = dataTable;

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
                case MassSpectrometry.DataAnalysis.IonizationMode.Unspecified:
                    // ChemStation translated .D files do not specify the ionization mode
                    if (isGCEI)
                        rv = "EI";
                    break;
                case MassSpectrometry.DataAnalysis.IonizationMode.Esi:
                case MassSpectrometry.DataAnalysis.IonizationMode.JetStream:
                case MassSpectrometry.DataAnalysis.IonizationMode.MsChip:
                case MassSpectrometry.DataAnalysis.IonizationMode.NanoEsi:
                    rv = "ESI";
                    break;
                case MassSpectrometry.DataAnalysis.IonizationMode.Apci:
                case MassSpectrometry.DataAnalysis.IonizationMode.Appi:
                case MassSpectrometry.DataAnalysis.IonizationMode.CI:
                case MassSpectrometry.DataAnalysis.IonizationMode.EI:
                case MassSpectrometry.DataAnalysis.IonizationMode.ICP:
                case MassSpectrometry.DataAnalysis.IonizationMode.Maldi:
                    rv = msfi.IonModes.ToString().ToUpperInvariant();
                    break;
            }

            // 2. +/- polarity
            switch (msfi.IonPolarity)
            {
                case MassSpectrometry.DataAnalysis.IonPolarity.Positive:
                    rv += "+";
                    break;
                case MassSpectrometry.DataAnalysis.IonPolarity.Negative:
                    rv += "-";
                    break;
                case MassSpectrometry.DataAnalysis.IonPolarity.Mixed:
                    rv += "\u00B1"; // +/- character
                    break;
            }

            return rv;
        }

        #endregion
    }

    

}