using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static partial class TextBoxHelper
    {

        #region Filtering

        public static bool GetIsFiltering(DependencyObject obj) => (bool)obj.GetValue(IsFilteringProperty);

        public static void SetIsFiltering(DependencyObject obj, bool value) => obj.SetValue(IsFilteringProperty, value);

        // Using a DependencyProperty as the backing store for IsFiltering.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFilteringProperty =
            DependencyProperty.RegisterAttached("IsFiltering", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false));

        public static ICommand GetFilterCommand(DependencyObject obj) => (ICommand)obj.GetValue(FilterCommandProperty);

        public static void SetFilterCommand(DependencyObject obj, ICommand value) => obj.SetValue(FilterCommandProperty, value);

        // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterCommandProperty =
            DependencyProperty.RegisterAttached("FilterCommand", typeof(ICommand), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static ICommand GetClearFilterCommand(DependencyObject obj) => (ICommand)obj.GetValue(ClearFilterCommandProperty);

        public static void SetClearFilterCommand(DependencyObject obj, ICommand value) => obj.SetValue(ClearFilterCommandProperty, value);

        // Using a DependencyProperty as the backing store for ClearFilterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearFilterCommandProperty =
            DependencyProperty.RegisterAttached("ClearFilterCommand", typeof(ICommand), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static object GetFilterButtonContent(DependencyObject obj)
        {
            return (object)obj.GetValue(FilterButtonContentProperty);
        }

        public static void SetFilterButtonContent(DependencyObject obj, object value)
        {
            obj.SetValue(FilterButtonContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonContentProperty =
            DependencyProperty.RegisterAttached("FilterButtonContent", typeof(object), typeof(TextBoxHelper), new PropertyMetadata(null));

        //public static System.Windows.Shapes.Path GetSearchIcon() => new System.Windows.Shapes.Path() { Data = System.Windows.Media.Geometry.Parse("M42.5,22A12.5,12.5 0 0,1 55,34.5A12.5,12.5 0 0,1 42.5,47C40.14,47 37.92,46.34 36,45.24L26.97,54.27C25.8,55.44 23.9,55.44 22.73,54.27C21.56,53.1 21.56,51.2 22.73,50.03L31.8,40.96C30.66,39.08 30,36.86 30,34.5A12.5,12.5 0 0,1 42.5,22M42.5,26A8.5,8.5 0 0,0 34,34.5A8.5,8.5 0 0,0 42.5,43A8.5,8.5 0 0,0 51,34.5A8.5,8.5 0 0,0 42.5,26Z") };


        public static bool GetRequireFilterAction(DependencyObject obj)
        {
            return (bool)obj.GetValue(RequireFilterActionProperty);
        }

        public static void SetRequireFilterAction(DependencyObject obj, bool value)
        {
            obj.SetValue(RequireFilterActionProperty, value);
        }

        // Using a DependencyProperty as the backing store for RequireFilterAction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequireFilterActionProperty =
            DependencyProperty.RegisterAttached("RequireFilterAction", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));



        public static object GetClearButtonToolTip(DependencyObject obj)
        {
            return (object)obj.GetValue(ClearButtonToolTipProperty);
        }

        public static void SetClearButtonToolTip(DependencyObject obj, object value)
        {
            obj.SetValue(ClearButtonToolTipProperty, value);
        }

        // Using a DependencyProperty as the backing store for ClearButtonToolTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearButtonToolTipProperty =
            DependencyProperty.RegisterAttached("ClearButtonToolTip", typeof(object), typeof(TextBoxHelper), new PropertyMetadata("Clear"));



        public static object GetFilterButtonToolTip(DependencyObject obj)
        {
            return (object)obj.GetValue(FilterButtonToolTipProperty);
        }

        public static void SetFilterButtonToolTip(DependencyObject obj, object value)
        {
            obj.SetValue(FilterButtonToolTipProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonToolTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonToolTipProperty =
            DependencyProperty.RegisterAttached("FilterButtonToolTip", typeof(object), typeof(TextBoxHelper), new PropertyMetadata("Filter"));


        public static object GetRequireFilterButtonToolTip(DependencyObject obj)
        {
            return (object)obj.GetValue(RequireFilterButtonToolTipProperty);
        }

        public static void SetRequireFilterButtonToolTip(DependencyObject obj, object value)
        {
            obj.SetValue(RequireFilterButtonToolTipProperty, value);
        }

        // Using a DependencyProperty as the backing store for RequireFilterButtonToolTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequireFilterButtonToolTipProperty =
            DependencyProperty.RegisterAttached("RequireFilterButtonToolTip", typeof(object), typeof(TextBoxHelper), new PropertyMetadata("Filter"));




        public static bool GetFilterButtonIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(FilterButtonIsEnabledProperty);
        }

        public static void SetFilterButtonIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(FilterButtonIsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonIsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonIsEnabledProperty =
            DependencyProperty.RegisterAttached("FilterButtonIsEnabled", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(true));


        public static bool GetClearButtonIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearButtonIsEnabledProperty);
        }

        public static void SetClearButtonIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearButtonIsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for ClearButtonIsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearButtonIsEnabledProperty =
            DependencyProperty.RegisterAttached("ClearButtonIsEnabled", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(true));




        public static DataTemplate GetFilterButtonContentTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(FilterButtonContentTemplateProperty);
        }

        public static void SetFilterButtonContentTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(FilterButtonContentTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonContentTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonContentTemplateProperty =
            DependencyProperty.RegisterAttached("FilterButtonContentTemplate", typeof(DataTemplate), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static ControlTemplate GetFilterButtonTemplate(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(FilterButtonTemplateProperty);
        }

        public static void SetFilterButtonTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(FilterButtonTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonTemplateProperty =
            DependencyProperty.RegisterAttached("FilterButtonTemplate", typeof(ControlTemplate), typeof(TextBoxHelper), new PropertyMetadata(null));

        public static Visibility GetFilterButtonVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(FilterButtonVisibilityProperty);
        }

        public static void SetFilterButtonVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(FilterButtonVisibilityProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterButtonVisibilityProperty =
            DependencyProperty.RegisterAttached("FilterButtonVisibility", typeof(Visibility), typeof(TextBoxHelper), new PropertyMetadata(Visibility.Visible));


        public static bool GetFilterByEnterKey(DependencyObject obj)
        {
            return (bool)obj.GetValue(FilterByEnterKeyProperty);
        }

        public static void SetFilterByEnterKey(DependencyObject obj, bool value)
        {
            obj.SetValue(FilterByEnterKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for FilterByEnterKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterByEnterKeyProperty =
            DependencyProperty.RegisterAttached("FilterByEnterKey", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnFilterByEnterKeyPropertyChanged)));

        private static void OnFilterByEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBoxBase tb)) return;
            if (e.NewValue is bool && (bool)e.NewValue)
            {
                //tb.KeyDown += Tb_KeyDown;
                tb.PreviewKeyDown += Tb_KeyDown;
            }
            else
            {
                //tb.KeyDown -= Tb_KeyDown;
                tb.PreviewKeyDown -= Tb_KeyDown;
            }

        }

        private static void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (e.IsRepeat) return;
            if (!(sender is TextBoxBase tb)) return;
            var fcommand = GetFilterCommand(tb);
            if (fcommand.CanExecute(null)) fcommand.Execute(null);
        }

        #endregion


    }
}
