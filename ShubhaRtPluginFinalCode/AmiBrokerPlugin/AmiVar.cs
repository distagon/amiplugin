///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------
//
///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential, Size = 8)]
    unsafe public struct AmiVar
    {
        public int Type;

        [StructLayoutAttribute(LayoutKind.Explicit, Size = 4)]
        unsafe public struct Val
        {
            [FieldOffset(0)]
            public float Value;

            [FieldOffset(0)]
            public float* Array;

            [FieldOffset(0)]
            public char* String;

            [FieldOffset(0)]
            public void* Disp;
        }
    }
}
