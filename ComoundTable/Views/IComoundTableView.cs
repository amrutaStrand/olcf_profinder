namespace Agilent.OpenLab.ComoundTable
{
    /// <summary>
    /// IComoundTableView
    /// </summary>
    public interface IComoundTableView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        IComoundTableViewModel Model { get; set; }

        /// <summary>
        ///     The update focus.
        /// </summary>
        void UpdateFocus();


        #endregion
    }
}