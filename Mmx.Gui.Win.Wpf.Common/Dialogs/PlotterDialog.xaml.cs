using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Plotter;
using ModernWpf.Controls;
using System;
using System.ComponentModel;
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
        private PlotterProcess plotterProcess = new PlotterProcess();
        public PlotterProcess PlotterProcess { get => plotterProcess; }

        private string logFileName;
        private string logFolder = Path.Combine(Node.MMX_HOME, "plotter");

        public PlotterDialog()
        {
            InitializeComponent();
            DataContext = this;

            plotterProcess.ProcessStart += ProcessStart;
            plotterProcess.ProcessExit += ProcessExit;
            plotterProcess.OutputDataReceived += (sender, args) => WriteLog(args.Data);
            plotterProcess.ErrorDataReceived += (sender, args) => WriteLog(args.Data);
            
        }


        public void StartPlotter()
        {
            if (!System.IO.Directory.Exists(logFolder))
            {
                System.IO.Directory.CreateDirectory(logFolder);
            }
            logFileName = "ploter_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";

            plotterProcess.Start();

        }

        private void ProcessExit(object sender, EventArgs e)
        {
            WriteLog("Process has exited.");
        }

        private void ProcessStart(object sender, EventArgs e)
        {
            LogTxt = "";
            WriteLog(string.Format("{0} {1}", plotterProcess.StartInfo.FileName, plotterProcess.StartInfo.Arguments));
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (plotterProcess.IsRunning)
            {
                if (!plotterProcess.Suspended)
                {
                    plotterProcess.Suspend();
                    PauseButton.Content = Properties.Resources.Plotter_Resume;
                    WriteLog("Process has suspended.");                    
                }
                else
                {
                    plotterProcess.Resume();
                    PauseButton.Content = Properties.Resources.Plotter_Pause;
                    WriteLog("Process has resumed.");                    
                }
            }
        }

        private void StopButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            Flyout f = FlyoutService.GetFlyout(StopButton) as Flyout;
            if (f != null)
            {
                f.Hide();
            }

            plotterProcess.Stop();
        }

        private void KillButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            Flyout f = FlyoutService.GetFlyout(KillButton) as Flyout;
            if (f != null)
            {
                f.Hide();
            }

            plotterProcess.Kill();
        }
    }
}
