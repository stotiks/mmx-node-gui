using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterOptionsBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PlotterOptionsBase()
        {
            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged();
            }

            LoadJSON();

            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => Save();
            }
            
        }

        protected IEnumerable<PropertyInfo> GetItemProperties()
        {
            return GetType().GetProperties()
                                .Where(
                                       property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Item<>)
                                    || property.PropertyType.BaseType.IsGenericType && property.PropertyType.BaseType.GetGenericTypeDefinition() == typeof(Item<>)
                                 ).OrderBy(property => ((OrderAttribute)property
                                    .GetCustomAttributes(typeof(OrderAttribute))
                                    .Single()).Order);
        }


        private void LoadJSON()
        {
            string json = "{}";
            try
            {
                json = File.ReadAllText(NodeHelpers.plotterConfigPath);
            } 
            catch
            {
                //System.Console.WriteLine(@"config not found");
            }
            LoadJSON(json);
        }

        private void LoadJSON(string json)
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

        private void Save()
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
            File.WriteAllText(NodeHelpers.plotterConfigPath, json);
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

        public static string FixDir(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return "";

            dir = dir.Replace('/', '\\');

            if (dir.Length > 0 && dir.Last() != '\\')
            {
                dir += '\\';
            }

            return dir;
        }
    }
}