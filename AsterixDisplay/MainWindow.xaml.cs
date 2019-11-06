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
            gridCAT.ItemsSource = data.DefaultView;
            gridCAT.Items.Refresh();
        }

        void ClikDataGrid(object sender, RoutedEventArgs e) //al clicar en una celda de la tabla
        {
            //cogemos la celda
            DataGridCell cell = (DataGridCell)sender;
            int c = cell.Column.DisplayIndex; //número de columna
            DataGridRow r2 = DataGridRow.GetRowContainingElement(cell);
            int f = r2.GetIndex(); //número de fila

            if(this.cat==10)
            {
                //expandimos información:


                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT10(f);
            }
            if (this.cat == 20)
            {
                //expandimos información:
                if (c == 8) //Track status
                    MessageBox.Show("Track status shit aqui");
                if (c == 20) //Pos Accuracy
                    MessageBox.Show("Doppler shit aqui");
                if (c == 22) //Mode S MB Data
                    MessageBox.Show("Data Mode S aqui");

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT20(f);
            }
            if (this.cat == 21)
            {
                //expandimos información:
                if (c == 4)
                    MessageBox.Show(" - ");

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT21(f);
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
            try { expanded.Rows.Add("Type of message", cat10exp.MessageType); } catch { }
            try { expanded.Rows.Add("Data Source ID", cat10exp.SAC + ": " + cat10exp.SIC); } catch { }
            //try { expanded.Rows.Add("Target Report:", cat10exp.TargetReport); } catch { } --- NO SÉ CÓMO PONERLO
            try { expanded.Rows.Add("Time of Day (UTC)", cat10exp.TimeOfDay); } catch { }
            try { expanded.Rows.Add("Position in WGS-84","Latitude: "+cat10exp.LatitudeWGS.ToString()+"º, Longitude: "+cat10exp.LongitudeWGS.ToString()+"º"); } catch { }
            try { expanded.Rows.Add("Position in Polar",cat10exp.RHO.ToString()+"m, "+cat10exp.Theta.ToString()+"º"); } catch { }
            try { expanded.Rows.Add("Position in Cartesian","Coord. X: "+cat10exp.Xcomponent.ToString()+"m, Coord. Y: "+cat10exp.Ycomponent.ToString()+"m"); } catch { }
            try { expanded.Rows.Add("Track Velocity in Polar","Ground Speed :"+cat10exp.GroundSpeed.ToString()+"NM/s, Track Angle: "+cat10exp.TrackAngle.ToString()+"º"); } catch { }
            try { expanded.Rows.Add("Track Velocity in Cartesian", "Coord. X: " + cat10exp.Vx.ToString() + "m/s, Coord. Y: " + cat10exp.Vy.ToString() + "m/s"); } catch { }
            try { expanded.Rows.Add("Track Number",cat10exp.TrackAngle); } catch { }
            try { expanded.Rows.Add("Track Status",cat10exp.TrackStatus); } catch { }
            try { expanded.Rows.Add("Mode 3/A Code",cat10exp.Mode3ACode); } catch { }
            try { expanded.Rows.Add("Target Address",cat10exp.TargetAddress); } catch { }
            try { expanded.Rows.Add("Target ID",cat10exp.TargetID); } catch { }
            try
            {
                int cont = 1;
                while (cont <= cat10exp.MBdata.Count())
                {
                    try { expanded.Rows.Add("Mode S MB Data #" + cont, "Message: " + cat10exp.MBdata[cont] + ", Address1: " + cat10exp.BDS1[cont] + ", Address 2: " + cat10exp.BDS2[cont]); } catch { }
                    cont++;
                }
            }
            catch { }
            try { expanded.Rows.Add("Vehicle Fleet ID",cat10exp.VehicleFeetID); } catch { }
            try { expanded.Rows.Add("Flight Level","FL"+cat10exp.FL.ToString()); } catch { }
            try { expanded.Rows.Add("Measured Height",cat10exp.Height.ToString()+"ft"); } catch { }
            try { expanded.Rows.Add("Target Size","Length: "+cat10exp.TargetLength.ToString()+"m, Width: "+cat10exp.TargetWidth.ToString()+"m"); } catch { } //length and width
            try { expanded.Rows.Add("Target Orientation",cat10exp.TargetOrientation+"º"); } catch { }
            //try { expanded.Rows.Add("System Status",); } catch { }             --- NO SÉ CÓMO PONERLO
            try { expanded.Rows.Add("Pre-programmed Message",cat10exp.MSG); } catch { }
            try { expanded.Rows.Add("Standard Deviation of Position","X component: "+cat10exp.DevX.ToString()+"m, Y component: "+cat10exp.DevY.ToString()+"m"); } catch { } //x and y
            try { expanded.Rows.Add("Covariance of Deviation",cat10exp.Covariance.ToString()+"m2"); } catch { }
            try
            {
                int count = 1;
                while (count <= cat10exp.DRHO.Count())
                {
                    try { expanded.Rows.Add("Presence #" + count.ToString(), cat10exp.DRHO[count].ToString() + "m, " + cat10exp.DTHETA[count].ToString() + "º"); } catch { }
                    count++;
                }
            }
            catch { }
            try { expanded.Rows.Add("Amplitude of Primary Plot",cat10exp.PAM.ToString()); } catch { }
            try { expanded.Rows.Add("Calculated Acceleration","X component: "+cat10exp.Ax.ToString()+"m/s2, Y component: "+cat10exp.Ay.ToString()+"m/s2"); } catch { }

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
            expanded.Rows.Add("FSPEC",cat20exp.FSPEC);
            try { expanded.Rows.Add("Data Source ID", "SAC: " + cat20exp.SAC + " SIC: " + cat20exp.SIC); } catch { }
            try { expanded.Rows.Add("Target Report", cat20exp.TargetReport); } catch { } //Falta desarrollar esto --> funcion en la clase para descifrar tipo de msg
            try { expanded.Rows.Add("Time of Day (UTC)", cat20exp.TOD); } catch { }
            try { expanded.Rows.Add("Position in WSG-84", "[" + cat20exp.LatWSG.ToString() + "," + cat20exp.LonWSG.ToString() + "]"); } catch { }
            try { expanded.Rows.Add("Position (X, Y)", "[" + cat20exp.X.ToString() + "," + cat20exp.Y.ToString() + "] m"); } catch { }
            try { expanded.Rows.Add("Track Number", cat20exp.TrackNum); } catch { }
            try { expanded.Rows.Add("Track Status", cat20exp.CDM); } catch { }
            try { expanded.Rows.Add("Mode-3/A Code", cat20exp.Mode3A.ToString()); } catch { }
            try { expanded.Rows.Add("Track Velocity (Vx, Vy)", "[" + cat20exp.Vy.ToString() + "," + cat20exp.Vy.ToString() + "] m/s"); } catch { }
            try { expanded.Rows.Add("Flight Level", cat20exp.FL[2]); } catch { }
            try { expanded.Rows.Add("Mode-C Code", cat20exp.ModeC.ToString()); } catch { }
            try { expanded.Rows.Add("Target Address", cat20exp.TargetAddress.ToString()); } catch { }
            try { expanded.Rows.Add("Target ID", cat20exp.TargetID[1]);} catch { }
            try { expanded.Rows.Add("Measured Height", cat20exp.MeasuredHeight.ToString()); } catch { }
            try { expanded.Rows.Add("Geometric Height", cat20exp.geoHeight.ToString()); } catch { }
            try { expanded.Rows.Add("Calculated Accel", cat20exp.calcAccel.ToString()); } catch { }
            try { expanded.Rows.Add("Vehicle Fleet ID", cat20exp.VehicleFleetID.ToString()); } catch { }
            try { expanded.Rows.Add("Pre-Programmed Message", cat20exp.PPMsg.ToString()); } catch { }
            try { expanded.Rows.Add("Position Accuracy:", "Falta desarrollar"); } catch { }
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
                CAT20 cat20search = f.getCAT20(package);

                dataCATsearch = f.getTablaCAT20Indv(cat20search, package);

                gridCAT.ItemsSource = dataCATsearch.DefaultView;
                gridCAT.Items.Refresh();

                filldataexpandedCAT20(package);
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
                gridCAT.ItemsSource = dataCAT20.DefaultView;
            }
            catch
            {

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SimulacionPanel sim = new SimulacionPanel(f.getListCAT20()); 
            sim.ShowDialog();
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
    }
}
