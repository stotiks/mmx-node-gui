using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class OptionsForm : Form
    {
        private static string MMX_HOME = Environment.GetEnvironmentVariable("MMX_HOME") == "" ? Environment.GetEnvironmentVariable("MMX_HOME") : (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.mmx");
        private static string configPath = MMX_HOME + @"\config\local";
        private static string harvesterConfigPath = configPath + @"\Harvester.json";
        private JObject harvesterConfig;

        public OptionsForm()
        {
            InitializeComponent();
            showInNotifitationCheckBox.Location = new Point(showInNotifitationGroupBox.Location.X + 5, showInNotifitationGroupBox.Location.Y);
            showInNotifitationCheckBox.BringToFront();

            startOnStartupCheckBox.Checked = Properties.Settings.Default.startOnStartup;
            startMinimizedCheckBox.Checked = Properties.Settings.Default.startMinimized;
            confirmationOnExitCheckBox.Checked = Properties.Settings.Default.confirmationOnExit;

            showInNotifitationCheckBox.Checked = Properties.Settings.Default.showInNotifitation;
            minimizeToNotificationCheckBox.Checked = Properties.Settings.Default.minimizeToNotification;
            closeToNotificationCheckBox.Checked = Properties.Settings.Default.closeToNotification;

            showInNotifitationGroupBox.Enabled = showInNotifitationCheckBox.Checked;

            inhibitSystemSleepCheckBox.Checked = Properties.Settings.Default.inhibitSystemSleep;


            string harvesterJson = File.ReadAllText(harvesterConfigPath);
            harvesterConfig = JObject.Parse(harvesterJson);

            int num_threads = harvesterConfig.Value<int>("num_threads");
            int reload_interval = harvesterConfig.Value<int>("reload_interval");
            JArray plot_dirs = harvesterConfig.Value<JArray>("plot_dirs");

            for(int i = 0; i < plot_dirs.Count; i++)
            {
                plotDirListBox.Items.Add(plot_dirs[i]);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.startOnStartup = startOnStartupCheckBox.Checked;
            Properties.Settings.Default.startMinimized = startMinimizedCheckBox.Checked;
            Properties.Settings.Default.confirmationOnExit = confirmationOnExitCheckBox.Checked;

            Properties.Settings.Default.showInNotifitation = showInNotifitationCheckBox.Checked;
            Properties.Settings.Default.minimizeToNotification = minimizeToNotificationCheckBox.Checked;
            Properties.Settings.Default.closeToNotification = closeToNotificationCheckBox.Checked;

            Properties.Settings.Default.inhibitSystemSleep = inhibitSystemSleepCheckBox.Checked;

            Properties.Settings.Default.Save();

            PowerManagement.ApplyPowerManagementSettings();
            RegisterInStartup(Properties.Settings.Default.startOnStartup);

            ((JArray)harvesterConfig["plot_dirs"]).Clear();
            for (int i = 0; i < plotDirListBox.Items.Count; i++)
            {
                ((JArray)harvesterConfig["plot_dirs"]).Add(plotDirListBox.Items[i].ToString());
            }
            File.WriteAllText(harvesterConfigPath, harvesterConfig.ToString());

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void showInNotifitationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showInNotifitationGroupBox.Enabled = showInNotifitationCheckBox.Checked;
        }


        private void RegisterInStartup(bool isChecked)
        {
            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue(appName, "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                if (registryKey.GetValue(appName, null) != null)
                {
                    registryKey.DeleteValue(appName);
                }

            }
        }


        private Color[] TColors = { Color.Salmon, Color.White, Color.LightBlue };

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // get ref to this page
            TabPage tp = ((TabControl)sender).TabPages[e.Index];

            using (Brush br = new SolidBrush(TColors[e.Index]))
            {
                Rectangle rect = e.Bounds;
                e.Graphics.FillRectangle(br, e.Bounds);

                rect.Offset(1, 1);
                TextRenderer.DrawText(e.Graphics, tp.Text,
                       tp.Font, rect, tp.ForeColor);

                // draw the border
                rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);

                // ControlDark looks right for the border
                using (Pen p = new Pen(SystemColors.ControlDark))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }

                if (e.State == DrawItemState.Selected) e.DrawFocusRectangle();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                plotDirListBox.Items.Add(dialog.FileName);
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            plotDirListBox.Items.Remove(plotDirListBox.SelectedItem);
        }

    }
}
