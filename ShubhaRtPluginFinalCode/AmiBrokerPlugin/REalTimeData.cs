using System;
using System.Collections.Generic;
using System.Text;

namespace AmiBrokerPlugin
{
    class REalTimeData
    {
       
            //[MarshalAs(UnmanagedType.Struct, SizeConst = 8)]
            public UInt64 DateTime;
            public int Price;
            public int Open;
            public int High;
            public int Low;
            public int Volume;
            public int OpenInterest;
            public int AuxData1;
            public int AuxData2;
        
    }
}
