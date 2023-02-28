using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Wpf.Common.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChiaPosSettings
    {
        private ObservableCollection<CudaVisibleDevice> CudaVisibleDevicesItemsSource = new ObservableCollection<CudaVisibleDevice>();

        public ChiaPosSettings()
        {
            InitializeComponent();
            DataContext = this;

            for (var i = 0; i < VideoDeviceInfo.Instance.CudaDevices.Count; i++)
            {
                var item = new CudaVisibleDevice(i, VideoDeviceInfo.Instance.CudaDevices[i], Settings.Default.CUDA_VISIBLE_DEVICES.Contains(i));
                item.PropertyChanged += (o, e) => UpdateCUDA_VISIBLE_DEVICES();
                CudaVisibleDevicesItemsSource.Add(item);
            }

            UpdateCUDA_VISIBLE_DEVICES();

            CUDA_VISIBLE_DEVICES_ItemsControl.ItemsSource = CudaVisibleDevicesItemsSource;
        }

        private void UpdateCUDA_VISIBLE_DEVICES()
        {
            Settings.Default.CUDA_VISIBLE_DEVICES = CudaVisibleDevicesItemsSource.Where(item => item.Enabled == true).Select(item => item.Index).ToArray();
        }

        public int CHIAPOS_MAX_CUDA_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_CUDA_DEVICES_Maximum => VideoDeviceInfo.Instance.CudaDevices.Count();

        public int CHIAPOS_MAX_CORES_Minimum => 1;
        public int CHIAPOS_MAX_CORES_Maximum => Environment.ProcessorCount;

        public int CHIAPOS_MAX_OPENCL_DEVICES_Minimum => 0;
        public int CHIAPOS_MAX_OPENCL_DEVICES_Maximum => VideoDeviceInfo.Instance.OpenCLDevices.Count();

    }

    internal class CudaVisibleDevice : INotifyPropertyChanged
    {
        public CudaVisibleDevice(int index, string name, bool enabled)
        {
            Index = index;
            Name = name;
            Enabled = enabled;
        }

        public int Index { get; }

        public string Name { get; }

        bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
