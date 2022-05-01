using static MMX_NODE_GUI.NativeMethods;

namespace MMX_NODE_GUI
{
    internal class PowerManagement
    {
        internal static void ApplyPowerManagementSettings()
        {
            if (Properties.Settings.Default.inhibitSystemSleep)
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
