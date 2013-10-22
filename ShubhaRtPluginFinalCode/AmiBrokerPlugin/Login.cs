/*file use for authentication of user for one time with registration 
 * 
 * */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Configuration;
using Microsoft.Win32;
namespace AmiBrokerPlugin
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
       
        public void SetRegKey()
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"windows-data\");
            regKey.SetValue("sd",DateTime.Today.Date.ToString() );
            regKey.SetValue("sp","Key for xp");
        }

        //close application 
        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Login_Load(object sender, EventArgs e)
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string leadurl = filepath.Substring(0, filepath.Length - 12) + "Ami reg.html";

            Uri a3 = new System.Uri("http://shubhalabha.in/amirt/Ami_reg.html");
            lead.Url = a3;
        }

       //check registartion is done or not 
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (lead.Url.ToString() == "http://shubhalabha.in/eng/crm/index.php?entryPoint=WebToLeadCapture")
            {
                System.Windows.Forms.MessageBox.Show("Thank you for using shubha real time  ");

                Configuration config;
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("ApplicationId");

                config.AppSettings.Settings.Add("ApplicationId", "2");
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("ApplicationId");
                SetRegKey();
                this.Hide();
                ShubhaPlugin s = new ShubhaPlugin();
                s.ShowDialog();
                return;
            }
        }

    }
}
