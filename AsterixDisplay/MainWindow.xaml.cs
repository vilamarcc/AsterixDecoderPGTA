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
            this.WindowState = WindowState.Maximized;

            //escondemos botones para evitar erroes
            searchbox.Visibility = Visibility.Hidden;
            clearsearchbut.Visibility = Visibility.Hidden;
            searchbut.Visibility = Visibility.Hidden;
            combobox.Visibility = Visibility.Hidden;
            cat10_butt.Visibility = Visibility.Hidden;
            cat20_butt.Visibility = Visibility.Hidden;
            cat21_butt.Visibility = Visibility.Hidden;
        }

        private async void cargarbut_Click(object sender, RoutedEventArgs e) // botón OPEN FILE
        {
            //vaciamos las tablas
            gridCAT.ItemsSource = null;
            dataexpanded.ItemsSource = null;
            gridCAT.Items.Clear();
            dataexpanded.Items.Clear();

            //escondemos botones para evitar erroes
            searchbox.Visibility = Visibility.Hidden;
            clearsearchbut.Visibility = Visibility.Hidden;
            searchbut.Visibility = Visibility.Hidden;
            combobox.Visibility = Visibility.Hidden;
            cat10_butt.Visibility = Visibility.Hidden;
            cat20_butt.Visibility = Visibility.Hidden;
            cat21_butt.Visibility = Visibility.Hidden;

            //lo que ha de poner en la cajita de búsqueda
            if (combobox.SelectedIndex == 0)
                searchbox.Text = "Package #";
            if (combobox.SelectedIndex == 1)
                searchbox.Text = "Callsign";
            if (combobox.SelectedIndex == 2)
                searchbox.Text = "Track Number";

            //mostramos la barra de progreso
            var progress = new Progress<int>(value => progressbar1.Value = value);
            progressbar1.Visibility = Visibility.Visible;

            //que sea un archivo asterix
            ofd.Filter = "AST |*.ast";
            if (ofd.ShowDialog() == true)
            {
                //preguntamos versión de la cat21
                versionCAT21 vc = new versionCAT21();
                vc.ShowDialog();
                int version = vc.GetVersion();
                
                await Task.Run(() =>
                {
                    //leemos fichero
                    this.fichero = ofd.FileName;
                    f = new Fichero(this.fichero);
                    ((IProgress<int>)progress).Report(10);

                    //leemos los paquetes 
                    f.Read(version);
                    ((IProgress<int>)progress).Report(70);

                    //decimos que se ha cargado ya el fichero
                    MessageBox.Show("File Loaded","AsterixDecoder",MessageBoxButton.OK,MessageBoxImage.Information);
                    ((IProgress<int>)progress).Report(75);

                    //guardamos las tablas y mostramos la que toque (this.cat)
                    this.dataCAT20 = f.getTablaCAT20();
                    this.dataCAT10 = f.getTablaCAT10();
                    this.dataCAT21 = f.getTablaCAT21();
                    ((IProgress<int>)progress).Report(85);

                    //cat que vamos a mostrar por defecto 
                    //si el archivo tiene más de una cat se mostrará la más grande (por defecto)
                    if (f.ComprobarCAT10() == true)
                        this.cat = 10;
                    if (f.ComprobarCAT20() == true)
                        this.cat = 20;
                    if (f.ComprobarCAT21() == true)
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

                //enseñamos los botones
                searchbox.Visibility = Visibility.Visible;
                clearsearchbut.Visibility = Visibility.Visible;
                searchbut.Visibility = Visibility.Visible;
                combobox.Visibility = Visibility.Visible;
                cat10_butt.Visibility = Visibility.Visible;
                cat20_butt.Visibility = Visibility.Visible;
                cat21_butt.Visibility = Visibility.Visible;

                //escondemos la barra de progreso
                progressbar1.Visibility = Visibility.Hidden;
            }
        }

        public void fillgridwithdata(DataTable data) //enseña en pantalla la tabla que le damos como input
        {
            gridCAT.ItemsSource = data.DefaultView;
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
                if (c == 5 && paquete.DataCharacteristics!=null) // Target Report - Data Characteristics
                    MessageBox.Show(paquete.DataCharacteristics);
                if (c == 13 && paquete.TrackStatus != null) // Track Status
                    MessageBox.Show(paquete.TrackStatus);
                if (c == 27 && paquete.Presence != null) // Presence
                    MessageBox.Show(paquete.Presence);
                if (c == 17 && paquete.ModeS != null) // Mode S MB Data
                    MessageBox.Show(paquete.ModeS);
                if (c == 23 && paquete.SystemStatus != null) // System Status
                    MessageBox.Show(paquete.SystemStatus);

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT10(fil);
            }
            if (this.cat == 20)
            {
                CAT20 paquete = f.getCAT20(fil);

                //expandimos información:
                if (c == 4 && paquete.getTargetReportDescriptortoString() != null)
                    MessageBox.Show(paquete.getTargetReportDescriptortoString());
                if (c == 9 && paquete.getTrackStatusToString() != null) //Track status
                    MessageBox.Show(paquete.getTrackStatusToString());
                if (c == 21 && paquete.getPositionAccuracyToString() != null) //Pos Accuracy
                    MessageBox.Show(paquete.getPositionAccuracyToString());

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT20(fil);
            }
            if (this.cat == 21)
            {
                CAT21 paquete = f.getCAT21(fil);

                //expandimos información:
                if (c == 4 && paquete.TargetReport != null) //Target Report Descriptor
                    MessageBox.Show(paquete.TargetReport);
                if (c == 10 && paquete.OperationalStatus != null) //Operational Status
                    MessageBox.Show(paquete.OperationalStatus);
                if (c == 45 && paquete.FigureOfMerit != null) //Figure Of Merit
                    MessageBox.Show(paquete.FigureOfMerit);
                if (c == 46 && paquete.ages != null) //Data ages
                    MessageBox.Show(paquete.ages);
                if (c == 41 && paquete.TrajectoryIntentData != null) //Trajectory Intent Data
                    MessageBox.Show(paquete.TrajectoryIntentData);
                if (c == 29 && paquete.LinkTech != null) //Link Technology
                    MessageBox.Show(paquete.LinkTech);
                if (c == 23 && paquete.TargetStatus != null) //Target Status
                    MessageBox.Show(paquete.TargetStatus);
                if (c == 21 && paquete.MetReport != null) //Met Report
                    MessageBox.Show(paquete.MetReport);
                if (c == 27 && paquete.QualityIndicators != null) //Quality Indicators
                    MessageBox.Show(paquete.QualityIndicators);
                if (c == 28 && paquete.ModeS != null) //Mode S
                    MessageBox.Show(paquete.ModeS);
                if (c == 16 && paquete.MOPS != null) //MOPS version
                    MessageBox.Show(paquete.MOPS);

                //mostramos las características de ese paquete a parte:
                filldataexpandedCAT21(fil);
            }
        }

        private void filldataexpandedCAT21(int index) //crea la tabla pequeña de CAT21 (solo con los items que existan)
        {
            DataTable expanded = new DataTable();
            CAT21 cat21exp = f.getCAT21(index);

            expanded.Columns.Add(new DataColumn());
            expanded.Columns.Add(new DataColumn());

            expanded.Rows.Add("Package #", index + 1);
            if (cat21exp.DataSourceID != null) { expanded.Rows.Add("Data Source", cat21exp.DataSourceID); }
            if (cat21exp.TargetID != null) { expanded.Rows.Add("Target ID", cat21exp.TargetID); }
            if (cat21exp.TrackNumber != null) { expanded.Rows.Add("Track Number", cat21exp.TrackNumber); }
            if (cat21exp.TOD != null) { expanded.Rows.Add("Time of Day", cat21exp.TOD); }
            if (cat21exp.TimeOfAsterixReportTransmission_ != null) { expanded.Rows.Add("Time of ASTERIX Report Transmission", cat21exp.TimeOfAsterixReportTransmission_); }
            if (cat21exp.positionWGS != null) { expanded.Rows.Add("Position WGS-84 coordinates\n[Latitude, Longitude]", cat21exp.positionWGS); }
            if (cat21exp.AirborneGroundVector != null) { expanded.Rows.Add("Airborne Ground vector\n[Ground Speed, Track Angle]", cat21exp.AirborneGroundVector); }
            if (cat21exp.TrackAngleRate != null) { expanded.Rows.Add("Track Angle Rate", cat21exp.TrackAngleRate); }
            if (cat21exp.FlightLevel != null) { expanded.Rows.Add("Flight Level", cat21exp.FlightLevel); }
            if (cat21exp.TargetAddress != null) { expanded.Rows.Add("Target Address", cat21exp.TargetAddress); }
            if (cat21exp.TargetReport != null) { expanded.Rows.Add("Target Report", cat21exp.TargetReport); }
            if (cat21exp.OperationalStatus != null) { expanded.Rows.Add("Operational Status", cat21exp.OperationalStatus); }
            if (cat21exp.HRpositionWGS != null) { expanded.Rows.Add("High Resolution position WGS-84\n[Latitude, Longitude]", cat21exp.HRpositionWGS); }
            if (cat21exp.AirSpeed != null) { expanded.Rows.Add("Air Speed", cat21exp.AirSpeed); }
            if (cat21exp.TrueAirspeed != null) { expanded.Rows.Add("True Air Speed", cat21exp.TrueAirspeed); }
            if (cat21exp.GeometricHeight != null) { expanded.Rows.Add("Geometric Height", cat21exp.GeometricHeight); }
            if (cat21exp.SelectedAltitude_IS != null) { expanded.Rows.Add("Selected Altitude\nIntermediate State", cat21exp.SelectedAltitude_IS); }
            if (cat21exp.SelectedAltitude_FS != null) { expanded.Rows.Add("Selected Altitude\nFinal State", cat21exp.SelectedAltitude_FS); }
            if (cat21exp.MOPS != null) { expanded.Rows.Add("MOPS version", cat21exp.MOPS); }
            if (cat21exp.MagneticHeading != null) { expanded.Rows.Add("Magnetic Heading", cat21exp.MagneticHeading); }
            if (cat21exp.BarometricVerticalRate != null) { expanded.Rows.Add("Barometric Vertical rate", cat21exp.BarometricVerticalRate); }
            if (cat21exp.GeometricVerticalRate != null) { expanded.Rows.Add("Geometric Vertical rate", cat21exp.GeometricVerticalRate); }
            if (cat21exp.Mode3ACode != null) { expanded.Rows.Add("Mode 3A Code", cat21exp.Mode3ACode); }
            if (cat21exp.MetReport != null) { expanded.Rows.Add("Met Report", cat21exp.MetReport); }
            if (cat21exp.ECAT != null) { expanded.Rows.Add("Emitter category", cat21exp.ECAT); }
            if (cat21exp.TargetStatus != null) { expanded.Rows.Add("Target Status", cat21exp.TargetStatus); }
            if (cat21exp.TrackAngleRate != null) { expanded.Rows.Add("Track Angle Rate", cat21exp.TrackAngleRate); }
            if (cat21exp.RateOfTurn != null) { expanded.Rows.Add("Rate of Turn", cat21exp.RateOfTurn); }
            if (cat21exp.RollAngle != null) { expanded.Rows.Add("Roll Angle", cat21exp.RollAngle); }
            if (cat21exp.serviceID != null) { expanded.Rows.Add("Service Identification", cat21exp.serviceID); }
            if (cat21exp.QualityIndicators != null) { expanded.Rows.Add("Quality Indicators", cat21exp.QualityIndicators); }
            if (cat21exp.ModeS != null) { expanded.Rows.Add("Mode S", cat21exp.ModeS); }
            if (cat21exp.LinkTech != null) { expanded.Rows.Add("Link Technology", cat21exp.LinkTech); }
            if (cat21exp.RP != null) { expanded.Rows.Add("Report period", cat21exp.RP); }
            if (cat21exp.MSGampl != null) { expanded.Rows.Add("Message amplitude", cat21exp.MSGampl); }
            if (cat21exp.TrackAngleRate != null) { expanded.Rows.Add("Track Angle rate", cat21exp.TrackAngleRate); }
            if (cat21exp.RID != null) { expanded.Rows.Add("Receiver ID", cat21exp.RID); }
            if (cat21exp.TimeOfApplicabilityForPosition_ != null) { expanded.Rows.Add("Time of Applicability\nfor position", cat21exp.TimeOfApplicabilityForPosition_); }
            if (cat21exp.TimeOfApplicabilityForVelocity_ != null) { expanded.Rows.Add("Time of Applicability\nfor velocity", cat21exp.TimeOfApplicabilityForVelocity_); }
            if (cat21exp.TimeOfMessageReceptionForPosition_ != null) { expanded.Rows.Add("Time of Message Reception\nfor position", cat21exp.TimeOfMessageReceptionForPosition_); }
            if (cat21exp.TimeOfMessageReceptionForVelocity_ != null) { expanded.Rows.Add("Time of Message Reception\nfor velocity", cat21exp.TimeOfMessageReceptionForVelocity_); }
            if (cat21exp.TimeOfMessageReceptionForPosition_HighPrecision_ != null) { expanded.Rows.Add("Time of Message Reception\nfor position - High Resolution", cat21exp.TimeOfMessageReceptionForPosition_HighPrecision_); }
            if (cat21exp.TimeOfMessageReceptionForVelocity_HighPrecision_ != null) { expanded.Rows.Add("Time of Message Reception\nfor velocity - High Resolution", cat21exp.TimeOfMessageReceptionForVelocity_HighPrecision_); }
            if (cat21exp.TrajectoryIntentData != null) { expanded.Rows.Add("Trajectory Intent Data", cat21exp.TrajectoryIntentData); }
            if (cat21exp.posAccuracy != null) { expanded.Rows.Add("Position accuracy", cat21exp.posAccuracy); }
            if (cat21exp.velAccuracy != null) { expanded.Rows.Add("Velocity accuracy", cat21exp.velAccuracy); }
            if (cat21exp.TODaccuracy != null) { expanded.Rows.Add("Time of Day accuracy", cat21exp.TODaccuracy); }
            if (cat21exp.FigureOfMerit != null) { expanded.Rows.Add("Figure of merit", cat21exp.FigureOfMerit); }
            if (cat21exp.ages != null) { expanded.Rows.Add("Data Ages", cat21exp.ages); }
            if (cat21exp.RP != null) { expanded.Rows.Add("Service Management", cat21exp.RP); }

            dataexpanded.ItemsSource = expanded.DefaultView;
            dataexpanded.Items.Refresh();
        }

        private void filldataexpandedCAT10(int index) //crea la tabla pequeña de CAT10 (solo con los items que existan)
        {
            DataTable expanded = new DataTable();
            CAT10 cat10exp = f.getCAT10(index);

            expanded.Columns.Add(new DataColumn());
            expanded.Columns.Add(new DataColumn());

            expanded.Rows.Add("Package #", index + 1);
            if (cat10exp.DataSourceID != null) { expanded.Rows.Add("Data Source ID", cat10exp.DataSourceID); }
            if (cat10exp.TargetID != null) { expanded.Rows.Add("Target ID", cat10exp.TargetID); }
            if (cat10exp.TrackNumber != null) { expanded.Rows.Add("Track Number", cat10exp.TrackNumber); }
            if (cat10exp.TYP != null) { expanded.Rows.Add("Data Type", cat10exp.TYP); }
            if (cat10exp.TimeOfDay != null) { expanded.Rows.Add("Time of Day (UTC)", cat10exp.TimeOfDay); }
            if (cat10exp.positionWGS != null) { expanded.Rows.Add("Position in WGS-84\n[Latitude, Longitude]", cat10exp.positionWGS); }
            if (cat10exp.positionPolar != null) { expanded.Rows.Add("Position in Polar\n[Distance, Angle]", cat10exp.positionPolar); }
            if (cat10exp.positionCartesian != null) { expanded.Rows.Add("Position in Cartesian\n[X, Y]", cat10exp.positionCartesian); }
            if (cat10exp.velocityPolar != null) { expanded.Rows.Add("Track Velocity in Polar\n[Ground Speed, Track Angle]", cat10exp.velocityPolar); }
            if (cat10exp.velocityCartesian != null) { expanded.Rows.Add("Track Velocity in Cartesian\n[Vx, Vy]", cat10exp.velocityCartesian); }
            if (cat10exp.FlightLevel != null) { expanded.Rows.Add("Flight Level", cat10exp.FlightLevel); }
            if (cat10exp.MessageType != null) { expanded.Rows.Add("Message Type", cat10exp.MessageType); }
            if (cat10exp.DataCharacteristics != null) { expanded.Rows.Add("Data Characteristics", cat10exp.DataCharacteristics); }
            if (cat10exp.TrackStatus != null) { expanded.Rows.Add("Track Status", cat10exp.TrackStatus); }
            if (cat10exp.Mode3ACode != null) { expanded.Rows.Add("Mode 3/A Code", cat10exp.Mode3ACode); }
            if (cat10exp.TargetAddress != null) { expanded.Rows.Add("Target Address", cat10exp.TargetAddress); }
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

        private void filldataexpandedCAT20(int index) //crea la tabla pequeña de CAT20 (solo con los items que existan)
        {
            DataTable expanded = new DataTable();
            CAT20 cat20exp = f.getCAT20(index);

            expanded.Columns.Add(new DataColumn());
            expanded.Columns.Add(new DataColumn());

            expanded.Rows.Add("Package #",index + 1);
            try { expanded.Rows.Add("Data Source ID", "SAC: " + cat20exp.SAC + " SIC: " + cat20exp.SIC); } catch { }
            try { expanded.Rows.Add("Target ID", cat20exp.TargetID[1]); } catch { }
            try { expanded.Rows.Add("Track Number", cat20exp.TrackNum); } catch { }
            try { expanded.Rows.Add("Time of Day (UTC)", cat20exp.TOD); } catch { }
            try { expanded.Rows.Add("Position in WSG-84", "[" + cat20exp.LatWSG.ToString() + "," + cat20exp.LonWSG.ToString() + "]"); } catch { }
            try { expanded.Rows.Add("Position (X, Y)", "[" + cat20exp.X.ToString() + "," + cat20exp.Y.ToString() + "] m"); } catch { }
            try { expanded.Rows.Add("Track Velocity (Vx, Vy)", "[" + cat20exp.Vy.ToString() + "," + cat20exp.Vy.ToString() + "] m/s"); } catch { }
            try { expanded.Rows.Add("Flight Level", "FL" + cat20exp.FL[2]); } catch { }
            try { expanded.Rows.Add("Target Report", cat20exp.getTargetReportDescriptortoString()); } catch { } 
            try { expanded.Rows.Add("Track Status", cat20exp.getTrackStatusToString()); } catch { }
            try { expanded.Rows.Add("Mode-3/A Code", cat20exp.Mode3A.ToString()); } catch { }
            try { expanded.Rows.Add("Mode-C Code", cat20exp.ModeC.ToString()); } catch { }
            try { expanded.Rows.Add("Target Address", cat20exp.TargetAddress.ToString()); } catch { }
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

        private void searchbut_Click(object sender, RoutedEventArgs e) // botón SEARCH
        {
            if (combobox.SelectedIndex == 0) // buscar por número de paquete
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
                    MessageBox.Show("Package not available, check the number");
                }
            }
            if(combobox.SelectedIndex == 1) // buscar por callsign
            {
                try
                {
                    if (this.cat == 10)
                    {
                        dataCATsearch = f.SearchCallsignCAT10(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                    if (this.cat == 20)
                    {
                        dataCATsearch = f.SearchCallsignCAT20(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                    if (this.cat == 21)
                    {
                        dataCATsearch = f.SearchCallsignCAT21(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                }
                catch
                {
                    MessageBox.Show("Package not available, check the callsign");
                }
            }
            if (combobox.SelectedIndex == 2) // buscar por track number
            {
                try
                {
                    if (this.cat == 10)
                    {
                        dataCATsearch = f.SearchTrackNumberCAT10(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                    if (this.cat == 20)
                    {
                        dataCATsearch = f.SearchTrackNumberCAT20(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                    if (this.cat == 21)
                    {
                        dataCATsearch = f.SearchTrackNumberCAT21(searchbox.Text);

                        gridCAT.ItemsSource = dataCATsearch.DefaultView;
                        gridCAT.Items.Refresh();
                    }
                }
                catch
                {
                    MessageBox.Show("Package not available, check the track number");
                }
            }
        }

        private void clearsearchbut_Click(object sender, RoutedEventArgs e) //botón CLEAR
        {
            try
            {
                //llenamos la tabla grande con todo el paquete
                if (this.cat == 20) { gridCAT.ItemsSource = dataCAT20.DefaultView; }
                if (this.cat == 10) { gridCAT.ItemsSource = dataCAT10.DefaultView; }
                if (this.cat == 21) { gridCAT.ItemsSource = dataCAT21.DefaultView; }

                //vaciamos la tabla pequeña
                dataexpanded.ItemsSource = null;
                dataexpanded.Items.Clear();

                //texto en la caja de búsqueda
                if (combobox.SelectedIndex == 0)
                    searchbox.Text = "Package #";
                if (combobox.SelectedIndex == 1)
                    searchbox.Text = "Callsign";
                if (combobox.SelectedIndex == 2)
                    searchbox.Text = "Track Number";
            }
            catch
            {

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) //botón SIMULATE
        {
            if (f != null)
            {
                if (f.checkMultiplecat() == false) //si solo hay paquetes de una cat
                {
                    this.f.computeFlights();
                    List<Flight> flightsim = f.getFlightList();
                    SimulacionPanel sim = new SimulacionPanel(f.getListCAT20(), f.getListCAT10(), f.getListCAT21(), this.cat, flightsim);
                    this.Hide();
                    sim.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("The file you are trying to load into simualtion window may contain multiple CATs in the same file or might be of a CAT not supported yet\nPlease wait for future updates or contact the developers \n        Sorry for the inconvenience", "Simulation", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else { MessageBox.Show("File not loaded", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void cat10_butt_Click(object sender, RoutedEventArgs e) //click en la redondita CAT10
        {
            if (f.ComprobarCAT10() == true)//si hay paquetes de CAT10
            {
                //asignamos categoría y llenamos la tabla grande
                this.cat = 10;
                this.fillgridwithdata(this.dataCAT10);

                //marcamos el botoncito de cat10
                cat10_butt.IsChecked = true;

                //vaciamos la tabla pequeña
                dataexpanded.ItemsSource = null;
                dataexpanded.Items.Clear();
            }
            else
            {
                MessageBox.Show("No CAT10 packages found");

                //marcamos botoncito que había marcado antes de darle
                cat10_butt.IsChecked = false;
                if (this.cat == 20)
                    cat20_butt.IsChecked = true;
                if (this.cat == 21)
                    cat21_butt.IsChecked = true;
            }
        }

        private void cat20_butt_Click(object sender, RoutedEventArgs e) //click en la redondita CAT20
        {
            if (f.ComprobarCAT20() == true)//si hay paquetes de CAT20
            {
                //asignamos categoría y llenamos la tabla grande
                this.cat = 20;
                this.fillgridwithdata(this.dataCAT20);

                //marcamos el botoncito de cat20
                cat20_butt.IsChecked = true;

                //vaciamos la tabla pequeña
                dataexpanded.ItemsSource = null;
                dataexpanded.Items.Clear();
            }
            else
            {
                MessageBox.Show("No CAT20 packages found");

                //marcamos botoncito que había marcado antes de darle
                cat20_butt.IsChecked = false;
                if (this.cat == 10)
                    cat10_butt.IsChecked = true;
                if (this.cat == 21)
                    cat21_butt.IsChecked = true;
            }
        }

        private void cat21_butt_Click(object sender, RoutedEventArgs e) //click en la redondita CAT21
        {
            if (f.ComprobarCAT21() == true) //si hay paquetes de CAT21
            {
                //asignamos categoría y llenamos la tabla grande
                this.cat = 21;
                this.fillgridwithdata(this.dataCAT21);

                //marcamos el botoncito de cat21
                cat21_butt.IsChecked = true;

                //vaciamos la tabla pequeña
                dataexpanded.ItemsSource = null;
                dataexpanded.Items.Clear();
            }
            else
            {
                MessageBox.Show("No CAT21 packages found");

                //marcamos botoncito que había marcado antes de darle
                cat21_butt.IsChecked = false;
                if (this.cat == 20)
                    cat20_butt.IsChecked = true;
                if (this.cat == 10)
                    cat10_butt.IsChecked = true;
            }
        }

        private void searchbox_GotFocus(object sender, RoutedEventArgs e) // cuando clicamos para escribir el filtro, se borra lo que pone
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= searchbox_GotFocus;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) //botón ABOUT US
        {
            AboutUs au = new AboutUs();
            au.ShowDialog();
        }

        private void combobox_selectionchanged(object sender, SelectionChangedEventArgs e) // cuando cambiamos la opción para buscar (click en el combobox)
        {
            if (combobox.SelectedIndex == 0)
                searchbox.Text = "Package #";
            if (combobox.SelectedIndex == 1)
                searchbox.Text = "Callsign";
            if (combobox.SelectedIndex == 2)
                searchbox.Text = "Track Number";
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e) //botón HELP
        {
            Help help = new Help();
            help.ShowDialog();
        }
    }
}
