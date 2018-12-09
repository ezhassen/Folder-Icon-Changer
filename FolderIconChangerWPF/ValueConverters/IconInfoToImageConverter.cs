using Ezz_Helper.Drawing.IconsManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.ValueConverters
{
    public class IconInfoToImageConverter : BaseValueConverter
    {
        public int? SizeWidth { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            if (!(value is IconInfo iconInfo)) return null;
            int? sizeW = null;
            if (parameter is int param)
            {
                sizeW = param;
            }
            else if (SizeWidth.HasValue)
            {
                sizeW = SizeWidth.Value;
            }
            //
            if (sizeW.HasValue)
            {
                return iconInfo?.GetBestFitIcon(new System.Drawing.Size(sizeW.Value, sizeW.Value))?.Image?.ToSWBitmapImage();
            }
            else
            {
                return iconInfo?.GetBestFitIcon()?.Image?.ToSWBitmapImage();
            }
        }

    }
}
