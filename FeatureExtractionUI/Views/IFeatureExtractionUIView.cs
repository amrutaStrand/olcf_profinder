namespace Agilent.OpenLab.FeatureExtractionUI
{
    /// <summary>
    /// IFeatureExtractionUIView
    /// </summary>
    public interface IFeatureExtractionUIView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        IFeatureExtractionUIViewModel Model { get; set; }

        #endregion
    }
}