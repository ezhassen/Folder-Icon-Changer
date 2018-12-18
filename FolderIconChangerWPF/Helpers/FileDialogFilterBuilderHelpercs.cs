using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ezz_Helper.Custom_Comparers;
using Ezz_Helper.Files.GetInfo;
using Microsoft.Win32;

namespace FolderIconChangerWPF.Helpers
{
    public static class FileDialogFilterBuilderHelper
    {
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
        public string Filter {
            get {
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
                    if (!newl.Contains(name, new stringIEqualityComparer())) newl.Add(name);
                }
            }
            return newl;
        }
        public FileFormat GenerateAllFormats(string SupFormats = DefaultAllFormatsString)
        {
            return new FileFormat(GetAllNames(), SupFormats);
        }



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
            public string FristName {
                get {
                    if (_Names.Count == 0) return "";
                    return _Names[0];
                }
            }
            private List<string> _Names;// = new List<string>(); 
            public List<string> Names {
                get {
                    return _Names;
                }
            }

            public string Filter {
                get {
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
