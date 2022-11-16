using Mmx.Gui.Win.Wpf.Common;
using Mmx.Gui.Win.Wpf.Common.Pages;

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
        }
    }
}
