using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Wpf.Common.Dialogs;
using ModernWpf.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Pages
{
    /// <summary>
    /// Interaction logic for PlotterPage.xaml
    /// </summary>
    ///     
    public partial class PlotterPage
    {
        private Process process;

        private PlotterOptions _plotterOptions = new PlotterOptions();
        private string logFileName;
        private string logFolder = Path.Combine(Node.MMX_HOME, "plotter");

        public PlotterOptions PlotterOptions { get => _plotterOptions; }

        public bool PlotterIsRunning { get => process != null && !process.HasExited; }

        public PlotterPage()
        {
            InitializeComponent();
            DataContext = this;

            //NFT PLOTS DISABLED
            PlotterOptions.nftplot.Value = false;
            createPlotNFT.IsEnabled = false;


            PlotterDialog.StopButtonConfirm.Click += (s, e) =>
            {
                Flyout f = FlyoutService.GetFlyout(PlotterDialog.StopButton) as Flyout;
                if (f != null)
                {
                    f.Hide();
                }

                if (this.PlotterIsRunning)
                {
                    process.Kill();
                }
            };

            PlotterDialog.PauseButton.Click += (s, e) =>
            {
                if (this.PlotterIsRunning)
                {
                    if (!PlotterDialog.ProcessSuspended)
                    {
                        PlotterDialog.ProcessSuspended = true;
                        NativeMethods.SuspendProcess(process.Id);
                        WriteLog("Process has suspended.");
                    } else
                    {
                        PlotterDialog.ProcessSuspended = false;
                        NativeMethods.ResumeProcess(process.Id);
                        WriteLog("Process has resumed.");
                    }
                }
            };
        }

        private void ChooseFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var property = typeof(PlotterOptions).GetProperty(button.Tag as string);
            dynamic item = property.GetValue(PlotterOptions);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(item.Value) ? "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}" : item.Value;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                item.Value = PlotterOptions.FixDir(dialog.FileName);
            }
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var result = PlotterDialog.ShowAsync(ContentDialogPlacement.InPlace);

            if (!System.IO.Directory.Exists(logFolder)) 
            {
                System.IO.Directory.CreateDirectory(logFolder);
            }
            logFileName = "ploter_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Node.workingDirectory;
            processStartInfo.FileName = Path.Combine(Node.workingDirectory, PlotterOptions.PlotterExe);
            processStartInfo.Arguments = PlotterOptions.PlotterArguments;

#if DEBUG
            processStartInfo.FileName = "ping";
            processStartInfo.Arguments = "google.com -n 30";
#endif

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = true;

            //if (!Settings.Default.ShowConsole)
            {
                processStartInfo.CreateNoWindow = true;

                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardInput = false;
            }

            process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;

            process.OutputDataReceived += (sender1, args) => WriteLog(args.Data);
            process.ErrorDataReceived += (sender1, args) => WriteLog(args.Data);

            process.Exited += (sender1, args) =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    OnProcessExit();
                }));
            };           

            process.Start();
            process.PriorityClass = (ProcessPriorityClass)PlotterOptions.priority.Value;

            OnProcessStart();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();

        }

        private void OnProcessExit()
        {
            PlotterDialog.CloseButton.IsEnabled = true;
            PlotterDialog.PauseButton.IsEnabled = false;
            PlotterDialog.StopButton.IsEnabled = false;
            WriteLog("Process has exited.");
        }

        private void OnProcessStart()
        {
            PlotterDialog.LogTxt = "";
            WriteLog(string.Format("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments));

            PlotterDialog.CloseButton.IsEnabled = false;
            PlotterDialog.PauseButton.IsEnabled = true;
            PlotterDialog.StopButton.IsEnabled = true;
            PlotterDialog.ProcessSuspended = false;
        }

        private readonly object logLock = new object();
        internal void WriteLog(string value)
        {
            lock (logLock)
            {
                var txt = string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), value);
                PlotterDialog.LogTxt += txt;
                File.AppendAllText(Path.Combine(logFolder, logFileName), txt);
            }
        }

    }
}
