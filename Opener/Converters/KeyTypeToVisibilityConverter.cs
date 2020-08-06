using Opener.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Opener.Converters
{
    public class KeyTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyType = (OKeyType)value;
            if (keyType != null && keyType.Id == (int)KeyTypeId.SecureData)
            {
                return string.Equals(parameter, "N") ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return string.Equals(parameter, "N") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
