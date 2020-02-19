namespace Agilent.OpenLab.TICPlot
{
    /// <summary>
    /// ITICPlotView
    /// </summary>
    public interface ITICPlotView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ITICPlotViewModel Model { get; set; }

        #endregion
    }
}