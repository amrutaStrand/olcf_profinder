namespace Agilent.OpenLab.CompoundGroupsTable
{
    using Events;
    using DataTypes;
    using System.Collections.Generic;
    using System;
    using Agilent.OpenLab.CompoundGroupsTable.ViewModels;

    /// <summary>
    /// CompoundGroupsTableViewModel
    /// </summary>
    partial class CompoundGroupsTableViewModel
    {
        #region Constants and Fields

        /// <summary>
        /// parameters used to track unhandled events
        /// </summary>
        private bool unhandledSomethingEventMonitored;

        #endregion

        #region Methods

        private void DoSomething()
        {
        }

        private void OnSomethingHappenedEvent(object sender)
        {
            if (!this.IsActive)
            {
                this.unhandledSomethingEventMonitored = true;
                return;
            }

            this.DoSomething();
        }

        /// <summary>
        /// Subscribes the events.
        /// </summary>
        /// <remarks>
        /// Subscribe to any events the module has to react on.
        /// </remarks>
        private void SubscribeEvents()
        {
            // This might look like the following line of code:
            this.EventAggregator.GetEvent<CompoundGroupsGenerated>().Subscribe(this.CompoundsGenerated);
        }

        private void CompoundsGenerated(List<ICompoundGroup> obj)
        {
            CompoundGroups.Clear();
            IEnumerator<ICompoundGroup> enumerator = obj.GetEnumerator();
            while(enumerator.MoveNext())
                CompoundGroups.Add(new CompoundGroupItem(enumerator.Current));

        }

        /// <summary>
        /// Unsubscribes the events.
        /// </summary>
        /// <remarks>
        /// Unsubscribe any events previously subscribed.
        /// </remarks>
        private void UnsubscribeEvents()
        {
            // This might look like the following line of code:
            // this.EventAggregator.GetEvent<SomethingHappenedEvent>().Unsubscribe(this.OnSomethingHappenedEvent);
            this.EventAggregator.GetEvent<CompoundGroupsGenerated>().Unsubscribe(this.CompoundsGenerated);

        }

        #endregion
    }
}