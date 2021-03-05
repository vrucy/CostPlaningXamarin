using CostPlaningXamarin.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CostPlaningXamarin.Converters
{
    class ReverseBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value is Category)
            {
                var x = (Category)value;
                if (x.Id == 0)
                {
                    return null;
                }
                return !(bool)x.IsVisible;
            }
            if (value is Order)
            {
                var x = (Order)value;
                if (x.Id == 0)
                {
                    return null;
                }
                return !(bool)x.IsVisible;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
