namespace WindowsFormsApp2
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLine = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
            this.mapToolBarButton1 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButton2 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButton3 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButton4 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButton5 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButton6 = new MapInfo.Windows.Controls.MapToolBarButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ricPaths = new System.Windows.Forms.RichTextBox();
            this.comBusNum = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBus = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comOffice = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comTrafic = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comLake = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tsmAddline = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmEditLine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmEditPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRun = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapControl1
            // 
            this.mapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mapControl1.IgnoreLostFocusEvent = false;
            this.mapControl1.Location = new System.Drawing.Point(274, 32);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(655, 683);
            this.mapControl1.TabIndex = 0;
            this.mapControl1.Text = "mapControl1";
            this.mapControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseDown);
            this.mapControl1.Tools.LeftButtonTool = null;
            this.mapControl1.Tools.MiddleButtonTool = null;
            this.mapControl1.Tools.RightButtonTool = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3,
            this.tsmData});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(955, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuClear,
            this.mnuExit});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItem1.Text = "File";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(152, 22);
            this.mnuOpen.Text = "Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(152, 22);
            this.mnuClear.Text = "Clear Map";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(152, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(39, 20);
            this.toolStripMenuItem3.Text = "Tool";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuItem4.Text = "Find Path";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuItem5.Text = "Find Bus";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // tsmData
            // 
            this.tsmData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPoint,
            this.tsmLine});
            this.tsmData.Name = "tsmData";
            this.tsmData.Size = new System.Drawing.Size(42, 20);
            this.tsmData.Text = "Data";
            // 
            // tsmPoint
            // 
            this.tsmPoint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAddPoint,
            this.tsmEditPoint});
            this.tsmPoint.Name = "tsmPoint";
            this.tsmPoint.Size = new System.Drawing.Size(152, 22);
            this.tsmPoint.Text = "Point";
            this.tsmPoint.Click += new System.EventHandler(this.tsmPoint_Click);
            // 
            // tsmLine
            // 
            this.tsmLine.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAddline,
            this.tsmEditLine});
            this.tsmLine.Name = "tsmLine";
            this.tsmLine.Size = new System.Drawing.Size(152, 22);
            this.tsmLine.Text = "Line";
            this.tsmLine.Click += new System.EventHandler(this.tsmLine_Click);
            // 
            // mapToolBar1
            // 
            this.mapToolBar1.AutoSize = false;
            this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.mapToolBarButton1,
            this.mapToolBarButton2,
            this.mapToolBarButton3,
            this.mapToolBarButton4,
            this.mapToolBarButton5,
            this.mapToolBarButton6});
            this.mapToolBar1.CausesValidation = false;
            this.mapToolBar1.Divider = false;
            this.mapToolBar1.DropDownArrows = true;
            this.mapToolBar1.Location = new System.Drawing.Point(0, 24);
            this.mapToolBar1.MapControl = this.mapControl1;
            this.mapToolBar1.Name = "mapToolBar1";
            this.mapToolBar1.ShowToolTips = true;
            this.mapToolBar1.Size = new System.Drawing.Size(955, 28);
            this.mapToolBar1.TabIndex = 4;
            // 
            // mapToolBarButton1
            // 
            this.mapToolBarButton1.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
            this.mapToolBarButton1.Name = "mapToolBarButton1";
            this.mapToolBarButton1.ToolTipText = "Zoom-in";
            // 
            // mapToolBarButton2
            // 
            this.mapToolBarButton2.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
            this.mapToolBarButton2.Name = "mapToolBarButton2";
            this.mapToolBarButton2.ToolTipText = "Zoom-out";
            // 
            // mapToolBarButton3
            // 
            this.mapToolBarButton3.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
            this.mapToolBarButton3.Name = "mapToolBarButton3";
            this.mapToolBarButton3.ToolTipText = "Select";
            // 
            // mapToolBarButton4
            // 
            this.mapToolBarButton4.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
            this.mapToolBarButton4.Name = "mapToolBarButton4";
            this.mapToolBarButton4.ToolTipText = "Layer Control";
            // 
            // mapToolBarButton5
            // 
            this.mapToolBarButton5.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.AddLine;
            this.mapToolBarButton5.Name = "mapToolBarButton5";
            this.mapToolBarButton5.ToolTipText = "Add Line";
            // 
            // mapToolBarButton6
            // 
            this.mapToolBarButton6.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
            this.mapToolBarButton6.Name = "mapToolBarButton6";
            this.mapToolBarButton6.ToolTipText = "Pan";
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            // 
            // tabSearch
            // 
            this.tabSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabSearch.Controls.Add(this.btnRun);
            this.tabSearch.Controls.Add(this.comLake);
            this.tabSearch.Controls.Add(this.label1);
            this.tabSearch.Controls.Add(this.comTrafic);
            this.tabSearch.Controls.Add(this.label12);
            this.tabSearch.Controls.Add(this.comOffice);
            this.tabSearch.Controls.Add(this.label6);
            this.tabSearch.Controls.Add(this.btnSearch);
            this.tabSearch.Controls.Add(this.groupBox6);
            this.tabSearch.Controls.Add(this.groupBox5);
            this.tabSearch.Location = new System.Drawing.Point(4, 22);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(264, 631);
            this.tabSearch.TabIndex = 2;
            this.tabSearch.Text = "Search";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.comBusNum);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Location = new System.Drawing.Point(26, 113);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(223, 241);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Search with bus number";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Bus Number:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ricPaths);
            this.groupBox1.Location = new System.Drawing.Point(9, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 189);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paths:";
            // 
            // ricPaths
            // 
            this.ricPaths.Location = new System.Drawing.Point(7, 15);
            this.ricPaths.Name = "ricPaths";
            this.ricPaths.Size = new System.Drawing.Size(190, 168);
            this.ricPaths.TabIndex = 0;
            this.ricPaths.Text = "";
            // 
            // comBusNum
            // 
            this.comBusNum.FormattingEnabled = true;
            this.comBusNum.Location = new System.Drawing.Point(110, 24);
            this.comBusNum.Name = "comBusNum";
            this.comBusNum.Size = new System.Drawing.Size(111, 21);
            this.comBusNum.TabIndex = 18;
            this.comBusNum.SelectedIndexChanged += new System.EventHandler(this.comBusNum_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtStart);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.groupBox2);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.txtEnd);
            this.groupBox6.Location = new System.Drawing.Point(26, 360);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(223, 215);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Search with bus stop";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(87, 58);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(100, 20);
            this.txtEnd.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "End Point:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDistance);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtBus);
            this.groupBox2.Location = new System.Drawing.Point(12, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 111);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Buses in the short path";
            // 
            // txtBus
            // 
            this.txtBus.Location = new System.Drawing.Point(52, 26);
            this.txtBus.Name = "txtBus";
            this.txtBus.Size = new System.Drawing.Size(145, 53);
            this.txtBus.TabIndex = 0;
            this.txtBus.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Buses:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Distance:";
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(94, 85);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(100, 20);
            this.txtDistance.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Start Point:";
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(87, 19);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(100, 20);
            this.txtStart.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(174, 590);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Office:";
            // 
            // comOffice
            // 
            this.comOffice.FormattingEnabled = true;
            this.comOffice.Location = new System.Drawing.Point(73, 14);
            this.comOffice.Name = "comOffice";
            this.comOffice.Size = new System.Drawing.Size(174, 21);
            this.comOffice.TabIndex = 15;
            this.comOffice.SelectedIndexChanged += new System.EventHandler(this.comOffice_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Bus Trafic";
            // 
            // comTrafic
            // 
            this.comTrafic.FormattingEnabled = true;
            this.comTrafic.Location = new System.Drawing.Point(74, 42);
            this.comTrafic.Name = "comTrafic";
            this.comTrafic.Size = new System.Drawing.Size(174, 21);
            this.comTrafic.TabIndex = 17;
            this.comTrafic.SelectedIndexChanged += new System.EventHandler(this.comTrafic_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Lake";
            // 
            // comLake
            // 
            this.comLake.FormattingEnabled = true;
            this.comLake.Location = new System.Drawing.Point(75, 78);
            this.comLake.Name = "comLake";
            this.comLake.Size = new System.Drawing.Size(174, 21);
            this.comLake.TabIndex = 19;
            this.comLake.SelectedIndexChanged += new System.EventHandler(this.comLake_SelectedIndexChanged);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.tabSearch);
            this.tabControl.Location = new System.Drawing.Point(0, 58);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(272, 657);
            this.tabControl.TabIndex = 3;
            // 
            // tsmAddline
            // 
            this.tsmAddline.Name = "tsmAddline";
            this.tsmAddline.Size = new System.Drawing.Size(152, 22);
            this.tsmAddline.Text = "Add Line";
            this.tsmAddline.Click += new System.EventHandler(this.tsmAddline_Click);
            // 
            // tsmEditLine
            // 
            this.tsmEditLine.Name = "tsmEditLine";
            this.tsmEditLine.Size = new System.Drawing.Size(152, 22);
            this.tsmEditLine.Text = "Edit Line";
            this.tsmEditLine.Click += new System.EventHandler(this.tsmEditLine_Click);
            // 
            // tsmAddPoint
            // 
            this.tsmAddPoint.Name = "tsmAddPoint";
            this.tsmAddPoint.Size = new System.Drawing.Size(152, 22);
            this.tsmAddPoint.Text = "Add Point";
            this.tsmAddPoint.Click += new System.EventHandler(this.tsmAddPoint_Click);
            // 
            // tsmEditPoint
            // 
            this.tsmEditPoint.Name = "tsmEditPoint";
            this.tsmEditPoint.Size = new System.Drawing.Size(152, 22);
            this.tsmEditPoint.Text = "Edit Point";
            this.tsmEditPoint.Click += new System.EventHandler(this.tsmEditPoint_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(56, 590);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 20;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(955, 737);
            this.Controls.Add(this.mapToolBar1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.mapControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Quan ly xe Bus";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabSearch.ResumeLayout(false);
            this.tabSearch.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapInfo.Windows.Controls.MapControl mapControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton1;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton2;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton3;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton4;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButton6;
        private System.Windows.Forms.ToolStripMenuItem tsmData;
        private System.Windows.Forms.ToolStripMenuItem tsmPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmLine;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.ComboBox comLake;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comTrafic;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comOffice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtBus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comBusNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox ricPaths;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem tsmAddPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmEditPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmAddline;
        private System.Windows.Forms.ToolStripMenuItem tsmEditLine;
        private System.Windows.Forms.Button btnRun;
    }
}

