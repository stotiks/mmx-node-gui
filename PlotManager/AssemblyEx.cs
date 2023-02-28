using System;
using System.Reflection;

namespace PlotManager.ExtensionMethods
{
    public static class AssemblyEx
    {
        public static Version GetFileVersion(this Assembly assembly)
        {
            AssemblyFileVersionAttribute attribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return attribute == null ? new Version("0.0.0.0") : new Version(attribute.Version);
        }
    }
}