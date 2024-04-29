using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using static Mmx.Gui.Win.Common.Plotter.PlotterOptions;

namespace Mmx.Gui.Win.Common.Plotter
{
    public class PlotterOptionsItems : PlotterOptionsBase
    {

        private const int BucketsDefaultValue = 256;

        protected static readonly IDictionary<int, double> efficiencies = new Dictionary<int, double>()
        {
            {1, 120.4}, {2, 122.8}, {3, 125.2}, {4, 127.7},
            {5, 130.3}, {6, 133.1}, {7, 135.9}, {8, 142.2}, {9, 148.9},

            {11, 118.3}, {12, 122.9}, {13, 128.5}, {14, 135.7},
            {15, 141.7}, {16, 156.5}, {17, 160.9}, {18, 170},
            {19, 179.9}, {20, 190.8},

            {29, 211}, {30, 234}, {31, 263}, {32, 300}, {33, 350}

        };

        [Order]
        public IntItem plotter { get; set; } = new IntItem
        {
            Name = "plotter",
            LongName = "plotter",
            DefaultValue = (int)Plotters.ChiaCpuPlotter,
            Type = ItemType.Other,
            Items = new ObservableCollection<ItemBase<int>>(
                ((IEnumerable<int>)Enum.GetValues(typeof(Plotters))).AsEnumerable()
                    .Where(value => IsMmxOnly && ((Scopes)value).HasFlag(Scopes.MmxPlotters) || !IsMmxOnly)
                    .Select(value =>
                        {
                            var isDefault = value == (int)Plotters.ChiaCpuPlotter;
                            var isDefaultString = isDefault ? " (default)" : "";
                            Plotters plotterEnum = (Plotters)Enum.ToObject(typeof(Plotters), value);
                            var name = PlotterOptionsHelpers.GetDescription(plotterEnum);
                            return new ItemBase<int> { Name = name + isDefaultString, Value = value };
                        })
                    .ToList()),
            Scope = Scopes.Common,
            IsVisible = !IsMmxOnly
        };
        [Order]
        public IntItem count { get; set; } = new IntItem
        {
            Name = "n",
            LongName = "count",
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 99999,
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
            Scope = Scopes.Common ^ Scopes.ChiaCudaPlotter_30
        };

        [Order]
        public IntItem level { get; set; } = new IntItem
        {
            Name = "C",
            LongName = "level",
            DefaultValue = 1,
            Scope = Scopes.PlottersWithCompression
        };

        [Order]
        public BoolItem ssdplot { get; set; } = new BoolItem
        {
            LongName = "ssd",
            UseCmdLongName = true,
            DefaultValue = false,
            Scope = Scopes.MmxPlotters
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
            Scope = Scopes.CudaPlotters,
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
            Scope = Scopes.CudaPlotters,
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
            Scope = Scopes.CudaPlotters,
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
            Scope = Scopes.CudaPlotters,
            SuppressDefaultValue = true
        };

        [Order]
        public BoolItem pin_memory { get; set; } = new BoolItem
        {
            Name = "pin_memory",
            LongName = "pin_memory",
            DefaultValue = true,
            Scope = Scopes.CudaPlotters,
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
            Scope = Scopes.CudaPlotters
        };

        [Order]
        public IntItem threads { get; set; } = new IntItem
        {
            Name = "r",
            LongName = "threads",
            DefaultValue = Environment.ProcessorCount,
            Minimum = 1,
            Maximum = 128,
            Scope = Scopes.CpuPlotters
        };

        [Order]
        public IntItem rmulti2 { get; set; } = new IntItem
        {
            Name = "K",
            LongName = "rmulti2",
            DefaultValue = 1,
            Minimum = 1,
            Maximum = 8,
            Scope = Scopes.CpuPlotters,
            SuppressDefaultValue = true
        };

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
            Scope = Scopes.CpuPlotters
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
            Scope = Scopes.CpuPlotters
        };

        [Order]
        public PathItem finaldir { get; set; } = new PathItem
        {
            Name = "d",
            LongName = "finaldir",
            DefaultValue = "",
            Scope = Scopes.Common ^ Scopes.CudaPlotters
        };

        [Order]
        public MultiPathItem multifinaldir { get; set; } = new MultiPathItem
        {
            Name = "d",
            LongName = "multifinaldir",
            DefaultValue = new ArrayList() { "" },
            Scope = Scopes.CudaPlotters
        };

        [Order]
        public PathItem tmpdir { get; set; } = new PathItem
        {
            Name = "t",
            LongName = "tmpdir",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public PathItem tmpdir2 { get; set; } = new PathItem
        {
            Name = "2",
            LongName = "tmpdir2",
            DefaultValue = "",
            Scope = Scopes.Common
        };

        [Order]
        public PathItem tmpdir3 { get; set; } = new PathItem
        {
            Name = "3",
            LongName = "tmpdir3",
            DefaultValue = "",
            Scope = Scopes.CudaPlotters
        };

        [Order]
        public PathItem stagedir { get; set; } = new PathItem
        {
            Name = "s",
            LongName = "stagedir",
            DefaultValue = "",
            Scope = Scopes.CpuPlotters
        };

        [Order]
        public BoolItem waitforcopy { get; set; } = new BoolItem
        {
            Name = "w",
            LongName = "waitforcopy",
            DefaultValue = false,
            Scope = Scopes.Common
        };

        [Order]
        public BoolItem tmptoggle { get; set; } = new BoolItem
        {
            Name = "G",
            LongName = "tmptoggle",
            DefaultValue = false,
            Scope = Scopes.CpuPlotters
        };

        [Order]
        public BoolItem directout { get; set; } = new BoolItem
        {
            Name = "D",
            LongName = "directout",
            DefaultValue = false,
            Scope = Scopes.CpuPlotters
        };

        [Order]
        public IntItem maxtmp { get; set; } = new IntItem
        {
            Name = "Q",
            LongName = "maxtmp",
            SuppressDefaultValue = true,
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 99999,
            Scope = Scopes.CudaPlotters
        };

        [Order]
        public IntItem copylimit { get; set; } = new IntItem
        {
            Name = "A",
            LongName = "copylimit",
            SuppressDefaultValue = true,
            DefaultValue = -1,
            Minimum = -1,
            Maximum = 99999,
            Scope = Scopes.CudaPlotters
        };

        [Order]
        public IntItem maxcopy { get; set; } = new IntItem
        {
            Name = "W",
            LongName = "maxcopy",
            SuppressDefaultValue = true,
            DefaultValue = 1,
            Minimum = -1,
            Maximum = 99999,
            Scope = Scopes.CudaPlotters
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
            Scope = Scopes.Common ^ Scopes.MmxPlotters
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
    }
}