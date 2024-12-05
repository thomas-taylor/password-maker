using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordMakerWpf;

public enum OptionsNameDialogMode
{
    NewOptions,
    RenameOptions,
    DeleteOptions
}

public partial class OptionsNameDialog : UserControl
{
    private OptionsNameDialogMode dialogMode = OptionsNameDialogMode.RenameOptions;

    public string OptionsName
    {
        get { return nameTextBox.Text; }
        set { nameTextBox.Text = value; }
    }

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
                captionPackIcon.Kind = PackIconKind.PlusOutline;
                saveButton.Caption = "Save";
                break;
            case OptionsNameDialogMode.DeleteOptions:
                captionTextBlock.Text = "Confirm Deletion";
                captionPackIcon.Kind = PackIconKind.Delete;
                saveButton.Caption = "Delete";
                nameTextBox.IsReadOnly = true;
                break;
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        switch (dialogMode)
        {
            case OptionsNameDialogMode.NewOptions:
            case OptionsNameDialogMode.RenameOptions:
                nameTextBox.Focus();
                nameTextBox.SelectAll();
                break;
            case OptionsNameDialogMode.DeleteOptions:
                saveButton.Focus();
                break;
        }
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void Save()
    {
        switch (dialogMode)
        {
            case OptionsNameDialogMode.NewOptions:
            case OptionsNameDialogMode.RenameOptions:
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    ShowMessage("Name is required");
                    nameTextBox.Focus();
                    return;
                }
                break;
        }
        DialogHost.CloseDialogCommand.Execute(true, this);
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        Cancel();
    }

    private void Cancel()
    {
        DialogHost.CloseDialogCommand.Execute(false, this);
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
                Cancel();
                e.Handled = true;
                break;
        }
    }

    private void HideMessage()
    {
        messageColorZone.Visibility = Visibility.Collapsed;
        messageTextBlock.Text = string.Empty;
    }

    private void ShowMessage(string message)
    {
        messageColorZone.Visibility = Visibility.Visible;
        messageTextBlock.Text = message;
    }

}