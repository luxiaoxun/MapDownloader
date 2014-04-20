namespace GMapWinFormDemo
{
    partial class MapForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTools = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonBeginBlink = new System.Windows.Forms.Button();
            this.buttonStopBlink = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSearchResult = new System.Windows.Forms.ComboBox();
            this.buttonSetStart = new System.Windows.Forms.Button();
            this.buttonFindRoute = new System.Windows.Forms.Button();
            this.buttonSetEnd = new System.Windows.Forms.Button();
            this.panelMap = new System.Windows.Forms.Panel();
            this.comboBoxRegion = new System.Windows.Forms.ComboBox();
            this.buttonMapType = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.地图操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.谷歌地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高德地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.腾讯地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.百度地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图操作ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.保存为图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapControl = new GMapWinFormDemo.MapControl();
            this.contextMenuStrip1.SuspendLayout();
            this.panelTools.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelMap.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // panelTools
            // 
            this.panelTools.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelTools.Controls.Add(this.groupBox3);
            this.panelTools.Controls.Add(this.groupBox2);
            this.panelTools.Controls.Add(this.groupBox1);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelTools.Location = new System.Drawing.Point(592, 0);
            this.panelTools.Name = "panelTools";
            this.panelTools.Size = new System.Drawing.Size(158, 517);
            this.panelTools.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(8, 259);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 94);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Drawing";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(74, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(67, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Polygon";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(73, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Rectangle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Circle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonBeginBlink);
            this.groupBox2.Controls.Add(this.buttonStopBlink);
            this.groupBox2.Location = new System.Drawing.Point(6, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 79);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Marker";
            // 
            // buttonBeginBlink
            // 
            this.buttonBeginBlink.Location = new System.Drawing.Point(6, 20);
            this.buttonBeginBlink.Name = "buttonBeginBlink";
            this.buttonBeginBlink.Size = new System.Drawing.Size(87, 23);
            this.buttonBeginBlink.TabIndex = 20;
            this.buttonBeginBlink.Text = "Marker Blink";
            this.buttonBeginBlink.UseVisualStyleBackColor = true;
            this.buttonBeginBlink.Click += new System.EventHandler(this.buttonBeginBlink_Click);
            // 
            // buttonStopBlink
            // 
            this.buttonStopBlink.Location = new System.Drawing.Point(6, 49);
            this.buttonStopBlink.Name = "buttonStopBlink";
            this.buttonStopBlink.Size = new System.Drawing.Size(87, 23);
            this.buttonStopBlink.TabIndex = 21;
            this.buttonStopBlink.Text = "Stop Blink";
            this.buttonStopBlink.UseVisualStyleBackColor = true;
            this.buttonStopBlink.Click += new System.EventHandler(this.buttonStopBlink_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSearch);
            this.groupBox1.Controls.Add(this.buttonSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxSearchResult);
            this.groupBox1.Controls.Add(this.buttonSetStart);
            this.groupBox1.Controls.Add(this.buttonFindRoute);
            this.groupBox1.Controls.Add(this.buttonSetEnd);
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 151);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Route";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(6, 20);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(85, 21);
            this.textBoxSearch.TabIndex = 12;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(97, 18);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(52, 23);
            this.buttonSearch.TabIndex = 13;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "Search Result:";
            // 
            // comboBoxSearchResult
            // 
            this.comboBoxSearchResult.FormattingEnabled = true;
            this.comboBoxSearchResult.Location = new System.Drawing.Point(5, 63);
            this.comboBoxSearchResult.Name = "comboBoxSearchResult";
            this.comboBoxSearchResult.Size = new System.Drawing.Size(127, 20);
            this.comboBoxSearchResult.TabIndex = 14;
            this.comboBoxSearchResult.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchResult_SelectedIndexChanged);
            // 
            // buttonSetStart
            // 
            this.buttonSetStart.Location = new System.Drawing.Point(3, 90);
            this.buttonSetStart.Name = "buttonSetStart";
            this.buttonSetStart.Size = new System.Drawing.Size(68, 23);
            this.buttonSetStart.TabIndex = 15;
            this.buttonSetStart.Text = "Set Start";
            this.buttonSetStart.UseVisualStyleBackColor = true;
            this.buttonSetStart.Click += new System.EventHandler(this.buttonSetStart_Click);
            // 
            // buttonFindRoute
            // 
            this.buttonFindRoute.Location = new System.Drawing.Point(3, 119);
            this.buttonFindRoute.Name = "buttonFindRoute";
            this.buttonFindRoute.Size = new System.Drawing.Size(75, 23);
            this.buttonFindRoute.TabIndex = 18;
            this.buttonFindRoute.Text = "Find Route";
            this.buttonFindRoute.UseVisualStyleBackColor = true;
            this.buttonFindRoute.Click += new System.EventHandler(this.buttonFindRoute_Click);
            // 
            // buttonSetEnd
            // 
            this.buttonSetEnd.Location = new System.Drawing.Point(78, 90);
            this.buttonSetEnd.Name = "buttonSetEnd";
            this.buttonSetEnd.Size = new System.Drawing.Size(68, 23);
            this.buttonSetEnd.TabIndex = 16;
            this.buttonSetEnd.Text = "Set End";
            this.buttonSetEnd.UseVisualStyleBackColor = true;
            this.buttonSetEnd.Click += new System.EventHandler(this.buttonSetEnd_Click);
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.comboBoxRegion);
            this.panelMap.Controls.Add(this.buttonMapType);
            this.panelMap.Controls.Add(this.mapControl);
            this.panelMap.Controls.Add(this.menuStrip1);
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(592, 517);
            this.panelMap.TabIndex = 15;
            // 
            // comboBoxRegion
            // 
            this.comboBoxRegion.FormattingEnabled = true;
            this.comboBoxRegion.Location = new System.Drawing.Point(516, 1);
            this.comboBoxRegion.Name = "comboBoxRegion";
            this.comboBoxRegion.Size = new System.Drawing.Size(65, 20);
            this.comboBoxRegion.TabIndex = 4;
            // 
            // buttonMapType
            // 
            this.buttonMapType.Location = new System.Drawing.Point(521, 32);
            this.buttonMapType.Name = "buttonMapType";
            this.buttonMapType.Size = new System.Drawing.Size(50, 49);
            this.buttonMapType.TabIndex = 3;
            this.buttonMapType.UseVisualStyleBackColor = true;
            this.buttonMapType.Click += new System.EventHandler(this.buttonMapType_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.地图操作ToolStripMenuItem,
            this.地图操作ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(592, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 地图操作ToolStripMenuItem
            // 
            this.地图操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.谷歌地图ToolStripMenuItem,
            this.高德地图ToolStripMenuItem,
            this.腾讯地图ToolStripMenuItem,
            this.百度地图ToolStripMenuItem});
            this.地图操作ToolStripMenuItem.Name = "地图操作ToolStripMenuItem";
            this.地图操作ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.地图操作ToolStripMenuItem.Text = "地图选择";
            // 
            // 谷歌地图ToolStripMenuItem
            // 
            this.谷歌地图ToolStripMenuItem.Name = "谷歌地图ToolStripMenuItem";
            this.谷歌地图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.谷歌地图ToolStripMenuItem.Text = "谷歌地图";
            this.谷歌地图ToolStripMenuItem.Click += new System.EventHandler(this.谷歌地图ToolStripMenuItem_Click);
            // 
            // 高德地图ToolStripMenuItem
            // 
            this.高德地图ToolStripMenuItem.Name = "高德地图ToolStripMenuItem";
            this.高德地图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.高德地图ToolStripMenuItem.Text = "高德地图";
            this.高德地图ToolStripMenuItem.Click += new System.EventHandler(this.高德地图ToolStripMenuItem_Click);
            // 
            // 腾讯地图ToolStripMenuItem
            // 
            this.腾讯地图ToolStripMenuItem.Name = "腾讯地图ToolStripMenuItem";
            this.腾讯地图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.腾讯地图ToolStripMenuItem.Text = "腾讯地图";
            this.腾讯地图ToolStripMenuItem.Click += new System.EventHandler(this.腾讯地图ToolStripMenuItem_Click);
            // 
            // 百度地图ToolStripMenuItem
            // 
            this.百度地图ToolStripMenuItem.Name = "百度地图ToolStripMenuItem";
            this.百度地图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.百度地图ToolStripMenuItem.Text = "百度地图";
            this.百度地图ToolStripMenuItem.Click += new System.EventHandler(this.百度地图ToolStripMenuItem_Click);
            // 
            // 地图操作ToolStripMenuItem1
            // 
            this.地图操作ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存为图片ToolStripMenuItem,
            this.保存缓存ToolStripMenuItem,
            this.读取缓存ToolStripMenuItem});
            this.地图操作ToolStripMenuItem1.Name = "地图操作ToolStripMenuItem1";
            this.地图操作ToolStripMenuItem1.Size = new System.Drawing.Size(67, 20);
            this.地图操作ToolStripMenuItem1.Text = "地图操作";
            // 
            // 保存为图片ToolStripMenuItem
            // 
            this.保存为图片ToolStripMenuItem.Name = "保存为图片ToolStripMenuItem";
            this.保存为图片ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.保存为图片ToolStripMenuItem.Text = "保存地图图片";
            this.保存为图片ToolStripMenuItem.Click += new System.EventHandler(this.保存为图片ToolStripMenuItem_Click);
            // 
            // 保存缓存ToolStripMenuItem
            // 
            this.保存缓存ToolStripMenuItem.Name = "保存缓存ToolStripMenuItem";
            this.保存缓存ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.保存缓存ToolStripMenuItem.Text = "保存缓存";
            this.保存缓存ToolStripMenuItem.Click += new System.EventHandler(this.保存缓存ToolStripMenuItem_Click);
            // 
            // 读取缓存ToolStripMenuItem
            // 
            this.读取缓存ToolStripMenuItem.Name = "读取缓存ToolStripMenuItem";
            this.读取缓存ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.读取缓存ToolStripMenuItem.Text = "读取缓存";
            this.读取缓存ToolStripMenuItem.Click += new System.EventHandler(this.读取缓存ToolStripMenuItem_Click);
            // 
            // mapControl
            // 
            this.mapControl.Bearing = 0F;
            this.mapControl.CanDragMap = true;
            this.mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl.EmptyTileColor = System.Drawing.Color.Navy;
            this.mapControl.GrayScaleMode = false;
            this.mapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.mapControl.LevelsKeepInMemmory = 5;
            this.mapControl.Location = new System.Drawing.Point(0, 24);
            this.mapControl.MarkersEnabled = true;
            this.mapControl.MaxZoom = 2;
            this.mapControl.MinZoom = 2;
            this.mapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.mapControl.Name = "mapControl";
            this.mapControl.NegativeMode = false;
            this.mapControl.PolygonsEnabled = true;
            this.mapControl.RetryLoadTile = 0;
            this.mapControl.RoutesEnabled = true;
            this.mapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.mapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.mapControl.ShowTileGridLines = false;
            this.mapControl.Size = new System.Drawing.Size(592, 493);
            this.mapControl.TabIndex = 1;
            this.mapControl.Zoom = 0D;
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 517);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelTools);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapForm";
            this.Text = "Map Form";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelTools.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelMap.ResumeLayout(false);
            this.panelMap.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.Panel panelTools;
        private System.Windows.Forms.Button buttonStopBlink;
        private System.Windows.Forms.Button buttonBeginBlink;
        private System.Windows.Forms.Button buttonFindRoute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSetEnd;
        private System.Windows.Forms.Button buttonSetStart;
        private System.Windows.Forms.ComboBox comboBoxSearchResult;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Panel panelMap;
        private MapControl mapControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 地图操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 谷歌地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高德地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 腾讯地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 百度地图ToolStripMenuItem;
        private System.Windows.Forms.Button buttonMapType;
        private System.Windows.Forms.ToolStripMenuItem 地图操作ToolStripMenuItem1;
        private System.Windows.Forms.ComboBox comboBoxRegion;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStripMenuItem 保存为图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存缓存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取缓存ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

