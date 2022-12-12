using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class Harvester : NodeBase
    {
        public override void Start()
        {
            OnBeforeStarted();
            
            if (!NodeApi.IsRunning)
            {
                _process = GetProcess(runHarvesterCMDPath);
            }

            Task.Run(async () => { await OnStartedAsync(); });
        }

        public override void Stop()
        {
            OnBeforeStop();

            if (_process != null && !_process.HasExited)
            {
                NativeMethods.KillProcessAndChildren(_process.Id);
            }

            OnStop();
        }
    }
}
