using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MMX_GUI
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
            Application.Run(new MainForm());
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
