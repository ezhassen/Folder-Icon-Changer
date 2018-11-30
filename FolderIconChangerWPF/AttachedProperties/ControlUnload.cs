using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ControlUnload
    {
        #region Private Methods

        private static void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            var unloadCommand = GetUnloadCommand(sender as Control);
            if (unloadCommand != null)
            {
                if (unloadCommand.CanExecute(null))
                {
                    unloadCommand.Execute(null);
                }
                else
                {
                    e.Handled = true;
                    var onHandledCommand = GetOnUnloadHandledCommand(sender as Control);
                    if (onHandledCommand != null && onHandledCommand.CanExecute(null)) onHandledCommand.Execute(null);
                }
            }
        }

        private static void OnUnloadCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as Control);
            if (control == null) return;
            var command = e.NewValue as ICommand;

            if (command != null)
            {
                control.Unloaded += Control_Unloaded;
            }
            else
            {
                control.Unloaded += Control_Unloaded;
            }
        }

        #endregion Private Methods

        #region Public Fields

        // Using a DependencyProperty as the backing store for OnUnloadHandledCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnUnloadHandledCommandProperty =
            DependencyProperty.RegisterAttached("OnUnloadHandledCommand", typeof(ICommand), typeof(ControlUnload), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for UnloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnloadCommandProperty =
            DependencyProperty.RegisterAttached("UnloadCommand", typeof(ICommand), typeof(ControlUnload), new PropertyMetadata(null, new PropertyChangedCallback(OnUnloadCommandPropertyChanged)));

        #endregion Public Fields

        #region Public Methods

        public static ICommand GetOnUnloadHandledCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OnUnloadHandledCommandProperty);
        }

        public static ICommand GetUnloadCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(UnloadCommandProperty);
        }

        public static void SetOnUnloadHandledCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(OnUnloadHandledCommandProperty, value);
        }

        public static void SetUnloadCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(UnloadCommandProperty, value);
        }

        #endregion Public Methods
    }
}