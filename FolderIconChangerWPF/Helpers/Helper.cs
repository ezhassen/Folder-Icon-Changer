using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;

namespace FolderIconChangerWPF
{
    public static class Helper
    {


        /// <summary>
        /// Unsecures a <see cref="SecureString"/> to plain text
        /// </summary>
        /// <param name="secureString">The secure string</param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            // Make sure we have a secure string
            if (secureString == null)
                return string.Empty;

            // Get a pointer for an unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                // Unsecures the password
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Clean up any memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static IntPtr GetCriticalHandle(this Window window) => (IntPtr)typeof(Window).GetProperty("CriticalHandle", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window, new object[0]);
        public static void MoveWindowOnMouseDown(this Window window)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(window.GetCriticalHandle(), NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
        }

    }


    public sealed class NativeMethods
    {

        #region Const

        // Get Icon.
        public const uint SHGFI_ICON = 0x100;
        //Get link overlay icon.
        public const uint SHGFI_LINKOVERLAY = 0x8000;
        //Get icon in selected state.
        public const uint SHGFI_SELECTED = 0x10000;
        //Get icon with addributes specified.
        public const uint SHGFI_ATTR_SPECIFIED = 0x20000;
        //Get large icon.
        public const uint SHGFI_LARGEICON = 0x0;
        //Get small icon.
        public const uint SHGFI_SMALLICON = 0x1;
        //Get icon in open state.
        public const uint SHGFI_OPENICON = 0x2;
        //Get icon size.
        public const uint SHGFI_SHELLICONSIZE = 0x4;
        //Get icon location.
        public const uint SHGFI_ICONLOCATION = 0x1000;
        //Get sytem image list index.
        public const uint SHGFI_SYSICONINDEX = 0x4000;
        //Add overlays to icon (e.g. link overlay)
        public const uint SHGFI_ADDOVERLAYS = 0x20;
        //Get overlay icon index
        public const uint SHGFI_OVERLAYINDEX = 0x40;

        //Get display name.
        public const uint SHGFI_DISPLAYNAME = 0x200;
        //Get type name.
        public const uint SHGFI_TYPENAME = 0x400;

        //Get attributes
        public const uint SHGFI_ATTRIBUTES = 0x800;
        //Get executable type.
        public const uint SHGFI_EXETYPE = 0x2000;

        //Use PIDLs instead of strings.
        public const uint SHGFI_PIDL = 0x8;
        //Use file attributes
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;


        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;

        //

        public const int SHCNE_ASSOCCHANGED = 0x8000000;
        public const int SHCNF_IDLIST = 0;


        //
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        #endregion

        #region struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public IntPtr dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        #endregion

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern int LockWindowUpdate(IntPtr hWnd);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
                                                      ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);



        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, uint nIconIndex);

        [DllImport("Shell32.DLL", CharSet = CharSet.Unicode)]
        public static extern int ExtractIconEx([In] string lpszFile, [In] int
    nIconIndex, [In, Out] IntPtr[] phiconLarge, [In, Out] IntPtr[] phiconSmall,
    [In] int nIcons);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);


        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int PickIconDlg(IntPtr hwndOwner, System.Text.StringBuilder lpstrFile,
                                                uint nMaxFile, ref int lpdwIconIndex);



        //
        // notify Windows that file associations have changed
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, uint dwItem1, uint dwItem2);
        public static void SHChangeNotify_FilesAssociations() { SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, 0, 0); }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszShortPath, uint cchBuffer);
        // Return short path format of a file name
        public static string GetShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = Convert.ToUInt32(s.Capacity);
            uint iRet = NativeMethods.GetShortPathName(longName, s, iSize);
            return s.ToString();
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern int GetLongPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszShortPath, [MarshalAs(UnmanagedType.LPWStr)]  StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] int cchBuffer);
        /// <summary>
        /// Converts a short path to a long path.
        /// </summary>
        /// <param name="shortPath">A path that may contain short path elements (~1).</param>
        /// <returns>The long path.</returns>
        public static string GetLongPathName(string shortPath)
        {
            if (String.IsNullOrEmpty(shortPath))
            {
                return shortPath;
            }

            StringBuilder builder = new StringBuilder(255);
            int result = NativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
            if (result > 0 && result < builder.Capacity)
            {
                return builder.ToString(0, result);
            }
            else
            {
                if (result > 0)
                {
                    builder = new StringBuilder(result);
                    result = NativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
                    return builder.ToString(0, result);
                }
                else
                {
                    //Throw New FileNotFoundException(String.Format(Globalization.CultureInfo.CurrentCulture,  "", shortPath), shortPath)
                    return shortPath;
                }
            }
        }


    }

    public struct ControlChars
    {
        public const char Back = '\b';
        public const char Cr = '\r';
        public const string CrLf = "\r\n";
        public const char FormFeed = '\f';
        public const char Lf = '\n';
        public const string NewLine = "\r\n";
        public const char NullChar = '\0';
        public const char Quote = '"';
        public const char Tab = '\t';
        public const char VerticalTab = '\v';
    }


}
