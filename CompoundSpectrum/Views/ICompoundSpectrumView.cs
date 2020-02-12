namespace Agilent.OpenLab.CompoundSpectrum
{
    /// <summary>
    /// ICompoundSpectrumView
    /// </summary>
    public interface ICompoundSpectrumView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ICompoundSpectrumViewModel Model { get; set; }

        #endregion
    }
}