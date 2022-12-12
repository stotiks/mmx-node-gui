using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common
{
    public class ProcessBase: INotifyPropertyChanged
    {

        private bool _isRunning;

        private bool _processSuspended;
        protected Process process = new Process();

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
        public ProcessStartInfo StartInfo => process.StartInfo;

        public bool Suspended
        {
            get => _processSuspended;

            private set
            {
                _processSuspended = value;
                NotifyPropertyChanged();
            }
        }

        public event EventHandler BeforeStart;
        public event DataReceivedEventHandler ErrorDataReceived;
        public event DataReceivedEventHandler OutputDataReceived;
        public event EventHandler ProcessExit;
        public event EventHandler ProcessStart;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Kill()
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

        public void Resume()
        {
            if (!process.HasExited)
            {
                NativeMethods.ResumeProcess(process.Id);
                Suspended = false;
            }
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

        public void Stop()
        {
            Kill();
        }

        public void Suspend()
        {
            if (!process.HasExited)
            {
                NativeMethods.SuspendProcess(process.Id);
                Suspended = true;
            }
        }
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnBeforeStart()
        {
            BeforeStart?.Invoke(this, null);
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(sender, e);
        }

        protected void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e);
        }

        protected void OnProcessExit(object sender, EventArgs e)
        {
            ProcessExit?.Invoke(this, null);
            IsRunning = false;
        }
        protected void OnProcessStart()
        {
            IsRunning = true;
            Suspended = false;            

            ProcessStart?.Invoke(this, null);
        }
    }
}