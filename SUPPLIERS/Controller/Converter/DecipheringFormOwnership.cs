using System;
using System.Globalization;
using System.Windows.Data;

namespace SUPPLIERS.Controller
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class DecipheringFormOwnership : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
             bool.TryParse(value.ToString(), out _) ? (((bool)value ? "Юридическое" : "Физическое") + " лицо") : "Ошибка конвертации";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
             bool.TryParse(value.ToString(), out _) ? (((bool)value? "Юридическое" : "Физическое") + " лицо") : "Ошибка конвертации";
    }
}
