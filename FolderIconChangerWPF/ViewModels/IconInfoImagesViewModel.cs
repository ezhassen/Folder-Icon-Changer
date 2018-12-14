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
using System.Windows.Input;
using static FolderIconChangerWPF.LocalizationProvider;

namespace FolderIconChangerWPF.ViewModels
{
    public class IconInfoImagesViewModelDesgin : IconInfoImagesViewModel
    {
        private IconInfoImagesViewModelDesgin _Instance;

        public IconInfoImagesViewModelDesgin Instance
        {
            get
            {
                if (_Instance is null) _Instance = new IconInfoImagesViewModelDesgin();
                return Instance;
            }
            set { Instance = value; }
        }

        public IconInfoImagesViewModelDesgin()
        {
            //icon.ico
            IconInfo = new IconInfo(Properties.Resources.icon);
            //GetImagesCommand.Execute(null);
            //GetImagesAsync().RunSynchronously();
            IconImageInfoCollection = IconInfo.ImageList;
        }
    }
    public class IconInfoImagesViewModel : BaseViewModel
    {
        public IconInfoImagesViewModel()
        {

        }
        public IconInfoImagesViewModel(IconInfo _IconInfo, string _FilePath)
        {
            IconInfo = _IconInfo;
            FilePath = _FilePath;
        }
        IconInfo _IconInfo;
        public IconInfo IconInfo
        {
            get { return _IconInfo; }
            set
            {
                if (_IconInfo != value)
                {
                    _IconInfo = value;
                    OnPropertyChanged(); //uses CallerMemberName
                                         //GetImagesCommand.Execute(null);
                }
            }
        }


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
                }
            }
        }

        DelegateCommand _GetImagesCommand;
        public DelegateCommand GetImagesCommand
            => _GetImagesCommand ?? (_GetImagesCommand = new DelegateCommand(async () =>
            {
                await this.GetImagesAsync();
            }));


        IconInfo.IconImageInfo _SelectedImage;
        public IconInfo.IconImageInfo SelectedImage
        {
            get { return _SelectedImage; }
            set
            {
                if (_SelectedImage != value)
                {
                    _SelectedImage = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        ICollection<IconInfo.IconImageInfo> _IconImageInfoCollection;
        public ICollection<IconInfo.IconImageInfo> IconImageInfoCollection
        {
            get { return _IconImageInfoCollection; }
            set
            {
                if (_IconImageInfoCollection != value)
                {
                    _IconImageInfoCollection = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
            }
        }

        public async Task GetImagesAsync()
        {
            IsWorking = true;
            IconImageInfoCollection = null;
            if (IconInfo is null)
            {
                IsWorking = false;
                return;
            }
            //
            //ImagesSources = null;
            var taskResult = await TaskResult.RunAsync(() =>
            {
                //return string.Format("{0}, {1}, {2}", iii.Size.ToString_1(), iii.ColorsType.ToString(), iii.IconImageFormat.ToString());

                return IconInfo.ImageList;
            });
            //

            if (taskResult.OperationWasSuccessful)
            {
                IconImageInfoCollection = taskResult.Result;
            }
            else if (taskResult.Exception != null)
            {
                MessageBox.Show(taskResult.Exception.ToString());
            }

            IsWorking = false;
        }

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


        System.Drawing.Size _CurrentImageViewSize = Ezz_Helper.Drawing.IconsManager.Sizes.px_256x256;
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

        int _MinSize = 64;
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


        DelegateCommand _ExportImageCommand;
        public DelegateCommand ExportImageCommand
            => _ExportImageCommand ?? (_ExportImageCommand = new DelegateCommand(() =>
            {
                //if (!(param is IconInfo.IconImageInfo iimageInfo)) return;
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    var iimageInfo = SelectedImage;
                    if (iimageInfo == null) return;
                    var saveFileDialog = new SaveFileDialog();
                    if (string.IsNullOrEmpty(FilePath))
                    {
                        saveFileDialog.FileName = "FileName";
                    }
                    else
                    {
                        var _FileNameWExt = Path.GetFileNameWithoutExtension(FilePath);
                        if (FilePath.EndsWith("ico", StringComparison.OrdinalIgnoreCase))
                        {
                            saveFileDialog.FileName = $"{(string.IsNullOrEmpty(_FileNameWExt) ? "" : (_FileNameWExt + " - "))}Image #{iimageInfo.Index}";
                        }
                        else
                        {
                            saveFileDialog.FileName = $"{(string.IsNullOrEmpty(_FileNameWExt) ? "" : (_FileNameWExt + " - "))}Icon # {IconInfo.Index} Image #{iimageInfo.Index}";
                        }
                    }
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.BuildFilter(new string[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" }, true, false, AllFormatsString_1: GetLocalizedString("Select_Icon_AllSupportedFormats"));
                    saveFileDialog.DefaultExt = ".png";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (FileStream FS = File.Create(saveFileDialog.FileName))
                        {
                            var _ext = Path.GetExtension(saveFileDialog.FileName).ToLower();
                            if (_ext.EndsWith("png"))
                            {
                                iimageInfo.Image?.Save(FS, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else if (_ext.EndsWith("jpg") || _ext.EndsWith("jpeg"))
                            {
                                iimageInfo.Image?.Save(FS, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else if (_ext.EndsWith("bmp"))
                            {
                                iimageInfo.Image?.Save(FS, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //throw;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }

            }));
    }
}
