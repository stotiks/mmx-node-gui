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
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable().Select(value =>
                {
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
                Enumerable.Range(30, 5).Select(i =>
                {
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
                Enumerable.Range(4, 7).Select(i =>
                {
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
                Enumerable.Range(4, 7).Select(i =>
                {
                    var value = (int)Math.Pow(2, i);
                    var isDefault = value == BucketsDefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = value.ToString() + isDefaultString, Value = value };
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
            Scope = Scopes.MadMaxCpuPlotters
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
        public BoolItem nftplot { get; set; } = new BoolItem
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
                ((IEnumerable<int>)Enum.GetValues(typeof(ProcessPriorityClass))).AsEnumerable().Select(value =>
                {
                    var isDefault = value == (int)ProcessPriorityClass.Normal;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new Item<int> { Name = Enum.GetName(typeof(ProcessPriorityClass), value) + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.None
        };


        private PlotterOptions() : base()
        {
            InitPlotter();
            plotter.PropertyChanged += (sender, e) => InitPlotter();

            foreach (PropertyInfo property in GetItemProperties())
            {
                ((INotifyPropertyChanged)property.GetValue(this)).PropertyChanged += (sender, e) => NotifyPropertyChanged(nameof(PlotterCmd));
            }
        }

        private void InitPlotter()
        {
            foreach (PropertyInfo property in GetItemProperties().Where(property => property.Name != nameof(plotter)))
            {
                dynamic item = property.GetValue(this);
                Scopes plotterScopeEnum = (Scopes)plotter.Value;
                item.IsVisible = (item.Scope & plotterScopeEnum) == plotterScopeEnum;
            }

            if (plotter.Value == (int)Plotters.MmxBladebit)
            {
                finaldir.SkipName = true;
            }
            else
            {
                finaldir.SkipName = false;
            }
        }

        public string PlotterCmd => $"{PlotterExe} {PlotterArguments}";

        public string PlotterExe
        {
            get
            {
                var exe = "";
                switch (plotter.Value)
                {
                    case (int)Plotters.MmxPlotter:
                        exe = "mmx_plot.exe";
                        if (size.Value > 32)
                        {
                            exe = "mmx_plot_k34.exe";
                        }
                        break;
                    case (int)Plotters.MmxPlotterCompressed:
                        exe = "mmx_plot_c.exe";
                        if (size.Value > 32)
                        {
                            exe = "mmx_plot_k34_c.exe";
                        }
                        break;
                    case (int)Plotters.MmxCudaPlotter:
                        exe = $"mmx_cuda_plot_k{size.Value}.exe";
                        break;
                    case (int)Plotters.MmxBladebit:
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

                    if ((item.Scope & plotterScopeEnum) != plotterScopeEnum)
                    {
                        continue;
                    }

                    if (!item.IsPlotterParam || nftplot.Value && property.Name == nameof(poolkey) || !nftplot.Value && property.Name == nameof(contract))
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
