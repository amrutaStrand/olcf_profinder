namespace Agilent.OpenLab.CompoundSpectrum
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;

    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicElements;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicObjects;
    using Microsoft.Practices.Unity;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paneManager"></param>
        protected void InitializePaneProperties(AgtPaneManager paneManager)
        {
            paneManager.BackgroundBrush = GraphicToolsRepository.AgilentWhiteBrush;

            paneManager.AxisX.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisX.AxisTitle.SetTitle("Mass");
            paneManager.AxisX.AxisTitle.Alignment = HorizontalAlignment.Center;
            paneManager.AxisX.AxisTitle.Font = GraphicToolsRepository.FontArial8;
            paneManager.AxisX.AxisTitle.Brush = GraphicToolsRepository.DefaultAxisTitleBrush;
            paneManager.AxisX.AxisStyles.Extent = 15;

            //            paneManager.AxisX.AxisStyles.TickLabelFont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
            //            paneManager.AxisY.AxisStyles.TickLabelFont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);

            paneManager.AxisY.AxisStyles.TickLabelFont = GraphicToolsRepository.FontArial8;
            paneManager.AxisY.AxisTitle.SetTitle("Count");
            paneManager.AxisY.AxisTitle.IsVisible = false;
            paneManager.AxisY.AxisTitle.Alignment = HorizontalAlignment.Center;
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

        #endregion
    }
}