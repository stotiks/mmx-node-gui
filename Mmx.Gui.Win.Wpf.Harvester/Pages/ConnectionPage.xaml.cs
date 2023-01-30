using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Harvester;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage : INotifyPropertyChanged
    {
        public HarvesterOptions HarvesterOptions => HarvesterOptions.Instance;
        private HarvesterProcess _harvesterProcess;
        public HarvesterProcess HarvesterProcess => _harvesterProcess;
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        public ConnectionPage(HarvesterProcess harvesterProcess)
        {
            InitializeComponent();
            DataContext = this;

            _harvesterProcess = harvesterProcess;
            _harvesterProcess.OutputDataReceived += _logger.OutputDataReceived;
            _harvesterProcess.ErrorDataReceived += _logger.ErrorDataReceived;

            PropertyChanged += (o, e) =>
            {
                if(e.PropertyName == nameof(DetectRunning))
                {
                    ConnectElementsEnabled = !DetectRunning;
                    //ConnectButtonEnabled = ConnectElementsEnabled;
                }

                if(e.PropertyName == nameof(ConnectElementsEnabled))
                {
                    ConnectButtonEnabled = !HarvesterProcess.IsRunning && ConnectElementsEnabled;
                }

            };

            HarvesterProcess.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(HarvesterProcess.IsRunning))
                {
                    ConnectElementsEnabled = !HarvesterProcess.IsRunning;
                    ConnectButtonVisibility = ConnectElementsEnabled ? Visibility.Visible : Visibility.Hidden;
                }
            };
            
        }

        private bool _detectRunning;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool DetectRunning
        {
            get => _detectRunning;

            private set
            {
                _detectRunning = value;
                NotifyPropertyChanged();
            }
        }

        private bool _connectElementsEnabled = true;
        public bool ConnectElementsEnabled
        {
            get => _connectElementsEnabled;

            private set
            {
                _connectElementsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _connectButtonEnabled = true;
        public bool ConnectButtonEnabled
        {
            get => _connectButtonEnabled;

            private set
            {
                _connectButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility _connectButtonVisibility = Visibility.Visible;
        public Visibility ConnectButtonVisibility
        {
            get => _connectButtonVisibility;

            private set
            {
                _connectButtonVisibility = value;
                NotifyPropertyChanged();
            }
        }
        

        private void DetectButton_Click(object sender, RoutedEventArgs e)
        {
            DetectRunning = true;
            HarvesterOptions.DetectNodeIP().ContinueWith(task =>
            {
                DetectRunning = false;
            });
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var dnsEndPoint = new DnsEndPoint(HostTextBox.Text, (int)PortNumberBox.Value);
            HarvesterOptions.SaveNodeDnsEndPoint(dnsEndPoint);
            _harvesterProcess.StartAsync();
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            _harvesterProcess.Stop();
        }
    }
}
