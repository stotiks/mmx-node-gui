using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class OptionsForm : Form
    {
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
    }
}
