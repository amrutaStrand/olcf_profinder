namespace Agilent.OpenLab.CompoundGroups
{
    /// <summary>
    /// ICompoundGroupsView
    /// </summary>
    public interface ICompoundGroupsView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        ICompoundGroupsViewModel Model { get; set; }

        #endregion
    }
}