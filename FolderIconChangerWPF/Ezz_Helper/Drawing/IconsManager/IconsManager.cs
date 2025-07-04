using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

//v 1.1.1.2 Beta
namespace Ezz_Helper.Drawing.IconsManager      //TODO : add LibraryFormats check
{
    #region " Enumerations"

    //[Flags]
    public enum IconFlags : int
    {
        Icon = 0x100,

        // get icon
        LinkOverlay = 0x8000,

        // put a link overlay on icon
        Selected = 0x10000,

        // show icon in selected state
        LargeIcon = 0x0,

        // get large icon
        SmallIcon = 0x1,

        // get small icon
        OpenIcon = 0x2,

        // get open icon
        ShellIconSize = 0x4

        // get shell size icon
    }

    /// <summary>
    /// List of image colors type in bits
    /// </summary>
    /// <remarks></remarks>
    public enum ImageColorsTypes : byte
    {
        /// <summary>
        /// Monochrome (Indexed colors- 1 bits)
        /// </summary>
        /// <remarks></remarks>
        Monochrome = 1,

        /// <summary>
        /// 16 (Indexed colors - 4 bits)
        /// </summary>
        /// <remarks></remarks>
        _16_IndexedColors = 4,

        /// <summary>
        /// 256 (Indexed colors - 8 bits)
        /// </summary>
        /// <remarks></remarks>
        _256_IndexedColors = 8,

        /// <summary>
        /// True colors (RGB - 24 bits)
        /// </summary>
        /// <remarks></remarks>
        True_Colors = 24,

        /// <summary>
        /// Alpha channel (RGB - 32 bits)
        /// </summary>
        /// <remarks></remarks>
        Alpha_Channel = 32,

        //
        Unknown = 0,
    }

    public enum ResourceType : uint
    {
        RT_CURSOR = 1,
        RT_BITMAP = 2,
        RT_ICON = 3,
        RT_MENU = 4,
        RT_DIALOG = 5,
        RT_STRING = 6,
        RT_FONTDIR = 7,
        RT_FONT = 8,
        RT_ACCELERATOR = 9,
        RT_RCDATA = 10,
        RT_MESSAGETABLE = 11,
        RT_GROUP_CURSOR = 12,
        RT_GROUP_ICON = 14,
        RT_VERSION = 16,
        RT_DLGINCLUDE = 17,
        RT_PLUGPLAY = 19,
        RT_VXD = 20,
        RT_ANICURSOR = 21,
        RT_ANIICON = 22,
        RT_HTML = 23
    }

    #endregion " Enumerations"

    #region "Structures"

    /// <summary>
    /// Presents an Icon Directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]  //Size = 6
    public struct IconDir
    {
        public Int16 Reserved; // Reserved (must be 0)
        public Int16 Type;    // Resource Type (1 for icons)
        public Int16 Count;     // How many images?

        public IconDir(Stream Stream_1)
        {
            this = Tools.ReadStructure<IconDir>(Stream_1);
        }

        public IconDir(Int16 Count_1 = 1)
        {
            Reserved = 0;
            Type = 1;
            Count = Count_1;
        }

        public static IconDir InitializeNew(Int16 Count_1 = 1)
        {
            return new IconDir(Count_1);
        }

        public void Write(Stream Stream_1)
        {
            Stream_1.Seek(0, SeekOrigin.Begin);
            Tools.WriteStructure<IconDir>(Stream_1, this);
        }

        public void Read(Stream Stream_1)
        {
            this = Tools.ReadStructure<IconDir>(Stream_1);
        }

        /// <summary>
        /// Converts the current IconsManager.IconDir into IconsManager.GroupIconDir.
        /// </summary>
        /// <returns>IconsManager.GroupIconDir</returns>
        public GRPICONDIR ToGroupIconDir()
        {
            GRPICONDIR grpDir = new GRPICONDIR();
            grpDir.Reserved = this.Reserved;
            grpDir.Type = this.Type;
            grpDir.Count = this.Count;
            return grpDir;
        }
    }

    /// <summary>
    /// Presents an Icon Directory Entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]  //Size = 6
    public struct IconDirEntry
    {
        /// <summary>
        /// Width, in pixels, of the image
        /// </summary>
        /// <remarks></remarks>
        public byte Width;

        //
        /// <summary>
        /// Height, in pixels, of the image
        /// </summary>
        /// <remarks></remarks>
        public byte Height;

        public Size GetSize()
        {
            return new Size(Width, Height);
        }

        //
        /// <summary>
        /// Number of colors in image (0 if >=8bpp)
        /// </summary>
        /// <remarks></remarks>
        public byte ColorCount;

        //
        /// <summary>
        /// Reserved ( must be 0)
        /// </summary>
        /// <remarks></remarks>
        public byte Reserved;

        //
        /// <summary>
        /// Color Planes
        /// </summary>
        /// <remarks></remarks>
        public Int16 Planes;

        //
        /// <summary>
        /// Bits per pixel
        /// </summary>
        /// <remarks></remarks>
        public Int16 BitCount;

        //
        /// <summary>
        /// How many bytes in this resource?
        /// </summary>
        /// <remarks></remarks>
        public Int32 BytesInRes;

        //
        /// <summary>
        /// Where in the file in this image?
        /// </summary>
        /// <remarks></remarks>
        public Int32 ImageOffset;

        /// <summary>
        /// Gets ColorsType name from BitCount
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ImageColorsTypes ColorsType
        {
            get { return Tools.GetColorsTypeName((byte)BitCount); }
        }

        public bool CheckSameSizeNColors(Size Size_, ImageColorsTypes ColorsType_)
        {
            if (Width != Size_.Width) return false;
            if (Height != Size_.Height) return false;
            if (ColorsType != ColorsType_) return false;
            return true;
        }

        public bool CheckSameSizeNColors(IconDirEntry IconDirEntry_)
        {
            if (Width != IconDirEntry_.Width) return false;
            if (Height != IconDirEntry_.Height) return false;
            if (BitCount != IconDirEntry_.BitCount) return false;
            return true;
        }

        public bool Equals_2(IconDirEntry IDEn2)
        {
            return Equals_2(this, IDEn2);
        }

        public static bool Equals_2(IconDirEntry IDEn1, IconDirEntry IDEn2)
        {
            if (IDEn1.Width != IDEn2.Width)
                return false;
            if (IDEn1.Height != IDEn2.Height)
                return false;
            if (IDEn1.BitCount != IDEn2.BitCount)
                return false;
            if (IDEn1.BytesInRes != IDEn2.BytesInRes)
                return false;
            if (IDEn1.ColorCount != IDEn2.ColorCount)
                return false;
            if (IDEn1.ImageOffset != IDEn2.ImageOffset)
                return false;
            if (IDEn1.Planes != IDEn2.Planes)
                return false;
            if (IDEn1.Reserved != IDEn2.Reserved)
                return false;
            return true;
        }

        /// <summary>
        /// Converts the current IconsManager.IconDirEntry into IconsManager.GroupIconDirEntry.
        /// </summary>
        /// <param name="id">The resource identifier.</param>
        /// <returns>IconsManager.GroupIconDirEntry</returns>
        public GRPICONDIRENTRY ToGroupIconDirEntry(int id)
        {
            GRPICONDIRENTRY grpEntry = new GRPICONDIRENTRY();
            grpEntry.Width = this.Width;
            grpEntry.Height = this.Height;
            grpEntry.ColorCount = this.ColorCount;
            grpEntry.Reserved = this.Reserved;
            grpEntry.Planes = this.Planes;
            grpEntry.BitCount = this.BitCount;
            grpEntry.BytesInRes = this.BytesInRes;
            grpEntry.ID = (short)id;
            //DirectCast(id, Short)
            return grpEntry;
        }
    }

    /// <summary>
    /// Presents a Group Icon Directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]  //Size = 6
    public struct GRPICONDIR
    {
        public Int16 Reserved;   // Reserved (must be 0)
        public Int16 Type;   // Resource Type (1 for icons)
        public Int16 Count;
        // How many images?
        //public GRPICONDIRENTRY[] Entries;

        public GRPICONDIR(Int16 reserved, Int16 type, Int16 count)
        {
            Reserved = reserved;
            Type = type;
            Count = count;
            //Entries = new GRPICONDIRENTRY[0];
        }

        public static GRPICONDIR Initalizated
        {
            get { return new GRPICONDIR(0, 1, 0); }
        }

        /// <summary>
        /// Converts the current IconsManager.GroupIconDir into IconsManager.IconDir.
        /// </summary>
        /// <returns>IconsManager.IconDir</returns>
        public IconDir ToIconDir()
        {
            IconDir dir = new IconDir();
            dir.Reserved = this.Reserved;
            dir.Type = this.Type;
            dir.Count = this.Count;
            return dir;
        }
    }

    /// <summary>
    /// Presents a Group Icon Directory Entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]  //Size = 14)]
    public struct GRPICONDIRENTRY
    {
        public byte Width;

        // Width, in pixels, of the image
        public byte Height;

        // Height, in pixels, of the image
        public byte ColorCount;

        // Number of colors in image (0 if >=8bpp)
        public byte Reserved;

        // Reserved ( must be 0)
        public Int16 Planes;

        // Color Planes
        public Int16 BitCount;

        // Bits per pixel
        public Int32 BytesInRes;

        // How many bytes in this resource?
        public Int16 ID;

        // the ID

        /// <summary>
        /// Gets ColorsType name from BitCount
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ImageColorsTypes ColorsType
        {
            get { return Tools.GetColorsTypeName((byte)BitCount); }
        }

        public static bool Equals_2(GRPICONDIRENTRY gIDEn1, GRPICONDIRENTRY gIDEn2)
        {
            if (gIDEn1.Width != gIDEn2.Width)
                return false;
            if (gIDEn1.Height != gIDEn2.Height)
                return false;
            if (gIDEn1.BitCount != gIDEn2.BitCount)
                return false;
            if (gIDEn1.BytesInRes != gIDEn2.BytesInRes)
                return false;
            if (gIDEn1.ColorCount != gIDEn2.ColorCount)
                return false;
            if (gIDEn1.Planes != gIDEn2.Planes)
                return false;
            if (gIDEn1.Reserved != gIDEn2.Reserved)
                return false;
            if (gIDEn1.ID != gIDEn2.ID)
                return false;
            return true;
        }

        /// <summary>
        /// Converts the current IconsManager.GroupIconDirEntry into IconsManager.IconDirEntry.
        /// </summary>
        /// <param name="imageOffiset">The resource identifier.</param>
        /// <returns>IconsManager.IconDirEntry</returns>
        public IconDirEntry ToIconDirEntry(int imageOffiset)
        {
            IconDirEntry entry = new IconDirEntry();
            entry.Width = this.Width;
            entry.Height = this.Height;
            entry.ColorCount = this.ColorCount;
            entry.Reserved = this.Reserved;
            entry.Planes = this.Planes;
            entry.BitCount = this.BitCount;
            entry.BytesInRes = this.BytesInRes;
            entry.ImageOffset = imageOffiset;
            return entry;
        }
    }

    //'

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct BITMAPINFOHEADER
    {
        public UInt32 biSize;
        public UInt32 biWidth;
        public UInt32 biHeight;
        public UInt16 biPlanes;
        public UInt16 biBitCount;
        public IconImageFormat biCompression;
        public UInt32 biSizeImage;
        public Int32 biXPelsPerMeter;
        public Int32 biYPelsPerMeter;
        public UInt32 biClrUsed;
        public UInt32 biClrImportant;
    }

    public enum IconImageFormat : int
    {
        //BI_RGB = 0
        //BI_RLE8 = 1
        //BI_RLE4 = 2
        //BI_BITFIELDS = 3
        //BI_JPEG = 4
        BMP = 0,

        PNG = 5,

        //BI_PNG = 5
        UNKNOWN = 255
    }

    //Enum BitmapCompressionMode As UInteger
    //    BI_RGB = 0
    //    BI_RLE8 = 1
    //    BI_RLE4 = 2
    //    BI_BITFIELDS = 3
    //    BI_JPEG = 4
    //    BI_PNG = 5
    //    UNKNOWN = 255
    //End Enum
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;

        public byte rgbReserved;

        public void Set(byte r, byte g, byte b)
        {
            rgbRed = r;
            rgbGreen = g;
            rgbBlue = b;
        }
    }

    //
    public static class Sizes
    {
        public static readonly Size px_16x16 = new Size(16, 16);
        public static readonly Size px_24x24 = new Size(24, 24);
        public static readonly Size px_32x32 = new Size(32, 32);
        public static readonly Size px_48x48 = new Size(48, 48);
        public static readonly Size px_64x64 = new Size(64, 64);
        public static readonly Size px_72x72 = new Size(72, 72);  //
        public static readonly Size px_80x80 = new Size(80, 80);//
        public static readonly Size px_96x96 = new Size(96, 96);
        public static readonly Size px_128x128 = new Size(128, 128);
        public static readonly Size px_256x256 = new Size(256, 256);
        public static readonly Size px_512x512 = new Size(512, 512);

        //
        public static Size[] GetAll()
        {
            return new Size[] {
			px_16x16,
			px_24x24,
			px_32x32,
			px_48x48,
			px_64x64,
			px_72x72,
			px_80x80,
			px_96x96,
			px_128x128,
			px_256x256,
            px_512x512
		};
        }

        //public static bool Equals_1(this Size S1, Size S2)
        //{
        //    return ((S1.Width == S2.Width) && (S1.Height == S2.Height));
        //}
        public static string SizeToShourtSring(int Width_, int Height_, string Format_ = "{0}x{1}")
        {
            return string.Format(Format_, Width_, Height_);
        }

        public static string SizeToShourtSring(Size Size_, string Format_ = "{0}x{1}")
        {
            return string.Format(Format_, Size_.Width, Size_.Height);
        }

        public class SizeEqualityComparer : IEqualityComparer<Size>
        {
            public static SizeEqualityComparer InitializeNew()
            {
                return new SizeEqualityComparer();
            }

            #region IEqualityComparer<Size> Members

            public bool Equals(Size x, Size y)
            {
                //return x.Equals(y);
                return ((x.Width == y.Width) && (x.Height == y.Height));
            }

            public Int32 GetHashCode(Size obj)
            {
                return obj.GetHashCode(); ;
            }

            #endregion IEqualityComparer<Size> Members
        }
    }

    #endregion "Structures"

    #region "Microsoft.API"

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
    public delegate bool EnumResNameProc(IntPtr hModule, int lpszType, IntPtr lpszName, IconExtractor.IconResInfo lParam);

    //[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
    //public delegate bool EnumResNameProc(IntPtr hModule, ResourceTypes lpszType, IntPtr lpszName, IntPtr lParam);
    //public delegate bool EnumResNameProc(IntPtr hModule, IntPtr pType, IntPtr pName, IntPtr param);
    public delegate Int32 EnumResTypeProc(IntPtr hModule, IntPtr lpszType, IntPtr lParam);

    #region " Enumurations"

    public enum LoadLibraryFlags
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010
    }

    //[Flags()]
    //public enum LoadLibraryExFlags : int
    //{
    //    DONT_RESOLVE_DLL_REFERENCES = 0x1,
    //    LOAD_LIBRARY_AS_DATAFILE = 0x2,
    //    LOAD_WITH_ALTERED_SEARCH_PATH = 0x8
    //}
    public enum GetLastErrorResult : int
    {
        ERROR_SUCCESS = 0,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_BAD_EXE_FORMAT = 193,
        ERROR_RESOURCE_TYPE_NOT_FOUND = 1813
    }

    public enum ResourceTypes : int
    {
        RT_ICON = 3,
        RT_GROUP_ICON = 14
    }

    public enum LookupIconIdFromDirectoryExFlags : int
    {
        LR_DEFAULTCOLOR = 0,
        LR_MONOCHROME = 1
    }

    public enum LoadImageTypes : int
    {
        IMAGE_BITMAP = 0,
        IMAGE_ICON = 1,
        IMAGE_CURSOR = 2
    }

    [Flags()]
    public enum SHGetFileInfoFlags : int
    {
        Icon = 0x100,

        // get icon
        DisplayName = 0x200,

        // get display name
        TypeName = 0x400,

        // get type name
        Attributes = 0x800,

        // get attributes
        IconLocation = 0x1000,

        // get icon location
        ExeType = 0x2000,

        // return exe type
        SysIconIndex = 0x4000,

        // get system icon index
        LinkOverlay = 0x8000,

        // put a link overlay on icon
        Selected = 0x10000,

        // show icon in selected state
        AttrSpecified = 0x20000,

        // get only specified attributes
        LargeIcon = 0x0,

        // get large icon
        SmallIcon = 0x1,

        // get small icon
        OpenIcon = 0x2,

        // get open icon
        ShellIconSize = 0x4,

        // get shell size icon
        PIDL = 0x8,

        // pszPath is a pidl
        UseFileAttributes = 0x10

        // use passed dwFileAttribute
    }

    #endregion " Enumurations"

    #region " Structures"

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    #endregion " Structures"

    public sealed class Win32
    {
        public const int MAX_PATH = 260;

        #region " Helper Functions"

        public static bool IsIntResource(IntPtr lpszName)
        {
            return ((lpszName.ToInt32() >> 16) == 0);
        }

        #endregion " Helper Functions"

        #region " API Functions"

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        #region SHELL32

        [DllImport("user32.dll")]
        public static extern int LookupIconIdFromDirectory(byte[] presbits, bool fIcon);

        [DllImport("user32.dll")]
        public static extern int LookupIconIdFromDirectory(IntPtr presbits, bool fIcon);

        [DllImport("user32.dll")]
        public static extern int LookupIconIdFromDirectoryEx(byte[] presbits, bool fIcon,
          int cxDesired, int cyDesired, uint Flags);

        [DllImport("user32.dll")]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, bool fIcon,
          int cxDesired, int cyDesired, uint Flags);

        [DllImport("user32.dll")]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, bool fIcon,
          int cxDesired, int cyDesired, LookupIconIdFromDirectoryExFlags Flags);

        //
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
          int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, SHGetFileInfoFlags uFlags);

        #endregion SHELL32

        #endregion " API Functions"

        #region KERNEL32

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, int lpType);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);
        //[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        //public static extern int SizeofResource(IntPtr hModule, IntPtr hResource);

        //

        ////[return: MarshalAs(UnmanagedType.Bool)] /* unnecessary, isn't it? */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        //[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        //public static extern int FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        public static extern void FreeLibraryAndExitThread(IntPtr hModule, uint dwExitCode);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        //[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, ResourceTypes lpType);

        //
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "CopyMemory")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(RGBQUAD dest, byte src, int cb);

        [DllImport("KERNEL32.DLL")]
        public unsafe static extern void CopyMemory(void* dest, void* src, int length);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UpdateResource(IntPtr hUpdate, uint lpType, ref string pName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool UpdateResource(IntPtr hUpdate, uint lpType, IntPtr pName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public unsafe static extern bool UpdateResource(IntPtr hUpdate, uint lpType, byte[] pName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UpdateResource(IntPtr hUpdate, uint lpType, uint lpName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public unsafe static extern void CopyMemory(RGBQUAD* dest, byte* src, int cb);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        //[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        //public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);
        //[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResource);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LockResource(IntPtr hGlobalResource);

        //[DllImport("kernel32.dll")]
        //public static extern IntPtr LockResource(IntPtr hResData);
        //[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        //public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumResourceNames(IntPtr hModule, int lpszType, EnumResNameProc lpEnumFunc, IconExtractor.IconResInfo lParam);

        //[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern bool EnumResourceNames(IntPtr hModule, ResourceTypes lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr pType, EnumResNameProc callback, IntPtr param);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EnumResourceTypes(IntPtr hModule, EnumResTypeProc callback, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, string resourceID, IntPtr type);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, Int32 resourceID, IntPtr type);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr resourceID, IntPtr type);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr resourceID, string resourceName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        //[DllImport("kernel32.dll")]
        //public static extern IntPtr LoadLibrary(string libraryName);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string path, IntPtr hFile, LoadLibraryFlags flags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, int dwFlags);

        //[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryExFlags dwFlags);

        #endregion KERNEL32
    }

    #endregion "Microsoft.API"

    /// <summary>
    /// Represents a resource name (either integer resource or string resource).
    /// </summary>
    public class ResourceName : IDisposable
    {
        #region " Properties"

        private System.Nullable<int> _id;

        /// <summary>
        /// Gets the resource identifier, returns null if the resource is not an integer resource.
        /// </summary>
        public System.Nullable<int> Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        private string _name;

        /// <summary>
        /// Gets the resource name, returns null if the resource is not a string resource.
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private IntPtr _value;

        /// <summary>
        /// Gets a pointer to resource name that can be used in FindResource function.
        /// </summary>
        public IntPtr Value
        {
            get
            {
                if (this.IsIntResource)
                {
                    return new IntPtr(this.Id.Value);
                }

                if (this._value == IntPtr.Zero)
                {
                    this._value = Marshal.StringToHGlobalAuto(this.Name);
                }

                return _value;
            }
            private set { _value = value; }
        }

        /// <summary>
        /// Gets whether the resource is an integer resource.
        /// </summary>
        public bool IsIntResource
        {
            get { return (this.Id.HasValue); }
        }

        //return (this.Id.HasValue); }

        #endregion " Properties"

        #region " Constructor/Destructor"

        /// <summary>
        /// Initializes a new ResourceName object.
        /// </summary>
        /// <param name="lpName">Specifies the resource name. For more ifnormation, see the Remarks section.</param>
        /// <remarks>
        /// If the high bit of lpszName is not set (=0), lpszName specifies the integer identifier of the givin resource.
        /// Otherwise, it is a pointer to a null terminated string.
        /// If the first character of the string is a pound sign (#), the remaining characters represent a decimal number that specifies the integer identifier of the resource. For example, the string "#258" represents the identifier 258.
        /// #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0).
        /// </remarks>
        public ResourceName(IntPtr lpName)
        {
            if (((uint)lpName >> 16) == 0)
            {
                //Integer resource
                this.Id = lpName.ToInt32();
                this.Name = null;
            }
            else
            {
                this.Id = null;
                this.Name = Marshal.PtrToStringAuto(lpName);
            }
        }

        /// <summary>
        /// Destructs the ResourceName object.
        /// </summary>
        private ResourceName()
        {
            Dispose();
        }

        #endregion " Constructor/Destructor"

        #region " Public Functions"

        /// <summary>
        /// Returns a System.String that represents the current IconsManager.ResourceName.
        /// </summary>
        /// <returns>Returns a System.String that represents the current IconsManager.ResourceName.</returns>
        public override string ToString()
        {
            if (this.IsIntResource)
            {
                return "#" + this.Id.ToString();
            }

            return this.Name;
        }

        /// <summary>
        /// Releases the pointer to the resource name.
        /// </summary>
        public void Free()
        {
            if (this._value != IntPtr.Zero)
            {
                try
                {
                    Marshal.FreeHGlobal(this._value);
                }
                catch
                {
                }
                this._value = IntPtr.Zero;
            }
        }

        #endregion " Public Functions"

        #region " IDisposable Members"

        /// <summary>
        /// Release the pointer to the resource name.
        /// </summary>
        public void Dispose()
        {
            Free();
        }

        #endregion " IDisposable Members"
    }

    /// <summary>
    /// Holds a set of utilities(Tools).
    /// </summary>
    internal static class Tools
    {
        #region " Stream Utilities"

        /// <summary>
        /// Reads a structure of type T from the input stream.
        /// </summary>
        /// <typeparam name="T">The structure type to be read.</typeparam>
        /// <param name="inputStream">The input stream to read from.</param>
        /// <returns>A structure of type T that was read from the stream.</returns>
        public unsafe static T ReadStructure<T>(this Stream inputStream) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            inputStream.ReadExactly(buffer, 0, size);
            //
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, ptr, size);
            object ret = Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            //
            return (T)ret;
        }

        /// <summary>
        /// Writes as structure of type T to the output stream.
        /// </summary>
        /// <typeparam name="T">The structure type to be written.</typeparam>
        /// <param name="outputStream">The output stream to write to.</param>
        /// <param name="structure">The structure to be written.</param>
        public unsafe static void WriteStructure<T>(this Stream outputStream, T structure) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            //
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            //
            outputStream.Write(buffer, 0, size);
        }

        #endregion " Stream Utilities"

        public static byte GetBitCount(this ImageColorsTypes ImageColorsTypes_)
        {
            return (byte)ImageColorsTypes_;
        }

        public static byte GetBitCount(this PixelFormat pixelFormat__1)
        {
            switch (pixelFormat__1)
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

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;

                default:
                    return 0;
            }
        }

        public static bool CompareRGBQUADToColor(RGBQUAD rgbQuad, Color color)
        {
            return rgbQuad.rgbRed == color.R && rgbQuad.rgbGreen == color.G && rgbQuad.rgbBlue == color.B;
        }

        public static RGBQUAD[] RGBQUADFromColorArray(Bitmap bmp)
        {
            // Some programs as Axialis have problems with a reduced palette, so lets create a full palette
            int bits = Tools.GetBitCount(bmp.PixelFormat);
            RGBQUAD[] rgbArray = new RGBQUAD[bits <= 8 ? (1 << bits) : 0];
            Color[] entries = bmp.Palette.Entries;
            for (int i = 0; i < entries.Length; i++)
            {
                rgbArray[i].rgbRed = entries[i].R;
                rgbArray[i].rgbGreen = entries[i].G;
                rgbArray[i].rgbBlue = entries[i].B;
            }
            return rgbArray;
        }

        //
        public static PixelFormat GetPixelFormat(this ImageColorsTypes ImageColorsTypes_)
        {
            return GetPixelFormat((byte)ImageColorsTypes_);
        }

        public static PixelFormat GetPixelFormat(this byte BitCount)
        {
            switch (BitCount)
            {
                case (byte)ImageColorsTypes.Monochrome:
                    //1
                    return PixelFormat.Format1bppIndexed;

                case (byte)ImageColorsTypes._16_IndexedColors:
                    //4
                    return PixelFormat.Format4bppIndexed;

                case (byte)ImageColorsTypes._256_IndexedColors:
                    //8
                    return PixelFormat.Format8bppIndexed;

                case (byte)ImageColorsTypes.True_Colors:
                    //24
                    return PixelFormat.Format24bppRgb;

                case (byte)ImageColorsTypes.Alpha_Channel:
                    //32
                    //, PixelFormat.Format32bppPArgb, PixelFormat.Format32bppRgb
                    return PixelFormat.Format32bppArgb;

                case 64:
                    //, PixelFormat.Format64bppPArgb
                    return PixelFormat.Format64bppArgb;

                case 16:
                    return PixelFormat.Format16bppArgb1555;

                default:
                    return 0;
            }
        }

        public static ImageColorsTypes GetColorsTypeName(this PixelFormat pixelFormat__1)
        {
            //Dim BitCount As byte = BitCountFromPixelFormat(pixelFormat__1)
            //Return GetColorsTypeNameFromBitCount(BitCount)
            return (ImageColorsTypes)GetBitCount(pixelFormat__1);
        }

        public static ImageColorsTypes GetColorsTypeName(this byte BitCount)
        {
            switch (BitCount)
            {
                case (byte)ImageColorsTypes.Monochrome:
                    return ImageColorsTypes.Monochrome;

                case (byte)ImageColorsTypes._16_IndexedColors:
                    return ImageColorsTypes._16_IndexedColors;

                case (byte)ImageColorsTypes._256_IndexedColors:
                    return ImageColorsTypes._256_IndexedColors;

                case (byte)ImageColorsTypes.True_Colors:
                    return ImageColorsTypes.True_Colors;

                case (byte)ImageColorsTypes.Alpha_Channel:
                    return ImageColorsTypes.Alpha_Channel;

                default:
                    return ImageColorsTypes.Unknown;
            }
        }

        //
        public static bool IsSupportedForIcons(this byte BitCount_)
        {
            switch (BitCount_)
            {
                case 0:
                    throw new System.Exception(BitCount_.ToString() + " Is invalid for BitCount(bpp)");
                //break;
                case 1:
                case 4:
                case 8:
                case 24:
                case 32:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsSupportedForIcons(this ImageColorsTypes ImageColorsTypes_)
        {
            return IsSupportedForIcons((byte)ImageColorsTypes_);
        }

        public static bool IsSupportedForIcons(this PixelFormat PixelFormat_1)
        {
            //return IsSupportedForIcons(BitCountFromPixelFormat(PixelFormat_1));
            switch (PixelFormat_1)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return true;

                default:
                    return false;
            }
        }

        //
        public static Bitmap ConvertImagePixelFormat(this Bitmap SourceImage, PixelFormat TargetPixelFormat, bool UseGrayColor_ = false, bool UsePreciseMethod = true)
        {
            return Drawing.ImagePixelEnumerator.SimpleHelper.ConvertBitmap(SourceImage, TargetPixelFormat, UseGrayColor_, UsePreciseMethod);
        }
    }
    
}