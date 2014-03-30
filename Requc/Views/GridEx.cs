using System.Windows;
using System.Windows.Controls;

namespace Requc.Views
{
    public class GridEx : Grid
    {
        public static DependencyProperty ItemsPerColumnProperty =
            DependencyProperty.Register("ItemsPerColumn", typeof(int), typeof(GridEx), new UIPropertyMetadata(0, OnItemsPerColumnPropertyChanged));

        public int ItemsPerColumn
        {
            get { return (int)GetValue(ItemsPerColumnProperty); }
            set { SetValue(ItemsPerColumnProperty, value); }
        }

        private static void OnItemsPerColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var itemsPerColumn = (int)e.NewValue;

            // construct the required row definitions
            grid.LayoutUpdated += (s, e2) =>
            {
                var childCount = grid.Children.Count;

                // add the required number of row definitions
                var columnsToAdd = (childCount - grid.ColumnDefinitions.Count) / itemsPerColumn;
                for (int column = 0; column < columnsToAdd; column++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                // set the row property for each chid
                for (int i = 0; i < childCount; i++)
                {
                    var child = grid.Children[i] as FrameworkElement;
                    SetColumn(child, i / itemsPerColumn);
                }
            };
        }
    }
}
