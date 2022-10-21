using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Open.Nat;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class Node
    {
        //private int NetworkPort = 11337; // mainnet
        private int NetworkPort = 12338; // testnet8

        public static string workingDirectory =
#if !DEBUG
    		Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
#else
            //@"C:\Program Files\MMX";
            @"C:\dev\mmx\MMX";
#endif
        private static string MMX_HOME_ENV = Environment.GetEnvironmentVariable("MMX_HOME");
        public static string MMX_HOME = string.IsNullOrEmpty(MMX_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mmx") : MMX_HOME_ENV;
        public static string configPath = Path.Combine(MMX_HOME, @"config\local");
        public static string harvesterConfigPath = Path.Combine(configPath, "Harvester.json");
        public static string walletConfigPath = Path.Combine(configPath, "Wallet.json");
        
        public static string plotterConfigPath = Path.Combine(configPath, "Plotter.json");
        public static string httpServerConfigPath = Path.Combine(configPath, "HttpServer.json");

        public static string activateCMDPath = Path.Combine(workingDirectory, "activate.cmd");
        public static string runNodeCMDPath = Path.Combine(workingDirectory, "run_node.cmd");
        public static string mmxNodeEXEPath = Path.Combine(workingDirectory, "mmx_node.exe");

        public static readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        public static readonly Uri guiUri = new Uri(baseUri, "/gui/");

        public static readonly Uri apiUri = new Uri(baseUri, "/api/");
        public static readonly Uri wapiUri = new Uri(baseUri, "/wapi/");

        private static readonly Uri sessionUri = new Uri(baseUri, "/server/session");

        public static readonly Uri dummyUri = new Uri(baseUri, "/dummyUri/");


        private static string logoutJS = GetResource("logout.js");
        private static string waitStartJS = GetResource("waitStart.js");

        private static string loadingHtmlTemplate = GetResource("loading.html");
        public static string loadingHtml
        {
            get => loadingHtmlTemplate.Replace("#background-color", !Properties.Settings.IsDarkTheme ? "#f2f2f2" : "#121212");
        }

        private static string jsString = "//javascript";
        public static string logoutHtml
        {
            get => loadingHtml.Replace(jsString, logoutJS);
        }
        public static string waitStartHtml
        {
            get => loadingHtml.Replace(jsString, waitStartJS);
        }

        private static string _xApiToken = GetRandomHexNumber(64);
        public static readonly string XApiTokenName = "x-api-token";

        public event EventHandler BeforeStarted;
        public event EventHandler Started;
        public event EventHandler BeforeStop;
        public event EventHandler Stoped;


        private static readonly HttpClient httpClient = new HttpClient();

        private Process process;

        public static Version Version { get; set; }

        public static string VersionTag { get; set; }

        public static string ProductName { get; set; }

        static Node()
        {
            httpClient.DefaultRequestHeaders.Add(XApiTokenName, XApiToken);
            
            try
            {
                var productVersion = FileVersionInfo.GetVersionInfo(mmxNodeEXEPath).ProductVersion;
                ProductName = FileVersionInfo.GetVersionInfo(mmxNodeEXEPath).ProductName;
                Version = new Version(productVersion);
            } catch {
                Version = new Version();
            }

            VersionTag = "v" + Version.ToString();
        }

        public Node()
        {
            BeforeStarted += (sender, args) =>
            {
                if (Settings.Default.UseUPnP)
                {
                    NetworkChange.NetworkAvailabilityChanged += (s, a) => UPnPCreatePortMap();
                    NetworkChange.NetworkAddressChanged += (s, a) => UPnPCreatePortMap();
                    UPnPCreatePortMap();
                }
            };

            Stoped += (sender, args) => Task.Run(() => UPnPDeletePortMapAsync());
        }

        private void UPnPCreatePortMap()
        {
            Task.Run(() => UPnPCreatePortMapAsync());
        }

        private async Task UPnPCreatePortMapAsync()
        {
            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(10000);
            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

            await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, NetworkPort, NetworkPort, ProductName));
        }

        private async Task UPnPDeletePortMapAsync()
        {
            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(10000);
            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

            await device.DeletePortMapAsync(new Mapping(Protocol.Tcp, NetworkPort, NetworkPort));
        }

        public static Task RemovePlotDirTask(string dirName)
        {
            return Task.Run(async () =>
            {
                dynamic data = new JObject();
                data.path = dirName;

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                var res = await httpClient.PostAsync(baseUri + "api/harvester/rem_plot_dir", byteContent);
                var res2 = await httpClient.GetAsync(baseUri + "api/harvester/reload");
            });

        }

        public static Task AddPlotDirTask(string dirName)
        {
            return Task.Run(async () =>
            {
                dynamic data = new JObject();
                data.path = dirName;

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                var res = await httpClient.PostAsync(baseUri + "api/harvester/add_plot_dir", byteContent);
                var res2 = await httpClient.GetAsync(baseUri + "api/harvester/reload");
            });
        }

        public static Task ReloadHarvester()
        {
            return Task.Run(async () =>
            {
                var res2 = await httpClient.GetAsync(baseUri + "api/harvester/reload");
            });
        }


        public static bool IsRunning
        {
            get
            {
                var task = Task.Run(CheckRunning);
                task.Wait();
                return task.Result;
            }
        }

        private static async Task<bool> CheckRunning()
        {
            try
            {
                var result = await httpClient.GetAsync(sessionUri);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Node not running");
            }

            return false;
        }


        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }

        public void Start()
        {
            OnBeforeStarted();

            Activate();
            InitXToken();

            if (!IsRunning)
            {
                StartProcess();
            }

            var delay = 100;
            var timeout = 2000;

            while (IsRunning && timeout >= 0)
            {
                timeout -= delay;
                Task.Delay(delay).Wait();
            }

            OnStart();
        }

        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            Random random = new Random();
            random.NextBytes(buffer);
            string result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        public static string XApiToken
        {
            get
            {
                return _xApiToken;
            }

            set
            {
                _xApiToken = value;
                httpClient.DefaultRequestHeaders.Remove(XApiTokenName);
                httpClient.DefaultRequestHeaders.Add(XApiTokenName, _xApiToken);
            }
        }

        static private void InitXToken()
        {
            string json = "{}";
            try
            {
                json = File.ReadAllText(Node.httpServerConfigPath);
            }
            catch
            {
                Console.WriteLine(@"config not found");
            }

            JObject httpServerConfig = JsonConvert.DeserializeObject<JObject>(json);
            var tokenMap = httpServerConfig["token_map"] as JObject;
            JProperty firstAdminToken = null;

            if (tokenMap != null)
            {
                foreach (JProperty prop in tokenMap.Properties())
                {
                    if (prop.Value.ToString() == "ADMIN")
                    {
                        firstAdminToken = prop;
                        break;
                    }
                }
            }

            if (IsRunning)
            {
                if (firstAdminToken != null)
                {
                    XApiToken = firstAdminToken.Name;
                }
                else
                {
                    //throw new Exception("Can not get XApiToken");
                }
            }
            else
            {
                var property = new JProperty(XApiToken, "ADMIN");

                if (firstAdminToken == null)
                {
                    if (tokenMap == null)
                    {
                        var jObject = new JObject();
                        jObject.Add(property);
                        httpServerConfig["token_map"] = jObject;
                    }
                    else
                    {
                        tokenMap.Add(property);
                    }

                }
                else
                {
                    firstAdminToken.Replace(property);
                }

                json = JsonConvert.SerializeObject(httpServerConfig, Formatting.Indented);
                File.WriteAllText(Node.httpServerConfigPath, json);
            }
        }


        private Process GetProcess(string cmd, string args = null)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDirectory;
            processStartInfo.FileName = cmd;
            if(!String.IsNullOrEmpty(args))
            {
                processStartInfo.Arguments = args;
            }

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

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;

            //process.OutputDataReceived += (sender1, args) => WriteProcessLog(args.Data);
            //process.ErrorDataReceived += (sender1, args) => WriteProcessLog(args.Data);

            process.Start();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();

            return process;
        }


        internal void Activate()
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
            process = GetProcess(runNodeCMDPath, args);
        }

        private void WriteProcessLog(string text)
        {
            Console.WriteLine(text);
        }

        public Task StopAsync()
        {
            return Task.Run(() => Stop());
        }

        public void Stop()
        {
            OnBeforeStop();

            var delay = 100;
            var timeout = 10000;

            if (process != null)
            {
                while (!process.HasExited && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }

                if (!process.HasExited)
                {
                    //process.Kill();
                    KillProcessAndChildren(process.Id);
                }
            }
            else
            {
                while (IsRunning && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }
            }

            //processStarted = false;

            OnStop();
        }

        private void OnBeforeStarted()
        {
            BeforeStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnStart()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        private void OnBeforeStop()
        {
            BeforeStop?.Invoke(this, EventArgs.Empty);
        }

        private void OnStop()
        {
            Stoped?.Invoke(this, EventArgs.Empty);
        }

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }

            try
            {
                Process process = Process.GetProcessById(pid);
                process.Kill();
                process.WaitForExit();
            }
            catch (Exception)
            {
                // Process already exited.
            }
        }

        private static string GetResource(string resName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resName));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
