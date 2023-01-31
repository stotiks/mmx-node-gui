using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Harvester;
using Mmx.Gui.Win.Wpf.Common.Pages;
using Mmx.Gui.Win.Wpf.Harvester.Pages;
using ModernWpf.Controls;
using System.Linq;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly HarvesterProcess harvesterProcess = new HarvesterProcess();
        private readonly HarvesterPage _harvesterPage;
        private readonly ConnectionPage _connectionPage;

        public MainWindow()
        {
            _harvesterPage = new HarvesterPage(harvesterProcess);

            InitializeComponent();

            _connectionPage = new ConnectionPage(harvesterProcess);
            ContentFrame.Content = _connectionPage;

            BeforeClose += async (o, e) =>
            {
                await harvesterProcess.StopAsync();
            };

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectionPage":
                    ContentFrame.Content = _connectionPage;
                    break;
                case "HarvesterPage":
                    ContentFrame.Content = _harvesterPage;
                    break;
            }

        }
    }
}
