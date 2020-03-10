using System;
using System.Collections.Generic;
using DataTypes;
using Events;

namespace Agilent.OpenLab.ProfinderController
{
    /// <summary>
    /// UIModuleProjectTemplateViewModel
    /// </summary>
    partial class ProfinderControllerViewModel
    {
        #region Methods

        private void OnSomethingHappenedEvent(object sender)
        {
            // Do something
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
            this.EventAggregator.GetEvent<RunMFEInitiated>().Subscribe(this.runMFEWithBusyIndicator);
            this.EventAggregator.GetEvent<SamplesAdded>().Subscribe(this.SamplesAddedEventHandler);
        }

        private void SamplesAddedEventHandler(List<ISample> samples)
        {
            var filePaths = new List<string>();
            foreach(ISample item in samples)
            {
                
                filePaths.Add(item.FileName);

            }
            if (filePaths.Count != 0)
            {
                this.FilePaths = filePaths;
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
            // this.EventAggregator.GetEvent<SomethingHappenedEvent>().Unsubscribe(this.OnSomethingHappenedEvent);
            this.EventAggregator.GetEvent<SamplesAdded>().Unsubscribe(this.SamplesAddedEventHandler);

            this.EventAggregator.GetEvent<RunMFEInitiated>().Unsubscribe(this.runMFEWithBusyIndicator);
        }

        #endregion
    }
}