using Mmx.Gui.Win.Common.Node;
using Mmx.Gui.Win.Common.Properties;
using System.Diagnostics;

namespace PlotSincGui
{
    public class PlotSincProcess : ProcessWrapper
    {
        public override void Start()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = NodeHelpers.plotSincEXEPath,
                CreateNoWindow = true,
                Arguments = "-- " + string.Join(" ", (Settings.Default.PlotSincDirectories ?? new System.Collections.ArrayList()).ToArray())
            };

            Start(processStartInfo);
        }
    }
}