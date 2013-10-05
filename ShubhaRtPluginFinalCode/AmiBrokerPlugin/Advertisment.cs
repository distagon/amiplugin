/*This File close Advertisment after 3 sec 
 * 
 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AmiBrokerPlugin
{
    public partial class Advertisment : Form
    {
        public Advertisment()
        {
            InitializeComponent();
        }

        private void cnt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Advertisment_Load(object sender, EventArgs e)
        {

        }
      

    }
}
