using System;
using System.Globalization;
using System.Windows.Data;

namespace SUPPLIERS.Controller
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class GetAddress : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.ToString().Replace('^', ' ');
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value.ToString().Replace('^', ' ');
    }
}
