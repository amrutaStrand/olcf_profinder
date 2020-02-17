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

    using Agilent.OpenLab.Framework.DataAccess.CoreTypes;

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
    ///     The chromatogram data provider.
    /// </summary>
    public static class ChromatogramDataProvider
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create chromatogram data.
        /// </summary>
        /// <param name="xMin">
        /// The xmin.
        /// </param>
        /// <param name="xStep">
        /// The x step.
        /// </param>
        /// <param name="numberDataPoints">
        /// The number data points.
        /// </param>
        /// <param name="noiseAmplitude">
        /// The noise amplitude.
        /// </param>
        /// <param name="peakDescriptions">
        /// The peak descriptions.
        /// </param>
        /// <returns>
        /// The <see cref="IChromData"/>.
        /// </returns>
        public static IChromData CreateChromatogramData(
            double xMin,
            double xStep,
            int numberDataPoints,
            double noiseAmplitude,
            params PeakDescription[] peakDescriptions)
        {
            var yData = new double[numberDataPoints];
            for (var i = 0; i < numberDataPoints; ++i)
            {
                yData[i] = 0.0;
            }

            foreach (var peakDescription in peakDescriptions)
            {
                AddPeak(yData, xMin, xStep, peakDescription);
            }

            AddNoise(yData, noiseAmplitude);
            var data = new EquidistantData(yData, xMin, xStep);
            var chromData = new ChromData(data, XUnit.Minutes, "something");

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