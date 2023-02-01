using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using ModernWpf;
using System.Windows;

namespace Mmx.Gui.Win.Wpf
{
    internal class WpfMMXBoundObject : MMXBoundObject
    {
        public override bool Theme_dark
        {
            get
            {
                var isDarkTheme = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    isDarkTheme = Settings.Default.Theme == ElementTheme.Default.ToString() ?
                    ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark :
                    Settings.Default.Theme == ElementTheme.Dark.ToString();
                });
                return isDarkTheme;
            }
        }
    }
}