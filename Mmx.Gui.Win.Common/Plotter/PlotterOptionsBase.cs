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

        public static readonly string MM_PLOTER_HOME_ENV = Environment.GetEnvironmentVariable("MM_PLOTER_HOME");
        public static readonly string MM_PLOTER_HOME = string.IsNullOrEmpty(MM_PLOTER_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".plotterGui") : MM_PLOTER_HOME_ENV;

        static private string plotterConfigPath;

        static PlotterOptionsBase()
        {
            if(IsMmx)
            {
                plotterConfigPath = NodeHelpers.plotterConfigPath;
            } else
            {
                plotterConfigPath = Path.Combine(MM_PLOTER_HOME, "Plotter.json");
            }
        }

        public static bool IsMmx { get; set; } = !Convert.ToBoolean(ConfigurationManager.AppSettings["ChiaPlotter"]);

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