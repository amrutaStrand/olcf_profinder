using Agilent.MassSpectrometry.DataAnalysis;

using DataTypes;
using Utils;

namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.ObjectModel;

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
        //private IRange rtRange;
        //public IRange RTRange
        //{
        //    get
        //    {
        //        return rtRange;
        //    }
        //    set
        //    {
        //        rtRange = value;
        //        OnPropertyChanged("RTRange");
        //        //MassHunterProcessingPSet.AcqTimeRange = new MinMaxRange(1, value);
        //        //OnPropertyChanged("MassHunterProcessingPSet");
        //    }
        //}

        //private IRange mzRange;
        //public IRange MZRange
        //{
        //    get
        //    {
        //        return mzRange;
        //    }
        //    set
        //    {
        //        mzRange = value;
        //        OnPropertyChanged("MZRange");
        //        ///MassHunterProcessingPSet.MzRange = new MinMaxRange(1, value);
        //        //OnPropertyChanged("MassHunterProcessingPSet");
        //    }
        //}
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
                CombinedChargeState = pSetChargeStateAssignmentPSet.MinimumChargeState + " - " + pSetChargeStateAssignmentPSet.MaximumChargeState;
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

        // Resources.resx file.
        public ObservableCollection<IsotopeModel> IsotopeModels
        {
            get
            {
                return new ObservableCollection<IsotopeModel>()
                        {
                            new IsotopeModel(){ IsotopeModelTypeValue = IsotopeModelType.Unspecified, DisplayText="Unspecified"},
                            new IsotopeModel(){ IsotopeModelTypeValue = IsotopeModelType.Peptides, DisplayText = "Peptides"},
                            new IsotopeModel(){ IsotopeModelTypeValue=IsotopeModelType.CommonOrganicMolecules, DisplayText="Common organic (no halogens)"},
                            new IsotopeModel(){ IsotopeModelTypeValue= IsotopeModelType.Unbaised, DisplayText="Unbaised"},
                            new IsotopeModel(){ IsotopeModelTypeValue = IsotopeModelType.Glycan, DisplayText="Glycans"},
                            new IsotopeModel(){ IsotopeModelTypeValue = IsotopeModelType.Biological, DisplayText="Biological"}
                        };
            }
        }

        private int isotopeTypeInd = 2;
        public int IsotopeTypeInd
        {
            get
            {
                return isotopeTypeInd;
            }
            set
            {
                this.isotopeTypeInd = value;
                OnPropertyChanged("IsotopeTypeInd");
                ChargeStateAssignmentPSet.IsotopeModel = IsotopeModels[value].IsotopeModelTypeValue;
                OnPropertyChanged("ChargeStateAssignmentPSet");
            }
        }

        private IPSetCpdGroupFilters cpdGroupFiltersPset;
        public IPSetCpdGroupFilters CPDGroupFiltersPset
        {
            get
            {
                return cpdGroupFiltersPset;
            }

            set
            {
                cpdGroupFiltersPset = value;
                OnPropertyChanged("CPDGroupFiltersPset");
            }
        }

        public void UpdateInputDefaults(MFEInputParameters allParameters)
        {
            AllInputsParameters = allParameters;
            MassHunterProcessingPSet = AllInputsParameters.AllParameters[MFEPSetKeys.MASS_HUNTER_PROCESSING] as IPSetMassHunterProcessing;
            
            /*
             * Extraction Parameters
             *RTRange = MassHunterProcessingPSet.AcqTimeRange;
             *MZRange = MassHunterProcessingPSet.MzRange;
             * 
             */

            ChargeStateAssignmentPSet = AllInputsParameters.AllParameters[MFEPSetKeys.CHARGE_STATE_ASSIGNMENT] as IPSetChargeStateAssignment;
            IsotopeTypeInd = 2;

            AlignmentInfoPSet = AllInputsParameters.AllParameters[MFEPSetKeys.ALIGNMENT_INFO] as IPSetAlignmentInfo;

            CPDGroupFiltersPset = AllInputsParameters.AllParameters[MFEPSetKeys.MFE_CPD_GROUP_FILTERS] as IPSetCpdGroupFilters;
        }
    }

    public class IsotopeModel
    {
        private IsotopeModelType isotopeModelType;

        public IsotopeModelType IsotopeModelTypeValue
        {
            get { return isotopeModelType; }
            set { isotopeModelType = value; }
        }

        private string displayText;

        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; }
        }

    }
}