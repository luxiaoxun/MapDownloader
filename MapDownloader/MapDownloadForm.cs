using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;

namespace MapDownloader
{
    public partial class MapDownloadForm : Form
    {
        public MapDownloadForm()
        {
            InitializeComponent();

            InitMap();
        }

        private void InitMap()
        {
            mapControl.ShowCenter = false;
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left;
            mapControl.CacheLocation = Environment.CurrentDirectory + "\\MapCache\\"; //缓存位置

            mapControl.MapProvider = GMapProviders.GoogleChinaMap; ;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;

            comboBoxMapType.ValueMember = "Name";
            comboBoxMapType.DataSource = GMapProviders.List;
            comboBoxMapType.SelectedItem = mapControl.MapProvider;

            this.comboBoxMapType.SelectedValueChanged += new EventHandler(comboBoxMapType_SelectedValueChanged);

            this.radioButtonMySQL.CheckedChanged += new EventHandler(radioButtonMySQL_CheckedChanged);
            this.radioButtonSQLite.CheckedChanged += new EventHandler(radioButtonSQLite_CheckedChanged);
        }

        void radioButtonSQLite_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonSQLite.Checked)
            {
                GMap.NET.CacheProviders.SQLitePureImageCache cache = new GMap.NET.CacheProviders.SQLitePureImageCache();
                mapControl.Manager.PrimaryCache = cache;
            }
        }

        void radioButtonMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonMySQL.Checked)
            {
                GMap.NET.CacheProviders.MySQLPureImageCache cache = new GMap.NET.CacheProviders.MySQLPureImageCache();
                cache.ConnectionString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";
                mapControl.Manager.PrimaryCache = cache;
            }
        }

        void comboBoxMapType_SelectedValueChanged(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = this.comboBoxMapType.SelectedItem as GMapProvider;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();

            RectLatLng area = mapControl.SelectedArea;
            if (!area.IsEmpty)
            {
                try
                {
                    int minZ = int.Parse(this.textBoxMinZoom.Text);
                    int maxZ = int.Parse(this.textBoxMaxZoom.Text);
                    minZ = minZ <= 0 ? 1 : minZ;
                    maxZ = maxZ >= mapControl.MaxZoom ? mapControl.MaxZoom : maxZ;

                    for (int i = minZ; i <= maxZ; i++)
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                        MapAreaInfo mapAreaInfo = new MapAreaInfo(i,area);
                        worker.RunWorkerAsync(mapAreaInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int zoom = (int)e.Result;
            //this.toolStripStatusLabel1.Text = "Zoom为" + zoom + "的地图下载完成";
            string text = "Zoom为" + zoom + "的地图下载完成";
            this.listBox1.Items.Add(text);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            MapAreaInfo mapAreaInfo = e.Argument as MapAreaInfo;
            if (mapAreaInfo != null)
            {
                using (TilePrefetcher obj = new TilePrefetcher())
                {
                    obj.Shuffle = mapControl.Manager.Mode != AccessMode.CacheOnly;
                    obj.Owner = this;
                    obj.ShowCompleteMessage = false;
                    obj.ShowProgressMessage = false;
                    obj.Start(mapAreaInfo.Area, mapAreaInfo.Zoom, mapControl.MapProvider, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 100, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 1);
                    e.Result = mapAreaInfo.Zoom;
                }
            }
        }
    }
}
