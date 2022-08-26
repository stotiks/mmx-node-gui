using Microsoft.Win32;
using System;
using System.Reflection;

namespace Mmx.Gui.Win.Common.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    public sealed partial class Settings {

        static public bool MinimizeToNotification => Settings.Default.ShowInNotifitation && Settings.Default._MinimizeToNotification;
        static public bool CloseToNotification => Settings.Default.ShowInNotifitation && Settings.Default._CloseToNotification;

        static public bool IsDarkTheme => Settings.Default.Theme == "Dark";

        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //

            this.PropertyChanged += (o, e) =>
            {
                if(e.PropertyName == nameof(Settings.Default.InhibitSystemSleep))
                {
                    PowerManagement.ApplyPowerManagementSettings(Settings.Default.InhibitSystemSleep);
                }

                if (e.PropertyName == nameof(Settings.Default.StartOnStartup))
                {
                    RegisterInStartup(Settings.Default.StartOnStartup);
                }

                Settings.Default.Save();
            };
        }
        private void RegisterInStartup(bool register)
        {
            var appName = Assembly.GetEntryAssembly().GetName().Name;
            // Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (register)
            {
                registryKey.SetValue(appName, "\"" + Assembly.GetEntryAssembly().Location + "\"");
            }
            else
            {
                if (registryKey.GetValue(appName, null) != null)
                {
                    registryKey.DeleteValue(appName);
                }

            }
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
