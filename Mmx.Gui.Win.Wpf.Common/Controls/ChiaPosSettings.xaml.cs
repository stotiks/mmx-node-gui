using Mmx.Gui.Win.Common;
using System;
using System.Linq;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChiaPosSettings
    {
        public ChiaPosSettings()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int CHIAPOS_MAX_CUDA_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_CUDA_DEVICES_Maximum => VideoDeviceInfo.Instance.CudaDevices.Count();

        public int CHIAPOS_MAX_CORES_Minimum => 1;
        public int CHIAPOS_MAX_CORES_Maximum => Environment.ProcessorCount;

        public int CHIAPOS_MAX_OPENCL_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_OPENCL_DEVICES_Maximum => VideoDeviceInfo.Instance.OpenCLDevices.Count();

    }
}
