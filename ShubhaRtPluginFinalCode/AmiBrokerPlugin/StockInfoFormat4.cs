///////////////////////////////////////////////////////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class StockInfoFormat4
    {
        public float MarginDeposit; // new futures fields - active if SI_MOREFLAGS_FUTURES is set 
        public float PointValue;
        public float RoundLotSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string LastSplitFactor = new string(new char[2]); // [ 0 ] - new, [ 1 ] - old
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string FullName = new string(new char[64]);
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Address = new string(new char[128]);
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Country = new string(new char[40]);
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Currency = new string(new char[4]);
        public sbyte ReservedA;
        public sbyte LastSplitYear; // 0..255 = Year - 1900 => 1900..2155
        public sbyte LastSplitMonth; // 1..12
        public sbyte LastSplitDay; // 1..31
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string AliasName = new string(new char[16]);
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 26)]
        public string ShortName = new string(new char[26]);
        public int Flags; // MarketID etc.
        public float SharesFloat; // int Code;
        public float SharesOut; //int StockQty;
        public uint DividendPayDate;
        public float BookValuePerShare;
        public uint ExDividendDate; // div date 
        public float DividendPerShare;
        public float PEGRatio; // PE Growth ratio
        public float ProfitMargin;
        public float OperatingMargin;
        public float OneYearTargetPrice;
        public int IndustryID;
        public int MoreFlags; // 0x80000000 - wartoфci w tysiвcach v2.52 
        public float ReturnOnAssets;
        public float ReturnOnEquity;
        public float QtrlyRevenueGrowth; // year over year 
        public float GrossProfitPerShare;
        public float SalesPerShare; // ttn Sales Revenue
        public float EBITDAPerShare;
        public float QtrlyEarningsGrowth;
        public float InsiderHoldPercent;
        public float InstitutionHoldPercent;
        public float SharesShort;
        public float SharesShortPrevMonth;
        public float ForwardEPS; // from Forward P/E
        public float EPS; // ttm EPS
        public float EPSEstCurrentYear;
        public float EPSEstNextYear;
        public float EPSEstNextQuarter;
        public float TickSize; // new futures fields - active if SI_MOREFLAGS_FUTURES is set 
#if _M_X64
		public string WebID = new string(new char[8]); // ON 64 BIT PLATFORM THIS HAS TO BE 8 due to alignment requirements
#else
        public string WebID = new string(new char[6]); // ON 32 BIT PLATFORM THIS HAS TO BE 6 !!!!
#endif
        public float ForwardDividendPerShare; // zysk brutto kwart 
        public int DataSource; // the ID of the data plug-in, 0 - accept workspace settings 
        public int DataLocalMode; // local mode of operation - 0 - accept workspace settings, 1 - store locally, 2 - don't store locally 
        public float Beta;
        public float OperatingCashFlow;
        public float LeveredFreeCashFlow;
        public int WatchListBits; // 32 watch lists 
        public int WatchListBits2; // another 32 watch lists 
    }
}
