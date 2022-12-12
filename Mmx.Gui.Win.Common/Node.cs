using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class Node : NodeBase
    {
        private void Activate()
        {
            if (!Directory.Exists(configPath))
            {
                var process = GetProcess(activateCMDPath);
                process.WaitForExit();

                var json = File.ReadAllText(walletConfigPath);

                JObject walletConfig = JsonConvert.DeserializeObject<JObject>(json);
                walletConfig.Property("key_files+").Remove();
                walletConfig.Add(new JProperty("key_files", new JArray()));

                json = JsonConvert.SerializeObject(walletConfig, Formatting.Indented);
                File.WriteAllText(walletConfigPath, json);
            }
        }

        public override void Start()
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

        private void StartProcess()
        {
            string args = "";
            if (Settings.Default.DisableOpenCL)
            {
                args += " --Node.opencl_device -1";
            }
            _process = GetProcess(runNodeCMDPath, args);
        }

        public override void Stop()
        {
            OnBeforeStop();

            _ = NodeApi.Exit();

            var delay = 100;
            var timeout = 10000;

            if (_process != null)
            {
                while (_process.IsRunning && timeout >= 0)
                {
                    timeout -= delay;
                    Task.Delay(delay).Wait();
                }
                _process.Stop();
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
