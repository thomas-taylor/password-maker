using System.Runtime.InteropServices.JavaScript;

namespace PasswordMakerBlazor;

public static partial class AppJs
{
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    internal static async Task Initialize()
    {
        await JSHost.ImportAsync("app", "/app.js");
    }

    [JSImport("copyToClipboard", "app")]
    public static partial string CopyToClipboard(string text);

    [JSImport("getLocalStorageValue", "app")]
    public static partial string GetLocalStorageValue(string key);

    [JSImport("setLocalStorageValue", "app")]
    public static partial void SetLocalStorageValue(string key, string value);
}