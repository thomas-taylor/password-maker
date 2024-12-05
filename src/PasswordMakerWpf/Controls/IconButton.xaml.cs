using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace PasswordMakerWpf;

public partial class IconButton : UserControl
{
    public IconButton()
    {
        InitializeComponent();
        DataContext = this;
    }

    public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
                "Click", typeof(RoutedEventHandler), typeof(IconButton));

    public RoutedEventHandler Click
    {
        get { return (RoutedEventHandler)GetValue(ClickProperty); }
        set { SetValue(ClickProperty, value); }
    }

    public static readonly DependencyProperty PackIconKindProperty = DependencyProperty.Register(
        "PackIconKind", typeof(PackIconKind), typeof(IconButton));

    public PackIconKind PackIconKind
    {
        get { return (PackIconKind)GetValue(PackIconKindProperty); }
        set { SetValue(PackIconKindProperty, value); }
    }

    public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
        "Caption", typeof(string), typeof(IconButton));

    public string Caption
    {
        get { return (string)GetValue(CaptionProperty); }
        set { SetValue(CaptionProperty, value); }
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}
