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
    using Agilent.OpenLab.TICPlot.ViewModels;
    using System.Drawing;

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
            EventAggregator.GetEvent<SamplesAdded>().Subscribe(UpdateData);
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

        private void UpdateData(List<string> sampleFiles)
        {
            Data = TICPlotDataExtractor.Extract(sampleFiles);
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
            paneManager.AxisX.AxisStyles.Extent = 15;

            paneManager.AxisY.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisY.AxisTitle.SetTitle("Counts");
            paneManager.AxisY.AxisTitle.IsVisible = false;
            paneManager.AxisY.AxisTitle.Alignment = UI.Controls.AgtPlotControl.GraphicElements.HorizontalAlignment.Center;
            paneManager.AxisY.AxisStyles.SeparateExponent = true;
            paneManager.AxisY.AxisTitle.Font = GraphicToolsRepository.FontArial8;
            paneManager.AxisY.AxisTitle.Brush = GraphicToolsRepository.DefaultAxisTitleBrush;
            paneManager.AxisY.AxisStyles.Extent = 40;
            paneManager.BoundingBoxExpansion = new BoundingBoxExpansion(
                5, 5, 5, 5, BoundingBoxExpansion.ExpansionMode.Relative);

            paneManager.HorizontalAxisScalingAnchorPoint = AnchorPoint.Max;
            paneManager.VerticalAxisScalingAnchorPoint = AnchorPoint.Min;

            paneManager.MinimumPlotPaneWidth = 100;
            paneManager.MinimumPlotPaneHeight = 50;
            paneManager.MaximumAllowedRelativeLegendHeight = 0.5;

            paneManager.LegendContainerSettings.LegendMode = LegendMode.AbovePlotPane;
        }


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