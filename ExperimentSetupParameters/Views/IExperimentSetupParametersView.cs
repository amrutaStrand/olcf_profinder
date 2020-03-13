namespace Agilent.OpenLab.ExperimentSetupParameters
{
    /// <summary>
    /// IExperimentSetupParametersView
    /// </summary>
    public interface IExperimentSetupParametersView
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model. 
        /// </value>
        IExperimentSetupParametersViewModel Model { get; set; }

        #endregion
    }
}