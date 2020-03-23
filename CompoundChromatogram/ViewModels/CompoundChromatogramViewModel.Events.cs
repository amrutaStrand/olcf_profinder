namespace Agilent.OpenLab.CompoundChromatogram
{

    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Interfaces;
    using Agilent.OpenLab.UI.Controls.GraphObjects;
    using Agilent.OpenLab.UI.DataStructures.Signals.Interfaces;
    using System.Collections.Generic;
    using System.Drawing;
    using Events;
    using DataTypes;
    using Utils;

    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.DataStructures.Converters;
    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.ObjectTransformation;


    /// <summary>
    /// CompoundChromatogramViewModel
    /// </summary>
    partial class CompoundChromatogramViewModel
    {
        #region Constants and Fields

        /// <summary>
        /// parameters used to track unhandled events
        /// </summary>
        private bool unhandledSomethingEventMonitored;
        private IDictionary<string, ICompound> sampleWiseDataDictionary;
        private int NUM_ROWS_TO_SHOW = PlotConstants.NUM_OF_PANES_TO_SHOW;

        #endregion

        Color[] colorArray = ColorConstants.COLOR_ARRAY;

        #region Methods

        private void DoSomething()
        {
        }

        private void OnSomethingHappenedEvent(object sender)
        {
            if (!this.IsActive)
            {
                this.unhandledSomethingEventMonitored = true;
                return;
            }

            this.DoSomething();
        }

        /// <summary>
        /// Subscribes the events.
        /// </summary>
        /// <remarks>
        /// Subscribe to any events the module has to react on.
        /// </remarks>
        private void SubscribeEvents()
        {
            // This might look like the following line of code:
            this.EventAggregator.GetEvent<CompoundSelectionChanged>().Subscribe(this.CompoundSelectionChanged);
        }

        /// <summary>
        /// Unsubscribes the events.
        /// </summary>
        /// <remarks>
        /// Unsubscribe any events previously subscribed.
        /// </remarks>
        private void UnsubscribeEvents()
        {
            // This might look like the following line of code:
            this.EventAggregator.GetEvent<CompoundSelectionChanged>().Unsubscribe(this.CompoundSelectionChanged);

        }
        private void CompoundSelectionChanged(ICompoundGroup obj)
        {
            this.PlotControl.RemoveAllItems();
            if (obj != null)
            {
                
                sampleWiseDataDictionary = obj.SampleWiseDataDictionary;
                
                UpdatePlotControlInListMode();
            }           
        }

        private void InitializePlotControl(int numberOfRows = 1, int numberOfColumns = 1)
        {
            this.PlotControl.RemoveAllItems();

            int numberOfRowsToShow = numberOfRows;
            if (numberOfRows > NUM_ROWS_TO_SHOW)
                numberOfRowsToShow = NUM_ROWS_TO_SHOW;

            this.PlotControl.Initialize(numberOfRows, numberOfColumns, numberOfRowsToShow, numberOfColumns);

            // Initialize Properties
            this.PlotControl.GraphManager.ShowLegend = true;
            this.PlotControl.GraphManager.ShowSignalHints = true;
            this.PlotControl.ControlSettings.PaneSelectionMode = PaneSelectionMode.None;
            plotControl.GraphManager.LinkXAxis = true;
            plotControl.GraphManager.LinkYAxis = true;

            foreach (var paneManager in this.plotControl.GraphManager.PaneManagers())
            {
                this.InitializePaneProperties(paneManager);
            }
        }

        private void UpdatePlotControlInListMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;

            InitializePlotControl(sampleWiseDataDictionary.Count, 1);

            int i = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                ChromatogramGraphObject chromatogramGraphObject = CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]);

                this.PlotControl.AddItem(i++, 0, chromatogramGraphObject);
            }
        }

        private void UpdatePlotControlInOverlayMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;

            InitializePlotControl(1, 1);

            int i = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                color = Color.FromArgb(10, color.R, color.G, color.B);
                ChromatogramGraphObject chromatogramGraphObject = CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]);

                this.PlotControl.AddItem(0, 0, chromatogramGraphObject);
                i++;
            }
        }

        private void UpdatePlotControlInGroupOverlayMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;

            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>();

            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                string group = sampleGrouping[samplename];
                if (!groups.Contains(group))
                    groups.Add(group);
            }

            InitializePlotControl(groups.Count, 1);

            int i = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                color = Color.FromArgb(10, color.R, color.G, color.B);
                ChromatogramGraphObject chromatogramGraphObject = CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]);

                string group = sampleGrouping[samplename];
                this.PlotControl.AddItem(groups.IndexOf(group), 0, chromatogramGraphObject);
                i++;
            }

        }

        private void UpdatePlotControlInSampleGroupMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;

            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>();

            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                string group = sampleGrouping[samplename];
                if (!groups.Contains(group))
                    groups.Add(group);
            }

            InitializePlotControl(sampleWiseDataDictionary.Count, 1);

            int horizontalCounter = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                string group = sampleGrouping[samplename];
                int index = groups.IndexOf(group);
                Color color = colorArray[index % colorArray.Length];

                ChromatogramGraphObject chromatogramGraphObject = CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]);

                this.PlotControl.AddItem(horizontalCounter++, 0, chromatogramGraphObject);
            }
        }


        /// <summary>
        /// The create chromatogram graph object.
        /// </summary>
        /// <param name="signalName">
        /// The signal name.
        /// </param>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="compound"></param>
        /// <returns>
        /// The <see cref="ChromatogramGraphObject"/>.
        /// </returns>
        private ChromatogramGraphObject CreateChromatogramGraphObject(
            string signalName,
            Color color,
            ICompound compound)
        {
            var chromatogramObject =
                new FilledChromatogramGraphObject(
                    AcamlSignalConverter.AcamlSignalToCommonSignal(
                        ChromatogramDataProvider.CreateChromatogramData(
                          compound.Chromatogram
                           )));
            chromatogramObject.DisplaySettings.Color = color;
            chromatogramObject.Hint = string.Format("Datapoints: {0}", compound.Chromatogram.Data.Count);
            chromatogramObject.HintTitle = compound.Chromatogram.Title;
            chromatogramObject.CreateLegendObject(signalName);
            chromatogramObject.CreateLegendObject(new List<string> { chromatogramObject.HintTitle
                //,signalName, chromatogramObject.Hint
            });

            return chromatogramObject;
        }



        #endregion
    }
}