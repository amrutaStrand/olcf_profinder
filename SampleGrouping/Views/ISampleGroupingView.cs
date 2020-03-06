namespace Agilent.OpenLab.SampleGrouping
{
    /// <summary>
    /// ISampleGroupingView
    /// </summary>
    public interface ISampleGroupingView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ISampleGroupingViewModel Model { get; set; }

        /// <summary>
        /// Updates focus
        /// </summary>
        void UpdateFocus();

        #endregion
    }
}