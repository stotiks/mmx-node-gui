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
        public BoolItem bb_mmx { get; set; } = new BoolItem
        {
            Name = "-mmx",
            LongName = "bb_mmx",
            DefaultValue = true,
            Persistent = false,
            Scope = Scopes.MmxBladebit            
        };

        [Order]
        public IntItem plotter { get; set; } = new IntItem
        {
            Name = "plotter",
            LongName = "plotter",
            DefaultValue = (int)Plotters.MmxPlotter,
            Type = ItemType.Other,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable().Select(value =>
                {
                    var isDefault = value == (int)Plotters.MmxPlotter;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = Enum.GetName(typeof(Plotters), value) + isDefaultString, Value = value };
                }).ToList()),
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
                Enumerable.Range(30, 5).Select(i =>
                {
                    var value = i;
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
                Enumerable.Range(1, 9).Select(i =>
                {
                    var value = i;
                    var isDefault = value == 1;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList()),
            Scope = Scopes.MadMaxPlottersCompressed
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
            Scope = Scopes.MadMaxCpuPlotters
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

            PlotterChanged();
            plotter.PropertyChanged += (sender, e) => PlotterChanged();

            NftPlotChanged();
            nftplot.PropertyChanged += (sender, e) => NftPlotChanged();
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
        }

        private void PlotterChanged()
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
