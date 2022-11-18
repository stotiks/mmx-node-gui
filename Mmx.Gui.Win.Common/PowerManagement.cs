using static Mmx.Gui.Win.Common.NativeMethods;

namespace Mmx.Gui.Win.Common
{
    public static class PowerManagement
    {
        public static void ApplyPowerManagementSettings(bool inhibitSystemSleep)
        {
            if (inhibitSystemSleep)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
            }
            else
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }

        }
    }
}
