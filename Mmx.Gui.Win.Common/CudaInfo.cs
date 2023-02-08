using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace Mmx.Gui.Win.Common
{
    public class CudaInfo
    {
        public List<ManagementObject> Devices { get; private set; } = new List<ManagementObject>();
        private CudaInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    Devices.Add(obj);

                    //Debug.WriteLine("Name  -  " + obj["Name"]);
                    //Debug.WriteLine("DeviceID  -  " + obj["DeviceID"]);
                    //Debug.WriteLine("AdapterRAM  -  " + obj["AdapterRAM"]);
                    //Debug.WriteLine("AdapterDACType  -  " + obj["AdapterDACType"]);
                    //Debug.WriteLine("Monochrome  -  " + obj["Monochrome"]);
                    //Debug.WriteLine("InstalledDisplayDrivers  -  " + obj["InstalledDisplayDrivers"]);
                    //Debug.WriteLine("DriverVersion  -  " + obj["DriverVersion"]);
                    //Debug.WriteLine("VideoProcessor  -  " + obj["VideoProcessor"]);
                    //Debug.WriteLine("VideoArchitecture  -  " + obj["VideoArchitecture"]);
                    //Debug.WriteLine("VideoMemoryType  -  " + obj["VideoMemoryType"]);
                }
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly CudaInfo instance = new CudaInfo();
        }

        public static CudaInfo Instance => Nested.instance;

    }
}
