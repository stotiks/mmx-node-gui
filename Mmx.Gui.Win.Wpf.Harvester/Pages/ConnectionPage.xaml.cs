using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Plotter;
using Open.Nat;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage : Page
    {
        public HarversterOptions HarversterOptions { get => HarversterOptions.Instance; }

        public ConnectionPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void DetectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionGroup.IsEnabled = false;
            HarversterOptions.Detect().ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ConnectionGroup.IsEnabled = true;
                }));
            });

            }


    }
}
