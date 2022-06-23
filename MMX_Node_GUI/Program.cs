using CefSharp;
using CefSharp.WinForms;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MMX_NODE_GUI
{
    static class Program
    {

        // The subfolder, where the cefsharp files will be moved to
        private static string cefSubFolder = @"gui\cefsharp";
        // If the assembly resolver loads cefsharp from another folder, set this to true
        private static bool resolved = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //NativeMethods.AllocConsole();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (!SingleInstance.Start())
            {
                if(!SingleInstance.ShowFirstInstance())
                {
                    MessageBox.Show("Another instance of this program is already running by other user", "Warning!");
                }
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            InitializeCefSharp();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PowerManagement.ApplyPowerManagementSettings();

            Application.Run(new MainForm());

        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show( e.Exception.ToString(), "Warning! ThreadException");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show( (e.ExceptionObject as Exception).ToString(), "Warning! UnhandledException");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath when cefsharp moved to the subfolder
            if (resolved)
            {
                settings.BrowserSubprocessPath = Path.Combine(Application.StartupPath, cefSubFolder, "CefSharp.BrowserSubprocess.exe");
            }

            settings.UserAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36 (mmx.gui.win)";
            settings.Locale = Properties.Settings.Default.langCode;

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
