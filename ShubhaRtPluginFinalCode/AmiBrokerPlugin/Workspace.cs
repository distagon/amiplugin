///////////////////////////////////////////////////////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct Workspace
    {
        /// <summary>
        /// 0 - use preferences, 1 - local, ID of plugin
        /// </summary>
        public int DataSource;

        /// <summary>
        /// 0 - use preferences, 1 - store locally, 2 - don't
        /// </summary>
        public int DataLocalMode;

        public int NumBars;

        public int TimeBase;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int[] reservedB;

        public int AllowMixedEODIntra;

        public int RequestDataOnSave;

        public int PadNonTradingDays;

        public int ReservedC;

        public IntradaySettings IntradaySettings;

        public int ReservedD;
    }
}
