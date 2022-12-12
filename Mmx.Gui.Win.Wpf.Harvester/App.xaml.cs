using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            Environment.CurrentDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.LanguageCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.LanguageCode);

            if (!SingleInstance.IsFirstInstance())
            {
                if (!SingleInstance.ShowFirstInstance())
                {
                    System.Windows.MessageBox.Show(Common.Properties.Resources.Another_Instance_Running, Common.Properties.Resources.Warning);
                }
                Current.Shutdown();
            }
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            using (ThreadExceptionDialog dlg = new ThreadExceptionDialog(e.Exception))
            {
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.Abort)
                {
                    Environment.Exit(-1);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Handled = true;
                }
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show((e.ExceptionObject as Exception).ToString(), "Warning! UnhandledException");
        }
    }
}
