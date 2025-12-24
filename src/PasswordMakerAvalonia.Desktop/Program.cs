using System;
using Avalonia;

namespace PasswordMakerAvalonia.Desktop;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        AppBuilder builder = BuildAvaloniaApp();
        App.InitializeClient(new SettingsStorage());
        builder.StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}