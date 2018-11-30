using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class TextBlockBindable
    {
        public static ObservableCollection<Inline> GetInlines(DependencyObject obj)
        {
            return (ObservableCollection<Inline>)obj.GetValue(InlinesProperty);
        }

        public static void SetInlines(DependencyObject obj, ObservableCollection<Inline> value)
        {
            obj.SetValue(InlinesProperty, value);
        }

        public static readonly DependencyProperty InlinesProperty =
            DependencyProperty.RegisterAttached("Inlines", typeof(ObservableCollection<Inline>), typeof(TextBlockBindable), new PropertyMetadata(null, OnInlinesChanged));

        private static void OnInlinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var Target = d as TextBlock;

            if (Target != null)
            {
                Target.Inlines.Clear();
                var newVal = e.NewValue as System.Collections.ICollection;

                if (newVal == null || newVal.Count == 0) return;
                Target.Inlines.AddRange(newVal);
            }
        }
    }

    public static class TextBlockDynamicStringColors
    {

        public static Color[] GetColors(DependencyObject obj)
        {
            return (Color[])obj.GetValue(ColorsProperty);
        }

        public static void SetColors(DependencyObject obj, Color[] value)
        {
            obj.SetValue(ColorsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Colors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.RegisterAttached("Colors", typeof(Color[]), typeof(TextBlockDynamicStringColors), new PropertyMetadata(null));

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TextBlockDynamicStringColors), new PropertyMetadata(null, new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as TextBlock;
            if (tb == null) return;
            tb.Inlines.Clear();
            var val = e.NewValue as string;
            if (string.IsNullOrEmpty(val)) return;
            var sp = val.Split(new string[] { " " }, StringSplitOptions.None);

            var textBlock = new TextBlock();
            //textBlock.Inlines.Add(new )
            //var inlines = new ObservableCollection<Inline>();
            int index = 0;

            var randomColors = MediaHelper.GetRandomSWMColors(sp.Length);
            var customColors = GetColors(d);
            //var randomColors = MediaHelper.GetRandomSWMColorsByHSV(sp.Length);

            foreach (var word in sp)
            {
                if (string.IsNullOrEmpty(word)) continue;
                var scolor = randomColors[index];
                if (customColors != null && index < customColors.Length) scolor = customColors[index];
                //
                //inlines.Add(new Run(word) { Foreground = new SolidColorBrush(scolor) });
                if (index != 0) tb.Inlines.Add(" ");
                tb.Inlines.Add(new Run(word) { Foreground = new SolidColorBrush(scolor) });
                //
                index++;
            }


        }
    }
}
