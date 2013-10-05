namespace AmiBrokerPlugin
{
    partial class Righclick
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.StopRt = new System.Windows.Forms.Button();
            this.StartRt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(2, -1);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(954, 500);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.Url = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=22&amp;target=_blank&am" +
                    "p;cb=INSERT_RANDOM_NUMBER_HERE", System.UriKind.Absolute);
            // 
            // webBrowser2
            // 
            this.webBrowser2.Location = new System.Drawing.Point(62, 425);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(44, 29);
            this.webBrowser2.TabIndex = 12;
            this.webBrowser2.Url = new System.Uri("http://list.shubhalabha.in/amibrokerwindows-data.html", System.UriKind.Absolute);
            this.webBrowser2.Visible = false;
            // 
            // StopRt
            // 
            this.StopRt.Location = new System.Drawing.Point(842, 447);
            this.StopRt.Name = "StopRt";
            this.StopRt.Size = new System.Drawing.Size(75, 23);
            this.StopRt.TabIndex = 14;
            this.StopRt.Text = "Disconnect ";
            this.StopRt.UseVisualStyleBackColor = true;
            this.StopRt.Click += new System.EventHandler(this.StopRt_Click);
            // 
            // StartRt
            // 
            this.StartRt.Location = new System.Drawing.Point(719, 447);
            this.StartRt.Name = "StartRt";
            this.StartRt.Size = new System.Drawing.Size(75, 23);
            this.StartRt.TabIndex = 13;
            this.StartRt.Text = "Connect ";
            this.StartRt.UseVisualStyleBackColor = true;
            this.StartRt.Click += new System.EventHandler(this.StartRt_Click);
            // 
            // Righclick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(38)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(925, 482);
            this.Controls.Add(this.StopRt);
            this.Controls.Add(this.StartRt);
            this.Controls.Add(this.webBrowser2);
            this.Controls.Add(this.webBrowser1);
            this.Location = new System.Drawing.Point(10000, 500);
            this.Name = "Righclick";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Connection ";
            this.Load += new System.EventHandler(this.Righclick_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.WebBrowser webBrowser2;
        private System.Windows.Forms.Button StopRt;
        private System.Windows.Forms.Button StartRt;
    }
}