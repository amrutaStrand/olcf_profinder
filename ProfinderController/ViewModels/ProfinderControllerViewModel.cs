﻿namespace Agilent.OpenLab.ProfinderController
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Events;
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using MFEProcessor;
    using Agilent.OpenLab.Framework.UI.Layout;
    #endregion

    public partial class ProfinderControllerViewModel : BaseViewModel, IProfinderControllerViewModel
    {
        #region Constructors and Destructors

        List<string> filePaths = null;
        /// <summary>
        /// Holds Cef file path
        /// </summary>
        /// <remarks>
        /// </remarks>
        public List<string> FilePaths
        {
            get
            {
                return filePaths;
            }
            set
            {
                filePaths = value;
                EventAggregator.GetEvent<SamplesAdded>().Publish(filePaths);
                SetApplicationState("SamplesAdded");
            }
        }

        private void runMFE(object unused)
        {

            //List<string> sampleFiles = new List<string>();
            //sampleFiles.Add(@"D:\Profinder\D01B.d");
            //sampleFiles.Add(@"D:\Profinder\D02B.d");
            MFEProcessor.MFE mfe = new MFEProcessor.MFE(FilePaths);
            List<DataTypes.ICompoundGroup> compoundGroups = mfe.Execute();
            //ProfinderDummyDataGenerator generator = new ProfinderDummyDataGenerator();
            //List<DataTypes.ICompoundGroup> compoundGroups = generator.GenerateDemoData(20, 20);
            EventAggregator.GetEvent<CompoundGroupsGenerated>().Publish(compoundGroups);
            SetApplicationState("MFEExecuted");
        }

        private void SetApplicationState(string state)
        {
            var applicationStateService = UnityContainer.Resolve<IApplicationStateService>();
            applicationStateService.ResetApplicationStates();
            applicationStateService.SetApplicationState(state, true, ApplicationStateUpdateMode.Immediate);
            applicationStateService.ApplyApplicationStates();
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProfinderControllerViewModel" /> class.
        /// </summary>
        /// <param name = "container">The container.</param>
        public ProfinderControllerViewModel(IUnityContainer container)
            : base(container)
        {
            this.InitializeCommands();
            this.SubscribeEvents();
        }

        #endregion                    
    }
}