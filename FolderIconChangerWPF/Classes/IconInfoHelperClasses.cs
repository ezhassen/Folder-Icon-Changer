using Ezz_Helper.Drawing.IconsManager;
using Ezz_Helper.WinForms.IconsManager;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static Ezz_Helper.Drawing.IconsManager.IconInfo;

namespace FolderIconChangerWPF.IconInfoCore
{
    //public enum IconViewTypes
    //{
    //    /// <summary>
    //    /// To show icon group of one icon.
    //    /// </summary>
    //    /// <remarks></remarks>
    //    OneIcon = 0,
    //    /// <summary>
    //    /// To show icons of MultiIcons.
    //    /// </summary>
    //    /// <remarks></remarks>
    //    MultiIcons = 1
    //    //Custom
    //}

    public static class IconHelper
    {
        public static byte[] BuildImageBytes(this IconImageInfo IIF)
        {
            //byte[] res = null;

            try
            {
                //entry.HasValue && RawData != null
                if (!IIF.iEntry.HasValue || IIF.RawData is null) return null;
                //BitmapImage Res = null;
                if (IIF.IconImageFormat == IconImageFormat.BMP)
                {
                    using (MemoryStream sMS = new MemoryStream())
                    {
                        Icon TempIcon = IconInfo.IconImageInfo.BuildIcon(IIF.iEntry.Value, IIF.RawData);
                        if (TempIcon != null)
                        {
                            return TempIcon.ToBitmap()?.ToByteArray();
                        }
                    }
                }
                else if (IIF.IconImageFormat == IconImageFormat.PNG)
                {
                    return IIF.RawData;
                    //using (MemoryStream sMS = new MemoryStream(IIF.RawData))
                    //{
                    //    Res = new Bitmap(sMS);
                    //}
                }
                return null;
                //return Res;
            }
            catch (Exception)
            {

                return null;
                //throw;
            }
            //return res;
        }
        public static BitmapImage BuildBitmapImage(this IconImageInfo IIF)
        {
            try
            {
                //entry.HasValue && RawData != null
                if (!IIF.iEntry.HasValue || IIF.RawData is null) return null;
                //BitmapImage Res = null;
                if (IIF.IconImageFormat == IconImageFormat.BMP)
                {
                    Icon TempIcon = IconInfo.IconImageInfo.BuildIcon(IIF.iEntry.Value, IIF.RawData);
                    if (!(TempIcon is null))
                    {
                        //using (MemoryStream sMS = new MemoryStream())
                        //{
                        //    TempIcon.Save(sMS);
                        //    var res = new BitmapImage();
                        //    res.BeginInit();
                        //    res.CacheOption = BitmapCacheOption.OnLoad;
                        //    res.StreamSource = sMS;
                        //    res.EndInit();
                        //    res.Freeze();
                        //    return res;
                        return TempIcon.ToBitmap()?.ToSWBitmapImage();
                        //}
                    }
                }
                else if (IIF.IconImageFormat == IconImageFormat.PNG)
                {
                    return IIF.RawData?.ToBitmapImage();
                    //using (MemoryStream sMS = new MemoryStream(IIF.RawData))
                    //{
                    //    Res = new Bitmap(sMS);
                    //}
                }
                return null;
                //return Res;
            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }

        public static Task<Classes.TaskResult<SelectedIconInfo>> DirectSelectIconFromFileAsync(string FilePath, int iconIndex, CancellationToken cancellationToken = default)
        {
            return Classes.TaskResult.RunAsync(func: (cancel) => DirectSelectIconFromFile(FilePath, iconIndex), cancellationToken: cancellationToken);
        }
        public static SelectedIconInfo DirectSelectIconFromFile(string FilePath, int iconIndex)
        {
            if (!File.Exists(FilePath)) return null;
            var ExIcon = IconExtractor.ExtractIcon(FilePath, iconIndex);
            if (ExIcon == null) return null;
            var SNewIConInfo = new SelectedIconInfo
            {
                FilePath = FilePath,
                SourceIcon = ExIcon,
                Index = iconIndex,
                ICount = IconExtractor.GetIconsCount(FilePath)
            };

            return SNewIConInfo;
        }


        private static OneIconInfo[] oneIconInfoArry;
        public static OneIconInfo[] OneIconInfoArry
        {
            get
            {
                if (oneIconInfoArry != null) return oneIconInfoArry;
                oneIconInfoArry = new OneIconInfo[] {new OneIconInfo(Sizes.px_256x256, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_128x128, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_64x64, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes._16_IndexedColors),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes._16_IndexedColors) };
                return oneIconInfoArry;
            }
        }

        public static OneIconInfo[] BestFitIconsInfo => new OneIconInfo[] {new OneIconInfo(Sizes.px_256x256, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_128x128, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_64x64, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes.Alpha_Channel)};

    }

    //public class SelectedIconInfo
    //{
    //    public bool? DialogResult { get; set; }

    //    public IconViewTypes IconViewType { get; set; }

    //    public Icon SourceIcon { get; set; }

    //    public IconInfo.IconImageInfo SelectedIconImageInfo { get; set; }

    //    /// <summary>
    //    /// Icon Index in the file
    //    /// </summary>
    //    public int Index { get; set; }

    //    public string FilePath { get; set; }

    //    /// <summary>
    //    /// Icons Count in the file
    //    /// </summary>
    //    public int ICount { get; set; }

    //}
    //public sealed class SelectedIcons : List<SelectedIconInfo>
    //{

    //    public bool? DialogResult { get; set; }

    //    public IconViewTypes IconViewType { get; set; }

    //    public string FilePath { get; set; }

    //    public int ICount { get; set; }

    //    public SelectedIconInfo GetFirstItem()
    //    {
    //        if (this.Count == 0) return null;
    //        return this[0];
    //    }
    //    public IconInfo.IconImageInfo GetFirstIconImageInfo()
    //    {
    //        if (this.Count == 0) return null;
    //        return this[0].SelectedIconImageInfo;
    //    }
    //    public List<IconInfo.IconImageInfo> GetAllIconImageInfo()
    //    {
    //        var newL = new List<IconInfo.IconImageInfo>();
    //        foreach (SelectedIconInfo item in this)
    //        {
    //            newL.Add(item.SelectedIconImageInfo);
    //        }
    //        return newL;
    //    }

    //    public Icon GetFirstSourceIcon()
    //    {
    //        if (this.Count == 0) return null;
    //        return this[0].SourceIcon;
    //    }
    //    //public IEnumerable<Icon> GetAllSourceIcons() => this.Select(sii => sii.SourceIcon);
    //    public List<Icon> GetAllSourceIcons()
    //    {
    //        var newL = new List<Icon>();
    //        foreach (SelectedIconInfo item in this)
    //        {
    //            newL.Add(item.SourceIcon);
    //        }
    //        return newL;
    //    }

    //}

}
