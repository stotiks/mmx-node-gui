using Mmx.Gui.Win.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for ConsoleLogsPage.xaml
    /// </summary>
    public partial class ConsoleLogsPage : Page
    {
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;
        public ConsoleLogsPage(Win.Common.Node.NodeProcess nodeProcess)
        {
            InitializeComponent();
            DataContext = this;

            nodeProcess.OutputDataReceived += Logger.OutputDataReceived;
            nodeProcess.ErrorDataReceived += Logger.ErrorDataReceived;
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }
    }
}
