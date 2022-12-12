using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class Harvester : NodeBase
    {
        public override void Start()
        {
            OnBeforeStarted();
            
            //if (!NodeApi.IsRunning)
            {
                _process = GetProcess(runHarvesterCMDPath);
            }

            Task.Run(async () => { await OnStartedAsync(); });
        }

        public override void Stop()
        {
            OnBeforeStop();
            _process.Stop();
            OnStop();
        }
    }
}
