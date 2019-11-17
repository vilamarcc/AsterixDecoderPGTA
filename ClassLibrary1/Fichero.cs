using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using AsterixDecoder;
using System.Reflection;

namespace AsterixDecoder
{
    public class Fichero
    {
            // ATRIBUTS:
        //fichero elegido
        string path;

        //listas de paquetes --> las cargamos al leer el fichero (accedemos a la classe que toque)
        List<CAT10> listaCAT10 = new List<CAT10>();
        List<CAT20> listaCAT20 = new List<CAT20>();
        List<CAT21> listaCAT21 = new List<CAT21>();
        List<Flight> listflights = new List<Flight>();

        //tablas con los datos --> las cargamos cuando tengamos las listas
        DataTable tablaCAT10 = new DataTable();
        DataTable tablaCAT20 = new DataTable();
        DataTable tablaCAT21 = new DataTable();

            // CONSTRUCTOR:
        public Fichero(string nombre) //coge el nombre del fichero y crea la tabla (columnas)
        {
            this.path = nombre;

            this.CrearTablas();
        }

            // GETTERS:
        public List<CAT10> getListCAT10()
        {
            return listaCAT10;
        }

        public List<CAT20> getListCAT20()
        {
            return listaCAT20;
        }

        public List<CAT21> getListCAT21()
        {
            return listaCAT21;
        }

        public DataTable getTablaCAT10()
        {
            return tablaCAT10;
        }

        public DataTable getTablaCAT20()
        {
            return tablaCAT20;
        }

        public DataTable getTablaCAT21()
        {
            return tablaCAT21;
        }

            // ALTRES MÈTODES:
        public void Read(int version) //lee el fichero y crea las listas y las tablas
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            List<byte[]> listabyte = new List<byte[]>();

            int i = 0;
            int contador = fileBytes[2];
            while (i < fileBytes.Length)
            {
                byte[] array = new byte[contador];
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = fileBytes[i];
                    i++;
                }
                listabyte.Add(array);
                if (i + 2 < fileBytes.Length)
                    contador = fileBytes[i + 2];
            }

            List<string[]> listahex = new List<string[]>();
            for (int x = 0; x < listabyte.Count; x++)
            {
                byte[] buffer = listabyte[x];
                string[] arrayhex = new string[buffer.Length];
                for (int y = 0; y < buffer.Length; y++)
                {
                    string a = buffer[y].ToString("X");
                    if (a.Length == 1)
                        arrayhex[y] = "0" + a;
                    if (a.Length == 2)
                        arrayhex[y] = a;
                }
                listahex.Add(arrayhex);
            }

            int contadorCAT10 = 0;
            int contadorCAT20 = 0;
            int contadorCAT21 = 0;
            for (int q = 0; q < listahex.Count; q++)
            {
                string[] arraystring = listahex[q];
                int CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);

                if (CAT == 10)
                {
                    contadorCAT10++;
                    //llegim
                    CAT10 newcat10 = new CAT10(arraystring);
                    //afegim a la llista
                    listaCAT10.Add(newcat10);
                    //afegim a la taula
                    tablaCAT10.Rows.Add(contadorCAT10, newcat10.SACnum, newcat10.SICnum, newcat10.TargetID, newcat10.TrackNumber, newcat10.TYP, newcat10.MessageType, newcat10.datachar_, newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.trackstatus_, newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.modes_, newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, newcat10.systemstatus, newcat10.MSG, newcat10.deviation, newcat10.covariance, newcat10.presence_, newcat10.amplitudePP, newcat10.acceleration);
                }
                else if (CAT == 20)
                {
                    contadorCAT20++;
                    //llegim
                    CAT20 newcat20 = new CAT20(arraystring);
                    //afegim a la llista
                    listaCAT20.Add(newcat20);
                    //afegim a la taula
                    tablaCAT20.Rows.Add(contadorCAT20, newcat20.SAC, newcat20.SIC, newcat20.callsign, newcat20.TrackNum, newcat20.getMessageType(), newcat20.targetreport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.trackstatus, newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.posaccuracy, newcat20.Receivers, newcat20.ModeSData,newcat20.ACAScap, newcat20.ACASRAreport, newcat20.warning, newcat20.Mode1Code, newcat20.Mode2Code);
                }
                else if (CAT == 21)
                {
                    contadorCAT21++;
                    //llegim
                    CAT21 newcat21 = new CAT21(arraystring, version);
                    //afegim a la llista
                    listaCAT21.Add(newcat21);
                    //afegim a la taula
                    tablaCAT21.Rows.Add(contadorCAT21, newcat21.SAC, newcat21.SIC, newcat21.TargetID, newcat21.TrackNumber, newcat21.targetreport, newcat21.TOD, newcat21.positionWGS, newcat21.HRpositionWGS, newcat21.FlightLevel, newcat21.GeometricHeight, newcat21.operationalstatus, newcat21.AirSpeed, newcat21.TrueAirspeed, newcat21.AirborneGroundVector, newcat21.TrackAngleRate, newcat21.SelectedAltitude_IS, newcat21.SelectedAltitude_FS, newcat21.mopsv, newcat21.MagneticHeading, newcat21.BarometricVerticalRate, newcat21.GeometricVerticalRate, newcat21.Mode3ACode, newcat21.metreport, newcat21.ECAT, newcat21.TargetAddress, newcat21.targetstatus, newcat21.RateOfTurn, newcat21.RollAngle, newcat21.serviceID, newcat21.qualityindicators, newcat21.modes, newcat21.linktech, newcat21.RP, newcat21.MSGampl, newcat21.TrackAngleRate, newcat21.RID, newcat21.TimeOfApplicabilityForPosition_, newcat21.TimeOfApplicabilityForVelocity_, newcat21.TimeOfMessageReceptionForPosition_, newcat21.TimeOfMessageReceptionForVelocity_, newcat21.TimeOfMessageReceptionForPosition_HighPrecision_, newcat21.TimeOfMessageReceptionForVelocity_HighPrecision_, newcat21.TimeOfAsterixReportTransmission_, newcat21.trajectoryintentdata, newcat21.posAccuracy, newcat21.velAccuracy, newcat21.TODaccuracy, newcat21.figureofmerit, newcat21.ages_, newcat21.RP);
                }
            }
        }

        public void CrearTablas() //crea las columnas de la tabla, dependiendo de la cat
        {
            //CAT20
            tablaCAT20.Columns.Add(new DataColumn("#"));
            tablaCAT20.Columns.Add(new DataColumn("SAC"));
            tablaCAT20.Columns.Add(new DataColumn("SIC"));
            tablaCAT20.Columns.Add(new DataColumn("Target ID"));
            tablaCAT20.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20.Columns.Add(new DataColumn("Target Report Descriptor"));
            tablaCAT20.Columns.Add(new DataColumn("TOD"));
            tablaCAT20.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT20.Columns.Add(new DataColumn("Geometric Height (WSG-84)"));
            tablaCAT20.Columns.Add(new DataColumn("Calculated Acceleration"));
            tablaCAT20.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT20.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT20.Columns.Add(new DataColumn("Position Accuracy"));
            tablaCAT20.Columns.Add(new DataColumn("Contributing Devices (Receivers)"));
            tablaCAT20.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT20.Columns.Add(new DataColumn("Comms/ACAS Capability and Flight Status"));
            tablaCAT20.Columns.Add(new DataColumn("ACAS RA Report"));
            tablaCAT20.Columns.Add(new DataColumn("Warning/Error Conditions"));
            tablaCAT20.Columns.Add(new DataColumn("Mode 1 Code"));
            tablaCAT20.Columns.Add(new DataColumn("Mode 2 Code"));

            //CAT10
            tablaCAT10.Columns.Add(new DataColumn("#"));
            tablaCAT10.Columns.Add(new DataColumn("SAC"));
            tablaCAT10.Columns.Add(new DataColumn("SIC"));
            tablaCAT10.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10.Columns.Add(new DataColumn("Message type"));
            tablaCAT10.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Polar Coord\n(Ground Speed, Track Angle)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10.Columns.Add(new DataColumn("System Status"));
            tablaCAT10.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10.Columns.Add(new DataColumn("Presence"));
            tablaCAT10.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));

            //CAT21
            tablaCAT21.Columns.Add(new DataColumn("#"));
            tablaCAT21.Columns.Add(new DataColumn("SAC"));
            tablaCAT21.Columns.Add(new DataColumn("SIC"));
            tablaCAT21.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21.Columns.Add(new DataColumn("Target Report"));
            tablaCAT21.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21.Columns.Add(new DataColumn("Operational Status"));
            tablaCAT21.Columns.Add(new DataColumn("AirSpeed"));
            tablaCAT21.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Intermediate Stage\nSelected Altitude"));
            tablaCAT21.Columns.Add(new DataColumn("Final Stage\nSelected Altitude"));
            tablaCAT21.Columns.Add(new DataColumn("MOPS version"));
            tablaCAT21.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21.Columns.Add(new DataColumn("Met Report"));
            tablaCAT21.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21.Columns.Add(new DataColumn("Target Status"));
            tablaCAT21.Columns.Add(new DataColumn("Rate of Angle"));
            tablaCAT21.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21.Columns.Add(new DataColumn("Quality Indicators"));
            tablaCAT21.Columns.Add(new DataColumn("Mode S"));
            tablaCAT21.Columns.Add(new DataColumn("Link Technology"));
            tablaCAT21.Columns.Add(new DataColumn("Report period"));
            tablaCAT21.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21.Columns.Add(new DataColumn("Track angle rate"));
            tablaCAT21.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\for position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaCAT21.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaCAT21.Columns.Add(new DataColumn("Position accuracy"));
            tablaCAT21.Columns.Add(new DataColumn("Velocity accuracy"));
            tablaCAT21.Columns.Add(new DataColumn("Time of Day accuracy"));
            tablaCAT21.Columns.Add(new DataColumn("Figure of merit"));
            tablaCAT21.Columns.Add(new DataColumn("Data ages"));
            tablaCAT21.Columns.Add(new DataColumn("Service Management"));
        }

        public CAT20 getCAT20(int pack) //devuelve el paquete en la posición pack de la lista
        {
            return listaCAT20[pack];
        }

        public CAT10 getCAT10(int pack) //devuelve el paquete en la posición pack de la lista
        {
            return listaCAT10[pack];
        }

        public CAT21 getCAT21(int pack) //devuelve el paquete en la posición pack de la lista
        {
            return listaCAT21[pack];
        }

        public DataTable getTablaCAT10Indv(CAT10 newcat10, int q) //crea la tabla con el paquete dado como input (q es el número de paquete)
        {
            DataTable tablaCAT10i = new DataTable();

            tablaCAT10i.Columns.Add(new DataColumn("#"));
            tablaCAT10i.Columns.Add(new DataColumn("SAC"));
            tablaCAT10i.Columns.Add(new DataColumn("SIC"));
            tablaCAT10i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10i.Columns.Add(new DataColumn("Message type"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Polar Coords\n(Ground Speed, Track Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10i.Columns.Add(new DataColumn("System Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10i.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10i.Columns.Add(new DataColumn("Presence"));
            tablaCAT10i.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10i.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));

            tablaCAT10i.Rows.Add(q + 1, newcat10.SACnum, newcat10.SICnum, newcat10.TargetID, newcat10.TrackNumber, newcat10.TYP, newcat10.MessageType, newcat10.datachar_, newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.trackstatus_, newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.modes_, newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, newcat10.systemstatus, newcat10.MSG, newcat10.deviation, newcat10.covariance, newcat10.presence_, newcat10.amplitudePP, newcat10.acceleration);

            return tablaCAT10i;
        }

        public DataTable getTablaCAT20Indv(CAT20 newcat20, int q) //crea la tabla con el paquete dado como input (q es el número de paquete)
        {
            DataTable tablaCAT20i = new DataTable();

            tablaCAT20i.Columns.Add(new DataColumn("#"));
            tablaCAT20i.Columns.Add(new DataColumn("SAC"));
            tablaCAT20i.Columns.Add(new DataColumn("SIC"));
            tablaCAT20i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20i.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Report Descriptor"));
            tablaCAT20i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT20i.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT20i.Columns.Add(new DataColumn("Geometric Height (WSG-84)"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Acceleration"));
            tablaCAT20i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Accuracy"));
            tablaCAT20i.Columns.Add(new DataColumn("Contributing Devices (Receivers)"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT20i.Columns.Add(new DataColumn("Comms/ACAS Capability and Flight Status"));
            tablaCAT20i.Columns.Add(new DataColumn("ACAS RA Report"));
            tablaCAT20i.Columns.Add(new DataColumn("Warning/Error Conditions"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 1 Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 2 Code"));

            tablaCAT20i.Rows.Add(q + 1, newcat20.SAC, newcat20.SIC, newcat20.callsign, newcat20.TrackNum, newcat20.getMessageType(), newcat20.targetreport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.trackstatus, newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.posaccuracy, newcat20.Receivers, newcat20.ModeSData, newcat20.ACAScap, newcat20.ACASRAreport, newcat20.warning, newcat20.Mode1Code, newcat20.Mode2Code);

            return tablaCAT20i;
        }

        public DataTable getTablaCAT21Indv(CAT21 newcat21, int q) //crea la tabla con el paquete dado como input (q es el número de paquete)
        {
            DataTable tablaCAT21i = new DataTable();

            tablaCAT21i.Columns.Add(new DataColumn("#"));
            tablaCAT21i.Columns.Add(new DataColumn("SAC"));
            tablaCAT21i.Columns.Add(new DataColumn("SIC"));
            tablaCAT21i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21i.Columns.Add(new DataColumn("Operational Status"));
            tablaCAT21i.Columns.Add(new DataColumn("AirSpeed"));
            tablaCAT21i.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21i.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Intermediate Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Final Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("MOPS version"));
            tablaCAT21i.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21i.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21i.Columns.Add(new DataColumn("Met Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Status"));
            tablaCAT21i.Columns.Add(new DataColumn("Rate of Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21i.Columns.Add(new DataColumn("Quality Indicators"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode S"));
            tablaCAT21i.Columns.Add(new DataColumn("Link Technology"));
            tablaCAT21i.Columns.Add(new DataColumn("Report period"));
            tablaCAT21i.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Track angle rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21i.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaCAT21i.Columns.Add(new DataColumn("Position accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Velocity accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Day accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Figure of merit"));
            tablaCAT21i.Columns.Add(new DataColumn("Data ages"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Management"));

            tablaCAT21i.Rows.Add(q + 1, newcat21.SAC, newcat21.SIC, newcat21.TargetID, newcat21.TrackNumber, newcat21.targetreport, newcat21.TOD, newcat21.positionWGS, newcat21.HRpositionWGS, newcat21.FlightLevel, newcat21.GeometricHeight, newcat21.operationalstatus, newcat21.AirSpeed, newcat21.TrueAirspeed, newcat21.AirborneGroundVector, newcat21.TrackAngleRate, newcat21.SelectedAltitude_IS, newcat21.SelectedAltitude_FS, newcat21.mopsv, newcat21.MagneticHeading, newcat21.BarometricVerticalRate, newcat21.GeometricVerticalRate, newcat21.Mode3ACode, newcat21.metreport, newcat21.ECAT, newcat21.TargetAddress, newcat21.targetstatus, newcat21.RateOfTurn, newcat21.RollAngle, newcat21.serviceID, newcat21.qualityindicators, newcat21.modes, newcat21.linktech, newcat21.RP, newcat21.MSGampl, newcat21.TrackAngleRate, newcat21.RID, newcat21.TimeOfApplicabilityForPosition_, newcat21.TimeOfApplicabilityForVelocity_, newcat21.TimeOfMessageReceptionForPosition_, newcat21.TimeOfMessageReceptionForVelocity_, newcat21.TimeOfMessageReceptionForPosition_HighPrecision_, newcat21.TimeOfMessageReceptionForVelocity_HighPrecision_, newcat21.TimeOfAsterixReportTransmission_, newcat21.trajectoryintentdata, newcat21.posAccuracy, newcat21.velAccuracy, newcat21.TODaccuracy, newcat21.figureofmerit, newcat21.ages_, newcat21.RP);

            return tablaCAT21i;
        }

        public bool ComprobarCAT10() // comprueba si hay elementos en la lista (true=si hay, false=no hay)
        {
            if (this.listaCAT10.Count() != 0)
                return true;
            else
                return false;
        }

        public bool ComprobarCAT20() // comprueba si hay elementos en la lista (true=si hay, false=no hay)
        {
            if (this.listaCAT20.Count() != 0)
                return true;
            else
                return false;
        }

        public bool ComprobarCAT21() // comprueba si hay elementos en la lista (true=si hay, false=no hay)
        {
            if (this.listaCAT21.Count() != 0)
                return true;
            else
                return false;
        }

        public List<String> getcallsignsCAT20()
        {
            List<String> callsigns = new List<string>();

            if (this.listaCAT20.Count != 0)
            {
                for (int i = 0; i < this.listaCAT20.Count; i++)
                {
                    if(callsigns.Contains(this.listaCAT20[i].callsign) == false)
                    {
                        callsigns.Add(this.listaCAT20[i].callsign);
                    }
                }
            }
            return callsigns;
        }

        public void computeFlights()
        {
            List<String> tracknums = new List<string>();
            List<String> callsigns = new List<string>();

            if (this.listaCAT20.Count != 0)
            {
                for (int c = 0; c < this.listaCAT20.Count; c++)
                {


                    if (tracknums.Contains(this.listaCAT20[c].TrackNum) == false)
                    {
                        tracknums.Add(this.listaCAT20[c].TrackNum);
                        listflights.Add(new Flight(this.listaCAT20[c], c));
                    }
                    else
                    {
                        foreach (Flight flight in this.listflights)
                        {
                            if (flight.tracknumber == listaCAT20[c].TrackNum)
                            {
                                flight.updateFlight(listaCAT20[c], c);
                            }
                        }
                    }

                }
            }

            if( this.listaCAT10.Count != 0)
            {
                for (int c = 0; c < this.listaCAT10.Count; c++)
                {


                    if (tracknums.Contains(this.listaCAT10[c].TrackNumber) == false)
                    {
                        tracknums.Add(this.listaCAT10[c].TrackNumber);
                        listflights.Add(new Flight(this.listaCAT10[c], c));
                    }
                    else
                    {
                        foreach (Flight flight in this.listflights)
                        {
                            if (flight.tracknumber == listaCAT10[c].TrackNumber)
                            {
                                flight.updateFlight(listaCAT10[c], c);
                            }
                        }
                    }

                }
            }

            if (this.listaCAT21.Count != 0)
            {
                for (int c = 0; c < this.listaCAT21.Count; c++)
                {


                    if (callsigns.Contains(Convert.ToString(this.listaCAT21[c].TargetID)) == false)
                    {
                        callsigns.Add(Convert.ToString(this.listaCAT21[c].TargetID));
                        listflights.Add(new Flight(this.listaCAT21[c], c));
                    }
                    else
                    {
                        foreach (Flight flight in this.listflights)
                        {
                            if (flight.callsign == Convert.ToString(listaCAT21[c].TargetID))
                            {
                                flight.updateFlight(listaCAT21[c], c);
                            }
                        }
                    }

                }
            }
        }

        public List<Flight> getFlightList()
        {
            return this.listflights;
        }

        public DataTable SearchCallsignCAT10(string callsign) //buscar paquetes por el target id 
        {
            DataTable tablaCAT10i = new DataTable();

            tablaCAT10i.Columns.Add(new DataColumn("#"));
            tablaCAT10i.Columns.Add(new DataColumn("SAC"));
            tablaCAT10i.Columns.Add(new DataColumn("SIC"));
            tablaCAT10i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10i.Columns.Add(new DataColumn("Message type"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Polar Coords\n(Ground Speed, Track Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10i.Columns.Add(new DataColumn("System Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10i.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10i.Columns.Add(new DataColumn("Presence"));
            tablaCAT10i.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10i.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));

            int q = 0;
            while (q < listaCAT10.Count)
            {
                CAT10 newcat10 = listaCAT10[q];
                if (newcat10.TargetID == callsign)
                    tablaCAT10i.Rows.Add(q + 1, newcat10.SACnum, newcat10.SICnum, newcat10.TargetID, newcat10.TrackNumber, newcat10.TYP, newcat10.MessageType, newcat10.datachar_, newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.trackstatus_, newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.modes_, newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, newcat10.systemstatus, newcat10.MSG, newcat10.deviation, newcat10.covariance, newcat10.presence_, newcat10.amplitudePP, newcat10.acceleration);
                q++;
            }
            return tablaCAT10i;
        }

        public DataTable SearchCallsignCAT20(string callsign) //buscar paquetes por el target id 
        {
            DataTable tablaCAT20i = new DataTable();

            tablaCAT20i.Columns.Add(new DataColumn("#"));
            tablaCAT20i.Columns.Add(new DataColumn("SAC"));
            tablaCAT20i.Columns.Add(new DataColumn("SIC"));
            tablaCAT20i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20i.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Report Descriptor"));
            tablaCAT20i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT20i.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT20i.Columns.Add(new DataColumn("Geometric Height (WSG-84)"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Acceleration"));
            tablaCAT20i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Accuracy"));
            tablaCAT20i.Columns.Add(new DataColumn("Contributing Devices (Receivers)"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT20i.Columns.Add(new DataColumn("Comms/ACAS Capability and Flight Status"));
            tablaCAT20i.Columns.Add(new DataColumn("ACAS RA Report"));
            tablaCAT20i.Columns.Add(new DataColumn("Warning/Error Conditions"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 1 Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 2 Code"));

            string callsR = callsign.PadRight(8, ' ');
            string callsL = callsign.PadLeft(8, ' ');

            int q = 0;
            while (q < listaCAT20.Count)
            {
                CAT20 newcat20 = listaCAT20[q];
                if (newcat20.callsign == callsR || newcat20.callsign == callsL)
                    tablaCAT20i.Rows.Add(q + 1, newcat20.SAC, newcat20.SIC, newcat20.callsign, newcat20.TrackNum, newcat20.getMessageType(), newcat20.targetreport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.trackstatus, newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.posaccuracy, newcat20.Receivers, newcat20.ModeSData, newcat20.ACAScap, newcat20.ACASRAreport, newcat20.warning, newcat20.Mode1Code, newcat20.Mode2Code);
                q++;
            }
            return tablaCAT20i;
        }

        public DataTable SearchCallsignCAT21(string callsign) //buscar paquetes por el target id 
        {
            DataTable tablaCAT21i = new DataTable();

            tablaCAT21i.Columns.Add(new DataColumn("#"));
            tablaCAT21i.Columns.Add(new DataColumn("SAC"));
            tablaCAT21i.Columns.Add(new DataColumn("SIC"));
            tablaCAT21i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21i.Columns.Add(new DataColumn("Operational Status"));
            tablaCAT21i.Columns.Add(new DataColumn("AirSpeed"));
            tablaCAT21i.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21i.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Intermediate Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Final Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("MOPS version"));
            tablaCAT21i.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21i.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21i.Columns.Add(new DataColumn("Met Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Status"));
            tablaCAT21i.Columns.Add(new DataColumn("Rate of Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21i.Columns.Add(new DataColumn("Quality Indicators"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode S"));
            tablaCAT21i.Columns.Add(new DataColumn("Link Technology"));
            tablaCAT21i.Columns.Add(new DataColumn("Report period"));
            tablaCAT21i.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Track angle rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21i.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaCAT21i.Columns.Add(new DataColumn("Position accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Velocity accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Day accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Figure of merit"));
            tablaCAT21i.Columns.Add(new DataColumn("Data ages"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Management"));

            int q = 0;
            while (q < listaCAT21.Count)
            {
                CAT21 newcat21 = listaCAT21[q];
                if (newcat21.TargetID == callsign)
                    tablaCAT21i.Rows.Add(q + 1, newcat21.SAC, newcat21.SIC, newcat21.TargetID, newcat21.TrackNumber, newcat21.targetreport, newcat21.TOD, newcat21.positionWGS, newcat21.HRpositionWGS, newcat21.FlightLevel, newcat21.GeometricHeight, newcat21.operationalstatus, newcat21.AirSpeed, newcat21.TrueAirspeed, newcat21.AirborneGroundVector, newcat21.TrackAngleRate, newcat21.SelectedAltitude_IS, newcat21.SelectedAltitude_FS, newcat21.mopsv, newcat21.MagneticHeading, newcat21.BarometricVerticalRate, newcat21.GeometricVerticalRate, newcat21.Mode3ACode, newcat21.metreport, newcat21.ECAT, newcat21.TargetAddress, newcat21.targetstatus, newcat21.RateOfTurn, newcat21.RollAngle, newcat21.serviceID, newcat21.qualityindicators, newcat21.modes, newcat21.linktech, newcat21.RP, newcat21.MSGampl, newcat21.TrackAngleRate, newcat21.RID, newcat21.TimeOfApplicabilityForPosition_, newcat21.TimeOfApplicabilityForVelocity_, newcat21.TimeOfMessageReceptionForPosition_, newcat21.TimeOfMessageReceptionForVelocity_, newcat21.TimeOfMessageReceptionForPosition_HighPrecision_, newcat21.TimeOfMessageReceptionForVelocity_HighPrecision_, newcat21.TimeOfAsterixReportTransmission_, newcat21.trajectoryintentdata, newcat21.posAccuracy, newcat21.velAccuracy, newcat21.TODaccuracy, newcat21.figureofmerit, newcat21.ages_, newcat21.RP);
                q++;
            }

            return tablaCAT21i;
        }

        public DataTable SearchTrackNumberCAT10(string tracknumber) //buscar paquetes por el target address 
        {
            DataTable tablaCAT10i = new DataTable();

            tablaCAT10i.Columns.Add(new DataColumn("#"));
            tablaCAT10i.Columns.Add(new DataColumn("SAC"));
            tablaCAT10i.Columns.Add(new DataColumn("SIC"));
            tablaCAT10i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10i.Columns.Add(new DataColumn("Message type"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Polar Coords\n(Distance, Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Cartesian Coords\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Polar Coords\n(Ground Speed, Track Angle)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n(Vx, Vy)"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Size\n(Length x Width)"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10i.Columns.Add(new DataColumn("System Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10i.Columns.Add(new DataColumn("Standard Deviation of Position\n(X, Y)"));
            tablaCAT10i.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10i.Columns.Add(new DataColumn("Presence"));
            tablaCAT10i.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10i.Columns.Add(new DataColumn("Calculated Acceleration\n(Ax, Ay)"));

            int q = 0;
            while (q < listaCAT10.Count)
            {
                CAT10 newcat10 = listaCAT10[q];
                if (newcat10.TrackNumber == tracknumber)
                    tablaCAT10i.Rows.Add(q + 1, newcat10.SACnum, newcat10.SICnum, newcat10.TargetID, newcat10.TrackNumber, newcat10.TYP, newcat10.MessageType, newcat10.datachar_, newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.trackstatus_, newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.modes_, newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, newcat10.systemstatus, newcat10.MSG, newcat10.deviation, newcat10.covariance, newcat10.presence_, newcat10.amplitudePP, newcat10.acceleration);
                q++;
            }
            return tablaCAT10i;
        }

        public DataTable SearchTrackNumberCAT20(string tracknumber) //buscar paquetes por el target address
        {
            DataTable tablaCAT20i = new DataTable();

            tablaCAT20i.Columns.Add(new DataColumn("#"));
            tablaCAT20i.Columns.Add(new DataColumn("SAC"));
            tablaCAT20i.Columns.Add(new DataColumn("SIC"));
            tablaCAT20i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20i.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Report Descriptor"));
            tablaCAT20i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT20i.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT20i.Columns.Add(new DataColumn("Geometric Height (WSG-84)"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Acceleration"));
            tablaCAT20i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT20i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Accuracy"));
            tablaCAT20i.Columns.Add(new DataColumn("Contributing Devices (Receivers)"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT20i.Columns.Add(new DataColumn("Comms/ACAS Capability and Flight Status"));
            tablaCAT20i.Columns.Add(new DataColumn("ACAS RA Report"));
            tablaCAT20i.Columns.Add(new DataColumn("Warning/Error Conditions"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 1 Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 2 Code"));

            int q = 0;
            while (q < listaCAT20.Count)
            {
                CAT20 newcat20 = listaCAT20[q];
                if (newcat20.TrackNum == tracknumber)
                    tablaCAT20i.Rows.Add(q + 1, newcat20.SAC, newcat20.SIC, newcat20.callsign, newcat20.TrackNum, newcat20.getMessageType(), newcat20.targetreport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.trackstatus, newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.posaccuracy, newcat20.Receivers, newcat20.ModeSData, newcat20.ACAScap, newcat20.ACASRAreport, newcat20.warning, newcat20.Mode1Code, newcat20.Mode2Code);
                q++;
            }
            return tablaCAT20i;
        }

        public DataTable SearchTrackNumberCAT21(string tracknumber) //buscar paquetes por el target address 
        {
            DataTable tablaCAT21i = new DataTable();

            tablaCAT21i.Columns.Add(new DataColumn("#"));
            tablaCAT21i.Columns.Add(new DataColumn("SAC"));
            tablaCAT21i.Columns.Add(new DataColumn("SIC"));
            tablaCAT21i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Time Of Day"));
            tablaCAT21i.Columns.Add(new DataColumn("Position WSG-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("High Resolution Position WGS-84\n(Latitude, Longitude)"));
            tablaCAT21i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Height"));
            tablaCAT21i.Columns.Add(new DataColumn("Operational Status"));
            tablaCAT21i.Columns.Add(new DataColumn("AirSpeed"));
            tablaCAT21i.Columns.Add(new DataColumn("True Airspeed"));
            tablaCAT21i.Columns.Add(new DataColumn("Airborne Ground Vector\n(Ground Speed, Track Angle)"));
            tablaCAT21i.Columns.Add(new DataColumn("Track Angle Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Intermediate Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Final Stage\nSelected Altitude"));
            tablaCAT21i.Columns.Add(new DataColumn("MOPS version"));
            tablaCAT21i.Columns.Add(new DataColumn("Magnetic Heading"));
            tablaCAT21i.Columns.Add(new DataColumn("Barometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Geometric Vertical Rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT21i.Columns.Add(new DataColumn("Met Report"));
            tablaCAT21i.Columns.Add(new DataColumn("Emitter Category"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT21i.Columns.Add(new DataColumn("Target Status"));
            tablaCAT21i.Columns.Add(new DataColumn("Rate of Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Roll Angle"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Identification"));
            tablaCAT21i.Columns.Add(new DataColumn("Quality Indicators"));
            tablaCAT21i.Columns.Add(new DataColumn("Mode S"));
            tablaCAT21i.Columns.Add(new DataColumn("Link Technology"));
            tablaCAT21i.Columns.Add(new DataColumn("Report period"));
            tablaCAT21i.Columns.Add(new DataColumn("Message amplitude"));
            tablaCAT21i.Columns.Add(new DataColumn("Track angle rate"));
            tablaCAT21i.Columns.Add(new DataColumn("Receiver ID"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Applicability\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor position - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Message Reception\nfor velocity - High precision"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of ASTERIX\nReport Transmission"));
            tablaCAT21i.Columns.Add(new DataColumn("Trajectory Intent Data"));
            tablaCAT21i.Columns.Add(new DataColumn("Position accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Velocity accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Time of Day accuracy"));
            tablaCAT21i.Columns.Add(new DataColumn("Figure of merit"));
            tablaCAT21i.Columns.Add(new DataColumn("Data ages"));
            tablaCAT21i.Columns.Add(new DataColumn("Service Management"));

            int q = 0;
            while (q < listaCAT21.Count)
            {
                CAT21 newcat21 = listaCAT21[q];
                if (newcat21.TrackNumber == tracknumber)
                    tablaCAT21i.Rows.Add(q + 1, newcat21.SAC, newcat21.SIC, newcat21.TargetID, newcat21.TrackNumber, newcat21.targetreport, newcat21.TOD, newcat21.positionWGS, newcat21.HRpositionWGS, newcat21.FlightLevel, newcat21.GeometricHeight, newcat21.operationalstatus, newcat21.AirSpeed, newcat21.TrueAirspeed, newcat21.AirborneGroundVector, newcat21.TrackAngleRate, newcat21.SelectedAltitude_IS, newcat21.SelectedAltitude_FS, newcat21.mopsv, newcat21.MagneticHeading, newcat21.BarometricVerticalRate, newcat21.GeometricVerticalRate, newcat21.Mode3ACode, newcat21.metreport, newcat21.ECAT, newcat21.TargetAddress, newcat21.targetstatus, newcat21.RateOfTurn, newcat21.RollAngle, newcat21.serviceID, newcat21.qualityindicators, newcat21.modes, newcat21.linktech, newcat21.RP, newcat21.MSGampl, newcat21.TrackAngleRate, newcat21.RID, newcat21.TimeOfApplicabilityForPosition_, newcat21.TimeOfApplicabilityForVelocity_, newcat21.TimeOfMessageReceptionForPosition_, newcat21.TimeOfMessageReceptionForVelocity_, newcat21.TimeOfMessageReceptionForPosition_HighPrecision_, newcat21.TimeOfMessageReceptionForVelocity_HighPrecision_, newcat21.TimeOfAsterixReportTransmission_, newcat21.trajectoryintentdata, newcat21.posAccuracy, newcat21.velAccuracy, newcat21.TODaccuracy, newcat21.figureofmerit, newcat21.ages_, newcat21.RP);
                q++;
            }

            return tablaCAT21i;
        }

        public Boolean checkMultiplecat()
        {
            int i = 0;
            if (listaCAT10.Count != 0) { i++; }
            if(listaCAT20.Count != 0) { i++; }
            if(listaCAT21.Count != 0) { i++; }

            if (i == 1) { return false; }
            else { return true; }
        }
    }
}
