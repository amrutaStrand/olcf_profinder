namespace Agilent.OpenLab.ComoundTable
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
    /// ComoundTableViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class ComoundTableViewModel : BaseViewModel, IComoundTableViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComoundTableViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public ComoundTableViewModel(IUnityContainer container)
            : base(container)
        {
            this.Compounds = new BindingList<ICompound>();
            this.View = this.UnityContainer.Resolve<IComoundTableView>();
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
        public IComoundTableView View { get; set; }

        /// <summary>
        /// Gets or sets the CompoundGroups list.
        /// </summary>
        /// <value>
        /// The compound groups list. 
        /// </value>
        /// <remarks>
        /// </remarks>
        public BindingList<ICompound> Compounds { get; private set; }

        BindingList<ICompound> selectedCompounds;

        /// <summary>
        ///     Gets or sets the focused compound.
        /// </summary>
        public ICompound FocusedCompound { get; set; }

        /// <summary>
        /// To add the SelectedCompounds
        /// </summary>
        public BindingList<ICompound> SelectedCompounds
        {
            get
            {
                return this.selectedCompounds;
            }

            set
            {
                this.selectedCompounds = value;
                fireSelectionChanged(selectedCompounds);
            }
        }

        private void fireSelectionChanged(BindingList<ICompound> selectedCompounds)
        {
            List<ICompound> selected = new List<ICompound>();

            IEnumerator<ICompound> enumerator = selectedCompounds.GetEnumerator();

            while (enumerator.MoveNext())
            {
                selected.Add(enumerator.Current);
            }

            //this.EventAggregator.GetEvent<CompoundSelectionChanged>().Publish(selected);
        }




        #endregion
    }
}