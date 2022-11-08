using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mmx.Gui.Win.Wpf.Common.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
            {
                bValue = (bool)value;
            }
            else if (value is Nullable<bool>)
            {
                Nullable<bool> tmp = (Nullable<bool>)value;
                bValue = tmp.HasValue ? tmp.Value : false;
            }
            bValue = (parameter != null) ? !bValue : bValue;
            return (bValue) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //if (value is Visibility)
            //{
            //    return (Visibility)value == Visibility.Visible;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
