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

        string fichero;
        DataTable dataCAT20;
        DataTable dataCAT10;
        DataTable dataCAT21;
        DataTable dataCATsearch;
        Fichero f;
        int cat;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void cargarbut_Click(object sender, RoutedEventArgs e)
        {
            //per defecte mostrem cat 10:
            this.cat = 10;
            cat10_butt.IsChecked = true;

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
                    MessageBox.Show("Archivo cargado");
                    ((IProgress<int>)progress).Report(75);
                    //guardamos las tablas y mostramos la que toque (this.cat)
                    dataCAT20 = f.getTablaCAT20();
                    dataCAT10 = f.getTablaCAT10();
                    dataCAT21 = f.getTablaCAT21();
                    this.fillgridwithdata();
                    ((IProgress<int>)progress).Report(100);
                });
                progressbar1.Visibility = Visibility.Hidden;
            }
        }

        public void fillgridwithdata()
        {
            DataTable data = new DataTable();

            if (this.cat == 10)
                data = dataCAT10;
            if (this.cat == 20)
                data = dataCAT20;
            if (this.cat == 21)
                data = dataCAT21;

            gridCAT.ItemsSource = data.DefaultView;
            gridCAT.Items.Refresh();
        }

        void ClikDataGrid(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            int c = cell.Column.DisplayIndex;
            DataGridRow r2 = DataGridRow.GetRowContainingElement(cell);
            int f = r2.GetIndex();
            if (c == 8) //Track status
            {
                MessageBox.Show("Track status shit aqui");
            }
            if (c == 20) //Pos Accuracy
            {
                MessageBox.Show("Doppler shit aqui");
            }
            if (c == 22) //Mode S MB Data
            {
                MessageBox.Show("Data Mode S aqui");
            }

            filldataexpandedCAT20(f);
        }

        private void filldataexpandedCAT10(int index)
        {
            DataTable expanded = new DataTable();
            CAT10 cat10exp = f.getCAT10(index);
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
            SimulacionPanel sim = new SimulacionPanel();
            sim.ShowDialog();
        }

        private void cat10_butt_Click(object sender, RoutedEventArgs e)
        {
            this.cat = 10;
            this.fillgridwithdata();
        }

        private void cat20_butt_Click(object sender, RoutedEventArgs e)
        {
            this.cat = 20;
            this.fillgridwithdata();
        }

        private void cat21_butt_Click(object sender, RoutedEventArgs e)
        {
            this.cat = 21;
            this.fillgridwithdata();
        }
    }
}
