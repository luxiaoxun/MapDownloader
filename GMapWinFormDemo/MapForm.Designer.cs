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
            this.contextMenuStripMarker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.谷歌地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高德地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.腾讯地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.百度地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地图操作ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.保存为图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.panelMap = new System.Windows.Forms.Panel();
            this.buttonMapType = new System.Windows.Forms.Button();
            this.mapControl = new GMapWinFormDemo.MapControl();
            this.panelDock = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phoneNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historyGeoDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelButtonTools = new System.Windows.Forms.Panel();
            this.checkBoxFollow = new System.Windows.Forms.CheckBox();
            this.comboBoxTimeSpan = new System.Windows.Forms.ComboBox();
            this.buttonSetTimerInterval = new System.Windows.Forms.Button();
            this.buttonResume = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxRegion = new System.Windows.Forms.ComboBox();
            this.splitter1 = new BSE.Windows.Forms.Splitter();
            this.panelMenu = new BSE.Windows.Forms.Panel();
            this.xPanderPanelList1 = new BSE.Windows.Forms.XPanderPanelList();
            this.xPanderPanelRoute = new BSE.Windows.Forms.XPanderPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonHisTestData = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSearchResult = new System.Windows.Forms.ComboBox();
            this.buttonSetStart = new System.Windows.Forms.Button();
            this.buttonFindRoute = new System.Windows.Forms.Button();
            this.buttonSetEnd = new System.Windows.Forms.Button();
            this.xPanderPanelMarker = new BSE.Windows.Forms.XPanderPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbGMapMarkerScopeCircleAnimate = new System.Windows.Forms.RadioButton();
            this.rbGMapMarkerScopePieAnimate = new System.Windows.Forms.RadioButton();
            this.rbGMapTipMarker = new System.Windows.Forms.RadioButton();
            this.rbGMapDirectionMarker = new System.Windows.Forms.RadioButton();
            this.rbGMapGifMarker = new System.Windows.Forms.RadioButton();
            this.rbGMapFlashMarker = new System.Windows.Forms.RadioButton();
            this.rbGMarkerGoogle = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonStopBlink = new System.Windows.Forms.Button();
            this.buttonBeginBlink = new System.Windows.Forms.Button();
            this.checkBoxMarker = new System.Windows.Forms.CheckBox();
            this.xPanderPanelDraw = new BSE.Windows.Forms.XPanderPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonDistance = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLine = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonPolyline = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonRectangle = new System.Windows.Forms.Button();
            this.buttonPolygon = new System.Windows.Forms.Button();
            this.xPanderPanelChinaRegion = new BSE.Windows.Forms.XPanderPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.xPanderPanelMap = new BSE.Windows.Forms.XPanderPanel();
            this.buttonClearSArea = new System.Windows.Forms.Button();
            this.buttonPrefetchSArea = new System.Windows.Forms.Button();
            this.contextMenuStripMarker.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panelMap.SuspendLayout();
            this.panelDock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.historyGeoDataBindingSource)).BeginInit();
            this.panelButtonTools.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.xPanderPanelList1.SuspendLayout();
            this.xPanderPanelRoute.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.xPanderPanelMarker.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.xPanderPanelDraw.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.xPanderPanelChinaRegion.SuspendLayout();
            this.xPanderPanelMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripMarker
            // 
            this.contextMenuStripMarker.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMarker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStripMarker.Name = "contextMenuStrip1";
            this.contextMenuStripMarker.Size = new System.Drawing.Size(115, 30);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 地图操作ToolStripMenuItem
            // 
            this.地图操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.谷歌地图ToolStripMenuItem,
            this.高德地图ToolStripMenuItem,
            this.腾讯地图ToolStripMenuItem,
            this.百度地图ToolStripMenuItem});
            this.地图操作ToolStripMenuItem.Name = "地图操作ToolStripMenuItem";
            this.地图操作ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.地图操作ToolStripMenuItem.Text = "地图选择";
            // 
            // 谷歌地图ToolStripMenuItem
            // 
            this.谷歌地图ToolStripMenuItem.Name = "谷歌地图ToolStripMenuItem";
            this.谷歌地图ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.谷歌地图ToolStripMenuItem.Text = "谷歌地图";
            this.谷歌地图ToolStripMenuItem.Click += new System.EventHandler(this.谷歌地图ToolStripMenuItem_Click);
            // 
            // 高德地图ToolStripMenuItem
            // 
            this.高德地图ToolStripMenuItem.Name = "高德地图ToolStripMenuItem";
            this.高德地图ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.高德地图ToolStripMenuItem.Text = "高德地图";
            this.高德地图ToolStripMenuItem.Click += new System.EventHandler(this.高德地图ToolStripMenuItem_Click);
            // 
            // 腾讯地图ToolStripMenuItem
            // 
            this.腾讯地图ToolStripMenuItem.Name = "腾讯地图ToolStripMenuItem";
            this.腾讯地图ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.腾讯地图ToolStripMenuItem.Text = "腾讯地图";
            this.腾讯地图ToolStripMenuItem.Click += new System.EventHandler(this.腾讯地图ToolStripMenuItem_Click);
            // 
            // 百度地图ToolStripMenuItem
            // 
            this.百度地图ToolStripMenuItem.Name = "百度地图ToolStripMenuItem";
            this.百度地图ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
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
            this.地图操作ToolStripMenuItem1.Size = new System.Drawing.Size(81, 24);
            this.地图操作ToolStripMenuItem1.Text = "地图操作";
            // 
            // 保存为图片ToolStripMenuItem
            // 
            this.保存为图片ToolStripMenuItem.Name = "保存为图片ToolStripMenuItem";
            this.保存为图片ToolStripMenuItem.Size = new System.Drawing.Size(174, 26);
            this.保存为图片ToolStripMenuItem.Text = "保存地图图片";
            this.保存为图片ToolStripMenuItem.Click += new System.EventHandler(this.保存为图片ToolStripMenuItem_Click);
            // 
            // 保存缓存ToolStripMenuItem
            // 
            this.保存缓存ToolStripMenuItem.Name = "保存缓存ToolStripMenuItem";
            this.保存缓存ToolStripMenuItem.Size = new System.Drawing.Size(174, 26);
            this.保存缓存ToolStripMenuItem.Text = "保存缓存";
            this.保存缓存ToolStripMenuItem.Click += new System.EventHandler(this.保存缓存ToolStripMenuItem_Click);
            // 
            // 读取缓存ToolStripMenuItem
            // 
            this.读取缓存ToolStripMenuItem.Name = "读取缓存ToolStripMenuItem";
            this.读取缓存ToolStripMenuItem.Size = new System.Drawing.Size(174, 26);
            this.读取缓存ToolStripMenuItem.Text = "读取缓存";
            this.读取缓存ToolStripMenuItem.Click += new System.EventHandler(this.读取缓存ToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.地图操作ToolStripMenuItem,
            this.地图操作ToolStripMenuItem1});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1040, 28);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.buttonMapType);
            this.panelMap.Controls.Add(this.mapControl);
            this.panelMap.Controls.Add(this.panelDock);
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(262, 28);
            this.panelMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(778, 618);
            this.panelMap.TabIndex = 18;
            // 
            // buttonMapType
            // 
            this.buttonMapType.Location = new System.Drawing.Point(680, 15);
            this.buttonMapType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMapType.Name = "buttonMapType";
            this.buttonMapType.Size = new System.Drawing.Size(67, 61);
            this.buttonMapType.TabIndex = 0;
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
            this.mapControl.Location = new System.Drawing.Point(0, 0);
            this.mapControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.mapControl.Size = new System.Drawing.Size(778, 473);
            this.mapControl.TabIndex = 6;
            this.mapControl.Zoom = 0D;
            // 
            // panelDock
            // 
            this.panelDock.Controls.Add(this.dataGridView1);
            this.panelDock.Controls.Add(this.panelButtonTools);
            this.panelDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDock.Location = new System.Drawing.Point(0, 473);
            this.panelDock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelDock.Name = "panelDock";
            this.panelDock.Size = new System.Drawing.Size(778, 145);
            this.panelDock.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.phoneNumberDataGridViewTextBoxColumn,
            this.xDataGridViewTextBoxColumn,
            this.yDataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.historyGeoDataBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 35);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(778, 110);
            this.dataGridView1.TabIndex = 1;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // phoneNumberDataGridViewTextBoxColumn
            // 
            this.phoneNumberDataGridViewTextBoxColumn.DataPropertyName = "PhoneNumber";
            this.phoneNumberDataGridViewTextBoxColumn.HeaderText = "Number";
            this.phoneNumberDataGridViewTextBoxColumn.Name = "phoneNumberDataGridViewTextBoxColumn";
            // 
            // xDataGridViewTextBoxColumn
            // 
            this.xDataGridViewTextBoxColumn.DataPropertyName = "X";
            this.xDataGridViewTextBoxColumn.HeaderText = "Longitude";
            this.xDataGridViewTextBoxColumn.Name = "xDataGridViewTextBoxColumn";
            // 
            // yDataGridViewTextBoxColumn
            // 
            this.yDataGridViewTextBoxColumn.DataPropertyName = "Y";
            this.yDataGridViewTextBoxColumn.HeaderText = "Latitude";
            this.yDataGridViewTextBoxColumn.Name = "yDataGridViewTextBoxColumn";
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            // 
            // historyGeoDataBindingSource
            // 
            this.historyGeoDataBindingSource.DataSource = typeof(GMapWinFormDemo.HistoryGeoData);
            // 
            // panelButtonTools
            // 
            this.panelButtonTools.Controls.Add(this.checkBoxFollow);
            this.panelButtonTools.Controls.Add(this.comboBoxTimeSpan);
            this.panelButtonTools.Controls.Add(this.buttonSetTimerInterval);
            this.panelButtonTools.Controls.Add(this.buttonResume);
            this.panelButtonTools.Controls.Add(this.buttonPause);
            this.panelButtonTools.Controls.Add(this.buttonStop);
            this.panelButtonTools.Controls.Add(this.buttonStart);
            this.panelButtonTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtonTools.Location = new System.Drawing.Point(0, 0);
            this.panelButtonTools.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelButtonTools.Name = "panelButtonTools";
            this.panelButtonTools.Size = new System.Drawing.Size(778, 35);
            this.panelButtonTools.TabIndex = 0;
            // 
            // checkBoxFollow
            // 
            this.checkBoxFollow.AutoSize = true;
            this.checkBoxFollow.Checked = true;
            this.checkBoxFollow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFollow.Location = new System.Drawing.Point(617, 9);
            this.checkBoxFollow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxFollow.Name = "checkBoxFollow";
            this.checkBoxFollow.Size = new System.Drawing.Size(77, 19);
            this.checkBoxFollow.TabIndex = 6;
            this.checkBoxFollow.Text = "Follow";
            this.checkBoxFollow.UseVisualStyleBackColor = true;
            // 
            // comboBoxTimeSpan
            // 
            this.comboBoxTimeSpan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTimeSpan.FormattingEnabled = true;
            this.comboBoxTimeSpan.Items.AddRange(new object[] {
            "0.5秒",
            "1秒",
            "2秒",
            "3秒",
            "5秒",
            "10秒",
            "20秒",
            "30秒",
            "60秒"});
            this.comboBoxTimeSpan.Location = new System.Drawing.Point(331, 8);
            this.comboBoxTimeSpan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxTimeSpan.Name = "comboBoxTimeSpan";
            this.comboBoxTimeSpan.Size = new System.Drawing.Size(107, 23);
            this.comboBoxTimeSpan.TabIndex = 5;
            // 
            // buttonSetTimerInterval
            // 
            this.buttonSetTimerInterval.Location = new System.Drawing.Point(447, 4);
            this.buttonSetTimerInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSetTimerInterval.Name = "buttonSetTimerInterval";
            this.buttonSetTimerInterval.Size = new System.Drawing.Size(163, 29);
            this.buttonSetTimerInterval.TabIndex = 4;
            this.buttonSetTimerInterval.Text = "Set Timer Interval";
            this.buttonSetTimerInterval.UseVisualStyleBackColor = true;
            this.buttonSetTimerInterval.Click += new System.EventHandler(this.buttonSetTimerInterval_Click);
            // 
            // buttonResume
            // 
            this.buttonResume.Location = new System.Drawing.Point(248, 4);
            this.buttonResume.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(73, 29);
            this.buttonResume.TabIndex = 3;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = true;
            this.buttonResume.Click += new System.EventHandler(this.buttonResume_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(168, 4);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(73, 29);
            this.buttonPause.TabIndex = 2;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(88, 4);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(73, 29);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(8, 4);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(73, 29);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxRegion
            // 
            this.comboBoxRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegion.FormattingEnabled = true;
            this.comboBoxRegion.Location = new System.Drawing.Point(873, 1);
            this.comboBoxRegion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxRegion.Name = "comboBoxRegion";
            this.comboBoxRegion.Size = new System.Drawing.Size(117, 23);
            this.comboBoxRegion.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Transparent;
            this.splitter1.Location = new System.Drawing.Point(255, 28);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.MinSize = 5;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(7, 618);
            this.splitter1.TabIndex = 17;
            this.splitter1.TabStop = false;
            // 
            // panelMenu
            // 
            this.panelMenu.AssociatedSplitter = this.splitter1;
            this.panelMenu.BackColor = System.Drawing.Color.Transparent;
            this.panelMenu.CaptionFont = new System.Drawing.Font("Segoe UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.panelMenu.CaptionHeight = 27;
            this.panelMenu.Controls.Add(this.xPanderPanelList1);
            this.panelMenu.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.panelMenu.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panelMenu.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panelMenu.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panelMenu.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panelMenu.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panelMenu.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelMenu.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelMenu.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panelMenu.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelMenu.Image = null;
            this.panelMenu.Location = new System.Drawing.Point(0, 28);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMenu.MinimumSize = new System.Drawing.Size(36, 34);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelMenu.ShowExpandIcon = true;
            this.panelMenu.Size = new System.Drawing.Size(255, 618);
            this.panelMenu.TabIndex = 16;
            this.panelMenu.Text = "Menu";
            this.panelMenu.ToolTipTextCloseIcon = null;
            this.panelMenu.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelMenu.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // xPanderPanelList1
            // 
            this.xPanderPanelList1.CaptionStyle = BSE.Windows.Forms.CaptionStyle.Normal;
            this.xPanderPanelList1.Controls.Add(this.xPanderPanelRoute);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanelMarker);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanelDraw);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanelChinaRegion);
            this.xPanderPanelList1.Controls.Add(this.xPanderPanelMap);
            this.xPanderPanelList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPanderPanelList1.GradientBackground = System.Drawing.Color.Empty;
            this.xPanderPanelList1.Location = new System.Drawing.Point(0, 28);
            this.xPanderPanelList1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelList1.Name = "xPanderPanelList1";
            this.xPanderPanelList1.PanelColors = null;
            this.xPanderPanelList1.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelList1.Size = new System.Drawing.Size(255, 589);
            this.xPanderPanelList1.TabIndex = 0;
            this.xPanderPanelList1.Text = "xPanderPanelList1";
            // 
            // xPanderPanelRoute
            // 
            this.xPanderPanelRoute.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelRoute.Controls.Add(this.groupBox5);
            this.xPanderPanelRoute.Controls.Add(this.textBoxSearch);
            this.xPanderPanelRoute.Controls.Add(this.buttonSearch);
            this.xPanderPanelRoute.Controls.Add(this.label1);
            this.xPanderPanelRoute.Controls.Add(this.comboBoxSearchResult);
            this.xPanderPanelRoute.Controls.Add(this.buttonSetStart);
            this.xPanderPanelRoute.Controls.Add(this.buttonFindRoute);
            this.xPanderPanelRoute.Controls.Add(this.buttonSetEnd);
            this.xPanderPanelRoute.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanelRoute.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanelRoute.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanelRoute.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanelRoute.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanelRoute.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelRoute.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanelRoute.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelRoute.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelRoute.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelRoute.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelRoute.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanelRoute.Expand = true;
            this.xPanderPanelRoute.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.Image = null;
            this.xPanderPanelRoute.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelRoute.Name = "xPanderPanelRoute";
            this.xPanderPanelRoute.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelRoute.Size = new System.Drawing.Size(255, 489);
            this.xPanderPanelRoute.TabIndex = 0;
            this.xPanderPanelRoute.Text = "Route";
            this.xPanderPanelRoute.ToolTipTextCloseIcon = null;
            this.xPanderPanelRoute.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelRoute.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonHisTestData);
            this.groupBox5.Location = new System.Drawing.Point(12, 218);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(220, 81);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "History Route";
            // 
            // buttonHisTestData
            // 
            this.buttonHisTestData.Location = new System.Drawing.Point(25, 26);
            this.buttonHisTestData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonHisTestData.Name = "buttonHisTestData";
            this.buttonHisTestData.Size = new System.Drawing.Size(125, 29);
            this.buttonHisTestData.TabIndex = 0;
            this.buttonHisTestData.Text = "Get Test Data";
            this.buttonHisTestData.UseVisualStyleBackColor = true;
            this.buttonHisTestData.Click += new System.EventHandler(this.buttonHisTestData_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(13, 38);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(112, 25);
            this.textBoxSearch.TabIndex = 19;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(135, 35);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(69, 29);
            this.buttonSearch.TabIndex = 20;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 72);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "Search Result:";
            // 
            // comboBoxSearchResult
            // 
            this.comboBoxSearchResult.FormattingEnabled = true;
            this.comboBoxSearchResult.Location = new System.Drawing.Point(12, 91);
            this.comboBoxSearchResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxSearchResult.Name = "comboBoxSearchResult";
            this.comboBoxSearchResult.Size = new System.Drawing.Size(168, 23);
            this.comboBoxSearchResult.TabIndex = 21;
            this.comboBoxSearchResult.SelectedValueChanged += new System.EventHandler(this.comboBoxSearchResult_SelectedIndexChanged);
            // 
            // buttonSetStart
            // 
            this.buttonSetStart.Location = new System.Drawing.Point(9, 125);
            this.buttonSetStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSetStart.Name = "buttonSetStart";
            this.buttonSetStart.Size = new System.Drawing.Size(91, 29);
            this.buttonSetStart.TabIndex = 22;
            this.buttonSetStart.Text = "Set Start";
            this.buttonSetStart.UseVisualStyleBackColor = true;
            this.buttonSetStart.Click += new System.EventHandler(this.buttonSetStart_Click);
            // 
            // buttonFindRoute
            // 
            this.buttonFindRoute.Location = new System.Drawing.Point(9, 161);
            this.buttonFindRoute.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonFindRoute.Name = "buttonFindRoute";
            this.buttonFindRoute.Size = new System.Drawing.Size(100, 29);
            this.buttonFindRoute.TabIndex = 25;
            this.buttonFindRoute.Text = "Find Route";
            this.buttonFindRoute.UseVisualStyleBackColor = true;
            this.buttonFindRoute.Click += new System.EventHandler(this.buttonFindRoute_Click);
            // 
            // buttonSetEnd
            // 
            this.buttonSetEnd.Location = new System.Drawing.Point(109, 125);
            this.buttonSetEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSetEnd.Name = "buttonSetEnd";
            this.buttonSetEnd.Size = new System.Drawing.Size(91, 29);
            this.buttonSetEnd.TabIndex = 23;
            this.buttonSetEnd.Text = "Set End";
            this.buttonSetEnd.UseVisualStyleBackColor = true;
            this.buttonSetEnd.Click += new System.EventHandler(this.buttonSetEnd_Click);
            // 
            // xPanderPanelMarker
            // 
            this.xPanderPanelMarker.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelMarker.Controls.Add(this.groupBox2);
            this.xPanderPanelMarker.Controls.Add(this.groupBox1);
            this.xPanderPanelMarker.Controls.Add(this.checkBoxMarker);
            this.xPanderPanelMarker.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanelMarker.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanelMarker.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanelMarker.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanelMarker.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanelMarker.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMarker.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMarker.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelMarker.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanelMarker.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelMarker.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMarker.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMarker.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMarker.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelMarker.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelMarker.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanelMarker.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMarker.Image = null;
            this.xPanderPanelMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelMarker.Name = "xPanderPanelMarker";
            this.xPanderPanelMarker.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelMarker.Size = new System.Drawing.Size(255, 25);
            this.xPanderPanelMarker.TabIndex = 1;
            this.xPanderPanelMarker.Text = "Marker";
            this.xPanderPanelMarker.ToolTipTextCloseIcon = null;
            this.xPanderPanelMarker.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelMarker.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbGMapMarkerScopeCircleAnimate);
            this.groupBox2.Controls.Add(this.rbGMapMarkerScopePieAnimate);
            this.groupBox2.Controls.Add(this.rbGMapTipMarker);
            this.groupBox2.Controls.Add(this.rbGMapDirectionMarker);
            this.groupBox2.Controls.Add(this.rbGMapGifMarker);
            this.groupBox2.Controls.Add(this.rbGMapFlashMarker);
            this.groupBox2.Controls.Add(this.rbGMarkerGoogle);
            this.groupBox2.Location = new System.Drawing.Point(5, 70);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(241, 250);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Marker Type";
            // 
            // rbGMapMarkerScopeCircleAnimate
            // 
            this.rbGMapMarkerScopeCircleAnimate.AutoSize = true;
            this.rbGMapMarkerScopeCircleAnimate.Location = new System.Drawing.Point(7, 221);
            this.rbGMapMarkerScopeCircleAnimate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapMarkerScopeCircleAnimate.Name = "rbGMapMarkerScopeCircleAnimate";
            this.rbGMapMarkerScopeCircleAnimate.Size = new System.Drawing.Size(252, 19);
            this.rbGMapMarkerScopeCircleAnimate.TabIndex = 6;
            this.rbGMapMarkerScopeCircleAnimate.TabStop = true;
            this.rbGMapMarkerScopeCircleAnimate.Text = "GMapMarkerScopeCircleAnimate";
            this.rbGMapMarkerScopeCircleAnimate.UseVisualStyleBackColor = true;
            // 
            // rbGMapMarkerScopePieAnimate
            // 
            this.rbGMapMarkerScopePieAnimate.AutoSize = true;
            this.rbGMapMarkerScopePieAnimate.Location = new System.Drawing.Point(7, 189);
            this.rbGMapMarkerScopePieAnimate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapMarkerScopePieAnimate.Name = "rbGMapMarkerScopePieAnimate";
            this.rbGMapMarkerScopePieAnimate.Size = new System.Drawing.Size(228, 19);
            this.rbGMapMarkerScopePieAnimate.TabIndex = 5;
            this.rbGMapMarkerScopePieAnimate.TabStop = true;
            this.rbGMapMarkerScopePieAnimate.Text = "GMapMarkerScopePieAnimate";
            this.rbGMapMarkerScopePieAnimate.UseVisualStyleBackColor = true;
            // 
            // rbGMapTipMarker
            // 
            this.rbGMapTipMarker.AutoSize = true;
            this.rbGMapTipMarker.Location = new System.Drawing.Point(7, 156);
            this.rbGMapTipMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapTipMarker.Name = "rbGMapTipMarker";
            this.rbGMapTipMarker.Size = new System.Drawing.Size(132, 19);
            this.rbGMapTipMarker.TabIndex = 4;
            this.rbGMapTipMarker.TabStop = true;
            this.rbGMapTipMarker.Text = "GMapTipMarker";
            this.rbGMapTipMarker.UseVisualStyleBackColor = true;
            // 
            // rbGMapDirectionMarker
            // 
            this.rbGMapDirectionMarker.AutoSize = true;
            this.rbGMapDirectionMarker.Location = new System.Drawing.Point(7, 124);
            this.rbGMapDirectionMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapDirectionMarker.Name = "rbGMapDirectionMarker";
            this.rbGMapDirectionMarker.Size = new System.Drawing.Size(180, 19);
            this.rbGMapDirectionMarker.TabIndex = 3;
            this.rbGMapDirectionMarker.TabStop = true;
            this.rbGMapDirectionMarker.Text = "GMapDirectionMarker";
            this.rbGMapDirectionMarker.UseVisualStyleBackColor = true;
            // 
            // rbGMapGifMarker
            // 
            this.rbGMapGifMarker.AutoSize = true;
            this.rbGMapGifMarker.Location = new System.Drawing.Point(7, 91);
            this.rbGMapGifMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapGifMarker.Name = "rbGMapGifMarker";
            this.rbGMapGifMarker.Size = new System.Drawing.Size(132, 19);
            this.rbGMapGifMarker.TabIndex = 2;
            this.rbGMapGifMarker.TabStop = true;
            this.rbGMapGifMarker.Text = "GMapGifMarker";
            this.rbGMapGifMarker.UseVisualStyleBackColor = true;
            // 
            // rbGMapFlashMarker
            // 
            this.rbGMapFlashMarker.AutoSize = true;
            this.rbGMapFlashMarker.Location = new System.Drawing.Point(7, 59);
            this.rbGMapFlashMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMapFlashMarker.Name = "rbGMapFlashMarker";
            this.rbGMapFlashMarker.Size = new System.Drawing.Size(148, 19);
            this.rbGMapFlashMarker.TabIndex = 1;
            this.rbGMapFlashMarker.TabStop = true;
            this.rbGMapFlashMarker.Text = "GMapFlashMarker";
            this.rbGMapFlashMarker.UseVisualStyleBackColor = true;
            // 
            // rbGMarkerGoogle
            // 
            this.rbGMarkerGoogle.AutoSize = true;
            this.rbGMarkerGoogle.Location = new System.Drawing.Point(7, 26);
            this.rbGMarkerGoogle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbGMarkerGoogle.Name = "rbGMarkerGoogle";
            this.rbGMarkerGoogle.Size = new System.Drawing.Size(132, 19);
            this.rbGMarkerGoogle.TabIndex = 0;
            this.rbGMarkerGoogle.TabStop = true;
            this.rbGMarkerGoogle.Text = "GMarkerGoogle";
            this.rbGMarkerGoogle.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonStopBlink);
            this.groupBox1.Controls.Add(this.buttonBeginBlink);
            this.groupBox1.Location = new System.Drawing.Point(20, 328);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(184, 108);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flash Marker";
            // 
            // buttonStopBlink
            // 
            this.buttonStopBlink.Location = new System.Drawing.Point(29, 69);
            this.buttonStopBlink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStopBlink.Name = "buttonStopBlink";
            this.buttonStopBlink.Size = new System.Drawing.Size(116, 29);
            this.buttonStopBlink.TabIndex = 23;
            this.buttonStopBlink.Text = "Stop Blink";
            this.buttonStopBlink.UseVisualStyleBackColor = true;
            this.buttonStopBlink.Click += new System.EventHandler(this.buttonStopBlink_Click);
            // 
            // buttonBeginBlink
            // 
            this.buttonBeginBlink.Location = new System.Drawing.Point(29, 25);
            this.buttonBeginBlink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonBeginBlink.Name = "buttonBeginBlink";
            this.buttonBeginBlink.Size = new System.Drawing.Size(116, 29);
            this.buttonBeginBlink.TabIndex = 22;
            this.buttonBeginBlink.Text = "Marker Blink";
            this.buttonBeginBlink.UseVisualStyleBackColor = true;
            this.buttonBeginBlink.Click += new System.EventHandler(this.buttonBeginBlink_Click);
            // 
            // checkBoxMarker
            // 
            this.checkBoxMarker.AutoSize = true;
            this.checkBoxMarker.Checked = true;
            this.checkBoxMarker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarker.Location = new System.Drawing.Point(1, 42);
            this.checkBoxMarker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxMarker.Name = "checkBoxMarker";
            this.checkBoxMarker.Size = new System.Drawing.Size(229, 19);
            this.checkBoxMarker.TabIndex = 22;
            this.checkBoxMarker.Text = "Right Click to Add Marker";
            this.checkBoxMarker.UseVisualStyleBackColor = true;
            // 
            // xPanderPanelDraw
            // 
            this.xPanderPanelDraw.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelDraw.Controls.Add(this.groupBox4);
            this.xPanderPanelDraw.Controls.Add(this.groupBox3);
            this.xPanderPanelDraw.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanelDraw.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanelDraw.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanelDraw.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanelDraw.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanelDraw.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelDraw.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelDraw.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelDraw.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanelDraw.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelDraw.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelDraw.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelDraw.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelDraw.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelDraw.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelDraw.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanelDraw.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelDraw.Image = null;
            this.xPanderPanelDraw.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelDraw.Name = "xPanderPanelDraw";
            this.xPanderPanelDraw.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelDraw.Size = new System.Drawing.Size(255, 25);
            this.xPanderPanelDraw.TabIndex = 2;
            this.xPanderPanelDraw.Text = "Draw Tools";
            this.xPanderPanelDraw.ToolTipTextCloseIcon = null;
            this.xPanderPanelDraw.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelDraw.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonDistance);
            this.groupBox4.Location = new System.Drawing.Point(8, 210);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(224, 125);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Distance Tool";
            // 
            // buttonDistance
            // 
            this.buttonDistance.Location = new System.Drawing.Point(8, 25);
            this.buttonDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDistance.Name = "buttonDistance";
            this.buttonDistance.Size = new System.Drawing.Size(88, 29);
            this.buttonDistance.TabIndex = 5;
            this.buttonDistance.Text = "Distance";
            this.buttonDistance.UseVisualStyleBackColor = true;
            this.buttonDistance.Click += new System.EventHandler(this.buttonDistance_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonLine);
            this.groupBox3.Controls.Add(this.buttonClear);
            this.groupBox3.Controls.Add(this.buttonPolyline);
            this.groupBox3.Controls.Add(this.buttonCircle);
            this.groupBox3.Controls.Add(this.buttonRectangle);
            this.groupBox3.Controls.Add(this.buttonPolygon);
            this.groupBox3.Location = new System.Drawing.Point(5, 41);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(227, 146);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Draw Polygon";
            // 
            // buttonLine
            // 
            this.buttonLine.Location = new System.Drawing.Point(8, 106);
            this.buttonLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(89, 29);
            this.buttonLine.TabIndex = 10;
            this.buttonLine.Text = "Line";
            this.buttonLine.UseVisualStyleBackColor = true;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(124, 110);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(89, 29);
            this.buttonClear.TabIndex = 8;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonPolyline
            // 
            this.buttonPolyline.Location = new System.Drawing.Point(124, 69);
            this.buttonPolyline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPolyline.Name = "buttonPolyline";
            this.buttonPolyline.Size = new System.Drawing.Size(89, 29);
            this.buttonPolyline.TabIndex = 9;
            this.buttonPolyline.Text = "Polyline";
            this.buttonPolyline.UseVisualStyleBackColor = true;
            this.buttonPolyline.Click += new System.EventHandler(this.buttonPolyline_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.Location = new System.Drawing.Point(9, 29);
            this.buttonCircle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(89, 29);
            this.buttonCircle.TabIndex = 5;
            this.buttonCircle.Text = "Circle";
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonRectangle
            // 
            this.buttonRectangle.Location = new System.Drawing.Point(124, 30);
            this.buttonRectangle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRectangle.Name = "buttonRectangle";
            this.buttonRectangle.Size = new System.Drawing.Size(89, 29);
            this.buttonRectangle.TabIndex = 6;
            this.buttonRectangle.Text = "Rectangle";
            this.buttonRectangle.UseVisualStyleBackColor = true;
            this.buttonRectangle.Click += new System.EventHandler(this.buttonRectangle_Click);
            // 
            // buttonPolygon
            // 
            this.buttonPolygon.Location = new System.Drawing.Point(9, 66);
            this.buttonPolygon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPolygon.Name = "buttonPolygon";
            this.buttonPolygon.Size = new System.Drawing.Size(89, 29);
            this.buttonPolygon.TabIndex = 7;
            this.buttonPolygon.Text = "Polygon";
            this.buttonPolygon.UseVisualStyleBackColor = true;
            this.buttonPolygon.Click += new System.EventHandler(this.buttonPolygon_Click);
            // 
            // xPanderPanelChinaRegion
            // 
            this.xPanderPanelChinaRegion.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelChinaRegion.Controls.Add(this.treeView1);
            this.xPanderPanelChinaRegion.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanelChinaRegion.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanelChinaRegion.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanelChinaRegion.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanelChinaRegion.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelChinaRegion.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelChinaRegion.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanelChinaRegion.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelChinaRegion.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelChinaRegion.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelChinaRegion.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelChinaRegion.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelChinaRegion.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanelChinaRegion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelChinaRegion.Image = null;
            this.xPanderPanelChinaRegion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelChinaRegion.Name = "xPanderPanelChinaRegion";
            this.xPanderPanelChinaRegion.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelChinaRegion.Size = new System.Drawing.Size(255, 25);
            this.xPanderPanelChinaRegion.TabIndex = 3;
            this.xPanderPanelChinaRegion.Text = "China Region";
            this.xPanderPanelChinaRegion.ToolTipTextCloseIcon = null;
            this.xPanderPanelChinaRegion.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelChinaRegion.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(1, 25);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(253, 0);
            this.treeView1.TabIndex = 0;
            // 
            // xPanderPanelMap
            // 
            this.xPanderPanelMap.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelMap.Controls.Add(this.buttonClearSArea);
            this.xPanderPanelMap.Controls.Add(this.buttonPrefetchSArea);
            this.xPanderPanelMap.CustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.xPanderPanelMap.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.xPanderPanelMap.CustomColors.CaptionCheckedGradientBegin = System.Drawing.Color.Empty;
            this.xPanderPanelMap.CustomColors.CaptionCheckedGradientEnd = System.Drawing.Color.Empty;
            this.xPanderPanelMap.CustomColors.CaptionCheckedGradientMiddle = System.Drawing.Color.Empty;
            this.xPanderPanelMap.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelMap.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.xPanderPanelMap.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelMap.CustomColors.CaptionPressedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionPressedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionPressedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionSelectedGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.xPanderPanelMap.CustomColors.CaptionSelectedText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.CustomColors.FlatCaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.xPanderPanelMap.CustomColors.FlatCaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.xPanderPanelMap.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.xPanderPanelMap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.Image = null;
            this.xPanderPanelMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xPanderPanelMap.Name = "xPanderPanelMap";
            this.xPanderPanelMap.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelMap.Size = new System.Drawing.Size(255, 25);
            this.xPanderPanelMap.TabIndex = 4;
            this.xPanderPanelMap.Text = "Map";
            this.xPanderPanelMap.ToolTipTextCloseIcon = null;
            this.xPanderPanelMap.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelMap.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // buttonClearSArea
            // 
            this.buttonClearSArea.Location = new System.Drawing.Point(13, 90);
            this.buttonClearSArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClearSArea.Name = "buttonClearSArea";
            this.buttonClearSArea.Size = new System.Drawing.Size(195, 29);
            this.buttonClearSArea.TabIndex = 1;
            this.buttonClearSArea.Text = "Clear selected area";
            this.buttonClearSArea.UseVisualStyleBackColor = true;
            this.buttonClearSArea.Click += new System.EventHandler(this.buttonClearSArea_Click);
            // 
            // buttonPrefetchSArea
            // 
            this.buttonPrefetchSArea.Location = new System.Drawing.Point(13, 54);
            this.buttonPrefetchSArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPrefetchSArea.Name = "buttonPrefetchSArea";
            this.buttonPrefetchSArea.Size = new System.Drawing.Size(195, 29);
            this.buttonPrefetchSArea.TabIndex = 0;
            this.buttonPrefetchSArea.Text = "Prefetch selected area";
            this.buttonPrefetchSArea.UseVisualStyleBackColor = true;
            this.buttonPrefetchSArea.Click += new System.EventHandler(this.buttonPrefetch_Click);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 646);
            this.Controls.Add(this.comboBoxRegion);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.menuStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.contextMenuStripMarker.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelMap.ResumeLayout(false);
            this.panelDock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.historyGeoDataBindingSource)).EndInit();
            this.panelButtonTools.ResumeLayout(false);
            this.panelButtonTools.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.xPanderPanelList1.ResumeLayout(false);
            this.xPanderPanelRoute.ResumeLayout(false);
            this.xPanderPanelRoute.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.xPanderPanelMarker.ResumeLayout(false);
            this.xPanderPanelMarker.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.xPanderPanelDraw.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.xPanderPanelChinaRegion.ResumeLayout(false);
            this.xPanderPanelMap.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStripMarker;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private BSE.Windows.Forms.Panel panelMenu;
        private BSE.Windows.Forms.XPanderPanelList xPanderPanelList1;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelRoute;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelMarker;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSearchResult;
        private System.Windows.Forms.Button buttonSetStart;
        private System.Windows.Forms.Button buttonFindRoute;
        private System.Windows.Forms.Button buttonSetEnd;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelDraw;
        private System.Windows.Forms.ToolStripMenuItem 地图操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 谷歌地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高德地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 腾讯地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 百度地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地图操作ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存为图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存缓存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取缓存ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private BSE.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.ComboBox comboBoxRegion;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelChinaRegion;
        private System.Windows.Forms.TreeView treeView1;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelMap;
        private System.Windows.Forms.Button buttonPrefetchSArea;
        private System.Windows.Forms.Button buttonClearSArea;
        private System.Windows.Forms.CheckBox checkBoxMarker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonStopBlink;
        private System.Windows.Forms.Button buttonBeginBlink;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbGMarkerGoogle;
        private System.Windows.Forms.RadioButton rbGMapFlashMarker;
        private System.Windows.Forms.RadioButton rbGMapMarkerScopePieAnimate;
        private System.Windows.Forms.RadioButton rbGMapTipMarker;
        private System.Windows.Forms.RadioButton rbGMapDirectionMarker;
        private System.Windows.Forms.RadioButton rbGMapGifMarker;
        private System.Windows.Forms.RadioButton rbGMapMarkerScopeCircleAnimate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonDistance;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonPolyline;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonRectangle;
        private System.Windows.Forms.Button buttonPolygon;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonHisTestData;
        private System.Windows.Forms.Panel panelDock;
        private System.Windows.Forms.Button buttonMapType;
        private MapControl mapControl;
        private System.Windows.Forms.Panel panelButtonTools;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn phoneNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource historyGeoDataBindingSource;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonSetTimerInterval;
        private System.Windows.Forms.Button buttonResume;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.ComboBox comboBoxTimeSpan;
        private System.Windows.Forms.CheckBox checkBoxFollow;
        private System.Windows.Forms.Button buttonLine;
    }
}

