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
        double[] SMRcoords = new double[2];
        int secact;
        int minact;
        int horaact;
        int contador;
        List<CAT20> CAT20s;
        List<CAT10> CAT10s;
        List<CAT21> CAT21s;
        int iscat;
        string[] tiempo;
        int speed;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer(); //Para el tick del timer
        public SimulacionPanel(List<CAT20> cat20s, List<CAT10> cat10s, List<CAT21> cat21s, int cat)
        {
            InitializeComponent();
            MLATcoords[0] = 41.29706278;
            MLATcoords[1] = 2.078447222;
            SMRcoords[0] = 41.29561833;
            SMRcoords[1] = 2.095114167;
            contador = 0;
            this.CAT20s = cat20s;
            this.CAT10s = cat10s;
            this.CAT21s = cat21s;
            this.iscat = cat;
            this.speed = 1000;

            //Update clock with time of the first package
            if (cat == 20) { clockUpdate(cat20s[0].TOD.Split(':')); }
            if (cat == 10) { clockUpdate(cat10s[0].TimeOfDay.Split(':')); }
            //if (cat == 21) { clockUpdate(cat21s[0].Tod); }
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            mapView.MapProvider = OpenStreetMapProvider.Instance;
            mapView.MinZoom = 8;
            mapView.MaxZoom = 16;
            // whole world zoom
            mapView.Zoom = 13;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.Position = new PointLatLng(MLATcoords[0], MLATcoords[1]);
        }
        private PointLatLng fromXYtoLatLongMLAT(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double φ1 = MLATcoords[0] * (Math.PI / 180);
            double λ1 = MLATcoords[1] * (Math.PI / 180);
            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(d / R) + Math.Cos(φ1) * Math.Sin(d / R) * Math.Cos(brng));
            var λ2 = λ1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(φ1), Math.Cos(d / R) - Math.Sin(φ1) * Math.Sin(φ2));

            PointLatLng coordinates = new PointLatLng(φ2 * (180 / Math.PI), λ2 * (180 / Math.PI));
            return coordinates;
        }

        private PointLatLng fromXYtoLatLongSMR(double X, double Y)
        {
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double φ1 = SMRcoords[0] * (Math.PI / 180);
            double λ1 = SMRcoords[1] * (Math.PI / 180);
            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(d / R) + Math.Cos(φ1) * Math.Sin(d / R) * Math.Cos(brng));
            var λ2 = λ1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(φ1), Math.Cos(d / R) - Math.Sin(φ1) * Math.Sin(φ2));

            PointLatLng coordinates = new PointLatLng(φ2 * (180 / Math.PI), λ2 * (180 / Math.PI));
            return coordinates;
        }

        private void addMarkerMLAT(double X, double Y, string callsign)
        {
            GMapMarker marker = new GMapMarker((fromXYtoLatLongMLAT(X, Y)));
            marker.Position = (fromXYtoLatLongMLAT(X, Y));
            if (callsign != null)
            {
                marker.Shape = new System.Windows.Controls.Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/airplane1.png"))
                };
                marker.ZIndex = 0; //Indice = 0, airplane
                marker.Offset = new System.Windows.Point(-7.5, -7.5);
            }
            if (callsign == null)
            {
                marker.Shape = new System.Windows.Controls.Image
                {
                    Width = 30,
                    Height = 30,
                    Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/unidentified.png"))

                };
                marker.ZIndex = 1; //Index = 1, non airplane
                marker.Offset = new System.Windows.Point(-15, -15);
            }

            checkVisible(marker);
            mapView.Markers.Add(marker);
        }

        private void addMarkerSMR(double X, double Y, string callsign)
        {
            GMapMarker marker = new GMapMarker((fromXYtoLatLongSMR(X, Y)));
            marker.Position = (fromXYtoLatLongSMR(X, Y));
            if (callsign != null)
            {
                marker.Shape = new System.Windows.Controls.Image
                {
                    Width = 15,
                    Height = 15,
                    Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/airplane1.png"))
                };
                marker.ZIndex = 0; //Indice = 0, airplane
                marker.Offset = new System.Windows.Point(-7.5, -7.5);
            }
            if (callsign == null)
            {
                marker.Shape = new System.Windows.Controls.Image
                {
                    Width = 30,
                    Height = 30,
                    Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/unidentified.png"))

                };
                marker.ZIndex = 1; //Index = 1, non airplane
                marker.Offset = new System.Windows.Point(-15, -15);
            }

            checkVisible(marker);
            mapView.Markers.Add(marker);
        }

        private void playbut_Click(object sender, RoutedEventArgs e)
        {
            if (this.iscat == 20) { dispatcherTimer.Tick += dispatcherTimer_TickCAT20; }
            if (this.iscat == 10) { dispatcherTimer.Tick += dispatcherTimer_TickCAT10; }
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, this.speed);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_TickCAT20(object sender, EventArgs e) //lo que hace cada interval del timer
        {
            checkActual();
            //Retreive the packages which have the same time (same second)
            Boolean t = true;
            while (t == true)
            {           
                    CAT20 cat20 = CAT20s[this.contador];
                    this.tiempo = cat20.TOD.Split(':');
                    if (Convert.ToInt32(tiempo[2]) == secact)
                    {
                        addMarkerMLAT(cat20.X, cat20.Y, cat20.callsign);
                        this.contador++;
                        if (this.contador >= 200)
                        {
                            mapView.Markers[contador - 199].Clear();
                        }
                    }                
                
                else
                {
                    t = false;
                    secact++;
                }
                clockUpdate(this.tiempo);
            }
        }

        private void dispatcherTimer_TickCAT10(object sender, EventArgs e)
        {
            checkActual();
            Boolean t = true;
            while (t == true)
            {
                CAT10 cat10 = CAT10s[this.contador];
                this.tiempo = cat10.TimeOfDay.Split(':');
                if (Convert.ToInt32(tiempo[2]) == secact)
                {
                    if (cat10.TYP == "PSR") { addMarkerSMR(Convert.ToDouble(cat10.Xcomponent), Convert.ToDouble(cat10.Ycomponent), cat10.TargetID); }
                    if (cat10.TYP == "Mode S MLAT") { addMarkerMLAT(Convert.ToDouble(cat10.Xcomponent), Convert.ToDouble(cat10.Ycomponent), cat10.TargetID); }
                    this.contador++;
                    if (this.contador >= 200)
                    {
                        mapView.Markers[contador - 199].Clear();
                    }
                }
                else
                {
                    t = false;
                    secact++;
                }
                clockUpdate(this.tiempo);
            }
        }

        private void dispatcherTimer_TickCAT21(object sender, EventArgs e)
        {
            //implementar CAT21
        }

        private void clockUpdate(string[] TOD)
        {
            string[] TODexp = TOD;

            this.horaact = Convert.ToInt32(TODexp[0]);
            this.minact = Convert.ToInt32(TODexp[1]);
            this.secact = Convert.ToInt32(TODexp[2]);

            clockbox.Text = (TODexp[0] + ":" + TODexp[1] + ":" + TODexp[2]);
        }

        private void stopbut_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void checktrail_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checktrail.IsChecked == true)
            {
                foreach (GMapMarker marker in mapView.Markers)
                {
                    if (marker.Shape != null)
                    {
                        marker.Shape.Visibility = Visibility.Visible;
                    }

                }
            }
            if (checktrail.IsChecked == false)
            {
                foreach (GMapMarker marker in mapView.Markers)
                {
                    if (marker.Shape != null)
                    {
                        marker.Shape.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void checkairplanes_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checkairplanes.IsChecked == true)
            {
                foreach (GMapMarker marker in mapView.Markers)
                {
                    if (marker.Shape != null)
                    {
                        if (marker.ZIndex == 0)
                            marker.Shape.Visibility = Visibility.Visible;
                        else if (marker.ZIndex == 1)
                            marker.Shape.Visibility = Visibility.Collapsed;
                    }

                }
            }
            if (checkairplanes.IsChecked == false)
            {
                foreach (GMapMarker marker in mapView.Markers)
                {
                    if (marker.Shape != null)
                    {
                        marker.Shape.Visibility = Visibility.Visible;
                    }
                }
            }
        
        }

        private void checkVisible(GMapMarker marker)
        {
            if (checkairplanes.IsChecked == true)
            {
                if (marker.ZIndex == 1) { marker.Shape.Visibility = Visibility.Collapsed; }
            }
            else { marker.Shape.Visibility = Visibility.Visible; }
        }
 
        private void checkActual()
        {
            if (checktrail.IsChecked == false)
            {
                foreach (GMapMarker marker in mapView.Markers)
                {
                    if (marker.Shape != null)
                    {
                        marker.Shape.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void speed1but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 1000;
        }

        private void speed4but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 250;
        }

        private void speed2but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 500;
        }
    }
}