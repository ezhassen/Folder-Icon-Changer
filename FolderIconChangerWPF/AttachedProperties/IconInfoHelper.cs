using Ezz_Helper.Drawing.IconsManager;
using FolderIconChangerWPF.Classes;
using FolderIconChangerWPF.IconInfoCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class IconInfoHelper
    {

        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);

        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(IconInfoHelper), new PropertyMetadata(null));

        public static bool GetIsLoading(DependencyObject obj) => (bool)obj.GetValue(IsLoadingProperty);

        public static void SetIsLoading(DependencyObject obj, bool value) => obj.SetValue(IsLoadingProperty, value);

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.RegisterAttached("IsLoading", typeof(bool), typeof(IconInfoHelper), new PropertyMetadata(false));


        public static int? GetIconSizeW(DependencyObject obj) => (int?)obj.GetValue(IconSizeWProperty);

        public static void SetIconSizeW(DependencyObject obj, int? value) => obj.SetValue(IconSizeWProperty, value);

        // Using a DependencyProperty as the backing store for IconSizeW.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeWProperty =
            DependencyProperty.RegisterAttached("IconSizeW", typeof(int?), typeof(IconInfoHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnIconSizeWPropertyChanged)));

        private static void OnIconSizeWPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => LoadImageFromIconInfoAsync(d);

        public static IconInfo GetIconInfo(DependencyObject obj) => (IconInfo)obj.GetValue(IconInfoProperty);

        public static void SetIconInfo(DependencyObject obj, IconInfo value) => obj.SetValue(IconInfoProperty, value);

        // Using a DependencyProperty as the backing store for IconInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconInfoProperty =
            DependencyProperty.RegisterAttached("IconInfo", typeof(IconInfo), typeof(IconInfoHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnIconInfoPropertyChangedAsync)));

        private static async void OnIconInfoPropertyChangedAsync(DependencyObject d, DependencyPropertyChangedEventArgs e) => await LoadImageFromIconInfo(d, e.NewValue as IconInfo);

        static async void LoadImageFromIconInfoAsync(DependencyObject d) => await LoadImageFromIconInfo(d, GetIconInfo(d));
        //static void LoadImageFromIconInfo(DependencyObject d, IconInfo iconInfo)
        //{
        //    if (iconInfo is null) return;
        //    //TODO: Load Task

        //}

        static OneTaskHandler GetOneTaskHandler(DependencyObject obj) => (OneTaskHandler)obj.GetValue(OneTaskHandlerProperty);

        static void SetOneTaskHandler(DependencyObject obj, OneTaskHandler value) => obj.SetValue(OneTaskHandlerProperty, value);

        // Using a DependencyProperty as the backing store for OneTaskHandler.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty OneTaskHandlerProperty =
            DependencyProperty.RegisterAttached("OneTaskHandler", typeof(OneTaskHandler), typeof(IconInfoHelper), new PropertyMetadata(null));



        static async Task LoadImageFromIconInfo(DependencyObject d, IconInfo iconInfo)
        {
            var LoadImageFromIconInfoOneTaskHandler = GetOneTaskHandler(d);
            if (LoadImageFromIconInfoOneTaskHandler is null)
            {
                LoadImageFromIconInfoOneTaskHandler = new OneTaskHandler();
                SetOneTaskHandler(d, LoadImageFromIconInfoOneTaskHandler);
            }

            void ResetPropsMethod()
            {
                //Reset props code here like
                SetIsLoading(d, false);
            }

            //Add Waiting props Here Like
            SetIsLoading(d, true);
            SetImageSource(d, null);

            LoadImageFromIconInfoOneTaskHandler.CancelRunningTasks();

            //Add your code to check to run a new operation. Like
            if (iconInfo is null)
            {
                ResetPropsMethod();
                return;
            }

            //A CancellationTokenSource for the new Task
            var cancellationTokenSource = LoadImageFromIconInfoOneTaskHandler.PrepareNewTask();
            var sizeW = GetIconSizeW(d);
            //Await for the task
            var taskResult = await TaskResult.RunAsync((cancel) =>
            {
                if (sizeW.HasValue)
                {
                    //return iconInfo?.GetBestFitIcon(new System.Drawing.Size(sizeW.Value, sizeW.Value))?.Image?.ToSWBitmapImage();
                    return iconInfo?.GetBestFitIcon(new System.Drawing.Size(sizeW.Value, sizeW.Value))?.BuildImageBytes();
                }
                else
                {
                    //return iconInfo?.GetBestFitIcon()?.Image?.ToSWBitmapImage();
                    return iconInfo?.GetBestFitIcon()?.BuildImageBytes();
                }
            }, cancellationTokenSource.Token);

            // Remove cancellationTokenSource from running tasks
            LoadImageFromIconInfoOneTaskHandler.AfterTask(cancellationTokenSource);

            //If the task is Canceled by a newer task or other things
            if (cancellationTokenSource.IsCancellationRequested)
            {
                //Cleanup TaskResult
                //If the operation is Canceled and there is an other task running Let next task to reset props
                if (LoadImageFromIconInfoOneTaskHandler.ContainsTask()) return;
            }
            else
            {
                //Set Result Code from TaskResult
                if (taskResult.OperationWasSuccessful)
                {
                    SetImageSource(d, taskResult.Result?.ToBitmapImage());
                    //SetImageSource(d, taskResult.Result);
                    //SetImageSource(d, taskResult.Result.Clone());
                }
            }
            //Reset props code here
            ResetPropsMethod();
        }
    }
}
