///////////////////////////////////////////////////////////////////////////////////////////////////
//

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    /// <summary>
    /// The COLORREF value is used to specify or retrieve an RGB color
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public byte R;
        public byte G;
        public byte B;
    }
}
