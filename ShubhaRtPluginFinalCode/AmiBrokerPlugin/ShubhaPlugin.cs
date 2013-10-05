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
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace AmiBrokerPlugin
{
    public partial class ShubhaPlugin : Form
    {
         Configuration config;
         WebClient Client = new WebClient();
         string symbolnametostore = "";
        List<string> yahoortdata = new List<String>();
        IRtdServer m_server;
        System.Timers.Timer myTimer;
        public ShubhaPlugin()
        {
            InitializeComponent();
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
            for (int i = 0; i < 60; i++)
            {
                Sec.Items.Add(i.ToString());
            }
           Sec.SelectedIndex = 5;

           exchang.Items.Add("nse_cm");
           exchang.Items.Add("nse_fo");
           exchang.Items.Add("cde_fo");
           exchang.Items.Add("mcx_fo");

           exchang.SelectedIndex = 1;
            

            preset.Items.Add("NEST");
            preset.Items.Add("NOW");

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

            try
            {
                using (var reader = new StreamReader(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt"))
                {
                    string line = null;
                    // System.IO.File.WriteAllText("C:\\data\\csvfiledata.txt", reader.ReadLine());

                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);
                    }
                }



                using (var reader = new StreamReader(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt"))
                {
                    string line = null;
                    // System.IO.File.WriteAllText("C:\\data\\csvfiledata.txt", reader.ReadLine());

                    while ((line = reader.ReadLine()) != null)
                    {
                        mappingsymbol .Items.Add(line);
                    }
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
         }
     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            symbolnametostore = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            
            
        }

        private void Save_Config_Click(object sender, EventArgs e)
        {
            //save configaration of config file 
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("txtTargetFolderforami");

            config.AppSettings.Settings.Add("txtTargetFolderforami", txtTargetFolder.Text.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            MessageBox.Show("File save successfully");
        }

        private void Remove_Rtsymbol_Click(object sender, EventArgs e)
        {
            //removes symbol from ShubhaRtsymbollist.txt file 
            try
            {
                int count = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

                System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt");

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt", true))
                        writer.WriteLine(listBox1.Items[i].ToString());
                }


               
                mappingsymbol.Items.RemoveAt(count);
               
                System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt");

                for (int i = 0; i < mappingsymbol.Items.Count; i++)
                {
                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt", true))
                        writer.WriteLine(mappingsymbol.Items[i].ToString());
                }
                
            }
            catch
            {
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Movesymboltort_Click(object sender, EventArgs e)
        {
            //moves symbol to ShubhaRtsymbollist.txt
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (symbolnametostore == listBox1.Items[i].ToString())
                {
                    MessageBox.Show("Already present");
                    return;
                }
            }

            for (int i = 0; i < mappingsymbol .Items.Count; i++)
            {
                if (mappingsymbol_txt.Text  == mappingsymbol.Items[i].ToString())
                {
                    MessageBox.Show("Already present");
                    return;
                }
            }

            if (symbolnametostore != "")
            {
                listBox1.Items.Add(symbolnametostore);
                using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt", true))
                    writer.WriteLine(symbolnametostore);

                listBox1.Items.Add(mappingsymbol.Text );
                using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt", true))
                    writer.WriteLine(mappingsymbol.Text);
            }

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

        private void select_all_Click(object sender, EventArgs e)
        {
            //select all symbol and store it into ShubhaRtsymbollist.txt
            listBox1.Items.Clear();
            try
            {
                File.Delete(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    listBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt", true))
                        writer.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }

                MessageBox.Show("All symbols added successfully");
            }
            catch
            {

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shubhalabha.in/contact-us/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shubhalabha.in/disclaimer/license/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(symbolname.Text=="")
            {
                MessageBox.Show("Please Enter symbol name ");
                return;
            }
            if (mappingsymbol_txt .Text == "")
            {
                MessageBox.Show("Please Enter mappingsymbol  name ");
                return;
            }
            listBox1.Items.Add(exchang.SelectedItem + "|" + symbolname.Text);
            mappingsymbol.Items.Add(mappingsymbol_txt.Text );

            try
            {

                System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt");

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtsymbollist.txt", true))
                        writer.WriteLine(listBox1.Items[i].ToString());
                }


                System.IO.File.Delete(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt");

                for (int i = 0; i < mappingsymbol.Items.Count; i++)
                {
                    using (var writer = new StreamWriter(txtTargetFolder.Text + "\\ShubhaRtmappingsymbollist.txt", true))
                        writer.WriteLine(mappingsymbol.Items[i].ToString());
                }
               



           //     string symbolnametostore = exchang.SelectedItem + "|" + symbolname.Text;
           //     Type abType = Type.GetTypeFromProgID("Broker.Application");
           //     object abApp = Activator.CreateInstance(abType);
           //     object abDoc = abType.InvokeMember("ActiveDocument",
           //     BindingFlags.GetProperty, null, abApp, null);

           //     object ticker1 = abType.InvokeMember("Stocks", BindingFlags.GetProperty, null,
           //               abApp, null);


           //abType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public, null,
           //               ticker1, new object[1] { symbolnametostore });

               
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /////////////////////////////////////
            Type abType = Type.GetTypeFromProgID("Broker.Application");
            object abApp = Activator.CreateInstance(abType);
            object abDoc = abType.InvokeMember("ActiveDocument",
            BindingFlags.GetProperty, null, abApp, null);
            List<string> symbolname = new List<String>();

            object ticker1 = abType.InvokeMember("Stocks", BindingFlags.GetProperty, null,
                      abApp, null);

            for (int i = 0; i < listBox1.Items.Count;i++ )
            {
                string[] wordsdata1 = listBox1.Items[i].ToString().Split('|');

                string symbolnametostore = listBox1.Items[i].ToString();
              // MessageBox.Show( symbolnametostore);
               // symbolname.Add(symbolnametostore);
                abType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                              ticker1, new object[1] { mappingsymbol.Items[i].ToString()});
               
            }
           
            MessageBox.Show("All symbol added successfully  ");
            //int lengthforsymbol = 0;
            // for (int i = 0; i < symbolname.Count-1;i++ )
            //{

            // lengthforsymbol = symbolname[i].Length-1;
            // string sysmbol1 = symbolname[i].Substring(lengthforsymbol,;
            // MessageBox.Show(sysmbol1.Substring(0,lengthforsymbol-1));
            //abType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public, null,
            //                  ticker1, new object[1] { symbolname[i] });
            // }
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
    }
}
