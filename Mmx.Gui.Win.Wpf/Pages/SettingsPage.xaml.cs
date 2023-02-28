using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mmx.Gui.Win.Common;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INotifyPropertyChanged
    {

        private readonly Dictionary<string, string> _languages = new Dictionary<string, string>
        {
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

        public Dictionary<string, string> Languages => _languages;


        private readonly Dictionary<int, string> _updateIntervals = new Dictionary<int, string>
        {
            { 60 * 60, "hourly" },
            { 24 * 60 * 60, "daily" },
        };

        public Dictionary<int, string> UpdateIntervals => _updateIntervals;

        public Array Themes
        {
            get => Enum.GetValues(typeof(ModernWpf.ElementTheme));
        }

        public int CHIAPOS_MAX_CUDA_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_CUDA_DEVICES_Maximum => CudaInfo.Instance.Devices.Count();

        public int CHIAPOS_MAX_CORES_Minimum => 1;
        public int CHIAPOS_MAX_CORES_Maximum => Environment.ProcessorCount;

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private bool _debugGroupBoxIsEnabled;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool DebugGroupBoxIsEnabled
        {
            get => _debugGroupBoxIsEnabled;

            private set {
                _debugGroupBoxIsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
