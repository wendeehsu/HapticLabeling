using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace HapticLabeling.Model
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() != typeof(bool))
            {
                throw new ArgumentException("Only Boolean is supported");
            }

            if (targetType != typeof(Visibility))
            {
                throw new ArgumentException("Wrong Converter");
            }

            if (parameter != null && (string)parameter == "Inverse")
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
