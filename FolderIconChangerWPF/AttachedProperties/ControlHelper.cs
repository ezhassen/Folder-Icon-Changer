using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ControlHelper
    {


        public static bool GetMouseWheelCommandOnControlKey(DependencyObject obj) => (bool)obj.GetValue(MouseWheelCommandOnControlKeyProperty);

        public static void SetMouseWheelCommandOnControlKey(DependencyObject obj, bool value) => obj.SetValue(MouseWheelCommandOnControlKeyProperty, value);

        // Using a DependencyProperty as the backing store for MouseWheelCommandOnControlKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseWheelCommandOnControlKeyProperty =
            DependencyProperty.RegisterAttached("MouseWheelCommandOnControlKey", typeof(bool), typeof(ControlHelper), new PropertyMetadata(true));


        public static ICommand GetMouseWheelCommand(DependencyObject obj) => (ICommand)obj.GetValue(MouseWheelCommandProperty);

        public static void SetMouseWheelCommand(DependencyObject obj, ICommand value) => obj.SetValue(MouseWheelCommandProperty, value);

        // Using a DependencyProperty as the backing store for MouseWheelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseWheelCommandProperty =
            DependencyProperty.RegisterAttached("MouseWheelCommand", typeof(ICommand), typeof(ControlHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnMouseWheelCommandPropertyChanged)));

        private static void OnMouseWheelCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            control.PreviewMouseWheel -= Control_MouseWheel;
            if (e.NewValue is ICommand)
            {
                control.PreviewMouseWheel += Control_MouseWheel;
            }
        }

        private static void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            if (!(GetMouseWheelCommand(control) is ICommand command)) return;
            if (GetMouseWheelCommandOnControlKey(control) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) return;
            if (command.CanExecute(e.Delta)) {
                e.Handled = true;//Handle if there is scroll bar
                command.Execute(e.Delta); }
        }
    }
}
