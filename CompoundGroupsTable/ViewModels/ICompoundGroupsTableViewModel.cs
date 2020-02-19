namespace Agilent.OpenLab.CompoundGroupsTable
{
    using Agilent.OpenLab.CompoundGroupsTable.ViewModels;
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;
    using System.ComponentModel;

    #endregion

    /// <summary>
    /// ICompoundGroupsTableViewModel
    /// </summary>
    public interface ICompoundGroupsTableViewModel : IBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the toggle command A.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ToggleCommand<object> ToggleCommandA { get; }

        /// <summary>
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> TriggerCommandB { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        ICompoundGroupsTableView View { get; }

        /// <summary>
        /// Gets the compound groups list.
        /// </summary>
        BindingList<ICompoundGroupItem> CompoundGroups { get; }

        /// <summary>
        /// Gets the selected compound groups list.
        /// </summary>
        BindingList<ICompoundGroupItem> SelectedCompoundGroups { get; set; }

        /// <summary>
        ///     Gets or sets the focused compound.
        /// </summary>
        ICompoundGroupItem FocusedCompoundGroup { get; set; }

        #endregion
    }
}