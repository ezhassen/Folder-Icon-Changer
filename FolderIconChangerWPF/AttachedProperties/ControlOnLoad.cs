using System.Windows;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ControlOnLoad
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ControlOnLoad), new PropertyMetadata(null, new PropertyChangedCallback(OnCommandPropertyChanged)));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            control.Loaded -= Control_Loaded;
            if (e.NewValue is ICommand)
            {
                control.Loaded += Control_Loaded;
            }
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ControlOnLoad), new PropertyMetadata(null));

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            var command = GetCommand(control);
            var commandParam = GetCommandParameter(control);
            if (command?.CanExecute(commandParam) == true) command?.Execute(commandParam);
        }
    }
}