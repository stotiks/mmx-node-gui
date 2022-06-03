using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private void InitializeSettings()
        {
            showInNotifitationMaterialSwitch.Location = new System.Drawing.Point(showInNotifitationGroupBox.Location.X + 5, showInNotifitationGroupBox.Location.Y - 10);
            showInNotifitationMaterialSwitch.BringToFront();

            startOnStartupMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "startOnStartup", true, DataSourceUpdateMode.OnPropertyChanged);
            startOnStartupMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => RegisterInStartup(Properties.Settings.Default.startOnStartup));

            startMinimizedMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "startMinimized", true, DataSourceUpdateMode.OnPropertyChanged);
            confirmationOnExitMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "confirmationOnExit", true, DataSourceUpdateMode.OnPropertyChanged);


            showInNotifitationMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "showInNotifitation", true, DataSourceUpdateMode.OnPropertyChanged);
            showInNotifitationMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => {
                notifyIcon1.Visible = showInNotifitationMaterialSwitch.Checked;
                showInNotifitationGroupBox.Enabled = showInNotifitationMaterialSwitch.Checked;
            });
            showInNotifitationGroupBox.DataBindings.Add("Enabled", showInNotifitationMaterialSwitch, "Checked");


            minimizeToNotificationMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "minimizeToNotification", true, DataSourceUpdateMode.OnPropertyChanged);
            closeToNotificationMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "closeToNotification", true, DataSourceUpdateMode.OnPropertyChanged);

            inhibitSystemSleepMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "inhibitSystemSleep", true, DataSourceUpdateMode.OnPropertyChanged);
            inhibitSystemSleepMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => PowerManagement.ApplyPowerManagementSettings() );

            showConsoleMaterialSwitch.DataBindings.Add("Checked", Properties.Settings.Default, "showConsole", true, DataSourceUpdateMode.OnPropertyChanged);
            langMaterialComboBox.DataBindings.Add("SelectedValue", Properties.Settings.Default, "langCode", true, DataSourceUpdateMode.OnPropertyChanged);

#if !DEBUG
            debugGroupBox.Enabled = false;
#endif

        }


        private void saveSettings(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
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
