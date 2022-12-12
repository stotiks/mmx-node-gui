using Mmx.Gui.Win.Common.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public abstract class NodeBase
    {
        public static readonly string workingDirectory =
#if !DEBUG
    		Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
#else
            //@"C:\Program Files\MMX";
            @"C:\dev\mmx\MMX";
#endif
        private static readonly string MMX_HOME_ENV = Environment.GetEnvironmentVariable("MMX_HOME");
        public static readonly string MMX_HOME = string.IsNullOrEmpty(MMX_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mmx") : MMX_HOME_ENV;
        public static readonly string configPath = Path.Combine(MMX_HOME, @"config\local");
        public static readonly string harvesterConfigPath = Path.Combine(configPath, "Harvester.json");
        public static readonly string walletConfigPath = Path.Combine(configPath, "Wallet.json");

        public static readonly string plotterConfigPath = Path.Combine(configPath, "Plotter.json");
        public static readonly string httpServerConfigPath = Path.Combine(configPath, "HttpServer.json");
        public static readonly string nodeFilePath = Path.Combine(configPath, "node");

        public static readonly string activateCMDPath = Path.Combine(workingDirectory, "activate.cmd");
        public static readonly string runNodeCMDPath = Path.Combine(workingDirectory, "run_node.cmd");
        public static readonly string runHarvesterCMDPath = Path.Combine(workingDirectory, "run_harvester.cmd");
        private static readonly string mmxNodeEXEPath = Path.Combine(workingDirectory, "mmx_node.exe");


        protected ProcessBase _process;

        public static Version Version { get; private set; }

        public static string VersionTag => "v" + Version;

        public event DataReceivedEventHandler ErrorDataReceived;
        public event DataReceivedEventHandler OutputDataReceived;

        public event EventHandler BeforeStarted;
        public event EventHandler BeforeStop;

        public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);
        public event AsyncEventHandler<EventArgs> StartedAsync;
        public event EventHandler Stopped;

        static NodeBase()
        {
            try
            {
                var productVersion = FileVersionInfo.GetVersionInfo(mmxNodeEXEPath).ProductVersion;
                Version = new Version(productVersion);
            }
            catch
            {
                Version = new Version();
            }
        }

        protected ProcessBase GetProcess(string cmd, string args = null)
        {
//            var processStartInfo = new ProcessStartInfo
//            {
//                WorkingDirectory = workingDirectory,
//                FileName = cmd,
//                Arguments = args,

//                UseShellExecute = false
//            };
//            //processStartInfo.ErrorDialog = true;

//            if (!Settings.Default.ShowConsole)
//            {
//                processStartInfo.CreateNoWindow = true;            
                
//                processStartInfo.RedirectStandardOutput = true;
//                processStartInfo.RedirectStandardError = true;
//                processStartInfo.RedirectStandardInput = false;
//            } else
//            {
//#if !DEBUG
//                processStartInfo.Arguments = "--PauseOnExit " + processStartInfo.Arguments;
//#endif
//            }

//            var process = new Process
//            {
//                StartInfo = processStartInfo,
//                EnableRaisingEvents = true
//            };

//            process.OutputDataReceived += OutputDataReceived;
//            process.ErrorDataReceived += ErrorDataReceived;

//            process.Start();

//            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
//            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();

            var process = new ProcessBase();
            process.OutputDataReceived += OutputDataReceived;
            process.ErrorDataReceived += ErrorDataReceived;
            process.Start(cmd, args);
            return process;
        }

        public abstract void Start();

        public Task StartAsync()
        {
            return Task.Run(Start);
        }

        public abstract void Stop();

        public Task StopAsync()
        {
            return Task.Run(Stop);
        }

        protected void OnBeforeStarted()
        {
            BeforeStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void OnBeforeStop()
        {
            BeforeStop?.Invoke(this, EventArgs.Empty);
        }

        protected async Task OnStartedAsync()
        {
            if (!(StartedAsync is null)) await StartedAsync(this, EventArgs.Empty);
        }

        protected void OnStop()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}