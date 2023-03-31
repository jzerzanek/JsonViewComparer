using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JsonDataTool.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var element = new FrameworkElement();

            string iconName = value?.ToString();

            if (iconName != null)
            {
                var resource = element.TryFindResource(iconName);

                if (resource != null)
                {
                    return resource;
                }
            }

            return element.FindResource("NoneIcon");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
