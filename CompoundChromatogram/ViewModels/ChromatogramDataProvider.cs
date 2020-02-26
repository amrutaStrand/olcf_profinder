// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromatogramDataProvider.cs" company="">
//   
// </copyright>
// <summary>
//   The peak description.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Agilent.OpenLab.CompoundChromatogram
{
    using System;
    using System.Collections.Generic;
    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;
    using DataTypes;

    /// <summary>
    ///     The peak description.
    /// </summary>
    public class PeakDescription
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        ///     Gets or sets the retention time.
        /// </summary>
        public double RetentionTime { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        public double Width { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomeData : IData
    {
        double[] xValues;
        double[] yValues;
        /// <summary>
        /// 
        /// </summary>
        public double[] XValues { get { return xValues; } set { xValues = value; } }
        /// <summary>
        /// 
        /// </summary>
        public double[] YValues { get { return yValues; } set { yValues = value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public CustomeData( List <DataPoint> list)
        {
            List<double> xarray = new List<double>();
            List<double> yarray = new List<double>();
            foreach(DataPoint point in list)
            {
                xarray.Add(point.X);
                yarray.Add(point.Y);
            }
            if (xarray.Count == yarray.Count)
            {
                XValues = xarray.ToArray();
                YValues = yarray.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private CustomeData( double[] x, double[] y)
        {
            XValues = x;
            YValues = y;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IData Clone()
        {
            return new CustomeData(xValues, yValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetShift()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        public void MarkAsShifted(double delta)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        public void Shift(double delta)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToSubtract"></param>
        public void Subtract(IData dataToSubtract)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     The chromatogram data provider.
    /// </summary>
    public static class ChromatogramDataProvider
    {
        #region Public Methods and Operators

        /// <summary>
        /// generates IChromeData object
        /// </summary>
        /// <param name="chromatogram"></param>
        /// <returns></returns>
        public static IChromData CreateChromatogramData(
            IChromatogram chromatogram)
        {

            //var yData = new double[numberDataPoints];
            //for (var i = 0; i < numberDataPoints; ++i)
            //{
            //    yData[i] = 0.0;
            //}

            //foreach (var peakDescription in peakDescriptions)
            //{
            //    AddPeak(yData, xMin, xStep, peakDescription);
            //}

            //AddNoise(yData, noiseAmplitude);
            //var data = new EquidistantData(yData, xMin, xStep);
            IData data = new CustomeData(chromatogram.Data); 
            var chromData = new ChromData(data, XUnit.Minutes, "Abundance");

            return chromData;
        }

        /// <summary>
        /// The create peak desctiptions.
        /// </summary>
        /// <param name="numberOfPeaks">
        /// The number of peaks.
        /// </param>
        /// <param name="minWidth">
        /// The min width.
        /// </param>
        /// <param name="maxWidth">
        /// The max width.
        /// </param>
        /// <param name="minHeight">
        /// The min height.
        /// </param>
        /// <param name="maxHeight">
        /// The max height.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>        
        public static PeakDescription[] CreatePeakDesctiptions(
            int numberOfPeaks,
            double minWidth,
            double maxWidth,
            double minHeight,
            double maxHeight,
            double min,
            double max)
        {
            var random = new Random();
            var peaks = new PeakDescription[numberOfPeaks];
            for (var i = 0; i < numberOfPeaks; ++i)
            {
                var retentionTime = min + maxWidth + random.NextDouble() * (max - min - 2 * maxWidth);
                var height = minHeight + random.NextDouble() * (maxHeight - minHeight);
                var width = minWidth + random.NextDouble() * (maxWidth - minWidth);
                peaks[i] = new PeakDescription { Height = height, Width = width, RetentionTime = retentionTime };
            }

            return peaks;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add noise.
        /// </summary>
        /// <param name="yData">
        /// The y data.
        /// </param>
        /// <param name="noiseAmplitude">
        /// The noise amplitude.
        /// </param>
        private static void AddNoise(double[] yData, double noiseAmplitude)
        {
            var random = new Random();
            for (var i = 0; i < yData.Length; ++i)
            {
                yData[i] += noiseAmplitude * (2.0 * random.NextDouble() - 1.0);
            }
        }

        /// <summary>
        /// The add peak.
        /// </summary>
        /// <param name="yData">
        /// The y data.
        /// </param>
        /// <param name="xMin">
        /// The x min.
        /// </param>
        /// <param name="xStep">
        /// The x step.
        /// </param>
        /// <param name="peakDescription">
        /// The peak description.
        /// </param>
        private static void AddPeak(double[] yData, double xMin, double xStep, PeakDescription peakDescription)
        {
            var xStart = peakDescription.RetentionTime - 3 * peakDescription.Width;
            var xStop = peakDescription.RetentionTime + 3 * peakDescription.Width;
            var startIndex = Math.Max(0, (int)((xStart - xMin) / xStep));
            var stopIndex = Math.Min(yData.Length - 1, (int)((xStop - xMin) / xStep));
            for (var i = startIndex; i <= stopIndex; ++i)
            {
                var x = xMin + i * xStep;
                yData[i] += peakDescription.Height
                            * Math.Exp(
                                -(peakDescription.RetentionTime - x) * (peakDescription.RetentionTime - x)
                                / (peakDescription.Width * peakDescription.Width));
            }
        }

        #endregion
    }
}