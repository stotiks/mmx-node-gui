using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    class Node
    {
        static public readonly Uri baseUri = new Uri("http://127.0.0.1:11380");
        static public readonly Uri guiUri = new Uri(baseUri, "/gui/");
        private static readonly Uri exitUri = new Uri(baseUri, "/wapi/node/exit");
        private static readonly Uri checkUri = new Uri(baseUri, "/api/router/get_peer_info");

        public event EventHandler Started;
        public event EventHandler BeforeStop;
        public event EventHandler Stoped;

        private static readonly HttpClient client = new HttpClient();
        private Process process;
        private bool processStarted = false;

        public Node()
        {
        }

        public void Start()
        {
            var executed = false;

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        var result = await client.GetAsync(checkUri);
                        //Console.WriteLine(result);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine(e);
                    }

                    if(!executed)
                    {
                        executed = true;
                        StartProcess();
                    }

                    Console.WriteLine("Waiting node...");
                    await Task.Delay(500);
                }

                OnStart();
            });

        }

        private void StartProcess()
        {
            var exePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
#if DEBUG
            exePath = "C:\\Program Files\\MMX";
#endif
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = exePath;
            processStartInfo.FileName = exePath + "\\run_node.cmd";

            processStartInfo.UseShellExecute = false;
            //processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;
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

            process.Exited += BeforeStop;
            process.Exited += Stoped;

            process.OutputDataReceived += (sender1, args) => WriteProcessLog(args.Data);
            process.ErrorDataReceived += (sender1, args) => WriteProcessLog(args.Data);

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

            Task.Run(async () => await ExitAsync()).Wait();

            if (process != null && processStarted)
            {
                var delay = 500;
                var timeout = 10000;
                while (!process.HasExited && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }

                if (!process.HasExited)
                {
                    process.Kill();
                }
            }

            OnStop();
        }

        private async Task ExitAsync()
        {
            try
            {
                var result = await client.PostAsync(exitUri, null);
                //Console.WriteLine(result);
            }
            catch (Exception)
            {
                //Console.WriteLine(e);
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


    }
}
