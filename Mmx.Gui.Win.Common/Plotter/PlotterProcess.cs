using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterProcess : INotifyPropertyChanged
    {
        public delegate void PlotterProcessEventHandler(object sender, EventArgs args);

        private Process process;

        public void Start()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Node.workingDirectory;
            processStartInfo.FileName = Path.Combine(Node.workingDirectory, PlotterOptions.Instance.PlotterExe);
            processStartInfo.Arguments = PlotterOptions.Instance.PlotterArguments;

#if DEBUG
            //processStartInfo.FileName = "ping";
            //processStartInfo.Arguments = "google.com -n 30";
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

            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;

            process.Exited += OnProcessExit;

            NativeMethods.SetConsoleCtrlHandler(null, false);
            process.Start();
            process.PriorityClass = (ProcessPriorityClass)PlotterOptions.Instance.priority.Value;
            OnProcessStart();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
        }

        public event PlotterProcessEventHandler ProcessExit;
        private void OnProcessExit(object sender, EventArgs e)
        {
            NativeMethods.SetConsoleCtrlHandler(null, false);
            CleanFS(); 
            IsRunning = false;
            ProcessExit?.Invoke(this, null);
        }

        private void CleanFS()
        {
            var deletedCount = 0;

            deletedCount += DeleteTempFiles(PlotterOptions.Instance.finaldir.Value);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir.Value);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir2.Value);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.stagedir.Value);

            if (deletedCount > 0)
            {
                OnOutputDataReceived(this, new CustomDataReceivedEventArgs(""));
                OnOutputDataReceived(this, new CustomDataReceivedEventArgs(string.Format("Temp files deleted: {0}", deletedCount)));
            }
        }

        private int DeleteTempFiles(string dir)
        {
            var result = 0;
            if (!string.IsNullOrEmpty(currentPlotName) && !string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
                Regex reg = new Regex(string.Format(@"^({0})", currentPlotName));
                var files = Directory.GetFiles(dir, "*.tmp")
                                     .Where(path => reg.IsMatch(Path.GetFileName(path)))
                                     .ToList();
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                result = files.Count;
            }

            return result;
        }

        public delegate void CustomEventHandler(object o, CustomDataReceivedEventArgs e);
        public event CustomEventHandler OutputDataReceived;
        void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var x = new CustomDataReceivedEventArgs(e);
            OnOutputDataReceived(sender, x);
        }
        string currentPlotName = null;
        Regex plotNameRegex = new Regex(@"^Plot Name: (plot-.*)");
        void OnOutputDataReceived(object sender, CustomDataReceivedEventArgs e)
        {
            var str = e.Data;
            
            if(!string.IsNullOrEmpty(str))
            {
                var r = plotNameRegex.Match(str);
                if(r.Success)
                {
                    currentPlotName = r.Groups[1].Value;
                    //Console.WriteLine(currentPlotName);
                }
                
            }

            OutputDataReceived?.Invoke(sender, e);
        }
        public event DataReceivedEventHandler ErrorDataReceived;
        void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(this, e);
        }

        public event PlotterProcessEventHandler ProcessStart;
        private void OnProcessStart()
        {
            IsRunning = true;
            StopTry = 0;

            ProcessStart?.Invoke(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _processSuspended = false;
        public bool Suspended
        {
            get => _processSuspended;

            set
            {
                _processSuspended = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning;

            set
            {
                _isRunning = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("TryKill");

                if (value)
                {
                    Suspended = false;
                }
            }
        }

        public ProcessStartInfo StartInfo { get => process.StartInfo; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Suspend()
        {
            if (IsRunning)
            {
                NativeMethods.SuspendProcess(process.Id);
                Suspended = true;
            }
        }

        public void Resume()
        {
            if (IsRunning)
            {
                NativeMethods.ResumeProcess(process.Id);
                Suspended = false;
            }
        }

        private int _stopTry = 0;
        private int StopTry
        {
            get => _stopTry;
            set
            {
                _stopTry = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("TryKill");
            }            
        }
        public bool TryKill
        {
            get => IsRunning && StopTry > 0;
        }

        public void Stop()
        {
            if (IsRunning)
            {
                if (StopTry == 0)
                {
                    StopTry++;
                    NativeMethods.StopProgramByAttachingToItsConsoleAndIssuingCtrlCEvent(process);
                }
                else
                {
                    Kill();
                }
            }

        }

        public void Kill()
        {
            process.Kill();
        }
    }
}
