namespace Agilent.OpenLab.CompoundSpectrum
{

    using DataTypes;
    using Events;
    using Utils;

    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;


    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;
    using Agilent.OpenLab.UI.Controls.GraphObjects;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.ObjectTransformation;
    using Agilent.OpenLab.CompoundSpectrum.ViewModels;

    /// <summary>
    /// CompoundSpectrumViewModel
    /// </summary>
    partial class CompoundSpectrumViewModel
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
            this.EventAggregator.GetEvent<PlotDisplayModeChanged>().Subscribe(this.ActivateMode);
            this.EventAggregator.GetEvent<ColorBySampleGroupFlagChanged>().Subscribe(this.HandleColorBySampleGroupFlag);
        }

        private void HandleColorBySampleGroupFlag(bool isChecked)
        {
            ColorBySampleGroupFlag = isChecked;
            UpdatePlotControl();
        }

        private void ActivateMode(string mode)
        {
            DisplayMode = mode;
            UpdatePlotControl();
        }

        private Dictionary<string, Color> GetGroupColors()
        {
            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>(sampleGrouping.Values.Distinct());
            Dictionary<string, Color> groupColors = new Dictionary<string, Color>();
            for (int i = 0; i < groups.Count; i++)
            {
                string group = groups[i] ?? "";
                groupColors[group] = colorArray[i % colorArray.Length];
            }
            return groupColors;
        }

        private void CompoundSelectionChanged(ICompoundGroup compoundGroupObject)
        {
            if (compoundGroupObject == null) return;

            IDictionary<string, ICompound> sampleWiseDataDictionary = compoundGroupObject.SampleWiseDataDictionary;
            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            GroupColors = GetGroupColors();
            PlotItems = new List<PlotItem>();
            foreach (string sampleName in sampleWiseDataDictionary.Keys)
                PlotItems.Add(new PlotItem(sampleName, sampleGrouping[sampleName], sampleWiseDataDictionary[sampleName]));

            DisplayMode = "List";
            UpdatePlotControl();
        }

        private void UpdatePlotItems()
        {
            List<string> groups = PlotItems.Select(p => p.Group).Distinct().ToList();
            for (int i = 0; i < PlotItems.Count; i++)
            {
                PlotItem plotItem = PlotItems[i];
                string group = "";
                int groupIndex = 0;
                if (plotItem.Group != null)
                {
                    group = plotItem.Group;
                    groupIndex = groups.IndexOf(group);
                }
                plotItem.Color = colorArray[i];
                plotItem.Legend = null;
                if (ColorBySampleGroupFlag)
                    plotItem.Color = GroupColors[group];

                plotItem.HorizontalPosition = i;
                if (DisplayMode.Equals("Overlay"))
                    plotItem.HorizontalPosition = 0;
                if (DisplayMode.Equals("GroupOverlay"))
                {
                    plotItem.HorizontalPosition = groupIndex;
                    if (ColorBySampleGroupFlag)
                        plotItem.Legend = group;
                }
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
            foreach (PlotItem plotItem in PlotItems)
            {
                MsSpectrumGraphObject msSpectrumGraphObject = GetMsSpectrumGraphObject(plotItem.Compound, plotItem.Color, plotItem.Legend);
                this.PlotControl.AddItem(plotItem.HorizontalPosition, 0, msSpectrumGraphObject);
            }
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

        /// <summary>
        /// The get mass of most abundant ion.
        /// </summary>
        /// <param name="spectrumData">
        /// The spectrum data.
        /// </param>       
        public static double? GetMassOfMostAbundantIon(ISpectrumData spectrumData)
        {
            var maxY = spectrumData.Data.YValues.Max();
            for (var i = 0; i < spectrumData.Data.YValues.Length; ++i)
            {
                if (spectrumData.Data.YValues[i] == maxY)
                {
                    return spectrumData.Data.XValues[i];
                }
            }

            return null;
        }


        static MsSpectrumGraphObject GetMsSpectrumGraphObject(ICompound compound, Color color, string legend)
        {
            string spectrumName = compound.FileName;
            SpectrumData spectrumData = getSpectrumData(compound);
            var spectrumGraphObject = new MsSpectrumGraphObject(spectrumData);

            spectrumGraphObject.DisplaySettings.Color = color;

            //if (legend != null)
            //    spectrumGraphObject.CreateLegendObject(legend);     
            //else
                spectrumGraphObject.CreateLegendObject(new List<string>{ spectrumData.Name });

            spectrumGraphObject.MassOfPrecursorIon = GetMassOfMostAbundantIon(spectrumData);
            spectrumGraphObject.DisplaySettings.ShowPrecursorIonAnnotation = false;
            spectrumGraphObject.DisplaySettings.PenSizeFocusedIon = 2f;
            spectrumGraphObject.DisplaySettings.PenSizeDefaultIon = 1f;

            // spectrumGraphObject.DisplaySettings.DefaultIonAnnotationFont = GraphicToolsRepository.FontArial10;
            // spectrumGraphObject.DisplaySettings.FocusedIonAnnotationFont = GraphicToolsRepository.FontArial12Bold;
            spectrumGraphObject.DisplaySettings.IonAnnotationFormatString = "{0:0.0000}";
            spectrumGraphObject.DisplaySettings.IonAnnotationLabelingMode = IonAnnotationLabelingMode.Always;
            spectrumGraphObject.DisplaySettings.IonAnnotationLabelsMayOverlapIons = true;
            spectrumGraphObject.DisplaySettings.MaximumNumberOfVisibleIonAnnotationLabels = 20;
            return spectrumGraphObject;

        }

        static SpectrumData getSpectrumData(ICompound compound)
        {
            ICollection<IPeak> peaks = compound.Spectrum.Peaks;
            int count = peaks.Count;

            double[] xValues = new double[count];
            double[] yValues = new double[count];

            IEnumerator<IPeak> pe = peaks.GetEnumerator();
            int counter = 0;
            while (pe.MoveNext())
            {
                IPeak peak = pe.Current;
                xValues[counter] = peak.X;
                yValues[counter] = peak.Y;
                counter++;
            }

            SpectrumData spectrumData =  new SpectrumData(new Data(xValues, yValues), XUnit.MassToCharge, "Counts", 13.56);
            spectrumData.Name = compound.Spectrum.Name;
            return spectrumData;

        }
        #endregion
    }
}