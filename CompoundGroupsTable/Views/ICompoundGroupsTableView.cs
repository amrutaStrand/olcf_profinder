namespace Agilent.OpenLab.CompoundGroupsTable
{

    using Agilent.OpenLab.UI.Controls.WinFormsControls;

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

        /// <summary>
        ///     The update focus.
        /// </summary>
        void UpdateFocus();

        /// <summary>
        /// 
        /// </summary>
        AgtBaseUltraGrid UltraGrid { get; }

        #endregion
    }
}