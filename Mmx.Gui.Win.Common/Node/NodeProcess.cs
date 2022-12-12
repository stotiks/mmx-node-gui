using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Node
{
    public class NodeProcess : ProcessWrapper
    {
        private void Activate()
        {
            if (!Directory.Exists(NodeHelpers.configPath))
            {
                //TODO
                //var process = GetProcess(NodeHelpers.activateCMDPath);
                //process.WaitForExit();

                var json = File.ReadAllText(NodeHelpers.walletConfigPath);

                JObject walletConfig = JsonConvert.DeserializeObject<JObject>(json);
                walletConfig.Property("key_files+").Remove();
                walletConfig.Add(new JProperty("key_files", new JArray()));

                json = JsonConvert.SerializeObject(walletConfig, Formatting.Indented);
                File.WriteAllText(NodeHelpers.walletConfigPath, json);
            }
        }

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
            }

            OnStop();
        }
    }
}
