using System;
using System.Globalization;
using Xamarin.Forms;

namespace Zoom_Task.Core.Converters
{
	public class BoolInvertConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

