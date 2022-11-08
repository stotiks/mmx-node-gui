using Mmx.Gui.Win.Common;
using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Mmx.Gui.Win.Wpf.Common.Dialogs
{
    /// <summary>
    /// Interaction logic for PlotterDialog.xaml
    /// </summary>
    public partial class PlotterDialog : INotifyPropertyChanged
    {
        private Process process;
        private string logFileName;
        private string logFolder = Path.Combine(Node.MMX_HOME, "plotter");

        public PlotterDialog()
        {
            InitializeComponent();
            DataContext = this;

            StopButtonConfirm.Click += (s, e) =>
            {
                Flyout f = FlyoutService.GetFlyout(StopButton) as Flyout;
                if (f != null)
                {
                    f.Hide();
                }

                if (this.PlotterIsRunning)
                {
                    process.Kill();
                }
            };

            PauseButton.Click += (s, e) =>
            {
                if (this.PlotterIsRunning)
                {
                    if (!ProcessSuspended)
                    {
                        ProcessSuspended = true;
                        NativeMethods.SuspendProcess(process.Id);
                        WriteLog("Process has suspended.");
                    }
                    else
                    {
                        ProcessSuspended = false;
                        NativeMethods.ResumeProcess(process.Id);
                        WriteLog("Process has resumed.");
                    }
                }
            };

            PropertyChanged += (s, e) => 
            {
                switch (e.PropertyName)
                {
                    case "ProcessSuspended":
                        if (_processSuspended)
                        {
                            PauseButton.Content = Properties.Resources.Plotter_Resume;
                        }
                        else
                        {
                            PauseButton.Content = Properties.Resources.Plotter_Pause;
                        }
                        break;
                }
            };
        }


        public void StartPlotter()
        {
            if (!System.IO.Directory.Exists(logFolder))
            {
                System.IO.Directory.CreateDirectory(logFolder);
            }
            logFileName = "ploter_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Node.workingDirectory;
            processStartInfo.FileName = Path.Combine(Node.workingDirectory, PlotterOptions.Instance.PlotterExe);
            processStartInfo.Arguments = PlotterOptions.Instance.PlotterArguments;

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
            process.PriorityClass = (ProcessPriorityClass)PlotterOptions.Instance.priority.Value;
            OnProcessStart();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
        }

        private void OnProcessExit()
        {
            PlotterIsRunning = false;
            WriteLog("Process has exited.");
        }

        private void OnProcessStart()
        {
            LogTxt = "";
            WriteLog(string.Format("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments));

            PlotterIsRunning = true;
        }

        private readonly object logLock = new object();
        internal void WriteLog(string value)
        {
            lock (logLock)
            {
                var txt = string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), value);
                LogTxt += txt;
                File.AppendAllText(Path.Combine(logFolder, logFileName), txt);
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

        private bool _processSuspended = false;
        public bool ProcessSuspended
        {
            get => _processSuspended;

            set
            {
                _processSuspended = value;
                NotifyPropertyChanged();
            }
        }

        private bool _plotterIsRunning = false;
        public bool PlotterIsRunning
        {
            get => _plotterIsRunning;

            set
            {
                _plotterIsRunning = value;
                NotifyPropertyChanged();

                if (value)
                {
                    ProcessSuspended = false;
                }                
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("FullDialogSizing") is VisualState fullDialogSizing)
            {
                if (fullDialogSizing.Storyboard != null)
                {
                    var anim = new ObjectAnimationUsingKeyFrames
                    {
                        KeyFrames = { new DiscreteObjectKeyFrame(HorizontalAlignment.Stretch, TimeSpan.Zero) }
                    };
                    Storyboard.SetTargetName(anim, "BackgroundElement");
                    Storyboard.SetTargetProperty(anim, new PropertyPath(HorizontalAlignmentProperty));
                    fullDialogSizing.Storyboard.Children.Add(anim);
                }
            }
            
            base.OnApplyTemplate();
        }

        private void TextBoxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxLog.ScrollToEnd();
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                args.Cancel = true;
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
