using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMX_NODE_GUI
{
    public class Node
    {
        public static string workingDirectory =
#if !DEBUG
    		Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
#else
        	//@"C:\Program Files\MMX";
			@"C:\dev\mmx\MMX";
#endif
        private static string MMX_HOME_ENV = Environment.GetEnvironmentVariable("MMX_HOME");
        public static string MMX_HOME = string.IsNullOrEmpty(MMX_HOME_ENV) ? (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.mmx") : MMX_HOME_ENV;
        public static string configPath = MMX_HOME + @"\config\local";
        public static string harvesterConfigPath = configPath + @"\Harvester.json";
        public static string plotterConfigPath = configPath + @"\Plotter.json";
        public static string httpServerConfigPath = configPath + @"\HttpServer.json";
        
        public static string activateCMDPath = workingDirectory + @"\activate.cmd";
        public static string runNodeCMDPath = workingDirectory + @"\run_node.cmd";

        static public readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        static public readonly Uri guiUri = new Uri(baseUri, "/gui/");

        static public readonly Uri apiUri = new Uri(baseUri, "/api/");
        static public readonly Uri wapiUri = new Uri(baseUri, "/wapi/");

        static private readonly Uri sessionUri = new Uri(baseUri, "/server/session");


        private static string _xApiToken = GetRandomHexNumber(64);
        public static string XApiTokenName = "x-api-token";


        public event EventHandler BeforeStarted;

        public event EventHandler Started;
        public event EventHandler BeforeStop;
        public event EventHandler Stoped;


        private static readonly HttpClient client = new HttpClient();
        private Process process;
        private bool processStarted = false;


        static Node()
        {
            client.DefaultRequestHeaders.Add(XApiTokenName, XApiToken);
        }

        public Node()
        {
        }

        internal static Task RemovePlotDirTask(string dirName)
        { 
            return Task.Run(async () =>
            {
                dynamic data = new JObject();
                data.path = dirName;

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                var res = await client.PostAsync(baseUri + "api/harvester/rem_plot_dir", byteContent);
                var res2 = await client.GetAsync(baseUri + "api/harvester/reload");
            });

        }

        internal static Task AddPlotDirTask(string dirName)
        {
            return Task.Run(async () =>
            {
                dynamic data = new JObject();
                data.path = dirName;

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                var res = await client.PostAsync(baseUri + "api/harvester/add_plot_dir", byteContent);
                var res2 = await client.GetAsync(baseUri + "api/harvester/reload");
            });
        }


        public static bool IsRunning
        {
            get {
                var task = Task.Run(CheckRunning);
                task.Wait();
                return task.Result;
            }
        }

        private static async Task<bool> CheckRunning()
        {
            try
            {
                var result = await client.GetAsync(sessionUri);
                return true;
            } catch (Exception) {
                Console.WriteLine("Node not running");
            }

            return false;
        }

        public void Start()
        {
            OnBeforeStarted();

            Node.Activate();

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
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
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
                client.DefaultRequestHeaders.Remove(XApiTokenName);
                client.DefaultRequestHeaders.Add(XApiTokenName, _xApiToken);
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
                } else
                {
                    //throw new Exception("Can not get XApiToken");
                }
            }
            else
            {
                var property = new JProperty(XApiToken, "ADMIN");

                if (firstAdminToken == null) {
                    if(tokenMap == null)
                    {
                        var jObject = new JObject();
                        jObject.Add(property);
                        httpServerConfig["token_map"] = jObject;
                    } else
                    {
                        tokenMap.Add(property);
                    }
                    
                } else
                {
                    firstAdminToken.Replace(property);
                }

                json = JsonConvert.SerializeObject(httpServerConfig, Formatting.Indented);
                File.WriteAllText(Node.httpServerConfigPath, json);
            }
        }


        private static Process GetProcess(string cmd)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDirectory;
            processStartInfo.FileName = cmd;

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = true;

            if (!Properties.Settings.Default.showConsole)
            {
                processStartInfo.CreateNoWindow = true;
            }

            Process process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;

            return process;
        }


        internal static void Activate()
        {
            if (!Directory.Exists(configPath))
            {
                Process process = GetProcess(activateCMDPath);

                process.Start();
                process.WaitForExit();
            }

            InitXToken();
        }

        private void StartProcess()
        {
            process = GetProcess(runNodeCMDPath);
            processStarted = process.Start();
        }

        private void WriteProcessLog(string text)
        {
            Console.WriteLine(text);
        }

        public void Stop()
        {
            OnBeforeStop();

            var delay = 100;
            var timeout = 10000;

            if (process != null && processStarted)
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

            processStarted = false;

            OnStop();
        }

        private void OnBeforeStarted()
        {
            if (BeforeStarted != null)
            {
                BeforeStarted(this, EventArgs.Empty);
            }
        }

        private void OnStart()
        {
            if (Started != null)
            {
                Started(this, EventArgs.Empty);
            }
        }

        private void OnBeforeStop()
        {
            if (BeforeStop != null)
            {
                BeforeStop(this, EventArgs.Empty);
            }
        }

        private void OnStop()
        {
            if (Stoped != null)
            {
                Stoped(this, EventArgs.Empty);
            }
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

    }
}
