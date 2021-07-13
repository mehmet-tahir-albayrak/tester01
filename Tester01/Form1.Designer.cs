
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
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSSID = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSsıdGet = new System.Windows.Forms.Button();
            this.btnSsıdSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.lblExTemp = new System.Windows.Forms.Label();
            this.btnStartDataSend = new System.Windows.Forms.Button();
            this.btnClearChart2 = new System.Windows.Forms.Button();
            this.lblLinePow = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDoorStat = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRefTemp = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblInt = new System.Windows.Forms.Label();
            this.lblInTemp = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.JsonDataPost = new System.Windows.Forms.Button();
            this.btnEpochSync = new System.Windows.Forms.Button();
            this.JsonPost = new System.Windows.Forms.Button();
            this.btnGetDataLogs = new System.Windows.Forms.Button();
            this.getpostjsonnetwork = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.DataPost = new System.Windows.Forms.TabPage();
            this.LogTextBox = new System.Windows.Forms.RichTextBox();
            this.dataLogTxtBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.tsLblMain,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(812, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(355, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
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
            this.tsLblMain.Click += new System.EventHandler(this.tsLblMain_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // lblSSID
            // 
            this.lblSSID.AutoSize = true;
            this.lblSSID.Location = new System.Drawing.Point(19, 32);
            this.lblSSID.Name = "lblSSID";
            this.lblSSID.Size = new System.Drawing.Size(39, 17);
            this.lblSSID.TabIndex = 1;
            this.lblSSID.Text = "SSID";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(81, 32);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 22);
            this.textBox1.TabIndex = 2;
            // 
            // btnSsıdGet
            // 
            this.btnSsıdGet.Location = new System.Drawing.Point(81, 89);
            this.btnSsıdGet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSsıdGet.Name = "btnSsıdGet";
            this.btnSsıdGet.Size = new System.Drawing.Size(167, 23);
            this.btnSsıdGet.TabIndex = 3;
            this.btnSsıdGet.Text = "GET";
            this.btnSsıdGet.UseVisualStyleBackColor = true;
            this.btnSsıdGet.Click += new System.EventHandler(this.btnSsıdGet_Click);
            // 
            // btnSsıdSet
            // 
            this.btnSsıdSet.Location = new System.Drawing.Point(81, 60);
            this.btnSsıdSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSsıdSet.Name = "btnSsıdSet";
            this.btnSsıdSet.Size = new System.Drawing.Size(167, 23);
            this.btnSsıdSet.TabIndex = 3;
            this.btnSsıdSet.Text = "SET";
            this.btnSsıdSet.UseVisualStyleBackColor = true;
            this.btnSsıdSet.Click += new System.EventHandler(this.btnSsıdSet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "PAS";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(81, 146);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(167, 22);
            this.textBox2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "UID";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(327, 38);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(167, 22);
            this.textBox3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "ID";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(81, 256);
            this.textBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(167, 22);
            this.textBox4.TabIndex = 2;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(264, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "LDR";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(327, 146);
            this.textBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(167, 22);
            this.textBox5.TabIndex = 2;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(81, 284);
            this.textBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(167, 22);
            this.textBox6.TabIndex = 2;
            this.textBox6.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "VERSİON";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(327, 76);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(167, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "set";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(327, 105);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(167, 23);
            this.button9.TabIndex = 5;
            this.button9.Text = "get";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(81, 175);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 204);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "get";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(327, 178);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(167, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(327, 204);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(167, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "get";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(81, 313);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(167, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "SET";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(81, 343);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(167, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "GET";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.DataPost);
            this.tabControl1.Location = new System.Drawing.Point(13, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(724, 647);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.lblExTemp);
            this.tabPage1.Controls.Add(this.btnStartDataSend);
            this.tabPage1.Controls.Add(this.btnClearChart2);
            this.tabPage1.Controls.Add(this.lblLinePow);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.lblDoorStat);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.lblRefTemp);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.lblInt);
            this.tabPage1.Controls.Add(this.lblInTemp);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.lblSSID);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.btnSsıdGet);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBox5);
            this.tabPage1.Controls.Add(this.btnSsıdSet);
            this.tabPage1.Controls.Add(this.textBox6);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.textBox4);
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(716, 618);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "config";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(977, 127);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 18);
            this.label6.TabIndex = 205;
            this.label6.Text = "Ext Temp1 (C)";
            // 
            // lblExTemp
            // 
            this.lblExTemp.AutoSize = true;
            this.lblExTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblExTemp.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblExTemp.Location = new System.Drawing.Point(1113, 127);
            this.lblExTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExTemp.Name = "lblExTemp";
            this.lblExTemp.Size = new System.Drawing.Size(52, 24);
            this.lblExTemp.TabIndex = 204;
            this.lblExTemp.Text = "------";
            // 
            // btnStartDataSend
            // 
            this.btnStartDataSend.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnStartDataSend.ForeColor = System.Drawing.Color.White;
            this.btnStartDataSend.Location = new System.Drawing.Point(519, 87);
            this.btnStartDataSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartDataSend.Name = "btnStartDataSend";
            this.btnStartDataSend.Size = new System.Drawing.Size(192, 26);
            this.btnStartDataSend.TabIndex = 203;
            this.btnStartDataSend.Text = "GET DATA LOGS";
            this.btnStartDataSend.UseVisualStyleBackColor = false;
            this.btnStartDataSend.Click += new System.EventHandler(this.btnStartDataSend_Click);
            // 
            // btnClearChart2
            // 
            this.btnClearChart2.Location = new System.Drawing.Point(1166, 446);
            this.btnClearChart2.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearChart2.Name = "btnClearChart2";
            this.btnClearChart2.Size = new System.Drawing.Size(39, 27);
            this.btnClearChart2.TabIndex = 202;
            this.btnClearChart2.Text = "C";
            this.btnClearChart2.UseVisualStyleBackColor = true;
            // 
            // lblLinePow
            // 
            this.lblLinePow.AutoSize = true;
            this.lblLinePow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLinePow.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblLinePow.Location = new System.Drawing.Point(870, 149);
            this.lblLinePow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLinePow.Name = "lblLinePow";
            this.lblLinePow.Size = new System.Drawing.Size(52, 24);
            this.lblLinePow.TabIndex = 201;
            this.lblLinePow.Text = "------";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(744, 152);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 18);
            this.label7.TabIndex = 200;
            this.label7.Text = "Line Pow";
            // 
            // lblDoorStat
            // 
            this.lblDoorStat.AutoSize = true;
            this.lblDoorStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDoorStat.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblDoorStat.Location = new System.Drawing.Point(638, 155);
            this.lblDoorStat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDoorStat.Name = "lblDoorStat";
            this.lblDoorStat.Size = new System.Drawing.Size(52, 24);
            this.lblDoorStat.TabIndex = 199;
            this.lblDoorStat.Text = "------";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(516, 155);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 18);
            this.label8.TabIndex = 198;
            this.label8.Text = "Door Stat";
            // 
            // lblRefTemp
            // 
            this.lblRefTemp.AutoSize = true;
            this.lblRefTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblRefTemp.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblRefTemp.Location = new System.Drawing.Point(638, 127);
            this.lblRefTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRefTemp.Name = "lblRefTemp";
            this.lblRefTemp.Size = new System.Drawing.Size(52, 24);
            this.lblRefTemp.TabIndex = 197;
            this.lblRefTemp.Text = "------";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(516, 127);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 18);
            this.label9.TabIndex = 196;
            this.label9.Text = "Ref Temp (C)";
            // 
            // lblInt
            // 
            this.lblInt.AutoSize = true;
            this.lblInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblInt.ForeColor = System.Drawing.Color.Black;
            this.lblInt.Location = new System.Drawing.Point(744, 124);
            this.lblInt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInt.Name = "lblInt";
            this.lblInt.Size = new System.Drawing.Size(94, 18);
            this.lblInt.TabIndex = 195;
            this.lblInt.Text = "In Temp1 (C)";
            // 
            // lblInTemp
            // 
            this.lblInTemp.AutoSize = true;
            this.lblInTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblInTemp.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblInTemp.Location = new System.Drawing.Point(870, 124);
            this.lblInTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInTemp.Name = "lblInTemp";
            this.lblInTemp.Size = new System.Drawing.Size(52, 24);
            this.lblInTemp.TabIndex = 194;
            this.lblInTemp.Text = "------";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.JsonDataPost);
            this.tabPage2.Controls.Add(this.btnEpochSync);
            this.tabPage2.Controls.Add(this.JsonPost);
            this.tabPage2.Controls.Add(this.btnGetDataLogs);
            this.tabPage2.Controls.Add(this.getpostjsonnetwork);
            this.tabPage2.Controls.Add(this.button8);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(716, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // JsonDataPost
            // 
            this.JsonDataPost.Location = new System.Drawing.Point(6, 136);
            this.JsonDataPost.Name = "JsonDataPost";
            this.JsonDataPost.Size = new System.Drawing.Size(228, 39);
            this.JsonDataPost.TabIndex = 231;
            this.JsonDataPost.Text = "DataLogPost";
            this.JsonDataPost.UseVisualStyleBackColor = true;
            this.JsonDataPost.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // btnEpochSync
            // 
            this.btnEpochSync.BackColor = System.Drawing.Color.MintCream;
            this.btnEpochSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEpochSync.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEpochSync.Location = new System.Drawing.Point(263, 50);
            this.btnEpochSync.Margin = new System.Windows.Forms.Padding(4);
            this.btnEpochSync.Name = "btnEpochSync";
            this.btnEpochSync.Size = new System.Drawing.Size(229, 31);
            this.btnEpochSync.TabIndex = 115;
            this.btnEpochSync.Text = "EPOCH SYNC";
            this.btnEpochSync.UseVisualStyleBackColor = false;
            this.btnEpochSync.Click += new System.EventHandler(this.btnEpochSync_Click);
            // 
            // JsonPost
            // 
            this.JsonPost.Location = new System.Drawing.Point(6, 95);
            this.JsonPost.Name = "JsonPost";
            this.JsonPost.Size = new System.Drawing.Size(228, 35);
            this.JsonPost.TabIndex = 230;
            this.JsonPost.Text = "LogPost";
            this.JsonPost.UseVisualStyleBackColor = true;
            this.JsonPost.Click += new System.EventHandler(this.JsonPost_Click);
            // 
            // btnGetDataLogs
            // 
            this.btnGetDataLogs.Location = new System.Drawing.Point(6, 50);
            this.btnGetDataLogs.Name = "btnGetDataLogs";
            this.btnGetDataLogs.Size = new System.Drawing.Size(228, 39);
            this.btnGetDataLogs.TabIndex = 1;
            this.btnGetDataLogs.Text = "Get Data Logs";
            this.btnGetDataLogs.UseVisualStyleBackColor = true;
            this.btnGetDataLogs.Click += new System.EventHandler(this.btnGetDataLogs_Click);
            // 
            // getpostjsonnetwork
            // 
            this.getpostjsonnetwork.BackColor = System.Drawing.Color.LightCyan;
            this.getpostjsonnetwork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.getpostjsonnetwork.ForeColor = System.Drawing.SystemColors.Desktop;
            this.getpostjsonnetwork.Location = new System.Drawing.Point(263, 15);
            this.getpostjsonnetwork.Margin = new System.Windows.Forms.Padding(4);
            this.getpostjsonnetwork.Name = "getpostjsonnetwork";
            this.getpostjsonnetwork.Size = new System.Drawing.Size(229, 27);
            this.getpostjsonnetwork.TabIndex = 227;
            this.getpostjsonnetwork.Text = "getpostjsonnetwork";
            this.getpostjsonnetwork.UseVisualStyleBackColor = false;
            this.getpostjsonnetwork.Click += new System.EventHandler(this.btnGSMSend4_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.MintCream;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button8.Location = new System.Drawing.Point(5, 14);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(229, 31);
            this.button8.TabIndex = 196;
            this.button8.Text = "GET LOGS";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(716, 618);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox2.Location = new System.Drawing.Point(25, 6);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(551, 536);
            this.richTextBox2.TabIndex = 102;
            this.richTextBox2.Text = "";
            // 
            // DataPost
            // 
            this.DataPost.Location = new System.Drawing.Point(4, 25);
            this.DataPost.Margin = new System.Windows.Forms.Padding(4);
            this.DataPost.Name = "DataPost";
            this.DataPost.Size = new System.Drawing.Size(716, 618);
            this.DataPost.TabIndex = 3;
            this.DataPost.Text = "DataPost";
            this.DataPost.UseVisualStyleBackColor = true;
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(1032, 27);
            this.LogTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(298, 322);
            this.LogTextBox.TabIndex = 194;
            this.LogTextBox.Text = "";
            // 
            // dataLogTxtBox
            // 
            this.dataLogTxtBox.Location = new System.Drawing.Point(739, 27);
            this.dataLogTxtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataLogTxtBox.Name = "dataLogTxtBox";
            this.dataLogTxtBox.Size = new System.Drawing.Size(287, 322);
            this.dataLogTxtBox.TabIndex = 195;
            this.dataLogTxtBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1597, 562);
            this.Controls.Add(this.dataLogTxtBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LogTextBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button getpostjsonnetwork;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button btnEpochSync;
        private System.Windows.Forms.TabPage DataPost;
        private System.Windows.Forms.Button btnGetDataLogs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblExTemp;
        private System.Windows.Forms.Button btnStartDataSend;
        private System.Windows.Forms.Button btnClearChart2;
        private System.Windows.Forms.Label lblLinePow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDoorStat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRefTemp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblInt;
        private System.Windows.Forms.Label lblInTemp;
        private System.Windows.Forms.Button JsonPost;
        private System.Windows.Forms.Button JsonDataPost;
        private System.Windows.Forms.RichTextBox LogTextBox;
        private System.Windows.Forms.RichTextBox dataLogTxtBox;
    }
}

