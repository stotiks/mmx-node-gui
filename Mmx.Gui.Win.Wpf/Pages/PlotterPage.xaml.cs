using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Wpf.Dialogs;
using ModernWpf.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for PlotterPage.xaml
    /// </summary>
    ///     
    public partial class PlotterPage
    {
        Process process;

        private PlotterOptions _plotterOptions = new PlotterOptions();
        public PlotterOptions PlotterOptions { get => _plotterOptions; }

        public PlotterPage()
        {
            InitializeComponent();
            DataContext = this;

            PlotterDialog.StopButtonConfirm.Click += (s, e) =>
            {
                Flyout f = FlyoutService.GetFlyout(PlotterDialog.StopButton) as Flyout;
                if (f != null)
                {
                    f.Hide();
                }

                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
            };

            PlotterDialog.PauseButton.Click += (s, e) =>
            {
                if (process != null && !process.HasExited)
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
            var result = PlotterDialog.ShowAsync(ModernWpf.Controls.ContentDialogPlacement.InPlace);

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Node.workingDirectory;
            processStartInfo.FileName = Path.Combine(Node.workingDirectory, PlotterOptions.PlotterExe);
            processStartInfo.Arguments = PlotterOptions.PlotterArguments;

            //processStartInfo.FileName = "ping";
            //processStartInfo.Arguments = "google.com -n 20";

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
                Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    OnProcessExit();
                }));
            };

            OnProcessStart();
            process.Start();

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

        internal void WriteLog(string value)
        {
            var txt = string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), value);
            PlotterDialog.LogTxt += txt;
        }

    }
}
