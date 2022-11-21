using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class Node
    {
        public static readonly string workingDirectory =
#if !DEBUG
    		Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
#else
            //@"C:\Program Files\MMX";
            @"C:\dev\mmx\MMX";
#endif
        private static string MMX_HOME_ENV = Environment.GetEnvironmentVariable("MMX_HOME");
        public static readonly string MMX_HOME = string.IsNullOrEmpty(MMX_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mmx") : MMX_HOME_ENV;
        public static readonly string configPath = Path.Combine(MMX_HOME, @"config\local");
        public static readonly string harvesterConfigPath = Path.Combine(configPath, "Harvester.json");
        public static readonly string walletConfigPath = Path.Combine(configPath, "Wallet.json");
        
        public static readonly string plotterConfigPath = Path.Combine(configPath, "Plotter.json");
        public static readonly string httpServerConfigPath = Path.Combine(configPath, "HttpServer.json");
        public static readonly string nodeFilePath = Path.Combine(configPath, "node");

        public static readonly string activateCMDPath = Path.Combine(workingDirectory, "activate.cmd");
        public static readonly string runNodeCMDPath = Path.Combine(workingDirectory, "run_node.cmd");
        public static readonly string mmxNodeEXEPath = Path.Combine(workingDirectory, "mmx_node.exe");

        public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);
        public event AsyncEventHandler<EventArgs> StartedAsync;

        public event EventHandler BeforeStarted;
        public event EventHandler BeforeStop;
        public event EventHandler Stopped;

        private Process _process;

        public static Version Version { get; set; }

        public static string VersionTag { get; set; }

        static Node()
        {
            //httpClient.DefaultRequestHeaders.Add(XApiTokenName, XApiToken);
            
            try
            {
                var productVersion = FileVersionInfo.GetVersionInfo(mmxNodeEXEPath).ProductVersion;
                Version = new Version(productVersion);
            } catch {
                Version = new Version();
            }

            VersionTag = "v" + Version;
        }

        public Task StartAsync()
        {
            return Task.Run(Start);
        }

        public void Start()
        {
            OnBeforeStarted();

            Activate();
            NodeApi.InitXToken();

            if (!NodeApi.IsRunning)
            {
                StartProcess();
            }

            var delay = 100;
            var timeout = 2000;

            while (NodeApi.IsRunning && timeout >= 0)
            {
                timeout -= delay;
                Task.Delay(delay).Wait();
            }

            Task.Run(async () => { await OnStartedAsync(); });
        }

        private static Process GetProcess(string cmd, string args = null)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDirectory;
            processStartInfo.FileName = cmd;
            processStartInfo.Arguments = args;

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = true;

            if (!Settings.Default.ShowConsole)
            {
                processStartInfo.CreateNoWindow = true;            
                
                //processStartInfo.RedirectStandardOutput = true;
                //processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardInput = false;
            } else
            {
#if !DEBUG
                processStartInfo.Arguments = "--PauseOnExit " + processStartInfo.Arguments;
#endif
            }

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;

            //process.OutputDataReceived += (sender1, args) => WriteProcessLog(args.Data);
            //process.ErrorDataReceived += (sender1, args) => WriteProcessLog(args.Data);

            process.Start();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();

            return process;
        }


        private static void Activate()
        {
            if (!Directory.Exists(configPath))
            {
                Process process = GetProcess(activateCMDPath);
                process.WaitForExit();

                var json = File.ReadAllText(Node.walletConfigPath);

                JObject walletConfig = JsonConvert.DeserializeObject<JObject>(json);
                walletConfig.Property("key_files+").Remove();
                walletConfig.Add(new JProperty("key_files", new JArray()));                

                json = JsonConvert.SerializeObject(walletConfig, Formatting.Indented);
                File.WriteAllText(Node.walletConfigPath, json);
            }
        }

        private void StartProcess()
        {
            string args = "";
            if (Settings.Default.DisableOpenCL)
            {
                args += " --Node.opencl_device -1";
            }
            _process = GetProcess(runNodeCMDPath, args);
        }

        private void WriteProcessLog(string text)
        {
            Console.WriteLine(text);
        }

        public Task StopAsync()
        {
            return Task.Run(Stop);
        }

        public void Stop()
        {
            OnBeforeStop();
            
            _ = NodeApi.Exit();

            var delay = 100;
            var timeout = 10000;

            if (_process != null)
            {
                while (!_process.HasExited && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }

                if (!_process.HasExited)
                {
                    NativeMethods.KillProcessAndChildren(_process.Id);
                }
            }
            else
            {
                while (NodeApi.IsRunning && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }
            }

            OnStop();
        }

        private void OnBeforeStarted()
        {
            BeforeStarted?.Invoke(this, EventArgs.Empty);
        }

        private async Task OnStartedAsync()
        {
            if (!(StartedAsync is null)) await StartedAsync(this, EventArgs.Empty);
        }

        private void OnBeforeStop()
        {
            BeforeStop?.Invoke(this, EventArgs.Empty);
        }

        private void OnStop()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }



    }
}
