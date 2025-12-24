using Android.App;
using Android.Content;
using PasswordMakerClient;

namespace PasswordMakerAvalonia.Android;

public class SettingsStorage : ISettingsStorage
{
    private readonly ISharedPreferences _preferences;

    public SettingsStorage()
    {
        _preferences = Application.Context.GetSharedPreferences("PasswordMakerSettings", FileCreationMode.Private);
    }

    public PasswordMakerSettings LoadSettings()
    {
        string json = _preferences.GetString("settingsJson", null);
        return PasswordMakerSettings.FromJsonOrDefault(json);
    }

    public void SaveSettings(PasswordMakerSettings settings)
    {
        string json = settings.ToJson();
        using var editor = _preferences.Edit();
        editor.PutString("settingsJson", json);
        editor.Apply();
    }
}