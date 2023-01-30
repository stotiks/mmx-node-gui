using Mmx.Gui.Win.Common.Properties;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Node
{
    public class NodeProcess : ProcessWrapper
    {
        public override void Start()
        {
            OnBeforeStart();

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
            OnStarted();
        }

        private void Activate()
        {
            var aProcess = new ActivateProcess();
            aProcess.OutputDataReceived += OnOutputDataReceived;
            aProcess.ErrorDataReceived += OnErrorDataReceived;
            aProcess.Start();
        }

        private void StartProcess()
        {
            string arguments = "";
            if (Settings.Default.DisableOpenCL)
            {
                arguments += " --Node.opencl_device -1";
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = NodeHelpers.runNodeCMDPath,
                Arguments = arguments,
                CreateNoWindow = true
            };

            Start(processStartInfo);
        }

        public override void Stop()
        {
            OnBeforeStop();

            _ = NodeApi.Exit();

            var delay = 100;
            var timeout = 10000;

            if (process != null)
            {
                while (!process.HasExited && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }
                Kill();
            }
            else
            {
                while (NodeApi.IsRunning && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }
                KillByProcessName("mmx_node");
            }

            OnStop();
        }
    }
}
