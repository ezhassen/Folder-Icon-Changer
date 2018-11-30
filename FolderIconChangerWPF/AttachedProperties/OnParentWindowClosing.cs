using System.Windows;
using System.Windows.Input;


namespace FolderIconChangerWPF.AttachedProperties
{
    public class OnParentWindowClosing : DependencyObject
    {
        public static ICommand GetOnClosingCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OnClosingCommandProperty);
        }

        public static void SetOnClosingCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(OnClosingCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnClosingCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClosingCommandProperty =
            DependencyProperty.RegisterAttached("OnClosingCommand", typeof(ICommand), typeof(OnParentWindowClosing), new PropertyMetadata(null, OnOnClosingCommandProperty));

        private static void OnOnClosingCommandProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IHandleParentWindowClosing)?.OnClosingCommandChanged(e);
        }



        public static ICommand GetOnClosedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OnClosedCommandProperty);
        }

        public static void SetOnClosedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(OnClosedCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnClosedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClosedCommandProperty =
            DependencyProperty.RegisterAttached("OnClosedCommand", typeof(ICommand), typeof(OnParentWindowClosing), new PropertyMetadata(null, new PropertyChangedCallback(OnClosedCommandPropertyChanged)));

        private static void OnClosedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IHandleParentWindowClosing)?.OnClosedCommandChanged(e);
        }
    }
}
