using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JsonViewComparer.Converters
{
    internal class NullToVisibilityConverter: IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public NullToVisibilityConverter()
        {
            TrueValue = Visibility.Collapsed;
            FalseValue = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                return string.IsNullOrEmpty(value?.ToString()) ? TrueValue : FalseValue;
            }

            return value == null ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
