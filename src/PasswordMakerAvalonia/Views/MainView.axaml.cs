using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PasswordMakerClient;

namespace PasswordMakerAvalonia.Views;

public partial class MainView : UserControl
{
    private PasswordMakerVm vm;

    public MainView()
    {
        InitializeComponent();
    }

    private void UserControlLoaded(object sender, RoutedEventArgs e)
    {
        if (Design.IsDesignMode) return;

        vm = App.Client.PasswordMakerVm;
        vm.PropertyChanged += (o, args) =>
        {
            if (args.PropertyName == nameof(vm.CurrentOptions))
            {
                optionsEditor.PasswordMakerOptions = vm.CurrentOptions;
            }
        };

        DataContext = vm;
        optionsEditor.PasswordMakerOptions = vm.CurrentOptions;
    }

    private void GenerateButtonClick(object sender, RoutedEventArgs e)
    {
        if (vm.CurrentOptions.GetCharacterSetLength() == 0)
        {
            //MessageBox.Show("Zero characters selected for generation.\r\n\r\nSpecify one or more groups or specific characters to include.", "No Characters Selected", MessageBoxButton.OK);
            return;
        }

        generatedTextBox.Text = vm.Generate();
    }

    private void CopyToClipboardButtonClick(object sender, RoutedEventArgs e)
    {
        IClipboard clipboard = ClipboardHelper.GetClipboard();
        clipboard?.SetTextAsync(generatedTextBox.Text.Trim());
    }

    private async void NewOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OptionsNameDialog(string.Empty, OptionsNameDialogMode.NewOptions);
        var result = await dialog.ShowDialogAsync(this.GetVisualRoot() as Window);
        if (result)
        {
            vm.AddNewPasswordMakerOptions(dialog.OptionsName);
        }
    }

    private async void RenameOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OptionsNameDialog(vm.CurrentOptions.Name, OptionsNameDialogMode.RenameOptions);
        var result = await dialog.ShowDialogAsync(this.GetVisualRoot() as Window);
        if (result)
        {
            vm.RenameCurrentOptions(dialog.OptionsName);
        }
    }

    private async void DeleteOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OptionsNameDialog(vm.CurrentOptions.Name, OptionsNameDialogMode.DeleteOptions);
        var result = await dialog.ShowDialogAsync(this.GetVisualRoot() as Window);
        if (result)
        {
            vm.DeleteCurrentOptions();
        }
    }

    private void PreviewTextInputForPositiveIntegers(object sender, TextInputEventArgs e)
    {
        if (!int.TryParse(e.Text, out _))
        {
            e.Handled = true;
        }
    }

    private void PreviewKeyDownIgnoreSpace(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            e.Handled = true;
        }
    }
}