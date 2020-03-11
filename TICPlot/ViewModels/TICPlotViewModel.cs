namespace Agilent.OpenLab.TICPlot
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicElements;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicObjects;
    using Agilent.OpenLab.UI.Controls.GraphObjects.TimedEvents;
    using Microsoft.Practices.Unity;
    using TICPlotDataExtractor;
    using System.Collections.Generic;
    using DataTypes;
    using Events;
    using Utils;
    using Agilent.OpenLab.TICPlot.ViewModels;
    using System.Drawing;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.ObjectTransformation;

    #endregion

    /// <summary>
    /// TICPlotViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class TICPlotViewModel : BaseViewModel, ITICPlotViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TICPlotViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public TICPlotViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ITICPlotView>();
            this.View.Model = this;
            this.SubscribeEvents();
            this.InitializeCommands();
            
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ITICPlotView View { get; set; }

        /// <summary>
        ///     The plot control.
        /// </summary>
        private AgtPlotControl plotControl;

        /// <summary>
        ///     Gets PlotControl.
        /// </summary>
        public AgtPlotControl PlotControl
        {
            get
            {
                if (this.plotControl == null)
                {
                    this.plotControl = new AgtPlotControl();
                }

                return this.plotControl;
            }
        }

        /// <summary>
        /// Gets TIC data
        /// </summary>
        public Dictionary<string, TICData> Data { get; set; }
            
        #endregion

        #region Methods

        private void UpdateData(List<ISample> samples)
        {
            var lst = new List<string>();
            foreach(var sample in samples)
            {
                lst.Add(sample.FileName);
            }
            Data = TICPlotDataExtractor.Extract(lst);
            InitializeLayout();
            CreateGraphObjects();
        }

        /// <summary>
        ///     The initialize layout.
        /// </summary>
        private void InitializeLayout()
        {
            this.PlotControl.RemoveAllItems();

            int numRows = 1;
            if (Data != null)
                numRows = Data.Count;
            int numRowsToShow = numRows;
            if (numRowsToShow > 3)
                numRowsToShow = 3;
            this.PlotControl.Initialize(numRows, 1, numRowsToShow, 1);
            this.PlotControl.GraphManager.ShowLegend = true;
            this.PlotControl.GraphManager.ShowSignalHints = true;
            this.PlotControl.ControlSettings.PaneSelectionMode = PaneSelectionMode.None;

            var objectTransformation = new FullScaleRelativeObjectTransform();
            PlotControl.GraphManager.SetObjectTransform(objectTransformation);

            plotControl.GraphManager.LinkXAxis = true;
            plotControl.GraphManager.LinkYAxis = true;

            foreach (var paneManager in this.PlotControl.GraphManager.PaneManagers())
            {
                this.InitializePaneProperties(paneManager);
                paneManager.AxisX.AxisStyles.MajorGridStyle.Visible = false;
                paneManager.AxisY.AxisStyles.MajorGridStyle.Visible = false;
                paneManager.BoundingBoxExpansion = new BoundingBoxExpansion(
                    0, 0, 5, 40, BoundingBoxExpansion.ExpansionMode.Relative);
            }
        }

        /// <summary>
        /// The initialize pane properties.
        /// </summary>
        /// <param name="paneManager">
        /// The pane manager.
        /// </param>
        private void InitializePaneProperties(AgtPaneManager paneManager)
        {
            paneManager.BackgroundBrush = GraphicToolsRepository.AgilentWhiteBrush;

            paneManager.AxisX.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisX.AxisTitle.SetTitle("Minutes");
            paneManager.AxisX.AxisTitle.Alignment = HorizontalAlignment.Center;
            paneManager.AxisX.AxisTitle.Font = GraphicToolsRepository.FontArial8;
            paneManager.AxisX.AxisTitle.Brush = GraphicToolsRepository.DefaultAxisTitleBrush;
            paneManager.AxisX.AxisStyles.Extent = 20;

            paneManager.AxisY.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisY.AxisTitle.SetTitle("Counts");
            paneManager.AxisY.AxisTitle.IsVisible = true;
            paneManager.AxisY.AxisTitle.Alignment = UI.Controls.AgtPlotControl.GraphicElements.HorizontalAlignment.Center;
            paneManager.AxisY.AxisStyles.SeparateExponent = true;
            paneManager.AxisY.AxisTitle.Font = GraphicToolsRepository.FontArial8;
            paneManager.AxisY.AxisTitle.Brush = GraphicToolsRepository.DefaultAxisTitleBrush;
            paneManager.AxisY.AxisStyles.Extent = 30;
            paneManager.BoundingBoxExpansion = new BoundingBoxExpansion(
                5, 5, 5, 5, BoundingBoxExpansion.ExpansionMode.Relative);

            paneManager.HorizontalAxisScalingAnchorPoint = AnchorPoint.Max;
            paneManager.VerticalAxisScalingAnchorPoint = AnchorPoint.Min;

            paneManager.MinimumPlotPaneWidth = 100;
            paneManager.MinimumPlotPaneHeight = 50;
            paneManager.MaximumAllowedRelativeLegendHeight = 0.5;

            paneManager.LegendContainerSettings.LegendMode = LegendMode.AbovePlotPane;
        }


        Color[] colorArray = ColorConstants.COLOR_ARRAY;

        private void CreateGraphObjects()
        {
            if (Data == null)
                return;
            int index = 0;
            foreach (string sample in Data.Keys)
            {
                TICData sampleData = Data[sample];
                Color graphColor = colorArray[index % colorArray.Length];
                GraphBaseObject graphObject = new TICGraphObject(sampleData, graphColor);
                PlotControl.AddItem(index, 0, graphObject);
                index++;
            }
        }

        #endregion
    }
}