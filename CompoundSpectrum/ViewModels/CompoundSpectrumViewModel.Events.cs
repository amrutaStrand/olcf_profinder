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
        Color[] colorArray = ColorConstants.COLOR_ARRAY;
        IDictionary<string, ICompound> sampleWiseCompounds;
        private int NUM_ROWS_TO_SHOW = PlotConstants.NUM_OF_PANES_TO_SHOW;

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

        }

        private void ActivateMode(string mode)
        {
            if (mode.Equals("Overlay"))
                UpdatePlotControlInOverlayMode();
            else if (mode.Equals("Group"))
                UpdatePlotControlInSampleGroupMode();
            else if (mode.Equals("GroupOverlay"))
                UpdatePlotControlInGroupOverlayMode();
            else
                UpdatePlotControlInListMode();
        }

        private void CompoundSelectionChanged(ICompoundGroup compoundGroupObject)
        {
            this.PlotControl.RemoveAllItems();
            if (compoundGroupObject == null) return;

            sampleWiseCompounds = compoundGroupObject.SampleWiseDataDictionary;
            UpdatePlotControlInListMode();
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

            foreach (var paneManager in this.plotControl.GraphManager.PaneManagers())
            {
                this.InitializePaneProperties(paneManager);
            }
        }

        private void UpdatePlotControlInListMode()
        {
            if (sampleWiseCompounds == null || sampleWiseCompounds.Count == 0)
                return;

            InitializePlotControl(sampleWiseCompounds.Count, 1);

            int i = 0;
            foreach (string samplename in sampleWiseCompounds.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                MsSpectrumGraphObject msSpectrumGraphObject = GetMsSpectrumGraphObject(sampleWiseCompounds[samplename], color);

                this.PlotControl.AddItem(i++, 0, msSpectrumGraphObject);
            }
        }

        private void UpdatePlotControlInOverlayMode()
        {
            if (sampleWiseCompounds == null || sampleWiseCompounds.Count == 0)
                return;

            InitializePlotControl(1, 1);

            int i = 0;
            foreach (string samplename in sampleWiseCompounds.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                MsSpectrumGraphObject msSpectrumGraphObject = GetMsSpectrumGraphObject(sampleWiseCompounds[samplename], color);

                this.PlotControl.AddItem(0, 0, msSpectrumGraphObject);
                i++;
            }
        }

        private void UpdatePlotControlInGroupOverlayMode()
        {
            if (sampleWiseCompounds == null || sampleWiseCompounds.Count == 0)
                return;

            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>();

            foreach(string samplename in sampleWiseCompounds.Keys)
            {
                string group = sampleGrouping[samplename];
                if (!groups.Contains(group))
                    groups.Add(group);
            }

            InitializePlotControl(groups.Count, 1);

            int i = 0;
            foreach (string samplename in sampleWiseCompounds.Keys)
            {
                Color color = colorArray[i % colorArray.Length];
                MsSpectrumGraphObject msSpectrumGraphObject = GetMsSpectrumGraphObject(sampleWiseCompounds[samplename], color);

                string group = sampleGrouping[samplename];
                this.PlotControl.AddItem(groups.IndexOf(group), 0, msSpectrumGraphObject);
                i++;
            }      

        }

        private void UpdatePlotControlInSampleGroupMode()
        {
            if (sampleWiseCompounds == null || sampleWiseCompounds.Count == 0)
                return;

            Dictionary<string, string> sampleGrouping = ExperimentContext.GetGrouping();
            List<string> groups = new List<string>();

            foreach (string samplename in sampleWiseCompounds.Keys)
            {
                string group = sampleGrouping[samplename];
                if (!groups.Contains(group))
                    groups.Add(group);
            }

            InitializePlotControl(sampleWiseCompounds.Count, 1);

            int horizontalCounter = 0;
            foreach (string samplename in sampleWiseCompounds.Keys)
            {
                string group = sampleGrouping[samplename];
                int index = groups.IndexOf(group);
                Color color = colorArray[index % colorArray.Length];

                MsSpectrumGraphObject msSpectrumGraphObject = GetMsSpectrumGraphObject(sampleWiseCompounds[samplename], color);

                this.PlotControl.AddItem(horizontalCounter++, 0, msSpectrumGraphObject);
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


        static MsSpectrumGraphObject GetMsSpectrumGraphObject(ICompound compound, Color color)
        {
            string spectrumName = compound.FileName;
            SpectrumData spectrumData = getSpectrumData(compound);
            var spectrumGraphObject = new MsSpectrumGraphObject(spectrumData);

            spectrumGraphObject.DisplaySettings.Color = color;
            spectrumGraphObject.CreateLegendObject(
                new List<string>
                    {
                        spectrumData.Name
                        //,string.Format("{0};  #Datapoints = {1}", spectrumName, spectrumData.Data.XValues.Length),
                        //string.Format("Max. Abundance: {0:0}", spectrumData.Data.YValues.Max()),
                    });
            spectrumGraphObject.MassOfPrecursorIon = GetMassOfMostAbundantIon(spectrumData);
            spectrumGraphObject.DisplaySettings.ShowPrecursorIonAnnotation = true;
            spectrumGraphObject.DisplaySettings.PenSizeFocusedIon = 2f;
            spectrumGraphObject.DisplaySettings.PenSizeDefaultIon = 1f;

            // spectrumGraphObject.DisplaySettings.DefaultIonAnnotationFont = GraphicToolsRepository.FontArial10;
            // spectrumGraphObject.DisplaySettings.FocusedIonAnnotationFont = GraphicToolsRepository.FontArial12Bold;
            spectrumGraphObject.DisplaySettings.IonAnnotationFormatString = "{0:0.000}";
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