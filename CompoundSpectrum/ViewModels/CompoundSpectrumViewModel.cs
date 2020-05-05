namespace Agilent.OpenLab.CompoundSpectrum
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Export;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicElements;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicObjects;
    using DataTypes;
    using Microsoft.Practices.Unity;
    using System.Drawing.Imaging;
    using System.Windows;

    #endregion

    /// <summary>
    /// CompoundSpectrumViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundSpectrumViewModel : BaseViewModel, ICompoundSpectrumViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     The plot control.
        /// </summary>
        private AgtPlotControl plotControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundSpectrumViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public CompoundSpectrumViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<ICompoundSpectrumView>();
            this.View.Model = this;
            this.ExperimentContext = UnityContainer.Resolve<IExperimentContext>();
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
        public ICompoundSpectrumView View { get; set; }

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

        private IExperimentContext ExperimentContext { get; set; }

        /// <summary>
        /// Gets or sets the display mode of the plots.
        /// </summary>
        public string DisplayMode { get; set; }

        /// <summary>
        /// Gets or sets the color by sample group flag.
        /// </summary>
        public bool ColorBySampleGroupFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paneManager"></param>
        protected void InitializePaneProperties(AgtPaneManager paneManager)
        {
            paneManager.BackgroundBrush = GraphicToolsRepository.AgilentWhiteBrush;

            paneManager.AxisX.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisX.AxisTitle.SetTitle("m/z");
            paneManager.AxisX.AxisTitle.Alignment = UI.Controls.AgtPlotControl.GraphicElements.HorizontalAlignment.Center;
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
                10, 10, 5, 5, BoundingBoxExpansion.ExpansionMode.Relative);

            paneManager.HorizontalAxisScalingAnchorPoint = AnchorPoint.Max;
            paneManager.VerticalAxisScalingAnchorPoint = AnchorPoint.Min;

            paneManager.MinimumPlotPaneWidth = 100;
            paneManager.MinimumPlotPaneHeight = 50;
            paneManager.MaximumAllowedRelativeLegendHeight = 0.5;


            paneManager.AxisX.AxisStyles.MajorGridStyle.Visible = false;
            paneManager.AxisX.AxisStyles.MinorGridStyle.Visible = false;
            paneManager.AxisY.AxisStyles.MajorGridStyle.Visible = false;
            paneManager.AxisY.AxisStyles.MinorGridStyle.Visible = false;

            paneManager.LegendContainerSettings.LegendMode = LegendMode.AbovePlotPane;
        }

        private void ExportToPng(string filepath)
        {
            var paneExportSource = new PaneExportSourceAll() { SkipEmptyPanes = true };
            var bitmap = PlotControlExportUtilities.ExportToBitmap(this.plotControl, paneExportSource);
            PlotControlExportUtilities.BitmapToFile(bitmap, filepath, ImageFormat.Png);
            MessageBox.Show("Image exported to " + filepath);
        }

        #endregion
    }
}