using Mmx.Gui.Win.Common.Node;
using System.Diagnostics;

namespace ReComputeServerGui
{
    public class RecomputeProcess : ProcessWrapper
    {
        public override void Start()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = NodeHelpers.recomputeEXEPath,
                CreateNoWindow = true,
                //Arguments = "-- " + string.Join(" ", (Settings.Default.PlotSincDirectories ?? new System.Collections.ArrayList()).ToArray())
            };

            Start(processStartInfo);
        }
    }
}
