using PasswordMaker;
using PasswordMakerClient;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PasswordMakerBlazor;

public static class App
{
    private const string localStorageKey = "settingsJson";
    public static Client Client { get; private set; }

    public static void InitializeClient()
    {
        Client = new(GetSettings());
        Client.Settings.PropertyChanged += (o, e) => SaveSettings();
        SubscribeToSavedOptions(Client.Settings.SavedOptions);
    }

    private static PasswordMakerSettings GetSettings()
    {
        try
        {
            string settingsJson = AppJs.GetLocalStorageValue(localStorageKey);
            return PasswordMakerSettings.FromJsonOrDefault(settingsJson);
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
        string settingsJson = Client.Settings.ToJson();
        AppJs.SetLocalStorageValue(localStorageKey, settingsJson);
    }

    private static void SubscribeToSavedOptions(PasswordMakerOptionsCollection savedOptions)
    {
        // Subscribe to collection changes
        savedOptions.CollectionChanged += OnSavedOptionsCollectionChanged;

        // Subscribe to PropertyChanged for each existing item
        foreach (PasswordMakerOptions pmo in savedOptions)
            pmo.PropertyChanged += OnSavedOptionPropertyChanged;
    }

    private static void OnSavedOptionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (PasswordMakerOptions newItem in e.NewItems)
                newItem.PropertyChanged += OnSavedOptionPropertyChanged;
        }

        if (e.OldItems != null)
        {
            foreach (PasswordMakerOptions oldItem in e.OldItems)
                oldItem.PropertyChanged -= OnSavedOptionPropertyChanged;
        }
        SaveSettings();
    }

    private static void OnSavedOptionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        SaveSettings();
    }
}