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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCurrentPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonMapType = new System.Windows.Forms.Button();
            this.mapControl = new GMapWinFormDemo.MapControl();
            this.comboBoxRegion = new System.Windows.Forms.ComboBox();
            this.splitter1 = new BSE.Windows.Forms.Splitter();
            this.panelMenu = new BSE.Windows.Forms.Panel();
            this.xPanderPanelList1 = new BSE.Windows.Forms.XPanderPanelList();
            this.xPanderPanelRoute = new BSE.Windows.Forms.XPanderPanel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSearchResult = new System.Windows.Forms.ComboBox();
            this.buttonSetStart = new System.Windows.Forms.Button();
            this.buttonFindRoute = new System.Windows.Forms.Button();
            this.buttonSetEnd = new System.Windows.Forms.Button();
            this.xPanderPanelMarker = new BSE.Windows.Forms.XPanderPanel();
            this.buttonStopBlink = new System.Windows.Forms.Button();
            this.buttonBeginBlink = new System.Windows.Forms.Button();
            this.xPanderPanelDraw = new BSE.Windows.Forms.XPanderPanel();
            this.buttonDistance = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonPolyline = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonRectangle = new System.Windows.Forms.Button();
            this.buttonPolygon = new System.Windows.Forms.Button();
            this.xPanderPanelChinaRegion = new BSE.Windows.Forms.XPanderPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.xPanderPanelMap = new BSE.Windows.Forms.XPanderPanel();
            this.checkBoxTileHost = new System.Windows.Forms.CheckBox();
            this.buttonClearSArea = new System.Windows.Forms.Button();
            this.buttonPrefetchSArea = new System.Windows.Forms.Button();
            this.contextMenuStripMarker.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panelMap.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.xPanderPanelList1.SuspendLayout();
            this.xPanderPanelRoute.SuspendLayout();
            this.xPanderPanelMarker.SuspendLayout();
            this.xPanderPanelDraw.SuspendLayout();
            this.xPanderPanelChinaRegion.SuspendLayout();
            this.xPanderPanelMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripMarker
            // 
            this.contextMenuStripMarker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStripMarker.Name = "contextMenuStrip1";
            this.contextMenuStripMarker.Size = new System.Drawing.Size(99, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
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
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.地图操作ToolStripMenuItem,
            this.地图操作ToolStripMenuItem1});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(750, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.statusStrip);
            this.panelMap.Controls.Add(this.buttonMapType);
            this.panelMap.Controls.Add(this.mapControl);
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(177, 24);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(573, 493);
            this.panelMap.TabIndex = 18;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCurrentPos});
            this.statusStrip.Location = new System.Drawing.Point(0, 471);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(573, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCurrentPos
            // 
            this.toolStripStatusLabelCurrentPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelCurrentPos.Name = "toolStripStatusLabelCurrentPos";
            this.toolStripStatusLabelCurrentPos.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabelCurrentPos.Text = "toolStripStatusLabel1";
            // 
            // buttonMapType
            // 
            this.buttonMapType.Location = new System.Drawing.Point(503, 4);
            this.buttonMapType.Name = "buttonMapType";
            this.buttonMapType.Size = new System.Drawing.Size(50, 49);
            this.buttonMapType.TabIndex = 3;
            this.buttonMapType.UseVisualStyleBackColor = true;
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
            this.mapControl.Size = new System.Drawing.Size(573, 493);
            this.mapControl.TabIndex = 1;
            this.mapControl.Zoom = 0D;
            // 
            // comboBoxRegion
            // 
            this.comboBoxRegion.FormattingEnabled = true;
            this.comboBoxRegion.Location = new System.Drawing.Point(655, 1);
            this.comboBoxRegion.Name = "comboBoxRegion";
            this.comboBoxRegion.Size = new System.Drawing.Size(89, 20);
            this.comboBoxRegion.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Transparent;
            this.splitter1.Location = new System.Drawing.Point(172, 24);
            this.splitter1.MinSize = 5;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 493);
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
            this.panelMenu.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelMenu.Location = new System.Drawing.Point(0, 24);
            this.panelMenu.MinimumSize = new System.Drawing.Size(27, 27);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panelMenu.ShowExpandIcon = true;
            this.panelMenu.Size = new System.Drawing.Size(172, 493);
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
            this.xPanderPanelList1.Name = "xPanderPanelList1";
            this.xPanderPanelList1.PanelColors = null;
            this.xPanderPanelList1.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelList1.Size = new System.Drawing.Size(172, 464);
            this.xPanderPanelList1.TabIndex = 0;
            this.xPanderPanelList1.Text = "xPanderPanelList1";
            // 
            // xPanderPanelRoute
            // 
            this.xPanderPanelRoute.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
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
            this.xPanderPanelRoute.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelRoute.Image = null;
            this.xPanderPanelRoute.Name = "xPanderPanelRoute";
            this.xPanderPanelRoute.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelRoute.Size = new System.Drawing.Size(172, 25);
            this.xPanderPanelRoute.TabIndex = 0;
            this.xPanderPanelRoute.Text = "Route";
            this.xPanderPanelRoute.ToolTipTextCloseIcon = null;
            this.xPanderPanelRoute.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelRoute.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(10, 30);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(85, 21);
            this.textBoxSearch.TabIndex = 19;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(101, 28);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(52, 23);
            this.buttonSearch.TabIndex = 20;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "Search Result:";
            // 
            // comboBoxSearchResult
            // 
            this.comboBoxSearchResult.FormattingEnabled = true;
            this.comboBoxSearchResult.Location = new System.Drawing.Point(9, 73);
            this.comboBoxSearchResult.Name = "comboBoxSearchResult";
            this.comboBoxSearchResult.Size = new System.Drawing.Size(127, 20);
            this.comboBoxSearchResult.TabIndex = 21;
            this.comboBoxSearchResult.SelectedValueChanged += new System.EventHandler(this.comboBoxSearchResult_SelectedIndexChanged);
            // 
            // buttonSetStart
            // 
            this.buttonSetStart.Location = new System.Drawing.Point(7, 100);
            this.buttonSetStart.Name = "buttonSetStart";
            this.buttonSetStart.Size = new System.Drawing.Size(68, 23);
            this.buttonSetStart.TabIndex = 22;
            this.buttonSetStart.Text = "Set Start";
            this.buttonSetStart.UseVisualStyleBackColor = true;
            this.buttonSetStart.Click += new System.EventHandler(this.buttonSetStart_Click);
            // 
            // buttonFindRoute
            // 
            this.buttonFindRoute.Location = new System.Drawing.Point(7, 129);
            this.buttonFindRoute.Name = "buttonFindRoute";
            this.buttonFindRoute.Size = new System.Drawing.Size(75, 23);
            this.buttonFindRoute.TabIndex = 25;
            this.buttonFindRoute.Text = "Find Route";
            this.buttonFindRoute.UseVisualStyleBackColor = true;
            this.buttonFindRoute.Click += new System.EventHandler(this.buttonFindRoute_Click);
            // 
            // buttonSetEnd
            // 
            this.buttonSetEnd.Location = new System.Drawing.Point(82, 100);
            this.buttonSetEnd.Name = "buttonSetEnd";
            this.buttonSetEnd.Size = new System.Drawing.Size(68, 23);
            this.buttonSetEnd.TabIndex = 23;
            this.buttonSetEnd.Text = "Set End";
            this.buttonSetEnd.UseVisualStyleBackColor = true;
            this.buttonSetEnd.Click += new System.EventHandler(this.buttonSetEnd_Click);
            // 
            // xPanderPanelMarker
            // 
            this.xPanderPanelMarker.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelMarker.Controls.Add(this.buttonStopBlink);
            this.xPanderPanelMarker.Controls.Add(this.buttonBeginBlink);
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
            this.xPanderPanelMarker.Name = "xPanderPanelMarker";
            this.xPanderPanelMarker.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelMarker.Size = new System.Drawing.Size(172, 25);
            this.xPanderPanelMarker.TabIndex = 1;
            this.xPanderPanelMarker.Text = "Marker";
            this.xPanderPanelMarker.ToolTipTextCloseIcon = null;
            this.xPanderPanelMarker.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelMarker.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // buttonStopBlink
            // 
            this.buttonStopBlink.Location = new System.Drawing.Point(26, 89);
            this.buttonStopBlink.Name = "buttonStopBlink";
            this.buttonStopBlink.Size = new System.Drawing.Size(87, 23);
            this.buttonStopBlink.TabIndex = 21;
            this.buttonStopBlink.Text = "Stop Blink";
            this.buttonStopBlink.UseVisualStyleBackColor = true;
            this.buttonStopBlink.Click += new System.EventHandler(this.buttonStopBlink_Click);
            // 
            // buttonBeginBlink
            // 
            this.buttonBeginBlink.Location = new System.Drawing.Point(26, 45);
            this.buttonBeginBlink.Name = "buttonBeginBlink";
            this.buttonBeginBlink.Size = new System.Drawing.Size(87, 23);
            this.buttonBeginBlink.TabIndex = 20;
            this.buttonBeginBlink.Text = "Marker Blink";
            this.buttonBeginBlink.UseVisualStyleBackColor = true;
            this.buttonBeginBlink.Click += new System.EventHandler(this.buttonBeginBlink_Click);
            // 
            // xPanderPanelDraw
            // 
            this.xPanderPanelDraw.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelDraw.Controls.Add(this.buttonDistance);
            this.xPanderPanelDraw.Controls.Add(this.buttonClear);
            this.xPanderPanelDraw.Controls.Add(this.buttonPolyline);
            this.xPanderPanelDraw.Controls.Add(this.buttonCircle);
            this.xPanderPanelDraw.Controls.Add(this.buttonRectangle);
            this.xPanderPanelDraw.Controls.Add(this.buttonPolygon);
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
            this.xPanderPanelDraw.Name = "xPanderPanelDraw";
            this.xPanderPanelDraw.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelDraw.Size = new System.Drawing.Size(172, 25);
            this.xPanderPanelDraw.TabIndex = 2;
            this.xPanderPanelDraw.Text = "Draw Tools";
            this.xPanderPanelDraw.ToolTipTextCloseIcon = null;
            this.xPanderPanelDraw.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelDraw.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // buttonDistance
            // 
            this.buttonDistance.Location = new System.Drawing.Point(10, 190);
            this.buttonDistance.Name = "buttonDistance";
            this.buttonDistance.Size = new System.Drawing.Size(66, 23);
            this.buttonDistance.TabIndex = 4;
            this.buttonDistance.Text = "Distance";
            this.buttonDistance.UseVisualStyleBackColor = true;
            this.buttonDistance.Click += new System.EventHandler(this.buttonDistance_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(9, 138);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(67, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonPolyline
            // 
            this.buttonPolyline.Location = new System.Drawing.Point(95, 93);
            this.buttonPolyline.Name = "buttonPolyline";
            this.buttonPolyline.Size = new System.Drawing.Size(67, 23);
            this.buttonPolyline.TabIndex = 4;
            this.buttonPolyline.Text = "Polyline";
            this.buttonPolyline.UseVisualStyleBackColor = true;
            this.buttonPolyline.Click += new System.EventHandler(this.buttonPolyline_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.Location = new System.Drawing.Point(9, 49);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(67, 23);
            this.buttonCircle.TabIndex = 0;
            this.buttonCircle.Text = "Circle";
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonRectangle
            // 
            this.buttonRectangle.Location = new System.Drawing.Point(95, 50);
            this.buttonRectangle.Name = "buttonRectangle";
            this.buttonRectangle.Size = new System.Drawing.Size(67, 23);
            this.buttonRectangle.TabIndex = 1;
            this.buttonRectangle.Text = "Rectangle";
            this.buttonRectangle.UseVisualStyleBackColor = true;
            this.buttonRectangle.Click += new System.EventHandler(this.buttonRectangle_Click);
            // 
            // buttonPolygon
            // 
            this.buttonPolygon.Location = new System.Drawing.Point(9, 93);
            this.buttonPolygon.Name = "buttonPolygon";
            this.buttonPolygon.Size = new System.Drawing.Size(67, 23);
            this.buttonPolygon.TabIndex = 2;
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
            this.xPanderPanelChinaRegion.Name = "xPanderPanelChinaRegion";
            this.xPanderPanelChinaRegion.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelChinaRegion.Size = new System.Drawing.Size(172, 25);
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
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(170, 0);
            this.treeView1.TabIndex = 0;
            // 
            // xPanderPanelMap
            // 
            this.xPanderPanelMap.CaptionFont = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.xPanderPanelMap.Controls.Add(this.checkBoxTileHost);
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
            this.xPanderPanelMap.Expand = true;
            this.xPanderPanelMap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.xPanderPanelMap.Image = null;
            this.xPanderPanelMap.Name = "xPanderPanelMap";
            this.xPanderPanelMap.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.xPanderPanelMap.Size = new System.Drawing.Size(172, 364);
            this.xPanderPanelMap.TabIndex = 4;
            this.xPanderPanelMap.Text = "Map";
            this.xPanderPanelMap.ToolTipTextCloseIcon = null;
            this.xPanderPanelMap.ToolTipTextExpandIconPanelCollapsed = null;
            this.xPanderPanelMap.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // checkBoxTileHost
            // 
            this.checkBoxTileHost.AutoSize = true;
            this.checkBoxTileHost.Location = new System.Drawing.Point(9, 117);
            this.checkBoxTileHost.Name = "checkBoxTileHost";
            this.checkBoxTileHost.Size = new System.Drawing.Size(198, 16);
            this.checkBoxTileHost.TabIndex = 2;
            this.checkBoxTileHost.Text = "TileHost - LeafletJS web demo";
            this.checkBoxTileHost.UseVisualStyleBackColor = true;
            // 
            // buttonClearSArea
            // 
            this.buttonClearSArea.Location = new System.Drawing.Point(10, 72);
            this.buttonClearSArea.Name = "buttonClearSArea";
            this.buttonClearSArea.Size = new System.Drawing.Size(146, 23);
            this.buttonClearSArea.TabIndex = 1;
            this.buttonClearSArea.Text = "Clear selected area";
            this.buttonClearSArea.UseVisualStyleBackColor = true;
            this.buttonClearSArea.Click += new System.EventHandler(this.buttonClearSArea_Click);
            // 
            // buttonPrefetchSArea
            // 
            this.buttonPrefetchSArea.Location = new System.Drawing.Point(10, 43);
            this.buttonPrefetchSArea.Name = "buttonPrefetchSArea";
            this.buttonPrefetchSArea.Size = new System.Drawing.Size(146, 23);
            this.buttonPrefetchSArea.TabIndex = 0;
            this.buttonPrefetchSArea.Text = "Prefetch selected area";
            this.buttonPrefetchSArea.UseVisualStyleBackColor = true;
            this.buttonPrefetchSArea.Click += new System.EventHandler(this.buttonPrefetch_Click);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 517);
            this.Controls.Add(this.comboBoxRegion);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.menuStrip);
            this.Name = "MapForm";
            this.Text = "Map Form";
            this.contextMenuStripMarker.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelMap.ResumeLayout(false);
            this.panelMap.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.xPanderPanelList1.ResumeLayout(false);
            this.xPanderPanelRoute.ResumeLayout(false);
            this.xPanderPanelRoute.PerformLayout();
            this.xPanderPanelMarker.ResumeLayout(false);
            this.xPanderPanelDraw.ResumeLayout(false);
            this.xPanderPanelChinaRegion.ResumeLayout(false);
            this.xPanderPanelMap.ResumeLayout(false);
            this.xPanderPanelMap.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStripMarker;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.Button buttonStopBlink;
        private System.Windows.Forms.Button buttonBeginBlink;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonRectangle;
        private System.Windows.Forms.Button buttonPolygon;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonPolyline;
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
        private System.Windows.Forms.Button buttonMapType;
        private MapControl mapControl;
        private System.Windows.Forms.Button buttonDistance;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelChinaRegion;
        private System.Windows.Forms.TreeView treeView1;
        private BSE.Windows.Forms.XPanderPanel xPanderPanelMap;
        private System.Windows.Forms.Button buttonPrefetchSArea;
        private System.Windows.Forms.Button buttonClearSArea;
        private System.Windows.Forms.CheckBox checkBoxTileHost;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrentPos;
    }
}

