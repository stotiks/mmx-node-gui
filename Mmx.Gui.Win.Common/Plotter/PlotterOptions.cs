using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class Item<T> : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string LongName { get; set; }

        private bool _valueInitialized;
        private T _value;
        public T Value
        {
            get => _value;
            set {
                _value = value;
                _valueInitialized = true;
                NotifyPropertyChanged();
            }
        }

        private T _defaultValue;
        public T DefaultValue
        {  
            get => _defaultValue;
            internal set {
                _defaultValue = value;
                if (_valueInitialized == false)
                {
                    _value = _defaultValue;
                }
                NotifyPropertyChanged();
            }
        }

        public bool IsPlotterParam { get; set; } = true;

        public ObservableCollection<Item<T>> Items { get; internal set; }
        public T Minimum { get; internal set; }
        public T Maximum { get; internal set; }
        public bool AddQuotes { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                if (typeof(T) == typeof(bool))
                {
                    if ((bool)(object)Value)
                    {
                        result = $"-{Name}";
                    }
                }
                else if (!string.IsNullOrEmpty(Value.ToString()))
                {
                    var value = Value.ToString();
                    if(AddQuotes && value.Contains(" "))
                    {
                        value = $"\"{value}\"";
                        value = value.Replace("\\\"", "\\\\\"");
                    }
                    result = $"-{Name} {value}";
                }
            }

            return result;
        }

    }

    public class PlotterOptions : INotifyPropertyChanged
    {
        [Order]
        public Item<int> count { get; set; } = new Item<int>
        {
            Name = "n",
            LongName = "count",
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 999
        };


        [Order]
        public Item<int> size { get; set; } = new Item<int>
        {
            Name = "k",
            LongName = "size",
            DefaultValue = 32,
            Minimum = 30,
            Maximum = 34,
            Items = new ObservableCollection<Item<int>>(
                Enumerable.Range(30, 5).Select(i => {
                    var value = i;
                    var isDefault = value == 32;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList())
        };

        [Order]
        public Item<int> port { get; set; } = new Item<int>
        {
            Name = "x",
            LongName = "port",
            DefaultValue = 11337
        };

        [Order]
        public Item<int> threads { get; set; } = new Item<int>
        {
            Name = "r",
            LongName = "threads",
            DefaultValue = Environment.ProcessorCount,
            Minimum = 1,
            Maximum = 128
        };

        [Order]
        public Item<int> rmulti2 { get; set; } = new Item<int>
        {
            Name = "K",
            LongName = "rmulti2",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 8
        };

        private const int BucketsDefaultValue = 256;

        [Order]
        public Item<int> buckets { get; set; } = new Item<int>
        {
            Name = "u",
            LongName = "buckets",
            DefaultValue = BucketsDefaultValue,
            Items = new ObservableCollection<Item<int>>(
                Enumerable.Range(4, 7).Select(i => {
                    var value = (int)Math.Pow(2, i);
                    var isDefault = value == BucketsDefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList())
        };

        [Order]
        public Item<int> buckets3 { get; set; } = new Item<int>
        {
            Name = "v",
            LongName = "buckets3",
            DefaultValue = BucketsDefaultValue,
            Items = new ObservableCollection<Item<int>>(
                Enumerable.Range(4, 7).Select(i => {
                    var value = (int)Math.Pow(2, i);
                    var isDefault = value == BucketsDefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList())
        };

        [Order]
        public Item<string> finaldir { get; set; } = new Item<string>
        {
            Name = "d",
            LongName = "finaldir",
            DefaultValue = "",
            AddQuotes = true
        };

        [Order]
        public Item<string> tmpdir { get; set; } = new Item<string>
        {
            Name = "t",
            LongName = "tmpdir",
            DefaultValue = "",
            AddQuotes = true
        };

        [Order]
        public Item<string> tmpdir2 { get; set; } = new Item<string>
        {
            Name = "2",
            LongName = "tmpdir2",
            DefaultValue = "",
            AddQuotes = true
        };

        [Order]
        public Item<string> stagedir { get; set; } = new Item<string>
        {
            Name = "s",
            LongName = "stagedir",
            DefaultValue = "",
            AddQuotes = true
        };

        [Order]
        public Item<bool> waitforcopy { get; set; } = new Item<bool>
        {
            Name = "w",
            LongName = "waitforcopy",
            DefaultValue = false
        };

        [Order]
        public Item<bool> tmptoggle { get; set; } = new Item<bool>
        {
            Name = "G",
            LongName = "tmptoggle",
            DefaultValue = false
        };

        [Order]
        public Item<bool> directout { get; set; } = new Item<bool>
        {
            Name = "D",
            LongName = "directout",
            DefaultValue = false,
        };

        [Order]
        public Item<string> farmerkey { get; set; } = new Item<string>
        {
            Name = "f",
            LongName = "farmerkey",
            DefaultValue = ""
        };

        [Order]
        public Item<string> poolkey { get; set; } = new Item<string>
        {
            Name = "p",
            LongName = "poolkey",
            DefaultValue = ""
        };

        [Order]
        public Item<string> contract { get; set; } = new Item<string>
        {
            Name = "c",
            LongName = "contract",
            DefaultValue = ""
        };

        [Order]
        public Item<bool> nftplot { get; set; } = new Item<bool>
        {
            Name = "nftplot",
            LongName = "nftplot",
            DefaultValue = false,
            IsPlotterParam = false
        };

        [Order]
        public Item<int> priority { get; set; } = new Item<int>
        {
            Name = "priority",
            LongName = "priority",
            DefaultValue = 0x20,
            IsPlotterParam = false,
            Items = new ObservableCollection<Item<int>>(
                //ProcessPriorityClass.
                ((IEnumerable<int>)Enum.GetValues(typeof(ProcessPriorityClass))).AsEnumerable().Select(i => {
                    var value = i;
                    var isDefault = value == 0x20;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = Enum.GetName(typeof(ProcessPriorityClass), i) + isDefaultString, Value = value };
                }).ToList())
        };

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlotterCmd"));
            Save();
        }

        public static PlotterOptions Instance => Nested.instance;

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested(){}

            internal static readonly PlotterOptions instance = new PlotterOptions();
        }

        private PlotterOptions()
        {

            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged();
            }

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
                    item.Value = Convert.ChangeType(configItem.Value, property.PropertyType.GenericTypeArguments[0]);
                }
            }
        }

        private void Save()
        {
            dynamic jObject = new JObject();

            foreach (PropertyInfo property in GetItemProperties())
            {
                dynamic item = property.GetValue(this);
                jObject.Add(item.LongName, item.Value);
            }

            var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            File.WriteAllText(NodeHelpers.plotterConfigPath, json);
        }

        public string PlotterCmd => $"{PlotterExe} {PlotterArguments}";

        public string PlotterExe
        {
            get {
                var exe = "mmx_plot.exe";
                if (size.Value > 32)
                {
                    exe = "mmx_plot_k34.exe";
                }

                return exe;
            }
        }

        public string PlotterArguments
        {
            get
            {
                var result = "";
                foreach (PropertyInfo property in GetItemProperties())
                {
                    dynamic item = property.GetValue(this);

                    if ( !item.IsPlotterParam ||
                         nftplot.Value && property.Name == nameof(poolkey) ||
                         !nftplot.Value && property.Name == nameof(contract)
                       )
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(item.GetParam()))
                    {
                        result += string.Format(" {0}", item.GetParam());
                    }

                }

                return result;
            }
        }

        private IEnumerable<PropertyInfo> GetItemProperties()
        {
            return GetType().GetProperties()
                            .Where(property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Item<>))
                            .OrderBy(property => ((OrderAttribute)property
                                .GetCustomAttributes(typeof(OrderAttribute))
                                .Single()).Order);
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
