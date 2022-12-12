using Mmx.Gui.Win.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for ConnectionPage.xaml
    /// </summary>
    public partial class ConnectionPage
    {
        private readonly UILogger _logger = new UILogger();
        public UILogger Logger => _logger;

        public HarvesterOptions HarvesterOptions => HarvesterOptions.Instance;

        public ConnectionPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void DetectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionGroup.IsEnabled = false;
            HarvesterOptions.DetectNodeIP().ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ConnectionGroup.IsEnabled = true;
                }));
            });
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var dnsEndPoint = new DnsEndPoint(HostTextBox.Text, (int)PortNumberBox.Value);
            HarvesterOptions.SaveNodeDnsEndPoint(dnsEndPoint);

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Node.workingDirectory,
                FileName = Path.Combine(Node.workingDirectory, Node.runHarvesterCMDPath),

                UseShellExecute = false,
                CreateNoWindow = true,

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = false
            };

#if DEBUG
            //processStartInfo.FileName = "ping";
            //processStartInfo.Arguments = "google.com -n 3";
#endif
            Process process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += Logger.OutputDataReceived;
            process.ErrorDataReceived += Logger.ErrorDataReceived;

            process.Exited += OnProcessExit;
            process.Start();
            //OnProcessStart();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            //throw new Exception();
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }
    }
}
