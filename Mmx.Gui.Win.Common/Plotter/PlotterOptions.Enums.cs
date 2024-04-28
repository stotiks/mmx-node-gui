using System;
using System.ComponentModel;

namespace Mmx.Gui.Win.Common.Plotter
{
    public partial class PlotterOptions
    {
        public enum Plotters : int
        {
            [Description("Chia CPU plotter"), Url("https://github.com/madMAx43v3r/chia-plotter")]
            ChiaCpuPlotter = 1 << 0,

            [Description("Gigahorse v2.4 CPU plotter with compression"), Url("https://github.com/madMAx43v3r/chia-gigahorse/tree/master/cpu-plotter")]
            ChiaCpuPlotterWithCompression = 1 << 1,

            [Description("Gigahorse v2.5 CUDA plotter with compression"), Url("https://github.com/madMAx43v3r/chia-gigahorse/tree/master/cuda-plotter")]
            ChiaCudaPlotter_25 = 1 << 2,

            [Description("Gigahorse v3.0 K32 CUDA plotter with compression"), Url("https://github.com/madMAx43v3r/chia-gigahorse/tree/master/cuda-plotter")]
            ChiaCudaPlotter_30 = 1 << 3,

            [Description("MMX CUDA plotter")]
            MmxCudaPlotter = 1 << 4
        };

        [Flags]
        public enum Scopes : int
        {
            None = 0,

            ChiaCpuPlotter = Plotters.ChiaCpuPlotter,
            ChiaCpuPlotterWithCompression = Plotters.ChiaCpuPlotterWithCompression,
            ChiaCudaPlotter_25 = Plotters.ChiaCudaPlotter_25,
            ChiaCudaPlotter_30 = Plotters.ChiaCudaPlotter_30,
            MmxCudaPlotter = Plotters.MmxCudaPlotter,

            CpuPlotters = ChiaCpuPlotter | ChiaCpuPlotterWithCompression,
            CudaPlotters = ChiaCudaPlotter_25 | ChiaCudaPlotter_30 | MmxCudaPlotter,
            PlottersWithCompression = ChiaCpuPlotterWithCompression | CudaPlotters,

            MmxPlotters = MmxCudaPlotter,

            Common = CpuPlotters | CudaPlotters,
        };

    }

}
