namespace PasswordMakerClient;

public interface ISettingsStorage
{
    PasswordMakerSettings LoadSettings();
    void SaveSettings(PasswordMakerSettings settings);
}