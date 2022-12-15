using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Node
{
    internal class ActivateProcess : ProcessWrapper
    {
        public override void Start()
        {
            if (!Directory.Exists(NodeHelpers.configPath))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = NodeHelpers.activateCMDPath,
                    CreateNoWindow = true
                };

                Start(processStartInfo);
                WaitForExit();
                FixWalletConfig();
            }

        }

        private static void FixWalletConfig()
        {
            var json = File.ReadAllText(NodeHelpers.walletConfigPath);

            JObject walletConfig = JsonConvert.DeserializeObject<JObject>(json);
            walletConfig.Property("key_files+").Remove();
            walletConfig.Add(new JProperty("key_files", new JArray()));

            json = JsonConvert.SerializeObject(walletConfig, Formatting.Indented);
            File.WriteAllText(NodeHelpers.walletConfigPath, json);
        }
    }
}
