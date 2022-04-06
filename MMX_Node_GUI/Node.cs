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
        private readonly ConsoleControl.ConsoleControl consoleControl;

        public Node(ConsoleControl.ConsoleControl consoleControl)
        {
            this.consoleControl = consoleControl;
        }

        public void Start()
        {
            var exePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
#if DEBUG
            exePath = "C:\\Program Files\\MMX";
#endif
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = exePath;
            psi.FileName = exePath + "\\run_node.cmd";
            consoleControl.StartProcess(psi);

            Task.Run(async () => {
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
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                    Console.WriteLine("Waiting node...");
                    await Task.Delay(500);
                }

                OnStart();
            });
        }

        public void Stop()
        {
            OnBeforeStop();
            Task.Run(async () => await ExitAsync()).Wait();
            consoleControl.StopProcess();
            OnStop();
        }

        private async Task ExitAsync()
        {
            try
            {
                var result = await client.PostAsync(exitUri, null);
                //Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
