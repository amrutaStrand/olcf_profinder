namespace Agilent.OpenLab.CompoundChromatogram
{
    using Events;
    using DataTypes;
    using System.Drawing;
    using System.Collections.Generic;
    
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.Controls.GraphObjects;
    using Agilent.OpenLab.UI.DataStructures.Converters;


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

        Color[] colorArray = new Color[] {
                                            Color.Magenta,
                                            Color.Red,
                                            Color.Blue,
                                            Color.Green,
                                            Color.Cyan,
                                            Color.BlueViolet,
                                            Color.Black,
                                            Color.Chocolate,
                                            Color.Coral,
                                            Color.DarkSlateBlue,
                                            Color.AliceBlue,
                                            Color.Aqua,
                                            Color.Goldenrod,
                                            Color.Honeydew
                                            };

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

            List<ChromatogramGraphObject> chromatogramObjects = new List<ChromatogramGraphObject>();
            for (int i = 0; i < 10; i++)
            {
                Color color = colorArray[i % colorArray.Length];
                chromatogramObjects.Add(
                    this.CreateChromatogramGraphObject(
                        "Signal "+ (i+1), color, 0.0, 0.1, 1000, 4, 3, 5, 60, 200, 1));

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
        /// <param name="xmin">
        /// The xmin.
        /// </param>
        /// <param name="xstep">
        /// The xstep.
        /// </param>
        /// <param name="numberOfPoints">
        /// The number of points.
        /// </param>
        /// <param name="numberOfPeaks">
        /// The number of peaks.
        /// </param>
        /// <param name="minWidth">
        /// The min width.
        /// </param>
        /// <param name="maxWidth">
        /// The max width.
        /// </param>
        /// <param name="minHeight">
        /// The min height.
        /// </param>
        /// <param name="maxHeight">
        /// The max height.
        /// </param>
        /// <param name="maxNoise">
        /// The max noise.
        /// </param>
        /// <returns>
        /// The <see cref="ChromatogramGraphObject"/>.
        /// </returns>
        private ChromatogramGraphObject CreateChromatogramGraphObject(
            string signalName,
            Color color,
            double xmin,
            double xstep,
            int numberOfPoints,
            int numberOfPeaks,
            double minWidth,
            double maxWidth,
            double minHeight,
            double maxHeight,
            double maxNoise)
        {
            var chromatogramObject =
                new ChromatogramGraphObject(
                    AcamlSignalConverter.AcamlSignalToCommonSignal(
                        ChromatogramDataProvider.CreateChromatogramData(
                            xmin,
                            xstep,
                            numberOfPoints,
                            maxNoise,
                            ChromatogramDataProvider.CreatePeakDesctiptions(
                                numberOfPeaks,
                                minWidth,
                                maxWidth,
                                minHeight,
                                maxHeight,
                                xmin,
                                xmin + xstep * numberOfPoints))));
            chromatogramObject.DisplaySettings.Color = color;
            chromatogramObject.Hint = string.Format("Datapoints: {0}", numberOfPoints);
            chromatogramObject.HintTitle = signalName;
            chromatogramObject.CreateLegendObject(signalName);
            chromatogramObject.CreateLegendObject(new List<string> { signalName, chromatogramObject.Hint });

            return chromatogramObject;
        }



        #endregion
    }
}