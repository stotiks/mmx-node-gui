using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static Mmx.Gui.Win.Common.NativeMethods;


namespace Mmx.Gui.Win.Common
{
    public static class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramGuid);

        public static bool IsFirstInstance()
        {                       
            return !(GetProcesses().Length > 1);
        }

        private static bool ProcessAccessibleForCurrentUser(Process process)
        {
            try
            {
                _ = process.Handle;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ShowFirstInstance()
        {
            foreach(var process in GetProcesses())
            {
                if(ProcessAccessibleForCurrentUser(process))
                {
                    PostMessage((IntPtr)HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
                    return true;
                }
            }

            return false;
            
        }

        private static Process[] GetProcesses()
        {
            return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location));
        }

        private static string ProgramGuid
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
