using System;
using System.Globalization;
using Xamarin.Forms;

namespace CostPlaningXamarin.Converters
{
    class CategoryIsVisibleToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return "Disable";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
