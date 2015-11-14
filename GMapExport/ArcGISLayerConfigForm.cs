using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMapProvidersExt;
using GMapProvidersExt.Baidu;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapExport
{
    public partial class ArcGISLayerConfigForm : Form
    {
        private ExportParameter exportParameter;

        public ArcGISLayerConfigForm()
        {
            InitializeComponent();

            this.comboBoxMapProvider.Items.Add(GMapProvidersExt.AMap.AMapProvider.Instance);
            this.comboBoxMapProvider.Items.Add(GMapProvidersExt.Baidu.BaiduMapProvider.Instance);
            this.comboBoxMapProvider.Items.Add(GMapProvidersExt.Tencent.TencentMapProvider.Instance);
            this.comboBoxMapProvider.Items.Add(GMapProviders.GoogleChinaMap);
            this.comboBoxMapProvider.DisplayMember = "CnName";
            this.comboBoxMapProvider.SelectedIndex = 0;

            this.comboBoxExportType.Items.Add("XY切片");
            this.comboBoxExportType.Items.Add("YX切片");
            this.comboBoxExportType.Items.Add("ArcGIS切片");
            this.comboBoxExportType.Items.Add("TMS切片");
            this.comboBoxExportType.Items.Add("百度切片");
            this.comboBoxExportType.SelectedIndex = 0;
        }

        public ExportParameter GetExportParameter()
        {
            return exportParameter;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            exportParameter = new ExportParameter();

            GMapProvider mapProvider = this.comboBoxMapProvider.SelectedItem as GMapProvider;
            if (mapProvider != null)
            {
                ExportType exportType = ExportType.DefaultXYTile;
                int index = this.comboBoxExportType.SelectedIndex;
                switch (index)
                {
                    case 0:
                        exportType = ExportType.DefaultXYTile;
                        break;
                    case 1:
                        exportType = ExportType.DefaultYXTile;
                        break;
                    case 2:
                        exportType = ExportType.ArcGISTile;
                        break;
                    case 3:
                        exportType = ExportType.TMSTile;
                        break;
                    case 4:
                        exportType = ExportType.BaiduTile;
                        break;
                }

                if (exportType == ExportType.BaiduTile && !(mapProvider is BaiduMapProviderBase))
                {
                    MessageBox.Show("只有百度地图才能导出为百度切片");
                    return;
                }

                int minZ = int.Parse(this.textBoxMinZoom.Text);
                int maxZ = int.Parse(this.textBoxMaxZoom.Text);
                minZ = minZ <= 0 ? 1 : minZ;
                int providerMaxZoom = mapProvider.MaxZoom.Value;
                maxZ = maxZ >= providerMaxZoom ? providerMaxZoom : maxZ;

                string rootPath = this.textBox1.Text;
                if (string.IsNullOrEmpty(rootPath))
                {
                    MessageBox.Show("导出路劲不能为空！");
                }
                else
                {
                    exportParameter.ExportPath = rootPath;
                    exportParameter.ExportRect = mapProvider.Projection.Bounds;
                    exportParameter.MinZoom = minZ;
                    exportParameter.MaxZoom = maxZ;
                    exportParameter.MapProvider = mapProvider;
                    exportParameter.ExportType = exportType;

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult res =  folderDlg.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = folderDlg.SelectedPath;
            }
        }
    }
}
