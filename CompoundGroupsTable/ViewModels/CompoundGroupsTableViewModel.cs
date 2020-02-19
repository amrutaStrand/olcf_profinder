namespace Agilent.OpenLab.CompoundGroupsTable
{
    using Agilent.OpenLab.CompoundGroupsTable.ViewModels;
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using DataTypes;
    using Events;
    using Microsoft.Practices.Unity;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion

    /// <summary>
    /// CompoundGroupsTableViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class CompoundGroupsTableViewModel : BaseViewModel, ICompoundGroupsTableViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundGroupsTableViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public CompoundGroupsTableViewModel(IUnityContainer container)
            : base(container)
        {
            this.CompoundGroups = new BindingList<ICompoundGroupItem>();
            this.View = this.UnityContainer.Resolve<ICompoundGroupsTableView>();
            this.View.Model = this;
            this.SubscribeEvents();
            this.InitializeCommands();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public ICompoundGroupsTableView View { get; set; }

        /// <summary>
        /// Gets or sets the CompoundGroups list.
        /// </summary>
        /// <value>
        /// The compound groups list. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public BindingList<ICompoundGroupItem> CompoundGroups { get; private set; }

        BindingList<ICompoundGroupItem> selectedCompoundGroups;

        /// <summary>
        ///     Gets or sets the focused compound.
        /// </summary>
        public ICompoundGroupItem FocusedCompoundGroup { get; set; }

        /// <summary>
        /// To add the SelectedCompounds
        /// </summary>
        public BindingList<ICompoundGroupItem> SelectedCompoundGroups
        {
            get
            {
                return this.selectedCompoundGroups;
            }

            set
            {
                this.selectedCompoundGroups = value;
                fireSelectionChanged(selectedCompoundGroups);
            }
        }

        private void fireSelectionChanged(BindingList<ICompoundGroupItem> selectedCompounds)
        {
            IEnumerator<ICompoundGroupItem> enumerator = selectedCompounds.GetEnumerator();
            ICompoundGroup obj = null;
            while (enumerator.MoveNext()) {
                ICompoundGroupItem temp = enumerator.Current;
                if (obj == null)
                    obj = temp.CompoundGroupInfo;
            }
            EventAggregator.GetEvent<CompoundSelectionChanged>().Publish(obj);
        }
        #endregion
    }
}