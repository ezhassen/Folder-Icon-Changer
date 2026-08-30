using FolderIconChangerWPF.Windows;
using System.Threading.Tasks;
using System.Windows;

namespace FolderIconChangerWPF.Helpers
{
    /// <summary>
    /// Modern themed replacement for MessageBox.Show. Uses MetroWindow with MahApps theme (light/dark aware).
    /// Drop-in signature compatible with MessageBox.Show.
    /// </summary>
    public static class ModernMessageBox
    {
        static Window ResolveOwner(Window owner)
        {
            if (owner != null && owner.IsLoaded) return owner;
            var active = Application.Current?.MainWindow;
            if (active != null && active.IsLoaded && active.IsVisible) return active;
            foreach (Window w in Application.Current?.Windows ?? new WindowCollection())
                if (w.IsActive) return w;
            return active;
        }

        static MessageBoxResult ShowCore(Window owner, string message, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            Window o = ResolveOwner(owner);
            // Must run on UI thread
            if (Application.Current?.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
                return Application.Current.Dispatcher.Invoke(() => ShowCore(owner, message, caption, button, icon, defaultResult));

            var dlg = new ModernMessageBoxWindow(message, caption, button, icon, defaultResult);
            if (o != null && o != dlg && o.IsLoaded) dlg.Owner = o;
            else dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            bool? dr = dlg.ShowDialog();
            // ShowDialog returns null if closed via X; Result handles mapping
            return dlg.Result;
        }

        // Overloads matching MessageBox.Show
        public static MessageBoxResult Show(string message) => ShowCore(null, message, null, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        public static MessageBoxResult Show(string message, string caption) => ShowCore(null, message, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        public static MessageBoxResult Show(Window owner, string message) => ShowCore(owner, message, null, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button) => ShowCore(null, message, caption, button, MessageBoxImage.None, MessageBoxResult.None);
        public static MessageBoxResult Show(Window owner, string message, string caption) => ShowCore(owner, message, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon) => ShowCore(null, message, caption, button, icon, MessageBoxResult.None);
        public static MessageBoxResult Show(Window owner, string message, string caption, MessageBoxButton button) => ShowCore(owner, message, caption, button, MessageBoxImage.None, MessageBoxResult.None);
        public static MessageBoxResult Show(Window owner, string message, string caption, MessageBoxButton button, MessageBoxImage icon) => ShowCore(owner, message, caption, button, icon, MessageBoxResult.None);
        public static MessageBoxResult Show(Window owner, string message, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) => ShowCore(owner, message, caption, button, icon, defaultResult);
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult) => ShowCore(null, message, caption, button, icon, defaultResult);

        // Async variants for MahApps-style await support
        public static Task<MessageBoxResult> ShowAsync(Window owner, string message, string caption = null, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
            => Task.Run(() => ShowCore(owner, message, caption, button, icon, defaultResult));
    }
}
