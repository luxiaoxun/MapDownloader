namespace MapDownloader
{
    partial class MapDownloadForm
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
            this.panelTool = new System.Windows.Forms.Panel();
            this.gbMapImage = new System.Windows.Forms.GroupBox();
            this.buttonMapImage = new System.Windows.Forms.Button();
            this.textBoxImageZoom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbMapDownloader = new System.Windows.Forms.GroupBox();
            this.comboBoxMapType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonMySQL = new System.Windows.Forms.RadioButton();
            this.radioButtonSQLite = new System.Windows.Forms.RadioButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.textBoxMaxZoom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMinZoom = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMap = new System.Windows.Forms.Panel();
            this.mapControl = new MapDownloader.MapControl();
            this.panelTool.SuspendLayout();
            this.gbMapImage.SuspendLayout();
            this.gbMapDownloader.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTool
            // 
            this.panelTool.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelTool.Controls.Add(this.gbMapImage);
            this.panelTool.Controls.Add(this.gbMapDownloader);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelTool.Location = new System.Drawing.Point(584, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(219, 574);
            this.panelTool.TabIndex = 0;
            // 
            // gbMapImage
            // 
            this.gbMapImage.Controls.Add(this.buttonMapImage);
            this.gbMapImage.Controls.Add(this.textBoxImageZoom);
            this.gbMapImage.Controls.Add(this.label4);
            this.gbMapImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMapImage.Location = new System.Drawing.Point(0, 377);
            this.gbMapImage.Name = "gbMapImage";
            this.gbMapImage.Size = new System.Drawing.Size(219, 197);
            this.gbMapImage.TabIndex = 5;
            this.gbMapImage.TabStop = false;
            this.gbMapImage.Text = "Map Image";
            // 
            // buttonMapImage
            // 
            this.buttonMapImage.Location = new System.Drawing.Point(58, 87);
            this.buttonMapImage.Name = "buttonMapImage";
            this.buttonMapImage.Size = new System.Drawing.Size(75, 28);
            this.buttonMapImage.TabIndex = 6;
            this.buttonMapImage.Text = "Map Image";
            this.buttonMapImage.UseVisualStyleBackColor = true;
            // 
            // textBoxImageZoom
            // 
            this.textBoxImageZoom.Location = new System.Drawing.Point(64, 48);
            this.textBoxImageZoom.Name = "textBoxImageZoom";
            this.textBoxImageZoom.Size = new System.Drawing.Size(35, 21);
            this.textBoxImageZoom.TabIndex = 3;
            this.textBoxImageZoom.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Zoom：";
            // 
            // gbMapDownloader
            // 
            this.gbMapDownloader.Controls.Add(this.comboBoxMapType);
            this.gbMapDownloader.Controls.Add(this.label1);
            this.gbMapDownloader.Controls.Add(this.groupBox2);
            this.gbMapDownloader.Controls.Add(this.listBox1);
            this.gbMapDownloader.Controls.Add(this.buttonDownload);
            this.gbMapDownloader.Controls.Add(this.textBoxMaxZoom);
            this.gbMapDownloader.Controls.Add(this.label3);
            this.gbMapDownloader.Controls.Add(this.textBoxMinZoom);
            this.gbMapDownloader.Controls.Add(this.label2);
            this.gbMapDownloader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbMapDownloader.Location = new System.Drawing.Point(0, 0);
            this.gbMapDownloader.Name = "gbMapDownloader";
            this.gbMapDownloader.Size = new System.Drawing.Size(219, 377);
            this.gbMapDownloader.TabIndex = 4;
            this.gbMapDownloader.TabStop = false;
            // 
            // comboBoxMapType
            // 
            this.comboBoxMapType.FormattingEnabled = true;
            this.comboBoxMapType.Location = new System.Drawing.Point(84, 12);
            this.comboBoxMapType.Name = "comboBoxMapType";
            this.comboBoxMapType.Size = new System.Drawing.Size(121, 20);
            this.comboBoxMapType.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "MapProvider：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonMySQL);
            this.groupBox2.Controls.Add(this.radioButtonSQLite);
            this.groupBox2.Location = new System.Drawing.Point(19, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(186, 50);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Store DataBase";
            // 
            // radioButtonMySQL
            // 
            this.radioButtonMySQL.AutoSize = true;
            this.radioButtonMySQL.Location = new System.Drawing.Point(109, 20);
            this.radioButtonMySQL.Name = "radioButtonMySQL";
            this.radioButtonMySQL.Size = new System.Drawing.Size(53, 16);
            this.radioButtonMySQL.TabIndex = 10;
            this.radioButtonMySQL.Text = "MySQL";
            this.radioButtonMySQL.UseVisualStyleBackColor = true;
            // 
            // radioButtonSQLite
            // 
            this.radioButtonSQLite.AutoSize = true;
            this.radioButtonSQLite.Checked = true;
            this.radioButtonSQLite.Location = new System.Drawing.Point(26, 20);
            this.radioButtonSQLite.Name = "radioButtonSQLite";
            this.radioButtonSQLite.Size = new System.Drawing.Size(59, 16);
            this.radioButtonSQLite.TabIndex = 9;
            this.radioButtonSQLite.TabStop = true;
            this.radioButtonSQLite.Text = "SQLite";
            this.radioButtonSQLite.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(19, 166);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(186, 196);
            this.listBox1.TabIndex = 6;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(58, 132);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 28);
            this.buttonDownload.TabIndex = 5;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            // 
            // textBoxMaxZoom
            // 
            this.textBoxMaxZoom.Location = new System.Drawing.Point(140, 105);
            this.textBoxMaxZoom.Name = "textBoxMaxZoom";
            this.textBoxMaxZoom.Size = new System.Drawing.Size(35, 21);
            this.textBoxMaxZoom.TabIndex = 4;
            this.textBoxMaxZoom.Text = "20";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "---";
            // 
            // textBoxMinZoom
            // 
            this.textBoxMinZoom.Location = new System.Drawing.Point(69, 105);
            this.textBoxMinZoom.Name = "textBoxMinZoom";
            this.textBoxMinZoom.Size = new System.Drawing.Size(35, 21);
            this.textBoxMinZoom.TabIndex = 2;
            this.textBoxMinZoom.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Zoom：";
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.mapControl);
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(584, 574);
            this.panelMap.TabIndex = 3;
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
            this.mapControl.Size = new System.Drawing.Size(584, 574);
            this.mapControl.TabIndex = 0;
            this.mapControl.Zoom = 0D;
            // 
            // MapDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 574);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelTool);
            this.Name = "MapDownloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelTool.ResumeLayout(false);
            this.gbMapImage.ResumeLayout(false);
            this.gbMapImage.PerformLayout();
            this.gbMapDownloader.ResumeLayout(false);
            this.gbMapDownloader.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTool;
        private System.Windows.Forms.Panel panelMap;
        private MapControl mapControl;
        private System.Windows.Forms.GroupBox gbMapImage;
        private System.Windows.Forms.GroupBox gbMapDownloader;
        private System.Windows.Forms.ComboBox comboBoxMapType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonMySQL;
        private System.Windows.Forms.RadioButton radioButtonSQLite;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.TextBox textBoxMaxZoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMinZoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonMapImage;
        private System.Windows.Forms.TextBox textBoxImageZoom;
        private System.Windows.Forms.Label label4;
    }
}

