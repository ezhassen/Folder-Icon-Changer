using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static class SelectorHelper
    {

        public static ICommand GetSelectedCommand(DependencyObject obj) => (ICommand)obj.GetValue(SelectedCommandProperty);

        public static void SetSelectedCommand(DependencyObject obj, ICommand value) => obj.SetValue(SelectedCommandProperty, value);

        // Using a DependencyProperty as the backing store for SelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCommandProperty =
            DependencyProperty.RegisterAttached("SelectedCommand", typeof(ICommand), typeof(SelectorHelper), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedCommandPropertyChanged)));

        private static void OnSelectedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Selector selector)) return;
            selector.SelectionChanged -= Selector_SelectionChanged;
            if (e.NewValue is ICommand)
            {
                selector.SelectionChanged += Selector_SelectionChanged;
            }
        }

        private static void Selector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!(sender is Selector selector)) return;
            var command = GetSelectedCommand(selector);
            if (command.CanExecute(selector.SelectedItem))
            {
                command.Execute(selector.SelectedItem);
            }
        }
    }
}
