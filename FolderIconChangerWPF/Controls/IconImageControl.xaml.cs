using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.IconInfoCore;
using FolderIconChangerWPF.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FolderIconChangerWPF.Controls
{
    /// <summary>
    /// Interaction logic for IconImageControl.xaml
    /// </summary>
    public partial class IconImageControl : IconImageControlBase
    {
        public IconImageControl()
        {
            InitializeComponent();
        }

        private void IconImageControlBase_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsLoading || IsNullImage || ImageSource is null) return;
            PART_CommandsPanel.Visibility = Visibility.Visible;
        }

        private void IconImageControlBase_MouseLeave(object sender, MouseEventArgs e)
        {
            PART_CommandsPanel.Visibility = Visibility.Hidden;
        }
    }
    public class IconImageControlBase : UserControl
    {
        public IconImageControlBase()
        {
            //TODO: Create ViewIconGroupCommand
            ViewIconGroupCommand = new DelegateCommand(() =>
            {
                var newWindow = new Windows.IconInfoImagesWindow(this.IconInfo, this.FilePath)
                {
                    Owner = Application.Current.MainWindow
                };
                newWindow.ShowDialog();
            });
        }
        #region Props


        public IconInfo IconInfo
        {
            get { return (IconInfo)GetValue(IconInfoProperty); }
            set { SetValue(IconInfoProperty, value); }
        }


        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(IconImageControlBase), new PropertyMetadata(null));




        // Using a DependencyProperty as the backing store for IconInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconInfoProperty =
            DependencyProperty.Register("IconInfo", typeof(IconInfo), typeof(IconImageControlBase), new PropertyMetadata(null, new PropertyChangedCallback(OnIconInfoPropertyChanged)));



        public static int GetIconSizeW(DependencyObject obj) => (int)obj.GetValue(IconSizeWProperty);

        public static void SetIconSizeW(DependencyObject obj, int value) => obj.SetValue(IconSizeWProperty, value);

        // Using a DependencyProperty as the backing store for IconSizeW.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeWProperty =
            DependencyProperty.RegisterAttached("IconSizeW", typeof(int), typeof(IconImageControlBase), new PropertyMetadata(256));

        private static void OnIconInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as IconImageControlBase)?.OnIconInfoChanged(e);
        private async void OnIconInfoChanged(DependencyPropertyChangedEventArgs e)
        {
            var newIconInfo = e.NewValue as IconInfo;
            IsLoading = true;
            //ImageSource = null;
            try
            {
                if (newIconInfo is null)
                {
                    ImageSource = null;
                }
                else
                {
                    var iconSizeW = GetIconSizeW(this);
                    //ImageSource = null;
                    SetValue(ImageSourceProperty, null);

                    ImageSource = await Task.Run(() =>
                         {
                             try
                             {
                                 return newIconInfo.GetBestFitIcon(new System.Drawing.Size(iconSizeW, iconSizeW))?.BuildBitmapImage();
                             }
                             catch (Exception)
                             {
                                 //throw;
                                 return null;
                             }

                         });
                }

            }
            catch (Exception)
            {
                //throw;
                return;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set
            {
                if (value is null)
                {
                    IsNullImage = true;
                    SetValue(ImageSourceProperty, ShowNullImage ? NullImage : null);
                }
                else
                {
                    IsNullImage = false;
                    SetValue(ImageSourceProperty, value);
                }
            }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(IconImageControlBase), new PropertyMetadata(null));


        public static Stretch GetImageStretch(DependencyObject obj)
        {
            return (Stretch)obj.GetValue(ImageStretchProperty);
        }

        public static void SetImageStretch(DependencyObject obj, Stretch value)
        {
            obj.SetValue(ImageStretchProperty, value);
        }

        // Using a DependencyProperty as the backing store for ImageStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStretchProperty =
            DependencyProperty.RegisterAttached("ImageStretch", typeof(Stretch), typeof(IconImageControlBase), new PropertyMetadata(Stretch.Uniform));

        public static StretchDirection GetImageStretchDirection(DependencyObject obj)
        {
            return (StretchDirection)obj.GetValue(ImageStretchDirectionProperty);
        }

        public static void SetImageStretchDirection(DependencyObject obj, StretchDirection value)
        {
            obj.SetValue(ImageStretchDirectionProperty, value);
        }

        // Using a DependencyProperty as the backing store for ImageStretchDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStretchDirectionProperty =
            DependencyProperty.RegisterAttached("ImageStretchDirection", typeof(StretchDirection), typeof(IconImageControlBase), new PropertyMetadata(StretchDirection.DownOnly));



        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(IconImageControlBase), new PropertyMetadata(false, new PropertyChangedCallback(OnIsLoadingPropertyChanged)));

        private static void OnIsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as IconImageControlBase)?.OnIsLoadingChanged(e);
        Cursor controlCursor;
        private void OnIsLoadingChanged(DependencyPropertyChangedEventArgs e)
        {
            var val = (bool)e.NewValue;
            if (val)
            {
                controlCursor = this.Cursor;
                this.Cursor = Cursors.Wait;
            }
            else
            {
                //if (this.Cursor == Cursors.Wait) return;
                this.Cursor = controlCursor ?? Cursors.Arrow;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether IsNullImage
        /// </summary>
        public bool IsNullImage
        {
            get { return (bool)GetValue(IsNullImageProperty); }
            protected set { SetValue(IsNullImageProperty, value); }
        }
        public static readonly DependencyProperty IsNullImageProperty =
            DependencyProperty.Register("IsNullImage", typeof(bool), typeof(IconImageControlBase), new PropertyMetadata(true));

        public ImageSource NullImage
        {
            get { return (ImageSource)GetValue(NullImageProperty); }
            set { SetValue(NullImageProperty, value); }
        }
        public static readonly DependencyProperty NullImageProperty =
            DependencyProperty.Register("NullImage", typeof(ImageSource), typeof(IconImageControlBase), new PropertyMetadata(null, new PropertyChangedCallback(OnNullImagePropertyChanged)));
        private static void OnNullImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconImageControlBase)?.OnNullImageChanged();
        }
        internal void OnNullImageChanged()
        {
            if (IsNullImage) this.ImageSource = null;
        }

        public bool ShowNullImage
        {
            get { return (bool)GetValue(ShowNullImageProperty); }
            set { SetValue(ShowNullImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowNullImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowNullImageProperty =
            DependencyProperty.Register("ShowNullImage", typeof(bool), typeof(IconImageControlBase), new PropertyMetadata(true, new PropertyChangedCallback(OnShowNullImagePropertyChanged)));

        private static void OnShowNullImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as IconImageControlBase)?.OnShowNullImageChanged(e);
        private void OnShowNullImageChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsNullImage) this.ImageSource = null;
        }

        public ICommand ViewIconGroupCommand
        {
            get { return (ICommand)GetValue(ViewIconGroupCommandProperty); }
            set { SetValue(ViewIconGroupCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewIconGroupCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewIconGroupCommandProperty =
            DependencyProperty.Register("ViewIconGroupCommand", typeof(ICommand), typeof(IconImageControlBase), new PropertyMetadata(null));


        public ICommand GenerateBestFitCommand
        {
            get { return (ICommand)GetValue(GenerateBestFitCommandProperty); }
            set { SetValue(GenerateBestFitCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GenerateBestFitCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GenerateBestFitCommandProperty =
            DependencyProperty.Register("GenerateBestFitCommand", typeof(ICommand), typeof(IconImageControlBase), new PropertyMetadata(null));

        public bool AllowToGenerateBestFit
        {
            get { return (bool)GetValue(AllowToGenerateBestFitProperty); }
            set { SetValue(AllowToGenerateBestFitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllowToGenerateBestFit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowToGenerateBestFitProperty =
            DependencyProperty.Register("AllowToGenerateBestFit", typeof(bool), typeof(IconImageControlBase), new PropertyMetadata(true));



        #endregion
    }
}
