using FolderIconChangerWPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ezz_Helper.Files
{
    /// <summary>
    /// Working with INI Profile. By EzHassen. v1.0.1.8  [Cannot work alone].
    /// </summary>
    /// <remarks></remarks>
    [ToolboxItem(true), Serializable(), ToolboxBitmap(typeof(INI), "INIFile")] //To show the image. Copy INIFile.bmp file to the project by Solution Explorer and change in the Properties Build Action --> Embedded Resource
    public class INI : Component
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion Component Designer generated code

        //
        public INI()
            : base()
        {
            InitializeComponent();
            SectionsList_ = new List<Section>();
            iniF = new iniFile_API();
            SaveByStream = true;
            LoadByStream = true;
        }
        //private StreamWriter _myWriter;

        //{
        //    _myWriter = myWriter;
        //}

        #region "iniFile_API"

        /// <summary>
        /// Uses API functions.and Works directly with the ini file
        /// </summary>
        /// <remarks></remarks>
        [Serializable()]
        public class iniFile_API
        {
            public iniFile_API() : this(new INISettings()) { }

            public iniFile_API(INISettings iniSettings_)
            {
                Settings_ = iniSettings_;
            }
            public iniFile_API(string FileFullPath_1, Encoding TextEncoding__)
                : this(new INISettings(FileFullPath_1))
            {
                Settings_.TextEncoding = Get__Encoding(TextEncoding__);
            }
            public iniFile_API(string FileFullPath_1, _Encoding TextEncoding__ = _Encoding.Unicode)
                : this(new INISettings(FileFullPath_1))
            {
                Settings_.TextEncoding = TextEncoding__;
            }

            //sealed class NativeMethods
            public sealed class API_Functions
            {
                /// <summary>
                /// Marshal strings to Ansi
                /// </summary>
                /// <remarks></remarks>
                public sealed class Ansi
                {
                    //[return: MarshalAs(UnmanagedType.Bool)]
                    //For Getting all Sections
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNamesA", CharSet = CharSet.Ansi)]
                    public static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                                                  uint nSize, string lpFileName);
                    //For Getting keys in a Section
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionA", CharSet = CharSet.Ansi)]
                    public static extern uint GetPrivateProfileSection(string lpAppName,
                                                                           IntPtr lpReturnedString, uint nSize, string lpFileName);

                    /// <summary>
                    /// Replaces the keys and values for the specified section in an initialization file.
                    /// </summary>
                    /// <param name="lpAppName">The name of the section in which data is written. This section name is typically the name of the calling application.</param>
                    /// <param name="lpString">The new key names and associated values that are to be written to the named section. This string is limited to 65,535 bytes.</param>
                    /// <param name="lpFileName"></param>
                    /// <returns></returns>
                    /// <remarks></remarks>
                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileSectionA", CharSet = CharSet.Ansi)]
                    public static extern bool WritePrivateProfileSection(string lpAppName,
                                                                                        string lpString, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
                    public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
                                                                               StringBuilder lpReturnedString, uint nSize, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true)]
                    public static extern bool WritePrivateProfileString(string lpAppName,
                                                                             string lpKeyName, string lpString, string lpFileName);
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileIntA", CharSet = CharSet.Ansi)]
                    public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName,
                                                                         int nDefault, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
                    public static extern int FlushPrivateProfileString(int lpApplicationName, int lpKeyName, int lpString, string lpFileName);
                }

                /// <summary>
                /// Marshal strings to Unicode
                /// </summary>
                /// <remarks></remarks>
                public sealed class Unicode
                {
                    //[return: MarshalAs(UnmanagedType.Bool)]
                    //For Getting all Sections
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNamesW", CharSet = CharSet.Unicode)]
                    public static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                                                  uint nSize, string lpFileName);
                    //For Getting keys in a Section
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionW", CharSet = CharSet.Unicode)]
                    public static extern uint GetPrivateProfileSection(string lpAppName,
                                                                           IntPtr lpReturnedString, uint nSize, string lpFileName);

                    /// <summary>
                    /// Replaces the keys and values for the specified section in an initialization file.
                    /// </summary>
                    /// <param name="lpAppName">The name of the section in which data is written. This section name is typically the name of the calling application.</param>
                    /// <param name="lpString">The new key names and associated values that are to be written to the named section. This string is limited to 65,535 bytes.</param>
                    /// <param name="lpFileName"></param>
                    /// <returns></returns>
                    /// <remarks></remarks>
                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileSectionW", CharSet = CharSet.Unicode)]
                    public static extern bool WritePrivateProfileSection(string lpAppName,
                                                                                        string lpString, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode)]
                    public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
                                                                               StringBuilder lpReturnedString, uint nSize, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
                    public static extern bool WritePrivateProfileString(string lpAppName,
                                                                             string lpKeyName, string lpString, string lpFileName);
                    [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileIntW", CharSet = CharSet.Unicode)]
                    public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName,
                                                                         int nDefault, string lpFileName);

                    [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
                    public static extern int FlushPrivateProfileString(int lpApplicationName, int lpKeyName, int lpString, string lpFileName);
                }
            }

            #region "Properties"

            INISettings Settings_;
            public INISettings Settings
            {
                get { return Settings_; }
            }
            [Serializable()]
            public class INISettings
            {
                //private string FileFullPath_1;

                public INISettings()
                {
                    ChangeTrueFalseStrings = false;
                    TrueSting = "Enabled";
                    FalseSting = "Disabled";
                    StartCommentString = ";";
                    TrimTabsFromValue = true;
                    TrimSpacesFromValue = true;
                    NewLineString = "/n";
                    TextEncoding = _Encoding.Unicode;
                }
                public INISettings(string FileFullPath_1)
                    : this()
                {
                    FileFullPath = FileFullPath_1;
                }

                /// <summary>
                /// Gets or Sets the full path of the INI File
                /// </summary>
                /// <value></value>
                /// <returns></returns>
                /// <remarks></remarks>
                [ReadOnly(false), Category("INI File"), DisplayName("File Full Path"), Description("INI Profile path to work with it.")]
                public string FileFullPath { get; set; }

                [Category("INI File - True,Fales Strings"), Description("To Change the True and Fales Strings. When Get or Write a Boolean"), DefaultValue(false), DisplayName("Change the True and Fales String"), RefreshProperties(RefreshProperties.All)]
                public bool ChangeTrueFalseStrings { get; set; }

                [Category("INI File - True,Fales Strings"), Description("The True String of the Boolean"), ReadOnly(false), DefaultValue("Enabled"), DisplayName("True Sting"), RefreshProperties(RefreshProperties.All)]
                public string TrueSting { get; set; }

                [Category("INI File - True,Fales Strings"), Description("The False String of the Boolean"), ReadOnly(false), DefaultValue("Disabled"), DisplayName("False Sting"), RefreshProperties(RefreshProperties.All)]
                public string FalseSting { get; set; }

                [Category("INI File - Options"), ReadOnly(false), DefaultValue(";"), DisplayName("Start Comment String"), RefreshProperties(RefreshProperties.None)]
                public string StartCommentString { get; set; }

                //Private TrimTabsFromValue As Boolean = False
                [Category("INI File - Options"), ReadOnly(false), DefaultValue(true), DisplayName("Trim tabs from key value"), RefreshProperties(RefreshProperties.None)]
                public bool TrimTabsFromValue { get; set; }

                //Private TrimSpacesFromValue_ As Boolean = False
                [Category("INI File - Options"), ReadOnly(false), DefaultValue(true), DisplayName("Trim spaces from key value"), RefreshProperties(RefreshProperties.None)]
                public bool TrimSpacesFromValue { get; set; }
                //    Get
                //        Return TrimSpacesFromValue_
                //    End Get
                //    Set(ByVal value As Boolean)
                //        TrimSpacesFromValue_ = value
                //    End Set
                //End Property
                //Private NewLineString_ As String = "/n"
                [Category("INI File - Options"), ReadOnly(false), DefaultValue("/n"), DisplayName("NewLine String"), Description("Replace new line with it in values and comments."), RefreshProperties(RefreshProperties.None)]
                public string NewLineString { get; set; }

                //Private TextEncoding_ As _Encoding = _Encoding.Unicode
                [Category("INI File - Options"), ReadOnly(false), DefaultValue("Unicode"), DisplayName("Character Encoding"), RefreshProperties(RefreshProperties.All)]
                public _Encoding TextEncoding { get; set; }

                public string GetIncSComment()
                {
                    return string.Format("/* {0} */", StartCommentString);
                    // /* {0} */ |<!-- {0} -->
                }
                public bool ValueContainsIncSComment(string Value)
                {
                    return Value.Contains(GetIncSComment());
                }
                public string ReplaceIncSCommentToNormal(string Value)
                {
                    return Value.Replace(GetIncSComment(), StartCommentString);
                }
                public string SetIncSComment_ToValue(string Value)
                {
                    return Value.Replace(StartCommentString, GetIncSComment());
                }
                public int IncSComment_CountInString(string str)
                {
                    try
                    {
                        var phrase = GetIncSComment();
                        return (str.Length - str.Replace(phrase, string.Empty).Length) / phrase.Length;
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                }
                public int IncSComment_Last1index(string str)
                {
                    return str.LastIndexOf(GetIncSComment());
                }
            }

            #endregion "Properties"

            #region "Functions & Methods"

            /// <summary>
            /// Create or clear file if exist
            /// </summary>
            /// <remarks></remarks>
            private void CreateINIFile()
            {
                File.WriteAllLines(Settings.FileFullPath, new string[] {
                }, this.GetCurrentTextEncoding());
                //Dim sw As StreamWriter = New StreamWriter(Settings.FileFullPath, False, Me.GetCurrentTextEncoding)
                //sw.Close()
            }
            public enum _Encoding
            {
                DefaultANSI = 1,
                Unicode = 0
            }
            public object CheckFileExistsNCreate()
            {
                try
                {
                    if (!File.Exists(Settings.FileFullPath))
                        CreateINIFile();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public void ChangeFileEncoding(_Encoding _Encoding_)
            {
                File.WriteAllText(Settings.FileFullPath, File.ReadAllText(Settings.FileFullPath), GetTextEncoding(_Encoding_));
            }
            public Encoding GetCurrentTextEncoding()
            {
                return GetTextEncoding(Settings.TextEncoding);
            }
            public Encoding GetTextEncoding(_Encoding _Encoding_)
            {
                return _Encoding_ == _Encoding.DefaultANSI ? Encoding.Default : Encoding.Unicode;
            }
            public Encoding GetFileEncoding()
            {
                return DetectFileEncoding(Settings.FileFullPath);
            }
            public static _Encoding Get__Encoding(Encoding Encoding_)
            {
                if (Encoding_.Equals(Encoding.Default))
                    return _Encoding.DefaultANSI;
                return _Encoding.Unicode;
            }
            public static System.Text.Encoding DetectFileEncoding(string filename)
            {
                try
                {
                    Encoding enc = null;
                    if (System.IO.File.Exists(filename))
                    {
                        //Dim filein As New System.IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)
                        using (System.IO.FileStream filein = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            if ((filein.CanSeek))
                            {
                                byte[] bom = new byte[5];
                                filein.ReadExactly(bom, 0, 5);
                                //UTF-8: EF BB BF
                                //UTF-16 big endian byte order: FE FF
                                //UTF-16 little endian byte order: FF FE
                                //UTF-32 big endian byte order: 00 00 FE FF
                                //UTF-32 little endian byte order: FF FE 00 00
                                //UTF-7 byte order : 2B 2F 76 38 2D

                                //MsgBox(BitConverter.ToString(bom))
                                //UTF-8: EF BB BF
                                if ((((bom[0] == 0xef) & (bom[1] == 0xbb) & (bom[2] == 0xbf))))
                                {
                                    return System.Text.Encoding.UTF8;

                                    //'UTF-16 little endian byte order: FF FE
                                }
                                else if (((bom[0] == 0xff) & (bom[1] == 0xfe)))
                                {
                                    return System.Text.Encoding.Unicode;

                                    //UTF-16 big endian byte order: FE FF
                                }
                                else if (((bom[0] == 0xfe) & (bom[1] == 0xff)))
                                {
                                    return System.Text.Encoding.BigEndianUnicode;

                                    //UTF-32 little endian byte order: FF FE 00 00
                                }
                                else if ((bom[0] == 0xff) & (bom[1] == 0xfe) & (bom[2] == 0x0) & (bom[3] == 0x0))
                                {
                                    return System.Text.Encoding.UTF32;

                                    //UTF-7 byte order : 2B 2F 76 38 2D
                                }
                                //else if ((bom[0] == 0x2b) & (bom[1] == 0x2f) & (bom[2] == 0x76) & (bom[3] == 0x38) & (bom[4] == 0x2d))
                                //{
                                //    return System.Text.Encoding.UTF7;

                                //    //else if (bom[0] = &H0) And (bom[1] = &H0) And (bom[2] = &HFE) And (bom[3] = &HFF) Then 'UTF-32 big endian byte order: 00 00 FE FF (ucs-4)
                                //    //Return System.Text.Encoding.

                                //    //enc = "Unicode"
                                //}
                                else
                                {
                                    //enc = "ASCII"
                                    return System.Text.Encoding.Default;
                                }
                                //'Position the file cursor back to the start of the file
                                //filein.Seek(0, System.IO.SeekOrigin.Begin)
                                //' Do more stuff
                            }
                        }
                        //filein.Close()
                    }
                    return enc;
                }
                catch
                {
                }

                return null;
            }

            /// <summary>
            /// Clears all data in the ini file.
            /// </summary>
            /// <remarks></remarks>
            public void Clear()
            {
                StreamWriter sw = new StreamWriter(Settings.FileFullPath, false, GetCurrentTextEncoding());
                sw.Close();
            }

            #endregion "Functions & Methods"

            #region "Get"

            public List<string> GetAllSections()
            {
                //
                List<string> SecNames = new List<string>();
                ///'''''''''''''''''''''''''''''''''''''''''''''
                uint MAX_BUFFER = 32767;
                //9999999
                IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
                uint bytesReturned;
                string local;

                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    bytesReturned = API_Functions.Ansi.GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, Settings.FileFullPath);
                    if (bytesReturned == 0)
                    {
                        Marshal.FreeCoTaskMem(pReturnedString);
                        return SecNames;
                    }
                    local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
                }
                else
                {
                    bytesReturned = API_Functions.Unicode.GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, Settings.FileFullPath);
                    if (bytesReturned == 0)
                    {
                        Marshal.FreeCoTaskMem(pReturnedString);
                        return SecNames;
                    }
                    local = Marshal.PtrToStringUni(pReturnedString, (int)bytesReturned).ToString();
                }

                Marshal.FreeCoTaskMem(pReturnedString);
                //use of Substring below removes terminating null for split
                //SecNames.AddRange(local.Substring(0, local.Length - 1).Split(ControlChars.NullChar).ToList)
                if (local.LongCount() != 0)
                {
                    SecNames.AddRange(local.Substring(0, local.Length - 1).Split(ControlChars.NullChar));
                }

                //MsgBox(SecNames.Count)
                return SecNames;
                //
                //Dim AllLines As String() = File.ReadAllLines(Settings.FileFullPath)
                //For Each line As String In AllLines
                //    Dim StatrI As Integer = 0
                //    Dim SLength As Integer = 0
                //    If line.Contains("[") = True AndAlso line.Contains("]") = True Then
                //        For i As Integer = 0 To line.Length - 1
                //            Dim IsSection As Boolean
                //            If line(i) = "[" Then
                //                StatrI = i
                //                If i = 0 Then
                //                    IsSection = True
                //                Else
                //                    For i2 As Integer = 1 To i

                //                        If line.Substring(0, i2).EndsWith(" ", StringComparison.CurrentCultureIgnoreCase) Then
                //                            IsSection = True
                //                        ElseIf line.Substring(0, i2).EndsWith(Microsoft.VisualBasic.vbTab, StringComparison.CurrentCultureIgnoreCase) Then
                //                            IsSection = True
                //                        Else
                //                            IsSection = False
                //                            Exit For
                //                        End If
                //                    Next
                //                End If
                //                If IsSection = False Then
                //                    Exit For
                //                End If
                //            ElseIf line(i) = "]" Then
                //                If IsSection = True Then
                //                    SLength = line.Substring(0, i).Length - StatrI - 1
                //                    SecNames.Add(line.Substring(StatrI + 1, SLength))
                //                End If
                //                Exit For
                //            End If
                //        Next
                //    End If
                //Next
                //Return SecNames
            }

            public string[] GetSectionKeysWithValues(string SectionName)
            {
                uint MAX_BUFFER = 32767;
                //9999999
                IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
                uint bytesReturned;
                string local;

                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    bytesReturned = API_Functions.Ansi.GetPrivateProfileSection(SectionName, pReturnedString, MAX_BUFFER, Settings.FileFullPath);
                    if (bytesReturned == 0)
                    {
                        Marshal.FreeCoTaskMem(pReturnedString);
                        return null;
                    }
                    local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
                }
                else
                {
                    bytesReturned = API_Functions.Unicode.GetPrivateProfileSection(SectionName, pReturnedString, MAX_BUFFER, Settings.FileFullPath);
                    if (bytesReturned == 0)
                    {
                        Marshal.FreeCoTaskMem(pReturnedString);
                        return null;
                    }
                    local = Marshal.PtrToStringUni(pReturnedString, (int)bytesReturned).ToString();
                }

                Marshal.FreeCoTaskMem(pReturnedString);
                //'use of Substring below removes terminating null for split
                return local.Split(new char[] { ControlChars.NullChar }, StringSplitOptions.RemoveEmptyEntries);
                //local.Substring(0, local.Length - 1).Split(ControlChars.NullChar)
            }
            public string[] GetSectionKeys(string SectionName)
            {
                var Res = GetSectionKeysWithValues(SectionName);
                for (int SI = 0; SI <= Res.Length - 1; SI++)
                {
                    //Dim s = Res(SI)
                    //Dim EI = s.IndexOf("=")
                    //If EI >= 0 Then Res(SI) = Left(s, EI)
                    //
                    Res[SI] = GetKeyName_FromString(Res[SI]);
                }
                return Res;
            }
            public string GetKeyName_FromString(string KeyWithValue)
            {
                var EI = KeyWithValue.IndexOf("=");
                if (EI >= 0)
                    //return Microsoft.VisualBasic.Left(KeyWithValue, EI);
                    return KeyWithValue.Left(EI);
                return KeyWithValue;
            }

            // ''' <summary>
            // ''' Returns the value of a key from FromKeyWithValue . And Converting the /NewLine Character To NewLine. And Returns The String After Modifying it
            // ''' </summary>
            // ''' <param name="String_1"></param>
            // ''' <param name="ToStartRmoveFrom"></param>
            // ''' <param name="RemoveTabs"></param>
            // ''' <param name="NewLine"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            //Public Function GetKeyComment(ByVal SectionName As String, ByVal Key As String, Optional ByVal [Default] As String = "") As String
            //    Dim FullValueLineWithComment As String = GetValueLineWithComment(SectionName, Key)
            //    Dim Comment As String = ""

            //    'If FullValueLineWithComment.Contains(P_StartCommentString) Then
            //    '    For i As Integer = 0 To FullValueLineWithComment.Length - 1
            //    '        If FullValueLineWithComment(i) = P_StartCommentString Then
            //    '            Comment = FullValueLineWithComment.Remove(0, i + 1)
            //    '            Exit For
            //    '        End If
            //    '    Next
            //    '    If Comment.Contains("/n") = True Then
            //    '        Comment = Comment.Replace("/n", Environment.NewLine)
            //    '    End If
            //    'End If
            //    Return Comment
            //End Function

            //Public Function GetKValue_FromKeyWithValue(ByVal String_1 As String, Optional ByVal ToStartRmoveFrom As String = ";", _
            //                                             Optional ByRef RemoveTabs As Boolean = True _
            //                                                , Optional ByRef RemoveWhiteSpaces As Boolean = False _
            //                                                  , Optional ByVal NewLine As String = "/n") As String
            //    Dim FinalS As String = String_1
            //    If String_1.Contains(ToStartRmoveFrom) Then
            //        Dim SI = String_1.IndexOf(ToStartRmoveFrom)
            //        FinalS = Microsoft.VisualBasic.Left(String_1, IIf(SI < 0, 0, SI))
            //    End If
            //    If RemoveTabs Then FinalS = FinalS.Trim(ControlChars.Tab) 'Microsoft.VisualBasic.vbTab

            //    If RemoveWhiteSpaces Then FinalS = FinalS.Trim()

            //    FinalS = FinalS.Replace(NewLine, Environment.NewLine)

            //    Return FinalS
            //End Function
            public string GetKeyValueWithComment(string Section, string Key, string Default = "")
            {
                // Returns a string from your INI file
                uint intCharCount;
                System.Text.StringBuilder objResult = new System.Text.StringBuilder(256);
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    intCharCount = API_Functions.Ansi.GetPrivateProfileString(Section, Key, Default, objResult, (uint)objResult.Capacity, Settings.FileFullPath);
                }
                else
                {
                    intCharCount = API_Functions.Unicode.GetPrivateProfileString(Section, Key, Default, objResult, (uint)objResult.Capacity, Settings.FileFullPath);
                }
                return objResult.ToString();
                //Microsoft.VisualBasic.Left(objResult.ToString, intCharCount)
            }
            public string GetKeyValueWithComment_FromString(string KeyWithValue, string Default = "")
            {
                var EI = KeyWithValue.IndexOf("=");
                if (EI >= 0)
                {
                    EI += 1;
                    if (EI >= 0 && EI < KeyWithValue.Length)
                        return KeyWithValue.Substring(EI);
                    //Microsoft.VisualBasic.Right(KeyWithValue, KeyWithValue.Length - EI)
                }
                return Default;
            }

            public string GetKeyComment(string Section, string Key, string Default = "")
            {
                return GetKeyComment_FromString(GetKeyValueWithComment(Section, Key, Default));
            }
            public string GetKeyComment_FromString(string ValueWithComment_, string Default = "")
            {
                if (ValueWithComment_.Contains(Settings.StartCommentString))
                {
                    var IncSC_Lindex = Settings.IncSComment_Last1index(ValueWithComment_);
                    if (IncSC_Lindex >= 0)
                    {
                        IncSC_Lindex += Settings.GetIncSComment().Length;
                        //string Res = Microsoft.VisualBasic.Right(ValueWithComment_, ValueWithComment_.Length - IncSC_Lindex);
                        string Res = ValueWithComment_.Right(ValueWithComment_.Length - IncSC_Lindex);
                        return Res.Replace(Settings.NewLineString, Environment.NewLine);
                    }
                }
                return Default;
            }

            public string GetKeyValue(string Section, string Key, string Default = "")
            {
                return GetKeyValue(Section, Key, Default, Settings.TrimSpacesFromValue, Settings.TrimTabsFromValue);
            }
            public string GetKeyValue(string Section, string Key, string Default, bool RemoveSpaces__, bool RemoveTabs__)
            {
                string FinalS = GetKeyValueWithComment(Section, Key, Default);
                return GetFinalValue_FromValueWithComment(FinalS, Default, RemoveSpaces__, RemoveTabs__);
            }
            public string GetKeyValue_FromString(string KeyWithValue, string Default = "")
            {
                return GetKeyValue_FromString(KeyWithValue, Default, Settings.TrimSpacesFromValue, Settings.TrimTabsFromValue);
            }
            public string GetKeyValue_FromString(string KeyWithValue, string Default, bool RemoveSpaces__, bool RemoveTabs__)
            {
                var FinalS = GetKeyValueWithComment_FromString(KeyWithValue, Default);
                return GetFinalValue_FromValueWithComment(FinalS, Default, RemoveSpaces__, RemoveTabs__);
            }
            public string GetFinalValue_FromValueWithComment(string ValueWithComment, string Default, bool RemoveSpaces__, bool RemoveTabs__)
            {
                string FinalS = ValueWithComment;
                if (FinalS.Contains(Settings.StartCommentString))
                {
                    var IncSC_Lindex = Settings.IncSComment_Last1index(FinalS);
                    int SCommentIndx = -1;
                    if (IncSC_Lindex >= 0)
                    {
                        IncSC_Lindex += Settings.GetIncSComment().Length;
                        SCommentIndx = FinalS.IndexOf(Settings.StartCommentString, IncSC_Lindex);
                    }
                    else
                    {
                        SCommentIndx = FinalS.IndexOf(Settings.StartCommentString);
                    }

                    if (SCommentIndx >= 0)
                    {
                        //FinalS = Microsoft.VisualBasic.Left(FinalS, SCommentIndx);
                        FinalS = FinalS.Left(SCommentIndx);
                        //
                        //Dim Res As String = ValueWithComment.Substring(SCommentIndx + Settings.StartCommentString.Length)
                        //FComment = Res.Replace(Settings.NewLineString, ControlChars.NewLine)
                    }
                    if (IncSC_Lindex > 0)
                        FinalS = Settings.ReplaceIncSCommentToNormal(FinalS);
                }
                if (RemoveTabs__)
                    FinalS = FinalS.Trim(ControlChars.Tab);
                //Microsoft.VisualBasic.vbTab
                if (RemoveSpaces__)
                    FinalS = FinalS.Trim();
                FinalS.Replace(Settings.NewLineString, ControlChars.NewLine);
                return FinalS;
            }

            public Section.Key Get_Key_Value_Comment(string KeyWithValue, string Default = "")
            {
                return Get_Key_Value_Comment(KeyWithValue, Default, Settings.TrimSpacesFromValue, Settings.TrimSpacesFromValue);
            }
            public Section.Key Get_Key_Value_Comment(string KeyWithValue, string Default, bool TrimSpaces__, bool TrimTabs__)
            {
                //If String.IsNullOrWhiteSpace(KeyWithValue) Then Return New String() {KeyWithValue, "", ""}
                if (string.IsNullOrWhiteSpace(KeyWithValue))
                    return new Section.Key(false, KeyWithValue, Default);
                string KeyStr = "";
                string ValueWithComment = "";
                var EI = KeyWithValue.IndexOf("=");
                if (EI >= 0)
                {
                    //EI += 1
                    //If EI >= 0 AndAlso EI < KeyWithValue.Length Then KeyStr = Microsoft.VisualBasic.Right(KeyWithValue, KeyWithValue.Length - EI)
                    if (EI >= 0)
                    {
                        KeyStr = KeyWithValue.Substring(0, EI);
                        //IIf((EI - 1) <= 0, "", KeyWithValue.Substring(0, EI - 1)) 'KeyWithValue.Substring(0, EI - 1)
                        ValueWithComment = KeyWithValue.Substring(EI + 1);
                        //
                        // ''KeyStr = Microsoft.VisualBasic.Right(KeyWithValue, KeyWithValue.Length - EI)
                        //Dim SS = KeyWithValue.Split({"="}, StringSplitOptions.None)
                        //KeyStr = SS(0).Replace("=", "")
                        //ValueWithComment = IIf(SS.Length > 0, SS(1), "")
                    }
                    else
                    {
                        KeyStr = "";
                    }
                    //Not a key
                }
                else
                {
                    return new Section.Key(false, KeyWithValue, Default);
                }
                //
                if (ValueWithComment != "")
                {
                    string FComment = "";
                    string FValue = ValueWithComment;
                    if (FValue.Contains(Settings.StartCommentString))
                    {
                        var IncSC_Lindex = Settings.IncSComment_Last1index(FValue);
                        int SCommentIndx = -1;
                        if (IncSC_Lindex >= 0)
                        {
                            IncSC_Lindex += Settings.GetIncSComment().Length;
                            SCommentIndx = FValue.IndexOf(Settings.StartCommentString, IncSC_Lindex);
                        }
                        else
                        {
                            SCommentIndx = FValue.IndexOf(Settings.StartCommentString);
                        }

                        if (SCommentIndx >= 0)
                        {
                            //FValue = Microsoft.VisualBasic.Left(FValue, SCommentIndx);
                            FValue = FValue.Left(SCommentIndx);
                            //
                            string Res = ValueWithComment.Substring(SCommentIndx + Settings.StartCommentString.Length);
                            //Microsoft.VisualBasic.Right(ValueWithComment, ValueWithComment.Length - IncSC_Lindex)
                            FComment = Res.Replace(Settings.NewLineString, ControlChars.NewLine);
                        }
                        if (IncSC_Lindex > 0)
                            FValue = Settings.ReplaceIncSCommentToNormal(FValue);
                    }
                    if (TrimTabs__)
                        FValue = FValue.Trim(ControlChars.Tab);
                    //Microsoft.VisualBasic.vbTab
                    if (TrimSpaces__)
                        FValue = FValue.Trim();
                    FValue = FValue.Replace(Settings.NewLineString, ControlChars.NewLine);
                    //
                    return new Section.Key(true, KeyStr, FValue, FComment);
                }
                else
                {
                    return new Section.Key(true, KeyStr, Default);
                }
                //return null;
            }

            //
            public double GetKeyValue_Number(string Section, string Key, double Default = 0)
            {
                var str = GetKeyValue(Section, Key, "0", true, true);
                return str.Val();
            }
            public bool GetKeyValue_Boolean(string Section, string Key, bool Default = false)
            {
                return CheckBoolean(GetKeyValue(Section, Key, Default.ToString(), true, true));
            }
            public bool CheckBoolean(string value)
            {
                if (value.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase) ||
                    value.Equals("1") ||
                    value.Equals("-1"))
                {
                    //    return false;
                }

                if (Settings.ChangeTrueFalseStrings) return (string.Equals(value, Settings.TrueSting, StringComparison.CurrentCultureIgnoreCase));
                return false;
            }

            public System.Type GetValueType(string Value)
            {
                if (Settings.ChangeTrueFalseStrings == true)
                {
                    if (Value == bool.TrueString || Value == bool.FalseString || Value == Settings.TrueSting || Value == Settings.FalseSting)
                    {
                        return typeof(bool);
                    }
                    else
                    {
                        if (Value.IsNumeric())
                        {
                            return typeof(double);
                        }
                        else
                        {
                            return typeof(string);
                        }
                    }
                }
                else
                {
                    if (Value == bool.TrueString || Value == bool.FalseString)
                    {
                        return typeof(bool);
                    }
                    else
                    {
                        if (Value.IsNumeric())
                        {
                            return typeof(double);
                        }
                        else
                        {
                            return typeof(string);
                        }
                    }
                }
                //return typeof(string);
            }

            #endregion "Get"

            #region "Write/Set"

            public void WriteString(string Section, string Key, string Value, string Comment = "")
            {
                //
                //If File.Exists(FileFullPath) = False Then
                //    CreatingINIFile(FileFullPath)
                //End If

                if (!(Comment == ""))
                {
                    if (Settings.TrimTabsFromValue)
                    {
                        Comment = ControlChars.Tab + Settings.StartCommentString + Comment;
                    }
                    else
                    {
                        Comment = Settings.StartCommentString + Comment;
                    }
                }
                //' Writes a string to your INI file
                if (Value.Contains(Settings.StartCommentString))
                    Value = Settings.SetIncSComment_ToValue(Value);

                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.WritePrivateProfileString(Section, Key, Value + Comment, Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.WritePrivateProfileString(Section, Key, Value + Comment, Settings.FileFullPath);
                }
            }
            public void WriteNumber(string Section, string Key, double Value, string Comment = "")
            {
                WriteString(Section, Key, Value.ToString(), Comment);
            }
            public void WriteBoolen(string Section, string Key, bool Value, string Comment = "")
            {
                WriteString(Section, Key, GetBoolString(Value), Comment);
            }

            public void WriteCommentToNewLine(string Comment)
            {
                string ResC = Settings.StartCommentString + Comment.Replace(ControlChars.NewLine, Settings.StartCommentString);
                //File.AppendText(Settings.FileFullPath)
                StreamWriter sw = new StreamWriter(Settings.FileFullPath, true);
                using (sw)
                {
                    sw.Write(ResC);
                }
                //sw.Close()
            }

            /// <summary>
            /// Stores all the cached changes to the INI file
            /// </summary>
            /// <remarks></remarks>
            public void Flush()
            {
                // Stores all the cached changes to your INI file
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.FlushPrivateProfileString(0, 0, 0, Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.FlushPrivateProfileString(0, 0, 0, Settings.FileFullPath);
                }
            }

            #endregion "Write/Set"

            //Delete
            public void DeleteSection(string Section_)
            {
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.WritePrivateProfileString(Section_, null, null, Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.WritePrivateProfileString(Section_, null, null, Settings.FileFullPath);
                }
            }
            public void DeleteSectionKeys(string Section_)
            {
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.WritePrivateProfileSection(Section_, "", Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.WritePrivateProfileSection(Section_, "", Settings.FileFullPath);
                }
            }
            public void DeleteKey(string Section_, string key_)
            {
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.WritePrivateProfileString(Section_, key_, null, Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.WritePrivateProfileString(Section_, key_, null, Settings.FileFullPath);
                }
            }

            //Edit
            /// <summary>
            /// Replaces the keys and values for the specified section in an initialization file.
            /// </summary>
            /// <param name="Section_"></param>
            /// <param name="KeysWithValues"></param>
            /// <remarks></remarks>
            public void EditKeys(string Section_, string KeysWithValues)
            {
                if (Settings.TextEncoding == _Encoding.DefaultANSI)
                {
                    API_Functions.Ansi.WritePrivateProfileSection(Section_, KeysWithValues, Settings.FileFullPath);
                }
                else
                {
                    API_Functions.Unicode.WritePrivateProfileSection(Section_, KeysWithValues, Settings.FileFullPath);
                }
            }

            public string GetBoolString(bool bool_)
            {
                if (Settings.ChangeTrueFalseStrings)
                {
                    if (bool_)
                    {
                        return Settings.TrueSting;
                    }
                    else
                    {
                        return Settings.FalseSting;
                    }
                }
                else
                {
                    return bool_.ToString();
                }
            }
        }

        #endregion "iniFile_API"

        iniFile_API iniF;
        [Browsable(false), ReadOnly(false), Category("INI"), DisplayName("ini file"), Description("INI Profile to work with it.")]
        public iniFile_API INIFile
        {
            get { return iniF; }
        }

        #region "Properties" '9

        /// <summary>
        /// Gets or Sets the full path of the INI File
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [ReadOnly(false), Category("INI File"), DisplayName("File Full Path"), Description("INI Profile path to work with it.")]
        public string FileFullPath
        {
            get { return INIFile.Settings.FileFullPath; }
            set { INIFile.Settings.FileFullPath = value; }
        }

        [Category("INI File - True,Fales Strings"), Description("To Change the True and Fales Strings. When Get or Write a Boolean"), DefaultValue(false), DisplayName("Change the True and Fales String"), RefreshProperties(RefreshProperties.All)]
        public bool ChangeTrueFalseStrings
        {
            get { return INIFile.Settings.ChangeTrueFalseStrings; }
            set { INIFile.Settings.ChangeTrueFalseStrings = value; }
        }
        [Category("INI File - True,Fales Strings"), Description("The True String of the Boolean"), ReadOnly(false), DefaultValue("Enabled"), DisplayName("True Sting"), RefreshProperties(RefreshProperties.All)]
        public string TrueSting
        {
            get { return INIFile.Settings.TrueSting; }
            set { INIFile.Settings.TrueSting = value; }
        }
        [Category("INI File - True,Fales Strings"), Description("The False String of the Boolean"), ReadOnly(false), DefaultValue("Disabled"), DisplayName("False Sting"), RefreshProperties(RefreshProperties.All)]
        public string FalseSting
        {
            get { return INIFile.Settings.FalseSting; }
            set { INIFile.Settings.FalseSting = value; }
        }

        //Private StartCommentString_ As String = ";"
        [Category("INI File - Options"), ReadOnly(false), DefaultValue(";"), DisplayName("Start Comment String"), RefreshProperties(RefreshProperties.None)]
        public string StartCommentString
        {
            get { return INIFile.Settings.StartCommentString; }
            set { INIFile.Settings.StartCommentString = value; }
        }

        //Private TrimTabsFromValue As Boolean = False
        [Category("INI File - Options"), ReadOnly(false), DefaultValue(true), DisplayName("Trim tabs from key value"), RefreshProperties(RefreshProperties.None)]
        public bool TrimTabsFromValue
        {
            get { return INIFile.Settings.TrimTabsFromValue; }
            set { INIFile.Settings.TrimTabsFromValue = value; }
        }

        //Private TrimSpacesFromValue_ As Boolean = False
        [Category("INI File - Options"), ReadOnly(false), DefaultValue(true), DisplayName("Trim spaces from key value"), RefreshProperties(RefreshProperties.None)]
        public bool TrimSpacesFromValue
        {
            get { return INIFile.Settings.TrimSpacesFromValue; }
            set { INIFile.Settings.TrimSpacesFromValue = value; }
        }
        //Private NewLineString_ As String = "/n"
        [Category("INI File - Options"), ReadOnly(false), DefaultValue("/n"), DisplayName("NewLine String"), Description("Replace new line with it in values and comments."), RefreshProperties(RefreshProperties.None)]
        public string NewLineString
        {
            get { return INIFile.Settings.NewLineString; }
            set { INIFile.Settings.NewLineString = value; }
        }

        //Private TextEncoding_ As _Encoding = _Encoding.Unicode
        [Category("INI File - Options"), ReadOnly(false), DefaultValue("Unicode"), DisplayName("Character Encoding"), RefreshProperties(RefreshProperties.All)]
        public iniFile_API._Encoding TextEncoding
        {
            get { return INIFile.Settings.TextEncoding; }
            set { INIFile.Settings.TextEncoding = value; }
        }

        //
        /// <summary>
        /// Manual saving by stream(it's fast 100x) otherwise use the api functions(Slow)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Category("INI File - Save & Load"), DefaultValue(true), DisplayName("Save by stream"), Description("Manual saving by stream(it's fast 100x) otherwise use the api functions(Slow)"), RefreshProperties(RefreshProperties.All)]
        public bool SaveByStream { get; set; }
        /// <summary>
        /// Manual loading by stream otherwise use the api functions
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Category("INI File - Save & Load"), DefaultValue(true), DisplayName("Load by stream"), Description("Manual loading by stream otherwise use the api functions"), RefreshProperties(RefreshProperties.All)]
        public bool LoadByStream { get; set; }

        #endregion "Properties" '9

        #region "Sections & Keys"

        [Serializable()]
        public class Section
        {
            public Section(bool IsSection_)
            {
                IsSection = IsSection_;
                if (IsSection_)
                    KeysList_ = new List<Key>();
            }
            public Section(bool IsSection_, string SName_)
            {
                SectionName = SName_;
                IsSection = IsSection_;
                if (IsSection_)
                    KeysList_ = new List<Key>();
            }

            public string SectionName { get; set; }
            public bool IsSection { get; set; }
            [Serializable()]
            public class Key
            {
                public Key(bool IsKey_)
                {
                    IsKey = IsKey_;
                }
                public Key(bool IsKey_, string KeyName_, string Value_ = "", string Comment_ = "")
                {
                    KeyName = KeyName_;
                    IsKey = IsKey_;
                    Value = Value_;
                    Comment = Comment_;
                }

                public string KeyName { get; set; }
                public string Value { get; set; }
                public string Comment { get; set; }
                public bool IsKey { get; set; }

                public string ToiniFormat(INI.iniFile_API.INISettings iniSettings)
                {
                    if (!IsKey)
                        return KeyName;
                    string FValue = "=" + iniSettings.SetIncSComment_ToValue(Value);
                    string FComment = "";
                    if (Comment != "")

                        FComment = (iniSettings.TrimTabsFromValue ? ControlChars.Tab.ToString() : iniSettings.TrimSpacesFromValue ? " " : "") + iniSettings.StartCommentString + Comment;
                    //FComment = If(iniSettings.TrimTabsFromValue, ControlChars.Tab, If(iniSettings.TrimSpacesFromValue, " ", "")) + iniSettings.StartCommentString + Comment;
                    return string.Format("{0}{1}{2}", KeyName, FValue, FComment);
                }
                public static Section.Key FrominiFormat(INI.iniFile_API ini_, string KeyWithValue, string Default = "")
                {
                    return ini_.Get_Key_Value_Comment(KeyWithValue, Default);
                }
                public static Key FrominiFormat(INI.iniFile_API ini_, string KeyWithValue, string Default, bool TrimSpaces__, bool TrimTabs__)
                {
                    return ini_.Get_Key_Value_Comment(KeyWithValue, Default, TrimSpaces__, TrimTabs__);
                }
                public override string ToString()
                {
                    return this.KeyName;
                }
            }
            List<Key> KeysList_;
            public List<Key> KeysList
            {
                get { return KeysList_; }
            }

            public string ToiniFormat(INI.iniFile_API.INISettings iniSettings, bool AddKeys = true)
            {
                if (!IsSection)
                    return SectionName;
                StringBuilder newSB = new StringBuilder();
                newSB.Append(string.Format("[{0}]{1}", SectionName, ControlChars.NewLine));
                if (AddKeys)
                {
                    foreach (Key k in KeysList)
                    {
                        newSB.AppendLine(k.ToiniFormat(iniSettings));
                    }
                }
                //newSB.AppendLine()
                return newSB.ToString();
            }

            public override string ToString()
            {
                return this.SectionName;
            }
        }
        List<Section> SectionsList_;
        [Browsable(false)]
        public List<Section> SectionsList
        {
            get { return SectionsList_; }
        }

        #endregion "Sections & Keys"

        //
        private List<Section> listToClearSectionKeys = new List<Section>();
        //
        private List<Section> listToDeleteSection = new List<Section>();
        private List<SectionKey> listToDeleteKey = new List<SectionKey>();
        private List<SectionRename> listToSectionRename = new List<SectionRename>();

        private List<KeyRename> listToKeyRename = new List<KeyRename>();
        private class SectionKey
        {
            public SectionKey(Section Se, Section.Key Key_)
            {
                Section = Se;
                Key = Key_;
            }

            public Section Section { get; set; }
            public Section.Key Key { get; set; }
        }
        private class SectionRename
        {
            public SectionRename(Section Section_, string oldName_, string newName_)
            {
                Section = Section_;
                oldName = oldName_;
                newName = newName_;
            }
            public Section Section { get; set; }
            public string oldName { get; set; }
            public string newName { get; set; }
        }
        private class KeyRename
        {
            public KeyRename(Section Se, Section.Key Key_, string oldName_, string newName_)
            {
                Section = Se;
                Key = Key_;
                oldName = oldName_;
                newName = newName_;
            }
            public Section Section { get; set; }
            public Section.Key Key { get; set; }
            public string oldName { get; set; }
            public string newName { get; set; }
        }

        private void RestVers()
        {
            listToClearSectionKeys = new List<Section>();
            listToDeleteSection = new List<Section>();
            listToDeleteKey = new List<SectionKey>();
            //
            listToSectionRename = new List<SectionRename>();
            listToKeyRename = new List<KeyRename>();
        }

        public Section.Key AddNewKey(Section ownerSection)
        {
            string NewKeyN = "New Key";
            string FinalName = NewKeyN;
            int int_ = 0;
            do
            {
                if (ownerSection.KeysList.Find(s => s.KeyName.Equals(FinalName)) == null) break; ///// TO DO: might not be correct. Was : Exit Do
                int_ += 1;
                FinalName = string.Format("{0} ({1})", NewKeyN, int_);
            } while (true);
            //
            var newK = new INI.Section.Key(true, FinalName);
            ownerSection.KeysList.Add(newK);
            return newK;
        }
        public Section AddNew()
        {
            string NewSectionN = "New Section";
            string FinalName = NewSectionN;
            int int_ = 0;
            do
            {
                if (SectionsList_.Find(s => s.SectionName.Equals(FinalName)) == null)
                    break; // TO DO: might not be correct. Was : Exit Do
                int_ += 1;
                FinalName = string.Format("{0} ({1})", NewSectionN, int_);
            } while (true);
            //
            var newS = new INI.Section(true, FinalName);
            newS.KeysList.Add(new Section.Key(true, "New Key"));
            SectionsList_.Add(newS);
            return newS;
        }
        public void Add(params Section[] sections)
        {
            SectionsList_.AddRange(sections);
        }

        public void ClearSectionKeys(params Section[] sections)
        {
            foreach (Section se in sections)
            {
                se.KeysList.Clear();
                listToClearSectionKeys.Add(se);
            }
        }
        public void DeleteSection(params Section[] sections)
        {
            foreach (Section se in sections)
            {
                SectionsList_.Remove(se);
                listToDeleteSection.Add(se);
            }
        }
        public void DeleteKey(Section ownerSection, Section.Key Key_)
        {
            ownerSection.KeysList.Remove(Key_);
            listToDeleteKey.Add(new SectionKey(ownerSection, Key_));
        }

        public void RenameSection(Section Se, string NewName)
        {
            var FindR = listToSectionRename.Find(r => r.newName == Se.SectionName);
            if (FindR != null)
            {
                FindR.oldName = Se.SectionName;
                FindR.newName = NewName;
            }
            else
            {
                listToSectionRename.Add(new SectionRename(Se, Se.SectionName, NewName));
            }
            Se.SectionName = NewName;
        }
        public void RenameKey(Section ownerSe, Section.Key key_, string NewName)
        {
            var FindR = listToKeyRename.Find(r => r.newName == key_.KeyName);
            if (FindR != null)
            {
                FindR.oldName = key_.KeyName;
                FindR.newName = NewName;
            }
            else
            {
                listToKeyRename.Add(new KeyRename(ownerSe, key_, key_.KeyName, NewName));
            }

            key_.KeyName = NewName;
        }

        #region "Save"

        [Category("INI_Save")]
        public event EventHandler Save_Aborted;
        [Category("INI_Save")]
        public event ErrorEventHandler Save_Error;
        [Category("INI_Save")]
        public event EventHandler Save_Finish;
        [Category("INI_Save")]
        public event CancelEventHandler Save_Start;
        //
        bool _Save_IsWorking = false;
        [Browsable(false)]
        public bool Save_IsWorking { get { return _Save_IsWorking; } }
        bool _Save_ErrorInLastOperation = false;
        [Browsable(false)]
        public bool Save_ErrorInLastOperation { get { return _Save_ErrorInLastOperation; } }

        CancellationToken _save_CancellationToken;

        public async Task SaveAsync(iniFile_API.INISettings INISettings,
            Action BeforeThreadStart = null, Action OnThreadTaskStart = null,
            Action OnThreadTaskFinish = null, Action OnFinish = null, CancellationToken cancellationToken = default)
        {
            //return Task.Run(() => Save(INISettings, BeforeThreadStart, OnThreadTaskStart, OnThreadTaskFinish, OnFinish));
            iniF = new iniFile_API(INISettings);
            await SaveAsync(BeforeThreadStart: BeforeThreadStart, OnThreadTaskStart: OnThreadTaskStart, OnThreadTaskFinish: OnThreadTaskFinish, OnFinish: OnFinish, cancellationToken: cancellationToken);
        }

        public async Task SaveAsync(Action BeforeThreadStart = null, Action OnThreadTaskStart = null,
            Action OnThreadTaskFinish = null, Action OnFinish = null, CancellationToken cancellationToken = default)
        {
            _save_CancellationToken = cancellationToken;
            _Save_IsWorking = true;
            //
            if (BeforeThreadStart != null)
                BeforeThreadStart();
            Save_SynContext = SynchronizationContext.Current;
            try
            {

                await Task.Run(() =>
                {
                    this.Save_TTask(OnThreadTaskStart: OnThreadTaskStart, OnThreadTaskFinish: OnThreadTaskFinish, OnFinish: OnFinish, cancellationToken: cancellationToken);
                }, _save_CancellationToken);
            }
            finally
            {
                _Save_IsWorking = false;
            }


            _save_CancellationToken = default;
        }

        //
        System.Threading.SynchronizationContext Save_SynContext;

        private void Save_TTask(Action OnThreadTaskStart = null, Action OnThreadTaskFinish = null, Action OnFinish = null, CancellationToken cancellationToken = default)
        {
            _Save_ErrorInLastOperation = false;
            //_Save_IsWorkig = True
            try
            {
                if (OnThreadTaskStart != null)
                    OnThreadTaskStart();
                //
                //' 'Pause & Resume the thread :)
                // ''Use this in a save point to pause current thread. when IsPaused changed to True.
                //If _Save_IsPaused Then
                //    Save_ResetEvent_.WaitOne()
                //End If
                //
                //Write the operation code here.
                //
                INIFile.CheckFileExistsNCreate();

                if (SaveByStream)
                {
                    //For Each DS As Section In listToDeleteSection
                    //    DS.KeysList.Clear()
                    //Next
                    //For Each DS As Section In listToDeleteSection
                    //    SectionsList_.Remove(DS)
                    //Next
                    //For Each sn As SectionKey In listToDeleteKey
                    //    sn.Section.KeysList.Remove(sn.Key)
                    //Next

                    iniSaveByStream(cancellationToken);
                }
                else
                {
                    //
                    if (SectionsList.Count != 0)
                    {
                        foreach (Section DS in listToClearSectionKeys)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            //DS.KeysList.Clear()
                            INIFile.DeleteSectionKeys(DS.SectionName);
                        }
                        foreach (Section DS in listToDeleteSection)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            //SectionsList_.Remove(DS)
                            INIFile.DeleteSection(DS.SectionName);
                        }
                        foreach (SectionKey sk in listToDeleteKey)
                        {
                            //sk.Section.KeysList.Remove(sk.Key)
                            if (cancellationToken.IsCancellationRequested) break;
                            INIFile.DeleteKey(sk.Section.SectionName, sk.Key.KeyName);
                        }
                        foreach (KeyRename kr in listToKeyRename)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            INIFile.DeleteKey(kr.Section.SectionName, kr.oldName);
                            INIFile.WriteString(kr.Section.SectionName, kr.newName, kr.Key.Value, kr.Key.Comment);
                        }
                        foreach (SectionRename so in listToSectionRename)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            INIFile.DeleteSection(so.oldName);
                            //As it will be added next
                            //For Each k As Section.Key In so.Section.KeysList
                            //    If k.IsKey Then INIFile.WriteString(so.Section.SectionName, k.KeyName, k.Value, k.Comment)
                            //Next
                        }
                        //
                        foreach (Section s in SectionsList)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            if (s.IsSection)
                            {
                                foreach (Section.Key k in s.KeysList)
                                {
                                    if (cancellationToken.IsCancellationRequested) break;
                                    if (k.IsKey)
                                        INIFile.WriteString(s.SectionName, k.KeyName, k.Value, k.Comment);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!cancellationToken.IsCancellationRequested) INIFile.Clear();
                    }

                    INIFile.Flush();
                }

                //
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                _Save_ErrorInLastOperation = true;
                _Save_IsWorking = false;
                Save_SynContext.Send((s) => OnSave_Error(new ErrorEventArgs(ex)), null);
            }
            finally
            {
                _Save_IsWorking = false;
                RestVers();
                if (OnThreadTaskFinish != null) OnThreadTaskFinish();
                Save_SynContext.Send((s) => OnSave_Finish(OnFinish), null);
            }
        }
        public void OnSave_Start(CancelEventArgs e) { if (Save_Start != null) Save_Start(this, e); }
        public void OnSave_Finish(Action OnFinish = null)
        {
            if (OnFinish != null) OnFinish();
            if (Save_Finish != null) { Save_Finish(this, EventArgs.Empty); }
        }
        public void OnSave_Error(ErrorEventArgs ex) { if (Save_Error != null) { Save_Error(this, ex); } }
        //

        private void iniSaveByStream(CancellationToken cancellationToken = default)
        {
            //INIFile.CheckFileExistsNCreate()
            string FPath = FileFullPath;
            var TEncoding_1 = INIFile.GetCurrentTextEncoding();
            //Dim AllText = File.ReadLines(FPath, TEncoding)
            //Dim newAllText As String() = New String() {}
            //INIFile.Clear()
            StreamWriter sWriter = new StreamWriter(FPath, false, TEncoding_1);
            sWriter.AutoFlush = false;
            using (sWriter)
            {
                foreach (Section s in SectionsList)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        sWriter.Flush();
                        break;
                    }
                    sWriter.WriteLine(s.ToiniFormat(iniF.Settings));
                }
                sWriter.Flush();
            }
        }

        #endregion "Save"

        #region "Load"

        //<Category("INI_Load")>
        public event EventHandler Load_Aborted;
        //<Category("INI_Load")>
        public event ErrorEventHandler Load_Error;
        //<Category("INI_Load")>
        public event EventHandler Load_Finish;
        //<Category("INI_Load")>
        public event CancelEventHandler Load_Start;
        //
        bool _Load_IsWorking = false;
        [Browsable(false)]
        public bool Load_IsWorking
        {
            get { return _Load_IsWorking; }
        }
        bool _Load_ErrorInLastOperation = false;
        [Browsable(false)]
        public bool Load_ErrorInLastOperation
        {
            get { return _Load_ErrorInLastOperation; }
        }

        //
        System.Threading.SynchronizationContext Load_SynContext;
        bool DetectFileEncoding_ = false;
        CancellationToken _LoadCancellationToken;
        public async Task LoadAsync(iniFile_API.INISettings INISettings, Action BeforeThreadStart = null, Action OnThreadTaskStart = null, Action OnThreadTaskFinish = null, Action OnFinish = null, bool detectFileEncoding = false, CancellationToken cancellationToken = default)
        {
            iniF = new iniFile_API(INISettings);
            await LoadAsync(BeforeThreadStart: BeforeThreadStart, OnThreadTaskStart: OnThreadTaskStart, OnThreadTaskFinish: OnThreadTaskFinish, OnFinish: OnFinish, detectFileEncoding: detectFileEncoding, cancellationToken: cancellationToken);
        }
        public async Task LoadAsync(Action BeforeThreadStart = null, Action OnThreadTaskStart = null, Action OnThreadTaskFinish = null, Action OnFinish = null, bool detectFileEncoding = false, CancellationToken cancellationToken = default)
        {
            _Load_IsWorking = true;
            _LoadCancellationToken = cancellationToken;
            //
            if (BeforeThreadStart != null)
                BeforeThreadStart();
            Load_SynContext = SynchronizationContext.Current;
            DetectFileEncoding_ = detectFileEncoding;
            try
            {
                await Task.Run(() =>
                {
                    this.Load_TTask(OnThreadTaskStart: OnThreadTaskStart, OnThreadTaskFinish: OnThreadTaskFinish, OnFinish: OnFinish, cancellationToken: cancellationToken);
                }, _LoadCancellationToken);
            }
            finally
            {
                _Load_IsWorking = false;
                _LoadCancellationToken = default;
            }
        }


        private void Load_TTask(Action OnThreadTaskStart = null, Action OnThreadTaskFinish = null, Action OnFinish = null, CancellationToken cancellationToken = default)
        {
            _Load_ErrorInLastOperation = false;
            //_Load_IsWorkig = True
            try
            {
                if (OnThreadTaskStart != null)
                    OnThreadTaskStart();
                //
                //' 'Pause & Resume the thread :)
                // ''Use this in a save point to pause current thread. when IsPaused changed to True.
                //If _Load_IsPaused Then
                //    Load_ResetEvent_.WaitOne()
                //End If
                //
                //Write the operation code here.
                //
                RestVers();
                //SectionsList_.Clear()
                SectionsList_ = new List<Section>();
                if (DetectFileEncoding_)
                    INIFile.Settings.TextEncoding = iniFile_API.Get__Encoding(INIFile.GetFileEncoding());
                if (LoadByStream)
                {
                    iniLoadByStream(cancellationToken);
                }
                else
                {
                    var AllSections = INIFile.GetAllSections();
                    //MsgBox(AllSections.Count)
                    foreach (string s in AllSections)
                    {
                        if (cancellationToken.IsCancellationRequested) break;
                        Section TS = new Section(true, s);
                        var Ks = INIFile.GetSectionKeysWithValues(s);
                        foreach (string k in Ks)
                        {
                            if (cancellationToken.IsCancellationRequested) break;
                            //TS.KeysList.Add(New Section.Key(s, INIFile.GetKeyValue_FromString(k), INIFile.GetKeyComment_FromString(k)))
                            //TS.KeysList.Add(Section.Key.FrominiFormat(iniF, k))
                            TS.KeysList.Add(iniF.Get_Key_Value_Comment(k));
                        }
                        SectionsList_.Add(TS);
                    }
                }
                //
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                _Load_ErrorInLastOperation = true;
                _Load_IsWorking = false;
                Load_SynContext.Send((s) => OnLoad_Error(new ErrorEventArgs(ex)), null);
            }
            finally
            {
                _Load_IsWorking = false;
                if (OnThreadTaskFinish != null)
                    OnThreadTaskFinish();
                Load_SynContext.Send((s) => OnLoad_TFinish(OnFinish), null);
            }
        }
        public void OnLoad_Error(ErrorEventArgs ex)
        {
            if (Load_Error != null)
            {
                Load_Error(this, ex);
            }
        }
        public void OnLoad_TFinish(Action OnFinish = null)
        {
            if (OnFinish != null)
                OnFinish();
            if (Load_Finish != null)
            {
                Load_Finish(this, EventArgs.Empty);
            }
        }

        //
        private void iniLoadByStream(CancellationToken cancellationToken = default)
        {
            SectionsList_ = new List<Section>();
            Section tmpsection = null;
            string FPath = INIFile.Settings.FileFullPath;
            //Dim TEncoding = INIFile.GetCurrentTextEncoding
            Regex regexsection = new Regex("^[\\s]*?\\[([^\\[\\s].*[^\\s\\]])\\]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //Dim regexkey As New Regex("^\s*([^=\s]*)[^=]*=\s*(.*\S|)", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            //Dim regexkey As New Regex("^\\s*([^=\\s]*)[^=]*=(.*)", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            StreamReader oReader = new StreamReader(FPath);
            while (!oReader.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested) break;
                string line = oReader.ReadLine();
                Match m = null;
                m = regexsection.Match(line);
                if (m.Success)
                {
                    tmpsection = new Section(true, m.Groups[1].Value.Trim());
                    SectionsList_.Add(tmpsection);
                }
                else
                {
                    if (tmpsection == null)
                    {
                        tmpsection = new Section(false, line);
                        SectionsList_.Add(tmpsection);
                        tmpsection = null;
                        //load  s key
                    }
                    else
                    {
                        tmpsection.KeysList.Add(Section.Key.FrominiFormat(INIFile, line));
                    }
                }
                //tmpsection = Nothing
                //line = line.Trim()
                //If line <> "" Then
                //    Dim m As Match = Nothing
                //    If regexsection.Match(line).Success Then
                //        m = regexsection.Match(line)
                //        'Trace.WriteLine(String.Format("Adding section [{0}]", m.Groups(1).Value.Trim()))
                //        tempsection = AddSection(m.Groups(1).Value.Trim())
                //    ElseIf regexkey.Match(line).Success And Not tempsection Is Nothing Then
                //        m = regexkey.Match(line)
                //        'Trace.WriteLine(String.Format("Adding Key [{0}]=[{1}]", m.Groups(1).Value.Trim(), m.Groups(2).Value.Trim()))
                //        tempsection.AddKey(m.Groups(1).Value.Trim()).value = m.Groups(2).Value.Trim()
                //    Else
                //        Trace.WriteLine(String.Format("Skipping unknown type of data: {0}", line))
                //    End If
                //End If
            }
            oReader.Close();
        }

        #endregion "Load"
    }
}