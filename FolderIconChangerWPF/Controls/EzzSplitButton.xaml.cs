using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FolderIconChangerWPF.Controls
{
    /// <summary>
    /// Interaction logic for EzzDropDownButton.xaml
    /// </summary>
    public partial class EzzSplitButton : EzzSplitButtonBase
    {
        public EzzSplitButton()
        {
            InitializeComponent();
            //
            toggleButton = PART_ToggleButton;
        }





    }

    public class EzzSplitButtonBase : UserControl
    {
        protected ToggleButton toggleButton;

        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(object), typeof(EzzSplitButtonBase), new FrameworkPropertyMetadata(null));



        public object ButtonToolTip
        {
            get { return (object)GetValue(ButtonToolTipProperty); }
            set { SetValue(ButtonToolTipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonToolTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonToolTipProperty =
            DependencyProperty.Register("ButtonToolTip", typeof(object), typeof(EzzSplitButtonBase), new PropertyMetadata(null));



        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(EzzSplitButtonBase), new PropertyMetadata(null));



        public object ButtonCommandParameter
        {
            get { return (object)GetValue(ButtonCommandParameterProperty); }
            set { SetValue(ButtonCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandParameterProperty =
            DependencyProperty.Register("ButtonCommandParameter", typeof(object), typeof(EzzSplitButtonBase), new PropertyMetadata(null));




        public ContextMenu ArrowContextMenu
        {
            get { return (ContextMenu)GetValue(ArrowContextMenuProperty); }
            set { SetValue(ArrowContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowContextMenuProperty =
            DependencyProperty.Register("ArrowContextMenu", typeof(ContextMenu), typeof(EzzSplitButtonBase), new PropertyMetadata(null, new PropertyChangedCallback(OnArrowContextMenuPropertyChanged)));

        private static void OnArrowContextMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EzzSplitButtonBase)?.OnArrowContextMenuChanged(e);
        private void OnArrowContextMenuChanged(DependencyPropertyChangedEventArgs e)
        {
            if (toggleButton is null) return;
            if ((e.OldValue is ContextMenu contextMenu))
            {
                contextMenu.Closed -= ContextMenu_Closed;
            }
            toggleButton.Click -= PART_ToggleButton_Click;
            if ((e.NewValue is ContextMenu newcontextMenu))
            {
                newcontextMenu.Closed += ContextMenu_Closed;
                toggleButton.Click += PART_ToggleButton_Click;
            }
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu) || contextMenu.PlacementTarget == null) return;
            if (!(contextMenu.PlacementTarget is ToggleButton toggleButton)) return;
            if (toggleButton.IsChecked.GetValueOrDefault(false)) toggleButton.IsChecked = false;
        }

        private void PART_ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton)) return;

            var cMenu = ArrowContextMenu;
            if (cMenu is null) return;
            if (cMenu.PlacementTarget != toggleButton) {
                cMenu.PlacementTarget = toggleButton;
                cMenu.Placement = PlacementMode.Bottom;
            }
            var isChecked = toggleButton.IsChecked.GetValueOrDefault(false);
            if (cMenu.IsOpen != isChecked) cMenu.IsOpen = isChecked;
        }


    }
}
