using System.Windows;
using System.Windows.Controls;

namespace FolderIconChangerWPF.AttachedProperties
{
    /// <summary>
    /// Handles control unload event when true
    /// </summary>
    public static class HandleControlUnload
    {

        public static bool GetIsHandled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHandledProperty);
        }

        public static void SetIsHandled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHandledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsHandled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHandledProperty =
            DependencyProperty.RegisterAttached("IsHandled", typeof(bool), typeof(HandleControlUnload), new PropertyMetadata(false, new PropertyChangedCallback(IsHandledPropertyChanged)));

        private static void IsHandledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as Control);
            if (control == null) return;
            var val = e.NewValue as bool?;
            if (val.GetValueOrDefault(false))
            {
                control.Unloaded += Control_Unloaded;
            }
            else
            {
                control.Unloaded -= Control_Unloaded;
            }
        }
        private static void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            if (GetIsHandled(sender as Control)) e.Handled = true; //To not override routed event value in case it was/will be changed
        }
    }
}
