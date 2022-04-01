using Microsoft.Win32;
using System;
using System.Globalization;
using System.Windows.Forms;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;

namespace MMX_NODE_GUI
{
    public partial class MainForm
    {
        private void InitializeSettings()
        {
            showInNotifitationMaterialSwitch.Location = new System.Drawing.Point(showInNotifitationGroupBox.Location.X + 5, showInNotifitationGroupBox.Location.Y - 10);
            showInNotifitationMaterialSwitch.BringToFront();

            startOnStartupMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "StartOnStartup", true, DataSourceUpdateMode.OnPropertyChanged);
            //startOnStartupMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => RegisterInStartup(Settings.Default.startOnStartup));

            startMinimizedMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "StartMinimized", true, DataSourceUpdateMode.OnPropertyChanged);
            confirmationOnExitMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "ConfirmationOnExit", true, DataSourceUpdateMode.OnPropertyChanged);


            showInNotifitationMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "ShowInNotifitation", true, DataSourceUpdateMode.OnPropertyChanged);
            showInNotifitationMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => {
                notifyIcon1.Visible = showInNotifitationMaterialSwitch.Checked;
                showInNotifitationGroupBox.Enabled = showInNotifitationMaterialSwitch.Checked;
            });
            showInNotifitationGroupBox.DataBindings.Add("Enabled", showInNotifitationMaterialSwitch, "Checked");


            minimizeToNotificationMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "_MinimizeToNotification", true, DataSourceUpdateMode.OnPropertyChanged);
            closeToNotificationMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "_CloseToNotification", true, DataSourceUpdateMode.OnPropertyChanged);

            inhibitSystemSleepMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "InhibitSystemSleep", true, DataSourceUpdateMode.OnPropertyChanged);
            //inhibitSystemSleepMaterialSwitch.CheckStateChanged += new EventHandler((object sender, EventArgs e) => PowerManagement.ApplyPowerManagementSettings(Settings.Default.inhibitSystemSleep) );

            showConsoleMaterialSwitch.DataBindings.Add("Checked", Settings.Default, "ShowConsole", true, DataSourceUpdateMode.OnPropertyChanged);
            

            langMaterialComboBox.DisplayMember = "Value";
            langMaterialComboBox.ValueMember = "Key";
            langMaterialComboBox.DataSource = new BindingSource(launguages, null);

            langMaterialComboBox.DataBindings.Add("SelectedValue", Settings.Default, "LanguageCode", true, DataSourceUpdateMode.OnPropertyChanged);
            boundObject.Locale = Settings.Default.LanguageCode;

            langMaterialComboBox.SelectedIndexChanged += langMaterialComboBox_SelectedIndexChanged;

#if !DEBUG
            debugGroupBox.Enabled = false;
#endif

        }

        private void langMaterialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = langMaterialComboBox.SelectedValue;
            string lang = selected != null ? selected.ToString() : "en";

            this.Culture = new CultureInfo(lang);
            boundObject.Locale = lang;
        }

 

    }
}
