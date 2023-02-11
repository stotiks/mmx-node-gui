using Mmx.Gui.Win.Common.Node;
using System;
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
        [Flags]
        public enum Scopes : short
        {
            None = 0,

            ChiaPlotter = Plotters.ChiaPlotter,
            ChiaPlotterWithCompression = Plotters.ChiaPlotterWithCompression,
            CudaPlotter = Plotters.CudaPlotter,
            Bladebit = Plotters.Bladebit,

            MadMaxCpuPlotters = ChiaPlotter | ChiaPlotterWithCompression,
            MadMaxPlotters = ChiaPlotter | ChiaPlotterWithCompression | CudaPlotter,
            MadMaxPlottersWithCompression = ChiaPlotterWithCompression | CudaPlotter,

            Common = ChiaPlotter | ChiaPlotterWithCompression | CudaPlotter | Bladebit
        };

        [Order]
        public IntItem plotter { get; set; } = new IntItem
        {
            Name = "plotter",
            LongName = "plotter",
            DefaultValue = (int)Plotters.ChiaPlotter,
            Type = ItemType.Other,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable()
                    .Where(value => value != (int)Plotters.Bladebit && !(NodeHelpers.IsGigahorse == false && (value == (int)Plotters.CudaPlotter || value == (int)Plotters.ChiaPlotterWithCompression)) )
                    .Select(value =>
                        {
                            var isDefault = value == (int)Plotters.ChiaPlotter;
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

        [Order]
        public IntItem level { get; set; } = new IntItem
        {
            Name = "C",
            LongName = "level",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 9,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(1, 9).Select(value =>
                {
                    var isDefault = value == 1;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxPlottersWithCompression
        };

        [Order]
        public IntItem port { get; set; } = new IntItem
        {
            Name = "x",
            LongName = "port",
            DefaultValue = 11337,
            Scope = Scopes.MadMaxPlotters
        };


        [Order]
        public IntItem device { get; set; } = new IntItem
        {
            Name = "g",
            LongName = "device",
            DefaultValue = 0,
            Items = new ObservableCollection<ItemBase<int>>(
                Enumerable.Range(0, CudaInfo.Instance.Devices.Count).Select(value =>
                {
                    var isDefault = value == 0;
                    var isDefaultString = isDefault ? " (default)" : "";
                    var name = CudaInfo.Instance.Devices[value]["Name"];
                    return new ItemBase<int> { Name = name + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.CudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public IntItem ndevices { get; set; } = new IntItem
        {
            Name = "r",
            LongName = "ndevices",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = CudaInfo.Instance.Devices.Count,
            Scope = Scopes.CudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public IntItem streams { get; set; } = new IntItem
        {
            Name = "S",
            LongName = "streams",
            DefaultValue = 4,
            Minimum = 2,
            Maximum = 16,
            Scope = Scopes.CudaPlotter,
            SuppressDefaultValue = true
        };

        [Order]
        public BoolItem pin_memory { get; set; } = new BoolItem
        {
            Name = "pin_memory",
            LongName = "pin_memory",
            DefaultValue = true,
            Scope = Scopes.CudaPlotter,
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
            Scope = Scopes.CudaPlotter
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
            Scope = Scopes.Common
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
                ((IEnumerable<int>)Enum.GetValues(typeof(ProcessPriorityClass))).AsEnumerable().Select(value =>
                {
                    var isDefault = value == (int)ProcessPriorityClass.Normal;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = Enum.GetName(typeof(ProcessPriorityClass), value) + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.Common
        };


        private PlotterOptions() : base()
        {
            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged(nameof(PlotterCmd));
            }
            
            plotter.PropertyChanged += (sender, e) => PlotterChanged();
            PlotterChanged();
            
            nftplot.PropertyChanged += (sender, e) => NftPlotChanged();
            NftPlotChanged();
            
            pin_memory.PropertyChanged += (sender, e) => PinMemoryChanged();
            PinMemoryChanged();
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
            foreach (PropertyInfo property in GetItemProperties().Where(property => property.Name != nameof(plotter)))
            {
                dynamic item = property.GetValue(this);
                Scopes plotterScopeEnum = (Scopes)plotter.Value;
                item.IsVisible = (item.Scope & plotterScopeEnum) == plotterScopeEnum;
            }

            var isCudaPlotter = plotter.Value == (int)Plotters.CudaPlotter;
            size.Skip = isCudaPlotter;
        }

        public string PlotterCmd => $"{PlotterExe} {PlotterArguments}";

        public string PlotterExe
        {
            get
            {
                var gigahorsePath = "gigahorse\\";
                var exe = "";
                var chia_plot_name = IsMmx ? "mmx_plot" : "chia_plot";
                switch (plotter.Value)
                {
                    case (int)Plotters.ChiaPlotter:
                        exe = $"{chia_plot_name}.exe";
                        if (size.Value > 32)
                        {
                            exe = $"{chia_plot_name}_k34.exe";
                        }
                        break;
                    case (int)Plotters.ChiaPlotterWithCompression:
                        exe = $"{gigahorsePath}chia_plot.exe";
                        if (size.Value > 32)
                        {
                            exe = $"{gigahorsePath}chia_plot.exe";
                        }
                        break;
                    case (int)Plotters.CudaPlotter:
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
