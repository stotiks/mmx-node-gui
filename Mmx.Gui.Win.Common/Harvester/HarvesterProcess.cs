using Mmx.Gui.Win.Common.Node;
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

            Start(processStartInfo);
        }

        //public override void Stop()
        //{
        //    Kill();
        //    KillByProcessName("mmx_harvester");
        //}
    }
}
