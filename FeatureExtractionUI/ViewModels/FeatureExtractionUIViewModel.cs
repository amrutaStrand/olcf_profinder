﻿using Agilent.MassSpectrometry.DataAnalysis;

using DataTypes;
using Utils;

namespace Agilent.OpenLab.FeatureExtractionUI
{
    using Agilent.MassSpectrometry.MIDAC;
    #region

    using Agilent.OpenLab.Framework.UI.Module;
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
                CombinedRTRange = MassHunterProcessingPSet.AcqTimeRange.Start + " - " + MassHunterProcessingPSet.AcqTimeRange.End;
                OnPropertyChanged("CombinedRTRange");

                PositiveIonSpecies = GetFormulae('+');
                NegativeIonSpecies = GetFormulae('-');
                NeutralIonSpecies = GetFormulae('*');
            }
        }

        private List<string> _positiveIonSpecies;
        public List<string> PositiveIonSpecies
        {
            get
            {
                    return _positiveIonSpecies;
            }
            set
            {
                _positiveIonSpecies = value;
                OnPropertyChanged("PositiveIons");
            }
        }

        private List<string> _negativeIonSpecies;
        public List<string> NegativeIonSpecies
        {
            get
            {
                return _negativeIonSpecies;
            }
            set
            {
                _negativeIonSpecies = value;
                OnPropertyChanged("NegativeIons");
            }
        }

        private List<string> _neutralIonSpecies;
        public List<string> NeutralIonSpecies
        {
            get
            {
                return _neutralIonSpecies;
            }
            set
            {
                _neutralIonSpecies = value;
                OnPropertyChanged("NeutralIonSpecies");
            }
        }

        private string rtRange;
        public string CombinedRTRange
        {
            get
            {
                if (MassHunterProcessingPSet == null)
                    return string.Empty;
                return rtRange;
            }
            set
            {
                rtRange = value;
                OnPropertyChanged("CombinedRTRange");
                string[] range_t = value.Split('-');
                double minRange = double.Parse(range_t[0].Trim());
                double maxRange = double.Parse(range_t[1].Trim());
                if (minRange > 0.0 && maxRange > 0.0)
                {
                    IRange range = new MinMaxRange(minRange, maxRange);
                    range.DataValueType = DataValueType.AcqTime;
                    range.DataUnit = DataUnit.Minutes;
                    MassHunterProcessingPSet.AcqTimeRange = range;
                    OnPropertyChanged("MassHunterProcessingPSet");
                }

            }
        }

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
        private int _isSuccess;
        public int PeakFilterStatus {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
                OnPropertyChanged("PeakFilterStatus");
                if (value == 4)
                {
                    MassHunterProcessingPSet.PeakFilterType = PeakFilterType.SignalToNoiseThreshold;
                }
                else if (value == 8)
                {
                    MassHunterProcessingPSet.PeakFilterType = PeakFilterType.PeakHeightAbsThreshold;
                }
                OnPropertyChanged("MassHunterProcessingPSet");
            }
        }
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
                string[] states = value.Split('-');
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
            
            PeakFilterStatus = 8;
            ChargeStateAssignmentPSet = AllInputsParameters.AllParameters[MFEPSetKeys.CHARGE_STATE_ASSIGNMENT] as IPSetChargeStateAssignment;
            IsotopeTypeInd = 2;
            AlignmentInfoPSet = AllInputsParameters.AllParameters[MFEPSetKeys.ALIGNMENT_INFO] as IPSetAlignmentInfo;

            CPDGroupFiltersPset = AllInputsParameters.AllParameters[MFEPSetKeys.MFE_CPD_GROUP_FILTERS] as IPSetCpdGroupFilters;
        }

        private List<string> GetFormulae(char mode)
        {
            List<string> formulae = new List<string>();
            IEnumerator<ISpeciesDefinition> ps = null;
            if (mode == '+')
            {
                ps = MassHunterProcessingPSet.PositiveSpeciesDefinitions.GetEnumerator();
            } else if(mode == '-')
            {
                ps = MassHunterProcessingPSet.NegativeSpeciesDefinitions.GetEnumerator();
            }
            else
            {
                ps = MassHunterProcessingPSet.NeutralSpeciesDefinitions.GetEnumerator();
            }
             
            while (ps.MoveNext())
            {
                if (ps.Current is ISpeciesDefinition)
                {
                    ISpeciesDefinition speciesDefinition = ps.Current as ISpeciesDefinition;
                    Console.WriteLine(speciesDefinition.ModifierFormula + "   " +speciesDefinition.ShorthandSpeciesFormula);
                    if (speciesDefinition.ModifierFormula.Trim().Length > 0)
                        formulae.Add(speciesDefinition.ModifierFormula + "" + mode);
                    else
                        formulae.Add(speciesDefinition.NeutralLoss);
                }
                Console.WriteLine(ps.Current);
            }
            return formulae;
        }

        private string ValidateAllInputs()
        {
            string errorMsg = GetValidationMessageRecursive(MassHunterProcessingPSet);
            if (errorMsg != null && errorMsg.Trim().Length > 0) return errorMsg;

            errorMsg = GetValidationMessageRecursive(ChargeStateAssignmentPSet);
            if (errorMsg != null && errorMsg.Trim().Length > 0) return errorMsg;

            errorMsg = GetValidationMessageRecursive(AlignmentInfoPSet);
            if (errorMsg != null && errorMsg.Trim().Length > 0) return errorMsg;

            return null;
        }

        private string GetValidationMessageRecursive(object parameterObject)
        {
            if (parameterObject is IParameterSet)
            {
                IParameterSet paramSet = parameterObject as IParameterSet;
                IEnumerator enumerator = paramSet.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is IParameter)
                    {
                        string childMsg = GetValidationMessageRecursive(enumerator.Current);
                        if (childMsg != null && childMsg.Trim().Length > 0)
                            return childMsg;
                    }
                }

            } else if (parameterObject is IParameter)
            {
                IParameter param = parameterObject as IParameter;
                if (!param.Validate())
                {
                    string errorMsg = param.ValidationMessage;
                    if (errorMsg != null && errorMsg.Trim().Length > 0) return errorMsg;
                }

            }
            return null;
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