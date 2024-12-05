using PasswordMaker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PasswordMakerWpf;

public partial class OptionsEditor : UserControl
{
    private List<CheckBox> groupsCheckBoxes;

    public OptionsEditor()
    {
        InitializeComponent();
        LoadGroupsCheckBoxes();
        DataObject.AddPastingHandler(LengthUpDown, OnPaste);
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
        characterGroupsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        characterGroupsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        FontFamily charFont = new FontFamily("Consolas, Courier New");
        Thickness margin = new Thickness(5.0);
        groupsCheckBoxes = new List<CheckBox>();
        for (int i = 0; i < PasswordMakerCharacterGroups.CharacterGroupDefinitions.Count; i++)
        {
            PasswordMakerCharacterGroup def = PasswordMakerCharacterGroups.CharacterGroupDefinitions[i];
            characterGroupsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            CheckBox cb = new CheckBox();
            Grid.SetRow(cb, i);
            Grid.SetColumn(cb, 0);
            cb.Content = def.GroupName;
            cb.Tag = def;
            cb.Margin = margin;
            cb.FontSize = 14.0;
            cb.SetBinding(CheckBox.IsCheckedProperty, $"Include{def.CharacterGroupType}");
            characterGroupsGrid.Children.Add(cb);
            groupsCheckBoxes.Add(cb);

            TextBlock tb = new TextBlock();
            Grid.SetRow(tb, i);
            Grid.SetColumn(tb, 1);
            tb.Text = def.Display;
            tb.FontFamily = charFont;
            tb.FontSize = 16.0;
            tb.Margin = margin;
            characterGroupsGrid.Children.Add(tb);
        }
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