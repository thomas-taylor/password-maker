using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using PasswordMakerAvalonia.Views;
using PasswordMakerClient;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using PasswordMaker;

namespace PasswordMakerAvalonia;

public partial class App : Application
{

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ISettingsStorage settingsStorage;
    public static Client Client { get; private set; }

    public static void InitializeClient(ISettingsStorage iSettingsStorage)
    {
        settingsStorage = iSettingsStorage;
        Client = new(GetSettings());
        Client.Settings.PropertyChanged += (o, e) => SaveSettings();
        SubscribeToSavedOptions(Client.Settings.SavedOptions);
    }

    private static PasswordMakerSettings GetSettings()
    {
        try
        {
            return settingsStorage.LoadSettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings from local storage: {ex}");
            Console.WriteLine("Using default settings");
            return PasswordMakerSettings.GetDefaultSettings();
        }
    }

    public static void SaveSettings()
    {
        settingsStorage.SaveSettings(Client.Settings);
    }

    private static void SubscribeToSavedOptions(PasswordMakerOptionsCollection savedOptions)
    {
        // Subscribe to collection changes
        savedOptions.CollectionChanged += OnSavedOptionsCollectionChanged;

        // Subscribe to PropertyChanged for each existing item
        foreach (PasswordMakerOptions pmo in savedOptions)
        {
            pmo.PropertyChanged += OnSavedOptionPropertyChanged;
        }
    }

    private static void OnSavedOptionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (PasswordMakerOptions newItem in e.NewItems)
            {
                // Subscribe to PropertyChanged for new items
                newItem.PropertyChanged += OnSavedOptionPropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (PasswordMakerOptions oldItem in e.OldItems)
            {
                // Unsubscribe from PropertyChanged for removed items
                oldItem.PropertyChanged -= OnSavedOptionPropertyChanged;
            }
        }

        // Save settings when collection changes
        SaveSettings();
    }

    private static void OnSavedOptionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Save settings when an item property changes
        SaveSettings();
    }
}