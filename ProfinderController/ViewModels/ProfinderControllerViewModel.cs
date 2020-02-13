namespace Agilent.OpenLab.ProfinderController
{
    #region

    using Agilent.OpenLab.Framework.UI.Module;

    using Microsoft.Practices.Unity;

    #endregion

    public partial class ProfinderControllerViewModel : BaseViewModel, IProfinderControllerViewModel
    {
        #region Constructors and Destructors

        string filePath = null;
        /// <summary>
        /// Holds Cef file path
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                //parseFile();
            }
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