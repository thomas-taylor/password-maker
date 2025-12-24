using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using System.Collections.Generic;
using PasswordMaker;

namespace PasswordMakerAvalonia;

public partial class OptionsEditor : UserControl
{
    private List<CheckBox> groupsCheckBoxes;

    public OptionsEditor()
    {
        InitializeComponent();
        LoadGroupsCheckBoxes();
    }

    private PasswordMakerOptions passwordMakerOptions;
    public PasswordMakerOptions PasswordMakerOptions
    {
        get { return passwordMakerOptions; }
        set
        {
            passwordMakerOptions = value;
            this.DataContext = passwordMakerOptions;
        }
    }

    private void LoadGroupsCheckBoxes()
    {
        characterGroupsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        characterGroupsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        var charFont = new Avalonia.Media.FontFamily("Consolas");
        var margin = new Thickness(5.0);
        groupsCheckBoxes = [];

        for (int i = 0; i < PasswordMakerCharacterGroups.CharacterGroupDefinitions.Count; i++)
        {
            var def = PasswordMakerCharacterGroups.CharacterGroupDefinitions[i];
            characterGroupsGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            var cb = new CheckBox
            {
                Content = def.GroupName,
                Tag = def,
                Margin = margin,
                FontSize = 14.0,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            cb.Bind(CheckBox.IsCheckedProperty, new Avalonia.Data.Binding($"Include{def.CharacterGroupType}"));

            Grid.SetRow(cb, i);
            Grid.SetColumn(cb, 0);
            characterGroupsGrid.Children.Add(cb);
            groupsCheckBoxes.Add(cb);

            var tb = new TextBlock
            {
                Text = def.Display,
                FontFamily = charFont,
                FontSize = 16.0,
                Margin = margin,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            Grid.SetRow(tb, i);
            Grid.SetColumn(tb, 1);
            characterGroupsGrid.Children.Add(tb);
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

    private async void OnPaste(object sender, RoutedEventArgs e)
    {
        IClipboard clipboard = ClipboardHelper.GetClipboard();
        if (clipboard != null)
        {
            var text = await clipboard.GetTextAsync();
            if (!int.TryParse(text, out _))
            {
                e.Handled = true;
            }
        }
    }

    
}
