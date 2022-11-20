using Mmx.Gui.Win.Common;
using System;
using System.Net;
using System.Windows;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage
    {
        public HarvesterOptions HarvesterOptions => HarvesterOptions.Instance;

        public ConnectionPage()
        {
            InitializeComponent();
            DataContext = this;
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
        }


    }
}
