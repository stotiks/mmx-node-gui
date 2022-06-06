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

        public static string MMX_HOME = Environment.GetEnvironmentVariable("MMX_HOME") == "" ? Environment.GetEnvironmentVariable("MMX_HOME") : (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.mmx");
        public static string configPath = MMX_HOME + @"\config\local";
        public static string harvesterConfigPath = configPath + @"\Harvester.json";
        public static string plotterConfigPath = configPath + @"\Plotter.json";
        public static string httpServerConfigPath = configPath + @"\HttpServer.json";
        
        public static string activateCMDPath = workingDirectory + @"\activate.cmd";
        public static string runNodeCMDPath = workingDirectory + @"\run_node.cmd";

        static public readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        static public readonly Uri guiUri = new Uri(baseUri, "/gui/");
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
            } catch (Exception) {}

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

        internal static void Activate()
        {
            if(!Directory.Exists(configPath))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WorkingDirectory = workingDirectory;
                processStartInfo.FileName = activateCMDPath;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                Process process = new Process();
                process.EnableRaisingEvents = true;
                process.StartInfo = processStartInfo;

                process.Start();
                process.WaitForExit();
            }

            InitXToken();
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
                //System.Console.WriteLine(@"config not found");
            }

            dynamic httpServerConfig = JsonConvert.DeserializeObject(json);

            if (IsRunning)
            {
                var map = httpServerConfig["token_map"];

                foreach (JProperty prop in map.Properties())
                {
                    if(prop.Value.ToString() == "ADMIN")
                    {
                        XApiToken = prop.Name;
                        break;
                    }                    
                }
            }
            else
            {
                dynamic jObject = new JObject();
                var role = "ADMIN";
                var token = XApiToken;
                jObject.Add(token, role);
                httpServerConfig["token_map"] = jObject;

                json = JsonConvert.SerializeObject(httpServerConfig, Formatting.Indented);
                File.WriteAllText(Node.httpServerConfigPath, json);
            }
        }

        private void StartProcess()
        {            
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDirectory;
            processStartInfo.FileName = runNodeCMDPath;

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = false;

            if (!Properties.Settings.Default.showConsole)
            {
                processStartInfo.CreateNoWindow = true;
            }
            

            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //if (false)
            //{
            //    processStartInfo.RedirectStandardError = true;                
            //    processStartInfo.RedirectStandardOutput = true;
            //    processStartInfo.RedirectStandardInput = false;
            //}



            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;

            //process.Exited += BeforeStop;
            //process.Exited += Stoped;

            //process.OutputDataReceived += (sender1, args) => WriteProcessLog(args.Data);
            //process.ErrorDataReceived += (sender1, args) => WriteProcessLog(args.Data);

            processStarted = process.Start();

            if (process.StartInfo.RedirectStandardOutput) process.BeginOutputReadLine();
            if (process.StartInfo.RedirectStandardError) process.BeginErrorReadLine();
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
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

    }
}
