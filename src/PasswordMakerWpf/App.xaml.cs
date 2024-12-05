using PasswordMakerClient;
using System;
using System.IO;
using System.Windows;

namespace PasswordMakerWpf;

public partial class App : Application
{
    private static Client client;
    public static Client Client
    {
        get
        {
            client ??= new(PasswordMakerSettings.FromFileOrDefault(SettingsFileName));
            return client;
        }
    }

    private static string SettingsFileName
    {
        get
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(localFolder, "PasswordMaker");
            if (!Directory.Exists(appFolder)) Directory.CreateDirectory(appFolder);
            return Path.Combine(appFolder, "PasswordMakerSettings.json");
        }
    }

    internal static void SaveSettings()
    {
        Client.Settings.SaveToFile(SettingsFileName);
    }
}