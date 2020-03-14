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

        private void UpdatePlotControlInSampleGroupMode()
        {
        }

        private void UpdatePlotControlInListMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;
            
            List<ChromatogramGraphObject> chromatogramObjects = new List<ChromatogramGraphObject>();

            int i = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                Color color = colorArray[i % colorArray.Length];

                chromatogramObjects.Add(
                        this.CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]));

                i++;
            }

            int numberOfRows = chromatogramObjects.Count;
            int numberOfColumns = 1;

            int numberOfRowsToShow = numberOfRows;
            if (numberOfRows > 3)
                numberOfRowsToShow = 3;

            this.PlotControl.RemoveAllItems();
            this.PlotControl.Initialize(numberOfRows, numberOfColumns, numberOfRowsToShow, numberOfColumns);

            UpdatePlotProperties();

            IEnumerator<ChromatogramGraphObject> enumerator = chromatogramObjects.GetEnumerator();
            int horizontalCounter = 0;
            while (enumerator.MoveNext())
            {
                this.PlotControl.AddItem(horizontalCounter++, 0, enumerator.Current);
            }
        }

        private void UpdatePlotControlInOverlayMode()
        {
            if (sampleWiseDataDictionary == null || sampleWiseDataDictionary.Count == 0)
                return;

            
            List<ChromatogramGraphObject> chromatogramObjects = new List<ChromatogramGraphObject>();

            int i = 0;
            foreach (string samplename in sampleWiseDataDictionary.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                color = Color.FromArgb(10, color.R, color.G, color.B);

                chromatogramObjects.Add(
                        this.CreateChromatogramGraphObject(
                            samplename, color, sampleWiseDataDictionary[samplename]));

                i++;
            }

            this.PlotControl.RemoveAllItems();
            this.PlotControl.Initialize(1, 1);

            UpdatePlotProperties();

            IEnumerator<ChromatogramGraphObject> enumerator = chromatogramObjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this.PlotControl.AddItem(0, 0, enumerator.Current);
            }
        }

        private void UpdatePlotProperties()
        {
            this.PlotControl.GraphManager.ShowLegend = true;
            this.PlotControl.GraphManager.ShowSignalHints = true;
            this.PlotControl.ControlSettings.PaneSelectionMode = PaneSelectionMode.None;

            plotControl.GraphManager.LinkXAxis = true;

            foreach (var paneManager in this.plotControl.GraphManager.PaneManagers())
            {
                this.InitializePaneProperties(paneManager);
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