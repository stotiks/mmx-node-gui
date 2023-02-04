using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace Mmx.Gui.Win.Common.Plotter
{
    internal class CudaInfo
    {
        public List<ManagementObject> Devices { get; private set; } = new List<ManagementObject>();
        private CudaInfo()
        {
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    Devices.Add(obj);

                    Debug.Write("Name  -  " + obj["Name"] + "</br>");
                    Debug.Write("DeviceID  -  " + obj["DeviceID"] + "</br>");
                    Debug.Write("AdapterRAM  -  " + obj["AdapterRAM"] + "</br>");
                    Debug.Write("AdapterDACType  -  " + obj["AdapterDACType"] + "</br>");
                    Debug.Write("Monochrome  -  " + obj["Monochrome"] + "</br>");
                    Debug.Write("InstalledDisplayDrivers  -  " + obj["InstalledDisplayDrivers"] + "</br>");
                    Debug.Write("DriverVersion  -  " + obj["DriverVersion"] + "</br>");
                    Debug.Write("VideoProcessor  -  " + obj["VideoProcessor"] + "</br>");
                    Debug.Write("VideoArchitecture  -  " + obj["VideoArchitecture"] + "</br>");
                    Debug.Write("VideoMemoryType  -  " + obj["VideoMemoryType"] + "</br>");
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
