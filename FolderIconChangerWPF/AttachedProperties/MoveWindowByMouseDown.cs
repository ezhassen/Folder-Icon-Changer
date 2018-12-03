using System.Windows;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class MoveWindowByMouseDown
    {
        public static Window GetWindow(DependencyObject obj)
        {
            return (Window)obj.GetValue(WindowProperty);
        }

        public static void SetWindow(DependencyObject obj, Window value)
        {
            obj.SetValue(WindowProperty, value);
        }

        // Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowProperty =
            DependencyProperty.RegisterAttached("Window", typeof(Window), typeof(MoveWindowByMouseDown), new PropertyMetadata(null, new PropertyChangedCallback(OnWindowPropertyChanged)));

        private static void OnWindowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            if (e.NewValue is Window)
            {
                control.MouseDown += Control_MouseDown;
            }
            else
            {
                control.MouseDown -= Control_MouseDown;
            }
        }

        private static void Control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            GetWindow(control)?.MoveWindowByMouseDown();
        }
    }
}