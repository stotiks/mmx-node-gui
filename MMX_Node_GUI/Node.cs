using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public class Node
    {

        public static string MMX_HOME = Environment.GetEnvironmentVariable("MMX_HOME") == "" ? Environment.GetEnvironmentVariable("MMX_HOME") : (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.mmx");
        public static string configPath = MMX_HOME + @"\config\local";
        public static string harvesterConfigPath = configPath + @"\Harvester.json";
        public static string plotterConfigPath = configPath + @"\Plotter.json";

        static public readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        static public readonly Uri guiUri = new Uri(baseUri, "/gui/");
        static private readonly Uri sessionUri = new Uri(baseUri, "/server/session");

        public event EventHandler BeforeStarted;
        public event EventHandler Started;
        public event EventHandler BeforeStop;
        public event EventHandler Stoped;

        internal static string GetPassword()
        {
            string password = "";

            try
            {
                password = File.ReadAllText(MMX_HOME + @"\PASSWD").Trim();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            return password;
        }

        private static readonly HttpClient client = new HttpClient();
        private Process process;
        private bool processStarted = false;

        public Node()
        {
        }

        public bool IsRunning
        {
            get {
                var task = Task.Run(CheckRunning);
                task.Wait();
                return task.Result;
            }
        }

        private async Task<bool> CheckRunning()
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


        private void StartProcess()
        {
            var exePath = Path.GetDirectoryName(Application.ExecutablePath);

#if DEBUG
            //exePath = @"C:\Program Files\MMX";
            exePath = @"C:\dev\mmx\MMX_TEST6";
#endif

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = exePath;
            processStartInfo.FileName = exePath + "\\run_node.cmd";

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = false;

#if !DEBUG
            processStartInfo.CreateNoWindow = true;
#endif

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
