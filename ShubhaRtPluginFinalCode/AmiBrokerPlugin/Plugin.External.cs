///////////////////////////////////////////////////////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Timers;
using System.Configuration;
using System.Globalization;
using Microsoft.Win32;
namespace AmiBrokerPlugin
{
    /// <summary>
    /// All public static methods of this class will be exported for use by AmiBroker
    /// </summary>
    /// 
    
    public static partial class Plugin
    {

        #region Common Exported Functions

        /////////////////////////////////////////////////////////////////////////
        // COMMON EXPORTED FUNCTONS
        //
        // Each AmiBroker plug-in DLL must export the following
        // functions:
        // 1. GetPluginInfo	- called when DLL is loaded
        // 2. Init - called when AFL engine is being initialized 
        // 3. Release - called when AFL engine is being closed
        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Called when DLL is loaded
        /// </summary>
        public static int GetPluginInfo(ref PluginInfo pluginInfo)
        {
            // TODO: Insert information about the plugin
            pluginInfo.Name = "ShubhaRT ® data Plug-in";            // Name of the plug-in
            pluginInfo.Vendor = "Shubhulabha";                            // Vendor of the plug-in
            pluginInfo.Type = PluginType.Data;                      // Plug-in type. AFL, Data or Optimizer
            pluginInfo.Version = 10005;                             // Plug-in version. v1.0.0
            pluginInfo.IDCode = GetIDCode("SLRT");                  // Unique ID of the plug-in
            pluginInfo.Certificate = 0;                             // Is this plug-in certified by AmiBroker
            pluginInfo.MinAmiVersion = 387000;                      // Minimum supported version of AmiBroker
            pluginInfo.StructSize = Marshal.SizeOf(pluginInfo);     // Just keep it


            return 1;
        }

        /// <summary>
        /// Called when AFL engine is being initialized
        /// </summary>
        public static  int Init()
        {
            Advertisment a = new Advertisment();
            a.ShowDialog();
            Configuration config;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("timerflag");

            config.AppSettings.Settings.Add("timerflag", "2");
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            
            ////////////////////////////////////////////  
            // TODO: Add initialization logic here
            return 1;
        }

        /// <summary>
        /// Called when AFL engine is being closed
        /// </summary>
        public static int Release()
        {
            // TODO: Add cleanup logic here
            return 1;
        }

        #endregion
       
        #region AFL Exported Functions

        /////////////////////////////////////////////////////////////////////////
        // INDICATOR PLUGIN EXPORTED FUNCTIONS
        //
        // 1. GetFunctionTable - called when AFL engine is being initialized 
        // 1. SetSiteInteface - called when AFL engine is being initialized 
        //
        // Each function may be called multiple times.
        //
        // The order of calling functions during intialization is as follows:
        //
        // SetSiteInterface -> GetFunctionTable	-> Init -> 
        // ... normal work ....
        // Release
        //
        // This cycle may repeat multiple times
        /////////////////////////////////////////////////////////////////////////

        public static int GetFunctionTable(IntPtr functionTablePtr)
        {
            return 1;
        }

        public static int SetSiteInterface(IntPtr siteInterfacePtr)
        {
            return 1;
        }

        #endregion

        #region Data Exported Functions

        /// <summary>
        /// GetQuotesEx function is functional equivalent fo GetQuotes but
        /// handles new Quotation format with 64 bit date/time stamp and floating point volume/open int
        /// and new Aux fields
        /// it also takes pointer to context that is reserved for future use (can be null)
        /// Called by AmiBroker 5.27 and above 
        /// </summary>
        /// 

       public static List<string> yahoortdata = new List<String>();
               
        unsafe public static int GetQuotesEx(string ticker, Periodicity periodicity, int lastValid, int size, Quotation* quotes, GQEContext* context)
        {
            // TODO: Add logic here. Take a look at the demo below:
            int i = 0;
            string chktmp = ConfigurationManager.AppSettings["txtTargetFolderforami"];
            string timerflag = ConfigurationManager.AppSettings["timerflag"];
            if(timerflag=="1")
            {
                Status = StatusCode.OK;
              
            }
            if (timerflag == "2")
            {
                Status = StatusCode.Unknown ;
            }
            try
            {
                string tempfilepath = chktmp+"\\ShubhaRT.txt";
                int flag = 0;
                string datatostore = ticker;
                
                string Openinterest1 = ConfigurationManager.AppSettings["Openinterest"];
                string Open1 = ConfigurationManager.AppSettings["Open"];
                string High1 = ConfigurationManager.AppSettings["High"];
                string Low1 = ConfigurationManager.AppSettings["Low"];
            string Volume1 = ConfigurationManager.AppSettings["Volume"];

                string Ask1 = ConfigurationManager.AppSettings["Ask"];
                string Bid1 = ConfigurationManager.AppSettings["Bid"];

                string Preset1 = ConfigurationManager.AppSettings["preset"];
                string servername1 = ConfigurationManager.AppSettings["servername"];
                string refreshtime1 = ConfigurationManager.AppSettings["interval"];
                int volumecount = 0;
                int openinterestcount = 0;
                int opencount=0;
                int highcount=0;
                int lowcount=0;
                int askcount=0;
                int bidcount=0;
                int count = 2;
               //first 2 position of file is for 1.LTT,2.LTP,3.Volume
                if (Volume1  != "")
                {
                    //if user want openint it come at position 4 in file
                    count++;
                    volumecount = count;
                }
                if (Openinterest1 != "")
                {
                    //if user want openint it come at position 4 in file
                    count ++;
                    openinterestcount = count  ;
                }
                if (Open1 != "")
                {
                    count++;
                   opencount=count;
                }
                if (High1 != "")
                {
                    count++;
                    highcount=count;
                }
                if (Low1 != "")
                {
                   count++;
                    lowcount=count;
                }

                if (Ask1 != "")
                {
                   count++;
                    askcount=count;
                }
                if (Bid1 != "")
                {
                   count++;
                    bidcount=count;
                }

                ////////////////////////////////
                //very imp no | present in tiker name so add and then split
                ticker = "demo|" + ticker;
                    
               string[] wordsdata1 = ticker.Split('|');
                // string[] wordsdata1 = ticker.Split('|');

                //if (wordsdata1[1].Contains("-"))
                //{
                //    wordsdata1[1] = wordsdata1[1].Substring(0, wordsdata1[1].Length - 3);
                //}

                if (!File.Exists(chktmp +"\\" + wordsdata1[1] + ".csv"))
                {
                    return 0;
                }
               ////////////////////////////////imp

                using (var reader = new StreamReader(chktmp+"\\" + wordsdata1[1] + ".csv"))
                {
                    string line = null;

                   while ((line = reader.ReadLine()) != null)
                    {
                        string[] wordsdata = line.Split(',');

                DateTime date = Convert.ToDateTime(wordsdata[0]);
                TimeSpan time = TimeSpan.Parse(wordsdata[1]);
                DateTime dateTime = date.Add(time);

               
                quotes[i ].DateTime = PackDate(dateTime.AddDays(0));

                quotes[i ].Price = float.Parse(wordsdata[2], CultureInfo.InvariantCulture.NumberFormat);// Convert.ToInt32(wordsdata[3]);
                if (volumecount == 0)
                {
                    quotes[i].Volume = 0;
                }
                else
                {
                    quotes[i].Volume = float.Parse(wordsdata[volumecount], CultureInfo.InvariantCulture.NumberFormat);//Convert.ToInt32(wordsdata[4]);


                }
                if(opencount==0 )
                {
                    quotes[i].Open = float.Parse(wordsdata[2], CultureInfo.InvariantCulture.NumberFormat);// Convert.ToInt32(wordsdata[3]);;
                }
                else

                {
                quotes[i ].Open = float.Parse(wordsdata[opencount ], CultureInfo.InvariantCulture.NumberFormat);//Convert.ToInt32(wordsdata[3]);

                }

                       if(highcount==0)
                       {
                           quotes[i].High = float.Parse(wordsdata[2], CultureInfo.InvariantCulture.NumberFormat);// Convert.ToInt32(wordsdata[3]);;
                       }
                       else
                       {

                quotes[i ].High = float.Parse(wordsdata[highcount ], CultureInfo.InvariantCulture.NumberFormat);//Convert.ToInt32(wordsdata[3]);
                       }


                       if(lowcount==0 )
                       {
                           quotes[i].Low = float.Parse(wordsdata[2], CultureInfo.InvariantCulture.NumberFormat);// Convert.ToInt32(wordsdata[3]);;
                       }
                       else
                       {
                           quotes[i ].Low =float.Parse(wordsdata[lowcount ], CultureInfo.InvariantCulture.NumberFormat);// Convert.ToInt32(wordsdata[3]);
                       }

                       if(openinterestcount==0 )
                       {
                quotes[i ].OpenInterest = 0;

                       }
                       else
                       {
                quotes[i ].OpenInterest = float.Parse(wordsdata[openinterestcount ], CultureInfo.InvariantCulture.NumberFormat);;

                       }

                       if(askcount==0 )
                       {
                quotes[i ].AuxData1 = 0;
                       }
                       else
                       {
                quotes[i ].AuxData1 = float.Parse(wordsdata[askcount  ], CultureInfo.InvariantCulture.NumberFormat);;

                       }
               
                       if(bidcount ==0 )
                       {
                quotes[i ].AuxData2 = 0;
                       }
                       else
                       {
                quotes[i ].AuxData2 = float.Parse(wordsdata[bidcount], CultureInfo.InvariantCulture.NumberFormat);
                       }
                i++;
               
          }
                }
            }
            catch
            {
            }
            return i;
        }

        unsafe public delegate void* Alloc(uint size);
        
        ///// <summary>
        ///// GetExtra data is optional function for retrieving non-quotation data
        ///// </summary>
        public static AmiVar GetExtraData(string ticker, string name, int arraySize, Periodicity periodicity, Alloc alloc)
        {
            return new AmiVar();
        }

        /// <summary>
        /// Configure function is called when user presses "Configure" button in File->Database Settings
        /// </summary>
        /// <param name="path">Path to AmiBroker database</param>
        /// <param name="site">A pointer to <see cref="AmiBrokerPlugin.InfoSite"/></param>
        /// 
       

        public static int Configure(string path, IntPtr infoSitePtr)
        {
            GetStockQty = (GetStockQtyDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 4))), typeof(GetStockQtyDelegate));
            SetCategoryName = (SetCategoryNameDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 12))), typeof(SetCategoryNameDelegate));
            GetCategoryName = (GetCategoryNameDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 16))), typeof(GetCategoryNameDelegate));
            SetIndustrySector = (SetIndustrySectorDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 20))), typeof(SetIndustrySectorDelegate));
            GetIndustrySector = (GetIndustrySectorDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 24))), typeof(GetIndustrySectorDelegate));
            AddStock = (AddStockDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(Marshal.ReadInt32(new IntPtr(infoSitePtr.ToInt32() + 20))), typeof(AddStockDelegate));

           // MessageBox.Show(GetStockQty.ToString());
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"windows-data\");
            
            try
            {
                var registerdate = regKey.GetValue("sd");
                var paidornot = regKey.GetValue("sp");
               
           
            string chktmp = ConfigurationManager.AppSettings["ApplicationId"];
                //if user delete register entry then show login window agian 
            //if (chktmp == "2" && registerdate != null && paidornot!=null )
            //{
                ShubhaPlugin f = new ShubhaPlugin();
                f.ShowDialog();
            //}
            //else
            //{
            //    Login f = new Login();
            //    f.ShowDialog();
            //}
            }
            catch
            {

            }
            return 1;
        }

        /// <summary>
        /// GetRecentInfo function is optional, used only by real-time plugins 
        /// </summary>
        public static RecentInfo GetRecentInfo(string ticker)
        {
           return new RecentInfo();
        }

        /// <summary>
        /// New API function, optional, only for RT plugins
        /// </summary>
        public static int IsBackfillComplete(string ticker)
        {
            return 1;
        }

        /// <summary>
        /// GetSymbolLimit function is optional, used only by real-time plugins
        /// </summary>
        public static int GetSymbolLimit()
        {
            return 1;
        }

        /// <summary>
        /// GetStatus function is optional, used mostly by few real-time plugins
        /// </summary>
        /// <param name="statusPtr">A pointer to <see cref="AmiBrokerPlugin.PluginStatus"/></param>
        public static int  GetStatus(IntPtr statusPtr)
        {
            NotifyStreamingUpdate();

            
            switch (Status)
            {
                case StatusCode.OK:
                    SetStatus(statusPtr, StatusCode.OK, Color.LightGreen, "Connected", "Server Connected");
                    break;
                case StatusCode.Wait:
                    SetStatus(statusPtr, StatusCode.Wait, Color.LightBlue, "WAIT", "Wait a sec...");
                    break;
                case StatusCode.Error:
                    SetStatus(statusPtr, StatusCode.Error, Color.Red, "ERR", "An error occured");
                    break;
                default:
                    SetStatus(statusPtr, StatusCode.Unknown, Color.Red , "Dissconnected", "Server Dissconnected");
                    break;
            }

            return 1;
        }

        /// <summary>
        /// SetTimeBase function is called when user is changing base time interval in File->Database Settings
        /// </summary>
        public static int SetTimeBase(int periodicity)
        {
            return periodicity >= (int)Periodicity.OneSecond  && periodicity <= (int)Periodicity.OneMinute     ? 1 : 0;
        }

        /// <summary>
        /// Notify function (optional) is called when database is loaded, unloaded
        /// settings are changed, or right mouse button in the plugin status area is clicked
        /// </summary>
        public static void  Notify(ref PluginNotification notifyData)
        {         
            if (MainWnd == IntPtr.Zero)
            {
                MainWnd = notifyData.MainWnd;
            }

            if (notifyData.Reason == PluginNotificationReason.StatusRightClick)
            {
                Righclick r = new Righclick();
                r.ShowDialog();
             
            }

            //return 1;
        }

        #endregion
    }    
}
