using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;

namespace Ezz_Helper.Drawing.IconsManager
{

   
    /// <summary>
    /// Provides information about a given icon.(Load, Create, Add, Remove icons :))
    /// This class cannot be inherited.
    /// </summary>
    [Serializable()]
    public sealed class IconInfo : IDisposable
    {
        #region " ReadOnly"
        public static int SizeOfIconDir = Marshal.SizeOf(typeof(IconDir));
        public static int SizeOfIconDirEntry = Marshal.SizeOf(typeof(IconDirEntry));
        public static int SizeOfGroupIconDir = Marshal.SizeOf(typeof(GRPICONDIR));

        public static int SizeOfGroupIconDirEntry = Marshal.SizeOf(typeof(GRPICONDIRENTRY));

        #endregion " ReadOnly"

        #region " Properties"

        private Icon _sourceIcon;
        /// <summary>
        /// Gets the source System.Drawing.Icon.
        /// </summary>
        public Icon SourceIcon
        {
            get { return _sourceIcon; }
        }

        private string _fileName = null;
        /// <summary>
        /// Gets the icon's file name.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private List<IconImageInfo> _imageList;
        ReadOnlyCollection<IconImageInfo> _imageListReadOnly;
        public ReadOnlyCollection<IconImageInfo> ImageList => _imageListReadOnly ??= _imageList.AsReadOnly();
        public IconImageInfo this[int ImageIndex]
        {
            get
            {
                return _imageList[ImageIndex];
            }
        }

        /// <summary>
        /// Get whether the icon contain more than one image or not.
        /// </summary>
        public bool IsMultiIcon
        {
            get { return (this._imageList.Count > 1); }
            //get { return (this.Images.Count > 1); }
        }

        public int Index { get; set; }

        //private int _bestFitIconIndex;
        ///// <summary>
        ///// Gets icon index that best fits to screen resolution.
        ///// </summary>
        //public int BestFitIconIndex
        //{
        //    get { return _bestFitIconIndex; }
        //}

        //private int _width;
        ///// <summary>
        ///// Gets icon width.
        ///// </summary>
        //public int Width
        //{
        //    get { return _width; }
        //}

        //private int _height;
        ///// <summary>
        ///// Gets icon height.
        ///// </summary>
        //public int Height
        //{
        //    get { return _height; }
        //}

        //private int _colorCount;
        ///// <summary>
        ///// Gets number of colors in icon (0 if >=8bpp).
        ///// </summary>
        //public int ColorCount
        //{
        //    get { return _colorCount; }
        //}

        //private int _planes;
        ///// <summary>
        ///// Gets icon color planes.
        ///// </summary>
        //public int Planes
        //{
        //    get { return _planes; }
        //}

        //private int _bitCount;
        ///// <summary>
        ///// Gets icon bits per pixel (0 if is less than 8bpp)
        ///// </summary>
        ///// <value></value>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public int BitCount
        //{
        //    get { return _bitCount; }
        //}

        ///// <summary>
        ///// Gets icon bits per pixel.
        ///// </summary>
        //public int ColorDepth
        //{
        //    get
        //    {
        //        if (this.BitCount != 0)
        //        {
        //            return this.BitCount;
        //        }
        //        if (this.ColorCount == 0)
        //        {
        //            return 0;
        //        }
        //        return (int)Math.Log(this.ColorCount, 2);
        //    }
        //}

        #endregion " Properties"

        #region " Icon Headers Properties"
        private IconDir _iconDir;
        /// <summary>
        /// Gets the IconsManager.IconDir of the icon.
        /// </summary>
        public IconDir IconDir
        {
            get { return _iconDir; }
        }

        //private List<IconDirEntry> _iconDirEntries;
        ///// <summary>
        ///// Gets a list of IconsManager.IconDirEntry of the icon.
        ///// </summary>
        //public ReadOnlyCollection<IconDirEntry> IconDirEntries
        //{
        //    get { return _iconDirEntries.AsReadOnly(); }
        //}

        private GRPICONDIR _groupIconDir;
        /// <summary>
        /// Gets the IconsManager.GroupIconDir of the icon.
        /// </summary>
        public GRPICONDIR GroupIconDir
        {
            get { return _groupIconDir; }
        }

        //private List<GRPICONDIRENTRY> _groupIconDirEntries;
        ///// <summary>
        ///// Gets a list of IconsManager.GroupIconDirEntry of the icon.
        ///// </summary>
        //public ReadOnlyCollection<GRPICONDIRENTRY> GroupIconDirEntries
        //{
        //    get { return _groupIconDirEntries.AsReadOnly(); }
        //}
        //Private Set(value As List(Of GroupIconDirEntry))
        //    _groupIconDirEntries = value
        //End Set

        //private List<byte[]> _rawData;
        ///// <summary>
        ///// Gets a list of raw data for each icon image.
        ///// </summary>
        //public ReadOnlyCollection<byte[]> RawData
        //{
        //    get { return _rawData.AsReadOnly(); }
        //}

        private byte[] _resourceRawData;
        /// <summary>
        /// Gets the icon raw data as a resource data.
        /// </summary>
        public byte[] ResourceRawData
        {
            get { return _resourceRawData; }
        }
        #endregion " Icon Headers Properties"

        #region " Constructors"

        private bool _BuildIcons;
        /// <summary>
        /// Initializes a new instance of IconsManager.IconInfo (Empty)
        /// </summary>
        /// <remarks></remarks>
        public IconInfo()
            : base()
        {
            RestLists();
        }

        /// <summary>
        /// Initializes a new instance of IconsManager.IconInfo which contains the information about the givin icon.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon object to retrieve the information about.</param>
        public IconInfo(Icon icon, int _Index = 0, bool BuildIcons_ = false)
            : this()
        {
            Index = _Index;
            _BuildIcons = BuildIcons_;
            this._fileName = null;
            LoadIconInfo(icon);
        }

        /// <summary>
        /// Initializes a new instance of IconsManager.IconInfo which contains the information about the icon in the givin file.
        /// </summary>
        /// <param name="fileName__1">A fully qualified name of the icon file, it can contain environment variables.</param>
        public IconInfo(string fileName__1, bool BuildIcons_ = false)
            : this()
        {
            Index = 0;
            _BuildIcons = BuildIcons_;
            this._fileName = FileName;
            LoadIconInfo(new Icon(fileName__1));
        }

        /// <summary>
        /// Initializes a new instance of IconsManager.IconInfo which contains the information about the icon which converted from givin Bitmap.
        /// </summary>
        /// <param name="Bitmap_">An image to convert it to new icon.</param>
        /// <remarks>A fully qualified name of the icon file, it can contain environment variables.</remarks>
        public IconInfo(Bitmap Bitmap_) : this(IconFromBitmap(Bitmap_)) { }

        #endregion " Constructors"

        #region " Public Methods"
        public IconImageInfo GetBestFitIcon()
        {
            return this._imageList[GetBestFitIconIndex()];
        }
        public IconImageInfo GetBestFitIcon(Size desiredSize)
        {
            return this._imageList[GetBestFitIconIndex(desiredSize)];
        }
        public IconImageInfo GetBestFitIcon(Size desiredSize, bool isMonochrome)
        {
            return this._imageList[GetBestFitIconIndex(desiredSize, isMonochrome)];
        }

        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex()
        {
            if (this._resourceRawData == null)
                return 0;
            unsafe
            {
                fixed (byte* p = this._resourceRawData)
                    return Win32.LookupIconIdFromDirectory((IntPtr)p, true);
            }
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize)
        {
            return GetBestFitIconIndex(desiredSize, false);
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize, bool isMonochrome)
        {
            if (this._resourceRawData == null)
                return 0;
            LookupIconIdFromDirectoryExFlags flags = isMonochrome ? LookupIconIdFromDirectoryExFlags.LR_MONOCHROME : LookupIconIdFromDirectoryExFlags.LR_DEFAULTCOLOR;
            unsafe
            {
                fixed (byte* p = this._resourceRawData)
                    return Win32.LookupIconIdFromDirectoryEx((IntPtr)p, true, desiredSize.Width, desiredSize.Height, flags);
            }
        }
        #endregion " Public Methods"

        #region "Shared methods"
   
        public static Icon ErrorIcon(string ErrText = "-Error-")
        {
            if (string.IsNullOrEmpty(ErrText)) ErrText = "-Error-";
            Bitmap newBM = new Bitmap(48, 48);
            var newGr = Graphics.FromImage(newBM);
            newGr.FillRectangle(Brushes.Black, 0, 0, 48, 48);
             var TFont = SystemFonts.IconTitleFont;
            var TSize = newGr.MeasureString(ErrText, TFont).ToSize();
            float TX = (float)(newBM.Width / 2) - (TSize.Width / 2);
            //Center
            float TY = (float)(newBM.Height / 2) - (TSize.Height / 2);
            //Center
            newGr.DrawString(ErrText, TFont, Brushes.Red, TX, TY);
             Icon _ErrorIcon;
            _ErrorIcon = Icon.FromHandle(newBM.GetHicon());
            newGr.Dispose();
            newBM.Dispose();
            return _ErrorIcon;
        }
        public static IconInfo Empty()
        {
            return new IconInfo();
        }

        /// <summary>
        /// Converts an System.Drawing.Bitmap() to new System.Drawing.Icon()
        /// </summary>
        /// <param name="SourceImage">The System.Drawing.Bitmap to convert (The size must be less than or equal 256x256)</param>
        /// <param name="DisposeSourceImage">Release all resources used by the SourceImage?</param>
        /// <param name="BitCount_">To Convet the PixelFormat.</param>
        /// <returns>New icon from SourceImage</returns>
        /// <remarks></remarks>
        public static Icon IconFromBitmap(Bitmap SourceImage, Bitmap bitmapMask = null, Color? TransparentColor_1 = null,
                                                    bool DisposeSourceImage = false, ImageColorsTypes BitCount_ = ImageColorsTypes.Unknown)
        {
            // We need to rotate the images, but we don't want to mess with the source image, lets create a clone
            var iSize_ = SourceImage.Size;
            //if (iSize_.Width > 256 || iSize_.Height > 256)
            //    throw new NotSupportedException(iSize_.ToString_1("{0}x{1}") + " Is not supported for Icons!, Max is 256x256");

            Bitmap Image_ = null;
            if (BitCount_ == ImageColorsTypes.Unknown)
            {
                //if (Tools.BitCountFromPixelFormat(SourceImage.PixelFormat) == 16)
                //    throw new NotSupportedException("16 bpp images are not supported for Icons");
                PixelFormat PixelFormat_1 = SourceImage.PixelFormat;
                if (!PixelFormat_1.IsSupportedForIcons())
                    throw new NotSupportedException(PixelFormat_1 + " Is not supported for Icons");
                Image_ = (Bitmap)SourceImage.Clone();
            }
            else if (!BitCount_.IsSupportedForIcons())
            {
                throw new NotSupportedException(BitCount_ + " Is not supported for Icons");
            }
            else
            {
                Image_ = SourceImage.ConvertImagePixelFormat(BitCount_.GetPixelFormat());
            }

            //
            Bitmap mask = bitmapMask != null ? (Bitmap)bitmapMask.Clone() : null;

            //
            //.NET has a bug flipping in the Y axis for 1bpp images, let do it ourself
            if (Image_.PixelFormat != PixelFormat.Format1bppIndexed)
            {
                Image_.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            else
            {
                FlipYBitmap(Image_);
            }

            if (mask != null) FlipYBitmap(mask);

            BitCount_ = Image_.PixelFormat.GetColorsTypeName();//(ImageColorsTypes)Tools.GetBitCount(Image_.PixelFormat);
            // Palette
            // Some icons programs like Axialis have program with a reduce palette, so lets create a complete palette instead
            RGBQUAD[] palette = Tools.RGBQUADFromColorArray(Image_);
            // Bitmap Header
            BITMAPINFOHEADER infoHeader = new BITMAPINFOHEADER();
            infoHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            infoHeader.biWidth = (uint)iSize_.Width;
            infoHeader.biHeight = (uint)iSize_.Height * 2;
            infoHeader.biPlanes = 1;
            infoHeader.biBitCount = BitCount_.GetBitCount();
            infoHeader.biCompression = IconImageFormat.BMP;//IconImageFormat.PNG
            infoHeader.biXPelsPerMeter = 0;
            infoHeader.biYPelsPerMeter = 0;
            infoHeader.biClrUsed = (uint)palette.Length;
            infoHeader.biClrImportant = 0;

            Color transparentColor;
            if (TransparentColor_1.HasValue)
            { transparentColor = TransparentColor_1.Value; }
            else
            {
                transparentColor = Color.Transparent;
            }

            //XOR Image
            byte[] XOR;
            BitmapData bmpData = Image_.LockBits(new Rectangle(0, 0, iSize_.Width, iSize_.Height), ImageLockMode.ReadOnly, Image_.PixelFormat);
            IntPtr scanColor = bmpData.Scan0;
            XOR = new byte[Math.Abs(bmpData.Stride) * bmpData.Height];
            Marshal.Copy(scanColor, XOR, 0, XOR.Length);
            Image_.UnlockBits(bmpData);

            infoHeader.biSizeImage = (uint)XOR.Length;

            //AND Image
            byte[] AND;
            if (mask == null)
            {
                // Lets create the AND Image from the Color Image
                Bitmap bmpBW = new Bitmap(iSize_.Width, iSize_.Height, PixelFormat.Format1bppIndexed);
                BitmapData bmpBWData = bmpBW.LockBits(new Rectangle(0, 0, iSize_.Width, iSize_.Height), ImageLockMode.ReadWrite, bmpBW.PixelFormat);
                IntPtr scanBW = bmpBWData.Scan0;
                AND = new byte[Math.Abs(bmpBWData.Stride) * bmpBWData.Height];

                //Let extract the AND image from the XOR image
                int strideC = Math.Abs(bmpData.Stride);
                int strideB = Math.Abs(bmpBWData.Stride);
                int bpp = BitCount_.GetBitCount();//Image_.PixelFormat.GetBitCount();//Tools.GetBitCount(Image_.PixelFormat);
                int posCY;
                int posCX;
                int posBY;
                int color;
                Color tColor;
                RGBQUAD paletteColor;

                //If the image is 24 bits, then lets make sure alpha channel is 0
                if (bpp == 24)
                    transparentColor = Color.FromArgb(0, transparentColor.R, transparentColor.G, transparentColor.B);

                for (int y = 0; y < bmpData.Height; y++)
                {
                    posBY = strideB * y;
                    posCY = strideC * y;
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        switch (bpp)
                        {
                            case 1:
                                AND[(x >> 3) + posCY] = (byte)XOR[(x >> 3) + posCY];
                                break;

                            case 4:
                                color = XOR[(x >> 1) + posCY];
                                paletteColor = palette[(x & 1) == 0 ? color >> 4 : color & 0x0F];
                                if (Tools.CompareRGBQUADToColor(paletteColor, transparentColor))
                                {
                                    AND[(x >> 3) + posBY] |= (byte)(0x80 >> (x & 7));
                                    XOR[(x >> 1) + posCY] &= (byte)((x & 1) == 0 ? 0x0F : 0xF0);
                                }
                                break;

                            case 8:
                                color = XOR[x + posCY];
                                paletteColor = palette[color];
                                if (Tools.CompareRGBQUADToColor(paletteColor, transparentColor))
                                {
                                    AND[(x >> 3) + posBY] |= (byte)(0x80 >> (x & 7));
                                    XOR[x + posCY] = 0;
                                }
                                break;

                            case 16:
                                throw new NotSupportedException("16 bpp images are not supported for Icons");
                            case 24:
                                posCX = x * 3;
                                tColor = Color.FromArgb(0, XOR[posCX + posCY + 0],
                                                            XOR[posCX + posCY + 1],
                                                            XOR[posCX + posCY + 2]);
                                if (tColor == transparentColor)
                                    AND[(x >> 3) + posBY] |= (byte)(0x80 >> (x & 7));
                                break;

                            case 32:
                                if (transparentColor == Color.Transparent)
                                {
                                    if (XOR[(x << 2) + posCY + 3] == 0)
                                        AND[(x >> 3) + posBY] |= (byte)(0x80 >> (x & 7));
                                }
                                else
                                {
                                    if (XOR[(x << 2) + posCY + 0] == transparentColor.B &&
                                        XOR[(x << 2) + posCY + 1] == transparentColor.G &&
                                        XOR[(x << 2) + posCY + 2] == transparentColor.R)
                                    {
                                        AND[(x >> 3) + posBY] |= (byte)(0x80 >> (x & 7));
                                        XOR[(x << 2) + posCY + 0] = 0;
                                        XOR[(x << 2) + posCY + 1] = 0;
                                        XOR[(x << 2) + posCY + 2] = 0;
                                    }
                                    else
                                    {
                                        XOR[(x << 2) + posCY + 3] = 255;
                                    }
                                }
                                break;
                        }
                    }
                }
                bmpBW.UnlockBits(bmpBWData);
            }
            else
            {
                // Mask is coming by parameter, so we don't need to create it
                BitmapData bmpBWData = mask.LockBits(new Rectangle(0, 0, mask.Width, mask.Height), ImageLockMode.ReadOnly, mask.PixelFormat);
                IntPtr scanBW = bmpBWData.Scan0;
                AND = new byte[Math.Abs(bmpBWData.Stride) * bmpBWData.Height];
                Marshal.Copy(scanBW, AND, 0, AND.Length);
                mask.UnlockBits(bmpBWData);
                //
                //BitmapData BData = Image_.LockBits(new Rectangle(0, 0, Image_.Width, Image_.Height), ImageLockMode.ReadOnly, Image_.PixelFormat);
                //IntPtr scanBW = BData.Scan0;
                //var ANDdataLength = Math.Abs(BData.Stride) * BData.Height;
                //AND = new byte[ANDdataLength];
                ////Marshal.Copy(scanBW, [AND], 0, [AND].Length)
                //Marshal.Copy(scanBW, AND, 0, ANDdataLength);
                //Image_.UnlockBits(BData);
                ////Marshal.FreeHGlobal(scanBW)
            }
            Image_.Dispose();
            //
            //Build the new icon

            Icon Icon_ = null;
            using (MemoryStream MS = new MemoryStream())
            {
                // ICONDIR
                var newDir = new IconDir();
                newDir.Reserved = 0;
                newDir.Type = 1;
                newDir.Count = 1;  //0
                Tools.WriteStructure<IconDir>(MS, newDir);

                IconDirEntry iconEntry = new IconDirEntry();
                int ColorsInPalette = (int)(infoHeader.biClrUsed != 0 ? infoHeader.biClrUsed : infoHeader.biBitCount <= 8 ? (uint)(1 << infoHeader.biBitCount) : 0);//(byte)(infoHeader.biClrUsed >= 8 ? 0 : infoHeader.biClrUsed);

                iconEntry.ColorCount = (byte)infoHeader.biClrUsed; // (0 if >= 8bpp-true colors)

                Int32 Height_ = (int)infoHeader.biHeight / 2;
                if (Height_ > byte.MaxValue) Height_ = 0;
                iconEntry.Height = (byte)Height_;
                iconEntry.Reserved = 0;
                Int32 Width_ = (int)infoHeader.biWidth;
                //if (Width_ > byte.MaxValue) Width_ = 0;
                iconEntry.Width = (byte)Width_;

                //var ResBytesInRes = (Marshal.SizeOf(typeof(BITMAPINFOHEADER)) + Marshal.SizeOf(typeof(RGBQUAD)) * ColorsInPalette + XOR.Length + AND.Length);
                iconEntry.BytesInRes = (Marshal.SizeOf(typeof(BITMAPINFOHEADER)) + Marshal.SizeOf(typeof(RGBQUAD)) * ColorsInPalette + XOR.Length + AND.Length);
                iconEntry.ImageOffset = SizeOfIconDir + SizeOfIconDirEntry;//(Int16)((uint)Marshal.SizeOf(IconDir) + Marshal.SizeOf(typeof(IconDirEntry)));
                iconEntry.BitCount = (Int16)infoHeader.biBitCount;
                iconEntry.Planes = (Int16)infoHeader.biPlanes;
                Tools.WriteStructure<IconDirEntry>(MS, iconEntry);

                MS.Seek(iconEntry.ImageOffset, SeekOrigin.Begin);
                //Write Image Info Header
                Tools.WriteStructure<BITMAPINFOHEADER>(MS, infoHeader);

                // Image Palette
                byte[] buffer = new byte[Marshal.SizeOf(typeof(RGBQUAD)) * ColorsInPalette];
                GCHandle handle = GCHandle.Alloc(palette, GCHandleType.Pinned);
                Marshal.Copy(handle.AddrOfPinnedObject(), buffer, 0, buffer.Length);
                handle.Free();
                MS.Write(buffer, 0, buffer.Length);

                // Image XOR Image
                MS.Write(XOR, 0, XOR.Length);

                // Image AND Image
                MS.Write(AND, 0, AND.Length);

                // Rewind the stream
                MS.Seek(0, SeekOrigin.Begin);

                //Finally :)
                Icon_ = new Icon(MS); //, iSize_)
                //TO DO : fix a bug with   Monochrome)
                //if (BitCount_ == ImageColorsTypes.Monochrome)
                //{
                //    using (FileStream FS = File.Create(@"D:\Test Folder2\Test Monochrome.ico"))
                //    {
                //        Icon_.Save(FS);
                //    }

                //}
            }//MS.Dispose();
            if (DisposeSourceImage) SourceImage.Dispose();
            return Icon_;
        }
        private static unsafe void FlipYBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format1bppIndexed)
                return;

            // .Net bug.. it can't flip in the Y axis a 1bpp properly
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

            byte* pixelPtr = (byte*)bitmapData.Scan0.ToPointer();
            byte[] tmpbuffer = new byte[bitmapData.Stride];

            fixed (byte* lptmpbuffer = tmpbuffer)
            {
                for (int i = 0; i < bitmap.Height / 2; i++)
                {
                    Win32.CopyMemory(lptmpbuffer, pixelPtr + (i * bitmapData.Stride), bitmapData.Stride);
                    Win32.CopyMemory(pixelPtr + (i * bitmapData.Stride), pixelPtr + (((bitmap.Height - 1) - i) * bitmapData.Stride), bitmapData.Stride);
                    Win32.CopyMemory(pixelPtr + (((bitmap.Height - 1) - i) * bitmapData.Stride), lptmpbuffer, bitmapData.Stride);
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        #endregion "Shared methods"

        #region private Methods

        #region "Consts"

        private const int LOAD_LIBRARY_AS_DATAFILE = 0x2;
        private const int RT_ICON = 3;

        private const int RT_GROUP_ICON = 14;

        private const int MAX_PATH = 260;
        private const int ERROR_SUCCESS = 0;
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_BAD_EXE_FORMAT = 193;

        private const int ERROR_RESOURCE_TYPE_NOT_FOUND = 1813;
        private const int sICONDIR = 6;// sizeof(ICONDIR)
        private const int sICONDIRENTRY = 16; // sizeof(ICONDIRENTRY)
        private const int sGRPICONDIRENTRY = 14; // sizeof(GRPICONDIRENTRY)

        #endregion "Consts"

        private void RestLists()
        {
            this._imageList = new List<IconImageInfo>();
            this._imageListReadOnly = null;
        }

        /// <summary>
        /// Loads the icon information from the givin icon into class members.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon object to retrieve the information about.</param>
        private void LoadIconInfo(Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("icon");
            }

            this._sourceIcon = icon;
            var DirCount = 0;
            using (MemoryStream inputStream = new MemoryStream())
            {
                icon.Save(inputStream);

                inputStream.Seek(0, SeekOrigin.Begin);
                IconDir dir = Tools.ReadStructure<IconDir>(inputStream);

                DirCount = dir.Count;
                this._iconDir = dir;
                this._groupIconDir = dir.ToGroupIconDir();

                DisposeImages();
                RestLists();

                int i = 0;
                if (_BuildIcons)
                {
                    while (i < DirCount)
                    {
                        IconImageInfo IIInfo = new IconImageInfo(inputStream, i);
                        IIInfo.Refresh();
                        _imageList.Add(IIInfo);
                        i++;
                    }
                }
                else
                {
                    while (i < DirCount)
                    {
                        IconImageInfo IIInfo = new IconImageInfo(inputStream, i);
                        _imageList.Add(IIInfo);
                        i++;
                    }
                }
            }  //inputStream.Close();

            this._resourceRawData = GetIconResourceData();
        }
        //
        /// <summary>
        /// Returns the icon's raw data as a resource data.
        /// </summary>
        /// <returns>The icon's raw as a resource data.</returns>
        private byte[] GetIconResourceData()
        {
            byte[] RData;
            using (MemoryStream outputStream = new MemoryStream())
            {
                Tools.WriteStructure<GRPICONDIR>(outputStream, this.GroupIconDir);
                foreach (IconImageInfo IIInf in this._imageList)
                {
                    Tools.WriteStructure<GRPICONDIRENTRY>(outputStream, IIInf.iEntry.Value.ToGroupIconDirEntry(IIInf.Index));
                }

                //foreach (GRPICONDIRENTRY entry in this._groupIconDirEntries)
                //{
                //    Tools.WriteStructure<GRPICONDIRENTRY>(outputStream, entry);
                //}
                RData = outputStream.ToArray();
            }
            return RData;
        }

        #endregion private Methods

        #region Check and get

        /// <summary>
        /// To reload icon info from sourceIcon.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshInfo()
        {
            this.DisposeImages();
            this.LoadIconInfo(this._sourceIcon);
        }

        //public IconImageInfo GetChildIconImageInfo(Icon ChildIcon_)
        //{
        //     using (IconImageInfo nIIF = new IconImageInfo(ChildIcon_))
        //    {
        //        //return _imageList.Find((res) => res.Icon == ChildIcon_);
        //    }

        //    //return _imageList.Find((res) => res.Icon == ChildIcon_);
        //}

        public IconDirEntry? GetChildIconDirEntry(int ChildIndex_)
        {
            if (ChildIndex_ < 0 || ChildIndex_ > this._imageList.Count - 1)
            {
                return null;
            }
            var Find_1 = _imageList.Find((res) => res.Index == ChildIndex_);
            if (Find_1 == null) return null;

            return Find_1.iEntry;
            //
            //if (ChildIndex_ < 0 || ChildIndex_ > this._iconDirEntries.Count - 1)
            //{
            //    return null;
            //}
            //return this._iconDirEntries[ChildIndex_];
        }
        //public IconDirEntry? GetChildIconDirEntry(Icon ChildIcon_)
        //{
        //    var Find_1 = _imageList.Find((res) => res.Icon == ChildIcon_);
        //    if (Find_1 == null) return null;
        //    return Find_1.iEntry;
        //    //return GetChildIconDirEntry(this.Images.IndexOf(ChildIcon_));
        //}

      
        public bool ContainsIconWithColors(IconImageInfo IconImageInfo_)
        {
            var Find_1 = _imageList.Find((res) => res.CheckSameSizeNColors(IconImageInfo_));
            return (Find_1 != null);
            //return ContainsIconWithColors(IconImageInfo_.iEntry.Value);
        }
        public bool ContainsIconWithColors(Icon Icon_)
        {
            return _imageList.ContainsIconWithColors(Icon_);
        }
        public bool ContainsIconWithColors(IconDirEntry IconDirEntry_)
        {
            return _imageList.ContainsIconWithColors(IconDirEntry_);
        }
        public bool ContainsIconWithColors(Size Size_, ImageColorsTypes ColorsType_)
        {
            return _imageList.ContainsIconWithColors(Size_, ColorsType_);
        }
        public bool ContainsIconWithColors(OneIconInfo OneIconInfos)
        {
            return _imageList.ContainsIconWithColors(OneIconInfos);
        }
        public bool ContainsAllIcons(params OneIconInfo[] OneIconInfos)
        {
            bool res = true;
            foreach (var oneIF in OneIconInfos)
            {
                if (!_imageList.ContainsIconWithColors(oneIF)) return false;
            }
            return res;
        }
        //

        public List<Bitmap> GetAllImages()
        {
            return _imageList.GetAllImages();
        }
        public List<IconDirEntry> GetAllIconDirEntries()
        {
            return _imageList.GetAllIconDirEntries();
        }
        public List<GRPICONDIRENTRY> GetAllGroupIconDirEntry()
        {
            return _imageList.GetAllGroupIconDirEntry();
        }

        #endregion Check and get

        #region IDisposable Members

        public void DisposeImages()
        {
            if (_imageList == null) return;
            foreach (IconImageInfo ic_ in _imageList)
            {
                ic_.Dispose();
            }
            _imageList = null;
            _imageListReadOnly = null;
            //
            //if (_images == null)  return;
            //foreach (Icon ic_ in this._images)
            //{
            //    ic_.Dispose();
            //}
        }

        public void Dispose()
        {
            DisposeImages();
            //throw new NotImplementedException();
        }

        #endregion IDisposable Members

        #region IconImageInfo Class

         public sealed class IconImageInfo : IDisposable
        {
            private IconDirEntry? entry;
            public IconDirEntry? iEntry { get { return entry; } }

            public byte[] RawData { get; private set; }
            public int Index { get; set; }

            #region Constructors

            public IconImageInfo()
            {
                Index = 0;
                _IconImageFormat = IconImageFormat.UNKNOWN;
                _ChangeImageFormat = IconImageFormat.UNKNOWN;
            }
            public IconImageInfo(Stream ParentStream, int imageIndex)
                : this()
            {
                Read(ParentStream, imageIndex);
            }

            public IconImageInfo(Icon OneIcon)
                : this()
            {
                Read(OneIcon);
            }

            public IconImageInfo(Icon _ParentIcon, int iconIndex)
                : this()
            {
                Read(_ParentIcon, iconIndex);
            }

            #endregion Constructors

            #region Properties

            //private Icon Icon_1;
            //public Icon Icon
            //{
            //    get
            //    {
            //        if (Icon_1 == null)
            //        {
            //            RebuildIcon();
            //        }
            //        return Icon_1;
            //    }
            //}
            private Bitmap Image_1;
            /// <summary>
            /// the image of this icon
            /// </summary>
            public Bitmap Image
            {
                get
                {
                    if (Image_1 == null) RebuildImage();
                    return Image_1;
                }
            }

            public Size Size {
                get {
                    if (Image == null) return Size.Empty;
                   return Image.Size;
                }
            }
            public ImageColorsTypes ColorsType
            {
                get
                {
                    if (!entry.HasValue) return 0;
                    return entry.Value.ColorsType;
                }
            }


            private IconImageFormat _IconImageFormat;
            /// <summary>
            /// Gets current Image Format of this icon.
            /// </summary>
            public IconImageFormat IconImageFormat
            {
                get
                {
                    if (_IconImageFormat == IconImageFormat.UNKNOWN) { RecheckIconImageFormat(); }

                    return _IconImageFormat;
                }
            }

            private IconImageFormat _ChangeImageFormat = IconImageFormat.UNKNOWN;
            /// <summary>
            /// To convert ImageFormat [ bmp , png ] on writing to a stream. it's only for 256x256 and more.
            /// </summary>
            public IconImageFormat ChangeIconImageFormat
            {
                get { return _ChangeImageFormat; }
                set
                {
                    if (entry.HasValue) if (entry.Value.Width == 0 && entry.Value.Height == 0 && IconImageFormat != value)
                        {
                            _ChangeImageFormat = value;
                            return;
                        }
                    _ChangeImageFormat = IconImageFormat.UNKNOWN;
                }
            }

            #endregion Properties

            #region Read

            /// <summary>
            /// Reads from one icon.
            /// </summary>
            public void Read(Icon OneIcon)
            {
                //Icon_1 = OneIcon;
                using (MemoryStream iMS = new MemoryStream())
                {
                    OneIcon.Save(iMS);
                    //
                    Read(iMS);
                }
                _IconIsFromSource = true;
            }
            public void Read(Icon _ParentIcon, int iconIndex)
            {
                using (MemoryStream iMS = new MemoryStream())
                {
                    _ParentIcon.Save(iMS);
                    //
                    Read(iMS, iconIndex);
                }
            }
            /// <summary>
            /// Reads the first icon in the stream.
            /// </summary>
            /// <param name="IStream"></param>
            public void Read(Stream IStream)
            {
                Read(IStream, 0);
            }
            /// <summary>
            /// Reads an icon in the stream.
            /// </summary>
            /// <param name="IStream"></param>
            /// <param name="imageIndex"></param>
            public void Read(Stream IStream, int imageIndex)
            {
                _IconIsFromSource = false;
                Index = imageIndex;
                IStream.Seek(SizeOfIconDir + imageIndex * SizeOfIconDirEntry, SeekOrigin.Begin);
                //
                IconDirEntry entry_1 = Tools.ReadStructure<IconDirEntry>(IStream);

                RawData = new byte[entry_1.BytesInRes];
                IStream.Seek(entry_1.ImageOffset, SeekOrigin.Begin);
                IStream.ReadExactly(RawData, 0, entry_1.BytesInRes);
                entry = entry_1;
                //
                //To save some memory icon will be built one time when called.
                //Refresh();
            }

            #endregion Read
            //
            #region Write
            /// <summary>
            /// Saves current info to target <see cref="Stream"/> as new Icon.
            /// </summary>
            /// <param name="stream_1"></param>
            public void SaveWithIconDir(Stream stream_1)
            {
                IconDir IDir = IconDir.InitializeNew(1);
                IDir.Write(stream_1);
                Write(stream_1, 0, SizeOfIconDir + SizeOfIconDirEntry);
            }

            /// <summary>
            /// Writes current info to target icon stream. then returns the new IconDirEntry in this stream_1.
            /// </summary>
            /// <param name="stream_1">an icon stream that already contains IconDir.</param>
            /// <param name="imageIndex">image index in this stream_1.</param>
            /// <param name="ImageOffset_1">ImageOffset in this stream_1 to start write RawData.</param>
            public IconDirEntry Write(Stream stream_1, int imageIndex, int ImageOffset_1)
            {
                IconDirEntry newEntry = entry.Value;
                newEntry.ImageOffset = ImageOffset_1;
                byte[] newRawData;
                if (ChangeIconImageFormat != IconImageFormat.UNKNOWN) //Convert[ bmp<-->png ]
                {
                    newRawData = ConvertImageFormatRawData(this, ChangeIconImageFormat);//ConverIconImageFormat();
                    newEntry.BytesInRes = newRawData.Length;
                }
                else { newRawData = RawData; }

                //
                //Write the IconDirEntry to the stream.
                stream_1.Seek(SizeOfIconDir + imageIndex * SizeOfIconDirEntry, SeekOrigin.Begin);
                Tools.WriteStructure<IconDirEntry>(stream_1, newEntry);
                //Write image raw data.
                stream_1.Seek(ImageOffset_1, SeekOrigin.Begin);
                stream_1.Write(newRawData, 0, newEntry.BytesInRes);

                return newEntry;
            }

            #endregion Write

            #region Other public Methods

            ///// <summary>
            ///// To rebuild the current Icon from current info.
            ///// </summary>
            //public void RebuildIcon()
            //{
            //    if (Icon_1 != null) Icon_1.Dispose();
            //    if (entry.HasValue && RawData != null)
            //    {
            //        Icon_1 = BuildIcon(entry.Value, _RawData, true);
            //    }
            //    else
            //    {
            //        Icon_1 = null;
            //    }
            //}
            public void RebuildImage()
            {
                if (Image_1 != null) Image_1.Dispose();
                if (entry.HasValue && RawData != null)
                {
                     Image_1 = BuildImage(this);
                }
                else
                {
                    Image_1 = null;
                }
            }
            public void RecheckIconImageFormat()
            {
                if (RawData != null)
                {
                    _IconImageFormat = GetIconImageFormat(RawData);
                }
                else
                {
                    _IconImageFormat = IconImageFormat.UNKNOWN;
                }
            }
            /// <summary>
            /// To refill current info (RebuildIcon ,RecheckIconImageFormat)
            /// </summary>
            public void Refresh()
            {
                //RebuildIcon();
                RebuildImage();
                RecheckIconImageFormat();
            }
            public void RefreshIfNull()
            {
                //if (Icon_1 == null) RebuildIcon();
                if (Image_1 == null) RebuildImage();
                if (_IconImageFormat == IconImageFormat.UNKNOWN) { RecheckIconImageFormat(); }
            }

            public IconImageInfo Clone()
            {
                IconImageInfo newIIInf = new IconInfo.IconImageInfo();
                //newIIInf.Icon_1 = Icon_1 == null ? null : (Icon)Icon_1.Clone();
                newIIInf.RawData = RawData;
                newIIInf.entry = entry;
                newIIInf.Index = Index;
                newIIInf._IconImageFormat = _IconImageFormat;
                return newIIInf;
            }

            public bool Equals_2(IconImageInfo IImI)
            {
                if (!entry.Value.Equals_2(IImI.iEntry.Value)) return false;
                if (_IconImageFormat != IImI.IconImageFormat) return false;
                return true;
            }
           
            public bool CheckSameSizeNColors(Size Size_, ImageColorsTypes ColorsType_)
            {
                if (!entry.Value.CheckSameSizeNColors(Size_, ColorsType_)) return false;
                return (this.Size == Size_);
            }
            public bool CheckSameSizeNColors(IconImageInfo IImI)
            { 
                if (!entry.Value.CheckSameSizeNColors(IImI.iEntry.Value)) return false;

                return (this.Size == IImI.Size);
            }

            /// <summary>
            /// converts current image raw data of current IconImageInfo.
            /// </summary>
            /// <param name="NewIFormat"></param>
            public void ConvertThisImageFormat(IconImageFormat NewIFormat)
            {
                if (this.IconImageFormat == NewIFormat || NewIFormat == IconImageFormat.UNKNOWN) return;
                this.RawData = ConvertImageFormatRawData(this, ChangeIconImageFormat);//ConverIconImageFormat();
                IconDirEntry newEntry = entry.Value;
                newEntry.BytesInRes = RawData.Length;
                this.ChangeIconImageFormat = IconImageFormat.UNKNOWN;
                entry = newEntry;
            }

            /// <summary>
            /// To generate a new IconImageInfo with deferent formats from current IconImageInfo.
            /// </summary>
            /// <returns></returns>
            public IconImageInfo GenerateNew(Size newSize, IconImageFormat NewIFormat, ImageColorsTypes newImageColors)
            {
                Bitmap newImage = new Bitmap(newSize.Width, newSize.Height);
                using (Graphics igr = Graphics.FromImage(newImage))
                {
                    igr.DrawImage(this.Image, new Rectangle(0, 0, newSize.Width, newSize.Height));
                }
                //
                Icon TIcon = IconFromBitmap(newImage, null, null, true, newImageColors);
                IconImageInfo Res = new IconImageInfo(TIcon);
                TIcon.Dispose();

                Res.ConvertThisImageFormat(NewIFormat);
                return Res;
            }

            #endregion Other public Methods

            #region private methods

            ///// <summary>
            ///// Converts the RawData  [ bmp , png ] formats.
            ///// </summary>
            ///// <returns></returns>
            //private byte[] ConverIconImageFormat()
            //{
            //    if (ChangeIconImageFormat == IconImageFormat.UNKNOWN || IconImageFormat == ChangeIconImageFormat) return _RawData;

            //    //PixelFormat _ImagePixelFormat = Tools.GetPixelFormat((byte)entry.Value.BitCount);
            //    byte[] RawData_1 = null;
            //    if (ChangeIconImageFormat == IconImageFormat.BMP)
            //    {
            //        Icon TempIcon;
            //        using (MemoryStream sMS = new MemoryStream(_RawData))
            //        {
            //            Bitmap bmp = new Bitmap(sMS);
            //            TempIcon = IconFromBitmap(bmp, null, null, true, entry.Value.BitCount == 32 ? 0 : Tools.GetColorsTypeName((byte)entry.Value.BitCount));
            //        }
            //        using (IconImageInfo newIIF = new IconImageInfo(TempIcon))
            //        {
            //            RawData_1 = newIIF.RawData;
            //        }
            //        TempIcon.Dispose();
            //    }
            //    else //if (ChangeIconImageFormat == IconImageFormat.PNG)
            //    {
            //        using (MemoryStream sMS = new MemoryStream())
            //        {
            //            //Bitmap bmp = Icon.ToBitmap();
            //            Bitmap bmp = (Bitmap)Image.Clone();
            //            //Recover the PixelFormat to the icon because it was changed To 32bppArgb.
            //            //if (entry.Value.BitCount != 32)
            //            bmp = bmp.ConvertImagePixelFormat(Tools.GetPixelFormat((byte)entry.Value.BitCount));
            //            bmp.Save(sMS, ImageFormat.Png);
            //            bmp.Dispose();
            //            RawData_1 = sMS.ToArray();
            //        }
            //    }
            //    //var NewIIF = GetIconImageFormat(RawData_1);

            //    return RawData_1;
            //}
         
            
            #endregion private methods

            #region static Methods

            public static Bitmap BuildImage(IconImageInfo IIF)
            {
                Bitmap Res = null;
                if (IIF.IconImageFormat == IconImageFormat.BMP)
                {
                    using (MemoryStream sMS = new MemoryStream())
                    {
                        Icon TempIcon = BuildIcon(IIF.iEntry.Value, IIF.RawData);
                        if (TempIcon != null)
                        {
                            Res = TempIcon.ToBitmap();
                        }
                    }
                }
                else if (IIF.IconImageFormat == IconImageFormat.PNG)
                {
                    using (MemoryStream sMS = new MemoryStream(IIF.RawData))
                    {
                        Res = new Bitmap(sMS);
                    }
                }
                return Res;
            }

            /// <summary>
            ///  Build one icon from given info.
            /// </summary>
            /// <param name="entry"></param>
            /// <param name="RawData_1"></param>
            /// <param name="setErrorIconOnError"></param>
            /// <returns></returns>
            public static Icon BuildIcon(IconDirEntry entry, byte[] RawData_1, bool setErrorIconOnError = false)
            {
                Icon ResIcon = null;
                using (MemoryStream NewIMS = new MemoryStream())
                {
                    //
                    try
                    {
                        //Check Image format
                        if (RawData_1[0] == 0x89)//137 //PNG
                        {
                            Int16 _ReadInt16 = BitConverter.ToInt16(new byte[] { RawData_1[1], RawData_1[2] }, 0);

                            if (_ReadInt16 == 0x4E50) //_ReadInt16, 20048
                            {
                                //it's a compressed png. so decompress it to bmp. (No changes to RawData)
                                Bitmap pngImage;
                                using (MemoryStream MS = new MemoryStream(RawData_1))
                                {
                                    pngImage = new Bitmap(MS);

                                    //ResIcon = IconFromBitmap(pngImage, null, null, true, entry.BitCount == 32 ? 0 : ((byte)entry.BitCount).GetColorsTypeName());
                                    //
                                    //if (entry.BitCount != 32)
                                    //{
                                        //Recover the PixelFormat to the icon because it was changed To 32bppArgb when the new Bitmap was created from RawData.
                                        pngImage = pngImage.ConvertImagePixelFormat(Tools.GetPixelFormat((byte)entry.BitCount));
                                    //}
                                    ResIcon = Icon.FromHandle(pngImage.GetHicon());
                                    pngImage.Dispose();
                                }
                            }
                        }
                        else //(RawData_1[0]==40) //BMP
                        {
                            ////Create Icon dir header
                            IconDir IDir = IconDir.InitializeNew(1);

                            //Write the IconDir header.
                            NewIMS.Seek(0, SeekOrigin.Begin);
                            Tools.WriteStructure<IconDir>(NewIMS, IDir);
                            //IDir.Write(NewIMS);
                            ////
                            IconDirEntry newEntry = entry;

                            newEntry.ImageOffset = SizeOfIconDir + SizeOfIconDirEntry;
                            //Write the IconDirEntry to the stream.
                            NewIMS.Seek(SizeOfIconDir + 0 * SizeOfIconDirEntry, SeekOrigin.Begin);
                            Tools.WriteStructure<IconDirEntry>(NewIMS, newEntry);
                            //Write image raw data.
                            NewIMS.Seek(newEntry.ImageOffset, SeekOrigin.Begin);
                            NewIMS.Write(RawData_1, 0, newEntry.BytesInRes);
                            //SaveWithIconDir(NewIMS);
                            NewIMS.Seek(0, SeekOrigin.Begin);
                            //
                            //Create the icon.
                            ResIcon = new Icon(NewIMS);
                        }
                    }
                    finally
                    {
                        if (setErrorIconOnError && ResIcon == null) ResIcon = IconInfo.ErrorIcon();
                    }
                }
                return ResIcon;
            }
            //
            public static IconImageFormat GetIconImageFormat(Icon _Icon)
            {
                IconImageFormat res = IconImageFormat.UNKNOWN;
                using (IconImageInfo IIF_1 = new IconImageInfo(_Icon))
                {
                    res = IIF_1.IconImageFormat;
                }
                return res;
            }
            public static IconImageFormat GetIconImageFormat(Icon _ParentIcon, int iconIndex)
            {
                IconImageFormat res = IconImageFormat.UNKNOWN;
                using (IconImageInfo IIF_1 = new IconImageInfo(_ParentIcon, iconIndex))
                {
                    res = IIF_1.IconImageFormat;
                }
                return res;
            }
            public static IconImageFormat GetIconImageFormat(byte[] ImageBuffer)
            {
                byte bSignature = ImageBuffer[0];
                switch (bSignature)
                {
                    case 40: // BMP ?
                        return IconImageFormat.BMP;
                    case 0x89: // PNG ?  //137
                        if (BitConverter.ToInt16(new byte[] { ImageBuffer[1], ImageBuffer[2] }, 0) == 0x4E50) //_ReadInt16, 20048
                            return IconImageFormat.PNG;
                        break;
                }
                return IconImageFormat.UNKNOWN;
            }
            public static unsafe IconImageFormat GetIconImageFormat(Stream ImageStream)
            {
                long streamPos = ImageStream.Position;

                IconImageFormat res = IconImageFormat.UNKNOWN;
                try
                {
                    BinaryReader br = new BinaryReader(ImageStream);
                    byte[] array = new byte[sizeof(BITMAPINFOHEADER)];
                    byte bSignature = br.ReadByte();
                    switch (bSignature)
                    {
                        case 40: // BMP ?
                            res = IconImageFormat.BMP;
                            break;

                        case 0x89: // PNG ?  //137
                            if (br.ReadInt16() == 0x4E50) //20048
                                res = IconImageFormat.PNG;
                            break;
                    }
                }
                finally
                {
                    ImageStream.Position = streamPos;
                }
                return res;
            }
            //
            public static byte[] ConvertImageFormatRawData(IconImageInfo SourceIImI, IconImageFormat newIconImageFormat, ImageColorsTypes? newColors = null)
            {
                if (SourceIImI.IconImageFormat == newIconImageFormat || newIconImageFormat == IconImageFormat.UNKNOWN) return SourceIImI.RawData;

                byte _BitCount;
                if (newColors.HasValue)
                {
                    _BitCount = newColors.Value.GetBitCount();
                }
                else
                {
                    _BitCount = (byte)SourceIImI.entry.Value.BitCount;
                }

                byte[] Res = null;
                if (newIconImageFormat == IconImageFormat.BMP)
                {
                    Icon TempIcon;
                    using (MemoryStream sMS = new MemoryStream(SourceIImI.RawData))
                    {
                        Bitmap bmp = new Bitmap(sMS);
                        //TempIcon = IconFromBitmap(bmp, null, null, true, SourceIImI.entry.Value.BitCount == 32 ? 0 : Tools.GetColorsTypeName((byte)SourceIImI.entry.Value.BitCount));
                        TempIcon = IconFromBitmap(bmp, null, null, true, Tools.GetColorsTypeName(_BitCount));
                    }
                    using (IconImageInfo newIIF = new IconImageInfo(TempIcon))
                    {
                        Res = newIIF.RawData;
                    }
                    TempIcon.Dispose();
                }
                else  //if (newIconImageFormat == IconImageFormat.PNG)
                {

                    using (MemoryStream sMS = new MemoryStream())
                    {
                        //Bitmap bmp = Icon.ToBitmap();
                        Bitmap bmp = (Bitmap)SourceIImI.Image.Clone();
                        //Recover the PixelFormat to the icon because it was changed To 32bppArgb.
                        //if (entry.Value.BitCount != 32)
                        bmp = bmp.ConvertImagePixelFormat(Tools.GetPixelFormat(_BitCount));
                        bmp.Save(sMS, ImageFormat.Png);
                        bmp.Dispose();
                        Res = sMS.ToArray();
                    }

                }
                return Res;
            }
           //
            public static IconImageInfo FromImage(Bitmap Image_1, IconImageFormat? newIconImageFormat =null, ImageColorsTypes newColors = ImageColorsTypes.Unknown)
            {
                Icon IconFI = IconFromBitmap(Image_1,BitCount_: newColors);
                IconImageInfo Res = new IconImageInfo(IconFI);
                IconFI.Dispose();
                if (newIconImageFormat.HasValue) Res.ConvertThisImageFormat(newIconImageFormat.Value);
                return Res;
            }
            #endregion static Methods

            #region IDisposable Members
            private bool _IconIsFromSource;
            public bool IconIsFromSource { get { return _IconIsFromSource; } }

            ~IconImageInfo() { Dispose(); }
            public void Dispose()
            {
                RawData = null;
                entry = null;
                //if (!_IconIsFromSource && Icon_1 != null) Icon_1.Dispose();
                if (Image_1 != null) Image_1.Dispose();
            }

            #endregion IDisposable Members

        }//IconImageInfo


        #endregion IconImageInfo Class

    }

    public static partial class Extensions
    {

            

        public static bool ContainsIconWithColors(this List<IconInfo.IconImageInfo> ImageList, IconInfo.IconImageInfo IconImageInfo_)
        {
            var Find_1 = ImageList.Find((res) => res.CheckSameSizeNColors(IconImageInfo_));
            return (Find_1 != null);
        }
        public static bool ContainsIconWithColors(this List<IconInfo.IconImageInfo> ImageList, Icon Icon_)
        {
            return ContainsIconWithColors(ImageList, ReadIconDirEntry(Icon_));
        }
        public static bool ContainsIconWithColors(this List<IconInfo.IconImageInfo> ImageList, IconDirEntry IconDirEntry_)
        {
            return ContainsIconWithColors(ImageList, IconDirEntry_.GetSize(), IconDirEntry_.ColorsType);
        }
        public static bool ContainsIconWithColors(this List<IconInfo.IconImageInfo> ImageList, OneIconInfo oneIconInfo)
        {
            return ImageList.ContainsIconWithColors(oneIconInfo.Size, oneIconInfo.ImageColorsType);
        }
        public static bool ContainsIconWithColors(this List<IconInfo.IconImageInfo> ImageList, Size Size_, ImageColorsTypes ColorsType_)
        {
            var Find_1 = ImageList.Find((res) => res.CheckSameSizeNColors(Size_, ColorsType_));
            return (Find_1 != null);
        }
        //
        public static IconDirEntry ReadIconDirEntry(this Icon Icon_, bool UseBinaryReader = false)
        {
            IconDirEntry? entry = null;
            //
            using (MemoryStream inputStream = new MemoryStream())
            {
                Icon_.Save(inputStream);
                //
                //inputStream.Position = 0;
                entry = ReadIconDirEntry(inputStream, UseBinaryReader);
            }
            //
            //var newIInf = new IconInfo(Icon_);
            //entry = newIInf.IconDirEntries[newIInf.BestFitIconIndex];
            //newIInf.Dispose();
            //CheckAndRepairEntry(entry)
            return entry.Value;
        }
        public static IconDirEntry ReadIconDirEntry(Stream iconStream, bool UseBinaryReader = false)
        {
            var new_entry = new IconDirEntry();
            var SPosition = iconStream.Position;
            iconStream.Seek(IconInfo.SizeOfIconDir, SeekOrigin.Begin);
            if (UseBinaryReader)
            {
                BinaryReader br = new BinaryReader(iconStream);
                //Read the IconDirEntry
                new_entry.Width = br.ReadByte();
                new_entry.Height = br.ReadByte();
                new_entry.ColorCount = br.ReadByte();
                new_entry.Reserved = br.ReadByte();
                //
                new_entry.Planes = br.ReadInt16();
                new_entry.BitCount = br.ReadInt16();
                new_entry.BytesInRes = br.ReadInt32();
                new_entry.ImageOffset = br.ReadInt32();
                //

                //iconStream.Position = new_entry.ImageOffset
                //'Read the BitmapInfoHeader
                //br.ReadInt32() '_bhSize
                //br.ReadInt32() '_width
                //br.ReadInt32() ' / 2'_height
                //br.ReadInt16() '_planes
                //new_entry.BitCount = br.ReadInt16
                //br.Dispose
            }
            else
            {
                new_entry = Tools.ReadStructure<IconDirEntry>(iconStream);
                new_entry.ImageOffset = IconInfo.SizeOfIconDir + IconInfo.SizeOfIconDirEntry;
            }
            iconStream.Position = SPosition;
            //new_entry = CheckAndRepairEntry(new_entry);
            new_entry.CheckAndRepairEntry();
            return new_entry;
        }
        private static unsafe IconDirEntry CheckAndRepairEntry(this IconDirEntry entry)
        {
            // If there is missing information in the header... lets try to calculate it
            //if (entry.Width == 0 || entry.Height == 0) {
            //  entry.Width=256;
            //  entry.Height = 256;
            //}

            if (entry.BitCount == 0)
            {
                int iHeight = entry.Height; //!= 0 ? entry.Height : 256;
                int iWidth = entry.Width; //!= 0 ? entry.Width : 256;
                //
                int stride, CLSSize, palette;
                int bmpSize = (ushort)(entry.BytesInRes - sizeof(BITMAPINFOHEADER));
                int BWStride = ((iWidth * 1 + 31) & ~31) >> 3;

                int BWSize = BWStride * iHeight;
                bmpSize -= BWSize;

                // Lets find the value;
                byte[] bpp = { 1, 4, 8, 16, 24, 32 };
                int j = 0;
                while (j <= 5)
                {
                    stride = ((iWidth * bpp[j] + 31) & ~31) >> 3;
                    CLSSize = iHeight * stride;
                    palette = bpp[j] <= 8 ? ((int)(1 << bpp[j]) * 4) : 0;
                    if (palette + CLSSize == bmpSize)
                    {
                        entry.BitCount = bpp[j];
                        break;
                    }
                    j++;
                }
                //gess 32A
                if (entry.BitCount == 0) { entry.BitCount = 32; }
            }

            if (entry.BitCount < 8 && entry.ColorCount == 0) entry.ColorCount = (byte)(1 << entry.BitCount);
            if (entry.Planes == 0) entry.Planes = 1;

            return entry;
        }
         //
        public static List<Bitmap> GetAllImages(this List<IconInfo.IconImageInfo> ImageList)
        {
            List<Bitmap> newL = new List<Bitmap>();

            if (ImageList == null) return newL;
            foreach (IconInfo.IconImageInfo ic_ in ImageList)
            {
                newL.Add(ic_.Image);
            }

            return newL;
        }
        public static List<IconDirEntry> GetAllIconDirEntries(this List<IconInfo.IconImageInfo> ImageList)
        {
            List<IconDirEntry> newL = new List<IconDirEntry>();
            if (ImageList == null) return newL;
            foreach (IconInfo.IconImageInfo ic_ in ImageList)
            {
                newL.Add(ic_.iEntry.Value);
            }

            return newL;
        }
        public static List<GRPICONDIRENTRY> GetAllGroupIconDirEntry(this List<IconInfo.IconImageInfo> ImageList)
        {
            List<GRPICONDIRENTRY> newL = new List<GRPICONDIRENTRY>();
            if (ImageList == null) return newL;
            foreach (IconInfo.IconImageInfo ic_ in ImageList)
            {
                newL.Add(ic_.iEntry.Value.ToGroupIconDirEntry(ic_.Index));
            }
            
            return newL;
        }


    }

}
