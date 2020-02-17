namespace Agilent.OpenLab.ComoundTable
{
    using DataTypes;
    using Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ComoundTableViewModel
    /// </summary>
    partial class ComoundTableViewModel
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
            this.EventAggregator.GetEvent<CompoundSelectionChanged>().Subscribe(this.CompoundGroupSelectionChanged);

        }

        private void CompoundGroupSelectionChanged(ICompoundGroup obj)
        {
            Compounds.Clear();
            if (obj == null) return;
            IDictionary<string, ICompound> sampleWiseICompounds = obj.SampleWiseDataDictionary;
            if (sampleWiseICompounds == null) return;
            IEnumerator<ICompound> enumerator = sampleWiseICompounds.Values.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Compounds.Add(enumerator.Current);
            }
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
            this.EventAggregator.GetEvent<CompoundSelectionChanged>().Unsubscribe(this.CompoundGroupSelectionChanged);
        }
        #endregion
    }
}