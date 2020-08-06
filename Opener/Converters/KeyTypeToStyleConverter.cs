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
    public class KeyTypeToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyType = (OKeyType)value;
            if (keyType != null && keyType.Id == (int)KeyTypeId.SecureData)
            {
                return Application.Current.Resources["passwordStyle"] as Style;
            }
            else
            {
                return Application.Current.Resources["normalStyle"] as Style;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
