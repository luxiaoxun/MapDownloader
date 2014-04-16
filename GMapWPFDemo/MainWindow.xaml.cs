using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace GMapWPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.google.com.hk");
            }
            catch
            {
                mapControl.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET Demo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            mapControl.MapProvider = GMapProviders.GoogleChinaMap; //google china 地图
            mapControl.MinZoom = 2;  //最小缩放
            mapControl.MaxZoom = 17; //最大缩放
            mapControl.Zoom = 5;     //当前缩放
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = MouseButton.Left; //左键拖拽地图
            mapControl.Position = new PointLatLng(32.064, 118.704); //地图中心位置：南京

            mapControl.OnMapZoomChanged += new MapZoomChanged(mapControl_OnMapZoomChanged);
            mapControl.MouseLeftButtonDown += new MouseButtonEventHandler(mapControl_MouseLeftButtonDown);
        }

        void mapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(mapControl);
            PointLatLng point = mapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
            GMapMarker marker = new GMapMarker(point);
            mapControl.Markers.Add(marker);
        }

        void mapControl_OnMapZoomChanged()
        {
        }
    }
}
