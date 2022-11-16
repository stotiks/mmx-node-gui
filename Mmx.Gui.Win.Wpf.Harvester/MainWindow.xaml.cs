using Mmx.Gui.Win.Wpf.Common;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using Open.Nat;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WpfMainWindow
    {
        private HarvesterPage harvesterPage = new HarvesterPage();
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Content = harvesterPage;
            Task.Run( async () =>
            {
                var mapping = await GetMapping();
                Console.WriteLine(mapping);
            });
        }

        public async Task<Mapping> GetMapping()
        {
            Mapping result = null;

            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(10000);
            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
            var mappings = await device.GetAllMappingsAsync();
            foreach (var mapping in mappings)
            {                
                if(mapping.Description == "MMX Node")
                {
                    result = mapping;
                    break;
                }
            }

            return result;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectPage":
                    contentFrame.Content = null;
                    break;
                case "HarvesterPage":
                    contentFrame.Content = harvesterPage;
                    break;
            }

        }

    }
}
