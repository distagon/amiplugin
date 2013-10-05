﻿///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    /// <summary>
    /// PluginInfo structure holds general information about plugin
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PluginInfo
    {
        /// <summary>
        /// This is sizeof(struct PluginInfo). int
        /// </summary>
        public int StructSize;

        /// <summary>
        /// Plug-in type currently 1 - indicator is the only one supported. int
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public PluginType Type;

        /// <summary>
        /// Plug-in version coded to int as MAJOR * 10000 + MINOR * 100 + RELEASE. int
        /// </summary>
        public int Version;

        /// <summary>
        /// ID code used to uniquely identify the data feed (set it to zero for AFL plugins). int
        /// </summary>
        public int IDCode;

        /// <summary>
        /// Long name of plug-in displayed in the Plugin dialog. char[64]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        /// <summary>
        /// Name of the plug-in vendor. char[64]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Vendor;

        /// <summary>
        /// Certificate code - set it to zero for private plug-ins. int
        /// </summary>
        public int Certificate;

        /// <summary>
        /// Minimum required AmiBroker version (should be >= 380000 -> AmiBroker 3.8). int
        /// </summary>
        public int MinAmiVersion;
    }
}
