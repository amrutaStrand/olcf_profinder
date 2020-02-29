using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.CompoundChromatogram
{
    using Agilent.OpenLab.Framework.BusinessObjectModel;
    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Basic;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.GraphicObjects;
    using Agilent.OpenLab.UI.Controls.AgtPlotControl.Interfaces;
    using Agilent.OpenLab.UI.Controls.Geometry;
    using Agilent.OpenLab.UI.Controls.Geometry.Utilities;
    using Agilent.OpenLab.UI.Controls.GraphObjects;
    using Agilent.OpenLab.UI.DataStructures.Converters;
    using Agilent.OpenLab.UI.DataStructures.Signals;
    using Agilent.OpenLab.UI.DataStructures.Signals.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 
    /// </summary>
    public class FilledChromatogramGraphObject : ChromatogramGraphObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromData"></param>
        public FilledChromatogramGraphObject(IChromData chromData): base(chromData)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signal"></param>
        public FilledChromatogramGraphObject(ICommonDataEx signal): base(signal)
        { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="paneManager"></param>
        /// <param name="graphics"></param>
        protected override void Draw(IPaneManager paneManager, Graphics graphics)
        {
            paneManager = ArgumentValidator.CheckNotNull<IPaneManager>(paneManager, "paneManager");
            graphics = ArgumentValidator.CheckNotNull<Graphics>(graphics, "graphics");
            if (!base.UsePersistentMode)
            {
                this.DrawSignal(graphics, paneManager.RenderingLoadManager, paneManager.CoordinateConverter, paneManager.RenderMode, paneManager.RenderTarget);
            }

            //else
            //{
            //    if (this.persistentLayer.Update(paneManager.CoordinateConverter, paneManager.RenderMode))
            //    {
            //        this.DrawSignal(this.persistentLayer.Graphics, paneManager.RenderingLoadManager, paneManager.CoordinateConverter, paneManager.RenderMode, paneManager.RenderTarget);
            //    }
            //    graphics.DrawImage(this.persistentLayer.Bitmap, paneManager.CoordinateConverter.MinPixelX, paneManager.CoordinateConverter.MinPixelY, this.persistentLayer.Bitmap.Width, this.persistentLayer.Bitmap.Height);
            // }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="renderingLoadManager"></param>
        /// <param name="coordinateConverter"></param>
        /// <param name="renderMode"></param>
        /// <param name="renderTarget"></param>

        private void DrawSignal(Graphics graphics, IRenderingLoadManager renderingLoadManager, IBasicCoordinateConverter coordinateConverter, PaneRenderMode renderMode, RenderTarget renderTarget)
        {
            graphics = ArgumentValidator.CheckNotNull<Graphics>(graphics, "graphics");
            coordinateConverter = ArgumentValidator.CheckNotNull<IBasicCoordinateConverter>(coordinateConverter, "coordinateConverter");
            if ((renderingLoadManager == null) || (!renderingLoadManager.RenderingLoadIsExceeded || (this.Selected || this.AssociatedInjectionIsSelected)))
            {
                SmoothingMode smoothingMode = graphics.SmoothingMode;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = (!this.Selected || ((renderMode != PaneRenderMode.Quality) || (renderTarget != RenderTarget.Screen))) ? new Pen(this.DisplaySettings.Color) : new Pen(this.DisplaySettings.Color);
                double num = base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.ControlToPhysicalX(2)) - base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.ControlToPhysicalX(1));
                int num2 = 1;
                IEquidistantDataEx signal = this.SignalData as IEquidistantDataEx;
                if (signal != null)
                {
                    num2 = (int)(num / signal.XStep);
                }
                int num3 = Math.Max(coordinateConverter.MinPixelX, coordinateConverter.PhysicalToControlX(base.ActiveTransformationHandler.TransformX(this.SignalData.XMin)));
                int num4 = Math.Min(coordinateConverter.MaxPixelX, coordinateConverter.PhysicalToControlX(base.ActiveTransformationHandler.TransformX(this.SignalData.XMax)));
                if (num2 > 3)
                {
                    for (int i = num3; i < num4; i++)
                    {
                        double num6 = base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.ControlToPhysicalX(i));
                        Range range = CommonDataExtensions.GetValueRange(this.SignalData, (double)(num6 - (0.5 * num)), (double)(num6 + (0.5 * num)));
                        double num7 = base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.ControlToPhysicalX((int)(i + 1)));
                        Range range2 = CommonDataExtensions.GetValueRange(this.SignalData, (double)(num7 - (0.5 * num)), (double)(num7 + (0.5 * num)));
                        int num8 = coordinateConverter.PhysicalToControlY(base.ActiveTransformationHandler.TransformY(range.Min));
                        int num9 = coordinateConverter.PhysicalToControlY(base.ActiveTransformationHandler.TransformY(range.Max));
                        int num10 = coordinateConverter.PhysicalToControlY(base.ActiveTransformationHandler.TransformY(range2.Min));
                        int num11 = coordinateConverter.PhysicalToControlY(base.ActiveTransformationHandler.TransformY(range2.Max));
                        graphics.DrawLine(pen, i, num8, i + 1, num11);
                        graphics.DrawLine(pen, i, num9, i + 1, num10);
                    }
                }
                else
                {
                    List<Point> plotPoints = new List<Point>();

                    int num13 = Math.Min(this.SignalData.GetNextIndex(base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.MaxPhysicalX)), this.SignalData.Count - 1);
                    for (int i = Math.Max(this.SignalData.GetPrevIndex(base.ActiveTransformationHandler.InverseTransformX(coordinateConverter.MinPhysicalX)), 0) + 1; i <= num13; i++)
                    {
                        Point point = coordinateConverter.PhysicalToControl(base.ActiveTransformationHandler.Transform(this.SignalData.GetXValue(i - 1), this.SignalData.GetValue((int)(i - 1))));
                        Point point2 = coordinateConverter.PhysicalToControl(base.ActiveTransformationHandler.Transform(this.SignalData.GetXValue(i), this.SignalData.GetValue(i)));
                        Point xPoint = coordinateConverter.PhysicalToControl(base.ActiveTransformationHandler.Transform(this.SignalData.GetXValue(i), 0));

                        if (((point.Y >= coordinateConverter.MinPixelY) || (point2.Y >= coordinateConverter.MinPixelY)) && (((point.Y <= coordinateConverter.MaxPixelY) || (point2.Y <= coordinateConverter.MaxPixelY)) && (((point.X >= coordinateConverter.MinPixelX) || (point2.X >= coordinateConverter.MinPixelX)) && (((point.X <= coordinateConverter.MaxPixelX) || (point2.X <= coordinateConverter.MaxPixelX)) && (IsPointValid(point) && IsPointValid(point2))))))
                        {
                            try
                            {
                                graphics.DrawLine(pen, point, point2);
                                //graphics.DrawLine(pen, point, xPoint);
                                //graphics.DrawLine(pen, point2, xPoint);
                                plotPoints.Add(point);
                                plotPoints.Add(point2);
                                
                            }
                            catch (OverflowException exception)
                            {
                                object[] args = new object[] { exception.Message };
                                TraceLog.Warning(string.Format(CultureInfo.InvariantCulture, "There was an overflow exception in the graph object.\nException Message = {0}", args));
                            }

                            int xMin = int.MaxValue;
                            int xMax = int.MinValue;
                            foreach (Point p in plotPoints)
                            {
                                int x = p.X;

                                if (x < xMin)
                                {
                                    xMin = x;
                                }

                                if (x > xMax)
                                {
                                    xMax = x;
                                }

                            }

                            Point start = new Point(xMin, coordinateConverter.PhysicalToControlY(0));
                            Point end = new Point(xMax, coordinateConverter.PhysicalToControlY(0));

                            List<Point> plotPoints1 = new List<Point>();
                            plotPoints1.Add(start);

                            foreach (Point p in plotPoints)
                            {
                                plotPoints1.Add(p);
                            }

                            plotPoints1.Add(end);
                            plotPoints1.Add(start);

                            Color color = pen.Color;

                            int transparancy = 10;
                            Pen tPen = new Pen(Color.FromArgb(transparancy, color));

                            graphics.FillPolygon(tPen.Brush, plotPoints1.ToArray());
                        }
                    }
                }
                graphics.SmoothingMode = smoothingMode;
            }
        }
    }
}
