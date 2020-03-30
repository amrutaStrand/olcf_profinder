namespace Agilent.OpenLab.CompoundGroupsTable
{
    #region

    using System;
    using System.Windows.Media.Imaging;

    using Agilent.OpenLab.CompoundGroupsTable.Properties;
    using DataTypes;

    #endregion

    partial class CompoundGroupsTableModule
    {
        #region Public Properties

        /// <summary>
        /// Gets the caption.
        /// </summary>
        public override string Caption
        {
            get
            {
                if (ExperimentContext.CompoundGroups != null)
                {
                    return Resources.CompoundGroupsTableCaption + "(" +ExperimentContext.CompoundGroups.Count + ")";
                }
                return Resources.CompoundGroupsTableCaption;
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
                            "pack://application:,,,/Agilent.OpenLab.CompoundGroupsTable;component/Images/TestImage.png"));
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
                return Resources.CompoundGroupsTableHint;
            }
        }

        #endregion
    }
}