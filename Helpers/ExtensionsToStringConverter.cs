using System.Globalization;
using System.Windows.Data;

namespace SmartFileOrganizer.Helpers;

public class ExtensionsToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<string> extensions)
        {
            return string.Join(", ", extensions);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}