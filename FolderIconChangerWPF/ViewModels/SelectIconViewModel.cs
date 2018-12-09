using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.Classes;
using FolderIconChangerWPF.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            => _LoadIconsCommand ?? (_LoadIconsCommand = new DelegateCommand(async () =>
            {
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
                return IconExtractor.ExtractAllIcons_AsIconInfo(fPath, BuildIcons: true);
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
                    Services.SettingsService.Instance.AddRecentFile(this.FilePath);
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
            FileDialogFilterBuilderHelper.BuildFilter(fd, new string[] { "*.ico", "*.dll", "*.exe" });
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
            //
            if (fd.ShowDialog() == true)
            {
                //this.NewIconIndex = 0;
                this.FilePath = fd.FileName;
                this.LoadIconsCommand.Execute(null);
            }
        }

        System.Drawing.Size _CurrentImageViewSize = Ezz_Helper.Drawing.IconsManager.Sizes.px_64x64;
        public System.Drawing.Size CurrentImageViewSize
        {
            get
            {
                return _CurrentImageViewSize;
            }
            set
            {
                if (_CurrentImageViewSize != value)
                {
                    _CurrentImageViewSize = value;
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
            => _ChangeImageViewSizeCommand ?? (_ChangeImageViewSizeCommand = new DelegateCommand((Delta) =>
            {
                if (Delta is int deltaInt) ChangeImageViewSize(deltaInt);
            }));

        private IEnumerable<System.Drawing.Size> _ImageViewSizes;

        public IEnumerable<System.Drawing.Size> ImageViewSizes
        {
            get
            {
                if (_ImageViewSizes is null) _ImageViewSizes = Ezz_Helper.Drawing.IconsManager.Sizes.GetAll().Where(s => s.Width > MinSize);
                return _ImageViewSizes;
            }
            set { _ImageViewSizes = value; }
        }
        void ChangeImageViewSize(int Delta)
        {
            //Up
            if (Delta > 0)
            {
                var findNext_ = ImageViewSizes.FirstOrDefault(s => s.Width > this.CurrentImageViewSize.Width || s.Height > this.CurrentImageViewSize.Height);
                if (findNext_ != System.Drawing.Size.Empty) CurrentImageViewSize = findNext_;
            }
            else //Down
            {
                var findPre_ = ImageViewSizes.LastOrDefault(s => s.Width < this.CurrentImageViewSize.Width || s.Height < this.CurrentImageViewSize.Height);
                if (findPre_ != System.Drawing.Size.Empty) CurrentImageViewSize = findPre_;
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
    }
}
