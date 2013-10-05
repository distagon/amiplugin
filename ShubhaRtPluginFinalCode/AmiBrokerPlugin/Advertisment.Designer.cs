namespace AmiBrokerPlugin
{
    partial class Advertisment
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
            this.cnt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(-4, -17);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(966, 500);
            this.webBrowser1.TabIndex = 3;
            this.webBrowser1.Url = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=22&amp;target=_blank&am" +
                    "p;cb=INSERT_RANDOM_NUMBER_HERE", System.UriKind.Absolute);
            // 
            // cnt
            // 
            this.cnt.Location = new System.Drawing.Point(846, 431);
            this.cnt.Name = "cnt";
            this.cnt.Size = new System.Drawing.Size(75, 23);
            this.cnt.TabIndex = 4;
            this.cnt.Text = "continue ";
            this.cnt.UseVisualStyleBackColor = true;
            this.cnt.Click += new System.EventHandler(this.cnt_Click);
            // 
            // Advertisment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(38)))), ((int)(((byte)(100)))));
            this.ClientSize = new System.Drawing.Size(929, 466);
            this.Controls.Add(this.cnt);
            this.Controls.Add(this.webBrowser1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Advertisment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShubhaRT plugin";
            this.Load += new System.EventHandler(this.Advertisment_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button cnt;


    }
}