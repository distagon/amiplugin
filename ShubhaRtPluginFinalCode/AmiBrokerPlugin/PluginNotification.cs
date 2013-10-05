///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    public enum PluginNotificationReason
    {
        DatabaseLoaded = 1,
        DatabaseUnloaded = 2,
        SettingsChange = 4,
        StatusRightClick = 8
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 24)]
    public struct PluginNotification
    {
        /// int
        [FieldOffset(0)]
        public int StructSize;

        /// int
        [FieldOffset(4)]
        public PluginNotificationReason Reason;

        /// LPCTSTR
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPStr)]
        public string DatabasePath;

        /// HWND
        [FieldOffset(12)]
        public IntPtr MainWnd;

        /// StockInfo
        [FieldOffset(16)]
        public IntPtr StockInfo;

        /// Workspace
        [FieldOffset(20)]
        public IntPtr Workspace;
    }
}
