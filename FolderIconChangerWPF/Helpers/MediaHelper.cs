using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using System.Threading.Tasks;

namespace FolderIconChangerWPF
{
    public static class MediaHelper
    {

        //public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    if (imageIn == null) return null;
        //    byte[] res;
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        res = ms.ToArray();
        //    }
        //    return res;
        //}

        //public static Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    if (byteArrayIn == null) return null;
        //    Image res;
        //    using (var ms = new MemoryStream(byteArrayIn))
        //    {
        //        res = System.Drawing.Image.FromStream(ms);
        //    }
        //    return res;
        //}

        public static byte[] ToByteArray(this System.Drawing.Bitmap imageIn)
        {
            if (imageIn is null) return null;
            byte[] res = null;
            using (var ms = new MemoryStream())
            {
                try
                {
                    //var rFormat = imageIn.RawFormat;
                    //imageIn.Save(ms, new System.Drawing.Imaging.ImageFormat(rFormat.Guid));
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                res = ms.ToArray();
            }
            return res;
        }
        public static void SaveTo(this BitmapImage imageIn, Stream stream, BitmapEncoder bitmapEncoder)
        {
            if (imageIn is null) return;
            bitmapEncoder.Frames.Add(BitmapFrame.Create(imageIn));
            bitmapEncoder.Save(stream);
        }
        public static byte[] ToByteArray(this BitmapImage imageIn)
        {
            return ToByteArray(imageIn, new JpegBitmapEncoder());
        }
        public static byte[] ToByteArray(this BitmapImage imageIn, BitmapEncoder bitmapEncoder)
        {
            if (imageIn is null) return null;
            byte[] res = null;
            bitmapEncoder.Frames.Add(BitmapFrame.Create(imageIn));
            using (var ms = new MemoryStream())
            {
                bitmapEncoder.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        //public static Task<BitmapImage> ToBitmapImageAsync(this byte[] byteArrayIn)
        //{
        //    return null;
        //    //var tcs = new TaskCompletionSource<BitmapImage>();
        //    //var bitmapImage = new BitmapImage();

        //    //bitmapImage.DownloadCompleted += (o, e) =>
        //    //{
        //    //    tcs.SetResult(bitmapImage);
        //    //};

        //    //bitmapImage.DownloadFailed += (o, e) =>
        //    //{
        //    //    tcs.SetResult(null);
        //    //};

        //    //using (var ms = new MemoryStream(byteArrayIn))
        //    //{
        //    //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //    //    bitmapImage.StreamSource = ms;
        //    //}

        //    //return tcs.Task;
        //}

        /// <summary>
        /// Converts <see cref="byte[]"/> to BitmapImage.
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <param name="freeze">calls image.Freeze() to make it cross-thread accessible</param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImage(this byte[] byteArrayIn, bool freeze = true)
        {
            if (byteArrayIn == null) return null;
            var res = new BitmapImage();
            using (var ms = new MemoryStream(byteArrayIn))
            {
                res.BeginInit();
                res.CacheOption = BitmapCacheOption.OnLoad;
                res.StreamSource = ms;
                res.EndInit();
                if (freeze) res.Freeze();
            }
            return res;
        }
        public static Task<BitmapImage> ToSWBitmapImageAsync(this System.Drawing.Bitmap SDBitmap) => Task.Run(() => SDBitmap?.ToByteArray()?.ToBitmapImage());

        public static BitmapImage ToSWBitmapImage(this System.Drawing.Bitmap SDBitmap) => SDBitmap?.ToByteArray()?.ToBitmapImage();


        public static Task<bool> SetSourceAsync(this BitmapImage image, Stream stream)
        {
            var tcs = new TaskCompletionSource<bool>();

            image.DownloadCompleted += (o, e) =>
            {
                tcs.SetResult(true);
            };

            image.DownloadFailed += (o, e) =>
            {
                tcs.SetResult(false);
            };

            image.StreamSource = stream;

            return tcs.Task;
        }


        public static string ToBase64(this BitmapImage imageIn, bool throwOnException = false)
        {
            if (imageIn is null) return null;
            try
            {
                return Convert.ToBase64String(imageIn.ToByteArray());
            }
            catch (Exception)
            {
                if (throwOnException) throw;
                return null;
            }
        }
        public static BitmapImage FromBase64(string base64, bool throwOnException = false)
        {
            try
            {
                var bi = new BitmapImage();

                bi.BeginInit();
                bi.StreamSource = new MemoryStream(Convert.FromBase64String(base64));
                bi.EndInit();
                return bi;
            }
            catch (Exception)
            {
                if (throwOnException) throw;
                return null;
            }
        }

        #region Color

        public static SDColor SDColorFromHEX(string hex) => System.Drawing.ColorTranslator.FromHtml(hex);
        public static SWMColor SWMColorFromHEX(string hex) => (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hex);
        public static SWMColor ToSWMColor(this SDColor color)
        {
            return SWMColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static SDColor ToSDColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
        //
        public static void ColorToHSV(this SWMColor color, out double hue, out double saturation, out double value)
        {
            var sdColor = color.ToSDColor();
            ColorToHSV(sdColor, out hue, out saturation, out value);
        }
        public static void ColorToHSV(this SDColor color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static SWMColor SWMColorFromHSV(double hue, double saturation, double Brightness)
        {
            //return SDColorFromHSV(hue, saturation, Brightness).ToSWMColor();
            //
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            Brightness = Brightness * 255;
            var v = (float)Brightness;
            var p = (float)(Brightness * (1 - saturation));
            var q = (float)(Brightness * (1 - f * saturation));
            var t = (float)(Brightness * (1 - (1 - f) * saturation));

            if (hi == 0)
                return SWMColor.FromScRgb(255, v, t, p);
            else if (hi == 1)
                return SWMColor.FromScRgb(255, q, v, p);
            else if (hi == 2)
                return SWMColor.FromScRgb(255, p, v, t);
            else if (hi == 3)
                return SWMColor.FromScRgb(255, p, q, v);
            else if (hi == 4)
                return SWMColor.FromScRgb(255, t, p, v);
            else
                return SWMColor.FromScRgb(255, v, p, q);
        }
        public static SDColor SDColorFromHSV(double hue, double saturation, double Brightness)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            Brightness = Brightness * 255;
            int v = Convert.ToInt32(Brightness);
            int p = Convert.ToInt32(Brightness * (1 - saturation));
            int q = Convert.ToInt32(Brightness * (1 - f * saturation));
            int t = Convert.ToInt32(Brightness * (1 - (1 - f) * saturation));

            if (hi == 0)
                return SDColor.FromArgb(255, v, t, p);
            else if (hi == 1)
                return SDColor.FromArgb(255, q, v, p);
            else if (hi == 2)
                return SDColor.FromArgb(255, p, v, t);
            else if (hi == 3)
                return SDColor.FromArgb(255, p, q, v);
            else if (hi == 4)
                return SDColor.FromArgb(255, t, p, v);
            else
                return SDColor.FromArgb(255, v, p, q);
        }
        /// <summary>
        /// GetRandomColorsByHSV
        /// </summary>
        /// <param name="NumberOfColours">NumberOfColours</param>
        /// <param name="saturation">From 0 to 255</param>
        /// <param name="HueSeparation">From 0 to 360</param>
        /// <param name="Brightness">From 0 to 255</param>
        /// <returns></returns>
        public static List<SWMColor> GetRandomSWMColorsByHSV(int NumberOfColours, double HueSeparation = 51.5, double saturation = 127.5, double Brightness = 229.5)
        {
            //https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            //////saturation = 0.5 * 255; //assuming we want range 0-255...
            //////Brightness = 0.9 * 255;
            //////NumberOfColours = 7;
            //////HueSeparation = 360 / 7;
            var res = new List<SWMColor>();
            for (int i = 0; i < NumberOfColours; i++) res.Add(SWMColorFromHSV(HueSeparation * i, saturation, Brightness));
            return res;
            //return GetRandomSDColorsByHSV(NumberOfColours, HueSeparation, saturation, Brightness).Select(sdc => sdc.ToSWMColor()).ToList();
        }
        public static List<SWMColor> GetRandomSWMColors(int NumberOfColours, int redMin = 0, int redMax = 256, int greenMin = 0, int greenMax = 256, int blueMin = 0, int blueMax = 256)
        {
            Random rnd = new Random();
            var res = new List<SWMColor>();
            for (int i = 0; i < NumberOfColours; i++) res.Add(SWMColor.FromArgb(byte.MaxValue, (byte)rnd.Next(redMin, redMax), (byte)rnd.Next(greenMin, greenMax), (byte)rnd.Next(blueMin, blueMax)));
            return res;
        }
        /// <summary>
        /// GetRandomColorsByHSV
        /// </summary>
        /// <param name="NumberOfColours">NumberOfColours</param>
        /// <param name="saturation">From 0 to 255</param>
        /// <param name="HueSeparation">From 0 to 360</param>
        /// <param name="Brightness">From 0 to 255</param>
        /// <returns></returns>
        public static List<SDColor> GetRandomSDColorsByHSV(int NumberOfColours, double HueSeparation = 51.5, double saturation = 127.5, double Brightness = 229.5)
        {
            //https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            //saturation = 0.5 * 255; //assuming we want range 0-255...
            //Brightness = 0.9 * 255
            //NumberOfColours = 7
            //HueSeparation = 360 / 7
            var res = new List<SDColor>();
            for (int i = 0; i < NumberOfColours; i++) res.Add(SDColorFromHSV(HueSeparation * i, saturation, Brightness));
            return res;
        }
        public static List<SDColor> GetRandomSDColors(int NumberOfColours, int redMin = 0, int redMax = 256, int greenMin = 0, int greenMax = 256, int blueMin = 0, int blueMax = 256)
        {
            Random rnd = new Random();
            var res = new List<SDColor>();
            for (int i = 0; i < NumberOfColours; i++) res.Add(SDColor.FromArgb(rnd.Next(redMin, redMax), rnd.Next(greenMin, greenMax), rnd.Next(blueMin, blueMax)));
            return res;
        }

        #endregion
    }
}
