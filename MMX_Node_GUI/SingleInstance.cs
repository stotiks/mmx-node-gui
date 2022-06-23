using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MMX_NODE_GUI
{

    internal class SingleInstance
    {
        static public readonly int WM_SHOWFIRSTINSTANCE = NativeMethods.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramGuid);

        static public bool Start()
        {                       
            return !(GetProcesses().Length > 1);
        }

        static public bool ProcessAccessibleForCurrentUser(Process process)
        {
            try
            {
                var ptr = process.Handle;
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool ShowFirstInstance()
        {
            foreach(var process in GetProcesses())
            {
                if(ProcessAccessibleForCurrentUser(process))
                {
                    NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
                    return true;
                }
            }

            return false;
            
        }

        static private Process[] GetProcesses()
        {
            return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location));
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
