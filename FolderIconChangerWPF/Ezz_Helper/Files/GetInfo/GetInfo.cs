using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Collections;
using Ezz_Helper;
using Ezz_Helper.WinForms.IconsManager;
using FolderIconChangerWPF;
using FolderIconChangerWPF.Helpers;

namespace Ezz_Helper.Files.GetInfo
{

	public class GetFileInfo
	{

		public GetFileInfo(string FilePath__)
		{
			FilePath_ = FilePath__;
		}
		public string FilePath_;
		public string FilePath
		{
			get { return FilePath_; }
		}

		public bool FileExists
		{
			get { return File.Exists(FilePath_); }
		}

		//public string FileSize
		//{
		//    get
		//    {
		//        try
		//        {
		//            object FileInfo = new FileInfo(FilePath_);
		//            if (ConvertUnit == false)
		//            {
		//                if (ShowSizeUnit_1 == false)
		//                {
		//                    return FileInfo.Length.ToString;
		//                }
		//                else
		//                {
		//                    if (_UnitStrings_ == null)
		//                        _UnitStrings_ = new GetSize.UnitsStrings();
		//                    return string.Format("{0} {1}", FileInfo.Length.ToString, _UnitStrings_.bytesString);
		//                }
		//            }
		//            else
		//            {
		//                return GetSize.ConvertTheMeasurementUnitOfFileSize(FileInfo.Length.ToString, ShowSizeUnit_1, Decimal_Places, _UnitStrings_);
		//            }
		//        }
		//        catch (Exception ex)
		//        {
		//            return null;
		//        }
		//    }
		//}


		public string FileType
		{
			get { return GetFileType(this.FilePath); }
		}
		/// <summary>
		/// Get the type of the file
		/// </summary>
		/// <param name="FullFilePath"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string GetFileType(string FullFilePath)
		{
			try
			{
				//IntPtr FileTypeIP;
				NativeMethods.SHFILEINFO shinfo = new NativeMethods.SHFILEINFO();
				NativeMethods.SHGetFileInfo(FullFilePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)NativeMethods.SHGFI_TYPENAME | (uint)NativeMethods.SHGFI_DISPLAYNAME);
				return shinfo.szTypeName;
			}
			catch (Exception)
			{
				return string.Empty;
				//ex.Message
			}
		}


		//public Bitmap Icon_Bitmap
		//{
		//    get { return OtherMethods.IconNImage.ConvertIconToImage(OtherMethods.IconNImage.GetFileIcon(this.FilePath, GetMethod, Size_), true); }
		//}


		//public RenameInfo Rename(string _NewName)
		//{
		//    RenameInfo RI = OtherMethods.Rename(this.FilePath, _NewName);
		//    if (RI.ErrInRenaming == false)
		//    {
		//        FilePath_ = RI.NewPath;
		//    }
		//    return RI;
		//}
		//public bool Delete(FileIO.RecycleOption _RecycleOption, FileIO.UIOption UIOption = FileIO.UIOption.OnlyErrorDialogs)
		//{
		//    return OtherMethods.Delete(this.FilePath, _RecycleOption, UIOption);
		//}

		public string ParentFolderPath
		{
			get
			{
				try
				{
					return Path.GetDirectoryName(FilePath_);
				}
				catch (Exception)
				{
					return "";
				}
			}
		}
		public string ParentFolderName
		{
			get
			{
				try
				{
					dynamic PFP = ParentFolderPath;
					if (Path.GetPathRoot(PFP) == PFP)
					{
						return "";
					}
					else
					{
						return Path.GetFileName(PFP);
					}
				}
				catch (Exception)
				{
					return "";
				}
			}
		}

		//
		public static string GetShell32_dll_EnvironmentVariablesPath()
		{
			return "%SystemRoot%\\system32\\SHELL32.dll";
		}
		public static string GetShell32_dll_FullPath()
		{
			//Dim Shell32_dll As String = Environment.ExpandEnvironmentVariables("%SystemRoot%\system32\SHELL32.dll")
			return Environment.ExpandEnvironmentVariables(GetShell32_dll_EnvironmentVariablesPath());
		}

		public void Rename(string newName)
		{
			try
			{
				var DestPath = Path.Combine(Path.GetDirectoryName(FilePath_), newName);
				System.IO.File.Move(FilePath, DestPath);
				FilePath_ = DestPath;
			}
			catch
			{
				throw;
			}

		}
		public static void Rename(string FilePath, string newName)
		{
			System.IO.File.Move(FilePath, Path.Combine(Path.GetDirectoryName(FilePath), newName));
		}

	}
	public class GetDirectoryInfo
	{
		public GetDirectoryInfo(string DirectoryPath__)
		{
			DirectoryPath_ = DirectoryPath__;
		}
		public string DirectoryPath_;
		public string DirectoryPath
		{
			get { return DirectoryPath_; }
		}

		public bool DirectoryExists
		{
			get { return Directory.Exists(DirectoryPath_); }
		}


		[ReadOnly(true), Browsable(false)]
		public string DirectoryType
		{
			get { return GetFileType(this.DirectoryPath); }
		}
		/// <summary>
		/// Get the type of the file
		/// </summary>
		/// <param name="FullFilePath"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string GetFileType(string FullFilePath)
		{
			try
			{
				NativeMethods.SHFILEINFO shinfo = new NativeMethods.SHFILEINFO();
				NativeMethods.SHGetFileInfo(FullFilePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), NativeMethods.SHGFI_TYPENAME | NativeMethods.SHGFI_DISPLAYNAME);
				return shinfo.szTypeName;
			}
			catch (Exception)
			{
				return string.Empty;
				//ex.Message
			}
		}

		//    public Bitmap Icon_Bitmap {
		////If UseSHGetFileInfoMethod = True Then
		//            //Else
		//            //Return GetExInfo.FileAssociation.GetAssociatedIconToBitmap(Me.FilePath, Size_)
		//            //End If
		//        get { return OtherMethods.IconNImage.ConvertIconToImage(OtherMethods.IconNImage.GetFolderIcon(this.DirectoryPath, GetMethod, Size_, FolderType), true); }
		//    }

		//public RenameInfo Rename(string _NewName)
		//{
		//    RenameInfo RI = OtherMethods.Rename(DirectoryPath, _NewName);
		//    if (RI.ErrInRenaming == false) {
		//        DirectoryPath_ = RI.NewPath;
		//    }
		//    return RI;
		//}
		//public bool Delete(FileIO.RecycleOption _RecycleOption, FileIO.UIOption UIOption = FileIO.UIOption.OnlyErrorDialogs)
		//{
		//    return OtherMethods.Delete(this.DirectoryPath, _RecycleOption, UIOption);
		//}

		public string ParentFolderPath
		{
			get
			{
				if (Path.GetPathRoot(DirectoryPath_) == DirectoryPath_)
				{
					return "";
				}
				else
				{
					return Path.GetDirectoryName(DirectoryPath_);
				}
			}
		}
		public string ParentFolderName
		{
			get
			{
				if (Path.GetPathRoot(DirectoryPath_) == DirectoryPath_)
				{
					return "";
				}
				else
				{
					//Return Path.GetFileName(ParentFolderPath)
					return Path.GetFileName(Path.GetDirectoryName(DirectoryPath_));
				}
				//Return ""
			}
		}

		//
		//public void SetFolderCustomIcon(string IconFile, int IconIndex = 0, string InfoTip = "", bool? ConfirmFileOp = null, bool? NoSharing = null, Action<Exception> ToDoOnError = null, bool CopyTheIconToTargetFolder = false, string iconFileNewName = "", bool RenameIfExists = true)
		//{
		//    IconNImage.SetFolderCustomIcon(DirectoryPath_, IconFile, IconIndex, InfoTip, ConfirmFileOp, NoSharing, ToDoOnError, CopyTheIconToTargetFolder, iconFileNewName, RenameIfExists);
		//} 

		[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
		static extern UInt32 SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, UInt32 dwReadWrite);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		struct LPSHFOLDERCUSTOMSETTINGS
		{
			public UInt32 dwSize;
			public UInt32 dwMask;
			public IntPtr pvid;
			public string pszWebViewTemplate;
			public UInt32 cchWebViewTemplate;
			public string pszWebViewTemplateVersion;
			public string pszInfoTip;
			public UInt32 cchInfoTip;
			public IntPtr pclsid;
			public UInt32 dwFlags;
			public string pszIconFile;
			public UInt32 cchIconFile;
			public int iIconIndex;
			public string pszLogo;
			public UInt32 cchLogo;
		}
		/// <summary>
		/// Sets custom icon on folder
		/// </summary>
		/// <param name="TargetFolder_"></param>
		/// <param name="IconFile"></param>
		/// <param name="IconIndex"></param>
		/// <param name="ToDoOnError"></param>
		/// <param name="CopyTheIconToTargetFolder"></param>
		/// <param name="iconFileNewName"></param>
		/// <param name="SameFileNameDecisions_1"></param>
		public static void SetFolderCustomIcon(string TargetFolder_, string IconFile, int IconIndex = 0,
			Action<Exception> ToDoOnError = null, bool CopyTheIconToTargetFolder = false, string iconFileNewName = "", SameFileNameDecisions SameFileNameDecisions_1 = SameFileNameDecisions.Rename, bool HideFileIfitInFolder = false)
		{
			//string InfoTip = "", Nullable<bool> ConfirmFileOp = null, Nullable<bool> NoSharing = null,
			try
			{
				if (!Directory.Exists(TargetFolder_)) return;
				//
				if (CopyTheIconToTargetFolder)
				{
					if (!(IconFile.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) && Path.GetDirectoryName(IconFile).Equals(TargetFolder_, StringComparison.CurrentCultureIgnoreCase)))
					{
						SaveToFileInfo Res = Drawing.IconsManager.IconExtractor.ExtractIconFromFileToFile(IconFile, IconIndex, Path.Combine(TargetFolder_, string.IsNullOrEmpty(iconFileNewName) ? $"{Path.GetFileNameWithoutExtension(IconFile)} - #{IconIndex}.ico" : iconFileNewName), SameFileNameDecisions_1);
						if (Res != null)
						{
							IconFile = Res.NewFilePath;
							if (IconIndex != 0) IconIndex = 0;
						}
					}
				}
				//
				string FileFullPath = IconFile;
				if (Path.GetDirectoryName(@IconFile) == TargetFolder_)
				{
					IconFile = Path.GetFileName(IconFile);
				}
				else
				{
					IconFile = PathFromFullToEV(IconFile);
				}
				//
				LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();
				FolderSettings.dwMask = 0x10;
				FolderSettings.pszIconFile = @IconFile;
				FolderSettings.iIconIndex = IconIndex;
				//if (InfoTip != "") FolderSettings .pszInfoTip= InfoTip;
				//if (ConfirmFileOp.HasValue) FolderSettings .// ConfirmFileOp.Value;
				//if (NoSharing.HasValue) FolderSettings.//NoSharing.Value;


				UInt32 FCS_READ = 0x00000001;
				UInt32 FCS_FORCEWRITE = 0x00000002;
				UInt32 FCS_WRITE = FCS_READ | FCS_FORCEWRITE;

				//string pszPath = TargetFolder_;
				UInt32 HRESULT = SHGetSetFolderCustomSettings(ref FolderSettings, TargetFolder_, FCS_FORCEWRITE);

				//using ini Note:- not Refreshing on vista>=
				string DesktopINI = Path.Combine(TargetFolder_, "Desktop.ini");

				INI.iniFile_API ini_ = new INI.iniFile_API(DesktopINI);
				ini_.Settings.ChangeTrueFalseStrings = true;
				ini_.Settings.TrueSting = "1";
				ini_.Settings.FalseSting = "0";
				////
				ini_.CheckFileExistsNCreate();
				////
				////
				ini_.WriteString(".ShellClassInfo", "IconResource", string.Format("{0},{1}", IconFile, IconIndex.ToString()));
				ini_.WriteString(".ShellClassInfo", "IconFile", string.Format("\"{0}\"", IconFile));
				ini_.WriteString(".ShellClassInfo", "IconIndex", IconIndex.ToString());
				//////
				//if (InfoTip != "") ini_.WriteString(".ShellClassInfo", "InfoTip", InfoTip);
				//if (ConfirmFileOp.HasValue) ini_.WriteBoolen(".ShellClassInfo", "ConfirmFileOp", ConfirmFileOp.Value);
				//if (NoSharing.HasValue) ini_.WriteBoolen(".ShellClassInfo", "NoSharing", NoSharing.Value);

				ini_.Flush();
				////

				if ((File.GetAttributes(DesktopINI) & FileAttributes.Hidden) != FileAttributes.Hidden) File.SetAttributes(DesktopINI, File.GetAttributes(DesktopINI) | FileAttributes.Hidden);
				if ((File.GetAttributes(DesktopINI) & FileAttributes.System) != FileAttributes.System) File.SetAttributes(DesktopINI, File.GetAttributes(DesktopINI) | FileAttributes.System);
				if ((File.GetAttributes(TargetFolder_) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly) File.SetAttributes(TargetFolder_, File.GetAttributes(TargetFolder_) | FileAttributes.ReadOnly);
				//
				if (HideFileIfitInFolder && (CopyTheIconToTargetFolder || Path.GetDirectoryName(FileFullPath) == TargetFolder_))
					if ((File.GetAttributes(FileFullPath) & FileAttributes.Hidden) != FileAttributes.Hidden) File.SetAttributes(FileFullPath, File.GetAttributes(FileFullPath) | FileAttributes.Hidden);
				//Refresh
				DirectoryInfo newDI = new DirectoryInfo(TargetFolder_);
				newDI.Refresh();
				OtherH.RefreshWindowsExplorers();
			}
			catch (Exception ex)
			{
				if (ToDoOnError != null)
					ToDoOnError(ex);
				else
					throw;
			}
		}

		public static Ezz_Helper.WinForms.IconsManager.SelectedIconInfo GetFolderIconInfo(string TargetFolder_)
		{
			string DesktoINIPath = Path.Combine(TargetFolder_, "Desktop.ini");
			if (!File.Exists(DesktoINIPath)) return null;

			string IconFile = "";
			int IconIndex = 0;


			//GetDesktopINIInfo
			INI.iniFile_API ininew = new INI.iniFile_API(DesktoINIPath);

			string FullStr = "";
			//Xp
			if (Environment.OSVersion.Version.Major <= 5)
			{
				FullStr = ininew.GetKeyValue(".ShellClassInfo", "IconFile");
				if (string.IsNullOrEmpty(FullStr))
				{
					//MsgBox("ad654sa4f")
					goto finish;
				}

				IconIndex = (int)ininew.GetKeyValue_Number(".ShellClassInfo", "IconIndex", 0);

			}
			else//above xp
			{
				FullStr = ininew.GetKeyValue(".ShellClassInfo", "IconResource");
				if (FullStr != "")
				{
					int symIndex = FullStr.LastIndexOf(',');
					if (symIndex > 0)
					{
						if (symIndex < FullStr.Length - 1) IconIndex = Convert.ToInt32(FullStr.Substring(symIndex + 1));
						FullStr = FullStr.Substring(0, symIndex);
					}
				}
				else
				{
					FullStr = ininew.GetKeyValue(".ShellClassInfo", "IconFile");
					if (string.IsNullOrEmpty(FullStr))
					{
						goto finish;
					}
					IconIndex = (int)ininew.GetKeyValue_Number(".ShellClassInfo", "IconIndex", 0);
				}
			}
			//
			FullStr = Environment.ExpandEnvironmentVariables(FullStr);
			//PathFromEVToFull(FullStr)
			bool TestP = Path.IsPathRooted(FullStr);
			//If Not File.Exists(FullStr) 
			if (TestP == false)
			{
				//Do
				//    Dim EnvironmentVariable As String = FilesOperations.GetStringBetween2Strings(FullStr, "%", "%")
				//    If String.IsNullOrEmpty(EnvironmentVariable) = True Then Exit Do
				//    FullStr = FullStr.Replace("%" & EnvironmentVariable & "%", Environment.GetEnvironmentVariable(EnvironmentVariable))
				//Loop
				//If Not File.Exists(FullStr) Then
				FullStr = Path.Combine(TargetFolder_, FullStr);
				FullStr = Path.GetFullPath(FullStr);
				//If File.Exists(FullStr) = False Then GoTo finish '
				//End If
			}

			IconFile = FullStr;
			finish:

			//
			//if (File.Exists(IconFile)) return null;

			var SIInfo = new Ezz_Helper.WinForms.IconsManager.SelectedIconInfo();

			SIInfo.FilePath = IconFile;
			SIInfo.Index = IconIndex;
			SIInfo.IconViewType = IconViewTypes.OneIcon;
			//SIInfo.iCount = GetIconsCountInFile(IconFile);
			SIInfo.SourceIcon = Drawing.IconsManager.IconExtractor.ExtractIcon(IconFile, IconIndex);
			SIInfo.DialogResult = true;


			return SIInfo;
		}
		public static void RestFolderIcon(string TargetFolder_)
		{
			try
			{
				LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();
				FolderSettings.dwMask = 0x10;
				FolderSettings.pszIconFile = "";
				//FolderSettings.iIconIndex = 0;

				//
				UInt32 HRESULT = SHGetSetFolderCustomSettings(ref FolderSettings, TargetFolder_, 0x00000002);
				string DesktopINI = Path.Combine(TargetFolder_, "Desktop.ini");
				INI.iniFile_API ini_ = new INI.iniFile_API(DesktopINI);
				ini_.DeleteKey(".ShellClassInfo", "IconResource");
				ini_.DeleteKey(".ShellClassInfo", "IconFile");
				ini_.DeleteKey(".ShellClassInfo", "IconIndex");

			}
			catch (Exception)
			{
				throw;
			}
		}

		private static string PathFromFullToEV(string FullPath_)
		{
			string finalS = FullPath_;
			var GEVs = Environment.GetEnvironmentVariables();

			foreach (DictionaryEntry EV_ in GEVs)
			{
				try
				{
					string EVValue = (string)EV_.Value;

					if (!string.IsNullOrEmpty(EVValue))
					{
						if (Path.IsPathRooted(EVValue) && Directory.Exists(EVValue))
						{
							if (Path.GetPathRoot(EVValue) != EVValue)
							{
								if (finalS.ToUpper().Contains(EVValue.ToUpper()))
								{
									string EVKey = (string)EV_.Key;
									finalS = finalS.Replace(EVValue, string.Format("%{0}%", EVKey));
									break;
								}
							}
						}
					}
				}
				catch (Exception)
				{

					//throw;
				}
			}

			return finalS;
		}

		//public static int GetIconsCountInFile(string FilePath_)
		//{
		//    try
		//    {
		//        //IntPtr intSmall = new IntPtr();
		//        //IntPtr intLarge = new IntPtr();
		//        //return NativeMethods.ExtractIconEx(FilePath_, -1, intLarge, intSmall, -1);
		//    }
		//    catch (Exception ex)
		//    {
		//        return 0;
		//    }
		//}
	}

	public class GetExInfo
	{

		public GetExInfo()
		{
		}
		public GetExInfo(string Extension__)
		{
			Extension = Extension__;
		}
		string Extension_ = "";
		public string Extension
		{
			get { return Extension_; }
			set { Extension_ = CheckNReturnExt(value); }
		}

		public static string CheckNReturnExt(string Ext)
		{
			if (string.IsNullOrEmpty(Ext))
				return "";
			if (!Ext.StartsWith("."))
				return "." + Ext;
			return Ext;
		}

		public string FileType
		{
			get { return GetFileTypeByExtension(this.Extension); }
		}
		public static string GetFileTypeByExtension(string Ext)
		{
			Ext = CheckNReturnExt(Ext);

			//IntPtr FileTypeIP;
			//Win32.SHFILEINFO shinfo = new Win32.SHFILEINFO();
			string FullFilePath_1 = Path.Combine(Path.GetTempPath(), "gTTYPE" + Ext);
			if (File.Exists(FullFilePath_1) == false)
			{
				File.Create(FullFilePath_1).Close();
			}
			string Res;
			FileInfo FI = new FileInfo(FullFilePath_1);
			//FileTypeIP = SHGetFileInfo(FullFilePath_1, 0, shinfo, Marshal.SizeOf(shinfo), SHGFI_TYPENAME | SHGFI_DISPLAYNAME);
			Res = GetFileInfo.GetFileType(FullFilePath_1);
			if (File.Exists(FullFilePath_1))
			{
				FI.Delete();
			}
			return Res;
			//return shinfo.szTypeName;
		}

		//public Bitmap Icon_Bitmap {
		//    get {
		//        if (UseSHGetFileInfoMethod == true) {
		//            return OtherMethods.IconNImage.ConvertIconToImage(OtherMethods.IconNImage.GetFileIcon(this.Extension, Size_));
		//        } else {
		//            return FileAssociation.GetAssociatedIconToBitmap(this.Extension, Size_);
		//        }
		//    }
		//}


		public static List<string> GetAllExtInRegistry()
		{
			List<string> newL = new List<string>();
			try
			{
				//newL.Add("*")
				newL.AddRange(Registry.ClassesRoot.GetSubKeyNames().Where(n => n.StartsWith(".")));
			}
			catch (Exception)
			{
			}
			return newL;
		}
		public static List<GetExInfo> GetAllExtInfoInRegistry()
		{
			List<GetExInfo> newL = new List<GetExInfo>();
			var KeyNames = Registry.ClassesRoot.GetSubKeyNames().Where(n => n.StartsWith("."));
			foreach (var Key in KeyNames)
			{
				newL.Add(new GetExInfo(Key));
			}
			//
			//dynamic Exts = GetAllExtInRegistry();
			//foreach (string Str in Exts)
			//{

			//    newL.Add(new GetExInfo(Str));
			//}
			return newL;
		}

		public override string ToString()
		{
			return this.Extension;
			//return base.ToString();
		}



		public class FileAssociation
		{
			private FileAssociation() { }
			/// <summary>
			/// Initializes a new instance of the <see cref="FileAssociation"/> class.
			/// </summary>
			/// <param name="extension_1">The extension to be associated with an app.</param>
			/// <param name="progID_1">progID(or GUID) for the extension.</param>
			/// <param name="applicationPath_1">The full path of the application to associate with the extension.</param>
			/// <param name="DefaultIconValue_1">DefaultIcon value (shortPath+,+iconID)  Like : (Directory\File~1.icon) or (Directory\File~1.exe,1).</param>
			/// <param name="description_1">File type (Description).</param>
			public FileAssociation(string extension_1, string progID_1, string applicationPath_1, string DefaultIconValue_1, string description_1)
			{
				Extension = extension_1;
				ProgID = progID_1;
				ApplicationPath = applicationPath_1;
				DefaultIconValue = DefaultIconValue_1;
				Description = description_1;
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="FileAssociation"/> class.
			/// </summary>
			/// <param name="extension_1">The extension to be associated with an app.</param>
			/// <param name="progID_1">progID(or GUID) for the extension.</param>
			/// <param name="applicationPath_1">The full path of the application to associate with the extension.</param>
			/// <param name="IconFile">IconFile full path.</param>
			/// <param name="IconID">Icon ID or Index.</param>
			/// <param name="description_1">File type (Description).</param>
			public FileAssociation(string extension_1, string progID_1, string applicationPath_1, string IconFile, int? IconID, string description_1)
			{
				Extension = extension_1;
				ProgID = progID_1;
				ApplicationPath = applicationPath_1;
				Description = description_1;
				SetDefaultIconValue(IconFile, IconID);
			}

			public string Extension { get; set; }
			private List<string> _OpenWithProgids = new List<string>();
			public ReadOnlyCollection<string> OpenWithProgids { get { return _OpenWithProgids.AsReadOnly(); } }//in ext key

			public string ProgID { get; set; }
			public string ApplicationPath { get; set; }
			public string DefaultIconValue { get; set; }
			public string Description { get; set; }


			public void SetDefaultIconValue(string IconFile, int? IconID)
			{
				if (IconID.HasValue)
				{
					DefaultIconValue = GetShortPathName(IconFile) + "," + IconID.ToString();
				}
				else
				{
					DefaultIconValue = GetShortPathName(IconFile);
				}
			}
			public int? GetIconID()
			{
				if (DefaultIconValue.IsNullOrEmpty()) return null;

				int syIndex = DefaultIconValue.LastIndexOf(',');
				if (syIndex == -1) return null;
				return DefaultIconValue.Substring(syIndex + 1).ValInt();
			}
			public string GetIconFile()
			{
				if (DefaultIconValue.IsNullOrEmpty()) return "";
				int syIndex = DefaultIconValue.LastIndexOf(',');
				if (syIndex != -1)
				{
					return syIndex == 0 ? "" : CommandLineParser.GetPathFromPathWithCommandLine(DefaultIconValue.Substring(0, syIndex));
				}
				else
				{
					return CommandLineParser.GetPathFromPathWithCommandLine(DefaultIconValue);
				}
			}
			public static string[] SplitPathNIconIndex(string path)
			{
				string[] res = new string[2];
				if (path.IsNullOrEmpty()) return res;
				if (path.StartsWith("\"") && path.EndsWith("\""))
				{
					if (path.Length > 3)
						path = path.Substring(1, path.Length - 2);
				}
				int syIndex = path.LastIndexOf(',');
				if (syIndex != -1)
				{
					res[0] = syIndex == 0 ? "" : CommandLineParser.GetPathFromPathWithCommandLine(path.Substring(0, syIndex));
					res[1] = path.Substring(syIndex + 1);
				}
				else
				{
					res[0] = CommandLineParser.GetPathFromPathWithCommandLine(path);
					res[1] = "";
				}
				return res;
			}

			//
			public bool IsAssociated()
			{
				return IsAssociated(Extension);
			}
			public bool IsAssociatedWithThisApp()
			{
				return IsAssociatedWithApp(Extension, ApplicationPath, ProgID);
			}

			public void Update(bool NotifyAssociationChanged_ = true)
			{
				Associate(NotifyAssociationChanged_);
			}
			public void Associate(bool NotifyAssociationChanged_ = true)
			{
				AssociateDef(Extension, ProgID, GetShortPathName(ApplicationPath), DefaultIconValue, Description, NotifyAssociationChanged_);
			}
			public void RemoveTheAssociation(bool DeleteProgIDKey = true, bool NotifyAssociationChanged_ = true)
			{
				RemoveAnAssociation(Extension, DeleteProgIDKey, NotifyAssociationChanged_);
			}

			public Microsoft.Win32.RegistryKey GetRegisteredExtKey()
			{
				return GetRegisteredExtKey(Extension);
			}
			public Microsoft.Win32.RegistryKey GetRegisteredProgIDKey()
			{
				return GetRegisteredProgIDKey(Extension);
			}
			public Microsoft.Win32.RegistryKey GetRegisteredOpenCommandKey()
			{
				return GetRegisteredOpenCommandKey(Extension);
			}
			public Microsoft.Win32.RegistryKey GetRegisteredDefaultIconKey()
			{
				return GetRegisteredDefaultIconKey(Extension);
			}

			public string GetAssociatedAppPath(bool GetLongPath = true)
			{
				return GetAssociatedAppPath(Extension, GetLongPath);
			}

			public IEnumerable<string> AddToOpenWithProgids(params string[] Progids_1)
			{
				var Res = AddToOpenWithProgids(this.Extension, Progids_1);
				_OpenWithProgids = new List<string>();
				if (Res.Count() != 0)
				{
					_OpenWithProgids.AddRange(Res);
				}
				return Res;
			}
			public IEnumerable<string> RemoveFromOpenWithProgids(params string[] Progids_1)
			{
				var Res = RemoveFromOpenWithProgids(this.Extension, Progids_1);
				_OpenWithProgids = new List<string>();
				if (Res.Count() != 0)
				{
					_OpenWithProgids.AddRange(Res);
				}
				return Res;
			}

			public static FileAssociation FromRegistry(string extension)
			{
				extension = CheckNReturnExt(extension);
				FileAssociation FA = new FileAssociation();
				FA.Extension = extension;

				var ExtKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension, false);
				string PrIDstr = "";
				if (ExtKey == null) return FA;

				var OpenWithProgidsKey = ExtKey.OpenSubKey("OpenWithProgids", false);
				if (OpenWithProgidsKey != null) if (OpenWithProgidsKey.ValueCount != 0) FA._OpenWithProgids.AddRange(OpenWithProgidsKey.GetValueNames());


				PrIDstr = (string)ExtKey.GetValue("");
				if (string.IsNullOrEmpty(PrIDstr)) return FA;

				var PID = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(PrIDstr, false);
				if (PID != null)
				{
					FA.ProgID = PrIDstr;
					FA.Description = (string)PID.GetValue("");
					var DefIconKey = PID.OpenSubKey("DefaultIcon");
					if (DefIconKey != null)
					{
						FA.DefaultIconValue = (string)DefIconKey.GetValue("");
					}
					//
					var ShellKey = PID.OpenSubKey("shell");
					if (ShellKey != null)
					{
						var openKey = ShellKey.OpenSubKey("open");
						if (openKey != null)
						{
							var oCommandKey = openKey.OpenSubKey("command");
							if (oCommandKey != null)
							{
								FA.ApplicationPath = CommandLineParser.GetPathFromPathWithCommandLine((string)oCommandKey.GetValue(""), true);
							}
						}
					}
				}
				return FA;
			}

			#region static methods

			public static void NotifyAssociationChanged()
			{
				NativeMethods.SHChangeNotify(NativeMethods.SHCNE_ASSOCCHANGED, NativeMethods.SHCNF_IDLIST, 0, 0);
			}

			public static bool Key_IsExists(Microsoft.Win32.RegistryKey regKey_)
			{
				return regKey_ != null;
			}
			public static bool ValueName_IsExists(Microsoft.Win32.RegistryKey regKey_, string ValueName_)
			{
				if (regKey_ == null) return false;
				return regKey_.GetValueNames().Contains(ValueName_);
			}

			public static Microsoft.Win32.RegistryKey GetRegisteredExtKey(string extension)
			{
				return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension, true);
			}
			public static Microsoft.Win32.RegistryKey GetRegisteredOpenWithProgidsKey(string extension)
			{
				var ExtKey = GetRegisteredExtKey(extension);
				if (ExtKey == null) return null;

				return ExtKey.OpenSubKey("OpenWithProgids", true);
			}

			public static string GetRegisteredProgIDString(string extension)
			{
				extension = CheckNReturnExt(extension);

				var exkey = GetRegisteredExtKey(extension);
				if (Key_IsExists(exkey) == true)
				{
					string value = (string)exkey.GetValue("");
					//exkey.Close();
					return value;
				}
				return "";
			}
			public static Microsoft.Win32.RegistryKey GetRegisteredProgIDKey(string extension)
			{
				extension = CheckNReturnExt(extension);
				string PrIDstr = GetRegisteredProgIDString(extension);
				if (string.IsNullOrEmpty(PrIDstr)) return null;
				return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(PrIDstr, true);
			}
			public static Microsoft.Win32.RegistryKey GetRegisteredOpenCommandKey(string extension)
			{
				var PID = GetRegisteredProgIDKey(extension);
				if (Key_IsExists(PID) == true)
				{
					var ShellKey = PID.OpenSubKey("shell");
					if (Key_IsExists(ShellKey) == true)
					{
						var openKey = ShellKey.OpenSubKey("open");
						if (Key_IsExists(openKey) == true)
						{
							return openKey.OpenSubKey("command");
						}
					}
				}
				return null;
			}
			public static Microsoft.Win32.RegistryKey GetRegisteredDefaultIconKey(string extension)
			{
				var PID = GetRegisteredProgIDKey(extension);
				if (Key_IsExists(PID))
				{
					return PID.OpenSubKey("DefaultIcon");
				}
				return null;
			}
			public static string GetAssociatedAppPath(string extension, bool GetLongPath = true)
			{
				string res = "";
				extension = CheckNReturnExt(extension);
				var CommandKey = GetRegisteredOpenCommandKey(extension);
				if (CommandKey != null)
				{
					res = CommandLineParser.GetPathFromPathWithCommandLine((string)CommandKey.GetValue(""), true);
					//CommandKey.Close();
				}
				return res;
			}



			/// <summary>
			/// Checks in registry if the extension is associated with an application
			/// </summary>
			/// <param name="extension">The extension to check for.</param>
			/// <returns>True if extension already associated with an application, Otherwise false.</returns>
			/// <remarks></remarks>
			public static bool IsAssociated(string extension)
			{
				bool res = false;
				extension = CheckNReturnExt(extension);
				var CommandKey = GetRegisteredOpenCommandKey(extension);
				if (CommandKey != null)
				{
					res = !string.IsNullOrEmpty((string)CommandKey.GetValue(""));
					//CommandKey.Close();
				}
				return res;
			}
			/// <summary>
			/// Checks in registry if the extension is associated with selected application path
			/// </summary>
			/// <param name="extension">The extension to check for.</param>
			/// <param name="applicationPath">Selected application path.</param>
			/// <returns>True if extension already associated with selected application path, Otherwise false.</returns>
			/// <remarks></remarks>
			public static bool IsAssociatedWithApp(string extension, string applicationPath)
			{
				bool res = false;
				extension = CheckNReturnExt(extension);
				var CommandKey = GetRegisteredOpenCommandKey(extension);
				if (CommandKey != null)
				{
					string DefValue = (string)CommandKey.GetValue("");
					if (!string.IsNullOrEmpty(DefValue))
					{
						string AssApp = CommandLineParser.GetPathFromPathWithCommandLine(DefValue, true);
						applicationPath = GetLongPathName(applicationPath);
						res = applicationPath.Equals(AssApp, StringComparison.CurrentCultureIgnoreCase);
					}
					//CommandKey.Close();
				}
				return res;
			}
			/// <summary>
			/// Checks in registry if the extension is associated with selected application path AndAlso progID
			/// </summary>
			/// <param name="extension">The extension to check for.</param>
			/// <param name="applicationPath">Selected application path.</param>
			/// <param name="progID">Selected progID.</param>
			/// <returns>True if extension already associated with selected application path AndAlso progID, Otherwise false.</returns>
			/// <remarks></remarks>
			public static bool IsAssociatedWithApp(string extension, string ApplicationPath, string progID)
			{
				bool res = false;
				extension = CheckNReturnExt(extension);
				var CommandKey = GetRegisteredOpenCommandKey(extension);
				if (CommandKey != null)
				{
					string DefValue = (string)CommandKey.GetValue("");
					if (!string.IsNullOrEmpty(DefValue))
					{
						string AssApp = CommandLineParser.GetPathFromPathWithCommandLine(DefValue, true);
						ApplicationPath = GetLongPathName(ApplicationPath);
						if (ApplicationPath.Equals(AssApp, StringComparison.CurrentCultureIgnoreCase))
						{
							res = GetRegisteredProgIDString(extension) == progID;
						}
					}
					//CommandKey.Close();
				}
				return res;
			}

			public static void RemoveAnAssociation(string extension, bool DeleteProgIDKey = true, bool NotifyAssociationChanged_ = true)
			{
				// ensure that there is a leading dot
				extension = CheckNReturnExt(extension);

				if (DeleteProgIDKey == true)
				{
					var PID = GetRegisteredProgIDKey(extension);
					if (Key_IsExists(PID) == true)
					{
						var PIDname = GetRegisteredProgIDString(extension);
						if (string.IsNullOrEmpty(PIDname) == false)
						{
							Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(PIDname, true);
						}
					}
				}

				var exkey = GetRegisteredExtKey(extension);
				if (Key_IsExists(exkey))
				{
					string PreProgIDValue = (string)exkey.GetValue("PreProgID");
					if (PreProgIDValue != null)
					{
						exkey.SetValue("", PreProgIDValue);
						exkey.DeleteValue("PreProgID");
					}
					else
					{
						exkey.SetValue("", "");
					}
					//exkey.Close();
				}
				Microsoft.Win32.Registry.ClassesRoot.Close();
				if (NotifyAssociationChanged_) NotifyAssociationChanged();
			}
			public static void RemoveMultiAssociation(string[] extensions, bool DeleteProgIDKey = true, bool NotifyAssociationChanged_ = true)
			{
				foreach (string Ext in extensions)
				{
					_RemoveAnAssociationMethod(Ext, DeleteProgIDKey);
				}
				Microsoft.Win32.Registry.ClassesRoot.Close();
				if (NotifyAssociationChanged_) NotifyAssociationChanged();
			}
			private static void _RemoveAnAssociationMethod(string extension, bool DeleteProgIDKey = true)
			{
				// ensure that there is a leading dot
				extension = CheckNReturnExt(extension);

				if (DeleteProgIDKey == true)
				{
					var PID = GetRegisteredProgIDKey(extension);
					if (Key_IsExists(PID) == true)
					{
						var PIDname = GetRegisteredProgIDString(extension);
						if (string.IsNullOrEmpty(PIDname) == false)
						{
							Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(PIDname, true);
						}
					}
				}

				var exkey = GetRegisteredExtKey(extension);
				if (Key_IsExists(exkey))
				{
					string PreProgIDValue = (string)exkey.GetValue("PreProgID");
					if (PreProgIDValue != null)
					{
						exkey.SetValue("", PreProgIDValue);
						exkey.DeleteValue("PreProgID");
					}
					else
					{
						exkey.SetValue("", "");
					}
					//exkey.Close();
				}

			}

			//TODO : Test Association
			/// <summary>
			/// Associate file extension with progID, description, icon and application. (path converted to ShortPath)
			/// </summary>
			/// <param name="extension"></param>
			/// <param name="progID"></param>
			/// <param name="applicationPath"></param>
			/// <param name="iconRes"></param>
			/// <param name="iconID"></param>
			/// <param name="description"></param>
			public static void Associate(string extension, string progID, string applicationPath, string iconRes, int iconID, string description = "", bool NotifyAssociationChanged_ = true)
			{
				if (string.IsNullOrEmpty(progID)) return;
				string IconResPath = GetShortPathName(iconRes);
				string IconRValue = string.IsNullOrEmpty(IconResPath) ? "" : IconResPath + "," + iconID.ToString();

				AssociateDef(extension, progID, GetShortPathName(applicationPath), IconRValue, description, NotifyAssociationChanged_);
			}
			/// <summary>
			/// Associate file extension with progID, description, icon and application
			/// </summary>
			/// <param name="extension"></param>
			/// <param name="progID"></param>
			/// <param name="applicationPath"></param>
			/// <param name="iconFile"></param>
			/// <param name="description"></param>
			public static void Associate(string extension, string progID, string applicationPath, string iconFile, string description = "", bool NotifyAssociationChanged_ = true)
			{
				AssociateDef(extension, progID, GetShortPathName(applicationPath), GetShortPathName(iconFile), description, NotifyAssociationChanged_);
			}

			/// <summary>
			/// Associate file extension with progID, description, icon and application
			/// </summary>
			/// <param name="extension"></param>
			/// <param name="progID"></param>
			/// <param name="applicationPath"></param>
			/// <param name="DefaultIconValue"></param>
			/// <param name="description"></param>
			/// <param name="NotifyAssociationChanged_"></param>
			public static void AssociateDef(string extension, string progID, string applicationPath, string DefaultIconValue, string description = "", bool NotifyAssociationChanged_ = true)
			{
				_AssociateMethod(extension, progID, applicationPath, DefaultIconValue, description);
				Microsoft.Win32.Registry.ClassesRoot.Close();
				if (NotifyAssociationChanged_) NotifyAssociationChanged();
			}
			public static void AssociateMultiExt(string[] extensions, string progID, string applicationPath, Func<string, string> DefaultIconValue, Func<string, string> description, bool NotifyAssociationChanged_ = true)
			{
				foreach (string Ext in extensions)
				{
					_AssociateMethod(Ext, progID, applicationPath, DefaultIconValue == null ? "" : DefaultIconValue(Ext), description == null ? "" : description(Ext));
				}
				Microsoft.Win32.Registry.ClassesRoot.Close();
				if (NotifyAssociationChanged_) NotifyAssociationChanged();
			}

			private static void _AssociateMethod(string extension, string progID, string applicationPath, string DefaultIconValue, string description = "")
			{
				if (string.IsNullOrEmpty(progID)) return;
				extension = CheckNReturnExt(extension);
				var exkey = GetRegisteredExtKey(extension);
				if (exkey != null)
				{
					string PreProgIDValue = (string)exkey.GetValue("");
					if (!PreProgIDValue.IsNullOrEmpty() && !string.Equals(PreProgIDValue, progID, StringComparison.CurrentCultureIgnoreCase))
					{
						// save current progID
						exkey.SetValue("PreProgID", PreProgIDValue);
					}
					else
					{
						exkey.DeleteValue("PreProgID", false);
					}
				}
				var ExtKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(extension);
				{
					ExtKey.SetValue("", progID);
					var OpenWithProgidsKey_1 = ExtKey.OpenSubKey("OpenWithProgids", true);
					if (OpenWithProgidsKey_1 == null)
					{
						OpenWithProgidsKey_1 = ExtKey.CreateSubKey("OpenWithProgids");
						OpenWithProgidsKey_1.SetValue(progID, null, RegistryValueKind.None);
					}
					else
					{
						if (!OpenWithProgidsKey_1.GetValueNames().Contains(progID, StringComparer.CurrentCultureIgnoreCase))
						{
							OpenWithProgidsKey_1.SetValue(progID, null, RegistryValueKind.None);
						}
					}
				}
				//
				var PIDKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(progID);
				{
					PIDKey.SetValue("", description);
					PIDKey.CreateSubKey("DefaultIcon").SetValue("", DefaultIconValue);
					PIDKey.CreateSubKey(@"Shell\Open\Command").SetValue("", "\"" + applicationPath + "\" \"%1\"");
				}
			}


			public static IEnumerable<string> AddToOpenWithProgids(string extension, params string[] Progids_1)
			{
				var ExtKey = GetRegisteredExtKey(extension);
				if (ExtKey == null) return new string[0];

				var OpenWithProgidsKey_1 = ExtKey.OpenSubKey("OpenWithProgids", true);
				if (OpenWithProgidsKey_1 == null)
				{
					OpenWithProgidsKey_1 = ExtKey.CreateSubKey("OpenWithProgids");
					foreach (var progID in Progids_1)
					{
						OpenWithProgidsKey_1.SetValue(progID, null, RegistryValueKind.None);
					}
				}
				else
				{
					foreach (var progID in Progids_1)
					{
						if (!OpenWithProgidsKey_1.GetValueNames().Contains(progID, StringComparer.CurrentCultureIgnoreCase))
						{
							OpenWithProgidsKey_1.SetValue(progID, null, RegistryValueKind.None);
						}
					}
				}
				string[] Res = OpenWithProgidsKey_1.GetValueNames();
				ExtKey.Close();
				return Res;
			}
			public static IEnumerable<string> RemoveFromOpenWithProgids(string extension, params string[] Progids_1)
			{
				var ExtKey = GetRegisteredExtKey(extension);
				if (ExtKey == null) return new string[0];
				var OpenWithProgidsKey_1 = ExtKey.OpenSubKey("OpenWithProgids", true);
				if (OpenWithProgidsKey_1 != null)
				{
					foreach (var progID in Progids_1)
					{
						OpenWithProgidsKey_1.DeleteValue(progID, false);
					}
					string[] Res = OpenWithProgidsKey_1.GetValueNames();
					if (OpenWithProgidsKey_1.ValueCount == 0)
					{
						ExtKey.DeleteSubKeyTree("OpenWithProgids", false);
					}
					ExtKey.Close();
					return Res;
				}
				return new string[0];
			}

			public static bool ChangeDefaultIcon(string extension, string NewDefaultIconValue, bool NotifyAssociationChanged_ = true)
			{
				var ProgIDKey = GetRegisteredDefaultIconKey(extension);
				if (ProgIDKey == null) return false;
				try
				{
					ProgIDKey.SetValue("", NewDefaultIconValue);
					ProgIDKey.Close();
					if (NotifyAssociationChanged_) NotifyAssociationChanged();
					return true;
				}
				catch
				{
					return false;
				}
			}


			// Return short path format of a file name
			public static string GetShortPathName(string longName)
			{
				//StringBuilder s = new StringBuilder(1000);
				//uint iSize = Convert.ToUInt32(s.Capacity);
				//uint iRet = NativeMethods.GetShortPathName(longName, s, iSize);
				//return s.ToString();
				//
				return NativeMethods.GetShortPathName(longName);
			}

			/// <summary>
			/// Converts a short path to a long path.
			/// </summary>
			/// <param name="shortPath">A path that may contain short path elements (~1).</param>
			/// <returns>The long path.</returns>
			public static string GetLongPathName(string shortPath)
			{
				return NativeMethods.GetLongPathName(shortPath);
				//
				//if (String.IsNullOrEmpty(shortPath))
				//{
				//    return shortPath;
				//}

				//StringBuilder builder = new StringBuilder(255);
				//int result = NativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
				//if (result > 0 && result < builder.Capacity)
				//{
				//    return builder.ToString(0, result);
				//}
				//else
				//{
				//    if (result > 0)
				//    {
				//        builder = new StringBuilder(result);
				//        result = NativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
				//        return builder.ToString(0, result);
				//    }
				//    else
				//    {
				//        //Throw New FileNotFoundException(String.Format(Globalization.CultureInfo.CurrentCulture,  "", shortPath), shortPath)
				//        return shortPath;
				//    }
				//}
			}

			public static string GetFullPathName(string ShortPath)
			{
				return System.IO.Path.GetFullPath(ShortPath);
			}

			//Private Declare Function ExtractIconEx Lib "shell32.dll" Alias "ExtractIconExA" (ByVal lpszFile As String, ByVal nIconIndex As Int32, <Out()> ByRef phiconLarge As IntPtr, <Out()> ByRef phiconSmall As IntPtr, ByVal nIcons As Int32) As Int32
			//public static Icon GetAssociatedIcon(string extension, IconNImage.GetIconMathods GetMethod = IconNImage.GetIconMathods.IconsManagerClass, OtherMethods.IconNImage.IconSize Size = OtherMethods.IconNImage.IconSize.Large)
			//{

			//    extension = CheckNReturnExt(extension);
			//    try {
			//        if (IsAssociated(extension) == false) {
			//            //MsgBox("Not Associated")
			//            return OtherMethods.IconNImage.GetFileIcon(extension, Size);
			//        }

			//        iconLocInfo iconLocInfo_;
			//        if (iconLocInfo_.HasValue == false) {
			//            return IconNImage.GetFileIcon(extension, IconNImage.GetIconMathods.DefaultMethod, Size);
			//        }
			//        Icon icon__1;
			//        icon__1 = IconNImage.GetIconFromFile(iconLocInfo_.Value.SourcePath, iconLocInfo_.Value.IconIndex, GetMethod, Size);
			//        //
			//        //Select Case GetMethod
			//        //    Case IconNImage.GetIconMathods.DefaultMethod
			//        //        Dim intSmall As New IntPtr
			//        //        Dim intLarge As New IntPtr
			//        //        '/// first we specify -1 for nIconIndex and nIcons ( this is to get the count of icons in the dll ) ...
			//        //        Dim count As Integer = ExtractIconEx(iconLocInfo_.Value.SourcePath, -1, intLarge, intSmall, -1)
			//        //        If count = 0 Then Return Nothing

			//        //        ExtractIconEx(iconLocInfo_.Value.SourcePath, iconLocInfo_.Value.IconIndex, intLarge, intSmall, 1)
			//        //        'MsgBox(IconIndex)

			//        //        If Size = OtherMethods.IconNImage.IconSize.Large Then
			//        //            icon__1 = DirectCast(Icon.FromHandle(intLarge).Clone(), Icon)
			//        //        Else
			//        //            icon__1 = DirectCast(Icon.FromHandle(intSmall).Clone(), Icon)
			//        //        End If
			//        //    Case Else 'IconNImage.GetIconMathods.IconsManagerClass
			//        //        icon__1 = IconNImage.GetIconFromFile(iconLocInfo_.Value.SourcePath, iconLocInfo_.Value.IconIndex)
			//        //End Select

			//        return icon__1;
			//    } catch (Exception ex) {
			//    }
			//    return null;
			//}
			//public static Bitmap GetAssociatedIconToBitmap(string extension, IconNImage.GetIconMathods GetMethod = IconNImage.GetIconMathods.IconsManagerClass, OtherMethods.IconNImage.IconSize Size = OtherMethods.IconNImage.IconSize.Large, bool GetBestFitIcon = true)
			//{
			//    object AIcon = GetAssociatedIcon(extension, GetMethod, Size);
			//    //If AIcon IsNot Nothing Then
			//    //    Return AIcon.ToBitmap
			//    //End If
			//    //Return Nothing
			//    return IconNImage.ConvertIconToImage(AIcon, GetBestFitIcon);
			//    }

			#endregion

			public XElement ToXElement()
			{
				var newObj = new XElement("FileAssociation",
									new XAttribute("Extension", this.Extension),
									new XAttribute("ProgID", this.ProgID),
									new XAttribute("ApplicationPath", this.ApplicationPath),
									new XAttribute("DefaultIconValue", this.DefaultIconValue),
									new XAttribute("Description", this.Description));

				return newObj;
			}
		}
		public class FileAssociationCollection : List<FileAssociation>
		{
			public FileAssociationCollection() : base() { }

			public void Add(string Extension, string ProgID, string ApplicationPath, string DefaultIconValue, string Description = "")
			{
				base.Add(new FileAssociation(Extension, ProgID, ApplicationPath, DefaultIconValue, Description));
			}

			public void UpdateAllAssociations(bool NotifyAssociationChanged_ = true)
			{
				foreach (FileAssociation item in this)
				{
					item.Update(false);
				}
				if (NotifyAssociationChanged_) FileAssociation.NotifyAssociationChanged();
			}
			public void RemoveAllAssociations(bool DeleteProgIDKey = true, bool NotifyAssociationChanged_ = true)
			{
				foreach (FileAssociation item in this)
				{
					item.RemoveTheAssociation(DeleteProgIDKey, false);
				}
				if (NotifyAssociationChanged_) FileAssociation.NotifyAssociationChanged();
			}


		}


	}



}
