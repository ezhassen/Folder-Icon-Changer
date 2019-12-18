using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using Ezz_Helper.Files.GetInfo;
using Microsoft.Win32;
using FolderIconChangerWPF;

namespace Ezz_Helper
{
    public static class OtherH
    {

        public static void AbortAThread(this Thread _Thread__)
        {
            if (_Thread__ != null)
            {
                if (_Thread__.IsAlive)
                {
                    _Thread__.Abort();
                }
            }
        }
        public static void AbortAThread(this List<Thread> _Threads__)
        {
            foreach (Thread Th in _Threads__)
            {
                AbortAThread(Th);
            }
        }


        public static bool ObjectsContainsNull(params object[] objs)
        {
            foreach (object obj in objs)
            {
                //try {
                if (obj == null)
                    return true;
                //} catch (Exception) {
                //    //throw;
                //}
            }
            return false;
        }
        public static void DisposeObject(params object[] objs)
        {
            foreach (dynamic obj in objs)
            {
                try
                {
                    //unchecked {
                    if (obj != null)
                    {
                        obj.Dispose();
                    }
                    //}
                }
                catch { }
            }
        }

        public static Size TextSize(this string Text_, Font Font_)
        {
            Bitmap newB = new Bitmap(48, 48);
            dynamic newGr = Graphics.FromImage(newB);
            dynamic TSize = newGr.MeasureString(Text_, Font_).ToSize;
            newGr.Dispose();
            newB.Dispose();
            return TSize;
        }
        public static Point TextNewPoint(this Size OwnerSize_, string Text_, Font Font_, ContentAlignment TextAlignment)
        {
            Bitmap newBM = new Bitmap(OwnerSize_.Width, OwnerSize_.Height);
            dynamic newGr = Graphics.FromImage(newBM);
            Font TFont = Font_;
            Size TSize = newGr.MeasureString(Text_, TFont).ToSize;
            Point Res = GetNewPoint(OwnerSize_, TSize, TextAlignment);
            //float TX = 0;
            //float TY = 0;
            //
            //switch (TextAlignment)
            //{
            //    case ContentAlignment.TopLeft:
            //        TX = 0;
            //        TY = 0;
            //        break;
            //    case ContentAlignment.TopCenter:
            //        TX = (float)(newBM.Width / 2) - (TSize.Width / 2);
            //        TY = 0;
            //        break;
            //    case ContentAlignment.TopRight:
            //        TX = (float)newBM.Width - TSize.Width;
            //        TY = 0;
            //        break;
            //    //
            //    case ContentAlignment.MiddleLeft:
            //        TX = 0;
            //        TY = (float)(newBM.Height / 2) - (TSize.Height / 2);
            //        break;
            //    case ContentAlignment.MiddleCenter:
            //        TX = (float)(newBM.Width / 2) - (TSize.Width / 2);
            //        TY = (float)(newBM.Height / 2) - (TSize.Height / 2);
            //        break;
            //    case ContentAlignment.MiddleRight:
            //        TX = (float)newBM.Width - TSize.Width;
            //        TY = (float)(newBM.Height / 2) - (TSize.Height / 2);
            //    //
            //        break;
            //    case ContentAlignment.BottomLeft:
            //        TX = 0;
            //        TY = (float)newBM.Height - TSize.Height;
            //        break;
            //    case ContentAlignment.BottomCenter:
            //        TX = (float)(newBM.Width / 2) - (TSize.Width / 2);
            //        TY = (float)newBM.Height - TSize.Height;
            //        break;
            //    case ContentAlignment.BottomRight:
            //        TX = (float)newBM.Width - TSize.Width;
            //        TY = (float)newBM.Height - TSize.Height;
            //        break;
            //}
            newGr.Dispose();
            newBM.Dispose();
            //return new Point((int)TX, (int)TY);
            return Res;
        }
        public static Point GetNewPoint(this Size OwnerSize_, Size ObjSize, ContentAlignment CAlignment)
        {
            float TX = 0;
            float TY = 0;

            switch (CAlignment)
            {
                case ContentAlignment.TopLeft:
                    TX = 0;
                    TY = 0;
                    break;

                case ContentAlignment.TopCenter:
                    TX = (float)(OwnerSize_.Width / 2) - (ObjSize.Width / 2);
                    TY = 0;
                    break;

                case ContentAlignment.TopRight:
                    TX = (float)OwnerSize_.Width - ObjSize.Width;
                    TY = 0;
                    break;
                //
                case ContentAlignment.MiddleLeft:
                    TX = 0;
                    TY = (float)(OwnerSize_.Height / 2) - (ObjSize.Height / 2);
                    break;

                case ContentAlignment.MiddleCenter:
                    TX = (float)(OwnerSize_.Width / 2) - (ObjSize.Width / 2);
                    TY = (float)(OwnerSize_.Height / 2) - (ObjSize.Height / 2);
                    break;

                case ContentAlignment.MiddleRight:
                    TX = (float)OwnerSize_.Width - ObjSize.Width;
                    TY = (float)(OwnerSize_.Height / 2) - (ObjSize.Height / 2);
                    //
                    break;

                case ContentAlignment.BottomLeft:
                    TX = 0;
                    TY = (float)OwnerSize_.Height - ObjSize.Height;
                    break;

                case ContentAlignment.BottomCenter:
                    TX = (float)(OwnerSize_.Width / 2) - (ObjSize.Width / 2);
                    TY = (float)OwnerSize_.Height - ObjSize.Height;
                    break;

                case ContentAlignment.BottomRight:
                    TX = (float)OwnerSize_.Width - ObjSize.Width;
                    TY = (float)OwnerSize_.Height - ObjSize.Height;
                    break;
            }
            return new Point((int)TX, (int)TY);
        }

        public static System.Text.Encoding DetectFileEncoding(string filename, System.Text.Encoding Default_1 = null)
        {
            try
            {
                System.Text.Encoding ResEnc = null;
                if (System.IO.File.Exists(filename))
                {
                    using (System.IO.FileStream filein = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if ((filein.CanSeek))
                        {
                            byte[] bom = new byte[5];
                            filein.Read(bom, 0, 5);
                            //UTF-8: EF BB BF
                            //UTF-16 big endian byte order: FE FF
                            //UTF-16 little endian byte order: FF FE
                            //UTF-32 big endian byte order: 00 00 FE FF
                            //UTF-32 little endian byte order: FF FE 00 00
                            //UTF-7 byte order : 2B 2F 76 38 2D

                            if ((((bom[0] == 0xef) & (bom[1] == 0xbb) & (bom[2] == 0xbf))))//UTF-8: EF BB BF
                            {
                                ResEnc = System.Text.Encoding.UTF8;
                            }
                            else if (((bom[0] == 0xff) & (bom[1] == 0xfe))) //UTF-16 little endian byte order: FF FE
                            {
                                ResEnc = System.Text.Encoding.Unicode;
                            }
                            else if (((bom[0] == 0xfe) & (bom[1] == 0xff))) //UTF-16 big endian byte order: FE FF
                            {
                                ResEnc = System.Text.Encoding.BigEndianUnicode;
                            }
                            else if ((bom[0] == 0xff) & (bom[1] == 0xfe) & (bom[2] == 0x0) & (bom[3] == 0x0)) //UTF-32 little endian byte order: FF FE 00 00
                            {
                                ResEnc = System.Text.Encoding.UTF32;
                            }
                            else if ((bom[0] == 0x2b) & (bom[1] == 0x2f) & (bom[2] == 0x76) & (bom[3] == 0x38) & (bom[4] == 0x2d))  //UTF-7 byte order : 2B 2F 76 38 2D
                            {
                                ResEnc = System.Text.Encoding.UTF7;
                                //}else if ((bom[0] == 0x0) & (bom[1] == 0x0) & (bom[2] ==0xfe) & (bom[3] == 0xff))  //UTF-32 big endian byte order: 00 00 FE FF (ucs-4)
                                //{
                                //return System.Text.Encoding.??
                            }
                            else //Default
                            {
                                ResEnc = Default_1 == null ? System.Text.Encoding.Default : Default_1;
                            }
                        }
                    }//filein.Close()
                }
                return ResEnc;
            }
            catch { return null; }
        }
        //
        //
        public static void BuildFilter(this FileDialog FileDialog_1, string[] Names, bool SetTypeByExtension_1 = true, bool AddAllSupportedFormats_1 = true, string AllFormatsString_1 = FileDialogFilterBuilder.DefaultAllFormatsString)
        {
            FileDialogFilterBuilder newFB = new FileDialogFilterBuilder(Names, SetTypeByExtension_1, AddAllSupportedFormats_1, AllFormatsString_1);
            FileDialog_1.Filter = newFB.Filter;
        }
        public static void BuildFilter(this FileDialog FileDialog_1, List<FileDialogFilterBuilder.FileFormat> FileFormats_1, bool AddAllSupportedFormats_1 = true, string AllFormatsString_1 = FileDialogFilterBuilder.DefaultAllFormatsString)
        {
            FileDialogFilterBuilder newFB = new FileDialogFilterBuilder(FileFormats_1, AddAllSupportedFormats_1, AllFormatsString_1);
            FileDialog_1.Filter = newFB.Filter;
        }

        //
        public static string DateToBinaryString(this DateTime? Date_1)
        {
            if (!Date_1.HasValue) return "";
            return Date_1.Value.ToBinary().ToString();
        }

        public static void DateFromBinaryString(ref DateTime? Date_1, string BinaryString)
        {
            Date_1 = DateFromBinaryString(BinaryString);
            //return Date_1;
        }
        public static DateTime? DateFromBinaryString(this string BinaryString)
        {
            if (BinaryString.IsNullOrEmpty()) return null;
            try
            {
                return DateTime.FromBinary(BinaryString.ValLong());
            }
            catch
            {
                return null;
            }
        }

        //
        public static string AttributeValue(this XElement XE, string AttXName, string DefaultVal = "")
        {
            XAttribute XA = XE.Attribute(AttXName);
            if (XA == null) return DefaultVal;
            return XA.Value;
        }


        //
        public static void RefreshWindowsExplorers()
        {
            // based on http://stackoverflow.com/questions/2488727/refresh-windows-explorer-in-win7
            Guid CLSID_ShellApplication = new Guid("13709620-C279-11CE-A49E-444553540000");
            Type shellApplicationType = Type.GetTypeFromCLSID(CLSID_ShellApplication, true);

            object shellApplication = Activator.CreateInstance(shellApplicationType);
            object windows = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, null, shellApplication, new object[] { });

            Type windowsType = windows.GetType();
            object count = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, windows, null);
            for (int i = 0; i < (int)count; i++)
            {
                object item = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, null, windows, new object[] { i });
                Type itemType = item.GetType();

                // only refresh windows explorers
                string itemName = (string)itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, item, null);
                if (itemName == "Windows Explorer")
                {
                    itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
                }
            }
        }


        //

        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999);
        }
        //
    }
    
    public class FileDialogFilterBuilder
    {
        public const char FSeperator = '|';
        public const char NameSeperator = ';';
        public const string DefaultAllFormatsString = "(All supported formats)";

        public FileDialogFilterBuilder(string[] Names, bool SetTypeByExtension_1 = true, bool AddAllSupportedFormats_1 = true, string AllFormatsString_1 = DefaultAllFormatsString)
        {
            AllFormatsString = AllFormatsString_1;
            AddAllSupportedFormats = AddAllSupportedFormats_1;
            _FileFormats = new List<FileFormat>();
            //
            foreach (string name in Names)
            {
                FileFormats.Add(new FileFormat(name, SetTypeByExtension_1));
            }
        }
        public FileDialogFilterBuilder(List<FileFormat> FileFormats_1, bool AddAllSupportedFormats_1 = true, string AllFormatsString_1 = DefaultAllFormatsString)
        {
            AllFormatsString = AllFormatsString_1;
            AddAllSupportedFormats = AddAllSupportedFormats_1;
            _FileFormats = FileFormats_1;
        }

        private List<FileFormat> _FileFormats;
        public List<FileFormat> FileFormats { get { return _FileFormats; } }

        public bool AddAllSupportedFormats { get; set; }
        public string AllFormatsString { get; set; }
        public string Filter
        {
            get
            {
                string res = "";

                if (AddAllSupportedFormats) res = GenerateAllFormats(AllFormatsString).Filter;

                foreach (FileFormat FFormat in FileFormats)
                {
                    string FFilter = FFormat.Filter;
                    if (!FFilter.IsNullOrEmpty())
                    {
                        if (!res.IsNullOrEmpty()) res += '|'; //add FSeperator
                        res += FFilter; //add filter
                    }
                }
                return res;
            }
        }

        public List<string> GetAllNames()
        {
            List<string> newl = new List<string>();
            foreach (FileFormat item in FileFormats)
            {
                foreach (string name in item.Names)
                {
                    if (!newl.Contains(name, new StringIEqualityComparer())) newl.Add(name);
                }
            }
            return newl;
        }
        public FileFormat GenerateAllFormats(string SupFormats = DefaultAllFormatsString) => new FileFormat(GetAllNames(), SupFormats);

        //
        public sealed class FileFormat
        {

            public FileFormat(List<string> Names_1, bool SetTypeByExtension_1 = true, Func<string, string, string> TypeStr = null)
            {
                _Names = Names_1;
                if (SetTypeByExtension_1) SetTypeByExtension(TypeStr);
            }
            public FileFormat(string Name_1, bool SetTypeByExtension_1 = true, Func<string, string, string> TypeStr = null)
            {
                _Names = new List<string>();
                _Names.Add(Name_1);
                if (SetTypeByExtension_1) SetTypeByExtension(TypeStr);
            }
            public FileFormat(List<string> Names_1, string Type_1)
            {
                _Names = Names_1;
                Type = Type_1;
            }
            public FileFormat(string Name_1, string Type_1)
            {
                _Names = new List<string>();
                _Names.Add(Name_1);
                Type = Type_1;
            }

            public string Type { get; set; }
            public string FristName
            {
                get
                {
                    if (_Names.Count == 0) return "";
                    return _Names[0];
                }
            }
            private List<string> _Names;// = new List<string>(); 
            public List<string> Names
            {
                get
                {
                    return _Names;
                }
            }

            public string Filter
            {
                get
                {
                    string Formats = "";
                    int NCount = _Names.Count;
                    if (NCount == 0)
                    {
                        return "";
                    }
                    else if (NCount == 1)
                    {
                        Formats = _Names[0]; //one format|*.ico
                    }
                    else
                    {
                        foreach (string name in _Names)  //multi formats|*.ico;*.dll;*.exe;*.jpg;*.png;*.bmp;
                        {
                            Formats += string.Format("{0};", name);
                        }
                    }
                    return string.Format("{0}|{1}", Type, Formats);
                }
            }
            public bool IsMultiFormats { get { return (_Names.Count > 0); } }

            public void SetTypeByExtension(Func<string, string, string> TypeStr = null)
            {
                try
                {
                    string FName = FristName;
                    if (!FName.IsNullOrEmpty() && !FName.EndsWith("*"))
                    {
                        string Ext = Path.GetExtension(FName);
                        string ExtT = GetExInfo.GetFileTypeByExtension(Ext);

                        Type = TypeStr == null ? ExtT : TypeStr(ExtT, Ext);//string.Format("{0}",TypeStr == null ? "" : TypeStr(Ext), GetExInfo.GetFileTypeByExtension(Ext), Ext); 
                    }
                }
                catch { }
            }

            public override string ToString()
            {
                if (this.Type.IsNullOrEmpty()) return base.ToString();
                return this.Type;
            }
        }


    }

}