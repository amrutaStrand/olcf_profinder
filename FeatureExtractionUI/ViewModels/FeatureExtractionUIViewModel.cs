using Agilent.MassSpectrometry.DataAnalysis;

using DataTypes;
using Utils;

namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Microsoft.Practices.Unity;
    using System;

    #endregion

    /// <summary>
    /// FeatureExtractionUIViewModel
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class FeatureExtractionUIViewModel : BaseViewModel, IFeatureExtractionUIViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureExtractionUIViewModel"/> class.
        /// </summary>
        /// <param name="container">
        /// The container. 
        /// </param>
        /// <remarks>
        /// </remarks>
        public FeatureExtractionUIViewModel(IUnityContainer container)
            : base(container)
        {
            this.View = this.UnityContainer.Resolve<IFeatureExtractionUIView>();
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
        public IFeatureExtractionUIView View { get; set; }

        private MFEInputParameters AllInputsParameters { get; set; }

        private IPSetAlignmentInfo pSetAlignmentInfo;

        public IPSetAlignmentInfo AlignmentInfoPSet {
            get
            {
                return pSetAlignmentInfo;
            } 
            
            set
            {
                pSetAlignmentInfo = value;
                OnPropertyChanged("AlignmentInfoPSet");
            }
            
        }

        private IPSetMassHunterProcessing pSetMassHunterProcessing;

        public IPSetMassHunterProcessing MassHunterProcessingPSet
        {
            get
            {
                return pSetMassHunterProcessing;
            }

            set
            {
                pSetMassHunterProcessing = value;
                OnPropertyChanged("MassHunterProcessingPSet");
            }
        }
        #endregion

        private IPSetChargeStateAssignment pSetChargeStateAssignmentPSet;
        public IPSetChargeStateAssignment ChargeStateAssignmentPSet
        {
            get
            {
                return pSetChargeStateAssignmentPSet;
            }

            set
            {
                pSetChargeStateAssignmentPSet = value;
                OnPropertyChanged("ChargeStateAssignmentPSet");
            }
        }
        private string combinedChargeState;
        public string CombinedChargeState
        {
            get
            {
                if (ChargeStateAssignmentPSet == null)
                    return string.Empty;
                return combinedChargeState;
            }
            set
            {
                combinedChargeState = value;
                OnPropertyChanged("CombinedChargeState");
                string[]  states = value.Split('-');
                ChargeStateAssignmentPSet.MinimumChargeState = long.Parse(states[0]);
                ChargeStateAssignmentPSet.MaximumChargeState = long.Parse(states[1]);
                OnPropertyChanged("ChargeStateAssignmentPSet");

            }
        }
        public void UpdateInputDefaults(MFEInputParameters allParameters)
        {
            AllInputsParameters = allParameters;
            AlignmentInfoPSet = AllInputsParameters.AllParameters[MFEPSetKeys.ALIGNMENT_INFO] as IPSetAlignmentInfo;
            MassHunterProcessingPSet = AllInputsParameters.AllParameters[MFEPSetKeys.MASS_HUNTER_PROCESSING] as IPSetMassHunterProcessing;
            ChargeStateAssignmentPSet = AllInputsParameters.AllParameters[MFEPSetKeys.CHARGE_STATE_ASSIGNMENT] as IPSetChargeStateAssignment;
            CombinedChargeState = ChargeStateAssignmentPSet.MinimumChargeState + " - " + ChargeStateAssignmentPSet.MinimumChargeState;
        }
    }
}