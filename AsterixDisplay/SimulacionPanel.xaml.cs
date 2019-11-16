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
using System.Data;

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
        List<Flight> flights;
        DataTable flightdata;
        int iscat;
        string[] tiempo;
        int speed;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer(); //Para el tick del timer
        public SimulacionPanel(List<CAT20> cat20s, List<CAT10> cat10s, List<CAT21> cat21s, int cat, List<Flight> listflights)
        {
            InitializeComponent();
            MLATcoords[0] = 41.29706278;
            MLATcoords[1] = 2.078447222;
            SMRcoords[0] = 41.29561833;
            SMRcoords[1] = 2.095114167;
            contador = 0;
            this.flightdata = new DataTable();
            this.CAT20s = cat20s;
            this.CAT10s = cat10s;
            this.CAT21s = cat21s;
            this.flights = listflights;
            this.iscat = cat;
            this.speed = 1000;

            //Set up the table:
            flightdata.Columns.Add(new DataColumn("CallSign"));
            flightdata.Columns.Add(new DataColumn("Track Number"));
            if (this.iscat == 21) { flightdata.Columns.Add(new DataColumn("Position in WSG-84 [Lat, Lng]")); }
            else { flightdata.Columns.Add(new DataColumn("Local Position [X, Y]")); }
            flightdata.Columns.Add(new DataColumn("Flight Level"));

            resetbut.Click += resetbut_Click; ;
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            mapView.MapProvider = OpenStreetMapProvider.Instance;
            mapView.MinZoom = 7;
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
            double brng = Math.Atan2(Y, - X) - (Math.PI / 2);
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
                marker.ZIndex = 0; // Index 0 == Aeronave
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
                marker.ZIndex = 1; //Index 1 == non airplane
                marker.Offset = new System.Windows.Point(-15, -15);
            }

            checkVisible(marker);
            mapView.Markers.Add(marker);
        }


        private void addMarkerSMR(double X, double Y, string callsign)
        {
            GMapMarker marker = new GMapMarker((fromXYtoLatLongSMR(X, Y)));
            marker.Position = (fromXYtoLatLongSMR(X, Y));
            marker.Shape = new Ellipse
            {
                StrokeThickness = 2,
                Stroke = System.Windows.Media.Brushes.Black,
                Height = 15,
                Width = 15,
                Fill = System.Windows.Media.Brushes.Black
            };
            marker.ZIndex = 0; //Indice = 0, airplane
            marker.Offset = new System.Windows.Point(-7.5, -7.5);

            checkVisible(marker);
            mapView.Markers.Add(marker);
        }

        private void addMarkerADSB(double lat, double lng, string callsing)
        {
            GMapMarker marker = new GMapMarker(new PointLatLng(lat, lng));
            marker.Position = new PointLatLng(lat, lng);
            marker.Shape = new System.Windows.Controls.Image
            {
                Width = 15,
                Height = 15,
                Source = new BitmapImage(new System.Uri("pack://application:,,,/Resources/airplane1.png"))
            };
            marker.ZIndex = 0; // Index 0 == Aeronave
            marker.Offset = new System.Windows.Point(-7.5, -7.5);

            checkVisible(marker);
            mapView.Markers.Add(marker);
        }

        private void playbut_Click(object sender, RoutedEventArgs e)
        {
            //dispatcherTimer.Tick += dispatcherTimer_TickFlightCAT20;
            if (this.iscat == 20) { dispatcherTimer.Tick += dispatcherTimer_TickCAT20; }
            if (this.iscat == 10) { dispatcherTimer.Tick += dispatcherTimer_TickCAT10; }
            if (this.iscat == 21) { dispatcherTimer.Tick += dispatcherTimer_TickCAT21; }
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
                    if (mapView.Markers.Count >= 200)
                    {
                        mapView.Markers[mapView.Markers.Count - 200].Clear();
                    }
                    
                }
                else
                {
                    t = false;
                    secact++;
                }
                clockUpdate(this.tiempo);
            }
            flightdata.Rows.Clear();
            fillGridFlightsCAT20();
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
                    if (mapView.Markers.Count >= 200)
                    {
                        mapView.Markers[mapView.Markers.Count - 200].Clear();
                    }
                }
                else
                {
                    t = false;
                    secact++;
                }
                
                clockUpdate(this.tiempo);
            }
            flightdata.Rows.Clear();
            fillGridFlightsCAT10();
        }

        private void dispatcherTimer_TickCAT21(object sender, EventArgs e)
        {
            checkActual();
            Boolean t = true;
            while (t == true)
            {
                CAT21 cat21 = CAT21s[this.contador];
                this.tiempo = cat21.TOD.Split(':');
                if (Convert.ToInt32(tiempo[2]) == secact)
                {
                    addMarkerADSB(cat21.LatitudeWGS, cat21.LongitudeWGS, cat21.TargetID);
                    this.contador++;
                    if (mapView.Markers.Count >= 200)
                    {
                        mapView.Markers[mapView.Markers.Count - 200].Clear();
                    }
                }
                else
                {
                    t = false;
                    secact++;
                }

                clockUpdate(this.tiempo);
            }
            flightdata.Rows.Clear();
            fillGridFlightsCAT21();
        }

        /*
        private void dispatcherTimer_TickFlightCAT20(object sender, EventArgs e)
        {
            string[] timepack = this.tiempo;

            foreach (Flight f in flights)
            {
                int contadorTOD = 0;
                bool t = true;
                while (t == true && contadorTOD < f.TODs.Count())
                {
                    if(Convert.ToInt32(timepack[2]) == secact && Convert.ToInt32(timepack[1]) == minact && Convert.ToInt32(timepack[0]) == horaact)
                    {
                        this.tiempo = f.TODs[contadorTOD].Split(':');
                        addMarkerMLAT(f.Xs[contadorTOD], f.Ys[contadorTOD], f.callsign, Convert.ToInt32(f.tracknumber));
                        t = false;
                    }
                    contadorTOD++;
                }
            }
            this.tiempo[2] = Convert.ToString(Convert.ToInt32(this.tiempo[2]) + 1);
            clockUpdate(this.tiempo);
            
        }
        */

        private void clockUpdate(string[] TOD)
        {
            string[] TODexp = TOD;

            this.horaact = Convert.ToInt32(TODexp[0]);
            this.minact = Convert.ToInt32(TODexp[1]);
            this.secact = Convert.ToInt32(TODexp[2]);
            
            TimeSpan time = TimeSpan.FromSeconds(this.horaact*3600 + this.minact*60 + this.secact);

            string tod = time.ToString(@"hh\:mm\:ss");

            clockbox.Text = tod;
        }

        private void stopbut_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void checktrail_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checktrail.IsChecked == true)
            {
                for (int i = 0; i < mapView.Markers.Count; i++)
                {
                    if (mapView.Markers[i].Shape != null)
                    {
                        mapView.Markers[i].Shape.Visibility = Visibility.Visible;
                    }
                }
            }
            if (checktrail.IsChecked == false)
            {
                for (int i = 0; i < mapView.Markers.Count; i++)
                {
                    if (mapView.Markers[i].Shape != null)
                    {
                        mapView.Markers[i].Shape.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void checkairplanes_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checkairplanes.IsChecked == true)
            {
                for (int i = 0; i < mapView.Markers.Count; i++)
                {
                    if (mapView.Markers[i].Shape != null)
                    {
                        if (mapView.Markers[i].ZIndex == 0)
                            mapView.Markers[i].Shape.Visibility = Visibility.Visible;
                        else if (mapView.Markers[i].ZIndex == 1)
                            mapView.Markers[i].Shape.Visibility = Visibility.Collapsed;
                    }
                }
            }
            if (checkairplanes.IsChecked == false)
            {
                for (int i = 0; i < mapView.Markers.Count; i++)
                {
                    if (mapView.Markers[i].Shape != null)
                    {
                        mapView.Markers[i].Shape.Visibility = Visibility.Visible;
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
                for (int i = 0; i < mapView.Markers.Count; i++)
                {
                    if (mapView.Markers[i].Shape != null)
                    {
                        mapView.Markers[i].Shape.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void speed1but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 1000;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, this.speed);
        }

        private void speed4but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 250;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, this.speed);
        }

        private void speed2but_Click(object sender, RoutedEventArgs e)
        {
            this.speed = 500;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, this.speed);
        }

        private void resetbut_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            mapView.Markers.Clear();
            this.contador = 0;
            if(this.CAT10s.Count != 0) { clockUpdate(CAT10s[0].TimeOfDay.Split(':')); }
            if (this.CAT20s.Count != 0) { clockUpdate(CAT20s[0].TOD.Split(':')); }
            if (this.CAT21s.Count != 0) { clockUpdate(CAT21s[0].TOD.Split(':')); }
        }

        private void fillGridFlightsCAT20()
        {
            foreach (Flight f in flights)
            {
                int i = 0;
                int pf = 0;
                bool act = false;
                while (i < f.TODs.Count())
                {
                    string[] t = f.TODs[i].Split(':');
                    if (t[0] == this.tiempo[0] && t[1] == this.tiempo[1] && t[2] == this.tiempo[2])
                    {
                        pf = i;
                        i = f.TODs.Count();
                        act = true;
                    }
                    i++;
                }
                if (act == true) { flightdata.Rows.Add(f.callsign, f.tracknumber, "[" + f.Xs[pf] + "," + f.Ys[pf] + "]", "FL " + f.fls[pf]); }
            }
            gridFlights.ItemsSource = flightdata.DefaultView;
        }

        private void fillGridFlightsCAT10()
        {
            foreach (Flight f in flights)
            {
                int i = 0;
                int pf = 0;
                bool act = false;
                while (i < f.TODs.Count())
                {
                    string[] t = f.TODs[i].Split(':');
                    if (t[0] == this.tiempo[0] && t[1] == this.tiempo[1] && t[2] == this.tiempo[2])
                    {
                        pf = i;
                        i = f.TODs.Count();
                        act = true;
                    }
                    i++;
                }
                if (act == true) { flightdata.Rows.Add(f.callsign, f.tracknumber, "[" + f.Xs[pf] + "," + f.Ys[pf] + "]", "FL " + f.fls[pf]); }
            }
            gridFlights.ItemsSource = flightdata.DefaultView;
        }

        private void fillGridFlightsCAT21()
        {
            foreach (Flight f in flights)
            {
                int i = 0;
                int pf = 0;
                bool act = false;
                while (i < f.TODs.Count())
                {
                    string[] t = f.TODs[i].Split(':');
                    if (t[0] == this.tiempo[0] && t[1] == this.tiempo[1] && t[2] == this.tiempo[2])
                    {
                        pf = i;
                        i = f.TODs.Count();
                        act = true;
                    }
                    i++;
                }
                if (act == true) { flightdata.Rows.Add(f.callsign, f.tracknumber, "[" + f.lats[pf] + "," + f.lngs[pf] + "]", "FL " + f.fls[pf]); }
            }
            gridFlights.ItemsSource = flightdata.DefaultView;
        }

        private void elpratbut_Click(object sender, RoutedEventArgs e)
        {
            mapView.Zoom = 13;
            mapView.Position = new PointLatLng(MLATcoords[0], MLATcoords[1]);
        }

        private void bcnbut_Click(object sender, RoutedEventArgs e)
        {
            mapView.Zoom = 11;
            mapView.Position = new PointLatLng(MLATcoords[0], MLATcoords[1]);
        }

        private void catbut_Click(object sender, RoutedEventArgs e)
        {
            mapView.Zoom = 8;
            mapView.Position = new PointLatLng(MLATcoords[0], MLATcoords[1]);
        }
    }
}