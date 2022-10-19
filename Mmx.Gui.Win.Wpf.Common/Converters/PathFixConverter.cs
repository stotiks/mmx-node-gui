using Mmx.Gui.Win.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Mmx.Gui.Win.Wpf.Common.Converters
{
    public class PathFixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dir = value as string;
            return PlotterOptions.FixDir(dir);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}


