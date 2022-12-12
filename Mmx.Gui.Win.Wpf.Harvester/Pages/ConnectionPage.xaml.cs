using Mmx.Gui.Win.Common;
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
        private Mmx.Gui.Win.Common.Harvester _harvester;
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        public ConnectionPage(Mmx.Gui.Win.Common.Harvester harvester)
        {
            InitializeComponent();
            DataContext = this;

            _harvester = harvester;
            _harvester.OutputDataReceived += _logger.OutputDataReceived;
            _harvester.ErrorDataReceived += _logger.ErrorDataReceived;
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
            _harvester.StartAsync();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            //throw new Exception();
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }
    }
}
