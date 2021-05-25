namespace WindowsFormsApp2
{
    partial class mapControl
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
            this.button1 = new MapInfo.Windows.Controls.Button();
            this.MapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
            this.MapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
            this.ToolBarButtonLayerControl = new System.Windows.Forms.ToolBarButton();
            this.ToolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
            this.MapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
            this.MapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
            this.MapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
            this.MapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.thêmĐiểmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comOffice = new System.Windows.Forms.ComboBox();
            this.comBusNum = new System.Windows.Forms.ComboBox();
            this.comTrafic = new System.Windows.Forms.ComboBox();
            this.comLake = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapControl1
            // 
            this.mapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapControl1.IgnoreLostFocusEvent = false;
            this.mapControl1.Location = new System.Drawing.Point(93, 58);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(684, 380);
            this.mapControl1.TabIndex = 0;
            this.mapControl1.Text = "mapControl1";
            this.mapControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseDown);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(12, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MapToolBar1
            // 
            this.MapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.MapToolBarButtonOpenTable,
            this.ToolBarButtonLayerControl,
            this.ToolBarButtonSeparator,
            this.MapToolBarButtonSelect,
            this.MapToolBarButtonZoomIn,
            this.MapToolBarButtonZoomOut,
            this.MapToolBarButtonPan});
            this.MapToolBar1.Divider = false;
            this.MapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.MapToolBar1.DropDownArrows = true;
            this.MapToolBar1.Location = new System.Drawing.Point(12, 3);
            this.MapToolBar1.MapControl = this.mapControl1;
            this.MapToolBar1.Name = "MapToolBar1";
            this.MapToolBar1.ShowToolTips = true;
            this.MapToolBar1.Size = new System.Drawing.Size(200, 26);
            this.MapToolBar1.TabIndex = 8;
            // 
            // MapToolBarButtonOpenTable
            // 
            this.MapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
            this.MapToolBarButtonOpenTable.Name = "MapToolBarButtonOpenTable";
            this.MapToolBarButtonOpenTable.ToolTipText = "Open Table";
            // 
            // ToolBarButtonLayerControl
            // 
            this.ToolBarButtonLayerControl.ImageIndex = 10;
            this.ToolBarButtonLayerControl.Name = "ToolBarButtonLayerControl";
            this.ToolBarButtonLayerControl.Tag = "";
            this.ToolBarButtonLayerControl.ToolTipText = "Layer Control";
            // 
            // ToolBarButtonSeparator
            // 
            this.ToolBarButtonSeparator.Name = "ToolBarButtonSeparator";
            this.ToolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // MapToolBarButtonSelect
            // 
            this.MapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
            this.MapToolBarButtonSelect.Name = "MapToolBarButtonSelect";
            this.MapToolBarButtonSelect.ToolTipText = "Select";
            // 
            // MapToolBarButtonZoomIn
            // 
            this.MapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
            this.MapToolBarButtonZoomIn.Name = "MapToolBarButtonZoomIn";
            this.MapToolBarButtonZoomIn.ToolTipText = "Zoom-in";
            // 
            // MapToolBarButtonZoomOut
            // 
            this.MapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
            this.MapToolBarButtonZoomOut.Name = "MapToolBarButtonZoomOut";
            this.MapToolBarButtonZoomOut.ToolTipText = "Zoom-out";
            // 
            // MapToolBarButtonPan
            // 
            this.MapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
            this.MapToolBarButtonPan.Name = "MapToolBarButtonPan";
            this.MapToolBarButtonPan.ToolTipText = "Pan";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thêmĐiểmToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 26);
            // 
            // thêmĐiểmToolStripMenuItem
            // 
            this.thêmĐiểmToolStripMenuItem.Name = "thêmĐiểmToolStripMenuItem";
            this.thêmĐiểmToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.thêmĐiểmToolStripMenuItem.Text = "Thêm điểm";
            this.thêmĐiểmToolStripMenuItem.Click += new System.EventHandler(this.thêmĐiểmToolStripMenuItem_Click);
            // 
            // comOffice
            // 
            this.comOffice.FormattingEnabled = true;
            this.comOffice.Location = new System.Drawing.Point(12, 116);
            this.comOffice.Name = "comOffice";
            this.comOffice.Size = new System.Drawing.Size(75, 21);
            this.comOffice.TabIndex = 11;
            // 
            // comBusNum
            // 
            this.comBusNum.FormattingEnabled = true;
            this.comBusNum.Location = new System.Drawing.Point(12, 154);
            this.comBusNum.Name = "comBusNum";
            this.comBusNum.Size = new System.Drawing.Size(75, 21);
            this.comBusNum.TabIndex = 12;
            // 
            // comTrafic
            // 
            this.comTrafic.FormattingEnabled = true;
            this.comTrafic.Location = new System.Drawing.Point(12, 195);
            this.comTrafic.Name = "comTrafic";
            this.comTrafic.Size = new System.Drawing.Size(75, 21);
            this.comTrafic.TabIndex = 13;
            // 
            // comLake
            // 
            this.comLake.FormattingEnabled = true;
            this.comLake.Location = new System.Drawing.Point(12, 234);
            this.comLake.Name = "comLake";
            this.comLake.Size = new System.Drawing.Size(75, 21);
            this.comLake.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comLake);
            this.Controls.Add(this.comTrafic);
            this.Controls.Add(this.comBusNum);
            this.Controls.Add(this.comOffice);
            this.Controls.Add(this.MapToolBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mapControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapInfo.Windows.Controls.MapControl mapControl1;
        private MapInfo.Windows.Controls.Button button1;
        internal MapInfo.Windows.Controls.MapToolBar MapToolBar1;
        internal MapInfo.Windows.Controls.MapToolBarButton MapToolBarButtonOpenTable;
        internal System.Windows.Forms.ToolBarButton ToolBarButtonLayerControl;
        internal System.Windows.Forms.ToolBarButton ToolBarButtonSeparator;
        internal MapInfo.Windows.Controls.MapToolBarButton MapToolBarButtonSelect;
        internal MapInfo.Windows.Controls.MapToolBarButton MapToolBarButtonZoomIn;
        internal MapInfo.Windows.Controls.MapToolBarButton MapToolBarButtonZoomOut;
        internal MapInfo.Windows.Controls.MapToolBarButton MapToolBarButtonPan;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem thêmĐiểmToolStripMenuItem;
        private System.Windows.Forms.ComboBox comOffice;
        private System.Windows.Forms.ComboBox comBusNum;
        private System.Windows.Forms.ComboBox comTrafic;
        private System.Windows.Forms.ComboBox comLake;
    }
}

