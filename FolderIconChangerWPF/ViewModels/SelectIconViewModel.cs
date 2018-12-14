using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.Classes;
using FolderIconChangerWPF.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static FolderIconChangerWPF.LocalizationProvider;

namespace FolderIconChangerWPF.ViewModels
{
    public class SelectIconViewModel : BaseViewModel
    {
        //static SelectIconViewModel _Instance;

        //public static SelectIconViewModel Instance
        //{
        //    get
        //    {
        //        if (_Instance is null) _Instance = new SelectIconViewModel();
        //        return _Instance;
        //    }
        //    //set { _Instance = value; }
        //}
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

        public string TargetFolder { get; set; }

        string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (_FilePath != value)
                {
                    _FilePath = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    RequireRefresh = true;
                }
            }
        }

        DelegateCommand _ClearCommand;
        public DelegateCommand ClearCommand
            => _ClearCommand ?? (_ClearCommand = new DelegateCommand(() =>
            {
                if (IsWorking) return;
                FilePath = null;
                this.LoadIconsCommand.Execute(null);
            }, (param) => !IsWorking));

        bool _RequireRefresh;
        public bool RequireRefresh
        {
            get { return _RequireRefresh; }
            set
            {
                if (_RequireRefresh != value)
                {
                    _RequireRefresh = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        int? _SelectedIndex;
        public int? SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }
        bool isOK;
        IconInfo _SelectedIconInfo;
        public IconInfo SelectedIconInfo
        {
            get { return _SelectedIconInfo; }
            set
            {
                if (_SelectedIconInfo != value)
                {
                    _SelectedIconInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                if (!isOK) SelectedIndex = value?.Index;
            }
        }


        bool _IsLoadingIcons;
        public bool IsLoadingIcons
        {
            get { return _IsLoadingIcons; }
            set
            {
                if (_IsLoadingIcons != value)
                {
                    _IsLoadingIcons = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                IsWorking = value;
            }
        }

        ICollection<IconInfo> _Icons;
        public ICollection<IconInfo> Icons
        {
            get { return _Icons; }
            set
            {
                if (_Icons != value)
                {
                    _Icons = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        DelegateCommand _LoadIconsCommand;
        public DelegateCommand LoadIconsCommand
            => _LoadIconsCommand ?? (_LoadIconsCommand = new DelegateCommand(async (file) =>
            {
                var fileStr = file as string;
                if (!string.IsNullOrEmpty(fileStr)) _FilePath = fileStr;
                await LoadIcons();
            }));

        OneTaskHandler LoadIconsOneTaskHandler;

        async Task LoadIcons()
        {
            if (LoadIconsOneTaskHandler is null) LoadIconsOneTaskHandler = new OneTaskHandler();

            void ResetPropsMethod()
            {
                //Reset props code here like
                RequireRefresh = false;
                IsLoadingIcons = false;
            }

            //Add Waiting props Here Like
            IsLoadingIcons = true;

            LoadIconsOneTaskHandler.CancelRunningTasks();

            DisposeAllIcons();
            //Add your code to check to run a new operation. Like
            if (!File.Exists(FilePath))
            {
                ResetPropsMethod();
                return;
            }

            //A CancellationTokenSource for the new Task
            var cancellationTokenSource = LoadIconsOneTaskHandler.PrepareNewTask();

            //Await for the task
            var fPath = this.FilePath;
            var taskResult = await TaskResult.RunAsync((cancel) =>
            {
                return IconExtractor.ExtractAllIcons_AsIconInfo(fPath, BuildIcons: false);
            }, cancellationTokenSource.Token);

            // Remove cancellationTokenSource from running tasks
            LoadIconsOneTaskHandler.AfterTask(cancellationTokenSource);

            //If the task is Canceled by a newer task or other things
            if (cancellationTokenSource.IsCancellationRequested)
            {
                //Cleanup TaskResult
                //If the operation is Canceled and there is an other task running Let next task to reset props
                if (LoadIconsOneTaskHandler.ContainsTask()) return;
            }
            else
            {
                //Set Result Code from TaskResult
                //Icons
                if (taskResult.OperationWasSuccessful)
                {
                    Icons = taskResult.Result;
                    if (Icons != null && Icons.Count != 0) Services.SettingsService.Instance.AddRecentFile(this.FilePath);
                }
                else if (taskResult.Exception != null)
                {
                    MessageBox.Show(taskResult.Exception.Message);
                }
            }
            //Reset props code here
            ResetPropsMethod();
        }


        DelegateCommand _DisposeAllIconsCommand;
        public DelegateCommand DisposeAllIconsCommand
            => _DisposeAllIconsCommand ?? (_DisposeAllIconsCommand = new DelegateCommand(() =>
            {
                DisposeAllIcons();
            }));
        void DisposeAllIcons()
        {
            if (Icons is null) return;
            foreach (var icon in Icons)
            {
                icon.Dispose();
            }
            Icons = null;
        }

        DelegateCommand _BrowseForFileCommand;
        public DelegateCommand BrowseForFileCommand
            => _BrowseForFileCommand ?? (_BrowseForFileCommand = new DelegateCommand(() =>
            {
                BrowseForFile();
            }));
        void BrowseForFile()
        {
            if (IsWorking) return;
            //BrowseIcon(string.IsNullOrEmpty(NewIconPath) ? TargetFolder : Path.GetDirectoryName(NewIconPath), NewIconIndex.GetValueOrDefault(0));
            var fd = new OpenFileDialog
            {
                Multiselect = false,
                Title = GetLocalizedString("Select_Icon_Title")
            };
            FileDialogFilterBuilderHelper.BuildFilter(fd, new string[] { "*.ico", "*.dll", "*.exe" }, AllFormatsString_1: GetLocalizedString("Select_Icon_AllSupportedFormats"));
            var DefTarget = string.IsNullOrEmpty(FilePath) ? TargetFolder : Path.GetDirectoryName(FilePath);
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
            if (!string.IsNullOrEmpty(FilePath)) fd.FileName = Path.GetFileName(FilePath);
            //
            if (fd.ShowDialog() == true)
            {
                //this.NewIconIndex = 0;
                this.FilePath = fd.FileName;
                this.LoadIconsCommand.Execute(null);
            }
        }

        int _CurrentImageViewSizeW = 64;
        public int CurrentImageViewSizeW
        {
            get
            {
                return _CurrentImageViewSizeW;
            }
            set
            {
                if (_CurrentImageViewSizeW != value)
                {
                    _CurrentImageViewSizeW = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        int _MinSize = 16;
        public int MinSize
        {
            get { return _MinSize; }
            set
            {
                if (_MinSize != value)
                {
                    _MinSize = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }


        DelegateCommand _ChangeImageViewSizeCommand;
        public DelegateCommand ChangeImageViewSizeCommand
            => _ChangeImageViewSizeCommand ?? (_ChangeImageViewSizeCommand = new DelegateCommand((newSizeW) =>
            {
                if (newSizeW is string newSizeWStr)
                {
                    if (newSizeWStr.IsNumeric())
                    {
                        newSizeW = newSizeWStr.ValInt();
                    }
                }

                if (newSizeW is int newSizeWInt) CurrentImageViewSizeW = newSizeWInt;
            }));

        DelegateCommand _ChangeImageViewSizeDeltaCommand;
        public DelegateCommand ChangeImageViewSizeDeltaCommand
            => _ChangeImageViewSizeDeltaCommand ?? (_ChangeImageViewSizeDeltaCommand = new DelegateCommand((Delta) =>
            {
                if (Delta is string DeltaStr)
                {
                    if (DeltaStr.IsNumeric())
                    {
                        Delta = DeltaStr.ValInt();
                    }
                }
                if (Delta is int deltaInt) ChangeImageViewSizeDelta(deltaInt);
            }));

        private IEnumerable<System.Drawing.Size> _ImageViewSizes;

        public IEnumerable<System.Drawing.Size> ImageViewSizes
        {
            get
            {
                if (_ImageViewSizes is null) _ImageViewSizes = Sizes.GetAll().Where(s => s.Width > MinSize);
                return _ImageViewSizes;
            }
            set { _ImageViewSizes = value; }
        }
        void ChangeImageViewSizeDelta(int Delta)
        {
            //Up
            if (Delta > 0)
            {
                var findNext_ = ImageViewSizes.FirstOrDefault(s => s.Width > this.CurrentImageViewSizeW || s.Height > this.CurrentImageViewSizeW);
                if (findNext_ != System.Drawing.Size.Empty) CurrentImageViewSizeW = findNext_.Width;
            }
            else //Down
            {
                var findPre_ = ImageViewSizes.LastOrDefault(s => s.Width < this.CurrentImageViewSizeW || s.Height < this.CurrentImageViewSizeW);
                if (findPre_ != System.Drawing.Size.Empty) CurrentImageViewSizeW = findPre_.Width;
            }
        }


        DelegateCommand _OKCommand;
        public DelegateCommand OKCommand
            => _OKCommand ?? (_OKCommand = new DelegateCommand(() =>
            {
                if (!this.SelectedIndex.HasValue) return;
                isOK = true;
                //
                LoadIconsOneTaskHandler?.CancelRunningTasks();
                DisposeAllIcons();
                if (!(OwnerWindow is null)) OwnerWindow.DialogResult = true;
                CloseCommand?.Execute(null);
            }));


        bool _StretchedImages = true;
        public bool StretchedImages
        {
            get { return _StretchedImages; }
            set
            {
                if (_StretchedImages != value)
                {
                    _StretchedImages = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    OnPropertyChanged(nameof(ImageStretchDirection));
                    OnPropertyChanged(nameof(ImageStretch));
                }
            }
        }


        bool _StretchSmallImagesToo;
        public bool StretchSmallImagesToo
        {
            get { return _StretchSmallImagesToo; }
            set
            {
                if (_StretchSmallImagesToo != value)
                {
                    _StretchSmallImagesToo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                    OnPropertyChanged(nameof(ImageStretchDirection));
                    OnPropertyChanged(nameof(ImageStretch));
                }
            }
        }



        public StretchDirection ImageStretchDirection
        {
            get
            {
                if (StretchSmallImagesToo) return StretchDirection.Both;
                return StretchDirection.DownOnly;
            }
            set
            {
                if (value == StretchDirection.DownOnly)
                {
                    StretchSmallImagesToo = false;
                }
                else
                {
                    StretchSmallImagesToo = true;
                }
                //OnPropertyChanged(); //uses CallerMemberName
            }
        }


        public Stretch ImageStretch
        {
            get
            {
                if (StretchedImages) return Stretch.Uniform;
                return Stretch.None;
            }
            set
            {
                if (value == Stretch.Uniform)
                {
                    StretchedImages = true;
                }
                else
                {
                    StretchedImages = false;
                }
                //OnPropertyChanged(); //uses CallerMemberName
            }
        }


        DelegateCommand _ViewIconGroupCommandCommand;
        public DelegateCommand ViewIconGroupCommandCommand
            => _ViewIconGroupCommandCommand ?? (_ViewIconGroupCommandCommand = new DelegateCommand(() =>
            {
                if (this.SelectedIconInfo is null) return;
                var newWindow = new Windows.IconInfoImagesWindow(this.SelectedIconInfo, this.FilePath)
                {
                    Owner = this.OwnerWindow
                };
                //newWindow.Show();
                newWindow.ShowDialog();
            }));


        DelegateCommand _ExportSelectedIconCommand;
        public DelegateCommand ExportSelectedIconCommand
            => _ExportSelectedIconCommand ?? (_ExportSelectedIconCommand = new DelegateCommand(async () =>
            {
                if (SelectedIconInfo is null || !SelectedIndex.HasValue) return;
                IsWorking = true;
                var saveFileDialog = new SaveFileDialog();
                var _FileNameWExt = Path.GetFileNameWithoutExtension(FilePath);
                saveFileDialog.FileName = (string.IsNullOrEmpty(_FileNameWExt) ? "" : _FileNameWExt + " - ") + "Icon #" + SelectedIndex.Value;
                saveFileDialog.BuildFilter(new string[] { "*.ico" }, true, false, AllFormatsString_1: GetLocalizedString("Select_Icon_AllSupportedFormats"));
                saveFileDialog.DefaultExt = ".ico";
                saveFileDialog.OverwritePrompt = true;
                var ssIconI = SelectedIconInfo;
                if (saveFileDialog.ShowDialog(OwnerWindow) == true)
                {
                    var sFile = saveFileDialog.FileName;
                    var TRes = await TaskResult.RunAsync(() =>
                    {
                        using (FileStream FS = File.Create(sFile))
                        {
                            ssIconI.SourceIcon.Save(FS);
                        }
                    });
                    IsWorking = false;
                    if (TRes.Exception != null)
                    {
                        MessageBox.Show(OwnerWindow, TRes.Exception.ToString());
                    }
                }
                IsWorking = false;
            }));
    }
}
