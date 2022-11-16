using Open.Nat;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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

        const int defaultPort = 11330;

        private int _port = defaultPort;
        public int Port { 
            get => _port; 
            set
            {
                _port = value;
                NotifyPropertyChanged();
            }
        }

        HarversterOptions()
        {
            var dnsEndPoint = LoadNodeDnsEndPoint();
            if (dnsEndPoint != null)
            {
                Host = dnsEndPoint.Host;
                Port = dnsEndPoint.Port;
            }
        }

        public static DnsEndPoint LoadNodeDnsEndPoint()
        {
            DnsEndPoint result = null;

            try
            {
                string line1 = File.ReadLines(Node.nodeFilePath).First();
                Regex reg = new Regex(@"([a-zA-Z0-9\-_\.]+):([0-9]{1,5})");
                var match = reg.Match(line1);
                if (match.Groups.Count >= 3)
                {
                    var host = match.Groups[1].Value;
                    var port = int.Parse(match.Groups[2].Value);
                    result = new DnsEndPoint(host, port);
                }
            }
            catch
            {
                Console.WriteLine(string.Format("{0} not found", Node.nodeFilePath));
            }

            return result;
        }

        public static void SaveNodeDnsEndPoint(DnsEndPoint dnsEndPoint)
        {
            File.WriteAllText(Node.nodeFilePath, string.Concat(dnsEndPoint.Host, ":", dnsEndPoint.Port));
        }

        public async Task Detect()
        {
            var mapping = await GetMapping();

            if (mapping != null)
            {
                Port = defaultPort;
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
