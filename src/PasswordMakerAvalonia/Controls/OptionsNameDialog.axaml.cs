using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Threading.Tasks;
using Avalonia.VisualTree;

namespace PasswordMakerAvalonia;

public enum OptionsNameDialogMode
{
    NewOptions,
    RenameOptions,
    DeleteOptions
}

public partial class OptionsNameDialog : UserControl
{
    private readonly OptionsNameDialogMode dialogMode = OptionsNameDialogMode.RenameOptions;

    public string OptionsName
    {
        get => nameTextBox.Text;
        set => nameTextBox.Text = value;
    }

    private TaskCompletionSource<bool> _tcs;

    public OptionsNameDialog() { }

    public OptionsNameDialog(string name, OptionsNameDialogMode dialogMode)
    {
        InitializeComponent();
        HideMessage();
        OptionsName = name;
        this.dialogMode = dialogMode;

        switch (dialogMode)
        {
            case OptionsNameDialogMode.NewOptions:
                captionTextBlock.Text = "New Options";
                saveButton.Content = "Save";
                break;
            case OptionsNameDialogMode.DeleteOptions:
                captionTextBlock.Text = "Confirm Deletion";
                saveButton.Content = "Delete";
                nameTextBox.IsReadOnly = true;
                break;
        }
    }

    public Task<bool> ShowDialogAsync(Window owner)
    {
        var dialogWindow = new Window
        {
            Content = this,
            Width = 500,
            Height = 250,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        _tcs = new TaskCompletionSource<bool>();

        dialogWindow.Closed += (s, e) =>
        {
            _tcs.TrySetResult(false);
        };

        dialogWindow.ShowDialog(owner);
        return _tcs.Task;
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void Save()
    {
        if (dialogMode != OptionsNameDialogMode.DeleteOptions && string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            ShowMessage("Name is required");
            nameTextBox.Focus();
            return;
        }

        _tcs.TrySetResult(true);
        (this.GetVisualRoot() as Window)?.Close();
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        _tcs.TrySetResult(false);
        (this.GetVisualRoot() as Window)?.Close();
    }

    private void UserControl_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                Save();
                e.Handled = true;
                break;
            case Key.Escape:
                CancelButtonClick(sender, null);
                e.Handled = true;
                break;
        }
    }

    private void HideMessage()
    {
        messageTextBlock.IsVisible = false;
        messageTextBlock.Text = string.Empty;
    }

    private void ShowMessage(string message)
    {
        messageTextBlock.IsVisible = true;
        messageTextBlock.Text = message;
    }
}
