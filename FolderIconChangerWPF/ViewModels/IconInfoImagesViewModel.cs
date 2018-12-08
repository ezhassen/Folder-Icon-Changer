using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        public IconInfoImagesViewModel(IconInfo _IconInfo)
        {
            IconInfo = _IconInfo;
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


        DelegateCommand _GetImagesCommand;
        public DelegateCommand GetImagesCommand
            => _GetImagesCommand ?? (_GetImagesCommand = new DelegateCommand(async () =>
            {
                await this.GetImagesAsync();
            }));

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
                if (_ImageViewSizes is null) _ImageViewSizes = Ezz_Helper.Drawing.IconsManager.Sizes.GetAll().Where(s=>s.Width>64);
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
    }
}
