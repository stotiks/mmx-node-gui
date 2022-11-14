using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {

        private Dictionary<string, string> _launguages = new Dictionary<string, string>(){
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

        public Dictionary<string, string> Languages
        {
            get => _launguages;
        }


        private Dictionary<int, string> _updateIntervals = new Dictionary<int, string>(){
            { 60 * 60, "hourly" },
            { 24 * 60 * 60, "daily" },
        };

        public Dictionary<int, string> UpdateIntervals
        {
            get => _updateIntervals;
        }


        public Array Themes
        {
            get => Enum
                .GetValues(typeof(ModernWpf.ElementTheme))
                .Cast<ModernWpf.ElementTheme>()
                .Where(value => value == ModernWpf.ElementTheme.Light || value == ModernWpf.ElementTheme.Dark).ToArray();
        }

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private bool _debugGroupBoxIsEnabled = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DebugGroupBoxIsEnabled
        {
            get => _debugGroupBoxIsEnabled;

            private set {
                _debugGroupBoxIsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void DebugGroupBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 3)
            {
                DebugGroupBoxIsEnabled = !DebugGroupBoxIsEnabled;
            }
        }

    }
}
