using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace Mmx.Gui.Win.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu notifyIconContextMenu = new System.Windows.Forms.ContextMenu();

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            AppDomain.CurrentDomain.AssemblyResolve += CefUtils.CurrentDomain_AssemblyResolve;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.LanguageCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.LanguageCode);

            if (!SingleInstance.IsFirstInstance())
            {
                if (!SingleInstance.ShowFirstInstance())
                {
                    MessageBox.Show(Mmx.Gui.Win.Wpf.Properties.Resources.Another_Instance_Running, Mmx.Gui.Win.Wpf.Properties.Resources.Warning);
                }
                Application.Current.Shutdown();
            }

            PowerManagement.ApplyPowerManagementSettings(Settings.Default.InhibitSystemSleep);

            InitNotifyIcon();
        }

        private void InitNotifyIcon()
        {
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            notifyIcon.Visible = Settings.Default.ShowInNotifitation;
            //nIcon.ShowBalloonTip(5000, "Title", "Text", System.Windows.Forms.ToolTipIcon.Info);
            notifyIcon.DoubleClick += notifyIcon_Click;

            this.Exit += (sender, e) => {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            };

            notifyIcon.ContextMenu = notifyIconContextMenu;
            var menuItem1 = new System.Windows.Forms.MenuItem();
            menuItem1.Index = 0;
            menuItem1.Text = Mmx.Gui.Win.Wpf.Properties.Resources.Show;
            menuItem1.Click += notifyIcon_Click;

            var menuItem3 = new System.Windows.Forms.MenuItem();
            menuItem3.Index = 1;
            menuItem3.Text = Mmx.Gui.Win.Wpf.Properties.Resources.Exit;
            menuItem3.Click += (s, e) =>
            {
                MainWindow win = Window.GetWindow(App.Current.MainWindow) as MainWindow;
                win.Restore();
                win.Close();
            };

            notifyIconContextMenu.MenuItems.AddRange(
               new System.Windows.Forms.MenuItem[] {
                   menuItem1,
                   new System.Windows.Forms.MenuItem("-"),
                   menuItem3
               });

            Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Settings.Default.ShowInNotifitation))
                {
                    notifyIcon.Visible = Settings.Default.ShowInNotifitation;
                }
            };

        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            MainWindow win = Window.GetWindow(App.Current.MainWindow) as MainWindow;
            win.Show();
            win.Restore();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception).ToString(), "Warning! UnhandledException");
        }
    }

}
