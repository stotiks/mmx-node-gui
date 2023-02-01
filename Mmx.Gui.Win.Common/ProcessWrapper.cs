using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Node
{
    public abstract class ProcessWrapper: INotifyPropertyChanged
    {
        public event DataReceivedEventHandler ErrorDataReceived;
        public event DataReceivedEventHandler OutputDataReceived;

        private bool _isRunning;

        private bool _processSuspended;
        protected Process process;

        public Action WaitForExit => process.WaitForExit;

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
        protected static void KillByProcessName(string name)
        {
            var processes = Process.GetProcesses().Where(pr => pr.ProcessName == name);

            foreach (var process in processes)
            {
                NativeMethods.KillProcessAndChildren(process.Id);
            }
        }

        protected void Kill()
        {
            if (IsRunning)
            {
                try
                {
                    NativeMethods.KillProcessAndChildren(process.Id);
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

        public abstract void Start();

        protected void Start(ProcessStartInfo processStartInfo)
        {
            OnBeforeStart();

            processStartInfo.WorkingDirectory = NodeHelpers.workingDirectory;
            processStartInfo.UseShellExecute = false;

            if (processStartInfo.CreateNoWindow)
            {
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardInput = false;
            }

            process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;

            process.Exited += (o, e) => OnOutputDataReceived(this, "Process has exited.");
            process.Exited += OnProcessExit;

            try
            {
                OnOutputDataReceived(this, $"{process.StartInfo.FileName} {process.StartInfo.Arguments}");

                process.Start();

                if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
                if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
                OnStarted();
            }
            catch (Exception ex)
            {
                OnOutputDataReceived(this, ex.Message);
            }


        }

        public Task StartAsync()
        {
            return Task.Run(Start);
        }

        public virtual void Stop()
        {
            OnBeforeStop();
            Kill();
            OnStop();
        }

        public Task StopAsync()
        {
            return Task.Run(Stop);
        }

        public void Suspend()
        {
            if (!process.HasExited)
            {
                NativeMethods.SuspendProcess(process.Id);
                Suspended = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler BeforeStart;
        protected void OnBeforeStart()
        {
            BeforeStart?.Invoke(this, null);
        }

        protected void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(sender, e);
        }

        protected void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e);
        }
        private  static T CreateInstance<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }
        protected void OnOutputDataReceived(object sender, string msg)
        {
            OnOutputDataReceived(sender, CreateInstance<DataReceivedEventArgs>(msg));
        }

        protected event EventHandler ProcessExit;
        protected void OnProcessExit(object sender, EventArgs e)
        {
            ProcessExit?.Invoke(this, null);
            IsRunning = false;
            OnStop();
        }

        public event EventHandler Started;
        protected void OnStarted()
        {
            IsRunning = true;
            Suspended = false;            

            Started?.Invoke(this, null);
        }

        public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);
        public event AsyncEventHandler<EventArgs> StartedAsync;
        protected async Task OnStartedAsync()
        {
            if (!(StartedAsync is null)) await StartedAsync(this, EventArgs.Empty);
        }

        public event EventHandler BeforeStop;
        protected void OnBeforeStop()
        {
            BeforeStop?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Stopped;
        protected void OnStop()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}