using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ContextMenuHelper
    {

        public static bool GetShowOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowOnClickProperty);
        }

        public static void SetShowOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowOnClickProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowOnClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowOnClickProperty =
            DependencyProperty.RegisterAttached("ShowOnClick", typeof(bool), typeof(ContextMenuHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnShowOnClickPropertyChanged)));

        private static void OnShowOnClickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ButtonBase control)) return;
            if (e.NewValue is bool && (bool)e.NewValue)
            {
                control.Click += Control_Click;
            }
            else
            {
                control.Click -= Control_Click;
            }
        }

        private static void Control_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ButtonBase control) || control.ContextMenu == null) return;
            control.ContextMenu.IsOpen = true;
            //MessageBox.Show("Test");
        }
        //

        public static bool GetOnClickPlacementTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnClickPlacementTargetProperty);
        }

        public static void SetOnClickPlacementTarget(DependencyObject obj, UIElement value)
        {
            obj.SetValue(OnClickPlacementTargetProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnClickPlacementTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClickPlacementTargetProperty =
            DependencyProperty.RegisterAttached("OnClickPlacementTarget", typeof(bool), typeof(ContextMenuHelper), new PropertyMetadata(true));

        public static ContextMenu GetOnClickContextMenu(DependencyObject obj)
        {
            return (ContextMenu)obj.GetValue(OnClickContextMenuProperty);
        }

        public static void SetOnClickContextMenu(DependencyObject obj, ContextMenu value)
        {
            obj.SetValue(OnClickContextMenuProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnClickContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClickContextMenuProperty =
            DependencyProperty.RegisterAttached("OnClickContextMenu", typeof(ContextMenu), typeof(ContextMenuHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnOnClickContextMenuPropertyChanged)));

        private static void OnOnClickContextMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ButtonBase buttonMenu)) return;
            buttonMenu.Click -= ButtonMenu_Click;
            if (e.NewValue is ContextMenu contextMenu)
            {
                buttonMenu.Click += ButtonMenu_Click;
            }
        }

        private static void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ButtonBase buttonMenu)) return;
            var menu = GetOnClickContextMenu(buttonMenu);
            if (menu is null) return;

            //var placementTarrget = GetOnClickPlacementTarget(menu);
            if (GetOnClickPlacementTarget(menu) && menu.PlacementTarget != buttonMenu) menu.PlacementTarget = buttonMenu;

            menu.IsOpen = true;
        }

        //


        public static ContextMenu GetToggleButtonContextMenu(DependencyObject obj)
        {
            return (ContextMenu)obj.GetValue(ToggleButtonContextMenuProperty);
        }

        public static void SetToggleButtonContextMenu(DependencyObject obj, ContextMenu value)
        {
            obj.SetValue(ToggleButtonContextMenuProperty, value);
        }

        // Using a DependencyProperty as the backing store for ToggleButtonContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleButtonContextMenuProperty =
            DependencyProperty.RegisterAttached("ToggleButtonContextMenu", typeof(ContextMenu), typeof(ContextMenuHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnToggleButtonContextMenuPropertyChanged)));

        private static void OnToggleButtonContextMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ToggleButton toggleButton)) return;

            if ((e.OldValue is ContextMenu contextMenu))
            {
                contextMenu.Closed -= ContextMenu_Closed;
            }
            toggleButton.Click -= ToggleButton_Click;
            if ((e.NewValue is ContextMenu newcontextMenu))
            {
                newcontextMenu.Closed += ContextMenu_Closed;
                toggleButton.Click += ToggleButton_Click;
            }
        }

        private static void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu) || contextMenu.PlacementTarget == null) return;
            if (!(contextMenu.PlacementTarget is ToggleButton toggleButton)) return;
            if (toggleButton.IsChecked.GetValueOrDefault(false)) toggleButton.IsChecked = false;
        }

        private static void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton)) return;

            var cMenu = GetToggleButtonContextMenu(toggleButton);
            if (cMenu == null) return;
            if (cMenu.PlacementTarget != toggleButton) cMenu.PlacementTarget = toggleButton;
            var isChecked = toggleButton.IsChecked.GetValueOrDefault(false);
            if (cMenu.IsOpen != isChecked) cMenu.IsOpen = isChecked;
        }





        public static object GetMenuDataContext(DependencyObject obj) => (object)obj.GetValue(MenuDataContextProperty);

        public static void SetMenuDataContext(DependencyObject obj, object value) => obj.SetValue(MenuDataContextProperty, value);

        // Using a DependencyProperty as the backing store for MenuDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuDataContextProperty =
            DependencyProperty.RegisterAttached("MenuDataContext", typeof(object), typeof(ContextMenuHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnMenuDataContextPropertyChanged)));

        private static void OnMenuDataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            if (control.ContextMenu is null) return;
            
            control.ContextMenu.DataContext = GetMenuDataContext(d);
        }


    }
}
