///////////////////////////////////////////////////////////////////////////////////////////////////
//
// ------------------------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AmiBrokerPlugin
{
    public static partial class Plugin
    {
        /// <summary>
        /// A pointer to AmiBroker main window
        /// </summary>
        static IntPtr MainWnd = IntPtr.Zero;

        /// <summary>
        /// Plugin status code
        /// </summary>
        static StatusCode Status = StatusCode.Unknown ;

        /// <summary>
        /// Default encoding
        /// </summary>
        static Encoding Encoding = Encoding.GetEncoding("windows-1251"); // TODO: Update it based on your preferences

        #region AmiBroker Method Delegates

        // Data related

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int GetStockQtyDelegate();
        static GetStockQtyDelegate GetStockQty;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int SetCategoryNameDelegate(int category, int item, string name);
        static SetCategoryNameDelegate SetCategoryName;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate string GetCategoryNameDelegate(int category, int item);
        static GetCategoryNameDelegate GetCategoryName;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int SetIndustrySectorDelegate(int industry, int sector);
        static SetIndustrySectorDelegate SetIndustrySector;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int GetIndustrySectorDelegate(int industry);
        static GetIndustrySectorDelegate GetIndustrySector;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        public delegate IntPtr AddStockDelegate(string ticker); // returns a pointer to StockInfo
        static AddStockDelegate AddStock;


        #endregion

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Notify AmiBroker that new streaming data arrived
        /// </summary>
       static void NotifyStreamingUpdate()
        {
            //System.IO.File.WriteAllText("C:\\data\\NotifyStreamingUpdate.txt", "Funcion call");

            SendMessage(MainWnd, 0x0400 + 13000,IntPtr.Zero, IntPtr.Zero);

        }

        /// <summary>
        /// Converts string code into Int32 required by AmiBroker. Used to define a unique ID for the plug-in.
        /// </summary>
        static int GetIDCode(string id)
        {
            if (id.Length != 4)
            {
                throw new ArgumentException("Plugin ID must be 4 characters long.", "id");
            }

            return id[0] << 24 | id[1] << 16 | id[2] << 8 | id[3] << 0;
        }

        /// <summary>
        /// Pack AmiBroker DateTime object into UInt64
        /// </summary>
        static ulong PackDate(DateTime date)
        {
            return PackDate(date,false  );

        }

        /// <summary>
        /// Pack AmiBroker DateTime object into UInt64
        /// </summary>
        static ulong PackDate(DateTime date, bool isFeaturePad)
        {
            var isEOD = date.Hour == 0 && date.Minute == 0 && date.Second == 0;

            // lower 32 bits
            var ft = BitVector32.CreateSection(1);
            var rs = BitVector32.CreateSection(23, ft);
            var ms = BitVector32.CreateSection(999, rs);
            var ml = BitVector32.CreateSection(999, ms);
            var sc = BitVector32.CreateSection(59, ml);

            var bv1 = new BitVector32(0);
            bv1[ft] = isFeaturePad ? 1 : 0;         // bit marking "future data"
            bv1[rs] = 0;                            // reserved set to zero
            bv1[ms] = 0;                            // microseconds	0..999
            bv1[ml] = date.Millisecond;             // milliseconds	0..999
            bv1[sc] = date.Second;                  // 0..59

            // higher 32 bits
            var mi = BitVector32.CreateSection(59);
            var hr = BitVector32.CreateSection(23, mi);
            var dy = BitVector32.CreateSection(31, hr);
            var mn = BitVector32.CreateSection(12, dy);
            var yr = BitVector32.CreateSection(4095, mn);

            var bv2 = new BitVector32(0);
            bv2[mi] = isEOD ? 63 : date.Minute;     // 0..59        63 is reserved as EOD marker
            bv2[hr] = isEOD ? 31 : date.Hour;       // 0..23        31 is reserved as EOD marker
            bv2[dy] = date.Day;                     // 1..31
            bv2[mn] = date.Month;                   // 1..12
            bv2[yr] = date.Year;                    // 0..4095

            //return ((ulong)bv2.Data << 32) ^ (ulong)bv1.Data;
            return ((ulong)bv2.Data << 32) ^ ((ulong)bv1.Data << 32 >> 32);
        }

        /// <summary>
        /// Unpack UInt64 object into AmiBroker DateTime
        /// </summary>
        static DateTime UnpackDate(ulong date)
        {
            // lower 32 bits
            var ft = BitVector32.CreateSection(1);
            var rs = BitVector32.CreateSection(23, ft);
            var ms = BitVector32.CreateSection(999, rs);
            var ml = BitVector32.CreateSection(999, ms);
            var sc = BitVector32.CreateSection(59, ml);
            var bv1 = new BitVector32((int)(date << 32 >> 32));

            // higher 32 bits
            var mi = BitVector32.CreateSection(59);
            var hr = BitVector32.CreateSection(23, mi);
            var dy = BitVector32.CreateSection(31, hr);
            var mn = BitVector32.CreateSection(12, dy);
            var yr = BitVector32.CreateSection(4095, mn);
            var bv2 = new BitVector32((int)(date >> 32));

            var hour = bv2[hr];
            var minute = bv2[mi];
            var second = bv1[sc];
            var milsec = bv1[ml];

            if (hour > 24 || minute > 59 || second > 59 || milsec > 999)
            {
                return new DateTime(bv2[yr], bv2[mn], bv2[dy]);
            }

            return new DateTime(bv2[yr], bv2[mn], bv2[dy], hour, minute, second, milsec);
        }

        /// <summary>
        /// Update status of the plugin
        /// </summary>
        /// <param name="statusPtr">A pointer to <see cref="AmiBrokerPlugin.PluginStatus"/></param>
        static void SetStatus(IntPtr statusPtr, StatusCode code, Color color, string shortMessage, string fullMessage)
        {
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 4), (int)code);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 8), color.R);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 9), color.G);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 10), color.B);

            var msg = Encoding.GetBytes(fullMessage);
            for (int i = 0; i < (msg.Length > 255 ? 255 : msg.Length); i++)
            {
                Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 12 + i), msg[i]);
            }
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 12 + msg.Length), 0x0);

            msg = Encoding.GetBytes(shortMessage);
            for (int i = 0; i < (msg.Length > 31 ? 31 : msg.Length); i++)
            {
                Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 268 + i), msg[i]);
            }
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 268 + msg.Length), 0x0);
        }

        /// <summary>
        /// Write message into the log
        /// </summary>
        public static void Log(string message, params object[] args)
        {
            using (var writer = File.AppendText(@"C:\Users\Public\Documents\AmiBroker Plug-In Log.txt"))
            {
                var tmp = new List<string>(args.Length);

                foreach (var a in args)
                {
                    var type = a.GetType();
                    if (type.Namespace == "AmiBrokerPlugin" && !type.IsEnum && type.BaseType.Name != "Enum")
                    {
                        var sb = new StringBuilder("{ ");
                        var count = 0;

                        foreach (var field in type.GetFields())
                        {
                            var val = field.GetValue(a);

                            if (val == null)
                            {
                                sb.Append((count > 0 ? ", " : "") + field.Name + ": NULL");
                                count++;
                                continue;
                            }

                            var valType = val.GetType();

                            if (valType.Namespace == "AmiBrokerPlugin" && !type.IsEnum && valType.BaseType.Name != "Enum")
                            {
                                var sb2 = new StringBuilder("{ ");
                                var count2 = 0;

                                foreach (var field2 in valType.GetFields())
                                {
                                    var val2 = field2.GetValue(val);
                                    sb2.Append((count2 > 0 ? ", " : "") + field2.Name + ": " + (val2 == null ? "NULL" : val2.ToString()));
                                    count2++;
                                }

                                sb.Append(" }");

                                sb.Append((count > 0 ? ", " : "") + field.Name + ": " + sb2.ToString());
                                count++;
                            }
                            else
                            {
                                sb.Append((count > 0 ? ", " : "") + field.Name + ": " + field.GetValue(a).ToString());
                                count++;
                            }
                        }

                        sb.Append(" } ");

                        tmp.Add(sb.ToString());
                    }
                    else
                    {
                        tmp.Add(a.ToString());
                    }
                }

                writer.WriteLine(message, tmp.ToArray());
            }
        }
    }
}
