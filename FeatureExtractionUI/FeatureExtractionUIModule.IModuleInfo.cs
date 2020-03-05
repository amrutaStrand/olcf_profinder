namespace Agilent.OpenLab.FeatureExtractionUI
{
    #region

    using System;
    using System.Windows.Media.Imaging;

    using Agilent.OpenLab.FeatureExtractionUI.Properties;

    #endregion

    partial class FeatureExtractionUIModule
    {
        #region Public Properties

        /// <summary>
        /// Gets the caption.
        /// </summary>
        public override string Caption
        {
            get
            {
                return Resources.FeatureExtractionUICaption;
            }
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public override BitmapImage Image
        {
            get
            {
                return
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/Agilent.OpenLab.FeatureExtractionUI;component/Images/TestImage.png"));
            }
        }

        /// <summary>
        /// Gets the key tip.
        /// </summary>
        public override string KeyTip
        {
            get
            {
                return "M";
            }
        }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        public override string Tooltip
        {
            get
            {
                return Resources.FeatureExtractionUIHint;
            }
        }

        #endregion
    }
}