using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Wpf.Common;
using System.Windows;
using System.Windows.Controls;

namespace ReComputeServerGui
{
    /// <summary>
    /// Interaction logic for RecomputePage.xaml
    /// </summary>
    public partial class RecomputePage : Page
    {
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        private readonly RecomputeProcess _recomputeProcess = new RecomputeProcess();
        public RecomputeProcess RecomputeProcess => _recomputeProcess;

        public RecomputePage()
        {
            InitializeComponent();
            DataContext = this;

            RecomputeProcess.OutputDataReceived += _logger.OutputDataReceived;
            RecomputeProcess.ErrorDataReceived += _logger.ErrorDataReceived;

            this.Loaded += (s, ev) =>
            {
                WpfMainWindow mainWindow = Application.Current.MainWindow as WpfMainWindow;
                mainWindow.BeforeClose += async (o, e) =>
                {
                    await RecomputeProcess.StopAsync();
                };
            };
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RecomputeProcess.StartAsync();
        }
        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await RecomputeProcess.StopAsync();
        }
    }
}
