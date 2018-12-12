using FolderIconChangerWPF.Pages;
using System;
using System.Windows;

namespace FolderIconChangerWPF.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private static ApplicationViewModel _instance;
        public static ApplicationViewModel Instance
        {
            get
            {
                if (_instance == null) _instance = new ApplicationViewModel();
                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        #region Private Members

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        //private bool mSettingsMenuVisible;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public string CurrentPage { get; private set; } = nameof(MainPage);

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        #endregion

        DelegateCommand _AppExitCommand;
        public DelegateCommand AppExitCommand
            => _AppExitCommand ?? (_AppExitCommand = new DelegateCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            }));

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Create the commands
            //OpenChatCommand = new RelayCommand(OpenChat);
            //OpenContactsCommand = new RelayCommand(OpenContacts);
            //OpenMediaCommand = new RelayCommand(OpenMedia);
            if (_instance == null) _instance = this;
        }

        #endregion

        #region Public Helper Methods

        public void GoToPage(string pageName, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // See if page has changed
            var same = (CurrentPage?.Equals(pageName, StringComparison.OrdinalIgnoreCase) ?? false);

            // Set the current page
            CurrentPage = pageName;

            // If the page hasn't changed, fire off notification
            // So pages still update if just the view model has changed
            if (!same) OnPropertyChanged(nameof(CurrentPage));
        }

        ///// <summary>
        ///// Navigates to the specified page
        ///// </summary>
        ///// <param name="page">The page to go to</param>
        ///// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        //public void GoToPage(BasePage page, BaseViewModel viewModel = null)
        //{
        //    // Always hide settings page if we are changing pages
        //    //SettingsMenuVisible = false;

        //    // Set the view model
        //    CurrentPageViewModel = viewModel;

        //    // See if page has changed
        //    var same = (CurrentPage?.GetType().Equals(page) ?? false);

        //    // Set the current page
        //    CurrentPage = page.GetType();

        //    // If the page hasn't changed, fire off notification
        //    // So pages still update if just the view model has changed
        //    if (!same) OnPropertyChanged(nameof(CurrentPage));

        //    // Show side menu or not?
        //    //SideMenuVisible = page == ApplicationPage.Chat;

        //}

        #endregion

        DelegateCommand _GoToPageCommand;
        public DelegateCommand GoToPageCommand
            => _GoToPageCommand ?? (_GoToPageCommand = new DelegateCommand((name) =>
            {
                if (!(name is string nameStr)) return;
                GoToPage(nameStr);
            }));

        //static MainPage _MainPage;
        //static MainPage MainPage
        //{
        //    get
        //    {
        //        if (_MainPage == null) _MainPage = new MainPage();
        //        return _MainPage;
        //    }
        //    set => _MainPage = value;
        //}
        static BasePage NewMainPage => new MainPage();
        public static BasePage GetBasePage(string typeName, bool GetMainPageAsDefault = true)
        {
            if (string.IsNullOrEmpty(typeName)) return (GetMainPageAsDefault ? NewMainPage : null);

            switch (typeName)
            {
                case nameof(MainPage):
                    return NewMainPage;
                case nameof(SettingsPage):
                    return new SettingsPage();
                case nameof(AboutPage):
                    return new AboutPage();
                default:
                    return (GetMainPageAsDefault ? NewMainPage : null);
            }
        }


        DelegateCommand _DragNDropCommand;
        public DelegateCommand DragNDropCommand
            => _DragNDropCommand ?? (_DragNDropCommand = new DelegateCommand((e) =>
            {
                if (!(e is DragEventArgs dragEvent)) return;
                MainPageViewModel.Instance.DragNDropCommand?.Execute(e);
            }, (e) =>
            {
                if (!(e is DragEventArgs dragEvent)) return false;
                if (MainPageViewModel.Instance.DragNDropCommand?.CanExecute(e) ?? false)
                {
                    if (!CurrentPage.Equals("MainPage", StringComparison.OrdinalIgnoreCase))
                    {
                        GoToPage("MainPage");
                    }
                    return true;
                }
                return false;
                //return MainPageViewModel.Instance.DragNDropCommand?.CanExecute(e) ?? false;
            }));
    }

}
