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
                List<ChromatogramGraphObject> chromatogramObjects = new List<ChromatogramGraphObject>();
                var samplesDict = obj.SampleWiseDataDictionary;
                int i = 0;
                foreach (string samplename in samplesDict.Keys)
                {
                    Color color = colorArray[i % colorArray.Length];

                    chromatogramObjects.Add(
                            this.CreateChromatogramGraphObject(
                                samplename, color, samplesDict[samplename]));



                    i++;
                }
                updatePlotControl(chromatogramObjects);
            }
           
            

            
        }

        private void updatePlotControl(List<ChromatogramGraphObject> msSpectrumGraphObjects)
        {
            this.PlotControl.RemoveAllItems();
            if (msSpectrumGraphObjects.Count <= 0) return;

            int numberOfRows = msSpectrumGraphObjects.Count;
            int numberOfColumns = 1;

            int numberOfRowsToShow = numberOfRows;
            if (numberOfRows > 3)
                numberOfRowsToShow = 3;

            this.PlotControl.Initialize(numberOfRows, numberOfColumns, numberOfRowsToShow, numberOfColumns);
            this.PlotControl.GraphManager.ShowLegend = true;
            this.PlotControl.GraphManager.ShowSignalHints = true;
            this.PlotControl.ControlSettings.PaneSelectionMode = PaneSelectionMode.None;

            var objectTransformation = new FullScaleRelativeObjectTransform();
            PlotControl.GraphManager.SetObjectTransform(objectTransformation);

            plotControl.GraphManager.LinkXAxis = true;
            plotControl.GraphManager.LinkYAxis = true;

            foreach (var paneManager in this.plotControl.GraphManager.PaneManagers())
            {
                this.InitializePaneProperties(paneManager);
            }

            IEnumerator<ChromatogramGraphObject> enumerator = msSpectrumGraphObjects.GetEnumerator();
            int horizontalCounter = 0;
            while (enumerator.MoveNext())
            {
                this.PlotControl.AddItem(horizontalCounter++, 0, enumerator.Current);

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