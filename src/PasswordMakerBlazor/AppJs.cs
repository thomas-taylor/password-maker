using System.Runtime.InteropServices.JavaScript;

namespace PasswordMakerBlazor;

public static partial class AppJs
{
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    internal static async Task Initialize(string baseAddress)
    {
        string basePath = new Uri(baseAddress).AbsolutePath;
        string appJsPath = $"{basePath.TrimEnd('/')}/app.js";
        await JSHost.ImportAsync("app", appJsPath);
    }

    [JSImport("copyToClipboard", "app")]
    public static partial string CopyToClipboard(string text);

    [JSImport("getLocalStorageValue", "app")]
    public static partial string GetLocalStorageValue(string key);

    [JSImport("setLocalStorageValue", "app")]
    public static partial void SetLocalStorageValue(string key, string value);
}