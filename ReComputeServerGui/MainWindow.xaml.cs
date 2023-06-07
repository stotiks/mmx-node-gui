using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using System.Linq;

namespace ReComputeServerGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly SimpleSettingsPage settingsPage = new SimpleSettingsPage();
        private readonly RecomputePage recomputePage = new RecomputePage();

        public MainWindow() : base()
        {
            InitializeComponent();
            DataContext = this;

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
        }
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Content = settingsPage;
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;

                switch (selectedItem.Tag)
                {
                    case "RecomputePage":
                        ContentFrame.Content = recomputePage;
                        break;
                    case "SettingsPage":
                        ContentFrame.Content = settingsPage;
                        break;
                }

            }
        }
    }
}
