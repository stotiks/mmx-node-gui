using Mmx.Gui.Win.Common.Node;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Diagnostics;

namespace Mmx.Gui.Win.Common.Harvester
{
    public class HarvesterProcess : ProcessWrapper
    {
        public void Restart()
        {
            if (IsRunning)
            {
                Stop();
                Start();
            }
        }

        public override void Start()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = NodeHelpers.runHarvesterCMDPath,
                CreateNoWindow = true
            };

            if (Settings.Default.CHIAPOS_MIN_CUDA_LOG_ENTRIES_Enabled)
            {
                processStartInfo.EnvironmentVariables.Add("CHIAPOS_MIN_CUDA_LOG_ENTRIES", Settings.Default.CHIAPOS_MIN_CUDA_LOG_ENTRIES.ToString());
            }

            if (Settings.Default.CHIAPOS_MAX_CORES_Enabled)
            {
                processStartInfo.EnvironmentVariables.Add("CHIAPOS_MAX_CORES", Settings.Default.CHIAPOS_MAX_CORES.ToString());
            }

            if (Settings.Default.CHIAPOS_MAX_CUDA_DEVICES_Enabled)
            {
                processStartInfo.EnvironmentVariables.Add("CHIAPOS_MAX_CUDA_DEVICES", Settings.Default.CHIAPOS_MAX_CUDA_DEVICES.ToString());
            }

            Start(processStartInfo);
        }
    }
}
