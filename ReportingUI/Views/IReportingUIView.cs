namespace Agilent.OpenLab.ReportingUI
{
    /// <summary>
    /// IReportingUIView
    /// </summary>
    public interface IReportingUIView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        IReportingUIViewModel Model { get; set; }

        #endregion
    }
}