using Mmx.Gui.Win.Common.Node;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterOptions : PlotterOptionsBase
    {

        [AttributeUsage(AttributeTargets.All)]
        public class UrlAttribute : System.Attribute
        {
            public string Url { get; private set; }
            public double version;

            public UrlAttribute(string url)
            {
                this.Url = url;
                version = 1.0;
            }
        }

        public enum Plotters : int
        {
            [Description("CPU Plotter"), Url("https://github.com/madMAx43v3r/chia-plotter")]
            MadMaxChiaPlotter = 1 << 0,
            [Description("CPU Plotter v2.4 with compression"), Url("https://github.com/madMAx43v3r/chia-gigahorse/tree/master/cpu-plotter")]
            MadMaxChiaPlotterWithCompression = 1 << 1,
            [Description("Gigahorse CUDA plotter v2.5 with compression"), Url("https://github.com/madMAx43v3r/chia-gigahorse/tree/master/cuda-plotter")]
            MadMaxCudaPlotter = 1 << 2,
            [Description("Bladebit")]
            Bladebit = 1 << 8,
        };

        [Flags]
        public enum Scopes : short
        {
            None = 0,

            MadMaxChiaPlotter = Plotters.MadMaxChiaPlotter,
            MadMaxChiaPlotterWithCompression = Plotters.MadMaxChiaPlotterWithCompression,
            MadMaxCudaPlotter = Plotters.MadMaxCudaPlotter,
            Bladebit = Plotters.Bladebit,

            MadMaxCpuPlotters = MadMaxChiaPlotter | MadMaxChiaPlotterWithCompression,
            MadMaxPlotters = MadMaxChiaPlotter | MadMaxChiaPlotterWithCompression | MadMaxCudaPlotter,
            MadMaxPlottersWithCompression = MadMaxChiaPlotterWithCompression | MadMaxCudaPlotter,

            Common = MadMaxChiaPlotter | MadMaxChiaPlotterWithCompression | MadMaxCudaPlotter | Bladebit
        };

        [Order]
        public IntItem plotter { get; set; } = new IntItem
        {
            Name = "plotter",
            LongName = "plotter",
            DefaultValue = (int)Plotters.MadMaxChiaPlotter,
            Type = ItemType.Other,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable()
                    .Where(value => value != (int)Plotters.Bladebit && !( (IsMmxOnly && NodeHelpers.IsGigahorse == false) && (value == (int)Plotters.MadMaxCudaPlotter || value == (int)Plotters.MadMaxChiaPlotterWithCompression)) )
                    .Select(value =>
                        {
                            var isDefault = value == (int)Plotters.MadMaxChiaPlotter;
                            var isDefaultString = isDefault ? " (default)" : "";
                            Plotters plotterEnum = (Plotters)Enum.ToObject(typeof(Plotters), value);
                            var name = PlotterOptionsHelpers.GetDescription(plotterEnum);
                            return new ItemBase<int> { Name = name + isDefaultString, Value = value };
                        })
                    .ToList()),
            Scope = Scopes.Common
        };

        [Order]
        public IntItem count { get; set; } = new IntItem
        {
            Name = "n",
            LongName = "count",
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 999,
            Scope = Scopes.Common
        };

        [Order]
        public IntItem size { get; set; } = new IntItem
        {
            Name = "k",
            LongName = "size",
            DefaultValue = 32,
            Minimum = 30,
            Maximum = 34,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(30, 5).Select(value =>
                {
                    var isDefault = value == 32;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxPlotters
        };

        static readonly IDictionary<int, double> efficiencies = new Dictionary<int, double>()
        {
            {1, 120.4}, {2, 122.8}, {3, 125.2}, {4, 127.7},
            {5, 130.3}, {6, 133.1}, {7, 135.9}, {8, 142.2}, {9, 148.9},

            {11, 118.3}, {12, 122.9}, {13, 128.5}, {14, 135.7},
            {15, 141.7}, {16, 156.5}, {17, 160.9}, {18, 170},
            {19, 179.9}, {20, 190.8}

        };
        [Order]
        public IntItem level { get; set; } = new IntItem
        {
            Name = "C",
            LongName = "level",
            DefaultValue = 1,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(1, 9).Union(Enumerable.Range(11, 10)).Select(value =>
                {
                    var isDefault = value == 1;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = $"{value} - [{efficiencies[value]}%]{isDefaultString}", Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxPlottersWithCompression
        };

        enum Ports
        {
            [Description("Chia [8444]")]
            Chia = 8444,
            [Description("Chives [9699]")]
            Chives = 9699,
            [Description("MMX [11337]")]
            MMX = 11337
        }

        static readonly int PortDefaultValue = IsMmxOnly ? (int)Ports.MMX : (int)Ports.Chia;
        [Order]
        public IntItem port { get; set; } = new IntItem
        {
            Name = "x",
            LongName = "port",
            DefaultValue = PortDefaultValue,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Ports))).AsEnumerable()                    
                    .Select(value =>
                    {
                        var isDefault = value == PortDefaultValue;
                        var isDefaultString = isDefault ? " (default)" : "";
                        Ports portEnum = (Ports)Enum.ToObject(typeof(Ports), value);
                        var name = PlotterOptionsHelpers.GetDescription(portEnum);
                        return new ItemBase<int> { Name = name + isDefaultString, Value = value };
                    })
                    .ToList()),            
            Scope = Scopes.MadMaxPlotters,
            IsVisible = !IsMmxOnly
        };


        [Order]
        public IntItem device { get; set; } = new IntItem
        {
            Name = "g",
            LongName = "device",
            DefaultValue = 0,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(0, VideoDeviceInfo.Instance.CudaDevices.Count).Select(value =>
                {
                    var isDefault = value == 0;
                    var isDefaultString = isDefault ? " (default)" : "";
                    var name = VideoDeviceInfo.Instance.CudaDevices[value];
                    return new ItemBase<int> { Name = name + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxCudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public IntItem ndevices { get; set; } = new IntItem
        {
            Name = "r",
            LongName = "ndevices",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = VideoDeviceInfo.Instance.CudaDevices.Count,
            Scope = Scopes.MadMaxCudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public IntItem streams { get; set; } = new IntItem
        {
            Name = "S",
            LongName = "streams",
            DefaultValue = 3,
            Minimum = 2,
            Maximum = 16,
            Scope = Scopes.MadMaxCudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public IntItem chunksize { get; set; } = new IntItem
        {
            Name = "B",
            LongName = "chunksize",
            DefaultValue = 16,
            Minimum = 1,
            Maximum = 256,
            Scope = Scopes.MadMaxCudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public BoolItem pin_memory { get; set; } = new BoolItem
        {
            Name = "pin_memory",
            LongName = "pin_memory",
            DefaultValue = true,
            Scope = Scopes.MadMaxCudaPlotter,
            Skip = true   
        };

        [Order]
        public IntItem memory { get; set; } = new IntItem
        {
            Name = "M",
            LongName = "memory",
            DefaultValue = Convert.ToInt32(NativeMethods.GetTotalMemoryInGigaBytes() / 2.0),
            Minimum = 0,
            Maximum = NativeMethods.GetTotalMemoryInGigaBytes(),
            Scope = Scopes.MadMaxCudaPlotter
        };

        [Order]
        public IntItem threads { get; set; } = new IntItem
        {
            Name = "r",
            LongName = "threads",
            DefaultValue = Environment.ProcessorCount,
            Minimum = 1,
            Maximum = 128,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public IntItem rmulti2 { get; set; } = new IntItem
        {
            Name = "K",
            LongName = "rmulti2",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 8,
            Scope = Scopes.MadMaxCpuPlotters,
            SuppressDefaultValue = true
        };

        private const int BucketsDefaultValue = 256;

        [Order]
        public IntItem buckets { get; set; } = new IntItem
        {
            Name = "u",
            LongName = "buckets",
            DefaultValue = BucketsDefaultValue,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(4, 7).Select(i =>
                {
                    var value = (int)Math.Pow(2, i);
                    var isDefault = value == BucketsDefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public IntItem buckets3 { get; set; } = new IntItem
        {
            Name = "v",
            LongName = "buckets3",
            DefaultValue = BucketsDefaultValue,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(4, 7).Select(i =>
                {
                    var value = (int)Math.Pow(2, i);
                    var isDefault = value == BucketsDefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public PathItem finaldir { get; set; } = new PathItem
        {
            Name = "d",
            LongName = "finaldir",
            DefaultValue = "",
            Scope = Scopes.Common ^ Scopes.MadMaxCudaPlotter
        };

        [Order]
        public MultiPathItem multifinaldir { get; set; } = new MultiPathItem
        {
            Name = "d",
            LongName = "multifinaldir",
            DefaultValue = new ArrayList() { "" },
            Scope = Scopes.MadMaxCudaPlotter
        };

        [Order]
        public PathItem tmpdir { get; set; } = new PathItem
        {
            Name = "t",
            LongName = "tmpdir",
            DefaultValue = "",
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public PathItem tmpdir2 { get; set; } = new PathItem
        {
            Name = "2",
            LongName = "tmpdir2",
            DefaultValue = "",
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public PathItem tmpdir3 { get; set; } = new PathItem
        {
            Name = "3",
            LongName = "tmpdir3",
            DefaultValue = "",
            Scope = Scopes.MadMaxCudaPlotter
        };

        [Order]
        public PathItem stagedir { get; set; } = new PathItem
        {
            Name = "s",
            LongName = "stagedir",
            DefaultValue = "",
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public BoolItem waitforcopy { get; set; } = new BoolItem
        {
            Name = "w",
            LongName = "waitforcopy",
            DefaultValue = false,
            Scope = Scopes.MadMaxPlotters
        };

        [Order]
        public BoolItem tmptoggle { get; set; } = new BoolItem
        {
            Name = "G",
            LongName = "tmptoggle",
            DefaultValue = false,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public BoolItem directout { get; set; } = new BoolItem
        {
            Name = "D",
            LongName = "directout",
            DefaultValue = false,
            Scope = Scopes.MadMaxCpuPlotters
        };

        [Order]
        public StringItem farmerkey { get; set; } = new StringItem
        {
            Name = "f",
            LongName = "farmerkey",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public StringItem poolkey { get; set; } = new StringItem
        {
            Name = "p",
            LongName = "poolkey",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public StringItem contract { get; set; } = new StringItem
        {
            Name = "c",
            LongName = "contract",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public BoolItem nftplot { get; set; } = new BoolItem
        {
            Name = "nftplot",
            LongName = "nftplot",
            DefaultValue = false,
            Type = ItemType.Other,
            Scope = Scopes.Common
        };

        [Order]
        public IntItem priority { get; set; } = new IntItem
        {
            Name = "priority",
            LongName = "priority",
            DefaultValue = (int)ProcessPriorityClass.Normal,
            Type = ItemType.Other,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<ProcessPriorityClass>)Enum.GetValues(typeof(ProcessPriorityClass))).AsEnumerable()
                .OrderBy(value => {
                    switch (value)
                    {
                        case ProcessPriorityClass.Idle: return 10;
                        case ProcessPriorityClass.BelowNormal: return 20;
                        case ProcessPriorityClass.Normal: return 30;
                        case ProcessPriorityClass.AboveNormal: return 40;
                        case ProcessPriorityClass.High: return 50;
                        case ProcessPriorityClass.RealTime: return 60;
                        default: return 30;
                    }
                })
                .Select(value =>
                {
                    var isDefault = value == ProcessPriorityClass.Normal;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = Enum.GetName(typeof(ProcessPriorityClass), value) + isDefaultString, Value = (int)value };
                }).ToList()),
            Scope = Scopes.Common
        };


        private PlotterOptions() : base()
        {
            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged();
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged(nameof(PlotterCmd));
            }

            LoadJSON();

            plotter.PropertyChanged += (sender, e) => PlotterChanged();
            PlotterChanged();
            
            nftplot.PropertyChanged += (sender, e) => NftPlotChanged();
            NftPlotChanged();
            
            pin_memory.PropertyChanged += (sender, e) => PinMemoryChanged();
            PinMemoryChanged();

            if(IsMmxOnly)
            {
                port.Value = (int)Ports.MMX;
                NotifyPropertyChanged(nameof(PlotterCmd));
            }

            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => DebouncedSave();
            }
        }

        private void PinMemoryChanged()
        {
            memory.Skip = !pin_memory.Value;
        }

        private void NftPlotChanged()
        {
            if(nftplot.Value)
            {
                contract.Type = ItemType.CmdParameter;
                poolkey.Type = ItemType.Hidden;
            } else
            {
                contract.Type = ItemType.Hidden;
                poolkey.Type = ItemType.CmdParameter;
            }
            NotifyPropertyChanged(nameof(PlotterCmd));
        }

        private void PlotterChanged()
        {
            foreach (PropertyInfo property in GetItemProperties().Where(property => property.Name != nameof(plotter) && property.Name != nameof(port)))
            {
                dynamic item = property.GetValue(this);
                Scopes plotterScopeEnum = (Scopes)plotter.Value;
                item.IsVisible = (item.Scope & plotterScopeEnum) == plotterScopeEnum;
            }

            var isCudaPlotter = plotter.Value == (int)Plotters.MadMaxCudaPlotter;
            size.Skip = isCudaPlotter;
        }

        public string PlotterCmd => $"{PlotterExe} {PlotterArguments}";

        public string PlotterExe
        {
            get
            {
                var gigahorsePath = "gigahorse\\";
                var exe = "";
                var chia_plot_name = IsMmxOnly ? "mmx_plot" : "chia_plot";
                switch (plotter.Value)
                {
                    case (int)Plotters.MadMaxChiaPlotter:
                        exe = $"{chia_plot_name}.exe";
                        if (size.Value > 32)
                        {
                            exe = $"{chia_plot_name}_k34.exe";
                        }
                        break;
                    case (int)Plotters.MadMaxChiaPlotterWithCompression:
                        exe = $"{gigahorsePath}chia_plot.exe";
                        if (size.Value > 32)
                        {
                            exe = $"{gigahorsePath}chia_plot.exe";
                        }
                        break;
                    case (int)Plotters.MadMaxCudaPlotter:
                        exe = $"{gigahorsePath}cuda_plot_k{size.Value}.exe";
                        break;
                    case (int)Plotters.Bladebit:
                        exe = "mmx_bladebit.exe";
                        break;
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

                    Scopes plotterScopeEnum = (Scopes)plotter.Value;

                    if( (item.Scope & plotterScopeEnum) == plotterScopeEnum && item.Type == ItemType.CmdParameter )
                    {
                        var param = item.GetParam();
                        if (!string.IsNullOrEmpty(param))
                        {
                            result += string.Format(" {0}", param);
                        }
                    }
                }

                return result;
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly PlotterOptions instance = new PlotterOptions();
        }

        public static PlotterOptions Instance => Nested.instance;

    }

}
