using Microsoft.SqlServer.Server;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mmx.Gui.Win.Common
{
    public class VideoDeviceInfo
    {

        public static class OpenCL
        {
            public const int CL_DEVICE_TYPE_ALL = -1;

            [DllImport("OpenCL.dll", EntryPoint = "clGetPlatformIDs")]
            public static extern int clGetPlatformIDs(uint numPlatforms, IntPtr[] platformIDs, out uint numPlatformsRet);

            [DllImport("OpenCL.dll", EntryPoint = "clGetDeviceIDs")]
            public static extern int clGetDeviceIDs(IntPtr platformID, int deviceType, uint numDevices, IntPtr[] deviceIDs, out uint numDevicesRet);
        }

        public static class Cuda
        {

            [DllImport("nvcuda.dll")]
            public static extern int cuInit(int flags);

            [DllImport("nvcuda.dll")]
            public static extern int cuDeviceGetCount(ref int count);

            public static string GetDeviceName(int deviceIndex)
            {
                byte[] nameBytes = new byte[256];
                cuDeviceGetName(nameBytes, nameBytes.Length, deviceIndex);
                return System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
            }

            [DllImport("nvcuda.dll")]
            public static extern int cuDeviceGetName(byte[] name, int len, int device);
        }

        public List<string> CudaDevices { get; private set; } = new List<string>();
        public List<string> OpenCLDevices { get; private set; } = new List<string>();

        private VideoDeviceInfo()
        {
            GetCudaDevices();
            GetOpenCLDevices();
            InitSettings();
        }

        private void GetOpenCLDevices()
        {
            try
            {
                uint numberOfAvailablePlatforms;
                int result = OpenCL.clGetPlatformIDs(0, null, out numberOfAvailablePlatforms);
                if (result != 0)
                {
                    throw new Exception(string.Format("The number of platforms could not be queried. [{0}]", result));
                }

                IntPtr[] platformPointers = new IntPtr[numberOfAvailablePlatforms];
                result = OpenCL.clGetPlatformIDs(numberOfAvailablePlatforms, platformPointers, out _);

                if (result != 0)
                {
                    throw new Exception(string.Format("The platforms could not be retrieved. [{0}]", result));
                }

                foreach (IntPtr platformPointer in platformPointers)
                {
                    OpenCLDevices.Add(platformPointer.ToString());
                }
            }
            catch
            {
                Debug.WriteLine("OpenCL device detection failed");
            }

            //if (OpenCLDevices.Count == 0)
            //{
            //    OpenCLDevices.Add("Default device");
            //}
        }

        private void GetCudaDevices()
        {
            try
            {
                int deviceCount = 0;
                Cuda.cuInit(0);
                Cuda.cuDeviceGetCount(ref deviceCount);

                for (int i = 0; i < deviceCount; i++)
                {
                    CudaDevices.Add(Cuda.GetDeviceName(i));
                }
            }
            catch
            {
                Debug.WriteLine("Cuda device detection failed");
            }

            if (CudaDevices.Count == 0)
            {
                CudaDevices.Add("Default device");
            }
        }

        private void InitSettings()
        {
            if (Settings.Default.CHIAPOS_MAX_CUDA_DEVICES == -1)
            {
                Settings.Default.CHIAPOS_MAX_CUDA_DEVICES = CudaDevices.Count();
            }

            if (Settings.Default.CHIAPOS_MAX_CORES == -1)
            {
                Settings.Default.CHIAPOS_MAX_CORES = Environment.ProcessorCount;
            }

            if (Settings.Default.CHIAPOS_MAX_OPENCL_DEVICES == -1)
            {
                Settings.Default.CHIAPOS_MAX_OPENCL_DEVICES = OpenCLDevices.Count();
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly VideoDeviceInfo instance = new VideoDeviceInfo();
        }

        public static VideoDeviceInfo Instance => Nested.instance;

    }
}
