using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixDecoder
{
    public class CAT20
    {
        public string FSPEC;

        //010
        public int SAC;
        public int SIC;

        //020
        public string TargetReport;
        
        //140
        public string TOD;
        
        //042
        public string LatWSG;
        public string LonWSG;
        public string[] coordsWSG;
        
        //042
        public double X;
        public double Y;
        public double[] coordscc;

        //161
        public string TrackNum;

        //170 TrackStatus (en bin)
        public string CNF;
        public string TRE;
        public string CST;
        public string CDM;
        public string MAH;
        public string STH;
        public string GHO;
        public string[] TrackStatus;

        //070
        public string Mode3A; 

        //202
        public double Vx;
        public double Vy;
        public double[] Velocitycc;

        //090
        public string[] FL;

        //100
        public string ModeC;

        //220
        public string TargetAddress;

        //245
        public string[] TargetID;
        public string callsign;

        //110
        public string MeasuredHeight;

        //105
        public string geoHeight;

        //210
        public string calcAccel;

        //300
        public string VehicleFleetID;

        //310
        public string PPMsg;

        //500
        public double[] DOP; 
        public double[] SDP; 
        public double SDH;

        //400
        public int Receivers;

        //250
        public string MB;
        public string DATA;
        public string BDS1;
        public string BDS2;
        public string[] ModeSData;

        //230
        public string ACAScap;

        //260
        public string ACASRAreport;

        //030
        public string warning;

        //055
        public string Mode1Code;

        //050
        public string Mode2Code;

        public int length;

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
                string Octeto_010_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octecto_010_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                this.SAC = Convert.ToInt32(Octeto_010_1, 2);
                this.SIC = Convert.ToInt32(Octecto_010_2, 2);
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

                this.TOD = computeTOD(Octeto_140_1,Octeto_140_2,Octeto_140_3);
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

                this.X = computeX(Octeto_042_1,Octeto_042_2,Octeto_042_3);
                this.Y = computeY(Octeto_042_4,Octeto_042_5,Octeto_042_6);
                this.coordscc = new double[] { X, Y };
            }
            i++;
          
            //I020/161 Track Number
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_161_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                string Octeto_161_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;

                this.TrackNum = computeTrackNumber(Octeto_161_1,Octeto_161_2);
            }
            i++;

            //I020/170 Track status
            if (Convert.ToString(FSPEC_copia[i]) == "1")
            {
                string Octeto_170 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                OctetoIndex++;
                this.CNF = Convert.ToString(Octeto_170[0]);
                this.TRE = Convert.ToString(Octeto_170[1]);
                this.CST = Convert.ToString(Octeto_170[2]);
                this.CDM = Convert.ToString(Octeto_170[3]) + Convert.ToString(Octeto_170[4]);
                this.MAH = Convert.ToString(Octeto_170[5]);
                this.STH = Convert.ToString(Octeto_170[6]);
                if (Octeto_170[7] == '1')
                {
                    string Octeto_170_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    this.GHO = Convert.ToString(Octeto_170_2[0]);
                    this.TrackStatus = new string[]{ CNF, TRE, CST, CDM, MAH, STH, GHO};
                }
                else if (Octeto_170[7] == '0') 
                { 
                    this.TrackStatus = new string[]{CNF, TRE, CST, CDM, MAH, STH}; 
                }
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

                    this.Mode3A = computeMode3A(Octeto_070_1,Octeto_070_2);
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

                    this.Vx = computeV(Octeto_202_1,Octeto_202_2);
                    this.Vy = computeV(Octeto_202_3,Octeto_202_4);
                    this.Velocitycc = new double[] { Vx, Vy };
                }
                i++;

                //I020/090 Flight Level
                if (Convert.ToString(FSPEC_copia[i]) == "1")
                {
                    string Octeto_090_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;
                    string Octeto_090_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                    OctetoIndex++;

                    this.FL = computeFL(Octeto_090_1 , Octeto_090_2);
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

                    this.TargetID = computeID(Octeto_245_1,Octeto_245_2,Octeto_245_3,Octeto_245_4,Octeto_245_5,Octeto_245_6,Octeto_245_7);
                    this.callsign = this.TargetID[1];               
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

                            this.DOP = computeDOP(Octeto_500_2,Octeto_500_3,Octeto_500_4,Octeto_500_5,Octeto_500_6,Octeto_500_7);

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

                            this.SDP = computeDOP(Octeto_500_2, Octeto_500_3, Octeto_500_4, Octeto_500_5, Octeto_500_6, Octeto_500_7); //Usamos la misma funcion porque tienen estructura identica
                        }

                        if (subfield_3 == "1")
                        {
                            string Octeto_500_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            string Octeto_500_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;

                            this.SDH = computeSDH(Octeto_500_2,Octeto_500_3);
                        }
                            
                    }
                    i++;

                    //I020/400 Contributing Devices 
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_400_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0'); //Repetition Factor
                        OctetoIndex++;
                        int Repetition_Factor = Convert.ToInt32(Octeto_400_1, 2);
                        int rf = 0;
                        int recep = 0;
                        while (rf < Repetition_Factor)
                        {
                            string Octeto_400_x = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                            OctetoIndex++;
                            int cont = 0;
                            while (cont < 8)
                            {
                                if (Convert.ToString(Octeto_400_x[cont]) == "1") { recep++; }
                                cont++;
                            }
                            rf++;
                        }
                        this.Receivers = recep;
                    }
                    i++;

                    //I020/250 Mode S MB Data 
                    if (Convert.ToString(FSPEC_copia[i]) == "1")
                    {
                        string Octeto_250_1 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_2 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_3 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        this.MB = Octeto_250_2 + Octeto_250_3;
                        string Octeto_250_4 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_5 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_6 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_7 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        string Octeto_250_8 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        this.DATA = Octeto_250_4 + Octeto_250_5 + Octeto_250_6 + Octeto_250_7 + Octeto_250_8;

                        string Octeto_250_9 = Convert.ToString(Convert.ToInt32(array[OctetoIndex], 16), 2).PadLeft(8, '0');
                        OctetoIndex++;
                        this.BDS1 = Octeto_250_9.Substring(0, 4);
                        this.BDS2 = Octeto_250_9.Substring(4, 4);
                    }
                    i++;
                        
                }

                
                    }

                }

        private void computeFSPEC(string[] paquete)
        {
            //Leemos los dos primeros octetos
            string FSPEC_1 = Convert.ToString(Convert.ToInt32(paquete[3], 16), 2).PadLeft(8,'0');
            
            string FX = Convert.ToString(FSPEC_1[FSPEC_1.Length - 1]);

            //Sacamos los campos hasta que el bit de FX sea 0 y nos indique que el FX se termina
            int i = 4;
            while (FX == "1")
            {
                FSPEC_1 = FSPEC_1 + Convert.ToString(Convert.ToInt32(paquete[i], 16), 2).PadLeft(8,'0');
                FX = Convert.ToString(FSPEC_1[FSPEC_1.Length - 1]);
                i++;
            }
            this.FSPEC = FSPEC_1;
        }

        private double computeX(string oct_1, string oct_2, string oct_3)
        {
            string xx = oct_1 + oct_2 + oct_3;
            StringBuilder str = new StringBuilder(xx);
            double Xx = 0.0;
            if (oct_1[0] == '1')
            {
                xx = findTwoscomplement(str);
                Xx = (-1) * Convert.ToDouble(Convert.ToInt32(xx, 2)) * 0.5;
            }
            else
            {
                Xx = Convert.ToDouble(Convert.ToInt32(xx, 2)) * 0.5;
            }

            return Xx;
        }

        private string findTwoscomplement(StringBuilder bits) //Converter de internet
        {
            int n = bits.Length;

            // Traverse the string to get  
            // first '1' from the last of string  
            int i;
            for (i = n - 1; i >= 0; i--)
            {
                if (bits[i] == '1')
                {
                    break;
                }
            }

            // If there exists no '1' concat 1  
            // at the starting of string  
            if (i == -1)
            {
                return "1" + bits;
            }

            // Continue traversal after the 
            // position of first '1'  
            for (int k = i - 1; k >= 0; k--)
            {
                // Just flip the values  
                if (bits[k] == '1')
                {
                    bits.Remove(k, k + 1 - k).Insert(k, "0");
                }
                else
                {
                    bits.Remove(k, k + 1 - k).Insert(k, "1");
                }
            }

            // return the modified string  
            return bits.ToString();
        }  

        private double computeY(string oct_1, string oct_2,string oct_3)
        {
            string yy = oct_1 + oct_2 + oct_3;
            StringBuilder str = new StringBuilder(yy);
            double Yy;

            if (oct_1[0] == '1')
            {
                yy = findTwoscomplement(str);
                Yy = (-1) * Convert.ToDouble(Convert.ToInt32(yy,2)) * 0.5;
            }
            else
            {
                Yy = Convert.ToDouble(Convert.ToInt32(yy, 2)) * 0.5;
            }

            return Yy;
        }

        private string computeTOD(string oct_1, string oct_2 ,string oct_3)
        {

            string TOD = oct_1 + oct_2 + oct_3;
            double sect = Convert.ToDouble(Convert.ToInt32(TOD, 2) / 128);
            TimeSpan time = TimeSpan.FromSeconds(sect);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string tod = time.ToString(@"hh\:mm\:ss\:fff");

            return tod;

        }

        private string[] computeID(string oct_1, string oct_2, string oct_3, string oct_4, string oct_5, string oct_6,string oct_7)
        {
            string OctetoT = oct_1+oct_2+oct_3+oct_4+oct_5+oct_6+oct_7;
            string STI_1 = Convert.ToString(OctetoT[0]);
            string STI_2 = Convert.ToString(OctetoT[1]);
            string STI = STI_1 + STI_2;

            if (STI == "00")
            {
                STI = "Callsign or registration not downlinked from transponder";
            }
            else if (STI == "01")
            {
                STI = "Registration downlinked from transponder";
            }
            else if (STI == "10")
            {
                STI = "Callsign downlinked from transponder";
            }
            else if (STI == "11")
            {
                STI = "Not defined";
            }


            string characters = Convert.ToString(OctetoT.Substring(8));

            int h = characters.Length;

            int c = 0;

            string code = "";
            while (c < characters.Length)
            {
               
                string character = Convert.ToString(characters.Substring(c, 6));
                string b6_b5 = Convert.ToString(character.Substring(0,2));
                string b4_b3_b2_b1 = Convert.ToString(character.Substring(2,4));

                if (b6_b5 == "00")
                {
                    if (b4_b3_b2_b1 == "0001") { code = code + "A"; }
                    if (b4_b3_b2_b1 == "0010") { code = code + "B"; }
                    if (b4_b3_b2_b1 == "0011") { code = code + "C"; }
                    if (b4_b3_b2_b1 == "0100") { code = code + "D"; }
                    if (b4_b3_b2_b1 == "0101") { code = code + "E"; }
                    if (b4_b3_b2_b1 == "0110") { code = code + "F"; }
                    if (b4_b3_b2_b1 == "0111") { code = code + "G"; }
                    if (b4_b3_b2_b1 == "1000") { code = code + "H"; }
                    if (b4_b3_b2_b1 == "1001") { code = code + "I"; }
                    if (b4_b3_b2_b1 == "1010") { code = code + "J"; }
                    if (b4_b3_b2_b1 == "1011") { code = code + "K"; }
                    if (b4_b3_b2_b1 == "1100") { code = code + "L"; }
                    if (b4_b3_b2_b1 == "1101") { code = code + "M"; }
                    if (b4_b3_b2_b1 == "1110") { code = code + "N"; }
                    if (b4_b3_b2_b1 == "1111") { code = code + "O"; }
                }
                if (b6_b5 == "01")
                {
                    if (b4_b3_b2_b1 == "0000") { code = code + "P"; }
                    if (b4_b3_b2_b1 == "0001") { code = code + "Q"; }
                    if (b4_b3_b2_b1 == "0010") { code = code + "R"; }
                    if (b4_b3_b2_b1 == "0011") { code = code + "S"; }
                    if (b4_b3_b2_b1 == "0100") { code = code + "T"; }
                    if (b4_b3_b2_b1 == "0101") { code = code + "U"; }
                    if (b4_b3_b2_b1 == "0110") { code = code + "V"; }
                    if (b4_b3_b2_b1 == "0111") { code = code + "W"; }
                    if (b4_b3_b2_b1 == "1000") { code = code + "X"; }
                    if (b4_b3_b2_b1 == "1001") { code = code + "Y"; }
                    if (b4_b3_b2_b1 == "1010") { code = code + "Z"; }
                }

                if (b6_b5 == "10")
                {
                    if (b4_b3_b2_b1 == "0000") { code = code + " "; }
                }

                if (b6_b5 == "11")
                {
                    if (b4_b3_b2_b1 == "0000") { code = code + "1"; }
                    if (b4_b3_b2_b1 == "0001") { code = code + "2"; }
                    if (b4_b3_b2_b1 == "0010") { code = code + "3"; }
                    if (b4_b3_b2_b1 == "0011") { code = code + "4"; }
                    if (b4_b3_b2_b1 == "0100") { code = code + "5"; }
                    if (b4_b3_b2_b1 == "0101") { code = code + "6"; }
                    if (b4_b3_b2_b1 == "0110") { code = code + "7"; }
                    if (b4_b3_b2_b1 == "0111") { code = code + "8"; }
                    if (b4_b3_b2_b1 == "1000") { code = code + "9"; }
                }

                c = c + 6;
            }


            string [] TIDs = {STI, code};
            return TIDs;
        }

        private string[] computeFL(string oct_1,string oct_2)
        {
            string V = "";
            if (Convert.ToString(oct_1[0]) == "0") { V = "Code validated"; }
            else if (Convert.ToString(oct_1[0]) == "1") { V = "Code not validated"; }

            string G = "";
            if (Convert.ToString(oct_1[1]) == "0") { G = "Default"; }
            else if (Convert.ToString(oct_1[1]) == "1") { G = "Garbled code"; }

            string FL_bit = oct_1.Substring(2) + oct_2;
            string FL = Convert.ToString(Convert.ToDouble(Convert.ToInt32(FL_bit, 2)) * 0.25);

            string[] FLBR = {V,G,FL};
            return FLBR;
        }

        private string computeTrackNumber(string oct_1, string oct_2)
        {
            string TN = Convert.ToString(Convert.ToInt32(oct_1.Substring(2) + oct_2,2));
            return TN;
        }

        private double computeV(string oct_1, string oct_2)
        {
            string v = oct_1 + oct_2;
            StringBuilder str = new StringBuilder(v);

            double V;
            if (oct_1[0] == '1')
            {
                v = findTwoscomplement(str);
                V = (-1) * Convert.ToDouble(Convert.ToInt32(v, 2)) * 0.25;
            }
            else
            {
                V = Convert.ToDouble(Convert.ToInt32(v, 2)) * 0.25;
            }

            return V;
        }

        private double[] computeDOP(string oct_1, string oct_2, string oct_3, string oct_4, string oct_5, string oct_6)
        {
            string dop_x = oct_1 + oct_2;
            string dop_y = oct_3 + oct_4;
            string dop_xy = oct_5 + oct_6;

            double dop_x_double = Convert.ToDouble(Convert.ToInt32(dop_x, 2)) * 0.25;
            double dop_y_double = Convert.ToDouble(Convert.ToInt32(dop_y, 2)) * 0.25;
            double dop_xy_double = Convert.ToDouble(Convert.ToInt32(dop_xy, 2)) * 0.25;

            double[] DOP = { dop_x_double, dop_y_double, dop_xy_double };

            return DOP;
        }

        private double computeSDH(string oct_1,string oct_2)
        {
            string oct_t = oct_1 + oct_2;
            double sdh = Convert.ToDouble(Convert.ToInt32(oct_t, 2)) * 0.5;
            return sdh;
        }

        public string getTrackStatusToString()
        {
            string cnf = "", tre = "", cst="", cdm="", mah="", sth="", gho="";

            if(this.CNF == "0") { cnf = "Confirmed Track"; }
            else if(this.CNF == "1") {  cnf = "Track in initiation phase"; }
            if(this.TRE == "0") {  tre = "Default"; }
            else if(this.TRE == "1") {  tre = "Last report for a track"; }
            if(this.CST == "0") {  cst = "Not extrapolated"; }
            else if(this.CST == "1") {  cst = "Extrapolated"; }
            if(this.CDM == "00") {  cdm = "Maintaining"; }
            else if(this.CDM == "01") {  cdm = "Climbing"; }
            else if(this.CDM == "10") {  cdm = "Descending"; }
            else if (this.CDM == "11") {  cdm = "Invalid"; }
            if(this.MAH == "0") {  mah = "Default"; }
            else if(this.MAH == "1") {  mah = "Horizontal manoeuvre"; }
            if(this.STH == "0") {  sth = "Measured position"; }
            else if(this.STH == "1") {  sth = "Smoothed position"; }
            if (this.GHO == "0") {  gho = "Default"; }
            else if (this.GHO == "1") {  gho = "Ghost track"; }

            string trackstatustostring = " - " + cnf + "\n - " + tre + "\n - " + cst + "\n - " + cdm + "\n - " + mah + "\n - " + sth;

            if (gho != "") { trackstatustostring = trackstatustostring + "\n - " + gho; }
            return trackstatustostring;
        }

        private string computeMode3A(string oct1, string oct2)
        {
            string octT = oct1 + oct2;
            string numeros = octT.Substring(4, 12);
            string Mode3Acode = "";
            for (int c = 0; c < numeros.Length;c= c + 3)
            {
                string num = Convert.ToString(Convert.ToInt32(numeros.Substring(c, 3),2));
                Mode3Acode = Mode3Acode + num;
            }

            return (Mode3Acode);
        }

        public string getTargetReportDescriptortoString()
        {
            string targetreporttostring = "";

            if(this.TargetReport[0] == '1') { targetreporttostring = "- Non-Mode S 1090MHz MLAT"; }
            if (this.TargetReport[1] == '1') { targetreporttostring = "- Mode S 1090MHz MLAT"; }
            if (this.TargetReport[2] == '1') { targetreporttostring = "- HF MLAT"; }
            if (this.TargetReport[3] == '1') { targetreporttostring = "- VDL Mode 4 MLAT"; }
            if (this.TargetReport[4] == '1') { targetreporttostring = "- UAT MLAT"; }
            if (this.TargetReport[5] == '1') { targetreporttostring = "- DME/TACAN MLAT"; }
            if (this.TargetReport[6] == '1') { targetreporttostring = "- Other Technology MLAT"; }

            if (this.TargetReport[7] == '1')
            {
                if (this.TargetReport[8] == '1') { targetreporttostring += "\n- Report from field monitor (fixed transponder)"; }
                else if (this.TargetReport[8] == '0') { targetreporttostring += "\n- Report from target transponder"; }
                if (this.TargetReport[9] == '0') { targetreporttostring += "\n- Absence of Special Position Identification"; }
                else if (this.TargetReport[9] == '1') { targetreporttostring += "\n- Special Position Identification"; }
                if (this.TargetReport[10] == '0') { targetreporttostring += "\n- Chain"; }
                else if (this.TargetReport[10] == '1') { targetreporttostring += "\n- Chain 2"; }
                if (this.TargetReport[11] == '0') { targetreporttostring += "\n- Transponder Ground bit not set"; }
                else if (this.TargetReport[11] == '1') { targetreporttostring += "\n- Transponder Ground bit set"; }
                if (this.TargetReport[12] == '0') { targetreporttostring += "\n- No Corrupted reply in MLAT"; }
                else if (this.TargetReport[12] == '1') { targetreporttostring += "\n- Corrupted replies in MLAT"; }
                if (this.TargetReport[13] == '0') { targetreporttostring += "\n- Actual Target Report"; }
                else if (this.TargetReport[13] == '1') { targetreporttostring += "\n- Simulated Target Report"; }
                if (this.TargetReport[14] == '0') { targetreporttostring += "\n- Default"; }
                else if (this.TargetReport[14] == '1') { targetreporttostring += "\n- Test Target"; }
            }

            return targetreporttostring;
        }
    }
}