namespace Agilent.OpenLab.CompoundChromatogram
{
    using Agilent.OpenLab.UI.Controls.GraphObjects;
    using System.Collections.Generic;
    using System.Drawing;
    using Events;
    using DataTypes;
    using Utils;

    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.DataStructures.Converters;
    using System.Linq;
    using Agilent.OpenLab.CompoundChromatogram.ViewModels;


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
        
        private Color[] colorArray = ColorConstants.COLOR_ARRAY;
        private int NUM_ROWS_TO_SHOW = PlotConstants.NUM_OF_PANES_TO_SHOW;
        private int OVERLAY_OPACITY = PlotConstants.OVERLAY_OPACITY;

        private List<PlotItem> PlotItems;
        private Dictionary<string, Color> GroupColors;

        #endregion

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

        private Dictionary<string, Color> GetGroupColors()
        {
            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>(sampleGrouping.Values.Distinct());
            Dictionary<string, Color> groupColors = new Dictionary<string, Color>();
            for(int i=0; i<groups.Count; i++)
            {
                string group = groups[i] ?? "";
                groupColors[group] = colorArray[i % colorArray.Length];
            }
            return groupColors;
        }

        private void CompoundSelectionChanged(ICompoundGroup obj)
        {
            if (obj != null)
            {             
                IDictionary<string, ICompound> sampleWiseDataDictionary = obj.SampleWiseDataDictionary;
                Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
                GroupColors = GetGroupColors();
                PlotItems = new List<PlotItem>();
                foreach (string sampleName in sampleWiseDataDictionary.Keys)
                    PlotItems.Add(new PlotItem(sampleName, sampleGrouping[sampleName], sampleWiseDataDictionary[sampleName]));

                UpdatePlotControl();
            }           
        }

        private void UpdatePlotItems()
        {
            List<string> groups = PlotItems.Select(p => p.Group).Distinct().ToList();
            for (int i=0; i<PlotItems.Count; i++)
            {
                PlotItem plotItem = PlotItems[i];
                string group = plotItem.Group ?? "";
                int groupIndex = groups.IndexOf(group);
                plotItem.Legend = null;
                plotItem.Color = colorArray[i];
                if (ColorBySampleGroupFlag)                    
                    plotItem.Color = GroupColors[group];

                plotItem.HorizontalPosition = i;
                if (DisplayMode.Equals("Overlay"))
                {
                    plotItem.HorizontalPosition = 0;
                    Color color = plotItem.Color;
                    plotItem.Color = Color.FromArgb(OVERLAY_OPACITY, color.R, color.G, color.B);
                }
                if (DisplayMode.Equals("GroupOverlay"))
                {
                    plotItem.HorizontalPosition = groupIndex;
                    Color color = plotItem.Color;
                    plotItem.Color = Color.FromArgb(OVERLAY_OPACITY, color.R, color.G, color.B);
                    if (ColorBySampleGroupFlag)
                        plotItem.Legend = group;
                }
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

        private int GetNumberOfPanes()
        {
            if (DisplayMode.Equals("List"))
                return PlotItems.Count;
            if (DisplayMode.Equals("GroupOverlay"))
                return PlotItems.Select(p => p.Group).Distinct().Count();
            return 1;
        }

        private void UpdatePlotControl()
        {
            UpdatePlotItems();
            int num_panes = GetNumberOfPanes();
            InitializePlotControl(num_panes, 1);
            foreach(PlotItem plotItem in PlotItems)
            {
                ChromatogramGraphObject chromatogramGraphObject = CreateChromatogramGraphObject(
                            plotItem.Legend, plotItem.Color, plotItem.Compound);

                this.PlotControl.AddItem(plotItem.HorizontalPosition, 0, chromatogramGraphObject);
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
            //if (signalName != null)
            //    chromatogramObject.CreateLegendObject(signalName);
            //else
                chromatogramObject.CreateLegendObject(new List<string> { chromatogramObject.HintTitle});

            return chromatogramObject;
        }



        #endregion
    }
}