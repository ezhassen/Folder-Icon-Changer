using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using Ezz_Helper.Files;

namespace Ezz_Helper.Drawing.IconsManager
{
    /// <summary>
    /// Get icon resources (RT_GROUP_ICON and RT_ICON) from an executable module (either a .dll or an .exe file).
    /// </summary>
    public sealed class IconExtractor : IDisposable
    {

        #region Win32 interop.

        #region Consts.

        private const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        private const int RT_ICON = 3;
        private const int RT_GROUP_ICON = 14;

        private const int MAX_PATH = 260;

        private const int ERROR_SUCCESS = 0;
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_BAD_EXE_FORMAT = 193;
        private const int ERROR_RESOURCE_TYPE_NOT_FOUND = 1813;

        private const int sICONDIR = 6;            // sizeof(ICONDIR)
        private const int sICONDIRENTRY = 16;      // sizeof(ICONDIRENTRY)
        private const int sGRPICONDIRENTRY = 14;   // sizeof(GRPICONDIRENTRY)

        #endregion Consts.

        #endregion Win32 interop.

        #region Managed Types

        public class IconResInfo
        {
            public List<ResourceName> IconNames = new List<ResourceName>();
        }

        public class ResourceName
        {
            public IntPtr Id { get; private set; }
            public string Name { get; private set; }

            private IntPtr _bufPtr = IntPtr.Zero;

            public ResourceName(IntPtr lpName)
            {
                if (((uint)lpName >> 16) == 0) // #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0)
                {
                    this.Id = lpName;
                    this.Name = null;
                }
                else
                {
                    this.Id = IntPtr.Zero;
                    this.Name = Marshal.PtrToStringAuto(lpName);
                }
            }

            public IntPtr GetValue()
            {
                if (this.Name == null)
                {
                    return this.Id;
                }
                else
                {
                    this._bufPtr = Marshal.StringToHGlobalAuto(this.Name);
                    return this._bufPtr;
                }
            }

            public void Free()
            {
                if (this._bufPtr != IntPtr.Zero)
                {
                    try { Marshal.FreeHGlobal(this._bufPtr); }
                    catch { }

                    this._bufPtr = IntPtr.Zero;
                }
            }
        }

        #endregion Managed Types

        #region Private Fields

        private Icon[] _iconCache = null;

        #endregion Private Fields

        #region Public Properties

        private string _filename = null;

        // Full path
        public string Filename
        {
            get { return this._filename; }
        }

        public int IconCount
        {
            get { return this._resInfo.IconNames.Count; }
        }

        private IntPtr _hModule = IntPtr.Zero;
        public IntPtr hModule
        {
            get { return hModule; }
        }
        private IconResInfo _resInfo = null;
        public IconResInfo IconResInfo_1
        {
            get { return _resInfo; }
        }

        #endregion Public Properties

        #region Constructor/Destructor and relatives

        /// <summary>
        /// Load the specified executable file or DLL, and get ready to extract the icons.
        /// </summary>
        /// <param name="filename">The name of a file from which icons will be extracted.</param>
        public IconExtractor(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            this._hModule = Win32.LoadLibrary(filename);
            if (this._hModule == IntPtr.Zero)
            {
                this._hModule = Win32.LoadLibraryEx(filename, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
                if (this._hModule == IntPtr.Zero)
                {
                    switch (Marshal.GetLastWin32Error())
                    {
                        case ERROR_FILE_NOT_FOUND:
                            throw new FileNotFoundException("Specified file '" + filename + "' not found.");

                        case ERROR_BAD_EXE_FORMAT:
                            throw new ArgumentException("Specified file '" + filename + "' is not an executable file or DLL.");

                        default:
                            throw new Win32Exception();
                    }
                }
            }

            StringBuilder buf = new StringBuilder(MAX_PATH);
            int len = Win32.GetModuleFileName(this._hModule, buf, buf.Capacity + 1);
            if (len != 0)
            {
                this._filename = buf.ToString();
            }
            else
            {
                this._filename = filename;
                //switch (Marshal.GetLastWin32Error())
                //{
                //    case ERROR_SUCCESS:
                //        this._filename = filename;
                //        break;

                //    default:
                //        throw new Win32Exception();
                //}
            }

            this._resInfo = new IconResInfo();
            bool success = Win32.EnumResourceNames(this._hModule, RT_GROUP_ICON, EnumResNameCallBack, this._resInfo);
            //System.Windows.Forms.MessageBox.Show(success.ToString());
            if (!success)
            {
                throw new Win32Exception();
            }

            this._iconCache = new Icon[this.IconCount];
        }

        ~IconExtractor()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (this._hModule != IntPtr.Zero)
            {
                try { Win32.FreeLibrary(this._hModule); }
                catch { }

                this._hModule = IntPtr.Zero;
            }

            if (this._iconCache != null)
            {
                foreach (Icon i in this._iconCache)
                {
                    if (i != null)
                    {
                        try { i.Dispose(); }
                        catch { }
                    }
                }

                this._iconCache = null;
            }
        }

        #endregion Contructor/Destructor and relatives

        #region Public Methods

        /// <summary>
        /// Extract an icon from the loaded executable file or DLL.
        /// </summary>
        /// <param name="iconIndex">The zero-based index of the icon to be extracted.</param>
        /// <returns>A System.Drawing.Icon object which may consists of multiple icons.</returns>
        /// <remarks>Always returns new copy of the Icon. It should be disposed by the user.</remarks>
        public Icon GetIconAt(int iconIndex)
        {
            if (this._hModule == IntPtr.Zero)
            {
                throw new ObjectDisposedException("IconExtractor");
            }

            if (iconIndex < 0 || this.IconCount <= iconIndex)
            {
                throw new ArgumentException(
                    "iconIndex is out of range. It should be between 0 and " + (this.IconCount - 1).ToString() + ".");
            }

            if (this._iconCache[iconIndex] == null)
            {
                this._iconCache[iconIndex] = CreateIcon(iconIndex);
            }

            return (Icon)this._iconCache[iconIndex].Clone();
        }

        ///// <summary>
        ///// Split an Icon consists of multiple icons into an array of Icon each consist of single icons.
        ///// </summary>
        ///// <param name="icon">The System.Drawing.Icon to be split.</param>
        ///// <returns>An array of System.Drawing.Icon each consist of single icons.</returns>
        //public static Icon[] SplitIcon(Icon icon)
        //{
        //    if (icon == null)
        //    {
        //        throw new ArgumentNullException("icon");
        //    }

        //    // Get multiple .ico file image.
        //    byte[] srcBuf = null;
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        icon.Save(stream);
        //        srcBuf = stream.ToArray();
        //    }

        //    List<Icon> splitIcons = new List<Icon>();
        //    {
        //        int count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount

        //        for (int i = 0; i < count; i++)
        //        {
        //            using (MemoryStream destStream = new MemoryStream())
        //            using (BinaryWriter writer = new BinaryWriter(destStream))
        //            {
        //                // Copy ICONDIR and ICONDIRENTRY.
        //                writer.Write(srcBuf, 0, sICONDIR - 2);
        //                writer.Write((short)1);    // ICONDIR.idCount == 1;

        //                writer.Write(srcBuf, sICONDIR + sICONDIRENTRY * i, sICONDIRENTRY - 4);
        //                writer.Write(sICONDIR + sICONDIRENTRY);    // ICONDIRENTRY.dwImageOffset = sizeof(ICONDIR) + sizeof(ICONDIRENTRY)

        //                // Copy picture and mask data.
        //                int imgSize = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 8);       // ICONDIRENTRY.dwBytesInRes
        //                int imgOffset = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 12);    // ICONDIRENTRY.dwImageOffset
        //                writer.Write(srcBuf, imgOffset, imgSize);

        //                // Create new icon.
        //                destStream.Seek(0, SeekOrigin.Begin);
        //                splitIcons.Add(new Icon(destStream));
        //            }
        //        }
        //    }

        //    return splitIcons.ToArray();
        //}

        public override string ToString()
        {
            string text = String.Format("IconExtractor (Filename: '{0}', IconCount: {1})", this.Filename, this.IconCount);
            return text;
        }

        #endregion Public Methods

        #region Private Methods

        private bool EnumResNameCallBack(IntPtr hModule, int lpszType, IntPtr lpszName, IconResInfo lParam)
        {
            // Callback function for EnumResourceNames().

            if (lpszType == RT_GROUP_ICON)
            {
                lParam.IconNames.Add(new ResourceName(lpszName));
            }

            return true;
        }

        private Icon CreateIcon(int iconIndex)
        {
            // Get group icon resource.
            byte[] srcBuf = GetResourceData(this._hModule, this._resInfo.IconNames[iconIndex], RT_GROUP_ICON);

            // Convert the resouce into an .ico file image.
            using (MemoryStream destStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(destStream))
            {
                int count = BitConverter.ToUInt16(srcBuf, 4); // ICONDIR.idCount
                int imgOffset = sICONDIR + sICONDIRENTRY * count;

                // Copy ICONDIR.
                writer.Write(srcBuf, 0, sICONDIR);

                for (int i = 0; i < count; i++)
                {
                    // Copy GRPICONDIRENTRY converting into ICONDIRENTRY.
                    writer.BaseStream.Seek(sICONDIR + sICONDIRENTRY * i, SeekOrigin.Begin);
                    writer.Write(srcBuf, sICONDIR + sGRPICONDIRENTRY * i, sICONDIRENTRY - 4);   // Common fields of structures
                    writer.Write(imgOffset);                                                    // ICONDIRENTRY.dwImageOffset

                    // Get picture and mask data, then copy them.
                    IntPtr nID = (IntPtr)BitConverter.ToUInt16(srcBuf, sICONDIR + sGRPICONDIRENTRY * i + 12); // GRPICONDIRENTRY.nID
                    byte[] imgBuf = GetResourceData(this._hModule, nID, RT_ICON);

                    writer.BaseStream.Seek(imgOffset, SeekOrigin.Begin);
                    writer.Write(imgBuf, 0, imgBuf.Length);

                    imgOffset += imgBuf.Length;
                }

                destStream.Seek(0, SeekOrigin.Begin);
                return new Icon(destStream);
            }
        }

        private byte[] GetResourceData(IntPtr hModule, IntPtr lpName, int lpType)
        {
            // Get binary image of the specified resource.

            IntPtr hResInfo = Win32.FindResource(hModule, lpName, lpType);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            IntPtr hResData = Win32.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            IntPtr hGlobal = Win32.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            int resSize = Win32.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }

            byte[] buf = new byte[resSize];
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        private byte[] GetResourceData(IntPtr hModule, ResourceName name, int lpType)
        {
            try
            {
                IntPtr lpName = name.GetValue();
                return GetResourceData(hModule, lpName, lpType);
            }
            finally
            {
                name.Free();
            }
        }

        #endregion Private Methods

        //
        #region Static Methods

        /// <summary>
        /// Returns IconsManager.IconInfo object that holds the information about the icon.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to get the information about.</param>
        /// <returns>IconsManager.IconInfo object that holds the information about the icon.</returns>
        public static IconInfo GetIconInfo(Icon icon)
        {
            return new IconInfo(icon);
        }
        /// <summary>
        /// Returns IconsManager.IconInfo object that holds the information about the icon.
        /// </summary>
        /// <param name="fileName">The icon file path.</param>
        /// <returns>IconsManager.IconInfo object that holds the information about the icon.</returns>
        public static IconInfo GetIconInfo(string fileName)
        {
            return new IconInfo(fileName);
        }

        /// <summary>
        /// if the index is negatif, is not a index but is a ID NAME  (see msdn.microsoft.com/en-us/library/ms682212(VS.85).aspx)
        ///  example in registry DefaultIcon of "txtfile"  is "%SystemRoot%\system32\imageres.dll,-102" = the icon with index=97 in imageres.dll
        /// </summary>
        /// <param name="iconIndex"></param>
        /// <returns></returns>
        public int GetIconIndex(int iconIndex)
        {
            if (iconIndex >= 0) return iconIndex;
            //iconIndex = -iconIndex;
            //var IntPI = new IntPtr(Math.Abs(iconIndex));
            int Sourceindex = Math.Abs(iconIndex);
            int index = 0;
            foreach (ResourceName resourceName in this.IconResInfo_1.IconNames)
            {
                if (resourceName.Id != null)
                {
                    if (resourceName.Id.ToInt32() == Sourceindex)//(resourceName.Id == IntPI)
                    {
                        return index;
                        //break;
                    }
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Extracts an icon from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <returns>A System.Drawing.Icon extracted from the file at the specified index in case of an executable module.</returns>
        public static Icon ExtractIcon(string fileName, int iconIndex)
        {
            Icon icon_ = null;
            //Try to load the file as icon file.
            try
            {
                icon_ = new Icon(Environment.ExpandEnvironmentVariables(fileName));
            }
            catch { }

            //if The file was an icon file, return the icon.
            if ((icon_ != null)) return icon_;

            try
            {
                //Load the file as an executable module.
                using (IconExtractor extractor = new IconExtractor(fileName))
                {
                    iconIndex = extractor.GetIconIndex(iconIndex);
                    icon_ = extractor.GetIconAt(iconIndex);
                }
                return icon_;
            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }
        public static IconInfo ExtractIcon_AsIconInfo(string fileName, int iconIndex)
        {
            Icon icon_ = ExtractIcon(fileName, iconIndex);
            if (icon_ != null)
            {
                IconInfo newII = new IconInfo(icon_);
                newII.Index = iconIndex;
                return newII;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="iconIndex"></param>
        /// <param name="DestPath"></param>
        /// <param name="SameFileNameDecisions_1"></param>
        /// <returns>new file name</returns>
        public static SaveToFileInfo ExtractIconFromFileToFile(string fileName, int iconIndex, string DestPath, SameFileNameDecisions SameFileNameDecisions_1 = SameFileNameDecisions.Rename)
        {
            Icon ExtrIcon = ExtractIcon(fileName, iconIndex);
            if (ExtrIcon == null) return null;
            SaveToFileInfo SaveInfo = new SaveToFileInfo(DestPath, SameFileNameDecisions_1);
            //try
            //{
            if (SaveInfo.GenNewFilePath_ReturnCanWrite())
            {
                using (FileStream newFI = new FileStream(SaveInfo.NewFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExtrIcon.Save(newFI);
                }
            }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return SaveInfo;
        }


        /// <summary>
        /// Extracts all the icons from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <returns>
        /// A list of System.Drawing.Icon found in the file.
        /// If the file was an icon file, it will return a list containing a single icon.
        /// </returns>
        public static List<Icon> ExtractAllIcons(string fileName)
        {
            Icon icon = null;
            List<Icon> list = new List<Icon>();
            //Try to load the file as icon file.
            try
            {
                icon = new Icon(Environment.ExpandEnvironmentVariables(fileName));
            }
            catch
            {
            }

            if (!(icon == null))
            {
                //The file was an icon file.
                list.Add(icon);
                return list;
            }

            //Load the file as an executable module.
            using (IconExtractor extractor = new IconExtractor(fileName))
            {
                int i = 0;
                while (i < extractor.IconCount)
                {
                    list.Add(extractor.GetIconAt(i));
                    //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1);
                    i++;
                }
            }
            return list;
        }
        /// <summary>
        /// Extracts all the icons from a givin icon file or an executable module (.dll or an .exe file).
        ///  as <see cref="IconNImage.GIconsInfo"/>
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Drawing.IconH.GIconsInfo ExtractAllIcons_AsGIconsInfo(string fileName)
        {
            var NewGsII = Drawing.IconH.GIconsInfo.InitializeNew(fileName, Drawing.IconH.GetIconMathods.IconsManagerClass);
            //Try to load the file as icon file.
            Icon icon_ = null;
            try
            {
                icon_ = new Icon(Environment.ExpandEnvironmentVariables(fileName));
            }
            catch
            {
            }
            if (icon_ != null)
            {
                NewGsII.iconsList.Add(Drawing.IconH.GIconInfo.InitializeNew(icon_, 0, fileName));
                NewGsII.iCount = 1;
                NewGsII.OK = false;
                return NewGsII;
            }
            try
            {
                //Load the file as an executable module.
                using (IconExtractor extractor = new IconExtractor(fileName))
                {
                    NewGsII.iCount = extractor.IconCount;
                    int i = 0;
                    while (i < NewGsII.iCount)
                    {
                        //List.Add(extractor.GetIconAt(i))
                        NewGsII.iconsList.Add(Drawing.IconH.GIconInfo.InitializeNew(extractor.GetIconAt(i), i, fileName));
                        //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1);
                        i++;
                    }
                    NewGsII.OK = true;
                }
            }
            catch (Exception)
            {
            }

            return NewGsII;
        }
        public static List<IconInfo> ExtractAllIcons_AsIconInfo(string fileName, Action<IconInfo> TodoEveryGet = null, bool BuildIcons = false)
        {
            List<IconInfo> list = new List<IconInfo>();
            //Try to load the file as icon file.
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            Icon icon_1 = null;
            try
            {
                icon_1 = new Icon(fileName);
            }
            catch { }

            if (icon_1 != null)
            {
                //The file was an icon file.
                IconInfo IF_1 = new IconInfo(icon_1, 0, BuildIcons);
                list.Add(IF_1);
                if (TodoEveryGet != null) { TodoEveryGet(IF_1); }
            }
            else
            {
                using (IconExtractor extractor = new IconExtractor(fileName))
                {
                    int i = 0;
                    while (i < extractor.IconCount)
                    {
                        IconInfo IF_1 = new IconInfo(extractor.GetIconAt(i), i, BuildIcons);
                        list.Add(IF_1);
                        if (TodoEveryGet != null) { TodoEveryGet(IF_1); }
                        i++;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Splits the group icon into a list of icons (the single icon file can contain a set of icons).
        /// </summary>
        /// <param name="icon">The System.Drawing.Icon need to be splitted.</param>
        /// <returns>List of System.Drawing.Icon.</returns>
        public static List<IconInfo.IconImageInfo> SplitGroupIcon(Icon icon)
        {
            IconInfo info = new IconInfo(icon);
            //return info.Images;
            return info.ImageList.ToList();
        }

        /// <summary>
        /// Gets count of icons in DLL/EXE Files.
        /// </summary>
        /// <param name="FilePath">DLL/EXE File path.</param>
        /// <returns>Icons Count</returns>
        public static int GetIconsCount(string FilePath)
        {
            int lCount = 0;
            IntPtr[] phiconSmall = null, phiconLarge = null; // Pointers to arrays of 
            try
            {
                lCount = NativeMethods.ExtractIconEx(FilePath, -1, phiconSmall, phiconLarge, 0);   // Get count of icons 
            }
            catch (Exception)
            {
            }
            return lCount;
        }

        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static IconInfo.IconImageInfo GetBestFitIcon(Icon icon)
        {
            IconInfo info = new IconInfo(icon);
            IconInfo.IconImageInfo REsi = info.GetBestFitIcon().Clone();
            info.Dispose();
            return REsi;
        }
        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static IconInfo.IconImageInfo GetBestFitIcon(Icon icon, Size desiredSize)
        {
            IconInfo info = new IconInfo(icon);
            IconInfo.IconImageInfo REsi = info.GetBestFitIcon(desiredSize).Clone();
            info.Dispose();
            return REsi;
        }
        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static IconInfo.IconImageInfo GetBestFitIcon(Icon icon, Size desiredSize, bool isMonochrome)
        {
            IconInfo info = new IconInfo(icon);
            IconInfo.IconImageInfo REsi = info.GetBestFitIcon(desiredSize, isMonochrome).Clone();
            info.Dispose();
            return REsi;
        }

        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static IconInfo.IconImageInfo ExtractBestFitIcon(string fileName, int iconIndex)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon);
        }
        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static IconInfo.IconImageInfo ExtractBestFitIcon(string fileName, int iconIndex, Size desiredSize)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon, desiredSize);
        }
        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static IconInfo.IconImageInfo ExtractBestFitIcon(string fileName, int iconIndex, Size desiredSize, bool isMonochrome)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon, desiredSize, isMonochrome);
        }

        //
        /// <summary>
        /// Gets icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <param name="flags">Specifies which icon to be retrieved (Larg, Small, Selected, Link Overlay and Shell Size).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedIcon(string fileName, IconFlags flags)
        {
            flags = flags | IconFlags.Icon;
            SHFILEINFO fileInfo = new SHFILEINFO();
            IntPtr result = Win32.SHGetFileInfo(fileName, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), (SHGetFileInfoFlags)flags);

            if (fileInfo.hIcon == IntPtr.Zero) return null;
            Icon RIcon = (Icon)Icon.FromHandle(fileInfo.hIcon).Clone();
            Win32.DestroyIcon(fileInfo.hIcon);

            return RIcon;
        }
        /// <summary>
        /// Gets large icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedLargeIcon(string fileName)
        {
            return GetAssociatedIcon(fileName, IconFlags.LargeIcon);
        }
        /// <summary>
        /// Gets small icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedSmallIcon(string fileName)
        {
            return GetAssociatedIcon(fileName, IconFlags.SmallIcon);
        }

        /// <summary>
        /// Merges a list of icons into one single icon.
        /// </summary>
        /// <param name="icons">The icons to be merged.</param>
        /// <returns>System.Drawing.Icon that contains all the images of the givin icons.</returns>
        //public static Icon Merge(params Icon[] icons)
        //{
        //    List<IconInfo> list = new List<IconInfo>(icons.Length);
        //    int numImages = 0;
        //    foreach (Icon icon in icons)
        //    {
        //        if (icon != null)
        //        {
        //            IconInfo info = new IconInfo(icon);
        //            list.Add(info);
        //            numImages += info.IconDir.Count;//info.Images.Count;
        //        }
        //    }
        //    if (list.Count == 0) { throw new ArgumentNullException("icons", "The icons list should contain at least one icon."); }

        //    Icon resultIcon = null;
        //    //Write the icon to a stream.
        //    using (MemoryStream outputStream = new MemoryStream())
        //    {
        //        int imageIndex = 0;
        //        int imageOffset = IconInfo.SizeOfIconDir + numImages * IconInfo.SizeOfIconDirEntry;
        //        int i = 0;
        //        while (i < list.Count)
        //        {
        //            IconInfo iconInfo__1 = list[i];
        //            //The firs image, we should write the icon header.
        //            if (i == 0)
        //            {
        //                //Get the IconDir and update image count with the new count.
        //                IconDir dir = iconInfo__1.IconDir;
        //                dir.Count = (short)numImages;

        //                //Write the IconDir header.
        //                outputStream.Seek(0, SeekOrigin.Begin);
        //                Tools.WriteStructure<IconDir>(outputStream, dir);
        //            }
        //            //For each image in the current icon, we should write the IconDirEntry and the image raw data.
        //            int j = 0;
        //            while (j < iconInfo__1.Images.Count)
        //            {
        //                //Get the IconDirEntry and update the ImageOffset to the new offset.
        //                IconDirEntry entry = iconInfo__1.IconDirEntries[j];
        //                entry.ImageOffset = imageOffset;

        //                //Write the IconDirEntry to the stream.
        //                outputStream.Seek(IconInfo.SizeOfIconDir + imageIndex * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
        //                Tools.WriteStructure<IconDirEntry>(outputStream, entry);

        //                //Write the image raw data.
        //                outputStream.Seek(imageOffset, SeekOrigin.Begin);
        //                outputStream.Write(iconInfo__1.RawData[j], 0, entry.BytesInRes);

        //                //Update the imageIndex and the imageOffset
        //                //System.Math.Max(System.Threading.Interlocked.Increment(imageIndex), imageIndex - 1);
        //                imageIndex++;
        //                imageOffset += entry.BytesInRes;
        //                //System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1);
        //                j++;
        //            }
        //            //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1);
        //            i++;
        //        }

        //        //Create the icon from the stream.
        //        outputStream.Seek(0, SeekOrigin.Begin);
        //        resultIcon = new Icon(outputStream);
        //    }// outputStream.Close();

        //    //'Free memory
        //    foreach (IconInfo iconinf in list)
        //    {
        //        //iconinf.SourceIcon.Dispose
        //        iconinf.Dispose();
        //    }

        //    return resultIcon;
        //}

        #endregion Static Methods
    }

}
