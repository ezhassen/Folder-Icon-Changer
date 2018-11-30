using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class WindowClosing
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
            DependencyProperty.RegisterAttached("OnClosingCommand", typeof(ICommand), typeof(WindowClosing), new PropertyMetadata(null, new PropertyChangedCallback(OnClosingCommandPropertyChanged)));

        private static void OnClosingCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (d as Window);
            if (window == null) return;
            var command = e.NewValue as ICommand;

            if (command != null)
            {
                window.Closing += Window_Closing;
            }
            else
            {
                window.Closing -= Window_Closing;
            }
        }

        public static ICommand GetClosedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosedCommandProperty);
        }

        public static void SetClosedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosedCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for ClosedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClosedCommandProperty =
            DependencyProperty.RegisterAttached("ClosedCommand", typeof(ICommand), typeof(WindowClosing), new PropertyMetadata(null, OnClosedCommandChanged));

        private static void OnClosedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (d as Window);
            if (window == null) return;
            var command = e.NewValue as ICommand;

            if (command != null)
            {
                window.Closed += Window_Closed;
            }
            else
            {
                window.Closed -= Window_Closed;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            var closedCommand = GetClosedCommand(sender as Window);
            if (closedCommand != null && closedCommand.CanExecute(null)) closedCommand.Execute(null);
        }

        private static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var OnClosingCommand = GetOnClosingCommand(sender as Window);
            if (OnClosingCommand != null)
            {
                if (OnClosingCommand.CanExecute(e))
                {
                    OnClosingCommand.Execute(e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
