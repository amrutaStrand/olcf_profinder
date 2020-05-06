namespace Agilent.OpenLab.ComoundTable
{
    #region

    using Agilent.OpenLab.Framework.UI.Common.Commands;
    using Agilent.OpenLab.Framework.UI.Layout.ModuleInterfaces;
    using DataTypes;
    using System.ComponentModel;

    #endregion

    /// <summary>
    /// IComoundTableViewModel
    /// </summary>
    public interface IComoundTableViewModel : IBaseViewModel
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
        /// Gets the trigger command B.
        /// </summary>
        /// <remarks>
        /// </remarks>
        TriggerCommand<object> ExportCommand { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        IComoundTableView View { get; }

        /// <summary>
        /// Gets the compound groups list.
        /// </summary>
        BindingList<ICompound> Compounds { get; }

        /// <summary>
        /// Gets the selected compound groups list.
        /// </summary>
        BindingList<ICompound> SelectedCompounds { get; set; }

        /// <summary>
        ///     Gets or sets the focused compound.
        /// </summary>
        ICompound FocusedCompound { get; set; }

        /// <summary>
        /// Exports data to a selected file.
        /// </summary>
        void ExportData();

        #endregion
    }
}