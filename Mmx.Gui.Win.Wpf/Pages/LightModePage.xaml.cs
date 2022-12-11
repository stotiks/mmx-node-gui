using Mmx.Gui.Win.Common;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for LightModePage.xaml
    /// </summary>
    public partial class LightModePage : INotifyPropertyChanged
    {

        public LightModePage()
        {
            InitializeComponent();
            DataContext = this;

            Node.OutputDataReceived += WriteLog;
            Node.ErrorDataReceived += WriteLog;
        }

        private readonly object _logLock = new object();
        Queue myQueue = new Queue();
        private void WriteLog(object sender, DataReceivedEventArgs args)
        {
            lock (_logLock)
            {
                var txt = args.Data;
                myQueue.Enqueue(txt);
                if (myQueue.Count > 100)
                {
                    myQueue.Dequeue();
                }
                LogTxt = string.Join("\r\n", myQueue.ToArray());
            }
        }

        private string _logTxt;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LogTxt
        {
            get => _logTxt;
            set
            {
                _logTxt = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
