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

    public enum Plotters : int
    {
        MmxPlotter           = 1 << 0,
        MmxPlotterCompressed = 1 << 1,
        MmxCudaPlotter       = 1 << 2,
        MmxBladebit          = 1 << 8,
    };

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

        private bool _isVisible;
        public bool IsVisible { 
            get => _isVisible;
            internal set
            {
                _isVisible = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsPlotterParam { get; internal set; } = true;
        public PlotterOptions.Scopes Scope { get; internal set; } = PlotterOptions.Scopes.None;

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
        [Flags]
        public enum Scopes : short
        {
            None = 0,
            MmxPlotter = Plotters.MmxPlotter,
            MmxPlotterCompressed = Plotters.MmxPlotterCompressed,
            MmxCudaPlotter = Plotters.MmxCudaPlotter,
            MmxBladebit = Plotters.MmxBladebit,

            MadMaxCpuPlotters = MmxPlotter | MmxPlotterCompressed,
            MadMaxPlotters = MmxPlotter | MmxPlotterCompressed | MmxCudaPlotter,
            MadMaxPlottersCompressed = MmxPlotterCompressed | MmxCudaPlotter,
            Common = MmxPlotter | MmxPlotterCompressed | MmxCudaPlotter | MmxBladebit
        };

        [Order]
        public Item<int> plotter { get; set; } = new Item<int>
        {
            Name = "plotter",
            LongName = "plotter",
            DefaultValue = (int)Plotters.MmxPlotter,
            IsPlotterParam = false,
            Items = new ObservableCollection<Item<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable().Select(value => {
                    var isDefault = value == (int)Plotters.MmxPlotter;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = Enum.GetName(typeof(Plotters), value) + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.Common
        };

        [Order]
        public Item<int> count { get; set; } = new Item<int>
        {
            Name = "n",
            LongName = "count",
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 999,
            Scope = Scopes.Common
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
                }).ToList()),
            Scope = Scopes.Common ^ Scopes.MmxBladebit
        };

        [Order]
        public Item<int> level { get; set; } = new Item<int>
        {
            Name = "C",
            LongName = "level",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 9,
            Items = new ObservableCollection<Item<int>>(
                Enumerable.Range(1, 9).Select(i =>
                {
                    var value = i;
                    var isDefault = value == 1;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxPlottersCompressed
        };

        [Order]
        public Item<int> port { get; set; } = new Item<int>
        {
            Name = "x",
            LongName = "port",
            DefaultValue = 11337,
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public Item<int> threads { get; set; } = new Item<int>
        {
            Name = "r",
            LongName = "threads",
            DefaultValue = Environment.ProcessorCount,
            Minimum = 1,
            Maximum = 128,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<int> rmulti2 { get; set; } = new Item<int>
        {
            Name = "K",
            LongName = "rmulti2",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 8,
            Scope = Scopes.MadMaxCpuPlotters
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
                }).ToList()),
            Scope = Scopes.MadMaxCpuPlotters
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
                }).ToList()),
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<string> finaldir { get; set; } = new Item<string>
        {
            Name = "d",
            LongName = "finaldir",
            DefaultValue = "",
            AddQuotes = true,
            Scope = Scopes.Common
        };

        [Order]
        public Item<string> tmpdir { get; set; } = new Item<string>
        {
            Name = "t",
            LongName = "tmpdir",
            DefaultValue = "",
            AddQuotes = true,
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public Item<string> tmpdir2 { get; set; } = new Item<string>
        {
            Name = "2",
            LongName = "tmpdir2",
            DefaultValue = "",
            AddQuotes = true,
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public Item<string> stagedir { get; set; } = new Item<string>
        {
            Name = "s",
            LongName = "stagedir",
            DefaultValue = "",
            AddQuotes = true,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<bool> waitforcopy { get; set; } = new Item<bool>
        {
            Name = "w",
            LongName = "waitforcopy",
            DefaultValue = false,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<bool> tmptoggle { get; set; } = new Item<bool>
        {
            Name = "G",
            LongName = "tmptoggle",
            DefaultValue = false,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<bool> directout { get; set; } = new Item<bool>
        {
            Name = "D",
            LongName = "directout",
            DefaultValue = false,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public Item<string> farmerkey { get; set; } = new Item<string>
        {
            Name = "f",
            LongName = "farmerkey",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public Item<string> poolkey { get; set; } = new Item<string>
        {
            Name = "p",
            LongName = "poolkey",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public Item<string> contract { get; set; } = new Item<string>
        {
            Name = "c",
            LongName = "contract",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public Item<bool> nftplot { get; set; } = new Item<bool>
        {
            Name = "nftplot",
            LongName = "nftplot",
            DefaultValue = false,
            IsPlotterParam = false,
            Scope = Scopes.Common
        };

        [Order]
        public Item<int> priority { get; set; } = new Item<int>
        {
            Name = "priority",
            LongName = "priority",
            DefaultValue = 0x20,
            IsPlotterParam = false,
            Items = new ObservableCollection<Item<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(ProcessPriorityClass))).AsEnumerable().Select(value => {
                    var isDefault = value == (int)ProcessPriorityClass.Normal;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = Enum.GetName(typeof(ProcessPriorityClass), value) + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.None
        };

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlotterCmd)));
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

            plotter.PropertyChanged += (sender, e) =>
            {
                foreach (PropertyInfo property in GetItemProperties().Where(property => property.Name != nameof(plotter)))
                {
                    dynamic item = property.GetValue(this);
                    Scopes plotterScopeEnum = (Scopes)plotter.Value;
                    item.IsVisible = (item.Scope & plotterScopeEnum) == plotterScopeEnum;
                }
            };

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

                    Scopes plotterScopeEnum = (Scopes) plotter.Value;

                    if ((item.Scope & plotterScopeEnum) != plotterScopeEnum)
                    {
                        continue;
                    }

                    if ( !item.IsPlotterParam || nftplot.Value && property.Name == nameof(poolkey) || !nftplot.Value && property.Name == nameof(contract) )
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
