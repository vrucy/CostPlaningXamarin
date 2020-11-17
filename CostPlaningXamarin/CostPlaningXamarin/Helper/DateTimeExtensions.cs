using System;

namespace CostPlaningXamarin.Helper
{
    public static class DateTimeExtensions
    {
        public static string DateTimeToString(this DateTime value)
        {
            var x = string.Format("{0}/{1}", value.ToString("MMM"), value.ToString("yyyy"));
            return x;

        }
        public static DateTime StringToDateTime(this string value)
        {
            var x = DateTime.Parse(value);
            return x;
        }
    }
}
