using Mmx.Gui.Win.Common;
using System.Windows;
using System.Windows.Controls;

namespace PlotSincGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        private readonly PlotSincProcess _plotSincProcess = new PlotSincProcess();
        public PlotSincProcess PlotSincProcess => _plotSincProcess;

        public MainWindow() : base()
        {
            InitializeComponent();
            DataContext = this;

            PlotSincProcess.OutputDataReceived += _logger.OutputDataReceived;
            PlotSincProcess.ErrorDataReceived += _logger.ErrorDataReceived;

            BeforeClose += async (o, e) =>
            {
                await PlotSincProcess.StopAsync();
            };
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlotSincProcess.StartAsync();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            PlotSincProcess.Stop();
        }

    }
}
