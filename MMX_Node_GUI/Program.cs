using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Threading;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //NativeMethods.AllocConsole();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (!SingleInstance.IsFirstInstance())
            {
                if(!SingleInstance.ShowFirstInstance())
                {
                    MessageBox.Show("Another instance of this program is already running by other user", "Warning!");
                }
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += CefUtils.CurrentDomain_AssemblyResolve;
            //CefUtils.InitializeCefSharp(new CefSettings());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PowerManagement.ApplyPowerManagementSettings(Settings.Default.InhibitSystemSleep);

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

    }
}
