using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using GMap.NET.MapProviders;
using AsterixDecoder;
using System.Drawing;
using System.Resources;

namespace AsterixDisplay
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SimulacionPanel : Window
    {
        double[] MLATcoords = new double[2];
        
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer(); //Para el tick del timer
        public SimulacionPanel()
        {
            InitializeComponent();
            MLATcoords[0] = 41.297063;
            MLATcoords[1] = 2.078447;
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.MinZoom = 14;
            mapView.MaxZoom = 16;
            // whole world zoom
            mapView.Zoom = 2;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.Position = new GMap.NET.PointLatLng(MLATcoords[0], MLATcoords[1]);

            GMap.NET.WindowsPresentation.GMapMarker marker = new GMap.NET.WindowsPresentation.GMapMarker(new PointLatLng(MLATcoords[0], MLATcoords[1]));
            marker.Position = new PointLatLng(MLATcoords[0], MLATcoords[1]);
            marker.Shape = new System.Windows.Controls.Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/airplane.jpg"))
                
            };

            mapView.Markers.Add(marker);
            mapView.ReloadMap();
        }
    }
}
