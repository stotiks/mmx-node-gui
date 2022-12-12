using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Harvester;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage
    {
        public HarvesterOptions HarvesterOptions => HarvesterOptions.Instance;
        private HarvesterProcess _harvesterProcess;
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        public ConnectionPage(HarvesterProcess harvesterProcess)
        {
            InitializeComponent();
            DataContext = this;

            _harvesterProcess = harvesterProcess;
            _harvesterProcess.OutputDataReceived += _logger.OutputDataReceived;
            _harvesterProcess.ErrorDataReceived += _logger.ErrorDataReceived;
        }

        private void DetectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionGroup.IsEnabled = false;
            HarvesterOptions.DetectNodeIP().ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ConnectionGroup.IsEnabled = true;
                }));
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
    }
}
