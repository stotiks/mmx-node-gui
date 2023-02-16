using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mmx.Gui.Win.Common
{
    public class CudaInfo
    {
        [DllImport("nvcuda.dll")]
        static extern int cuInit(int flags);

        [DllImport("nvcuda.dll")]
        static extern int cuDeviceGetCount(ref int count);

        static string GetDeviceName(int deviceIndex)
        {
            byte[] nameBytes = new byte[256];
            cuDeviceGetName(nameBytes, nameBytes.Length, deviceIndex);
            return System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
        }

        [DllImport("nvcuda.dll")]
        static extern int cuDeviceGetName(byte[] name, int len, int device);

        public List<string> Devices { get; private set; } = new List<string>();
        private CudaInfo()
        {
            try
            {
                int deviceCount = 0;
                cuInit(0);
                cuDeviceGetCount(ref deviceCount);

                for (int i = 0; i < deviceCount; i++)
                {
                    Devices.Add(GetDeviceName(i));
                }
            }
            catch {
                Debug.WriteLine("Cuda device detection failed");
            }

            if(Devices.Count == 0)
            {
                Devices.Add("Default device");
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
