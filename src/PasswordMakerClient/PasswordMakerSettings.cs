using PasswordMaker;
using System;
using System.Text.Json;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PasswordMakerClient;

public class PasswordMakerSettings : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private PasswordMakerOptionsCollection savedOptions = [];
    public PasswordMakerOptionsCollection SavedOptions
    {
        get { return savedOptions; }
        set
        {
            if (savedOptions != value)
            {
                savedOptions = value;
                NotifyPropertyChanged();
            }
        }
    }

    public int lastSavedOptionsIndex = 0;
    public int LastSavedOptionsIndex
    {
        get { return lastSavedOptionsIndex; }
        set
        {
            if (lastSavedOptionsIndex != value)
            {
                lastSavedOptionsIndex = value;
                NotifyPropertyChanged();
            }
        }
    }

    public int numberToGenerate = 1;
    public int NumberToGenerate
    {
        get { return numberToGenerate; }
        set
        {
            if (value < 1) value = 1;
            if (numberToGenerate != value)
            {
                numberToGenerate = value;
                NotifyPropertyChanged();
            }
        }
    }

    public static PasswordMakerSettings GetDefaultSettings()
    {
        return new PasswordMakerSettings
        {
            SavedOptions =
            [
                PasswordMakerOptions.NewOptionsDefault(),
                PasswordMakerOptions.NewOptionsAlphanumeric(),
                PasswordMakerOptions.NewOptionsNumbers()
            ],
            LastSavedOptionsIndex = 0,
            NumberToGenerate = 1
        };
    }

    public static PasswordMakerSettings FromFileOrDefault(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return GetDefaultSettings();
        if (!File.Exists(fileName)) return GetDefaultSettings();
        return FromFile(fileName);
    }

    public static PasswordMakerSettings FromFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("fileName parameter is null or empty");
        if (!File.Exists(fileName)) throw new ArgumentException($"Specified file ({fileName}) not found");
        string json = File.ReadAllText(fileName);
        return FromJson(json);
    }

    public void SaveToFile(string fileName)
    {
        File.WriteAllText(fileName, ToJson());
    }

    public static PasswordMakerSettings FromJsonOrDefault(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return GetDefaultSettings();
        return FromJson(json);
    }

    public static PasswordMakerSettings FromJson(string json)
    {
        if (string.IsNullOrEmpty(json)) throw new ArgumentException("json parameter is null or empty");
        PasswordMakerSettings passwordMakerSettings = JsonSerializer.Deserialize<PasswordMakerSettings>(json);

        // If supplied json does not have any saved opions add a default one
        if ((passwordMakerSettings.SavedOptions == null) || (passwordMakerSettings.SavedOptions.Count == 0))
            passwordMakerSettings.SavedOptions = [new PasswordMakerOptions()];
        // Make sure last number to generate is a positive number
        if (passwordMakerSettings.NumberToGenerate <= 0) passwordMakerSettings.NumberToGenerate = 1;

        return passwordMakerSettings;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    internal void UpdateLastSavedOptionsIndex(PasswordMakerOptions currentOptions)
    {
        for (int i = 0; i < SavedOptions.Count; i++)
        {
            if (SavedOptions[i] == currentOptions)
            {
                LastSavedOptionsIndex = i;
                break;
            }
        }
    }
}