///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    public enum InfoSiteCategory
    {
        Market,
        Group,
        Sector,
        Industry,
        Watchlist
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class InfoSite
    {
        public int StructSize;

        public delegate int GetStockQtyDelegate();

        public GetStockQtyDelegate GetStockQty;

        public delegate StockInfoFormat4 AddStockDelegate(string ticker);

        public AddStockDelegate AddStock;

        public delegate int SetCategoryNameDelegate(int category, int item, string name);

        public SetCategoryNameDelegate SetCategoryName;

        public delegate string GetCategoryNameDelegate(int category, int item);

        public GetCategoryNameDelegate GetCategoryName;

        public delegate int SetIndustrySectorDelegate(int industry, int sector);

        public SetIndustrySectorDelegate SetIndustrySector;

        public delegate int GetIndustrySectorDelegate(int industry);

        public GetIndustrySectorDelegate GetIndustrySector;

        /// <summary>
        /// Only available if called from AmiBroker 5.27 or higher
        /// </summary>
        public delegate StockInfo AddStockNewDelegate(string ticker);
        
        public AddStockNewDelegate AddStockNew;
    }
}
