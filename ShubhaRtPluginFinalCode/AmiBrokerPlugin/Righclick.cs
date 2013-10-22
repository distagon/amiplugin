
/*
 * Functions Description:
 * 1.RtdataRecall() is use to call nowdata () and nestdata() after given time interval 
 * 2.nestdata() is use to download real time data from nest terminal 
 * 3.nowdata() is use to download real time data from NOW terminal 
 * 4.StartRt_Click() start the server 
 * 5.StopRt_Click() stop the server 
 * 
 */

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
using Microsoft.Win32;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Management;
using System.Linq;



namespace AmiBrokerPlugin
{
    public partial class Righclick : Form
    {
        Configuration config;
        WebClient Client = new WebClient();
        List<string> marketsymbol = new List<string>();
        List<string> Exchangename = new List<string>();
        Type type;
        List<string> yahoortname = new List<String>();
        List<string> yahoortdata = new List<String>();
        List<string> symbolname = new List<String>();
        List<string> exchagename = new List<string>();
        int timeinterval = 0;
        IRtdServer m_server;

        object[] args = new object[3];

        Type ExcelType;
        object ExcelInst;
        List<int> marketsymboltoremove = new List<int>();
        System.Timers.Timer myTimer=new System.Timers.Timer() ;
        bool exitFlag = false;

        public Righclick()
        {
            InitializeComponent();
        }

        public void RtdataRecall()
        {




            string timerflag = ConfigurationManager.AppSettings["timerflag"];
            string interval = ConfigurationManager.AppSettings["interval"];
            
            int intervalforrt =1000* Convert.ToInt32(interval);
                // Tell the timer what top do when it elapses
            if(timerflag=="1" )
            {
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                if (intervalforrt == 0)
                {
                    myTimer.Interval = 500;
                }
                else
                {
                    myTimer.Interval = intervalforrt;

                }
            myTimer.Enabled = true;


               
            }
            else
            {
                myTimer.Enabled = false;
                myTimer.Stop();
                this.Close();
                MessageBox.Show("Server Stop");
            }

        }
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string timerflag = ConfigurationManager.AppSettings["timerflag"];

            string terminalname = ConfigurationManager.AppSettings["terminal"];
            if (timerflag == "1" )
            {
                //according to terminal real time data function call 
                if (terminalname == "nest.scriprtd")
                {
                    nestdata();

                }
                else if (terminalname == "now.scriprtd")
                {
                    nowdata();
                }

            }
            else
            {
            }

        }
        public void nestdata()
        {
            string chktmp = ConfigurationManager.AppSettings["txtTargetFolderforami"];
            string timetorep = "";
            List<string> mappingsymbolname = new List<string>();

            
            using (var reader1 = new StreamReader(chktmp + "\\ShubhaRtmappingsymbollist.txt"))
            {
                string line1 = null;
                while ((line1 = reader1.ReadLine()) != null)
                {
                    
                  //  MessageBox.Show(line1);

                    mappingsymbolname.Add(line1 );
                }


            }

            try
            {
                yahoortdata.Clear();
                int countformappingsymbol = 0;
                int flagfortotaldatacount = 0;
                int predatacount = 0;
                //read symbol list from file to download real time data
                using (var reader = new StreamReader(chktmp + "\\ShubhaRtsymbollist.txt"))
                {
                    string line = null;
                    int RTtopiccount = 0;

                    //take terminal name of perticular topic
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
                    while ((line = reader.ReadLine()) != null)
                    {

                        Array retval;

                        int i=0;
                        int j = m_server.Heartbeat();

                        bool bolGetNewValue = true;
                        object[,] arrayForSymbol = new object[2,9];

                        //inserting topic
                        if (LTT1!="")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = LTT1;
                            i++;
                        }

                        if (LTP1 !="")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = LTP1 ;
                            i++;
                        }
                        if (Volume1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Volume1 ;
                            i++;
                        }
                        if (Openinterest1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Openinterest1 ;
                            i++;
                        }
                        if (Open1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Open1 ;
                            i++;
                        }
                        if (High1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = High1 ;
                            i++;
                        }
                        if (Low1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Low1 ;
                            i++;
                        }
                       
                        if (Ask1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Ask1 ;
                            i++;
                        }
                        if (Bid1 != "")
                        {
                            arrayForSymbol[0,i] = line;
                            arrayForSymbol[1,i] = Bid1 ;
                            i++;
                        }

                        for (int j1 = 0; j1  < i ;j1++ )
                        {
                        object[] array= new object[2];

                            
                        array[0] = arrayForSymbol[0,j1];
                        array[1] = arrayForSymbol[1,j1];

                        Array sysArrParams = (Array)array;
                            m_server.ConnectData(j1 , sysArrParams, bolGetNewValue);
                            
                        }
                        retval = m_server.RefreshData(10);
                        for (int count = 0; count <= i ; count++)
                        {
                            m_server.DisconnectData(count);
                        }
                        foreach (var item in retval)
                        {
                            yahoortdata.Add(item.ToString());
                        }
                        m_server.ServerTerminate();
                        flagfortotaldatacount++;

                        string[] wordsdata = line.Split('|');
                        if(wordsdata[1].Contains("-"))
                        {
                            wordsdata[1] = wordsdata[1].Substring(0, wordsdata[1].Length - 3);
                        }
                        string datatostore = "";
                        int countofarray=0;
                        for (int filecount = i; filecount < i + i; filecount++)
                        {
                            datatostore = datatostore + "," + yahoortdata[filecount];
                            countofarray++;
                        }
                       // MessageBox.Show("asdasd------------"+mappingsymbolname[1]);
                     //  string tempfilepath = chktmp + "\\" + wordsdata[1] + ".csv";
                        string tempfilepath = chktmp + "\\" + mappingsymbolname[countformappingsymbol].ToString() + ".csv";
                        countformappingsymbol++;

                        //string tempfilepath = chktmp + "\\SHANTESH.csv";
                       

                        if (LTT1 == "LUT")
                        {
                            string[] sepratedateandtime = datatostore.Split(' ');
                            //MessageBox.Show(sepratedateandtime[1]);
                            datatostore = "," + sepratedateandtime[1];

                        }
                        string[] datacheck = datatostore.Split(',');
                        if (countofarray < 4)
                       {
                       }
                       else
                       {
                        if (datacheck[1].Length < 8)
                        {
                        }
                        else
                        {
                            DateTime date = DateTime.Today.Date;
                            //TimeSpan time = DateTime.Now.TimeOfDay;
                            //DateTime dateTime = date.Add(time);
                           string predata = ConfigurationManager.AppSettings["predata"];

                           // MessageBox.Show(predata[predatacount] + "====" + datatostore);

                            if (predata != datatostore)
                            {
                                datacheck=datatostore.Split(',');
                                try
                                {
                                    if (datacheck[4]=="")
                                    {
                                    }
                                    using (var writer = new StreamWriter(tempfilepath, true))
                                        writer.WriteLine(date.ToShortDateString() + datatostore);

                                }
                                catch
                                { 
                                }
                               
                                    
                                
                            }
                        }
                       
                        config.AppSettings.Settings.Remove("predata");

                        config.AppSettings.Settings.Add("predata", datatostore);
                        config.Save(ConfigurationSaveMode.Full);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                        yahoortdata.Clear();

                    }

                }
            }
            catch(Exception ex)
            {
               // MessageBox.Show(ex.Message );
            }
        }
        public void nowdata()
        {
            string chktmp = ConfigurationManager.AppSettings["txtTargetFolderforami"];
            string timetorep = "";
            try
            {
                yahoortdata.Clear();

                int flagfortotaldatacount = 0;
                using (var reader = new StreamReader(chktmp + "\\ShubhaRtsymbollist.txt"))
                {
                    string line = null;
                    int RTtopiccount = 0;
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
                    string refreshtime1 = ConfigurationManager.AppSettings["interval"];
                    while ((line = reader.ReadLine()) != null)
                    {

                        Array retval;

                        int i = 0;
                        int j = m_server.Heartbeat();

                        bool bolGetNewValue = true;
                        object[,] arrayForSymbol = new object[3, 9];

                        if (LTT1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = LTT1;
                            i++;
                        }

                        if (LTP1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = LTP1;
                            i++;
                        }
                        if (Volume1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Volume1;
                            i++;
                        }
                        if (Openinterest1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Openinterest1;
                            i++;
                        }
                        if (Open1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Open1;
                            i++;
                        }
                        if (High1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = High1;
                            i++;
                        }
                        if (Low1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Low1;
                            i++;
                        }

                        if (Ask1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Ask1;
                            i++;
                        }
                        if (Bid1 != "")
                        {
                            arrayForSymbol[0, i] = "MktWatch";

                            arrayForSymbol[1, i] = line;
                            arrayForSymbol[2, i] = Bid1;
                            i++;
                        }

                        for (int j1 = 0; j1 < i; j1++)
                        {
                            object[] array = new object[3];


                            array[0] = arrayForSymbol[0, j1];
                            array[1] = arrayForSymbol[1, j1];
                            array[2] = arrayForSymbol[2, j1];

                            Array sysArrParams = (Array)array;
                            
                            m_server.ConnectData(j1, sysArrParams, bolGetNewValue);

                        }
                        retval = m_server.RefreshData(10);
                        for (int count = 0; count <= i; count++)
                        {
                            m_server.DisconnectData(count);
                        }
                        foreach (var item in retval)
                        {
                            yahoortdata.Add(item.ToString());
                        }

                        m_server.ServerTerminate();
                        flagfortotaldatacount++;
                        string[] wordsdata = line.Split('|');
                        if (wordsdata[1].Contains("-"))
                        {
                            wordsdata[1] = wordsdata[1].Substring(0, wordsdata[1].Length - 3);
                        }

                        string datatostore = "";
                        for (int filecount = i; filecount < i + i; filecount++)
                        {
                            datatostore = datatostore + "," + yahoortdata[filecount];
                        }
                       
                        string tempfilepath = chktmp + "\\" + wordsdata[1] + ".csv";                       
                        string[] datacheck = datatostore.Split(',');
                        if (datacheck[1].Length < 8)
                        {
                        }
                        else
                        {
                            DateTime date = DateTime.Today.Date;
                            TimeSpan time = DateTime.Now.TimeOfDay;
                            DateTime dateTime = date.Add(time);
                            string predata = ConfigurationManager.AppSettings["predata"];
                            if (predata != datatostore)
                            {
                                using (var writer = new StreamWriter(tempfilepath, true))
                                    writer.WriteLine(dateTime.ToShortDateString() + datatostore);
                            }
                        }
                        config.AppSettings.Settings.Remove("predata");

                        config.AppSettings.Settings.Add("predata", datatostore);
                        config.Save(ConfigurationSaveMode.Full);
                        ConfigurationManager.RefreshSection("appSettings");
                        yahoortdata.Clear();

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void StartRt_Click(object sender, EventArgs e)
        {
            //check sp user or trial version 
            ///////////////////////////////
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"windows-data\");

            try
            {
                var registerdate = regKey.GetValue("sd");
                var paidornot = regKey.GetValue("sp");

                DateTime reg = Convert.ToDateTime(registerdate);
                reg = reg.AddDays(2);

                if (paidornot.ToString() == "Key for xp")
                {
                    if (reg < DateTime.Today.Date)
                    {
                        Uri a = new System.Uri("http://besttester.com/lic/licforami.txt");

                        // webBrowser1.Source = a;
                        string credentials = "liccheck:lic123!@#";
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(a);
                        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
                        request.PreAuthenticate = true;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        StreamReader reader = new StreamReader(response.GetResponseStream());

                        ////////////////////////////////////////////

                        string[] serverdata = reader.ReadToEnd().Split(',');
                        string[] serverdata1 = null;
                        int flagforuserpresentonserver = 0;
                        for (int i = 0; i < serverdata.Count(); i++)
                        {
                            serverdata1 = serverdata[i].Split(' ');
                            DateTime dateonserver = Convert.ToDateTime(serverdata1[1]);

                            ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                            dsk1.Get();
                            string id1 = dsk1["VolumeSerialNumber"].ToString();
                            if (id1 == serverdata1[0])
                            {
                                flagforuserpresentonserver = 1;
                                if (dateonserver < DateTime.Today.Date)
                                {
                                    System.Windows.Forms.MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");

                                }
                            }
                        }

                        if (flagforuserpresentonserver == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
                        }

                    }
                    else
                    {

                    }

                }
                else
                {
                    if (paidornot.ToString() == "1001")
                    {
                    }
                    else
                    {
                        MessageBox.Show("Trial version expired please contact to sale@shubhalabha.in ");
                        this.Close();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            string targetpath = ConfigurationManager.AppSettings["txtTargetFolderforami"];

           



                //ExcelType = Type.GetTypeFromProgID("Broker.Application");
                //ExcelInst = Activator.CreateInstance(ExcelType);
                //ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                //          ExcelInst, new object[1] { true });
                //ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                //     ExcelInst, new string[1] { "c://RTDATA//amirtdatabase" });
                





            ///////////////////////////////////////////////////////////

                ///////////////////////////////////
                //start server or terminal 

                string terminalname = ConfigurationManager.AppSettings["terminal"];

                try
                {

                    Type type = Type.GetTypeFromProgID(terminalname);

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.AppSettings.Settings.Remove("timerflag");

                    config.AppSettings.Settings.Add("timerflag", "1");
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                    this.Hide();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Please start ........" + terminalname);
                    return;
                }
                ///////////////////////////////////////////////////////////




                RtdataRecall();
           
        }

        private void StopRt_Click(object sender, EventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("timerflag");

            config.AppSettings.Settings.Add("timerflag", "2");
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Server Stop  ");
            this.Close();
        }
        private static void JoinCsvFiles(string[] csvFileNames, string outputDestinationPath)
        {
            StringBuilder sb = new StringBuilder();

            bool columnHeadersRead = false;

            foreach (string csvFileName in csvFileNames)
            {
                TextReader tr = new StreamReader(csvFileName);

                string columnHeaders = tr.ReadLine();

                // Skip appending column headers if already appended
                if (!columnHeadersRead)
                {
                    sb.AppendLine(columnHeaders);
                    columnHeadersRead = true;
                }




                sb.AppendLine(tr.ReadToEnd());

                tr.Close();


            }


            File.WriteAllText(outputDestinationPath, sb.ToString());


        }
     
        private void Righclick_Load(object sender, EventArgs e)
        {
            
        }
    }
}
