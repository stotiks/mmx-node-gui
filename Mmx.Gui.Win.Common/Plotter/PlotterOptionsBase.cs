using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterOptionsBase : INotifyPropertyChanged
    {

        public static readonly string PLOT_MANAGER_HOME_ENV = Environment.GetEnvironmentVariable("PLOT_MANAGER_HOME");
        public static readonly string PLOT_MANAGER_HOME = string.IsNullOrEmpty(PLOT_MANAGER_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".plotManager") : PLOT_MANAGER_HOME_ENV;

        public static readonly string plotterConfigPath;
        public static readonly string plotterLogFolder;

        static PlotterOptionsBase()
        {
            if(IsMmxOnly)
            {                
                plotterConfigPath = Path.Combine(NodeHelpers.MMX_HOME, "plotter/Plotter.json");
                plotterLogFolder = Path.Combine(NodeHelpers.MMX_HOME, "plotter/logs");

                var plotterOldConfigPath = Path.Combine(NodeHelpers.configPath, "Plotter.json");
                if (File.Exists(plotterOldConfigPath) && !File.Exists(plotterConfigPath))
                {
                    File.Move(plotterOldConfigPath, plotterConfigPath);
                }

            } else
            {
                plotterConfigPath = Path.Combine(PLOT_MANAGER_HOME, "Plotter.json");
                plotterLogFolder = Path.Combine(PLOT_MANAGER_HOME, "logs");
            }
        }

        public static bool IsMmxOnly = !Convert.ToBoolean(ConfigurationManager.AppSettings["PlotManager"]);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected IEnumerable<PropertyInfo> GetItemProperties()
        {
            return GetType().GetProperties()
                                .Where(property => 
                                           property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Item<>)
                                        || property.PropertyType.BaseType.IsGenericType && property.PropertyType.BaseType.GetGenericTypeDefinition() == typeof(Item<>)
                                 ).OrderBy(property => ((OrderAttribute)property.GetCustomAttributes(typeof(OrderAttribute)).Single()).Order);
        }


        protected void LoadJSON()
        {
            string json = "{}";
            try
            {
                json = File.ReadAllText(plotterConfigPath);
            } 
            catch
            {
                //System.Console.WriteLine(@"config not found");
            }
            LoadJSON(json);
        }

        protected void LoadJSON(string json)
        {
            dynamic config = JsonConvert.DeserializeObject(json);
            foreach (var configItem in config)
            {
                var property = typeof(PlotterOptions).GetProperty(configItem.Name);
                if (property != null)
                {
                    dynamic item = property.GetValue(this);
                    item.SetValue(configItem.Value);
                }
            }
        }

        protected void Save()
        {
            dynamic jObject = new JObject();

            foreach (PropertyInfo property in GetItemProperties())
            {
                dynamic item = property.GetValue(this);
                if (item.Persistent)
                {
                    jObject.Add(item.LongName, item.Value);
                }
            }

            var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);

            var plotterConfigDir = Path.GetDirectoryName(plotterConfigPath);
            if (!System.IO.Directory.Exists(plotterConfigDir))
            {
                System.IO.Directory.CreateDirectory(plotterConfigDir);
            }

            File.WriteAllText(plotterConfigPath, json);
        }


        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class OrderAttribute : Attribute
        {
            public OrderAttribute([CallerLineNumber] int order = 0)
            {
                Order = order;
            }

            public int Order { get; }
        }
    }
}