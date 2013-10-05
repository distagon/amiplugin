///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    public enum StatusCode
    {
        
        OK,
        Wait,
        Error,
        Unknown
    }


    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class PluginStatus
    {
        public int StructSize;

        public int StatusCode;

        public COLORREF StatusColor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string LongMessage;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ShortMessage;
    }
}
