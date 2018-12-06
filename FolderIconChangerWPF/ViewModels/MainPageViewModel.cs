using Ezz_Helper;
using Ezz_Helper.Drawing.IconsManager;
using Ezz_Helper.WinForms.IconsManager;
using FolderIconChangerWPF.Classes;
using FolderIconChangerWPF.IconInfoCore;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static FolderIconChangerWPF.IconInfoCore.IconHelper;
using static FolderIconChangerWPF.LocalizationProvider;
using Microsoft.Win32;
using FolderIconChangerWPF.Helpers;
using System.Diagnostics;
using System.ComponentModel;

namespace FolderIconChangerWPF.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        private static MainPageViewModel _instance;
        public static MainPageViewModel Instance
        {
            get
            {
                if (_instance == null) _instance = new MainPageViewModel();
                return _instance;
            }

            set
            {
                _instance = value;
            }
        }


        Window _OwnerWindow;
        public Window OwnerWindow
        {
            get { return _OwnerWindow; }
            set
            {
                if (_OwnerWindow != value)
                {
                    _OwnerWindow = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }


        private bool _IsWorking;

        public bool IsWorking
        {
            get { return _IsWorking; }
            set
            {
                if (_IsWorking != value)
                {
                    _IsWorking = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                if (value)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private string _StatusMsg;

        public string StatusMsg
        {
            get { return _StatusMsg; }
            set
            {
                if (_StatusMsg != value)
                {
                    _StatusMsg = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private string _TargetFolder;

        public string TargetFolder
        {
            get { return _TargetFolder; }
            set
            {
                if (_TargetFolder != value)
                {
                    _TargetFolder = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    CurrentRequireRefresh = true;
                }
                //if (!string.Equals(CurrentRefreshedPath, value))
                //{
                //}
            }
        }

        private DelegateCommand _ClearTargetFolderCommand;

        public DelegateCommand ClearTargetFolderCommand
            => _ClearTargetFolderCommand ?? (_ClearTargetFolderCommand = new DelegateCommand(() =>
            {
                if (IsWorking) return;
                TargetFolder = null;
                ResetCurrentInfo();
            }, (param) => !IsWorking));

        #region CurrentIconInfo

        private IconInfo _CurrentIconInfo;

        public IconInfo CurrentIconInfo
        {
            get { return _CurrentIconInfo; }
            set
            {
                if (_CurrentIconInfo != value)
                {
                    _CurrentIconInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }

                //if (value is null)
                //{
                //    CurrentIconPath = null;
                //    CurrentIconIndex = null;
                //}
                //else
                //{
                //    CurrentIconPath = GetIconFileShortPathIfInFolder(TargetFolder, value.FileName);
                //    CurrentIconIndex = value.Index;
                //}
            }
        }

        private bool _IsLoadingCurrentInfo;

        //string CurrentRefreshedPath;
        private bool _CurrentRequireRefresh;

        public bool CurrentRequireRefresh
        {
            get { return _CurrentRequireRefresh; }
            set
            {
                if (_CurrentRequireRefresh != value)
                {
                    _CurrentRequireRefresh = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        public bool IsLoadingCurrentInfo
        {
            get { return _IsLoadingCurrentInfo; }
            set
            {
                if (_IsLoadingCurrentInfo != value)
                {
                    _IsLoadingCurrentInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                IsWorking = value;
            }
        }

        private string _CurrentIconPath;

        public string CurrentIconPath
        {
            get { return _CurrentIconPath; }
            set
            {
                if (_CurrentIconPath != value)
                {
                    _CurrentIconPath = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private int? _CurrentIconIndex;

        public int? CurrentIconIndex
        {
            get { return _CurrentIconIndex; }
            set
            {
                if (_CurrentIconIndex != value)
                {
                    _CurrentIconIndex = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private DelegateCommand _RefreshCurrentInfoCommand;

        public DelegateCommand RefreshCurrentInfoCommand
            => _RefreshCurrentInfoCommand ?? (_RefreshCurrentInfoCommand = new DelegateCommand(async () =>
            {
                await RefreshCurrentInfo();
            }, (param) => !IsWorking));

        DelegateCommand _ResetTargetFolderIconCommand;
        public DelegateCommand ResetTargetFolderIconCommand
            => _ResetTargetFolderIconCommand ?? (_ResetTargetFolderIconCommand = new DelegateCommand(async () =>
            {
                await ResetTargetFolderIcon();
            }, (param) => !IsWorking));


        DelegateCommand _CurrentGenBestFitCommand;
        public DelegateCommand CurrentGenBestFitCommand
            => _CurrentGenBestFitCommand ?? (_CurrentGenBestFitCommand = new DelegateCommand(async () =>
            {
                await CurrentGenBestFit();
            }, (param) => !IsWorking));


        DelegateCommand _CurrentOpenTargetFolderCommand;
        public DelegateCommand CurrentOpenTargetFolderCommand
            => _CurrentOpenTargetFolderCommand ?? (_CurrentOpenTargetFolderCommand = new DelegateCommand(() =>
            {
                OpenFolder(TargetFolder);
            }, (param) => !IsWorking));

        DelegateCommand _CurrentIconOpenContainingFolderCommand;
        public DelegateCommand CurrentIconOpenContainingFolderCommand
            => _CurrentIconOpenContainingFolderCommand ?? (_CurrentIconOpenContainingFolderCommand = new DelegateCommand(() =>
            {
                OpenContainingFolder(GetFileFullPathIfInFolder(TargetFolder, CurrentIconPath));
            }, (param) => !IsWorking));

        void OpenContainingFolder(string file)
        {
            if (string.IsNullOrEmpty(file)) return;
            OpenFolder(Path.GetDirectoryName(file));
        }
        void OpenFolder(string folder)
        {
            if (!Directory.Exists(folder)) return;

            //%SystemRoot%\explorer.exe
            IsWorking = true;
            try
            {
                var process = new Process();
                process.StartInfo.FileName = folder;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            finally
            {
                IsWorking = false;
            }
        }

        private async Task RefreshCurrentInfo()
        {
            if (IsWorking) return;

            if (!Directory.Exists(TargetFolder))
            {
                ResetCurrentInfo();
                StatusMsg = null;
                IsLoadingCurrentInfo = false;
                CurrentRequireRefresh = false;
                return;
            }
            var _currentIconInfo = CurrentIconInfo;
            ResetCurrentInfo();
            StatusMsg = GetLocalizedString("Loading");

            var tRes = await Task.Run(() =>
             {
                 Select_Icon.SelectedIconInfo FIInfo = null;
                 Exception Exception = null;
                 string IconFilePath = null;
                 int? IconIndex = null;
                 bool isDefaultIcon = false;
                 Icon fDefaultIcon = null;

                 try
                 {
                     FIInfo = Ezz_Helper.Files.GetInfo.GetDirectoryInfo.GetFolderIconInfo(TargetFolder);
                     isDefaultIcon = (FIInfo is null || FIInfo.SourceIcon is null);

                     if (GetDefaultFolderIcon && isDefaultIcon)
                     {
                         var Shell32File = Ezz_Helper.Files.GetInfo.GetFileInfo.GetShell32_dll_FullPath();
                         //Folder Icon Index is 3 or 4

                         fDefaultIcon = IconExtractor.ExtractIcon(Shell32File, 3);
                     }
                 }
                 catch (Exception ex)
                 {
                     Exception = ex;
                     //MessageBox.Show(ex.ToString());
                 }
                 if (isDefaultIcon)
                 {
                     if (_currentIconInfo != null) _currentIconInfo.Dispose();
                     if (fDefaultIcon is null)
                     {
                         _currentIconInfo = null;
                     }
                     else
                     {
                         _currentIconInfo = new IconInfo(fDefaultIcon);
                     }

                     return Tuple.Create(_currentIconInfo, IconFilePath, IconIndex, Exception);
                 }
                 if (FIInfo is null)
                 {
                     _currentIconInfo = null;
                     return Tuple.Create(_currentIconInfo, IconFilePath, IconIndex, Exception);
                 }

                 if (FIInfo.SourceIcon is null)
                 {
                     if (!(_currentIconInfo is null)) _currentIconInfo.Dispose();
                     _currentIconInfo = null;
                 }
                 else
                 {
                     _currentIconInfo = new IconInfo(FIInfo.SourceIcon);
                     IconFilePath = GetFileShortPathIfInFolder(TargetFolder, FIInfo.FilePath);
                     IconIndex = FIInfo.Index;
                 }
                 return Tuple.Create(_currentIconInfo, IconFilePath, IconIndex, Exception);
             });

            if (tRes.Item4 != null)
            {
                //TODO: Handle Exception
                MessageBox.Show(OwnerWindow, tRes.Item4.ToString());
            }
            else
            {
                this.CurrentIconInfo = tRes.Item1;
                this.CurrentIconPath = tRes.Item2;
                this.CurrentIconIndex = tRes.Item3;
            }

            //Gemt

            StatusMsg = null;
            IsLoadingCurrentInfo = false;
            CurrentRequireRefresh = false;
        }

        private void ResetCurrentInfo()
        {
            this.CurrentIconPath = null;
            this.CurrentIconIndex = null;
            this.CurrentIconInfo = null;
            this.CurrentRequireRefresh = false;
        }


        bool _GetDefaultFolderIcon = true;
        public bool GetDefaultFolderIcon
        {
            get { return _GetDefaultFolderIcon; }
            set
            {
                if (_GetDefaultFolderIcon != value)
                {
                    _GetDefaultFolderIcon = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private async Task CurrentGenBestFit()
        {
            if (IsWorking || CurrentIconInfo is null) return;
            var msgGenBestFit = GetLocalizedString("msgGenBestFit");
            if (MessageBox.Show(OwnerWindow, msgGenBestFit, GetLocalizedString("MainFormTitle"), MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes) return;
            await RefreshCurrentInfo();
            //IsWorking = true;
            StatusMsg = GetLocalizedString("Working");
            var taskRes = await Classes.TaskResult.RunAsync(() => IsWorking, () =>
            {
                var IconFilePath = Path.Combine(TargetFolder, "FolderIcon.ico");
                //
                var bestFitImage = CurrentIconInfo.GetBestFitIcon();
                var SourceImage = new Bitmap(bestFitImage.Image);
                using (var IconEd = new IconEditor(SourceImage, OneIconInfoArry))
                {
                    var res = IconEd.SaveTo(IconFilePath, SameFileNameDecisions.Rename);
                    IconFilePath = res.FilePath;
                }
                CurrentIconInfo.Dispose();
                SourceImage.Dispose();

                return IconFilePath;
            });
            //IsWorking = false;
            if (taskRes.OperationWasSuccessful)
            {
                await GetNewIconInfo(taskRes.Result, 0);
                StatusMsg = GetLocalizedString("Done");
                /*
                    BApply_Click(sender, e);
                 */
            }
            else if (taskRes.Exception != null)
            {
                //TODO: Handle Exception msg
                MessageBox.Show(OwnerWindow, taskRes.Exception.ToString());
            }
        }

        async Task ResetTargetFolderIcon()
        {
            if (IsWorking) return;
            if (Directory.Exists(TargetFolder))
            {
                if (MessageBox.Show(OwnerWindow, GetLocalizedString("ResetToDefaultIconMsg"), GetLocalizedString("MainFormTitle"), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes) return;
                IsWorking = true;
                var TaskRes = await TaskResult.RunAsync(() =>
                {
                    Ezz_Helper.Files.GetInfo.GetDirectoryInfo.RestFolderIcon(TargetFolder);
                });
                IsWorking = false;
                //
                if (TaskRes.OperationWasSuccessful)
                {
                    RefreshCurrentInfoCommand.Execute(null);
                }
                else if (TaskRes.Exception != null)
                {
                    MessageBox.Show(OwnerWindow, TaskRes.Exception.Message);
                }
            }
        }


        DelegateCommand _BrowseForTargetFolderCommand;
        public DelegateCommand BrowseForTargetFolderCommand
            => _BrowseForTargetFolderCommand ?? (_BrowseForTargetFolderCommand = new DelegateCommand(() =>
            {
                BrowseForTargetFolder();
            }));
        void BrowseForTargetFolder()
        {
            if (IsWorking) return;
            IsWorking = true;
            try
            {
                var dialog = new Win32.FolderBrowserDialog
                {
                    ShowEditBox = true,
                    BrowseShares = true,
                    RootType = Win32.RootType.SpecialFolder,
                    RootSpecialFolder = Environment.SpecialFolder.Desktop,
                    ShowStatusText = true,
                    BrowseFiles = false
                };
                if (Directory.Exists(TargetFolder)) dialog.SelectedPath = TargetFolder;
                if (dialog.ShowDialog() == true)
                {
                    IsWorking = false;
                    TargetFolder = dialog.SelectedPath;
                    RefreshCurrentInfoCommand.Execute(null);
                }
            }
            finally
            {
                IsWorking = false;
            }

            //var dialog = Microsoft.Win32.
        }

        #endregion CurrentIconInfo

        #region NewIconInfo

        //Commands

        DelegateCommand _ApplyNewIconCommand;
        public DelegateCommand ApplyNewIconCommand
            => _ApplyNewIconCommand ?? (_ApplyNewIconCommand = new DelegateCommand(async () =>
            {
                await ApplyNewIcon();
            }, (param) => !IsWorking));
        private DelegateCommand _RefreshNewInfoCommand;

        public DelegateCommand RefreshNewInfoCommand
            => _RefreshNewInfoCommand ?? (_RefreshNewInfoCommand = new DelegateCommand(async () =>
            {
                await RefreshNewInfoFromProps();
            }, (param) => !IsWorking));

        private DelegateCommand _IconFromImageCommand;

        public DelegateCommand IconFromImageCommand
            => _IconFromImageCommand ?? (_IconFromImageCommand = new DelegateCommand(async () =>
            {
                await this.IconFromImage();
            }, (param) => !IsWorking));

        private DelegateCommand _BrowseForIconCommand;

        public DelegateCommand BrowseForIconCommand
            => _BrowseForIconCommand ?? (_BrowseForIconCommand = new DelegateCommand(() =>
            {
                BrowseIcon();
            }, (param) => !IsWorking));

        private DelegateCommand _ClearNewIconPathCommand;

        public DelegateCommand ClearNewIconPathCommand
            => _ClearNewIconPathCommand ?? (_ClearNewIconPathCommand = new DelegateCommand(() =>
            {
                if (IsWorking) return;
                NewIconPath = null;
                ResetNewInfo();
            }, (param) => !IsWorking));

        //
        private IconInfo _NewIconInfo;

        public IconInfo NewIconInfo
        {
            get { return _NewIconInfo; }
            set
            {
                if (_NewIconInfo != value)
                {
                    _NewIconInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private bool _IsLoadingNewInfo;

        public bool IsLoadingNewInfo
        {
            get { return _IsLoadingNewInfo; }
            set
            {
                if (_IsLoadingNewInfo != value)
                {
                    _IsLoadingNewInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                IsWorking = value;
            }
        }

        private string _NewIconPath;

        public string NewIconPath
        {
            get { return _NewIconPath; }
            set
            {
                if (_NewIconPath != value)
                {
                    _NewIconPath = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    if (!IsWorking) NewRequireRefresh = true;
                }
            }
        }

        private bool _NewRequireRefresh;

        public bool NewRequireRefresh
        {
            get { return _NewRequireRefresh; }
            set
            {
                if (_NewRequireRefresh != value)
                {
                    _NewRequireRefresh = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    NewIconMaxIndex = null;
                }
            }
        }

        private int? _NewIconIndex;

        public int? NewIconIndex
        {
            get { return _NewIconIndex; }
            set
            {
                if (_NewIconIndex != value)
                {
                    _NewIconIndex = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    if (!IsWorking)
                    {
                        //NewRequireRefresh = true;
                        RefreshNewInfoCommand.Execute(null);
                    }
                }
            }
        }

        private int? _NewIconMaxIndex;

        public int? NewIconMaxIndex
        {
            get { return _NewIconMaxIndex; }
            set
            {
                if (_NewIconMaxIndex != value)
                {
                    _NewIconMaxIndex = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        private void ResetNewIconInfo()
        {
            if (this.NewIconInfo != null) this.NewIconInfo.Dispose();
            this.NewIconInfo = null;
        }

        private void ResetNewInfo(bool setIconIndexNPath = true)
        {
            if (setIconIndexNPath)
            {
                this.NewIconPath = null;
                this.NewIconIndex = null;
            }
            this.NewIconMaxIndex = null;
            NewRequireRefresh = false;
            ResetNewIconInfo();
        }

        private async Task RefreshNewInfoFromProps()
        {
            //if (!NewIconIndex.HasValue)
            //{
            //    ResetNewInfo();
            //    return;
            //}
            await GetNewIconInfo(NewIconPath, NewIconIndex.GetValueOrDefault(0));
        }

        //TODO: Create A Task Manager class to handle multiple task requests and allow to take the result of last one
        private string TaskGetNewIconFilePath;

        private int TaskGetNewIconIndex;

        public bool IsSameGetNewIconTask(string _TaskGetNewIconFilePath, int? _TaskGetNewIconIndex)
        {
            if (TaskGetNewIconIndex != _TaskGetNewIconIndex) return false;
            if (_TaskGetNewIconFilePath != TaskGetNewIconFilePath) return false;
            return true;
        }

        private void setNew_TaskGetNewIcon(string FilePath, int iconIndex)
        {
            lock (TaskGetNewIconFilePath)
            {
                TaskGetNewIconFilePath = FilePath;
                TaskGetNewIconIndex = iconIndex;
            }
        }

        private OneTaskHandler TaskManager_GetNewInfoByFilePath;

        private async Task GetNewIconInfo(string FilePath, int iconIndex)
        {
            if (IsGeneratingFromImage) return;
            //if (IsWorking) return;
            //setNew_TaskGetNewIcon(FilePath, iconIndex);
            if (TaskManager_GetNewInfoByFilePath is null) TaskManager_GetNewInfoByFilePath = new OneTaskHandler();
            TaskManager_GetNewInfoByFilePath.CancelRunningTasks();
            IsLoadingNewInfo = true;
            if (!File.Exists(FilePath))
            {
                ResetNewInfo(false);
                IsLoadingNewInfo = false;
                NewRequireRefresh = false;
                return;
            }
            var cancellationTokenSource = TaskManager_GetNewInfoByFilePath.PrepareNewTask();
            var res = await DirectSelectIconFromFileAsync(GetFileFullPathIfInFolder(TargetFolder, FilePath), iconIndex, cancellationTokenSource.Token);
            TaskManager_GetNewInfoByFilePath.AfterTask(cancellationTokenSource);
            if (res.OperationWasSuccessful)
            { //TODO: Handle Exception
                await GetNewIconInfo(res.Result, false);
            }
            else if (cancellationTokenSource.IsCancellationRequested && TaskManager_GetNewInfoByFilePath.ContainsTask())
            {
                return;
            }
            IsLoadingNewInfo = false;
            NewRequireRefresh = false;
        }

        private OneTaskHandler TaskManager_GetNewInfoBySelectedIconInfo = new OneTaskHandler();

        private async Task GetNewIconInfo(SelectedIconInfo SNewIConInfo, bool setIconIndexNPath = true)
        {
            if (IsGeneratingFromImage) return;
            //if (checkIsWorking && IsWorking) return;
            if (TaskManager_GetNewInfoBySelectedIconInfo is null) TaskManager_GetNewInfoBySelectedIconInfo = new OneTaskHandler();
            CancellationTokenSource cancellationTokenSource = null;

            TaskManager_GetNewInfoBySelectedIconInfo.CancelRunningTasks();
            cancellationTokenSource = TaskManager_GetNewInfoByFilePath.PrepareNewTask();

            IsLoadingNewInfo = true;
            //if (_setNew_TaskGetNewIcon)
            //{
            //    TaskManager_GetNewInfo.CancelRunningTasks();
            //    //setNew_TaskGetNewIcon(SNewIConInfo.FilePath, SNewIConInfo.Index);
            //}

            //bNewShowIconGroup.Enabled = false;
            ResetNewIconInfo();
            if (SNewIConInfo == null)
            {
                ResetNewInfo(setIconIndexNPath);
                IsLoadingNewInfo = false;
                StatusMsg = null;
                return;
            }
            //
            NewIconPath = SNewIConInfo.FilePath;
            int IIndex = SNewIConInfo.Index == 0 ? 0 : SNewIConInfo.FilePath.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ? 0 : SNewIConInfo.Index;

            //if (!IsSameGetNewIconTask(SNewIConInfo.FilePath, SNewIConInfo.Index)) return;
            if (SNewIConInfo.SourceIcon == null)
            {
                //ResetNewIconInfo();
                NewIconMaxIndex = null;
                if (setIconIndexNPath)
                    NewIconIndex = null;
            }
            else
            {
                var iconInfoTask = await Task.Run(() => new IconInfo(SNewIConInfo.SourceIcon)
                {
                    Index = IIndex
                }, cancellationTokenSource.Token);
                TaskManager_GetNewInfoBySelectedIconInfo.AfterTask(cancellationTokenSource);
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    //if (!IsSameGetNewIconTask(SNewIConInfo.FilePath, SNewIConInfo.Index)) return;
                    NewIconMaxIndex = SNewIConInfo.ICount == 0 ? 0 : SNewIConInfo.ICount - 1;
                    if (setIconIndexNPath)
                        NewIconIndex = IIndex;
                    this.NewIconInfo = iconInfoTask;
                }
                else //Canceled
                {
                    iconInfoTask?.Dispose();
                    //Let next task to reset props
                    if (TaskManager_GetNewInfoByFilePath.ContainsTask()) return;
                }
                //CheckNSetAllowToApply(); //TODO: CheckNSetAllowToApply
            }
            IsLoadingNewInfo = false;
            StatusMsg = null;
            NewRequireRefresh = false;
        }


        bool _IsGeneratingFromImage;
        public bool IsGeneratingFromImage
        {
            get { return _IsGeneratingFromImage; }
            set
            {
                if (_IsGeneratingFromImage != value)
                {
                    _IsGeneratingFromImage = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                this.IsWorking = value;
            }
        }

        private async Task IconFromImage()
        {
            if (IsWorking) return;
            var fd = new OpenFileDialog
            {
                Multiselect = false,
                Title = GetLocalizedString("SelectAnyImageToBeConverted")
            };
            FileDialogFilterBuilderHelper.BuildFilter(fd, new string[] { "*.jpg", "*.Jpeg", "*.png", "*.bmp" });
            if (Services.SettingsService.Instance.ShowCurrentFolderForIconFromImage && Directory.Exists(TargetFolder)) fd.InitialDirectory = TargetFolder;
            if (fd.ShowDialog() == true) await IconFromImage(fd.FileName);
        }
        private async Task IconFromImage(string SourceImageFile)
        {
            if (IsWorking) return;
            if (!File.Exists(SourceImageFile))
            {
                IsGeneratingFromImage = false;
                MessageBox.Show(OwnerWindow, GetLocalizedString("FileNotExists"));
                return;
            }
            IsGeneratingFromImage = true;

            var SaveDefDir = Directory.Exists(TargetFolder) ? TargetFolder : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var SaveFD = new SaveFileDialog();
            FileDialogFilterBuilderHelper.BuildFilter(SaveFD, new string[] { "*.ico" });
            SaveFD.AddExtension = true;
            SaveFD.DefaultExt = ".ico";
            SaveFD.InitialDirectory = SaveDefDir;
            SaveFD.Title = GetLocalizedString("SelectToSaveNewGeneratedIcon");
            SaveFD.FileName = GetLocalizedString("NewIcon") + ".ico";
            SaveFD.OverwritePrompt = true;
            if (SaveFD.ShowDialog() == true)
            {
                var tRes = await TaskResult.RunAsync(() =>
                {
                    var SourceImage = new Bitmap(SourceImageFile);
                    string iconFile = "";
                    using (var IconEd = new IconEditor(SourceImage, OneIconInfoArry))
                    {
                        var res = IconEd.SaveTo(SaveFD.FileName, SameFileNameDecisions.Overwrite);
                        iconFile = res.FilePath;
                    }
                    SourceImage.Dispose();
                    //
                    return iconFile;

                });
                IsGeneratingFromImage = false;
                if (tRes.OperationWasSuccessful)
                {
                    this.NewIconPath = tRes.Result;
                    this.NewIconIndex = 0;
                    RefreshNewInfoCommand.Execute(null);
                }
                else if (tRes.Exception != null)
                {
                    MessageBox.Show(OwnerWindow, tRes.Exception.Message);
                }
            }
            IsGeneratingFromImage = false;
        }

        private async Task ApplyNewIcon()
        {
            if (IsWorking || !Directory.Exists(TargetFolder)) return;
            IsWorking = true;
            //
            var TaskRes = await TaskResult.RunAsync(() =>
            {
                var iconPath = Services.SettingsService.Instance.CopyIconToFolder ? NewIconPath : GetFileShortPathIfInFolder(TargetFolder, NewIconPath);
                Ezz_Helper.Files.GetInfo.GetDirectoryInfo.SetFolderCustomIcon(TargetFolder, iconPath, NewIconIndex.GetValueOrDefault(0),
                CopyTheIconToTargetFolder: Services.SettingsService.Instance.CopyIconToFolder,
                HideFileIfitInFolder: Services.SettingsService.Instance.HideIcon);
            });
            IsWorking = false;
            if (TaskRes.OperationWasSuccessful)
            {
                RefreshCurrentInfoCommand.Execute(null);
            }
            else if (TaskRes.Exception != null)
            {
                MessageBox.Show(OwnerWindow, TaskRes.Exception.Message);
            }
        }


        private void BrowseIcon()
        {
            if (IsWorking) return;
            BrowseIcon(string.IsNullOrEmpty(NewIconPath) ? TargetFolder : Path.GetDirectoryName(NewIconPath), NewIconIndex.GetValueOrDefault(0));
        }

        private void BrowseIcon(string DefTarget, int defindex = 0)
        {
            if (IsWorking) return;
            //var SICon = Select_Icon.ShowD(null, DefTarget, defindex, string.IsNullOrEmpty(DefTarget) ? true : string.IsNullOrEmpty(NewIconPath));
            //if (SICon.DialogResult == DialogResult.OK)
            //{
            //    GetNewIconInfo(SICon.GetFirstItem());
            //}
            //

            var fd = new OpenFileDialog
            {
                Multiselect = false,
                Title = GetLocalizedString("Select_Icon_Title")
            };
            FileDialogFilterBuilderHelper.BuildFilter(fd, new string[] { "*.ico" });
            if (!Directory.Exists(DefTarget))
            {
                if (Directory.Exists(TargetFolder))
                {
                    DefTarget = TargetFolder;
                }
                else//desktop
                {
                    DefTarget = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                }
            }
            fd.InitialDirectory = DefTarget;
            //
            if (fd.ShowDialog() == true)
            {
                this.NewIconIndex = 0;
                this.NewIconPath = fd.FileName;
                this.RefreshNewInfoCommand.Execute(null);
            }

        }

        #endregion NewIconInfo

        #region Helper Methods

        private string GetFileShortPathIfInFolder(string Folder_, string iconFP)
        {
            try
            {
                if (!Path.IsPathRooted(iconFP))
                    return iconFP.Trim();

                if (iconFP.Contains(Folder_, StringComparison.OrdinalIgnoreCase))
                {
                    if (Path.GetDirectoryName(iconFP).Equals(Folder_, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return Path.GetFileName(iconFP);
                    }
                    return iconFP.Replace(Folder_, "").Trim('\\');
                }
                //else
                //{
                //    if (Path.GetDirectoryName(iconFP).Equals(Folder_, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        return Path.GetFileName(iconFP);
                //    }
                //}
                return iconFP.Trim();
            }
            catch (Exception)
            {
                return iconFP.Trim();
            }
        }

        private string GetFileFullPathIfInFolder(string Folder_, string iconFP)
        {
            if (string.IsNullOrEmpty(iconFP))
                return "";
            string FPath = iconFP;
            //.Trim
            try
            {
                if (Path.IsPathRooted(FPath))
                    return FPath;
                FPath = Path.Combine(Folder_, iconFP);
                if (!File.Exists(FPath))
                    return iconFP.Trim();
            }
            catch (Exception)
            {
                return iconFP.Trim();
            }
            return FPath.Trim();
        }

        #endregion Helper Methods


    }
}