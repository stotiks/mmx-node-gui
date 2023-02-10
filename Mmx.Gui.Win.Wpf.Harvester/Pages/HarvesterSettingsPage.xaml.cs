using Mmx.Gui.Win.Common;
using System;
using System.Linq;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester.Pages
{
    /// <summary>
    /// Interaction logic for HarvesterSettingsPage.xaml
    /// </summary>
    public partial class HarvesterSettingsPage : Page
    {
        public HarvesterSettingsPage()
        {
            InitializeComponent();
        }

        public int CHIAPOS_MAX_CUDA_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_CUDA_DEVICES_Maximum => CudaInfo.Instance.Devices.Count();

        public int CHIAPOS_MAX_CORES_Minimum => 1;
        public int CHIAPOS_MAX_CORES_Maximum => Environment.ProcessorCount;
    }
}
