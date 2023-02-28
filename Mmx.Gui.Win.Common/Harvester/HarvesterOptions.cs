using Mmx.Gui.Win.Common.Node;
using Open.Nat;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common.Harvester
{
    public class HarvesterOptions : INotifyPropertyChanged
    {
        private string _host = "localhost";
        public string Host {
            get => _host;
            set { 
                _host = value;
                NotifyPropertyChanged();
            }
        }

        private const int DefaultPort = 11330;

        private int _port = DefaultPort;
        public int Port { 
            get => _port; 
            set
            {
                _port = value;
                NotifyPropertyChanged();
            }
        }

        private HarvesterOptions()
        {
            var dnsEndPoint = LoadNodeDnsEndPoint();
            if (dnsEndPoint != null)
            {
                Host = dnsEndPoint.Host;
                Port = dnsEndPoint.Port;
            }
        }

        private static DnsEndPoint LoadNodeDnsEndPoint()
        {
            DnsEndPoint result = null;

            var reg = new Regex(@"([a-zA-Z0-9\-_\.]+):([0-9]{1,5})");
            try
            {
                string line1 = File.ReadLines(NodeHelpers.nodeFilePath).First();
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
                Console.WriteLine($"{NodeHelpers.nodeFilePath} not found");
            }

            return result;
        }

        public static void SaveNodeDnsEndPoint(DnsEndPoint dnsEndPoint)
        {
            File.WriteAllText(NodeHelpers.nodeFilePath, $"{dnsEndPoint.Host}:{dnsEndPoint.Port}");
        }

        public async Task DetectNodeIP(int port = DefaultPort)
        {
            var mapping = await GetMapping();

            if (mapping != null)
            {
                var isOpen = IsPortOpen(mapping.PrivateIP, port, 20);
                if (isOpen)
                {
                    Port = port;
                    Host = mapping.PrivateIP.ToString();
                    return;
                }
            }

            var localIpAddress = GetLocalIPAddress();
            if (localIpAddress != null)
            {
                var addressBytes = localIpAddress.GetAddressBytes();

                var addressesWithOpenPort = new ConcurrentBag<IPAddress>();
                Parallel.For(1, 256, i =>
                {
                    addressBytes[3] = Convert.ToByte(i);
                    IPAddress ipAddress = new IPAddress(addressBytes);
                    var isOpen = IsPortOpen(ipAddress, port, 20);
                    if(isOpen)
                    {
                        addressesWithOpenPort.Add(ipAddress);
                    }
                });
                    
                if(addressesWithOpenPort.Count() > 0)
                {
                    Port = port;
                    Host = addressesWithOpenPort.First().ToString();
                    return;
                }
            }

        }

        private bool IsPortOpen(IPAddress host, int port, int timeout)
        {

            using (var tcp = new TcpClient())
            {
                var ar = tcp.BeginConnect(host, port, null, null);
                using (ar.AsyncWaitHandle)
                {
                    if (ar.AsyncWaitHandle.WaitOne(timeout, false))
                    {
                        try
                        {
                            tcp.EndConnect(ar);
                            //Connect was successful.
                            return true;
                        }
                        catch
                        {
                            //EndConnect threw an exception.
                            //Most likely means the server refused the connection.
                        }
                    }
                    else
                    {
                        //Connection timed out.
                    }
                }
            }

            return false;
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

        private static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return null;
            //throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static HarvesterOptions Instance => Nested.instance;

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly HarvesterOptions instance = new HarvesterOptions();
        }
    }
}
