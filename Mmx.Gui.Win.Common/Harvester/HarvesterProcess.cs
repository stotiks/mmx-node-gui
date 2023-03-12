using Mmx.Gui.Win.Common.Node;
using System.Diagnostics;

namespace Mmx.Gui.Win.Common.Harvester
{
    public class HarvesterProcess : ProcessWrapper
    {
        public override void Start()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = NodeHelpers.runHarvesterCMDPath,
                CreateNoWindow = true
            };

            NodeHelpers.SetEnvVariables(processStartInfo);

            Start(processStartInfo);
        }
    }
}
