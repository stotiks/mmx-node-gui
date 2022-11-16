using Open.Nat;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class HarversterOptions : INotifyPropertyChanged
    {
        private string _host = "localhost";
        public string Host {
            get => _host;
            set { 
                _host = value;
                NotifyPropertyChanged();
            }
        }

        private int _port = 11337;
        public int Port { 
            get => _port; 
            set
            {
                _port = value;
                NotifyPropertyChanged();
            }
        }

        public async Task Detect()
        {
            var mapping = await GetMapping();

            if (mapping != null)
            {
                Port = mapping.PrivatePort;
                Host = mapping.PrivateIP.ToString();
            }
        }

        private async Task<Mapping> GetMapping()
        {
            Mapping result = null;

            var discoverer = new NatDiscoverer();
            var cts = new CancellationTokenSource(5000);
            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
            var mappings = await device.GetAllMappingsAsync();
            foreach (var mapping in mappings)
            {
                if (mapping.Description == "MMX Node")
                {
                    result = mapping;
                    break;
                }
            }

            return result;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static HarversterOptions Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly HarversterOptions instance = new HarversterOptions();
        }
    }
}
