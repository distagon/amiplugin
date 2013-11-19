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
    public partial class Wizard : Form
    {
        public Wizard()
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
        private void Wizard_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.Width = 400;
        }

        //checking exchange col present or not in terminal
        public void checkexchange()
        {


            IRtdServer m_server;


            string terminalname = ConfigurationManager.AppSettings["terminalname"];
            Process[] processes = null;
            Type type;

            if (nest.Checked == true)
            {
                try
                {
                    type = Type.GetTypeFromProgID("nest.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    processes = Process.GetProcessesByName("NestTrader");


                }
                catch
                {

                   
                       Result.Text=" Please start Nest as Run as Administrator";
                        return;
                    

                }

            }
            else if (now.Checked == true)
            {
                try
                {
                    type = Type.GetTypeFromProgID("now.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    processes = Process.GetProcessesByName("NOW");


                }
                catch
                {


                    Result.Text=" Please start Now as Run as Administrator";
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
                    Result.Text = "Nest is not running or Market Watch not present";
                    return;
                }
                sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);
            }
            catch
            {
                 Result.Text ="Market Watch not found ";

                return;
            }



            f = sao.Children[3];



            //////////////////////////////
            //checking nest fileds 

            finalobject = f.Children[0];
            string s1 = finalobject.Description;

            string[] checkterminalcol = s1.Split(',');
            try
            {
                
                    s1 = f.Children[0].Description;
                   
                    int flag1 = 0;
                    checkterminalcol = s1.Split(',');
                    try
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            if (checkterminalcol[j].Contains("Exchange"))
                            {
                                Result.Text = "Done";
                                flag1 = 1;
                            }

                        }
                    }
                    catch
                    {
                    }
                if(flag1==0)
                {
                    Result.Text = "Please add Exchange Column in terminal";

                }
                   
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message );
            }




        }
        private void Finish_Click(object sender, EventArgs e)
        {
       
            checkexchange();




            //if all done close wizard 
            if (Result.Text == "Done")
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"windows-data\");
                regKey.SetValue("Wizard", "done");
                if (nest.Checked == true)
                {
                    regKey.SetValue("terminal", "NEST");

                }
                else if(now.Checked==true )
                {
                    regKey.SetValue("terminal", "NOW");

                }
                var registerdate = regKey.GetValue("sd");
                var paidornot = regKey.GetValue("sp");
                this.Hide();

                string chktmp = ConfigurationManager.AppSettings["ApplicationId"];
                //if user is using first time 
               
                    //if user delete register entry then show login window agian 
                    if (registerdate != null && paidornot != null)
                    {
                        ShubhaPlugin f = new ShubhaPlugin();
                        f.ShowDialog();
                    }
                    else
                    {
                        Login f = new Login();
                        f.ShowDialog();
                    }

               
            }
            else
            {
                return;
            }
            
        }

        private void next_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            panel1.Visible = false;
            panel2.Visible = true ;

            next.Enabled = false;
            Back.Enabled = true;
            Finish.Enabled = true;

        }

        private void Back_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            panel1.Visible = true;
            panel2.Visible = false;

            next.Enabled = true;
            Back.Enabled = false;
            Finish.Enabled = false ;


        }

        private void Result_Click(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
