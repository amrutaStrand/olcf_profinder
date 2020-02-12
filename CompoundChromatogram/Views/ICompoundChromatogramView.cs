namespace Agilent.OpenLab.CompoundChromatogram
{
    /// <summary>
    /// ICompoundChromatogramView
    /// </summary>
    public interface ICompoundChromatogramView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ICompoundChromatogramViewModel Model { get; set; }

        #endregion
    }
}