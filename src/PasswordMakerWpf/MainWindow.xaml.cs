using MaterialDesignThemes.Wpf;
using PasswordMakerClient;
using System;
using System.Windows;
using System.Windows.Input;

namespace PasswordMakerWpf;

public partial class MainWindow : Window
{
    PasswordMakerVm vm;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        vm = App.Client.PasswordMakerVm;
        vm.PropertyChanged += (o, e) =>
        {
            switch (e.PropertyName)
            {
                case "CurrentOptions":
                    optionsEditor.PasswordMakerOptions = vm.CurrentOptions;
                    break;
            }
        };
        DataContext = vm;
        optionsEditor.PasswordMakerOptions = vm.CurrentOptions;
        DataObject.AddPastingHandler(NumberToGenerateUpDown, OnPaste);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        App.SaveSettings();
    }

    private void GenerateButtonClick(object sender, RoutedEventArgs e)
    {
        if (vm.CurrentOptions.GetCharacterSetLength() == 0)
        {
            MessageBox.Show(this, "Zero characters selected for generation.\r\n\r\nSpecify one or more groups or specific characters to include.", "No Characters Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        generatedTextBox.Text = vm.Generate();
    }
    
    private void CopyToClipboardButtonClick(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(generatedTextBox.Text.Trim());
    }

    private async void NewOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        OptionsNameDialog dialog = new(string.Empty, OptionsNameDialogMode.NewOptions);
        var result = await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");
        if (!(result is bool save) || !save) return;
        vm.AddNewPasswordMakerOptions(dialog.OptionsName);
    }

    private async void RenameOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        OptionsNameDialog dialog = new(vm.CurrentOptions.Name, OptionsNameDialogMode.RenameOptions);
        var result = await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");
        if (!(result is bool save) || !save) return;
        vm.RenameCurrentOptions(dialog.OptionsName);
    }

    private async void DeleteOptionsButtonClick(object sender, RoutedEventArgs e)
    {
        OptionsNameDialog dialog = new(vm.CurrentOptions.Name, OptionsNameDialogMode.DeleteOptions);
        var result = await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");
        if (!(result is bool save) || !save) return;
        vm.DeleteCurrentOptions();
        //if (App.Client.Settings.SavedOptions.Count > 0)
        //    savedOptionsComboBox.SelectedIndex = 0;
    }

    private void PreviewTextInputForPositiveIntegers(object sender, TextCompositionEventArgs e)
    {
        char c = Convert.ToChar(e.Text);
        if (!char.IsDigit(c)) e.Handled = true;
    }

    private void PreviewKeyDownIgnoreSpace(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
            e.Handled = true;
    }

    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(DataFormats.Text))
            e.CancelCommand();
        if (!int.TryParse((string)e.DataObject.GetData(DataFormats.Text), out _))
            e.CancelCommand();
    }
}