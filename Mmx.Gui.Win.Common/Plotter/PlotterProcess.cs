using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterProcess : INotifyPropertyChanged
    {
        private Process process = new Process();
        public ProcessStartInfo StartInfo => process.StartInfo;

        public void Start()
        {
            var fileName = Path.Combine(Node.workingDirectory, PlotterOptions.Instance.PlotterExe);
            var arguments = PlotterOptions.Instance.PlotterArguments;

#if DEBUG
            //fileName = "ping";
            //arguments = "google.com -n 30";
#endif

            BeforeStart += (s,o) => 
            {
                NativeMethods.SetConsoleCtrlHandler(null, false);
            };

            ProcessStart += (s, o) =>
            {
                process.PriorityClass = (ProcessPriorityClass)PlotterOptions.Instance.priority.Value;
            };

            ProcessExit += (s, o) =>
            {
                NativeMethods.SetConsoleCtrlHandler(null, false);
                CleanFs();
            };

            Start(fileName, arguments);
        }

        public void Start(string fileName, string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Node.workingDirectory,
                FileName = fileName,
                Arguments = arguments,

                UseShellExecute = false,
                CreateNoWindow = true,

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = false
            };

            process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;

            process.Exited += OnProcessExit;
            
            OnBeforeStart();
            process.Start();            
            OnProcessStart();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler BeforeStart;
        private void OnBeforeStart()
        {
            BeforeStart?.Invoke(this, null);
        }

        public event EventHandler ProcessStart;
        private void OnProcessStart()
        {
            IsRunning = true;
            Suspended = false;
            StopTry = 0;

            ProcessStart?.Invoke(this, null);
        }

        public event EventHandler ProcessExit;

        private void OnProcessExit(object sender, EventArgs e)
        {
            ProcessExit?.Invoke(this, null);
            IsRunning = false;
        }

        public static T CreateInstance<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }

        private void CleanFs()
        {
            //OnOutputDataReceived(this, CreateInstance<DataReceivedEventArgs>(""));
            OnOutputDataReceived(this, CreateInstance<DataReceivedEventArgs>("Temp files cleaning"));

            var deletedCount = 0;

            deletedCount += DeleteTempFiles(PlotterOptions.Instance.finaldir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir2.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.stagedir.Value, _currentPlotName);

            OnOutputDataReceived(this, CreateInstance<DataReceivedEventArgs>($"Temp files deleted: {deletedCount}"));
        }

        private int DeleteTempFiles(string dir, string plotName)
        {
            var result = 0; 
            if (!string.IsNullOrEmpty(plotName) && !string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
                var reg = new Regex($@"^({plotName})");
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

        public event DataReceivedEventHandler OutputDataReceived;

        private string _currentPlotName;
        private readonly Regex _plotNameRegex = new Regex(@"^Plot Name: (plot-.*)");

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var str = e.Data;

            if (!string.IsNullOrEmpty(str))
            {
                var r = _plotNameRegex.Match(str);
                if (r.Success)
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
            ErrorDataReceived?.Invoke(sender, e);
        }

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


                if (value)
                {
                    Suspended = false;
                }
            }
        }

        public void Suspend()
        {
            if (!process.HasExited)
            {
                NativeMethods.SuspendProcess(process.Id);
                Suspended = true;
            }
        }

        public void Resume()
        {
            if (!process.HasExited)
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
            }
        }

        public void Stop()
        {
            if (StopTry++ <= 2)
            {
                NativeMethods.StopProgramByAttachingToItsConsoleAndIssuingCtrlCEvent(process);
            }
            else
            {
                Kill();
            }
        }

        public void Kill()
        {
            if (!process.HasExited)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
