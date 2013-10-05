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
        private void register_Click(object sender, EventArgs e)
        {
            //to register user on to shubhalabha community
            System.Diagnostics.Process.Start("http://shubhalabha.in/community/wp-login.php?action=register");

        }
        public void SetRegKey()
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"windows-data\");
            regKey.SetValue("sd",DateTime.Today.Date.ToString() );
            regKey.SetValue("sp","Key for xp");
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            //user authentication
            try
            {
                string loginUri = "http://shubhalabha.in/community/wp-login.php";

                string reqString = "log=" + username.Text + "&pwd=" + password.Text;
                byte[] requestData = Encoding.UTF8.GetBytes(reqString);

                CookieContainer cc = new CookieContainer();
                var request = (HttpWebRequest)WebRequest.Create(loginUri);
                request.Proxy = null;
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.Method = "post";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = requestData.Length;
                using (Stream s = request.GetRequestStream())
                    s.Write(requestData, 0, requestData.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    int count = 1;
                    foreach (Cookie c in response.Cookies)
                    {
                        //responce 2 contain loggen in or not 
                        if (count == 2)
                        {
                            if (c.ToString().Contains("wordpress_logged_in_17e90d9fdb1ef2a442ed2d6aeb707f54"))
                            {
                                System.Windows.Forms.MessageBox.Show("Thank you for using shubhaRt plugin ");
                                try
                                {
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
                                catch
                                {

                                }
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Please Enter Valid UserName & Password ");

                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }
            catch
            {

            }
        }

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
