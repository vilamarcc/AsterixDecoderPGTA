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

        //tablas con los datos --> las cargamos cuando tengamos las listas
        DataTable tablaCAT10 = new DataTable();
        DataTable tablaCAT20 = new DataTable();
        DataTable tablaCAT21 = new DataTable();

        // CONSTRUCTOR:
        public Fichero(string nombre)
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
        public void Read() //lee el fichero y crea las listas y las tablas
        {
            //StreamReader fichero = new StreamReader(path);
            //string linea_1 = fichero.ReadLine();
            byte[] fileBytes = File.ReadAllBytes(path);
            List<byte[]> listabyte = new List<byte[]>();
            int i = 0;
            int contador = fileBytes[2];
            //int length = 0;

            while (i < fileBytes.Length)
            {
                byte[] array = new byte[contador];
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = fileBytes[i];
                    i++;
                }
                listabyte.Add(array);
                //length += array.Length;
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
                    tablaCAT10.Rows.Add(contadorCAT10, newcat10.MessageType, newcat10.SACnum, newcat10.SICnum, newcat10.TYP, "Click to expand", newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.TrackNumber, "Click to expand", newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.TargetID, "Click to expand", newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, "Click to expand", newcat10.MSG, newcat10.deviation, newcat10.covariance, "Click to expand", newcat10.amplitudePP, newcat10.acceleration);
                }
                else if (CAT == 20)
                {
                    contadorCAT20++;
                    //llegim
                    CAT20 newcat20 = new CAT20(arraystring);
                    //afegim a la llista
                    listaCAT20.Add(newcat20);
                    //afegim a la taula
                    tablaCAT20.Rows.Add(contadorCAT20, newcat20.getMessageType(), newcat20.SAC, newcat20.SIC, "Click to expand", newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.TrackNum, "Click to expand", newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.callsign, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, "Click to expand", newcat20.Receivers, newcat20.ModeSData,newcat20.ACAScap, newcat20.ACASRAreport, newcat20.warning, newcat20.Mode1Code, newcat20.Mode2Code);
                }
                else if (CAT == 21)
                {
                    contadorCAT21++;
                    //llegim
                    CAT21 newcat21 = new CAT21(arraystring);
                    //afegim a la llista
                    listaCAT21.Add(newcat21);
                    //afegim a la taula

                }
            }
            i++;
        }

        public void CrearTablas()
        {
            //CAT20
            tablaCAT20.Columns.Add(new DataColumn("#"));
            tablaCAT20.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20.Columns.Add(new DataColumn("SAC"));
            tablaCAT20.Columns.Add(new DataColumn("SIC"));
            tablaCAT20.Columns.Add(new DataColumn("Target Report Descriptor"));
            tablaCAT20.Columns.Add(new DataColumn("TOD"));
            tablaCAT20.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20.Columns.Add(new DataColumn("Target ID"));
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
            tablaCAT10.Columns.Add(new DataColumn("Message type"));
            tablaCAT10.Columns.Add(new DataColumn("SAC"));
            tablaCAT10.Columns.Add(new DataColumn("SIC"));
            tablaCAT10.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10.Columns.Add(new DataColumn("Position WSG-84\n[Latitude, Longitude]"));
            tablaCAT10.Columns.Add(new DataColumn("Position Polar Coords\n[Distance, Angle]"));
            tablaCAT10.Columns.Add(new DataColumn("Position Cartesian Coords\n[X, Y]"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Polar Coords\n[Ground Speed, Track Angle]"));
            tablaCAT10.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n[Vx, Vy]"));
            tablaCAT10.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10.Columns.Add(new DataColumn("Target Size\n[Length x Width]"));
            tablaCAT10.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10.Columns.Add(new DataColumn("System Status"));
            tablaCAT10.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10.Columns.Add(new DataColumn("Standard Deviation of Position\n[X, Y]"));
            tablaCAT10.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10.Columns.Add(new DataColumn("Presence"));
            tablaCAT10.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10.Columns.Add(new DataColumn("Calculated Acceleration\n[Ax, Ay]"));

            //CAT21

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

        public DataTable getTablaCAT10Indv(CAT10 newcat10, int q)
        {
            DataTable tablaCAT10i = new DataTable();

            tablaCAT10i.Columns.Add(new DataColumn("#"));
            tablaCAT10i.Columns.Add(new DataColumn("Message type"));
            tablaCAT10i.Columns.Add(new DataColumn("SAC"));
            tablaCAT10i.Columns.Add(new DataColumn("SIC"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Type"));
            tablaCAT10i.Columns.Add(new DataColumn("Data Characteristics"));
            tablaCAT10i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT10i.Columns.Add(new DataColumn("Position WSG-84\n[Latitude, Longitude]"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Polar Coords\n[Distance, Angle]"));
            tablaCAT10i.Columns.Add(new DataColumn("Position Cartesian Coords\n[X, Y]"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Polar Coords\n[Ground Speed, Track Angle]"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Velocity Cartesian Coords\n[Vx, Vy]"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT10i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT10i.Columns.Add(new DataColumn("Target ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Mode S MB Data"));
            tablaCAT10i.Columns.Add(new DataColumn("Vehicle Fleet ID"));
            tablaCAT10i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT10i.Columns.Add(new DataColumn("Measured Height"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Size\n[Length x Width]"));
            tablaCAT10i.Columns.Add(new DataColumn("Target Orientation"));
            tablaCAT10i.Columns.Add(new DataColumn("System Status"));
            tablaCAT10i.Columns.Add(new DataColumn("Pre Programmed MSG"));
            tablaCAT10i.Columns.Add(new DataColumn("Standard Deviation of Position\n[X, Y]"));
            tablaCAT10i.Columns.Add(new DataColumn("Covariance of deviation"));
            tablaCAT10i.Columns.Add(new DataColumn("Presence"));
            tablaCAT10i.Columns.Add(new DataColumn("Amplitude of Primary Plot"));
            tablaCAT10i.Columns.Add(new DataColumn("Calculated Acceleration\n[Ax, Ay]"));

            tablaCAT10i.Rows.Add(q + 1, newcat10.MessageType, newcat10.SACnum, newcat10.SICnum, newcat10.TYP, "Click to expand", newcat10.TimeOfDay, newcat10.positionWGS, newcat10.positionPolar, newcat10.positionCartesian, newcat10.velocityPolar, newcat10.velocityCartesian, newcat10.TrackNumber, "Click to expand", newcat10.Mode3ACode, newcat10.TargetAddress, newcat10.TargetID, "Click to expand", newcat10.VFI, newcat10.FlightLevel, newcat10.MeasuredHeight, newcat10.TargetSize, newcat10.TargetOrientation_, "Click to expand", newcat10.MSG, newcat10.deviation, newcat10.covariance, "Click to expand", newcat10.amplitudePP, newcat10.acceleration);

            return tablaCAT10i;
        }

        public DataTable getTablaCAT20Indv(CAT20 newcat20, int q)
        {
            DataTable tablaCAT20i = new DataTable();

            tablaCAT20i.Columns.Add(new DataColumn("#"));
            tablaCAT20i.Columns.Add(new DataColumn("Message Type"));
            tablaCAT20i.Columns.Add(new DataColumn("SAC"));
            tablaCAT20i.Columns.Add(new DataColumn("SIC"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Report \n   Descriptor"));
            tablaCAT20i.Columns.Add(new DataColumn("Time Of Day (UTC)"));
            tablaCAT20i.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 3A Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Calculated Velocity"));
            tablaCAT20i.Columns.Add(new DataColumn("Flight Level"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode C Code"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Address"));
            tablaCAT20i.Columns.Add(new DataColumn("Target ID"));
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

            tablaCAT20i.Rows.Add(q + 1, newcat20.getMessageType(), newcat20.SAC, newcat20.SIC, newcat20.TargetReport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.TrackNum, "Click to expand", newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.TargetID, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.DOP, newcat20.Receivers, newcat20.ModeSData,newcat20.ACAScap,newcat20.ACASRAreport,newcat20.warning,newcat20.Mode1Code,newcat20.Mode2Code);

            return tablaCAT20i;
        }

        public DataTable getTablaCAT21Indv(CAT21 newcat21, int q)
        {
            DataTable tablaCAT21i = new DataTable();



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
    }
}
