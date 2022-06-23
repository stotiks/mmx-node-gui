using System;
using System.Reflection;
using System.Threading;

namespace MMX_NODE_GUI
{

    internal class SingleInstance
    {
        static public readonly int WM_SHOWFIRSTINSTANCE = NativeMethods.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramGuid);
        static private readonly string mutexName = string.Format("Global\\{0}", ProgramGuid);

        static Mutex mutex;
        static public bool Start()
        {            
            bool onlyInstance;
            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }

        static public void ShowFirstInstance()
        {
            NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
        }

        static public void Stop()
        {
            mutex.ReleaseMutex();
        }

        static public string ProgramGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
    }
}
