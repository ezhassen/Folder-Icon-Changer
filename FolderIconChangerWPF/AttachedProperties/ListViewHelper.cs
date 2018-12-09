using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ListViewHelper
    {


        public static ListBox GetDeselectOnMouseDown(DependencyObject obj)
        {
            return (ListBox)obj.GetValue(DeselectOnMouseDownProperty);
        }

        public static void SetDeselectOnMouseDown(DependencyObject obj, ListBox value)
        {
            obj.SetValue(DeselectOnMouseDownProperty, value);
        }

        // Using a DependencyProperty as the backing store for DeselectOnMouseDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeselectOnMouseDownProperty =
            DependencyProperty.RegisterAttached("DeselectOnMouseDown", typeof(ListBox), typeof(ListViewHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnDeselectOnMouseDownPropertyChanged)));

        private static void OnDeselectOnMouseDownPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            control.MouseDown -= Control_MouseDown;
            if (e.NewValue is ListBox)
            {
                control.MouseDown += Control_MouseDown;
            }
        }
        private static void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            if (e.ChangedButton == MouseButton.Left)
            {
                if (!(GetDeselectOnMouseDown(control) is ListBox lv)) return;
                if (lv.SelectionMode == SelectionMode.Single)
                {
                    lv.SelectedItem = null;
                }
                else
                {
                    lv.SelectedItems?.Clear();
                }
            }
        }

    }
}
