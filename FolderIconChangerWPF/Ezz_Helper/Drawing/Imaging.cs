using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ezz_Helper.Drawing.ImagePixelEnumerator;
using FolderIconChangerWPF;

namespace Ezz_Helper.Drawing
{
    public static partial class ImageH
    {
        public static System.Drawing.Bitmap FromFile_Unlocked(string filename)
        {
            System.Drawing.Image ImaFromFile = System.Drawing.Bitmap.FromFile(filename);

            System.Drawing.Bitmap NewImage = new System.Drawing.Bitmap(ImaFromFile);

            ImaFromFile.Dispose();
            return NewImage;
        }
        public static System.Drawing.Bitmap FromFile_Unlocked(string filename, bool useEmbeddedColorManagement)
        {
            System.Drawing.Image ImaFromFile = System.Drawing.Bitmap.FromFile(filename, useEmbeddedColorManagement);

            System.Drawing.Bitmap NewImage = new System.Drawing.Bitmap(ImaFromFile);
            //NewImage.PixelFormat.
            ImaFromFile.Dispose();
            return NewImage;
        }

        /// <summary>
        /// Gets the bit count for a given pixel format.
        /// </summary>
        /// <param name="pixelFormat_1">The pixel format.</param>
        /// <returns>The bit count.</returns>
        public static Byte ToBitDepth(this PixelFormat pixelFormat_1)
        {
            switch (pixelFormat_1)
            {
                case PixelFormat.Format1bppIndexed:

                    return 1;
                case PixelFormat.Format4bppIndexed:

                    return 4;
                case PixelFormat.Format8bppIndexed:

                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:

                    return 16;
                case PixelFormat.Format24bppRgb:

                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                case PixelFormat.Format48bppRgb:

                    return 48;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;
                default:

                    String message = string.Format("A pixel format '{0}' not supported!", pixelFormat_1);
                    throw new NotSupportedException(message);
            }
        }

        public static System.Drawing.Bitmap ConvertPixelFormat(this System.Drawing.Bitmap SourceImage, PixelFormat TargePixelFormat_, bool UseGrayColor_ = false, bool UsePreciseMethod = true)
        {
            return SimpleHelper.ConvertBitmap(SourceImage, TargePixelFormat_, UseGrayColor_, UsePreciseMethod);
        }
        public static System.Drawing.Bitmap ConvertPixelFormat(this Bitmap SourceImage, PixelFormat TargePixelFormat_, Func<Color, Color> CustomGrayColor_, bool UsePreciseMethod = true)
        {
            return SimpleHelper.ConvertBitmap(SourceImage, TargePixelFormat_, CustomGrayColor_, UsePreciseMethod);
        }
        public static System.Drawing.Bitmap ConvertPixelFormat(this Bitmap SourceImage, PixelFormat TargePixelFormat_, BaseColorCacheQuantizer Quantizer, bool UseGrayColor_ = false)
        {
            return SimpleHelper.ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, UseGrayColor_);
        }
        public static System.Drawing.Bitmap ConvertPixelFormat(this Bitmap SourceImage, PixelFormat TargePixelFormat_, BaseColorCacheQuantizer Quantizer, Func<Color, Color> CustomGrayColor_)
        {
            return SimpleHelper.ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, CustomGrayColor_);
        }
        //

        /// <summary>
        /// Image Compression to Variou Quality (To JPEG)
        /// </summary>
        /// <param name="original"></param>
        /// <param name="quality">The quality from 10 (lowest) to 100 (highest)</param>
        /// http://bobcravens.com/2009/10/image-compression-in-c-for-asp-net-mvc/
        public static Image CompressImageJPEG(this Image original, long quality = 100)
        {
            if (quality > 100 || quality < 10)
            {
                throw new System.Exception(quality + " Is not vaild for JPEG quality!");
            }
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            Image Res = null;
            if (jpgEncoder != null)
            {
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                using (MemoryStream ms = new MemoryStream())
                {
                    original.Save(ms, jpgEncoder, encoderParameters);
                    Res = Image.FromStream(ms);
                }
            }
            return Res;
        }
        /// <summary>
        /// Image DefaultCompression (BMP --> Png)
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        /// http://bobcravens.com/2009/10/image-compression-in-c-for-asp-net-mvc/
        public static Image CompressImage_DefaultPng(this Image original)
        {
            Image Res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                original.Save(ms, ImageFormat.Png);
                Res = Image.FromStream(ms);
                //Bitmap compressed = new Bitmap(ms);
                //string fileOutPng = Path.Combine(ImagePath, "default.png");
                //compressed.Save(fileOutPng, ImageFormat.Png);
            }
            return Res;
        }
        //
        public static string ImageToBase64(this Image image)
        {
            return ImageToBase64(image, image.RawFormat);
        }
        public static string ImageToBase64(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            string res = "";
            //ImageConverter newIC = new ImageConverter();
            //byte[] imageBytes = newIC.ConvertTo(image, typeof(byte[])) as byte[];
            //res = Convert.ToBase64String(imageBytes);
            //
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(image);
                // Convert Image to byte[]
                bmp.Save(ms, format);
                //image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                res = Convert.ToBase64String(imageBytes);
            }
            return res;
        }
        public static Image Base64ToImage(this string base64String)
        {
            if (base64String.IsNullOrEmpty()) return null;
            Image image = null;
            try
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    // Convert byte[] to Image
                    //ms.Write(imageBytes, 0, imageBytes.Length);
                    image = Image.FromStream(ms);
                }
                return image;
            }
            catch
            {
                return null;
            }
        }

        public static Image ToImage(this byte[] byteArr)
        {
            if (byteArr == null) return null;
            Image res = null;
            using (MemoryStream ms = new MemoryStream(byteArr))
            {
                res = new Bitmap(ms);
            }
            //
            //var converter = new ImageConverter();
            //if (converter.CanConvertFrom(typeof(byte[])))
            //{
            //    try
            //    {
            //        res = converter.ConvertFrom(byteArr);
            //    }
            //    finally
            //    {
            //        res = null;
            //    }
            //}
            return res;
        }
        public static byte[] ToByteArr(this Image image)
        {
            if (image == null) return null;
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                res = ms.ToArray();
            }
            return res;
        }
        //
    }

    public static partial class IconH
    {
        //Todo : get it from icomNImage VB.

        public enum GetIconMathods
        {
            IconsManagerClass = 0,
            DefaultMethod = 1
        }

        public struct GIconsInfo : IDisposable
        {
            /// <summary>
            /// This Object is ok to work with it.?
            /// </summary>
            /// <remarks></remarks>
            public bool OK;
            public string path;
            //Public iconsList As List(Of Icon)
            public List<GIconInfo> iconsList;
            public IconSize IconSize;
            public int iCount;

            public GetIconMathods GetMethod;
            public static GIconsInfo InitializeNew(string FilePath, GetIconMathods GetMethod_ = GetIconMathods.IconsManagerClass, IconSize IconSize_ = IconSize.Large)
            {
                var newII = new GIconsInfo();
                //newII.iconsList = New List(Of Icon)
                newII.iconsList = new List<GIconInfo>();
                newII.path = FilePath;
                newII.OK = false;
                newII.IconSize = IconSize_;
                newII.GetMethod = GetMethod_;
                newII.iCount = 0;
                return newII;
            }
            /// <summary>
            /// Rest and calls foreach : (GIconInfo.Dispose) which Rest and calls : (icon.Dispose) //Then Me.Finalize()
            /// </summary>
            /// <remarks></remarks>
            public void Dispose()
            {
                if (iconsList != null)
                {
                    foreach (GIconInfo GIconI in iconsList)
                    {
                        GIconI.Dispose();
                    }
                }
                //this.Finalize();
                //OtherMethods.DisposeObject(iconsList)
            }
        }
        public struct GIconInfo : IDisposable
        {
            /// <summary>
            /// This Object is ok to work with it.?
            /// </summary>
            /// <remarks></remarks>
            public bool OK;
            public string path;
            public int index;
            public int iCount;
            public IconSize IconSize;

            public Icon icon;
            public static GIconInfo InitializeNew(Icon icon_ = null, int index_ = 0, string FilePath = "", IconSize IconSize_ = 0)
            {
                var newII = new GIconInfo();
                newII.OK = false;
                newII.icon = icon_;
                newII.index = index_;
                newII.path = FilePath;
                newII.IconSize = IconSize_;

                return newII;
            }

            /// <summary>
            /// Rest and calls : (icon.Dispose) //Then Me.Finalize()
            /// </summary>
            /// <remarks></remarks>
            public void Dispose()
            {
                try
                {
                    OK = false;
                    icon.Dispose();
                }
                finally
                {
                    //this.Finalize();
                }
            }
        }

        public enum IconSize
        {
            Large = 0,
            Small = 1
        }

        public static Icon ImageToIcon(this Bitmap Image_1, Bitmap bitmapMask = null, Color? TransparentColor_ = null,
                                            bool DisposeSourceImage = false, Drawing.IconsManager.ImageColorsTypes BitCount_ = Drawing.IconsManager.ImageColorsTypes.Unknown)
        {
            return Ezz_Helper.Drawing.IconsManager.IconInfo.IconFromBitmap(Image_1, bitmapMask, TransparentColor_, DisposeSourceImage, BitCount_);
        }

    }
}
