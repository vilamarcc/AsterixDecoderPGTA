using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsterixDecoder;

namespace AsterixDecoder
{
    public class CAT20
    {
        //Los atributos se guardan en binario y solo se decodifican si son llamados
        string SAC;
        string SIC;
        string TargetReport; //getTargetReport desglosa la informacion contenida dentro del data item
        string TOD;
        string LatWSG;
        string LonWSG;
        string X;
        string Y;
        string FSPEC;
        string TrackNum;
        string TrackStatus; //getTrackStatus devuelve todo los datos del data item desglosado
        string Mode3A; // getMode3A desglosa la informacion
        string Vx;
        string Vy;
        string FL;
        string ModeC;
        string TargetID; //getTargetID desglosa los datos en binario y devuelve la identificacion
        string TargetAddress;
        string MeasuredHeight;
        string geoHeight;
        string calcAccel;
        string VehicleFleetID;
        string PPMsg;
        string DOP; //Forma parte del paquete 500 Position accuracy, la funcion getDOP lo desglosa y descifra
        string SDP; //Forma parte del paquete 500 Position accuracy, la funcion getSDP lo desglosa y descifra
        string SDH; //Forma parte del paquete 500 Position accuracy, la funcion getSDH lo desglosa y descifra
        string ContributingDevices;
        string ModeS;
        int length;

        public CAT20(string[] array)
        {
            int Length = int.Parse(array[1] + array[2], System.Globalization.NumberStyles.HexNumber);
            this.length = Length + 3; // Las 3 primeras posiciones ya leidas
            computeFSPEC(array);

            string FSPEC_copia = this.FSPEC;
            int LengthFSPEC = this.FSPEC.Length / 8; //Longitud del FSPEC

            int OctetoIndex = LengthFSPEC + 3;
            int i = 0;

            //Orden establecido por UAP (Standard) para MLAT
            //I020/010 DataSource IDentifier
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                this.SAC = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                this.SIC = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
            }
            i++;

            //I020/020, Target Report Descriptor
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_020 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                this.TargetReport = Octeto_020;
                OctetoIndex++;

                if (Convert.ToString(Octeto_020[7]) == "1")
                {
                    string Octeto_021 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    this.TargetReport = this.TargetReport + Octeto_021;
                    OctetoIndex++;
                }
            }
            i++;

            //I020/140 Time of Day in UTC (3 octetos)
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_140_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_140_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_140_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                this.TOD = Octeto_140_1 + Octeto_140_2 + Octeto_140_3;
            }
            i++;

            //I020/041 Position in WGS-84 Coordinates
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_041_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_7 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_041_8 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;

                this.LatWSG = Octeto_041_1 + Octeto_041_2 + Octeto_041_3 + Octeto_041_4;
                this.LonWSG = Octeto_041_5 + Octeto_041_6 + Octeto_041_7 + Octeto_041_8;
            }
            i++;

            //I020/042 Position in Cartesian Coordinates
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_042_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_042_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_042_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_042_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_042_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_042_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;

                this.X = Octeto_042_1 + Octeto_042_2 + Octeto_042_3;
                this.Y = Octeto_042_4 + Octeto_042_5 + Octeto_042_6;
            }
            i++;

            //I020/161 Track Number
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_161_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_161_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;

                this.TrackNum = Octeto_161_1 + Octeto_161_2;
            }
            i++;

            //I020/170 Track status
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_170 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                this.TrackStatus = Octeto_170;
            }
            i++;

            // Primera Extensión
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                i++;

                //I020/070 Mode 3A code
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_070_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_070_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.Mode3A = Octeto_070_1 + Octeto_070_2;
                }
                i++;

                //I020/202 Calculated Track Velocity in cartesian coordinates
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_202_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_202_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_202_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_202_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.Vx = Octeto_202_1 + Octeto_202_2;
                    this.Vy = Octeto_202_3 + Octeto_202_4;
                }
                i++;

                //I020/090 Flight Level
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_090_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_090_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.FL = Octeto_090_1 + Octeto_090_2;
                }
                i++;

                //I020/100 Mode C Code
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_100_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_100_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_100_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_100_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.ModeC = Octeto_100_1 + Octeto_100_2 + Octeto_100_3 + Octeto_100_4;
                }
                i++;

                //I020/220 Target Address
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_220_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_220_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_220_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.TargetAddress = Octeto_220_1 + Octeto_220_2 + Octeto_220_3;
                }
                i++;

                //I020/245 Target Identification
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_245_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_245_7 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.TargetID = Octeto_245_1 + Octeto_245_2 + Octeto_245_3 + Octeto_245_4 + Octeto_245_5 + Octeto_245_6 + Octeto_245_7;
                }
                i++;

                //I020/110 Measured Height (Cartessian Coordinates)
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_110_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_110_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.MeasuredHeight = Octeto_110_1 + Octeto_110_2;
                }
                i++;


                //Segunda Extensión
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    i++;

                    //I020/105 GeoMetric Height (WSG84)
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_105_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_105_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;

                        this.geoHeight = Octeto_105_1 + Octeto_105_2;
                    }
                    i++;

                    //I020/210 Calculated Acceleration
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_210_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_210_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;

                        this.calcAccel = Octeto_210_1 + Octeto_210_2;
                    }
                    i++;

                    //I020/300 Vehicle Fleet Indentification
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_300_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;

                        this.VehicleFleetID = Octeto_300_1;
                    }
                    i++;

                    //I020/310 Pre-Programmed Message
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_310_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;

                        this.PPMsg = Octeto_310_1;
                    }
                    i++;

                    //I020/500 PositionAccuracy
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_500_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;

                        string subfield_1 = Convert.ToString(Octeto_500_1[0]);
                        string subfield_2 = Convert.ToString(Octeto_500_1[1]);
                        string subfield_3 = Convert.ToString(Octeto_500_1[2]);
                        if (subfield_1 == "1")
                        {
                            string Octeto_500_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_7 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;

                            this.DOP = Octeto_500_2 + Octeto_500_3 + Octeto_500_4 + Octeto_500_5 + Octeto_500_6 + Octeto_500_7;

                        }

                        if (subfield_2 == "1")
                        {
                            string Octeto_500_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_7 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;

                            this.SDP = Octeto_500_2 + Octeto_500_3 + Octeto_500_4 + Octeto_500_5 + Octeto_500_6 + Octeto_500_7;
                        }

                        if (subfield_3 == "1")
                        {
                            string Octeto_500_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;

                            this.SDH = Octeto_500_2 + Octeto_500_3;
                        }

                    }
                    i++;

                    //I020/400 Contributing Devices (No entiendo como es su estrutuctura)
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_400_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                    }
                    i++;

                    //I020/250 Mode S MB Data (0 idea de su estructura)
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_250_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                    }
                    i++;

                }


            }

        }





        public void computeFSPEC(string[] paquete)
        {
            //Leemos los dos primeros octetos
            string FSPEC_1 = Convert.ToString(Convert.ToInt32(paquete[3], 16), 2).PadLeft(8, '0');

            string FX = Convert.ToString(FSPEC_1[FSPEC_1.Length - 1]);

            //Sacamos los campos hasta que el bit de FX sea 0 y nos indique que el FX se termina
            int i = 4;
            while (FX == "1")
            {
                FSPEC_1 = FSPEC_1 + Convert.ToString(Convert.ToInt32(paquete[i], 16), 2).PadLeft(8, '0');
                FX = Convert.ToString(FSPEC_1[FSPEC_1.Length - 1]);
                i++;
            }
            this.FSPEC = FSPEC_1;
        }

        public string getFSPEC()
        {
            return this.FSPEC;
        }

        public string getSAC()
        {
            return this.SAC;
        }

        public string getSIC()
        {
            return this.SIC;
        }


        public string getTrackStatus()
        {
            return this.TrackStatus;
        }
        public string getMode3A()
        {
            return this.Mode3A;
        }

        public double getX()
        {
            double Xx;
            if (this.X[0] == '1')
            {
                Xx = (-1) * Convert.ToDouble(Convert.ToInt32(this.X, 2)) * 0.5;
            }
            else
            {
                Xx = Convert.ToDouble(Convert.ToInt32(this.X, 2)) * 0.5;
            }

            return Xx;
        }

        public double getY()
        {

            double Yy;
            if (this.Y[0] == '1')
            {
                Yy = (-1) * Convert.ToDouble(Convert.ToInt32(this.Y, 2)) * 0.5;
            }
            else
            {
                Yy = Convert.ToDouble(Convert.ToInt32(this.Y, 2)) * 0.5;
            }

            return Yy;
        }

        public string getTOD()
        {
            string[] tod = new string[3];

            double sect = Convert.ToDouble(Convert.ToInt32(this.TOD, 2) / 128);
            TimeSpan time = TimeSpan.FromSeconds(sect);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str = time.ToString(@"hh\:mm\:ss\:fff");

            return str;

        }
    }
}
