using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class ButtonHelper
    {

        public static object GetIcon(DependencyObject obj)
        {
            return (object)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, object value)
        {
            obj.SetValue(IconProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(ButtonHelper), new PropertyMetadata(null));

        public static object GetTextObject(DependencyObject obj)
        {
            return (object)obj.GetValue(TextObjectProperty);
        }

        public static void SetTextObject(DependencyObject obj, object value)
        {
            obj.SetValue(TextObjectProperty, value);
        }

        // Using a DependencyProperty as the backing store for TextObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextObjectProperty =
            DependencyProperty.RegisterAttached("TextObject", typeof(object), typeof(ButtonHelper), new PropertyMetadata(null));

    }
}
