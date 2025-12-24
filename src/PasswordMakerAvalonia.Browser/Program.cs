using Avalonia;
using Avalonia.Browser;
using PasswordMakerAvalonia;
using PasswordMakerAvalonia.Browser;
using System.Runtime.InteropServices.JavaScript;

AppBuilder builder = AppBuilder.Configure<App>().WithInterFont();
string baseAddress = JSHost.GlobalThis.GetPropertyAsJSObject("location").GetPropertyAsString("href");
await AppJs.Initialize(baseAddress);
App.InitializeClient(new SettingsStorage());
await builder.StartBrowserAppAsync("out");