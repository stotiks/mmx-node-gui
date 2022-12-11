using Mmx.Gui.Win.Common;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for LightModePage.xaml
    /// </summary>
    public partial class LightModePage
    {
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;
        public LightModePage()
        {
            InitializeComponent();
            DataContext = this;

            Node.OutputDataReceived += Logger.OutputDataReceived;
            Node.ErrorDataReceived += Logger.ErrorDataReceived;
        }

        private void FullModeButton_Click(object sender, RoutedEventArgs e)
        {
            FullModeButton.IsEnabled = false;
            var window = (MainWindow)Application.Current.MainWindow;
            window.InitializeCef();
        }

        public void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

    }
}
