using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using PasswordMakerBlazor;

internal class Program
{
    [System.Runtime.Versioning.SupportedOSPlatform("browser")] // This is to suppress warning about AppJs.Initialize() being supported only on 'browser'
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<AppRouter>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddFluentUIComponents();
        var host = builder.Build();
        await AppJs.Initialize(builder.HostEnvironment.BaseAddress);
        await host.RunAsync();
    }
}