using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.IconInfoCore;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.ValueConverters
{
    /// <summary>
    /// Async load image (Use Binding.IsAsync = true)
    /// </summary>
    public class IconInfoToImageAsyncConverter : BaseValueConverter
    {
        public int? SizeWidth { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            if (!(value is IconInfo iconInfo)) return null;
            var paraStr = parameter as string;

            int? sizeW = null;

            if ((!string.IsNullOrEmpty(paraStr)) && paraStr.IsNumeric())
            {
                var paraInt = paraStr.ValInt();
                sizeW = paraInt > 0 ? paraStr.ValInt() : SizeWidth;
            }
            else// if (SizeWidth.HasValue)
            {
                sizeW = SizeWidth;//.Value;
            }
            //
            if (sizeW.HasValue)
            {
                return Task.Run(() => iconInfo?.GetBestFitIcon(new System.Drawing.Size(sizeW.Value, sizeW.Value))?.BuildBitmapImage());
            }
            else
            {
                return Task.Run(() => iconInfo?.GetBestFitIcon()?.BuildBitmapImage());
            }
        }
    }
    /// <summary>
    /// Async load image (Use Binding.IsAsync = true)
    /// </summary>
    public class IconImageInfoToImageAsyncConverter : BaseValueConverter
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            if (!(value is IconInfo.IconImageInfo iconImageInfo)) return null;

            return Task.Run(() => iconImageInfo?.BuildBitmapImage());
        }
    }
}