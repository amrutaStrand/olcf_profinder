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
    using Agilent.OpenLab.Framework.UI.Common.Services;
    using DataTypes;
    using Agilent.OpenLab.Framework.UI.Common.Controls.Workflow;
    #endregion

    public partial class ProfinderControllerViewModel : BaseViewModel, IProfinderControllerViewModel
    {
        //IWorkflowNavigatorViewModel workflowNavigatorViewModel;
        #region Constructors and Destructors
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProfinderControllerViewModel" /> class.
        /// </summary>
        /// <param name = "container">The container.</param>
        public ProfinderControllerViewModel(IUnityContainer container)
            : base(container)
        {
            
            this.ExperimentContext = this.UnityContainer.Resolve<IExperimentContext>();
            this.InitializeCommands();
            this.SubscribeEvents();
            SetApplicationState("InitialState");
            //workflowNavigatorViewModel = this.UnityContainer.Resolve<IApplicationLayoutService>().WorkspaceLayoutService.ActiveWorkflowNavigatorViewModel;
            //workflowNavigatorViewModel.ActivateFirstPhase();
        }

        #endregion    

/*        List<string> filePaths = null;
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
            }
        }*/

        private List<ISample> samples;

        private List<ISample> Samples
        {
            get { return samples; }
            set {
                samples = value;
                //SetApplicationState("SamplesAdded");
                ActivateNextWorkflowPhase();
                InitializeMFE();
            }
        }


        private MFE mfeExecutor;

        private MFE MFEExecutor
        {
            get { return mfeExecutor; }
            set
            {
                mfeExecutor = value;
                LoadMFEInputs();
            }
        }

        private IExperimentContext ExperimentContext { get; set; }

   
        private void InitializeMFE()
        {
            if(Samples != null && Samples.Count > 0)
            {
                MFEExecutor = new MFE(Samples);
            } 
            //else throw exception
        }

        private void LoadMFEInputs()
        {
            if (MFEExecutor != null)
            {
                MFEInputParameters mfeParams = MFEExecutor.GetParameters();
                this.EventAggregator.GetEvent<MFEInputsLoaded>().Publish(mfeParams);
            }
            //else throw exception
        }

        private void runMFE(MFEInputParameters mfeInputs)
        {
            if (MFEExecutor == null)
                return; //TODO - throw exception
            
            List<DataTypes.ICompoundGroup> compoundGroups = MFEExecutor.Execute(mfeInputs);
            this.ExperimentContext.CompoundGroups = compoundGroups;
            EventAggregator.GetEvent<CompoundGroupsGenerated>().Publish(true);
            //SetApplicationState("MFEExecuted");
            ActivateNextWorkflowPhase();
        }

        private void ActivateNextWorkflowPhase()
        {
            var workflowNavigatorViewModel = this.UnityContainer.Resolve<IApplicationLayoutService>().WorkspaceLayoutService.ActiveWorkflowNavigatorViewModel;
            var phases = workflowNavigatorViewModel.WorkflowPhases.GetEnumerator();
            while (phases.MoveNext())
            {
                IWorkflowPhaseViewModel phase = phases.Current;
                if (phase.IsSelected)
                {
                    phases.MoveNext();
                    workflowNavigatorViewModel.ActivatePhase(phases.Current);
                    return;
                }
            }
        }

        private void SetApplicationState(string state)
        {
            var applicationStateService = UnityContainer.Resolve<IApplicationStateService>();
            applicationStateService.ResetApplicationStates();
            applicationStateService.SetApplicationState(state, true, ApplicationStateUpdateMode.Immediate);
            applicationStateService.ApplyApplicationStates();
            var layoutAutomationService = new LayoutAutomationService(UnityContainer);
            if (state.Equals("SamplesAdded"))
            {
                layoutAutomationService.ShowModuleByAssemblyName("Agilent.OpenLab.FeatureExtractionUI");
                layoutAutomationService.ShowModuleByAssemblyName("Agilent.OpenLab.TICPlot");
            }
            else if (state.Equals("ExperimentSetup"))
            {
                layoutAutomationService.ShowModuleByAssemblyName("Agilent.OpenLab.ExperimentSetupParameters");
                layoutAutomationService.ShowModuleByAssemblyName("Agilent.OpenLab.SampleGrouping");
            }           
        }

        private void runMFEWithBusyIndicator(bool isContextUpdated)
        {
            if (isContextUpdated)
            {
                var busyIndicatorService = UnityContainer.Resolve<IBusyIndicatorService>();
                using (new BusyIndicator(busyIndicatorService, "Running MFE", false))
                {
                    runMFE(this.ExperimentContext.MFEInputParameters);
                }
            }
            
        }

    }
}