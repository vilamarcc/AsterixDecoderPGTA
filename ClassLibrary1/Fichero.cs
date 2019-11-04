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
        string path;
        List<CAT10> listaCAT10 = new List<CAT10>();
        List<CAT20> listaCAT20 = new List<CAT20>();
        List<CAT21> listaCAT21 = new List<CAT21>();
        DataTable tablaCAT10 = new DataTable();
        DataTable tablaCAT20 = new DataTable();
        DataTable tablaCAT21 = new DataTable();

    


    public Fichero(string nombre)
        {
            this.path = nombre;

            //Load columns for each table (Poner CAT21 y CAT10)
            //CAT20
            tablaCAT20.Columns.Add(new DataColumn("#"));
            tablaCAT20.Columns.Add(new DataColumn("SAC"));
            tablaCAT20.Columns.Add(new DataColumn("SIC"));
            tablaCAT20.Columns.Add(new DataColumn("Target Report"));
            tablaCAT20.Columns.Add(new DataColumn("TOD"));
            tablaCAT20.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20.Columns.Add(new DataColumn("Mode 3/A Code"));
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

        }

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

        public void Read()
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
                {
                    contador = fileBytes[i + 2];
                }


            }


            List<string[]> listahex = new List<string[]>();
            for (int x = 0; x < listabyte.Count; x++)
            {
                byte[] buffer = listabyte[x];
                string[] arrayhex = new string[buffer.Length];
                for (int y = 0; y < buffer.Length; y++)
                {
                    arrayhex[y] = buffer[y].ToString("X");
                }
                listahex.Add(arrayhex);
            }

            for (int q = 0; q < listahex.Count; q++)
            {
                string[] arraystring = listahex[q];
                int CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);

                if (CAT == 10)
                {
                    CAT10 newcat10 = new CAT10(arraystring);
                    listaCAT10.Add(newcat10);
                }
                else if (CAT == 20)
                {
                    CAT20 newcat20 = new CAT20(arraystring);
                    listaCAT20.Add(newcat20);
                    tablaCAT20.Rows.Add(q + 1, newcat20.SAC, newcat20.SIC, newcat20.TargetReport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.TrackNum, "Click to expand", newcat20.Mode3A.ToString(), "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.callsign, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.DOP, newcat20.Receivers, newcat20.ModeSData); ;
                
                }
                else if (CAT == 21)
                {
                    CAT21 newcat21 = new CAT21(arraystring);
                    listaCAT21.Add(newcat21);
                }
            }
            i++;
            

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

        public CAT20 getCAT20(int pack)
        {
            return listaCAT20[pack];
        }

        public DataTable getTablaCAT20Indv(CAT20 newcat20, int q)
        {
            DataTable tablaCAT20i = new DataTable();

            tablaCAT20i.Columns.Add(new DataColumn("#"));
            tablaCAT20i.Columns.Add(new DataColumn("SAC"));
            tablaCAT20i.Columns.Add(new DataColumn("SIC"));
            tablaCAT20i.Columns.Add(new DataColumn("Target Report"));
            tablaCAT20i.Columns.Add(new DataColumn("TOD"));
            tablaCAT20i.Columns.Add(new DataColumn("Position WSG-84"));
            tablaCAT20i.Columns.Add(new DataColumn("Position Cartesian Coords"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Number"));
            tablaCAT20i.Columns.Add(new DataColumn("Track Status"));
            tablaCAT20i.Columns.Add(new DataColumn("Mode 3/A Code"));
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

            tablaCAT20i.Rows.Add(q + 1, newcat20.SAC, newcat20.SIC, newcat20.TargetReport, newcat20.TOD, newcat20.LonWSG, "[" + newcat20.coordscc[0].ToString() + "," + newcat20.coordscc[1].ToString() + "] m", newcat20.TrackNum, "Click to expand", newcat20.Mode3A, "[" + newcat20.Velocitycc[0].ToString() + "," + newcat20.Velocitycc[1].ToString() + "] m/s", newcat20.FL[2], newcat20.ModeC, newcat20.TargetAddress, newcat20.TargetID, newcat20.MeasuredHeight, newcat20.geoHeight, newcat20.calcAccel, newcat20.VehicleFleetID, newcat20.PPMsg, newcat20.DOP, newcat20.Receivers, newcat20.ModeSData);

            return tablaCAT20i;
        }
    }
}
