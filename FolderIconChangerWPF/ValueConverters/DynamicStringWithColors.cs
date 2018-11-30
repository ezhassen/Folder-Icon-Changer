using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Documents;

namespace FolderIconChangerWPF.ValueConverters
{
    public class DynamicStringWithColors : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            var val = value as string;
            if (string.IsNullOrEmpty(val)) return null;
            var sp = val.Split(new string[] { Seperator }, StringSplitOptions.None);

            var textBlock = new TextBlock();
            //textBlock.Inlines.Add(new )
            var inlines = new ObservableCollection<Inline>();
            int index = 0;

            var randomColors = MediaHelper.GetRandomSWMColors(sp.Length);
            //var randomColors = MediaHelper.GetRandomSWMColorsByHSV(sp.Length);

            foreach (var word in sp)
            {
                if (string.IsNullOrEmpty(word)) continue;
                Color scolor = randomColors[index];
                if (Colors != null && index < Colors.Length) scolor = Colors[index];
                //
                inlines.Add(new Run(word) { Foreground = new SolidColorBrush(scolor) });
                //
                index++;
            }
            return inlines;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// First words colors or will gen random color
        /// </summary>
        public Color[] Colors { get; set; }
        public string Seperator { get; set; } = " ";
    }
}
