﻿using Microsoft.Win32;
using Mmx.Gui.Win.Common.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;

namespace Mmx.Gui.Win.Common.Properties
{


    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    public sealed partial class Settings {

        public static bool MinimizeToNotification => Settings.Default.ShowInNotification && Settings.Default._MinimizeToNotification;
        public static bool CloseToNotification => Settings.Default.ShowInNotification && Settings.Default._CloseToNotification;

        public static Action DebouncedSave;
        
        static Settings()
        {
            DebouncedSave = ((Action)Settings.Default.Save).Debounce(500);

            if (Settings.Default.SettingsUpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.SettingsUpgradeRequired = false;
                Settings.Default.Save();
            }
        }

        private Settings() 
        {
            
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

                Settings.DebouncedSave();
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

        //private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
        //    // Add code to handle the SettingChangingEvent event here.
        //}

        //private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
        //    // Add code to handle the SettingsSaving event here.
        //}

        //public List<string> PlotSincDirectoriesList
        //{
        //    get => Settings.Default.PlotSincDirectories.Cast<string>.ToList();
        //    set
        //    {
        //        Settings.Default.PlotSincDirectories = value.;
        //    }
        //}
    }
}
