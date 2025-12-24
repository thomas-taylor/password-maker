using PasswordMakerClient;

namespace PasswordMakerAvalonia.Browser;

public class SettingsStorage : ISettingsStorage
{
    private const string localStorageKey = "settingsJson";

    public SettingsStorage() { }

    public PasswordMakerSettings LoadSettings()
    {

        string settingsJson = AppJs.GetLocalStorageValue(localStorageKey);
        return PasswordMakerSettings.FromJsonOrDefault(settingsJson);
    }

    public void SaveSettings(PasswordMakerSettings settings)
    {
        string settingsJson = settings.ToJson();
        AppJs.SetLocalStorageValue(localStorageKey, settingsJson);
    }
}