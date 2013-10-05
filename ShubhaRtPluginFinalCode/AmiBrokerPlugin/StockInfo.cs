///////////////////////////////////////////////////////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 

///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet  = CharSet.Ansi), Serializable]
    public struct StockInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string ShortName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string AliasName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string WebID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string FullName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Address;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Country;

        /// <summary>
        /// ISO 3 letter currency code
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Currency;

        /// <summary>
        /// The ID of the data plug-in, 0 - accept workspace settings
        /// </summary>
        public int DataSource;

        /// <summary>
        /// Local mode of operation - 0 - accept workspace settings, 1 - store locally, 2 - don't store locally
        /// </summary>
        public int DataLocalMode;

        public int MarketID;

        public int GroupID;

        public int IndustryID;

        public int GICS;

        /// <summary>
        /// continuous etc.
        /// </summary>
        public int Flags;

        public int MoreFlags;

        /// <summary>
        /// new futures fields - active if SI_MOREFLAGS_FUTURES is set
        /// </summary>
        public float MarginDeposit;

        public float PointValue;

        public float RoundLotSize;

        /// <summary>
        /// New futures fields - active if SI_MOREFLAGS_FUTURES is set
        /// </summary>
        public float TickSize;

        /// <summary>
        /// Number of decimal places to display
        /// </summary>
        public int Decimals;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public short[] LastSplitFactor;

        public ulong LastSplitDate;

        public ulong DividendPayDate;

        public ulong ExDividendDate;

        public float SharesFloat;

        public float SharesOut;

        public float DividendPerShare;

        public float BookValuePerShare;

        /// <summary>
        /// PE Growth ratio
        /// </summary>
        public float PEGRatio;

        public float ProfitMargin;

        public float OperatingMargin;

        public float OneYearTargetPrice;

        public float ReturnOnAssets;

        public float ReturnOnEquity;

        /// <summary>
        ///  Year over year
        /// </summary>
        public float QtrlyRevenueGrowth;

        public float GrossProfitPerShare;

        /// <summary>
        /// TTN Sales Revenue
        /// </summary>
        public float SalesPerShare;

        public float EBITDAPerShare;

        public float QtrlyEarningsGrowth;

        public float InsiderHoldPercent;

        public float InstitutionHoldPercent;

        public float SharesShort;

        public float SharesShortPrevMonth;

        /// <summary>
        /// From Forward P/E
        /// </summary>
        public float ForwardEPS;

        /// <summary>
        /// TTM EPS
        /// </summary>
        public float EPS;

        public float EPSEstCurrentYear;

        public float EPSEstNextYear;

        public float EPSEstNextQuarter;

        public float ForwardDividendPerShare;

        public float Beta;

        public float OperatingCashFlow;

        public float LeveredFreeCashFlow;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public float[] ReservedInternal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public float[] UserData;
    }
}
