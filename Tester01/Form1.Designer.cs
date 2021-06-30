
namespace Tester01
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ScanofPorts = new System.Windows.Forms.Timer(this.components);
            this.VaccPort = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pbMain = new System.Windows.Forms.ToolStripProgressBar();
            this.tsLblMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSSID = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSsıdGet = new System.Windows.Forms.Button();
            this.btnSsıdSet = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScanofPorts
            // 
            this.ScanofPorts.Interval = 1000;
            this.ScanofPorts.Tick += new System.EventHandler(this.SearchVaccineRef);
            // 
            // VaccPort
            // 
            this.VaccPort.BaudRate = 115200;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbMain,
            this.tsLblMain});
            this.statusStrip1.Location = new System.Drawing.Point(812, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(205, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pbMain
            // 
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(100, 18);
            // 
            // tsLblMain
            // 
            this.tsLblMain.Name = "tsLblMain";
            this.tsLblMain.Size = new System.Drawing.Size(86, 20);
            this.tsLblMain.Text = "lMainStatus";
            // 
            // lblSSID
            // 
            this.lblSSID.AutoSize = true;
            this.lblSSID.Location = new System.Drawing.Point(25, 63);
            this.lblSSID.Name = "lblSSID";
            this.lblSSID.Size = new System.Drawing.Size(39, 17);
            this.lblSSID.TabIndex = 1;
            this.lblSSID.Text = "SSID";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 22);
            this.textBox1.TabIndex = 2;
            // 
            // btnSsıdGet
            // 
            this.btnSsıdGet.Location = new System.Drawing.Point(88, 120);
            this.btnSsıdGet.Name = "btnSsıdGet";
            this.btnSsıdGet.Size = new System.Drawing.Size(167, 23);
            this.btnSsıdGet.TabIndex = 3;
            this.btnSsıdGet.Text = "GET";
            this.btnSsıdGet.UseVisualStyleBackColor = true;
            // 
            // btnSsıdSet
            // 
            this.btnSsıdSet.Location = new System.Drawing.Point(88, 91);
            this.btnSsıdSet.Name = "btnSsıdSet";
            this.btnSsıdSet.Size = new System.Drawing.Size(167, 23);
            this.btnSsıdSet.TabIndex = 3;
            this.btnSsıdSet.Text = "SET";
            this.btnSsıdSet.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 562);
            this.Controls.Add(this.btnSsıdSet);
            this.Controls.Add(this.btnSsıdGet);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblSSID);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer ScanofPorts;
        private System.IO.Ports.SerialPort VaccPort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar pbMain;
        private System.Windows.Forms.ToolStripStatusLabel tsLblMain;
        private System.Windows.Forms.Label lblSSID;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSsıdGet;
        private System.Windows.Forms.Button btnSsıdSet;
    }
}

