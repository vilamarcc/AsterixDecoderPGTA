using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsterixDecoder;
using System.IO;

namespace AsterixDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

        string fichero; //fichero que leemos

        //tablas con los paquetes
        DataTable dataCAT20;
        DataTable dataCAT10;
        DataTable dataCAT21;

        DataTable dataCATsearch;

        Fichero f; //clase fichero que abrimos

        int cat; //CAT de la tabla que estamos mostrando

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void cargarbut_Click(object sender, RoutedEventArgs e) // botón OPEN FILE
        {
            //mostramos la barra de progreso
            var progress = new Progress<int>(value => progressbar1.Value = value);
            progressbar1.Visibility = Visibility.Visible;

            //que sea un archivo asterix
            ofd.Filter = "AST |*.ast";
            if (ofd.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    //leemos fichero
                    this.fichero = ofd.FileName;
                    f = new Fichero(this.fichero);
                    ((IProgress<int>)progress).Report(10);

                    //leemos los paquetes 
                    f.Read();
                    ((IProgress<int>)progress).Report(70);

                    //decimos que se ha cargado ya el fichero
                    MessageBox.Show("Archivo cargado");
                    ((IProgress<int>)progress).Report(75);

                    //guardamos las tablas y mostramos la que toque (this.cat)
                    this.dataCAT20 = f.getTablaCAT20();
                    this.dataCAT10 = f.getTablaCAT10();
                    this.dataCAT21 = f.getTablaCAT21();
                    ((IProgress<int>)progress).Report(85);

                    //cat que vamos a mostrar por defecto 
                        //si el archivo tiene más de una cat se mostrará la más pequeña (por defecto), pero luego se puede elegir ver otra
                    if (f.ComprobarCAT10() == true)
                        this.cat = 10;
                    else if (f.ComprobarCAT20() == true)
                        this.cat = 20;
                    else if (f.ComprobarCAT21() == true)
                        this.cat = 21;
                    ((IProgress<int>)progress).Report(100);
                });

                //enseñamos la tabla
                if (this.cat == 10)
                {
                    cat10_butt.IsChecked = true;
                    this.fillgridwithdata(this.dataCAT10);
                }
                if (this.cat == 20)
                {
                    cat20_butt.IsChecked = true;
                    this.fillgridwithdata(this.dataCAT20);
                }
                if (this.cat == 21)
                {
                    cat21_butt.IsChecked = true;
                    this.fillgridwithdata(this.dataCAT21);
                }

                progressbar1.Visibility = Visibility.Hidden;
            }
        }

        public void fillgridwithdata(DataTable data) //enseña en pantalla la tabla que le damos como input
        {
            gridCAT.DataContext = data.DefaultView;
            gridCAT.Items.Refresh();
        }

        void ClikDataGrid(object sender, RoutedEventArgs e) //al clicar en una celda de la tabla
        {
            //cogemos la celda
            DataGridCell cell = (DataGridCell)sender;
            int c = cell.Column.DisplayIndex; //número de columna
            DataGridRow r2 = DataGridRow.GetRowContainingElement(cell);
            int fil = r2.GetIndex(); //número de fila

            if(this.cat==10)
            {
                //cojo el paquete
                CAT10 paquete = f.getCAT10(fil);

                //expandimos información:
                if (c == 5) // Target Report - Data Characteristics
                    MessageBox.Show(paquete.DataCharacteristics);
                if (c == 13) // Track Status
                    MessageBox.Show(paquete.TrackStatus);
                if (c == 27) // Presence
                    MessageBox.Show(paquete.Presence);
                if (c == 17) // Mode S MB Data
                    MessageBox.Show(paquete.ModeS);
                if (c == 23) // System Status
                    MessageBox.Show(paquete.SystemStatus);

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT10(fil);
            }
            if (this.cat == 20)
            {
                CAT20 paquete = f.getCAT20(fil);
                //expandimos información:
                if (c == 3)
                    MessageBox.Show(paquete.getTargetReportDescriptortoString());
                if (c == 9) //Track status
                    MessageBox.Show(paquete.getTrackStatusToString());
                if (c == 21) //Pos Accuracy
                    MessageBox.Show(paquete.getPositionAccuracyToString());

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT20(fil);
            }
            if (this.cat == 21)
            {
                //expandimos información:
                

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT21(fil);
            }
        }

        private void filldataexpandedCAT21(int index)
        {

        }
        private void filldataexpandedCAT10(int index)
        {
            DataTable expanded = new DataTable();
            CAT10 cat10exp = f.getCAT10(index);

            expanded.Columns.Add(new DataColumn());
            expanded.Columns.Add(new DataColumn());

            expanded.Rows.Add("Package #", index + 1);
            if (cat10exp.MessageType != null) { expanded.Rows.Add("Message Type", cat10exp.MessageType); }
            if (cat10exp.TrackNumber != null) { expanded.Rows.Add("Track Number", cat10exp.TrackNumber); }
            if (cat10exp.DataSourceID != null) { expanded.Rows.Add("Data Source ID", cat10exp.DataSourceID); }
            if (cat10exp.TYP != null) { expanded.Rows.Add("Data Type", cat10exp.TYP); }
            if (cat10exp.DataCharacteristics != null) { expanded.Rows.Add("Data Characteristics", cat10exp.DataCharacteristics); }
            if (cat10exp.TimeOfDay != null) { expanded.Rows.Add("Time of Day (UTC)", cat10exp.TimeOfDay); }
            if (cat10exp.positionWGS != null) { expanded.Rows.Add("Position in WGS-84\n[Latitude, Longitude]", cat10exp.positionWGS); }
            if (cat10exp.positionPolar != null) { expanded.Rows.Add("Position in Polar\n[Distance, Angle]", cat10exp.positionPolar); }
            if (cat10exp.positionCartesian != null) { expanded.Rows.Add("Position in Cartesian\n[X, Y]", cat10exp.positionCartesian); }
            if (cat10exp.velocityPolar != null) { expanded.Rows.Add("Track Velocity in Polar\n[Ground Speed, Track Angle]", cat10exp.velocityPolar); }
            if (cat10exp.velocityCartesian != null) { expanded.Rows.Add("Track Velocity in Cartesian\n[Vx, Vy]", cat10exp.velocityCartesian); }
            if (cat10exp.TrackStatus != null) { expanded.Rows.Add("Track Status", cat10exp.TrackStatus); }
            if (cat10exp.Mode3ACode != null) { expanded.Rows.Add("Mode 3/A Code", cat10exp.Mode3ACode); }
            if (cat10exp.TargetAddress != null) { expanded.Rows.Add("Target Address", cat10exp.TargetAddress); }
            if (cat10exp.TargetID != null) { expanded.Rows.Add("Target ID", cat10exp.TargetID); }
            try
            {
                int cont = 1;
                while (cont <= cat10exp.MBdata.Count())
                {
                    expanded.Rows.Add("Mode S MB Data #" + cont, cat10exp.ModeS[cont + 1]);
                    cont++;
                }
            }
            catch { }
            if (cat10exp.VFI != null) { expanded.Rows.Add("Vehicle Fleet ID", cat10exp.VFI); }
            if (cat10exp.FlightLevel != null) { expanded.Rows.Add("Flight Level", cat10exp.FlightLevel); }
            if (cat10exp.MeasuredHeight != null) { expanded.Rows.Add("Measured Height", cat10exp.MeasuredHeight); }
            if (cat10exp.TargetSize != null) { expanded.Rows.Add("Target Size\n[Length x Width]", cat10exp.TargetSize); }
            if (cat10exp.TargetOrientation_ != null) { expanded.Rows.Add("Target Orientation", cat10exp.TargetOrientation_); }
            if (cat10exp.SystemStatus != null) { expanded.Rows.Add("System Status", cat10exp.SystemStatus); }
            if (cat10exp.MSG != null) { expanded.Rows.Add("Pre-programmed Message", cat10exp.MSG); }
            if (cat10exp.deviation != null) { expanded.Rows.Add("Standard Deviation of Position\n[X, Y]", cat10exp.deviation); }
            if (cat10exp.covariance != null) { expanded.Rows.Add("Covariance of Deviation", cat10exp.covariance); }
            try
            {
                int count = 1;
                while (count <= cat10exp.Presence.Count())
                {
                    expanded.Rows.Add("Presence #" + count.ToString(), cat10exp.Presence[count - 1]);
                    count++;
                }
            }
            catch { }
            if (cat10exp.amplitudePP != null) { expanded.Rows.Add("Amplitude of Primary Plot", cat10exp.amplitudePP); }
            if (cat10exp.acceleration != null) { expanded.Rows.Add("Calculated Acceleration\n[Ax, Ay]", cat10exp.acceleration); }

            dataexpanded.ItemsSource = expanded.DefaultView;
            dataexpanded.Items.Refresh();
        }

        private void filldataexpandedCAT20(int index)
        {
            DataTable expanded = new DataTable();
            CAT20 cat20exp = f.getCAT20(index);

            expanded.Columns.Add(new DataColumn());
            expanded.Columns.Add(new DataColumn());

            expanded.Rows.Add("Package #",index + 1);
            try { expanded.Rows.Add("Data Source ID", "SAC: " + cat20exp.SAC + " SIC: " + cat20exp.SIC); } catch { }
            try { expanded.Rows.Add("Target Report", cat20exp.getTargetReportDescriptortoString()); } catch { } //Falta desarrollar esto --> funcion en la clase para descifrar tipo de msg
            try { expanded.Rows.Add("Time of Day (UTC)", cat20exp.TOD); } catch { }
            try { expanded.Rows.Add("Position in WSG-84", "[" + cat20exp.LatWSG.ToString() + "," + cat20exp.LonWSG.ToString() + "]"); } catch { }
            try { expanded.Rows.Add("Position (X, Y)", "[" + cat20exp.X.ToString() + "," + cat20exp.Y.ToString() + "] m"); } catch { }
            try { expanded.Rows.Add("Track Number", cat20exp.TrackNum); } catch { }
            try { expanded.Rows.Add("Track Status", cat20exp.getTrackStatusToString()); } catch { }
            try { expanded.Rows.Add("Mode-3/A Code", cat20exp.Mode3A.ToString()); } catch { }
            try { expanded.Rows.Add("Track Velocity (Vx, Vy)", "[" + cat20exp.Vy.ToString() + "," + cat20exp.Vy.ToString() + "] m/s"); } catch { }
            try { expanded.Rows.Add("Flight Level", "FL" + cat20exp.FL[2]); } catch { }
            try { expanded.Rows.Add("Mode-C Code", cat20exp.ModeC.ToString()); } catch { }
            try { expanded.Rows.Add("Target Address", cat20exp.TargetAddress.ToString()); } catch { }
            try { expanded.Rows.Add("Target ID", cat20exp.TargetID[1]);} catch { }
            try { expanded.Rows.Add("Measured Height", cat20exp.MeasuredHeight.ToString()); } catch { }
            try { expanded.Rows.Add("Geometric Height", cat20exp.geoHeight.ToString()); } catch { }
            try { expanded.Rows.Add("Calculated Accel", cat20exp.calcAccel.ToString()); } catch { }
            try { expanded.Rows.Add("Vehicle Fleet ID", cat20exp.VehicleFleetID.ToString()); } catch { }
            try { expanded.Rows.Add("Pre-Programmed Message", cat20exp.PPMsg.ToString()); } catch { }
            try { expanded.Rows.Add("Position Accuracy:", cat20exp.getPositionAccuracyToString()); } catch { }
            try { expanded.Rows.Add("Receivers", cat20exp.Receivers.ToString()); } catch { }
            try { expanded.Rows.Add("Mode S MB Data", cat20exp.ModeSData.ToString()); } catch { }

            dataexpanded.ItemsSource = expanded.DefaultView;
            dataexpanded.Items.Refresh();
        }

        private void searchbut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int package = Convert.ToInt32(searchbox.Text) - 1;

                if (this.cat == 10)
                {
                    CAT10 cat10search = f.getCAT10(package);

                    dataCATsearch = f.getTablaCAT10Indv(cat10search, package);

                    gridCAT.ItemsSource = dataCATsearch.DefaultView;
                    gridCAT.Items.Refresh();

                    filldataexpandedCAT10(package);
                }
                if (this.cat == 20)
                {
                    CAT20 cat20search = f.getCAT20(package);

                    dataCATsearch = f.getTablaCAT20Indv(cat20search, package);

                    gridCAT.ItemsSource = dataCATsearch.DefaultView;
                    gridCAT.Items.Refresh();

                    filldataexpandedCAT20(package);
                }
                if (this.cat == 21)
                {
                    CAT21 cat21search = f.getCAT21(package);

                    dataCATsearch = f.getTablaCAT21Indv(cat21search, package);

                    gridCAT.ItemsSource = dataCATsearch.DefaultView;
                    gridCAT.Items.Refresh();

                    filldataexpandedCAT21(package);
                }
            }
            catch
            {
                MessageBox.Show("Packete no dispoible, comprueba el #");
            }
        }

        private void clearsearchbut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(this.cat == 20) { gridCAT.ItemsSource = dataCAT20.DefaultView; }
                if(this.cat == 10) { gridCAT.ItemsSource = dataCAT10.DefaultView; }
                if(this.cat == 21) { gridCAT.ItemsSource = dataCAT21.DefaultView; }
            }
            catch
            {

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.f.computeFlights();
            List<Flight> flightsim = f.getFlightList();
            SimulacionPanel sim = new SimulacionPanel(f.getListCAT20(),f.getListCAT10(),f.getListCAT21(),this.cat, flightsim);
            this.Hide();
            sim.ShowDialog();
            this.Show();
        }

        private void cat10_butt_Click(object sender, RoutedEventArgs e)
        {
            if (f.ComprobarCAT10() == true)
            {
                this.cat = 10;
                this.fillgridwithdata(this.dataCAT10);

                cat10_butt.IsChecked = true;
            }
            else
            {
                MessageBox.Show("There is no CAT10 packages");

                cat10_butt.IsChecked = false;
            }
        }

        private void cat20_butt_Click(object sender, RoutedEventArgs e)
        {
            if (f.ComprobarCAT20() == true)
            {
                this.cat = 20;
                this.fillgridwithdata(this.dataCAT20);

                cat20_butt.IsChecked = true;
            }
            else
            {
                MessageBox.Show("There is no CAT20 packages");

                cat20_butt.IsChecked = false;
            }
        }

        private void cat21_butt_Click(object sender, RoutedEventArgs e)
        {
            if (f.ComprobarCAT21() == true)
            {
                this.cat = 21;
                this.fillgridwithdata(this.dataCAT21);

                cat21_butt.IsChecked = true;
            }
            else
            {
                MessageBox.Show("There is no CAT21 packages");

                cat21_butt.IsChecked = false;
            }
        }

        private void searchbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= searchbox_GotFocus;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AboutUs au = new AboutUs();
            au.ShowDialog();
        }
    }
}
