using PasswordMakerClient;
using System;
using System.IO;

namespace PasswordMakerAvalonia.Desktop;

public class SettingsStorage : ISettingsStorage
{
    private readonly string settingsFileName;

    public SettingsStorage()
    {
        string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appFolder = Path.Combine(localFolder, "PasswordMaker");
        if (!Directory.Exists(appFolder)) Directory.CreateDirectory(appFolder);
        settingsFileName = Path.Combine(appFolder, "PasswordMakerSettings.json");
    }

    public PasswordMakerSettings LoadSettings()
    {
        return PasswordMakerSettings.FromFileOrDefault(settingsFileName);
    }

    public void SaveSettings(PasswordMakerSettings settings)
    {
        settings.SaveToFile(settingsFileName);
    }
}