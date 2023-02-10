using System;
using System.Diagnostics;
using System.IO;

namespace Mmx.Gui.Win.Common.Node
{

    public enum NodeBuildType
    {
        Classic,
        Gigahorse
    }

    public static class NodeHelpers
    {
        public static readonly string workingDirectory =
#if !DEBUG
    		Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
#else
            //@"C:\Program Files\MMX";
            @"C:\dev\mmx\MMX";
#endif
        public static readonly string MMX_HOME_ENV = Environment.GetEnvironmentVariable("MMX_HOME");
        public static readonly string MMX_HOME = string.IsNullOrEmpty(MMX_HOME_ENV) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mmx") : MMX_HOME_ENV;
        public static readonly string configPath = Path.Combine(MMX_HOME, @"config\local");
        public static readonly string harvesterConfigPath = Path.Combine(configPath, "Harvester.json");
        public static readonly string walletConfigPath = Path.Combine(configPath, "Wallet.json");

        public static readonly string plotterConfigPath = Path.Combine(configPath, "Plotter.json");
        public static readonly string httpServerConfigPath = Path.Combine(configPath, "HttpServer.json");
        public static readonly string nodeFilePath = Path.Combine(configPath, "node");

        public static readonly string activateCMDPath = Path.Combine(workingDirectory, "activate.cmd");
        public static readonly string runNodeCMDPath = Path.Combine(workingDirectory, "run_node.cmd");
        public static readonly string runHarvesterCMDPath = Path.Combine(workingDirectory, "run_harvester.cmd");
        public static readonly string mmxNodeEXEPath = Path.Combine(workingDirectory, "mmx_node.exe");

        public static Version Version { get; private set; }
        public static NodeBuildType BuildType { get; private set; }

        public static string VersionTag => $"v{Version} - ({BuildType})";

        static NodeHelpers()
        {
            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(NodeHelpers.mmxNodeEXEPath);
                var productVersion = versionInfo.ProductVersion;
                Version = new Version(productVersion);

                var productName = versionInfo.ProductName;
                if (productName.Contains(nameof(NodeBuildType.Classic)))
                {
                    BuildType = NodeBuildType.Classic;
                } else
                {
                    BuildType = NodeBuildType.Gigahorse;
                }
            }
            catch
            {
                Version = new Version();
            }
        }
    }
}