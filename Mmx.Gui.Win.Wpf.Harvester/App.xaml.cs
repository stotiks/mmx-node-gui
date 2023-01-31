using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
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

            InitNotifyIcon();
        }

        readonly NotifyIcon notifyIcon = new NotifyIcon();
        readonly ContextMenu notifyIconContextMenu = new ContextMenu();

        private void InitNotifyIcon()
        {
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            notifyIcon.Visible = Settings.Default.ShowInNotification;
            //nIcon.ShowBalloonTip(5000, "Title", "Text", System.Windows.Forms.ToolTipIcon.Info);
            notifyIcon.DoubleClick += NotifyIcon_Click;

            this.Exit += (sender, e) => {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            };

            notifyIcon.ContextMenu = notifyIconContextMenu;
            var menuItem1 = new MenuItem
            {
                Index = 0,
                Text = Common.Properties.Resources.Show
            };
            menuItem1.Click += NotifyIcon_Click;

            var menuItem3 = new MenuItem
            {
                Index = 1,
                Text = Common.Properties.Resources.Exit
            };
            menuItem3.Click += (s, e) =>
            {
                MainWindow win = System.Windows.Window.GetWindow(App.Current.MainWindow) as MainWindow;
                win.Restore();
                win.Close();
            };

            notifyIconContextMenu.MenuItems.AddRange(
               new[] {
                   menuItem1,
                   new MenuItem("-"),
                   menuItem3
               }
            );

            Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Settings.Default.ShowInNotification))
                {
                    notifyIcon.Visible = Settings.Default.ShowInNotification;
                }
            };

        }

        void NotifyIcon_Click(object sender, EventArgs e)
        {
            MainWindow win = System.Windows.Window.GetWindow(App.Current.MainWindow) as MainWindow;
            win.Restore();
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
