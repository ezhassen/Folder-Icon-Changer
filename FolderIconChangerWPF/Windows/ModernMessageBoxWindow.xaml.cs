using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FolderIconChangerWPF.Windows
{
    public partial class ModernMessageBoxWindow : MetroWindow
    {
        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register(nameof(DialogTitle), typeof(string), typeof(ModernMessageBoxWindow), new PropertyMetadata("Folder Icon Changer"));

        MessageBoxResult _result = MessageBoxResult.None;
        MessageBoxButton _button;
        MessageBoxResult _defaultResult;

        public ModernMessageBoxWindow(string message, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
        {
            InitializeComponent();
            DataContext = this;
            DialogTitle = string.IsNullOrWhiteSpace(title) ? GetLocalizedTitle() : title;
            MessageTextBox.Text = message ?? string.Empty;
            _button = button;
            _defaultResult = defaultResult;
            ConfigureIcon(image);
            ConfigureButtons(button, defaultResult);
        }

        string GetLocalizedTitle()
        {
            try { return LocalizationProvider.GetLocalizedString("MainFormTitle") ?? "Folder Icon Changer"; }
            catch { return "Folder Icon Changer"; }
        }

        void ConfigureIcon(MessageBoxImage image)
        {
            string glyph = null;
            Brush bg = null;
            if (image == MessageBoxImage.Error || image == MessageBoxImage.Hand || image == MessageBoxImage.Stop)
            {
                glyph = "\uf00d";
                bg = (Brush)FindResource("EzzWordRedBrush");
            }
            else if (image == MessageBoxImage.Warning || image == MessageBoxImage.Exclamation)
            {
                glyph = "\uf071";
                bg = new SolidColorBrush(Color.FromRgb(0xE6, 0xA2, 0x3E));
            }
            else if (image == MessageBoxImage.Question)
            {
                glyph = "\uf128";
                bg = (Brush)TryFindResource("MahApps.Brushes.AccentBase") as Brush ?? Brushes.DodgerBlue;
            }
            else if (image == MessageBoxImage.Information || image == MessageBoxImage.Asterisk)
            {
                glyph = "\uf129";
                bg = (Brush)TryFindResource("MahApps.Brushes.AccentBase") as Brush ?? Brushes.DodgerBlue;
            }
            else
            {
                IconBorder.Visibility = Visibility.Collapsed;
                return;
            }
            IconText.Text = glyph;
            IconBorder.Background = bg;
            IconBorder.Visibility = Visibility.Visible;
        }

        void ConfigureButtons(MessageBoxButton button, MessageBoxResult defaultResult)
        {
            ButtonPanel.Children.Clear();
            string ok = "OK", cancel = "Cancel", yes = "Yes", no = "No";
            // Try localize common buttons if resource exists
            try
            {
                // keep English fallback; localized system buttons would need resx entries
                // e.g., LocalizationProvider.GetLocalizedString("OK") if exists
            }
            catch { }

            void Add(string content, MessageBoxResult result, bool isDefault, bool isPrimary)
            {
                var btn = new System.Windows.Controls.Button
                {
                    Content = content,
                    IsDefault = isDefault,
                    IsCancel = result == MessageBoxResult.Cancel || result == MessageBoxResult.No,
                    Style = (Style)FindResource(isPrimary ? "ModernDialogPrimaryButton" : "ModernDialogSecondaryButton"),
                    Tag = result
                };
                btn.Click += (s, e) => { _result = result; DialogResult = isDefault || result == MessageBoxResult.OK || result == MessageBoxResult.Yes; Close(); };
                // Make Enter/Esc work even without IsDefault/IsCancel via tag
                ButtonPanel.Children.Add(btn);
                if (isDefault) btn.Focus();
            }

            bool IsDefault(MessageBoxResult r) => r == defaultResult;

            switch (button)
            {
                case MessageBoxButton.OK:
                    Add(ok, MessageBoxResult.OK, true, true);
                    break;
                case MessageBoxButton.OKCancel:
                    Add(ok, MessageBoxResult.OK, IsDefault(MessageBoxResult.OK), true);
                    Add(cancel, MessageBoxResult.Cancel, IsDefault(MessageBoxResult.Cancel), false);
                    break;
                case MessageBoxButton.YesNo:
                    Add(yes, MessageBoxResult.Yes, IsDefault(MessageBoxResult.Yes), true);
                    Add(no, MessageBoxResult.No, IsDefault(MessageBoxResult.No), false);
                    break;
                case MessageBoxButton.YesNoCancel:
                    Add(yes, MessageBoxResult.Yes, IsDefault(MessageBoxResult.Yes), true);
                    Add(no, MessageBoxResult.No, IsDefault(MessageBoxResult.No), false);
                    Add(cancel, MessageBoxResult.Cancel, IsDefault(MessageBoxResult.Cancel), false);
                    break;
            }
            if (ButtonPanel.Children.Count > 0 && defaultResult == MessageBoxResult.None)
            {
                if (ButtonPanel.Children[0] is System.Windows.Controls.Button b) b.Focus();
            }
        }

        void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure owner-centered and focus
            if (ButtonPanel.Children.Count > 0 && ButtonPanel.Children[0] is System.Windows.Controls.Button fb && !fb.IsFocused)
                fb.Focus();
        }

        void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Map Esc to Cancel/No per button type
                if (_button == MessageBoxButton.OK)
                    _result = MessageBoxResult.OK;
                else if (_button == MessageBoxButton.OKCancel || _button == MessageBoxButton.YesNoCancel)
                    _result = MessageBoxResult.Cancel;
                else if (_button == MessageBoxButton.YesNo)
                    _result = MessageBoxResult.No;
                Close();
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            if (_result == MessageBoxResult.None)
            {
                // Treat close via X as Cancel/No
                _result = _button switch
                {
                    MessageBoxButton.OK => MessageBoxResult.OK,
                    MessageBoxButton.OKCancel => MessageBoxResult.Cancel,
                    MessageBoxButton.YesNo => MessageBoxResult.No,
                    MessageBoxButton.YesNoCancel => MessageBoxResult.Cancel,
                    _ => MessageBoxResult.None
                };
            }
        }

        public MessageBoxResult Result => _result;
    }
}
