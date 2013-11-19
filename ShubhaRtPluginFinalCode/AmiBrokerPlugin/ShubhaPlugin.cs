/*Functions Description:
 * 1.ShubhaPlugin_Load() laod form with all initial/save  values
 * 2.dataGridView1_CellContentClick() give symbol name of selected row
 * 3.Save_Config_Click() save config tab data
 * 4.Remove_Rtsymbol_Click() remove symbol from realtimesymbol file 
 * 5.Movesymboltort_Click() move symbol from amibroker to realtimesymbol file 
 * 6.RTtab_save_Click() save data of rt tab 
 * 7.select_all_Click() select all symbols and store it in realtimesymbol file 
 * 8.GetDataTable() load csv/txt file in datagridview
 * This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
 * */

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Timers;
using System.Configuration;
using System.Net;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Net.Mail;
using System.Text;
using System.Globalization;
using System.Data;
using System.Threading;
using System.Windows;

using System.ComponentModel;
using ManagedWinapi.Windows;
using ManagedWinapi.Accessibility;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;
using System.IO.Compression;
using System.Diagnostics;

using System.Text.RegularExpressions;
using System.Data.OleDb;

using System.Security.Principal;

namespace AmiBrokerPlugin
{
    public partial class ShubhaPlugin : Form
    {
         Configuration config;
         WebClient Client = new WebClient();
         string symbolnametostore = "";
        List<string> yahoortdata = new List<String>();
        IRtdServer m_server;
        List<string> tradingsymbolname = new List<string>();
        List<string> mappingsymbol = new List<string>();
        System.Timers.Timer myTimer;
        public ShubhaPlugin()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        public static IntPtr FindChildWindow(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszTitle)
        {
            // Try to find a match.
            IntPtr hwnd = FindWindowEx(hwndParent, IntPtr.Zero, lpszClass, lpszTitle);
            if (hwnd == IntPtr.Zero)
            {
                // Search inside the children.
                IntPtr hwndChild = FindWindowEx(hwndParent, IntPtr.Zero, null, null);
                while (hwndChild != IntPtr.Zero && hwnd == IntPtr.Zero)
                {
                    hwnd = FindChildWindow(hwndChild, IntPtr.Zero, lpszClass, lpszTitle);
                    if (hwnd == IntPtr.Zero)
                    {
                        // If we didn't find it yet, check the next child.
                        hwndChild = FindWindowEx(hwndParent, hwndChild, null, null);
                    }
                }
            }
            return hwnd;
        }
        public static System.Data.DataTable GetDataTable(string strFileName)
        {
            ADODB.Connection oConn = new ADODB.Connection();
            oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";", "", "", 0);
            string strQuery = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
            ADODB.Recordset rs = new ADODB.Recordset();
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
            System.Data.DataTable dt = new System.Data.DataTable();
            rs.Open(strQuery, "Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";",
                ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, 1);
            adapter.Fill(dt, rs);
            return dt;
        }

        private void ShubhaPlugin_Load(object sender, EventArgs e)
        {
           
            int  item = 1;
            Sec.Items.Add(item.ToString());
            item = 3;
            Sec.Items.Add(item.ToString());
            item = 5;
            Sec.Items.Add(item.ToString());
            item = 10;
            Sec.Items.Add(item.ToString());
            item = 20;
            Sec.Items.Add(item.ToString());



           Sec.SelectedIndex = 2;
           var terminalname1 = "";
           preset.Items.Add("NEST");
           preset.Items.Add("NOW");
           try
           {
               RegistryKey regKey = Registry.CurrentUser;
               regKey = regKey.CreateSubKey(@"windows-data\");
               terminalname1 = regKey.GetValue("terminal").ToString();


            if (terminalname1 == "NEST")
            {
                preset.SelectedIndex = 0;

            }
            else if (terminalname1=="NOW")
            {
                preset.SelectedIndex = 1;

            }
           }

           catch
           {

           }
           

            string terminalname = ConfigurationManager.AppSettings["terminal"];

            string LTT1 = ConfigurationManager.AppSettings["LTT"];
            string LTP1 = ConfigurationManager.AppSettings["LTP"];
            string Volume1 = ConfigurationManager.AppSettings["Volume"];
            string Openinterest1 = ConfigurationManager.AppSettings["Openinterest"];
            string Open1 = ConfigurationManager.AppSettings["Open"];
            string High1 = ConfigurationManager.AppSettings["High"];
            string Low1 = ConfigurationManager.AppSettings["Low"];

            string Ask1 = ConfigurationManager.AppSettings["Ask"];
            string Bid1 = ConfigurationManager.AppSettings["Bid"];

            string Preset1 = ConfigurationManager.AppSettings["preset"];
            string servername1 = ConfigurationManager.AppSettings["servername"];
            string   refreshtime1 = ConfigurationManager.AppSettings["interval"];
            //set user save data 
            if (refreshtime1 != null)
            {
                Sec.SelectedItem = refreshtime1;

            }
            if (servername1  != null)
            {
                servername.Text = servername1 ;
            }
            if(terminalname!=null )
            {
            terminal.Text = terminalname;
            }

            if (Preset1  != null)
            {
                preset.SelectedItem  = Preset1 ;
            }
            if (LTT1  != null)
            {
                LTT.Text = LTT1;
            }

            if (LTP1  != null)
            {
                LTP.Text = LTP1;
            }
            if (Volume1  != null)
            {
                Volume .Text = Volume1 ;
            }
            if (Openinterest1  != null)
            {
                Openinterest .Text = Openinterest1 ;
            }
            if (Open1  != null)
            {
                Open .Text = Open1 ;
            }
            if (High1  != null)
            {
                High.Text = High1 ;
            }
            if (Low1  != null)
            {
                Low.Text = Low1 ;
            }
            
            if (Ask1  != null)
            {
                Ask .Text = Ask1 ;
            }
            if (Bid1  != null)
            {
                Bid .Text = Bid1 ;
            }            
            try
            {
                string chktmp = ConfigurationManager.AppSettings["txtTargetFolderforami"];
                bool btemp = false;

                if (chktmp == null)
                {
                    this.txtTargetFolder.Text = "C:\\Data";
                }
                {
                    this.txtTargetFolder.Text = chktmp;

                }
                
            }
            catch
            {
            }

           
            
            //get symbol list from amibroker database 
            Type abType = Type.GetTypeFromProgID("Broker.Application");
            object abApp = Activator.CreateInstance(abType);
            object abDoc = abType.InvokeMember("ActiveDocument",
            BindingFlags.GetProperty, null, abApp, null);
            
            object ticker1 = abType.InvokeMember("Stocks", BindingFlags.GetProperty, null,
                      abApp, null);


            var symbolname = abType.InvokeMember("GetTickerList", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                      ticker1, new object[1] { 0 });

            string symbol = "    Symbol Name\n"+symbolname.ToString().Replace(',', '\n');
            File.WriteAllText(txtTargetFolder.Text + "\\ShubhaRT.txt", symbol);
            
            try
            {
               //load txt file to datagridview
                System.Data.DataTable dt = GetDataTable(txtTargetFolder.Text + "\\ShubhaRT.txt");
                dataGridView1.DataSource = dt.DefaultView;

            }
            catch
            {
            }


            try
            {
                using (var reader = new StreamReader(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt"))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        tradingsymbolname.Add(line);
                       
                    }
                }
            }
            catch
            {
            }
            try
            {
                using (var reader = new StreamReader(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt"))

                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        mappingsymbol .Add(line);

                    }
                }
            }
            catch
            {
            }


            dataGridView2.Columns.Clear();


            dataGridView2.Columns.Add("Symbol", "Symbol");
            dataGridView2.Columns.Add("Mapping Symbol", "Mapping Symbol");
            dataGridView2.Columns[0].Width = 200;
            dataGridView2.Columns[1].Width = 200;

            try
            {

                for (int i = 0; i < tradingsymbolname.Count; i++)
                {

                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = tradingsymbolname[i];
                    row.Cells[1].Value = mappingsymbol[i];
                    dataGridView2.Rows.Add(row);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

         }
     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            symbolnametostore = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            
            
        }

        private void Save_Config_Click(object sender, EventArgs e)
        {
            
        }

        private void Remove_Rtsymbol_Click(object sender, EventArgs e)
        {
            
        }

       
        private void Movesymboltort_Click(object sender, EventArgs e)
        {
           

        }

        private void path_Click(object sender, EventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                txtTargetFolder.Text = Target_Folder_Path;


            }
        }

        private void RTtab_save_Click(object sender, EventArgs e)
        {

            //save all user enter value in app config 
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            config.AppSettings.Settings.Remove("terminal");

            config.AppSettings.Settings.Add("terminal", terminal.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("servername");

            config.AppSettings.Settings.Add("servername", servername.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("interval");
            config.AppSettings.Settings.Add("interval", Sec.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("preset");

            config.AppSettings.Settings.Add("preset", preset.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("LTT");

            config.AppSettings.Settings.Add("LTT", LTT.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("LTP");

            config.AppSettings.Settings.Add("LTP", LTP.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("Volume");

            config.AppSettings.Settings.Add("Volume", Volume.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");



            config.AppSettings.Settings.Remove("Openinterest");

            config.AppSettings.Settings.Add("Openinterest", Openinterest.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("Open");

            config.AppSettings.Settings.Add("Open", Open.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("High");

            config.AppSettings.Settings.Add("High", High.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("Low");

            config.AppSettings.Settings.Add("Low", Low.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("Ask");

            config.AppSettings.Settings.Add("Ask", Ask.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");



            config.AppSettings.Settings.Remove("Bid");

            config.AppSettings.Settings.Add("Bid", Bid.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Setting saved.");

        }

        private void Rttab_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shubhalabha.in/contact-us/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shubhalabha.in/disclaimer/license/");
        }

        public void getsymbol()
        {




            string preset = ConfigurationManager.AppSettings["preset"];
            Process[] processes = null;
            Type type;

           
           
            if(preset=="NEST")
            {
           
            try
            {
                type = Type.GetTypeFromProgID("nest.scriprtd");

                m_server = (IRtdServer)Activator.CreateInstance(type);
                processes = Process.GetProcessesByName("NestTrader");


            }
            catch
            {

                MessageBox.Show(" Please start Teminal  as Run as Administrator");
                return;
              

            }
            }
            else  if (preset == "NOW")
            {
                try
                {
                    type = Type.GetTypeFromProgID("now.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    processes = Process.GetProcessesByName("NOW");

                }
                catch
                {
                    MessageBox.Show(" Please start Teminal  as Run as Administrator");
                    return;
                }

            }
                 IntPtr abcd = new IntPtr();
                IntPtr abcd1 = new IntPtr();
                IntPtr windowHandle = new IntPtr();



                List<Thread> processtostartback = new List<Thread>();
                SystemAccessibleObject sao, f, finalobject;

                foreach (Process p in processes)
                {
                    windowHandle = p.MainWindowHandle;

                    //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                    abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                    abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");


                    // do something with windowHandle
                }

                SystemWindow a = new SystemWindow(abcd1);


                // sao = SystemAccessibleObject.FromPoint(4, 200);
                try
                {
                    string marketwatch = abcd1.ToString();
                    if (marketwatch == "0")
                    {
                    MessageBox.Show("Nest is not running or Market Watch not present ");

                    }
                    sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);
                }
                catch
                {
                    MessageBox.Show("Market Watch not found ");
                    return;
                }



                f = sao.Children[3];



            //////////////////////////////
            //checking nest fileds 

                finalobject = f.Children[0];
                string s1 = finalobject.Description;

                int flag = 0;
                string[] checkterminalcol = s1.Split(',');
                string marketwathrequiredfield = "";
                try
                {
                    for (int i = 0; i < 20; i++)
                    {
                        marketwathrequiredfield = marketwathrequiredfield + checkterminalcol[i].ToString();
                    }
                }
                catch
                {
                }

              
                if (!marketwathrequiredfield.Contains("Exchange"))
                {
                    flag = 1;
                }
                if (flag == 1)
                {
                   MessageBox.Show("Trading symbol or Exchange column is missing \n Trading Symbol must be first column ");
                   return;
                }







            ///////////////////////////////














                //dataGridView2.Columns.Clear();


                //dataGridView2.Columns.Add("Symbol", "Symbol");
                //dataGridView2.Columns.Add("Mapping Symbol", "Mapping Symbol");
                //dataGridView2.Columns[0].Width = 200;
                //dataGridView2.Columns[1].Width = 200;

                try
                {
                    for (int i = 0; i < 50; i++)
                    {
                         s1 = f.Children[i].Description;
                         string []exchage = null ;
                         string exchagename = "";

                         checkterminalcol = s1.Split(',');
                         try
                         {
                             for (int j = 0; j < 20; j++)
                             {
                                 if (checkterminalcol[j].Contains("Exchange"))
                                 {

                                     exchage = checkterminalcol[j].Split(':');
                                     if (exchage[1].Contains("NSE"))
                                     {
                                         exchagename = "nse_cm";

                                     }
                                     else if (exchage[1].Contains("NFO"))
                                     {
                                         exchagename = "nse_fo";

                                     }
                                     else if (exchage[1].Contains("MCX"))
                                     {
                                         exchagename = "mcx_fo";

                                     }
                                     else if (exchage[1].Contains("CDE"))
                                     {
                                         exchagename = "cde_fo";

                                     }
                                 }
                             }
                         }
                         catch
                         {
                         }
                         if (exchagename != "" && f.Children[i].Name != "" && f.Children[i].Name != null )
                        {
                            int flagforsym = 0;
                            try
                            {
                                //check symbol already present or not 
                                for (int j = 0; j  < tradingsymbolname.Count-1; j++)
                                {
                                    if (tradingsymbolname[j ] == exchagename + "|" + f.Children[i].Name)
                                   {

                                       flagforsym = 1;
                                   }


                                }
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                   // MessageBox.Show(row.Cells[0].Value.ToString());
                                    if (exchagename + "|" + f.Children[i].Name == row.Cells[0].Value.ToString())
                                    {
                                        flagforsym = 1;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                              //  MessageBox.Show(ex.Message);
                            }

                             //add symbol if not present 
                            if (flagforsym==0)
                             {
                        DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                        row.Cells[0].Value =exchagename+"|"+ f.Children[i].Name;
                        row.Cells[1].Value = f.Children[i].Name;

                        dataGridView2.Rows.Add(row);
                        flagforsym = 0;
                             }
                        }
                    }
                }
                catch
                {

                }

            
           

        }

        //import symbol from nest
        private void button1_Click(object sender, EventArgs e)
        {
          
            ///////////////// save config


            save();


            if(txtTargetFolder.Text=="")
            {
                MessageBox.Show("Please select download path ");
                return;
            }
            getsymbol();
             

        }

        //Add symbol into amibroker 

        public void save()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            config.AppSettings.Settings.Remove("terminal");

            config.AppSettings.Settings.Add("terminal", terminal.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("servername");

            config.AppSettings.Settings.Add("servername", servername.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("interval");
            config.AppSettings.Settings.Add("interval", Sec.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("preset");

            config.AppSettings.Settings.Add("preset", preset.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("LTT");

            config.AppSettings.Settings.Add("LTT", LTT.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("LTP");

            config.AppSettings.Settings.Add("LTP", LTP.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("Volume");

            config.AppSettings.Settings.Add("Volume", Volume.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");



            config.AppSettings.Settings.Remove("Openinterest");

            config.AppSettings.Settings.Add("Openinterest", Openinterest.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("Open");

            config.AppSettings.Settings.Add("Open", Open.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("High");

            config.AppSettings.Settings.Add("High", High.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("Low");

            config.AppSettings.Settings.Add("Low", Low.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("Ask");

            config.AppSettings.Settings.Add("Ask", Ask.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");



            config.AppSettings.Settings.Remove("Bid");

            config.AppSettings.Settings.Add("Bid", Bid.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            //////////
        }
        private void preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(preset.SelectedItem=="NOW")
            {
                LTT.Text="Last Trade Time";
                LTP.Text = "Last Traded Price";
                Volume.Text = "Volume Traded Today";
                Openinterest.Text = "Open Interest";
                terminal.Text = "now.scriprtd";
                        
            }
            if (preset.SelectedItem == "NEST")
            {
                LTT.Text = "LTT";
                LTP.Text = "LTP";
                Volume.Text = "Volume Traded Today";
                Openinterest.Text = "Open Interest";
                terminal.Text = "nest.scriprtd";


            }
        }


        //save data in txt files 
        private void button3_Click(object sender, EventArgs e)
        {
            //save configaration of config file 
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("txtTargetFolderforami");

            config.AppSettings.Settings.Add("txtTargetFolderforami", txtTargetFolder.Text.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            
            //save data into file 
            System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt");

            System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt");
           
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt", true))
                    writer.WriteLine(dataGridView2.Rows[i].Cells[0].Value);
            }

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                


                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt", true))
                        writer.WriteLine(dataGridView2.Rows[i].Cells[1].Value);
              
               
            }


            //add all symbols into amibroker 
            try
            {
                string chktmp = ConfigurationManager.AppSettings["txtTargetFolderforami"];
                Type abType = Type.GetTypeFromProgID("Broker.Application");
                object abApp = Activator.CreateInstance(abType);
                object abDoc = abType.InvokeMember("ActiveDocument",
                BindingFlags.GetProperty, null, abApp, null);
                List<string> symbolname = new List<String>();

                object ticker1 = abType.InvokeMember("Stocks", BindingFlags.GetProperty, null,
                          abApp, null);
                using (var reader = new StreamReader(chktmp + "\\ShubhaRtmappingsymbollist.txt"))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        abType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                                ticker1, new object[1] { line });
                    }

                }
            }
            catch
            {
            }
            /////////////////////////////////////



           


            MessageBox.Show("File save successfully");  


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ShubhaPlugin_FormClosed(object sender, FormClosedEventArgs e)
        {
            save();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {



            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.RemoveAt(item.Index);
            }
        }

        private void txtTargetFolder_TextChanged(object sender, EventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("txtTargetFolderforami");

            config.AppSettings.Settings.Add("txtTargetFolderforami", txtTargetFolder.Text.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

      
    }
}
