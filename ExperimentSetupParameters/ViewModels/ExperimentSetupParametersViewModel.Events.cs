namespace Agilent.OpenLab.ExperimentSetupParameters
{
    /// <summary>
    /// ExperimentSetupParametersViewModel
    /// </summary>
    partial class ExperimentSetupParametersViewModel
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
            // this.EventAggregator.GetEvent<SomethingHappenedEvent>().Subscribe(this.OnSomethingHappenedEvent);
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
        }

        #endregion
    }
}