///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct IntradaySettings
    {
        /// <summary>
        /// In hours
        /// </summary>
        public int TimeShift;

        public int FilterAfterHours;

        /// <summary>
        /// Bit encoding HHHHH.MMMMMM.0000	 hours << 10 | ( minutes << 4 ) 
        /// </summary>
        public ulong SessionStart;

        /// <summary>
        /// Bit encoding HHHHH.MMMMMM.0000	 hours << 10 | ( minutes << 4 ) 
        /// </summary>
        public ulong SessionEnd;

        public int FilterWeekends;

        /// <summary>
        /// 0 - exchange, 1 - local, 2 -session based
        /// </summary>
        public int DailyCompressionMode;

        public ulong NightSessionStart;

        public ulong NightSessionEnd;
    }
}
