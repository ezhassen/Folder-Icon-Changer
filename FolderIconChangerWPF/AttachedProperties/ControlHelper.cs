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


        public static object GetTag2(DependencyObject obj) => (object)obj.GetValue(Tag2Property);

        public static void SetTag2(DependencyObject obj, object value) => obj.SetValue(Tag2Property, value);

        // Using a DependencyProperty as the backing store for Tag2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Tag2Property =
            DependencyProperty.RegisterAttached("Tag2", typeof(object), typeof(ControlHelper), new PropertyMetadata(null));



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

        #region ControlOnLoad


        public static ICommand GetOnLoadCommand(DependencyObject obj) => (ICommand)obj.GetValue(OnLoadCommandProperty);

        public static void SetOnLoadCommand(DependencyObject obj, ICommand value) => obj.SetValue(OnLoadCommandProperty, value);

        // Using a DependencyProperty as the backing store for OnLoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnLoadCommandProperty =
            DependencyProperty.RegisterAttached("OnLoadCommand", typeof(ICommand), typeof(ControlHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnOnLoadCommandPropertyChanged)));

        private static void OnOnLoadCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            control.Loaded -= Control_Loaded;
            if (e.NewValue is ICommand)
            {
                control.Loaded += Control_Loaded;
            }
        }


        public static object GetOnLoadCommandParameter(DependencyObject obj) => (object)obj.GetValue(OnLoadCommandParameterProperty);

        public static void SetOnLoadCommandParameter(DependencyObject obj, object value) => obj.SetValue(OnLoadCommandParameterProperty, value);

        // Using a DependencyProperty as the backing store for OnLoadCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnLoadCommandParameterProperty =
            DependencyProperty.RegisterAttached("OnLoadCommandParameter", typeof(object), typeof(ControlHelper), new PropertyMetadata(null));


        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            var command = GetOnLoadCommand(control);
            var commandParam = GetOnLoadCommandParameter(control);
            if (command?.CanExecute(commandParam) == true) command?.Execute(commandParam);
        }

        #endregion

        #region ControlUnload


        public static ICommand GetOnUnloadedCommand(DependencyObject obj) => (ICommand)obj.GetValue(OnUnloadedCommandProperty);

        public static void SetOnUnloadedCommand(DependencyObject obj, ICommand value) => obj.SetValue(OnUnloadedCommandProperty, value);

        // Using a DependencyProperty as the backing store for OnUnloadedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnUnloadedCommandProperty =
            DependencyProperty.RegisterAttached("OnUnloadedCommand", typeof(ICommand), typeof(ControlHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnOnUnloadedCommandPropertyChanged)));

        private static void OnOnUnloadedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
                control.Unloaded -= Control_Unloaded;
            if (e.NewValue is ICommand)
            {
                control.Unloaded += Control_Unloaded;
            }
        }
        private static void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement control)) return;
            var unloadCommand = GetOnUnloadedCommand(control);
            if (unloadCommand != null)
            {
                if (unloadCommand.CanExecute(e))
                {
                    unloadCommand.Execute(e);
                }
                else
                {
                    e.Handled = true;
                    var onHandledCommand = GetOnUnloadHandledCommand(sender as Control);
                    if (onHandledCommand != null && onHandledCommand.CanExecute(null)) onHandledCommand.Execute(null);
                }
            }
        }



        public static ICommand GetOnUnloadHandledCommand(DependencyObject obj) => (ICommand)obj.GetValue(OnUnloadHandledCommandProperty);

        public static void SetOnUnloadHandledCommand(DependencyObject obj, ICommand value) => obj.SetValue(OnUnloadHandledCommandProperty, value);

        // Using a DependencyProperty as the backing store for OnUnloadHandledCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnUnloadHandledCommandProperty =
            DependencyProperty.RegisterAttached("OnUnloadHandledCommand", typeof(ICommand), typeof(ControlHelper), new PropertyMetadata(null));



        #endregion


    }
}
