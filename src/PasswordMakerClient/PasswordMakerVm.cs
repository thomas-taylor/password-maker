using PasswordMaker;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PasswordMakerClient;

public class PasswordMakerVm : INotifyPropertyChanged
{
    private readonly Client client;
    private bool isInitialized = false;

    public PasswordMakerVm(Client client)
    {
        this.client = client;
        if (client.Settings.SavedOptions.Count > client.Settings.LastSavedOptionsIndex)
            CurrentOptions = client.Settings.SavedOptions[client.Settings.LastSavedOptionsIndex];
        else if (client.Settings.SavedOptions.Count > 0)
            CurrentOptions = client.Settings.SavedOptions[0];
        isInitialized = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PasswordMakerOptionsCollection SavedOptions => client.Settings.SavedOptions;

    private PasswordMakerOptions currentOptions;
    public PasswordMakerOptions CurrentOptions
    {
        get { return currentOptions; }
        set
        {
            if (value == null) return;
            if (currentOptions != value)
            {
                currentOptions = value;
                NotifyPropertyChanged();
                if (isInitialized)
                    client.Settings.UpdateLastSavedOptionsIndex(CurrentOptions);
            }
        }
    }

    public int NumberToGenerate
    {
        get { return client.Settings.NumberToGenerate; }
        set { client.Settings.NumberToGenerate = value; }
    }

    public string Generate()
    {
        if (CurrentOptions is null) throw new InvalidOperationException($"Cannot generate passwords. {nameof(CurrentOptions)} is null");
        StringBuilder sb = new();
        PasswordGenerator pg = new(CurrentOptions);
        for (int i = 0; i < client.Settings.NumberToGenerate; i++)
            sb.AppendLine(pg.GeneratePassword());
        return sb.ToString();
    }

    public PasswordMakerOptions AddNewPasswordMakerOptions(string newName = "New")
    {
        CurrentOptions = client.Settings.SavedOptions.AddNew(newName);
        return CurrentOptions;
    }

    public void DeleteCurrentOptions()
    {
        if (client.Settings.SavedOptions.Count == 1)
            throw new InvalidOperationException("Cannot delete. At least one set of options is required.");
        client.Settings.SavedOptions.Remove(CurrentOptions);
        CurrentOptions = client.Settings.SavedOptions[0];
    }

    public void RenameCurrentOptions(string newName)
    {
        string name = newName.Trim();
        if (string.IsNullOrEmpty(name)) return;
        if (name == CurrentOptions.Name) return;
        CurrentOptions.Name = newName.Trim();
    }
}