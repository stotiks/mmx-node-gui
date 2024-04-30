using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mmx.Gui.Win.Common.Plotter
{
    public partial class PlotterOptions : PlotterOptionsItems
    {
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
                plotter.Value = (int)Plotters.MmxCudaPlotter;
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
            foreach (PropertyInfo property in GetItemProperties().Where(property => property.Name != nameof(plotter)))
            {
                dynamic item = property.GetValue(this);
                item.IsVisible = item.Scope.HasFlag((Scopes)plotter.Value);
            }

            size.Skip = ((Plotters)plotter.Value) == Plotters.ChiaCudaPlotter_25
                || ((Plotters)plotter.Value) == Plotters.ChiaCudaPlotter_30
                || ((Plotters)plotter.Value) == Plotters.MmxCudaPlotter;


            UpdateLevel();
            UpdateSize();
        }

        private void UpdateSize()
        {
            size.DefaultValue = 32;
            IEnumerable<int> size_enum;
            switch ((Plotters)plotter.Value)
            {
                case Plotters.MmxCudaPlotter:
                    size_enum = Enumerable.Range(29, 4);
                    break;
                default:
                    size_enum = Enumerable.Range(30, 5);
                    break;
            }

            size.Items = new ObservableCollection<ItemBase<int>>(
                size_enum.Select(value =>
                {
                    var isDefault = value == size.DefaultValue;
                    var isDefaultString = isDefault ? " (default)" : "";
                    return new ItemBase<int> { Name = value.ToString() + isDefaultString, Value = value };
                }).ToList());

            if (!size_enum.Contains(size.Value))
            {
                size.Value = size.DefaultValue;
            }
        }

        private void UpdateLevel()
        {
            IEnumerable<int> level_enum;
            switch ((Plotters)plotter.Value)
            {
                case Plotters.ChiaCudaPlotter_30:
                    level_enum = Enumerable.Range(29, 5);
                    level.DefaultValue = 30;
                    break;
                case Plotters.ChiaCudaPlotter_25:
                    level_enum = Enumerable.Range(1, 9).Union(Enumerable.Range(11, 10));
                    level.DefaultValue = 1;
                    break;
                case Plotters.MmxCudaPlotter:
                    level_enum = Enumerable.Range(1, 15);
                    level.DefaultValue = 1;
                    break;
                default:
                    level_enum = Enumerable.Range(1, 9);
                    level.DefaultValue = 1;
                    break;
            }

            level.Items = new ObservableCollection<ItemBase<int>>(
                level_enum.Select(value =>
                {
                    var isDefault = value == level.DefaultValue;
                    var isDefaultStr = isDefault ? " (default)" : "";
                    _ = efficiencies.TryGetValue(value, out double efficiency);
                    var efficiencyStr = efficiency > 0 && !Scopes.MmxPlotters.HasFlag((Scopes)plotter.Value) ? $"- [{efficiency}%]" : "";
                    return new ItemBase<int> { Name = $"{value} {efficiencyStr}{isDefaultStr}", Value = value };
                }).ToList());

            if (!level_enum.Contains(level.Value))
            {
                level.Value = level.DefaultValue;
            }
        }

        public string PlotterCmd => $"{PlotterExe} {PlotterArguments}";

        public string PlotterExe
        {
            get
            {
                var gigahorsePlottersPath = "gigahorse\\";
                var mmxPlottersPath = "";

                if (!IsMmxOnly)
                {
                    mmxPlottersPath = "mmx\\";
                }

                var exe = "";

                switch (plotter.Value)
                {
                    case (int)Plotters.MmxCudaPlotter:
                        exe = $"{mmxPlottersPath}mmx_cuda_plot_k{size.Value}.exe";
                        break;

                    case (int)Plotters.ChiaCpuPlotter:
                        exe = "chia_plot.exe";
                        if (size.Value > 32)
                        {
                            exe = "chia_plot_k34.exe";
                        }
                        break;

                    case (int)Plotters.ChiaCpuPlotterWithCompression:
                        exe = $"{gigahorsePlottersPath}chia_plot.exe";
                        if (size.Value > 32)
                        {
                            exe = $"{gigahorsePlottersPath}chia_plot.exe";
                        }
                        break;

                    case (int)Plotters.ChiaCudaPlotter_25:
                        exe = $"{gigahorsePlottersPath}cuda_plot_k{size.Value}.exe";
                        break;

                    case (int)Plotters.ChiaCudaPlotter_30:
                        exe = $"{gigahorsePlottersPath}cuda_plot_k32_v3.exe";
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

                    if( item.Scope.HasFlag((Scopes)plotter.Value) && item.Type == ItemType.CmdParameter )
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
