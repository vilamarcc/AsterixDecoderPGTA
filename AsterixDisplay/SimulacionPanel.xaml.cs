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
        int secact;
        int minact;
        int horaact;
        int contador;
        
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer(); //Para el tick del timer
        public SimulacionPanel(List<CAT20> cat20s)
        {
            InitializeComponent();
            MLATcoords[0] = 41.29706278;
            MLATcoords[1] = 2.078447222;
            contador = 0;

            //Update clock with time of the first package
            clockUpdate(cat20s[0].TOD);
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.MinZoom = 13;
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
        }
        private PointLatLng fromXYtoLatLongMLAT(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt(X * X + Y * Y);
            double brng = Math.Atan2(Y, X);
            double φ1 = MLATcoords[0] * (Math.PI / 180);
            double λ1 = MLATcoords[1] * (Math.PI / 180);
            double φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(d / R) + Math.Cos(φ1) * Math.Sin(d / R) * Math.Cos(brng));
            double λ2 = λ1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(φ1), Math.Cos(d / R) - Math.Sin(φ1) * Math.Sin(φ2));

            PointLatLng coordinates = new PointLatLng(φ2 * (180 / Math.PI), λ2 * (180 / Math.PI));
            return coordinates;
        }

        private void addMarker(double X, double Y)
        {
            GMap.NET.WindowsPresentation.GMapMarker marker = new GMap.NET.WindowsPresentation.GMapMarker((fromXYtoLatLongMLAT(X, Y)));
            marker.Position =(fromXYtoLatLongMLAT(X,Y));
            marker.Shape = new System.Windows.Controls.Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/airplane1.png"))

            };
            marker.Offset = new System.Windows.Point(-7.5, -7.5);

            mapView.Markers.Add(marker);
        }

        private void playbut_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e) //lo que hace cada interval del timer
        {
            //Retreive the packages which have the same time (same second)


        }

            private void clockUpdate(string TOD)
        {
            string[] TODexp = TOD.Split(':');

            this.horaact = Convert.ToInt32(TODexp[0]);
            this.minact = Convert.ToInt32(TODexp[1]);
            this.secact = Convert.ToInt32(TODexp[2]);

            clockbox.Text = (TODexp[0] + ":" + TODexp[1] + ":" + TODexp[2]);
        }
    }
}
