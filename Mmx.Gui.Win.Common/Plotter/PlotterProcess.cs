using Mmx.Gui.Win.Common.Node;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterProcess : ProcessWrapper
    {
        private readonly Regex _plotNameRegex = new Regex(@"^Plot Name: (plot-.*)");

        private string _currentPlotName;

        public PlotterProcess()
        {
            BeforeStart += (s, e) =>
            {
                StopTry = 0;
                NativeMethods.SetConsoleCtrlHandler(null, false);
            };

            Started += (s, e) =>
            {
                process.PriorityClass = (ProcessPriorityClass)PlotterOptions.Instance.priority.Value;
            };

            ProcessExit += (s, e) =>
            {
                NativeMethods.SetConsoleCtrlHandler(null, false);
                CleanFs();
            };

            OutputDataReceived += (s, e) =>
            {
                var str = e.Data;

                if (!string.IsNullOrEmpty(str))
                {
                    var r = _plotNameRegex.Match(str);
                    if (r.Success)
                    {
                        _currentPlotName = r.Groups[1].Value;
                        //Console.WriteLine(currentPlotName);
                    }

                }
            };

        }

        public override void Start()
        {
            var fileName = Path.Combine(NodeHelpers.workingDirectory, PlotterOptions.Instance.PlotterExe);
            var arguments = PlotterOptions.Instance.PlotterArguments;

#if DEBUG
            //fileName = "ping";
            //arguments = "google.com -n 30";
#endif

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true
            };

            Start(processStartInfo);
        }


        private void CleanFs()
        {
            OnOutputDataReceived(this, "Temp files cleaning...");

            var deletedCount = 0;

            deletedCount += DeleteTempFiles(PlotterOptions.Instance.finaldir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.tmpdir2.Value, _currentPlotName);
            deletedCount += DeleteTempFiles(PlotterOptions.Instance.stagedir.Value, _currentPlotName);

            OnOutputDataReceived(this, $"Temp files deleted: {deletedCount}");
        }

        private int DeleteTempFiles(string dir, string plotName)
        {
            var result = 0; 
            if (!string.IsNullOrEmpty(plotName) && !string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
                var reg = new Regex($@"^({plotName})");
                var files = Directory.GetFiles(dir, "*.tmp")
                    .Where(path => reg.IsMatch(Path.GetFileName(path)))
                    .ToList();
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                result = files.Count;
            }

            return result;
        }

        private int _stopTry;

        private int StopTry
        {
            get => _stopTry;
            set
            {
                _stopTry = value;
                NotifyPropertyChanged();
            }
        }

        public new void Stop()
        {
            if (StopTry++ <= 2)
            {
                NativeMethods.StopProgramByAttachingToItsConsoleAndIssuingCtrlCEvent(process);
            }
            else
            {
                Kill();
            }
        }
    }
}
