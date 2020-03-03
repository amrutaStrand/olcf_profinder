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

        private void CompoundSelectionChanged(ICompoundGroup compoundGroupObject)
        {
            this.PlotControl.RemoveAllItems();
            if (compoundGroupObject == null) return;

            IDictionary<string, ICompound> sampleWiseCompounds = compoundGroupObject.SampleWiseDataDictionary;
            List<MsSpectrumGraphObject> msSpectrumGraphObjects = new List<MsSpectrumGraphObject>();

            IEnumerator<ICompound> ce = sampleWiseCompounds.Values.GetEnumerator ();
            int colorCounter = 0;
            int colorArraySize = colorArray.Length;
            while (ce.MoveNext())
            {
                Color color = colorArray[colorCounter];
                colorCounter = (colorCounter + 1) % colorArraySize;
                MsSpectrumGraphObject mso = GetMsSpectrumGraphObject(ce.Current, color);

                msSpectrumGraphObjects.Add(mso);
            }
            updatePlotControl(msSpectrumGraphObjects);


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


        private void updatePlotControl(List<MsSpectrumGraphObject> msSpectrumGraphObjects)
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

            IEnumerator<MsSpectrumGraphObject> enumerator = msSpectrumGraphObjects.GetEnumerator();
            int horizontalCounter = 0;
            while (enumerator.MoveNext())
            {
                this.PlotControl.AddItem(horizontalCounter++, 0, enumerator.Current);

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
                        string.Format("{0};  #Datapoints = {1}", spectrumName, spectrumData.Data.XValues.Length),
                        string.Format("Max. Abundance: {0:0}", spectrumData.Data.YValues.Max())
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

            return new SpectrumData(new Data(xValues, yValues), XUnit.MassToCharge, "Counts", 13.56);

        }
        #endregion
    }
}