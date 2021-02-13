﻿using System;
using Xamarin.Forms;

namespace CostPlaningXamarin.Converters
{
    public class EmptyStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? parameter : value;
        }

        public object ConvertBack(
              object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
