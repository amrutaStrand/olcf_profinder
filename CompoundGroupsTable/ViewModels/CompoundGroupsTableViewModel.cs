namespace Agilent.OpenLab.CompoundGroupsTable
{
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
            this.CompoundGroups = new BindingList<ICompoundGroup>();
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
        public BindingList<ICompoundGroup> CompoundGroups { get; private set; }

        BindingList<ICompoundGroup> selectedCompoundGroups;

        /// <summary>
        ///     Gets or sets the focused compound.
        /// </summary>
        public ICompoundGroup FocusedCompoundGroup { get; set; }

        /// <summary>
        /// To add the SelectedCompounds
        /// </summary>
        public BindingList<ICompoundGroup> SelectedCompoundGroups
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

        private void fireSelectionChanged(BindingList<ICompoundGroup> selectedCompounds)
        {
            IEnumerator<ICompoundGroup> enumerator = selectedCompounds.GetEnumerator();
            ICompoundGroup obj = null;
            while (enumerator.MoveNext()) {
                ICompoundGroup temp = enumerator.Current;
                if (obj == null)
                    obj = temp;
            }
            EventAggregator.GetEvent<CompoundSelectionChanged>().Publish(obj);
        }
        #endregion
    }
}