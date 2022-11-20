using System.Windows;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for LightModePage.xaml
    /// </summary>
    public partial class LightModePage
    {
        public LightModePage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void FullModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FullModeButton.IsEnabled = false;
            var window = (MainWindow)Application.Current.MainWindow;
            window.InitializeCef();
        }
    }
}
