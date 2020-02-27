using Agilent.OpenLab.Framework.UI.Common.Utilities;
using Agilent.OpenLab.UI.Controls.AgtPlotControl;
using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicObjects;
using Agilent.OpenLab.UI.Controls.AgtPlotControl.Interfaces;
using Agilent.OpenLab.UI.Controls.Geometry;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.TICPlot.ViewModels
{
    class TICGraphObject : GraphBaseObjectWithLegend
    {

        /// <summary>
        ///     The pen.
        /// </summary>
        private Pen pen = new Pen(Color.Black);

        /// <summary>
        ///     The color.
        /// </summary>
        private Color color;

        /// <summary>
        ///     The focused.
        /// </summary>
        private bool focused;

        #region Constructor
        public TICGraphObject(ITICData data)
        {
            Data = ArgumentValidator.CheckNotNull(data, "data");
            this.TransformationHandler.DataBoundingBox = new BoundingBox2D(
                this.Data.XArray.Min(),
                this.Data.YArray.Min(),
                this.Data.XArray.Max(),
                this.Data.YArray.Max());
            this.Color = Color.Black;

        }
        public TICGraphObject(ITICData data, Color graphColor)
        {
            Data = ArgumentValidator.CheckNotNull(data, "data");
            this.TransformationHandler.DataBoundingBox = new BoundingBox2D(
                this.Data.XArray.Min(),
                this.Data.YArray.Min(),
                this.Data.XArray.Max(),
                this.Data.YArray.Max());
            this.Color = graphColor;

        }
        #endregion

        #region Public Properties
        public ITICData Data { get; set; }

        /// <summary>
        ///     The disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///     Sets the color.
        /// </summary>
        public Color Color
        {
            set
            {
                if (value != this.color)
                {
                    this.color = value;
                    this.pen = new Pen(this.color);
                    if (this.Legend != null)
                    {
                        this.Legend.ForegroundColor = value;
                    }
                }
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether the spectrum graph object is visible.
        /// </summary>
        public bool Focused
        {
            get
            {
                return this.focused;
            }

            set
            {
                if (this.focused != value)
                {
                    this.focused = value;
                    if (this.Legend != null)
                    {
                        this.Legend.Focused = this.focused;
                    }
                }
            }
        }


        /// <summary>
        ///     Gets the legend.
        /// </summary>
        public FocusableTextLegendObject Legend
        {
            get
            {
                return this.LegendObject as FocusableTextLegendObject;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The create legend object.
        /// </summary>
        /// <param name="legendLine">
        /// The legend line.
        /// </param>
        public void CreateLegendObject(string legendLine)
        {
            this.CreateLegendObject(new List<string> { legendLine });
        }

        /// <summary>
        /// The create legend object.
        /// </summary>
        /// <param name="legendLines">
        /// The legend lines.
        /// </param>
        public void CreateLegendObject(IEnumerable<string> legendLines)
        {
            var textAnnotationObject = new FocusableTextLegendObject { ForegroundColor = this.color };
            textAnnotationObject.Annotation.SetLines(legendLines);
            this.LegendObject = textAnnotationObject;
            this.LegendObject.AssociatedGraphObject = this;
        }

        #endregion


        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    this.pen.Dispose();
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The select.
        /// </summary>
        /// <param name="paneManager">
        /// The pane manager.
        /// </param>
        /// <param name="selectionBuffer">
        /// The selection buffer.
        /// </param>
        /// <returns>
        /// The <see cref="SelectionElement"/>.
        /// </returns>
        protected override SelectionElement Select(IPaneManager paneManager, ISelectionBuffer selectionBuffer)
        {
            paneManager = ArgumentValidator.CheckNotNull(paneManager, "paneManager");
            selectionBuffer = ArgumentValidator.CheckNotNull(selectionBuffer, "selectionBuffer");

            selectionBuffer.Clear();
            this.DrawInternal(
                selectionBuffer.SelectionGraphics, paneManager.CoordinateConverter, paneManager.RenderMode);
            if (selectionBuffer.ContainsHit())
            {
                var selectionElement = new SelectionElement(this);
                this.OnObjectSelected(selectionElement, paneManager, selectionBuffer);
                return selectionElement;
            }

            return null;
        }


        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="paneManager">
        /// The pane manager.
        /// </param>
        /// <param name="graphics">
        /// The graphics.
        /// </param>
        protected override void Draw(IPaneManager paneManager, Graphics graphics)
        {
            paneManager = ArgumentValidator.CheckNotNull(paneManager, "paneManager");
            this.DrawInternal(graphics, paneManager.CoordinateConverter, paneManager.RenderMode);
        }


        /// <summary>
        /// The draw zoom overview.
        /// </summary>
        /// <param name="paneManager">
        /// The pane manager.
        /// </param>
        /// <param name="graphics">
        /// The graphics.
        /// </param>
        protected override void DrawZoomOverview(IPaneManager paneManager, Graphics graphics)
        {
            paneManager = ArgumentValidator.CheckNotNull(paneManager, "paneManager");
            this.DrawInternal(graphics, paneManager.ZoomOverviewInfo.CoordinateConverter, paneManager.RenderMode);
        }

        /// <summary>
        /// The draw internal.
        /// </summary>
        /// <param name="graphics">
        /// The graphics.
        /// </param>
        /// <param name="coordinateConverter">
        /// The coordinate converter.
        /// </param>
        /// <param name="renderMode">
        /// The render mode.
        /// </param>
        private void DrawInternal(
            Graphics graphics, IBasicCoordinateConverter coordinateConverter, PaneRenderMode renderMode)
        {
            graphics = ArgumentValidator.CheckNotNull(graphics, "graphics");
            coordinateConverter = ArgumentValidator.CheckNotNull(coordinateConverter, "coordinateConverter");

            var smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            this.pen.Width = this.Selected && renderMode == PaneRenderMode.Quality ? 2f : 1f;


            for (int i = 0; i < (Data.XArray.Length - 1); i++)
            {
                double x = Data.XArray[i];
                double y = Data.YArray[i];
                double x1 = Data.XArray[i + 1];
                double y1 = Data.YArray[i + 1];

                graphics.DrawLine(
                    this.pen,
                    coordinateConverter.PhysicalToControl(
                        this.ActiveTransformationHandler.Transform(x, y)),
                    coordinateConverter.PhysicalToControl(
                        this.ActiveTransformationHandler.Transform(x1, y1))
                );
            }
            graphics.SmoothingMode = smoothingMode;
        }
    }
}
