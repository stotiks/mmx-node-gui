using Mmx.Gui.Win.Wpf.Common;
using Mmx.Gui.Win.Wpf.Common.Pages;
using Mmx.Gui.Win.Wpf.Harvester.Pages;
using ModernWpf.Controls;
using System.Linq;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WpfMainWindow
    {
        private HarvesterPage harvesterPage = new HarvesterPage();
        private ConnectionPage connectionPage = new ConnectionPage();
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Content = connectionPage;

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectionPage":
                    contentFrame.Content = connectionPage;
                    break;
                case "HarvesterPage":
                    contentFrame.Content = harvesterPage;
                    break;
            }

        }

    }
}
