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
        private readonly PlotterProcess plotterProcess = new PlotterProcess();
        public PlotterProcess PlotterProcess => plotterProcess;

        private string _logFileName;
        private readonly string _logFolder = Path.Combine(Node.MMX_HOME, "plotter");

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
            if (!Directory.Exists(_logFolder))
            {
                Directory.CreateDirectory(_logFolder);
            }
            _logFileName = $"plotter_{DateTime.Now:yyyyMMdd_HHmmss}.log";

            plotterProcess.Start();

        }

        private void ProcessExit(object sender, EventArgs e)
        {
            WriteLog("Process has exited.");
        }

        private void ProcessStart(object sender, EventArgs e)
        {
            LogTxt = "";
            WriteLog($"{plotterProcess.StartInfo.FileName} {plotterProcess.StartInfo.Arguments}");
        }


        private readonly object _logLock = new object();

        private void WriteLog(string value)
        {
            lock (_logLock)
            {
                var txt = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] {value}\r\n";
                LogTxt += txt;
                File.AppendAllText(Path.Combine(_logFolder, _logFileName), txt);
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
            if (FlyoutService.GetFlyout(StopButton) is Flyout f)
            {
                f.Hide();
            }

            plotterProcess.Stop();
        }

    }
}
