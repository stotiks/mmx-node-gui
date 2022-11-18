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
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Node.workingDirectory,
                FileName = Path.Combine(Node.workingDirectory, PlotterOptions.Instance.PlotterExe),
                Arguments = PlotterOptions.Instance.PlotterArguments,

                UseShellExecute = false,
                CreateNoWindow = true,

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = false
        };

#if DEBUG
            //processStartInfo.FileName = "ping";
            //processStartInfo.Arguments = "google.com -n 30";
#endif
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
            CleanFs(); 
            IsRunning = false;
            ProcessExit?.Invoke(this, null);
        }

        private void CleanFs()
        {
            var deletedCount = 0;

            deletedCount += DeleteTempFiles(PlotterOptions.Instance.finaldir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir2.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.stagedir.Value, _currentPlotName);

            if (deletedCount > 0)
            {
                OnOutputDataReceived(this, new CustomDataReceivedEventArgs(""));
                OnOutputDataReceived(this, new CustomDataReceivedEventArgs($"Temp files deleted: {deletedCount}"));
            }
        }

        private int DeleteTempFiles(string dir, string plotName)
        {
            var result = 0;
            var reg = new Regex($@"^({plotName})");
            if (!string.IsNullOrEmpty(plotName) && !string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
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

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var x = new CustomDataReceivedEventArgs(e);
            OnOutputDataReceived(sender, x);
        }

        private string _currentPlotName;
        private readonly Regex _plotNameRegex = new Regex(@"^Plot Name: (plot-.*)");

        private void OnOutputDataReceived(object sender, CustomDataReceivedEventArgs e)
        {
            var str = e.Data;
            
            if(!string.IsNullOrEmpty(str))
            {
                var r = _plotNameRegex.Match(str);
                if(r.Success)
                {
                    _currentPlotName = r.Groups[1].Value;
                    //Console.WriteLine(currentPlotName);
                }
                
            }

            OutputDataReceived?.Invoke(sender, e);
        }
        public event DataReceivedEventHandler ErrorDataReceived;

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
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

        private bool _processSuspended;
        public bool Suspended
        {
            get => _processSuspended;

            private set
            {
                _processSuspended = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;

            private set
            {
                _isRunning = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TryKill));

                if (value)
                {
                    Suspended = false;
                }
            }
        }

        public ProcessStartInfo StartInfo => process.StartInfo;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private int _stopTry;
        private int StopTry
        {
            get => _stopTry;
            set
            {
                _stopTry = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TryKill));
            }            
        }
        public bool TryKill => IsRunning && StopTry > 0;

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
