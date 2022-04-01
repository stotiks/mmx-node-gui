using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        public Dictionary<string, string> launguages = new Dictionary<string, string>(){
            { "en", "English" },
            { "id", "Bahasa Indonesia" },
            { "de", "Deutsch" },
            { "es", "Español" },
            { "nl", "Nederlands"},
            { "pt", "Português" },
            { "ru", "Русский" },
            { "uk", "Українська" },
            { "zh", "简体中文" },
        };

        public Dictionary<string, string> Languages { 
            get => launguages;
        }

        public Array Themes
        {
            get => Enum.GetValues(typeof(ModernWpf.ElementTheme));
        }

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

    }
}
