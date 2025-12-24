using System;
using Avalonia.Data.Converters;
using System.Globalization;

namespace PasswordMakerAvalonia;

public class NullToDefaultConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Directly pass through the value (used for UI-to-property binding).
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // When converting back to the property, replace null with a default value.
        return value ?? 1; // Default value if null.
    }
}