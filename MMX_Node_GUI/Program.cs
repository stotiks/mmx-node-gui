using CefSharp;
using CefSharp.WinForms;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static MMX_NODE_GUI.NativeMethods;

namespace MMX_NODE_GUI
{
    static class Program
    {

        // The subfolder, where the cefsharp files will be moved to
        private static string cefSubFolder = "cefsharp";
        // If the assembly resolver loads cefsharp from another folder, set this to true
        private static bool resolved = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            InitializeCefSharp();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1)
            {
                MessageBox.Show("Another instance of this program is already running. Cannot proceed further.", "Warning!");
                return;
            }

            ApplyPowerManagementSettings();

            Application.Run(new MainForm());
        }

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath when cefsharp moved to the subfolder
            if (resolved)
                settings.BrowserSubprocessPath = Path.Combine(Application.StartupPath, cefSubFolder, "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false);
        }

        /// <summary>
        /// Will attempt to load missing assemblys from subfolder
        /// </summary>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                resolved = true; // Set to true, so BrowserSubprocessPath will be set

                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string subfolderPath = Path.Combine(Application.StartupPath, cefSubFolder, assemblyName);
                return File.Exists(subfolderPath) ? Assembly.LoadFile(subfolderPath) : null;
            }

            return null;
        }
    }
}
