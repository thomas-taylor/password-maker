using Microsoft.JSInterop;
using PasswordMaker;
using PasswordMakerClient;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PasswordMakerBlazor;

public static class App
{
    private static IJSRuntime jsRuntime;
    public static Client Client { get; private set; }

    public static async Task InitializeClient(IJSRuntime jsRuntimeInstance)
    {
        jsRuntime = jsRuntimeInstance;
        Client = new(await GetSettings());
        Client.Settings.PropertyChanged += async (o, e) => await SaveSettings();
        SubscribeToSavedOptions(Client.Settings.SavedOptions);
    }

    private static async Task<PasswordMakerSettings> GetSettings()
    {
        try
        {
            string settingsJson = await jsRuntime.InvokeAsync<string>("getLocalStorageValue", "settingsJson");
            return PasswordMakerSettings.FromJsonOrDefault(settingsJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings from local storage: {ex}");
            Console.WriteLine("Using default settings");
            return PasswordMakerSettings.GetDefaultSettings();
        }
    }

    public static async Task SaveSettings()
    {
        string settingsJson = Client.Settings.ToJson();
        await jsRuntime.InvokeVoidAsync("setLocalStorageValue", "settingsJson", settingsJson);
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

    private static void UnsubscribeFromSavedOptions(PasswordMakerOptionsCollection savedOptions)
    {
        // Unsubscribe from collection changes
        savedOptions.CollectionChanged -= OnSavedOptionsCollectionChanged;

        // Unsubscribe from PropertyChanged for each item
        foreach (var option in savedOptions)
        {
            option.PropertyChanged -= OnSavedOptionPropertyChanged;
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
        SaveSettings().ConfigureAwait(false);
    }

    private static void OnSavedOptionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Save settings when an item property changes
        SaveSettings().ConfigureAwait(false);
    }
}