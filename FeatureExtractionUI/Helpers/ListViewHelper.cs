using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Agilent.OpenLab.FeatureExtractionUI.Helpers
{
    public class ListViewHelper
    {

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListViewHelper),
                new FrameworkPropertyMetadata((IList)null,
                    new PropertyChangedCallback(OnSelectedItemsChanged)));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }


        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListView)d;
            ReSetSelectedItems(listBox);
            listBox.SelectionChanged += delegate
            {
                ReSetSelectedItems(listBox);
            };
        }


        private static void ReSetSelectedItems(ListView listBox)
        {
            IList selectedItems = GetSelectedItems(listBox);
            selectedItems.Clear();
            if (listBox.SelectedItems != null)
            {
                foreach (var item in listBox.SelectedItems)
                    selectedItems.Add(item);
            }
        }
    }
}
