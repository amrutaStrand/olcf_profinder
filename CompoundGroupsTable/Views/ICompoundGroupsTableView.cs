namespace Agilent.OpenLab.CompoundGroupsTable
{
    /// <summary>
    /// ICompoundGroupsTableView
    /// </summary>
    public interface ICompoundGroupsTableView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ICompoundGroupsTableViewModel Model { get; set; }

        #endregion
    }
}