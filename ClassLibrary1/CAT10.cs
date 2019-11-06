using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixDecoder
{
    public class CAT10
    {
        // ATRIBUTS:

        public string FSPEC;

        public string MessageType;

        public int SACnum;
        public int SICnum;
        public string SAC;
        public string SIC;

        public string TYP;
        public string DCR;
        public string CHN;
        public string GBS;
        public string CRT;
        public string SIM;
        public string TST;
        public string RAB;
        public string LOP;
        public string TOT;
        public string SPI;

        public string TimeOfDay;

        public double LatitudeWGS;
        public double LongitudeWGS;

        public double RHO;
        public double Theta;

        public double Xcomponent;
        public double Ycomponent;

        public double GroundSpeed;
        public double TrackAngle;

        public double Vx;
        public double Vy;

        public int TrackNumber;

        public string CNF;
        public string TRE;
        public string CST;
        public string MAH;
        public string TCC;
        public string STH;
        public string TOM;
        public string DOU;
        public string MRS;
        public string GHO;
        public string TrackStatus;

        public string V_mode3A;
        public string G_mode3A;
        public string L;
        public string Mode3ACode;

        public string TargetAddress;

        public string STI;
        public string TargetID;

        public string[] MBdata;
        public string[] BDS1;
        public string[] BDS2;

        public int VehicleFeetID;

        public string V_FL;
        public string G_FL;
        public double FL;

        public double Height;

        public double TargetLength;
        public double TargetOrientation;
        public double TargetWidth;

        public string NOGO;
        public string OVL;
        public string TSV;
        public string DIV;
        public string TTF;

        public string TRB;
        public int MSG_num;
        public string MSG;

        public double DevX;
        public double DevY;
        public double Covariance;

        public double[] DRHO;
        public double[] DTHETA;

        public double PAM;

        public double Ax;
        public double Ay;

        // CONSTRUCTOR:
        public CAT10(string[] paquete) //decodifica el missatge (paquet)
        {
            //el primer octet (posició 0) és la categoria --> ja l'hem llegida

            //els segon i tercet octet (posicions 1 i 2) és la longitud del missatge = nombre d'octets
            int longitud = int.Parse(paquete[1] + paquete[2], System.Globalization.NumberStyles.HexNumber);
            //sense comptar aquests tres primers octets:
            longitud = longitud - 3;

            //llegim el FSPEC
            ComputeFSPEC(paquete);
            int longFSPEC = this.FSPEC.Length / 8;

            //posició on comencen les dades
            int pos = 0 + 1 + 2 + longFSPEC;

            // Data Source Identifier
            if (Convert.ToString(FSPEC[0]) == "1")
            {
                this.ComputeDataSourceIdentifier(paquete[pos], paquete[pos + 1]);
                pos = pos + 2;
            }

            // Message Type
            if (Convert.ToString(FSPEC[1]) == "1")
            {
                this.ComputeMessageType(paquete[pos]);
                pos = pos + 1;
            }

            // Target Report Descriptor
            if (Convert.ToString(FSPEC[2]) == "1")
                pos = pos + this.ComputeTargetReportDescriptor(paquete, pos);

            // Time of Day
            if (Convert.ToString(FSPEC[3]) == "1")
            {
                this.ComputeTimeOfDay(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                pos = pos + 3;
            }

            // Position in WGS-84 Co-ordinates
            if (Convert.ToString(FSPEC[4]) == "1")
            {
                this.ComputePositionInWGS84(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3], paquete[pos + 4] + paquete[pos + 5] + paquete[pos + 6] + paquete[pos + 7]);
                pos = pos + 8;
            }

            // Measured Position in Polar Co-ordinates
            if (Convert.ToString(FSPEC[5]) == "1")
            {
                this.ComputePositionInPolar(paquete[pos] + paquete[pos + 1], paquete[pos + 2] + paquete[pos + 3]);
                pos = pos + 4;
            }

            // Posititon in Cartesian Co-ordinates
            if (Convert.ToString(FSPEC[6]) == "1")
            {
                this.ComputePositionInCartesian(Convert.ToString(paquete[pos] + paquete[pos + 1]), Convert.ToString(paquete[pos + 2] + paquete[pos + 3]));
                pos = pos + 4;
            }

            if (longFSPEC >= 2)
            {
                // Calculated Track Velocity in Polar Co-ordinates
                if (Convert.ToString(FSPEC[8]) == "1")
                {
                    this.ComputeTrackVelocityInPolar(paquete[pos] + paquete[pos + 1], paquete[pos + 2] + paquete[pos + 3]);
                    pos = pos + 4;
                }

                // Calculated Track Velocity in Cartesian Co-ordinates
                if (Convert.ToString(FSPEC[9]) == "1")
                {
                    this.ComputeTrackVelocityInCartesian(paquete[pos] + paquete[pos + 1], paquete[pos + 2] + paquete[pos + 3]);
                    pos = pos + 4;
                }

                // Track Number
                if (Convert.ToString(FSPEC[10]) == "1")
                {
                    this.ComputeTrackNumber(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // Track Status
                if (Convert.ToString(FSPEC[11]) == "1")
                    pos = pos + this.ComputeTrackStatus(paquete, pos);

                // Mode-3/A Code in Octal representation
                if (Convert.ToString(FSPEC[12]) == "1")
                {
                    this.ComputeMode3ACode(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // Target Address
                if (Convert.ToString(FSPEC[13]) == "1")
                {
                    this.ComputeTargetAddress(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Target Identification
                if (Convert.ToString(FSPEC[14]) == "1")
                {
                    this.ComputeTargetIdentification(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5] + paquete[pos + 6]);
                    pos = pos + 7;
                }

                if (longFSPEC >= 3)
                {
                    // Mode S MB Data
                    if (Convert.ToString(FSPEC[16]) == "1")
                    {
                        pos = pos + this.ComputeModeS_MBdata(paquete, pos);
                    }

                    // Vehicle Fleet Identification
                    if (Convert.ToString(FSPEC[17]) == "1")
                    {
                        this.ComputeVehicleFleetID(paquete[pos]);
                        pos++;
                    }

                    // Flight Level in Binary Representation
                    if (Convert.ToString(FSPEC[18]) == "1")
                    {
                        this.ComputeFL(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Measured Height
                    if (Convert.ToString(FSPEC[19]) == "1")
                    {
                        this.ComputeMesuredHeight(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Target Size & Orientation
                    if (Convert.ToString(FSPEC[20]) == "1")
                        pos = pos + this.ComputeTargetSizeAndOrientation(paquete[pos], paquete[pos + 1], paquete[pos + 2]);

                    // System Status
                    if (Convert.ToString(FSPEC[21]) == "1")
                    {
                        this.ComputeSystemStatus(paquete[pos]);
                        pos++;
                    }

                    // Pre-programmed Message
                    if (Convert.ToString(FSPEC[22]) == "1")
                    {
                        this.ComputePreprogrammedMessage(paquete[pos]);
                        pos++;
                    }

                    if (longFSPEC >= 4)
                    {
                        // Standard Deviation of Position
                        if (Convert.ToString(FSPEC[24]) == "1")
                        {
                            this.ComputeStandardDeviationOfPosition(paquete[pos], paquete[pos + 1], paquete[pos + 2] + paquete[pos + 3]);
                            pos = pos + 4;
                        }

                        // Presence
                        if (Convert.ToString(FSPEC[25]) == "1")
                            pos = pos + this.ComputePresence(paquete, pos);

                        // Amplitude of primary plot
                        if (Convert.ToString(FSPEC[26]) == "1")
                        {
                            this.ComputeAmplitudeOfPrimaryPlot(paquete[pos]);
                            pos++;
                        }

                        // Calculated Acceleration
                        if (Convert.ToString(FSPEC[27]) == "1")
                        {
                            this.ComputeCalculatedAcceleration(paquete[pos], paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Bit FSPEC[28] --> no diu res

                        // Posició 29 i 30 ni idea
                    }
                }
            }
        }

        // MÈTODES:
        public void ComputeFSPEC(string[] paquete)
        {
            //llegim octets fins que la última posició d'un d'ells sigui "0"
            int pos = 3;
            bool continua = true;
            while (continua == true && pos <= 6)
            {
                //llegim nou octeto
                string newocteto = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

                //fiquem aquest octeto al FSPEC
                this.FSPEC = this.FSPEC + newocteto;

                if (newocteto.Substring(7, 1) == "1") //llegim el següent
                    pos++;
                else //sortim del bucle
                    continua = false;
            }
        }

        public void ComputeDataSourceIdentifier(string octetoSAC, string octetoSIC) // Data Item I010/010
        {
            //passem a string de bits
            string SAC = Convert.ToString(Convert.ToInt32(octetoSAC, 16), 2).PadLeft(8, '0');
            string SIC = Convert.ToString(Convert.ToInt32(octetoSIC, 16), 2).PadLeft(8, '0');

            //passem a número
            this.SACnum = Convert.ToInt32(SAC, 2);
            this.SICnum = Convert.ToInt32(SIC, 2);

            //decodifiquem (hi ha més codis però aquests són els únics que sortiran)
            if (this.SACnum == 0)
                this.SAC = "Data flow local to the airport";
            if (this.SICnum == 107)
                this.SIC = "Barcelona airport - LEBL";
        }

        public void ComputeMessageType(string octetoMT) // Data Item I010/000
        {
            //llegim aquest valor en decimal
            int MT_dec = int.Parse(octetoMT, System.Globalization.NumberStyles.HexNumber);

            //assignem el tipus de missatge (nom tal qual hi és al manual) segons el valor [diapo19]
            if (MT_dec == 1)
                this.MessageType = "Target Report";
            if (MT_dec == 2)
                this.MessageType = "Start of Update Cycle";
            if (MT_dec == 3)
                this.MessageType = "Periodic Status Message";
            if (MT_dec == 4)
                this.MessageType = "Event-triggered Status Message";
        }

        public int ComputeTargetReportDescriptor(string[] paquete, int pos) // Data Item I010/020
        {
            //contador de posicions que ocupa
            int cont = 1;

            //llegim el primer octet
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            //analitzem el octeto 1 (First Part):
            string TYP = octeto1.Substring(0, 3);
            if (TYP == "000")
                this.TYP = "SSR multilateration";
            if (TYP == "001")
                this.TYP = "Mode S multilateration";
            if (TYP == "010")
                this.TYP = "ADS-B";
            if (TYP == "011")
                this.TYP = "PSR";
            if (TYP == "100")
                this.TYP = "Magnetic Loop System";
            if (TYP == "101")
                this.TYP = "HF multilateration";
            if (TYP == "110")
                this.TYP = "Not defined";
            if (TYP == "111")
                this.TYP = "Other types";

            string DCR = octeto1.Substring(3, 1);
            if (DCR == "0")
                this.DCR = "No differential correction";
            if (DCR == "1")
                this.DCR = "Differential correction";

            string CHN = octeto1.Substring(4, 1);
            if (CHN == "1")
                this.CHN = "Chain 2";
            if (CHN == "0")
                this.CHN = "Chain 1";

            string GBS = octeto1.Substring(5, 1);
            if (GBS == "0")
                this.GBS = "Transponder Ground bit not set";
            if (GBS == "1")
                this.GBS = "Transponder Ground bit set";

            string CRT = octeto1.Substring(6, 1);
            if (CRT == "0")
                this.CRT = "No Corrupted reply in multilateration";
            if(CRT=="1")
                this.CRT= "Corrupted replies in multilateration";

            //mirem el bit FX:
            string FX = Convert.ToString(octeto1[7]);
            while (FX == "1")
            {
                //llegim nou octeto i actualitzem FX:
                string newoctet = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                FX = Convert.ToString(newoctet[7]);

                //analitzem el nou octeto:
                if (cont == 2) //(First Extent):
                {
                    string SIM = octeto1.Substring(0, 1);
                    if (SIM == "0")
                        this.SIM = "Actual target report";
                    if (SIM == "1")
                        this.SIM = "Simulated target report";

                    string TST = octeto1.Substring(1, 1);
                    if (TST == "0")
                        this.TST = "Default";
                    if (TST == "1")
                        this.TST = "Test Target";

                    string RAB = octeto1.Substring(2, 1);
                    if (RAB == "0")
                        this.RAB = "Report from target transponder";
                    if (RAB == "1")
                        this.RAB = "Report from field monitor (fixed transponder)";

                    string LOP = octeto1.Substring(3, 2);
                    if (LOP == "00")
                        this.LOP = "Undetermined";
                    if (LOP == "01")
                        this.LOP = "Loop start";
                    if (LOP == "10")
                        this.LOP = "Loop finish";

                    string TOT = octeto1.Substring(5, 2);
                    if (TOT == "00")
                        this.TOT = "Undetermined";
                    if (TOT == "01")
                        this.TOT = "Aircraft";
                    if (TOT == "10")
                        this.TOT = "Ground vehicle";
                    if (TOT == "11")
                        this.TOT = "Helicopter";
                }
                else //(Second &+ Extent):
                {
                    //SPI
                    if (Convert.ToString(newoctet[0]) == "0")
                        this.SPI = "Absence of SPI (Special Position Identification)";
                    if (Convert.ToString(newoctet[0]) == "1")
                        this.SPI = "SPI (Special Position Identification)";
                }

                cont++;
            }

            return cont;
        }

        public void ComputeTimeOfDay(string octetoTimeOfDay) // Data Item I010/140
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetoTimeOfDay, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            this.TimeOfDay = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputePositionInWGS84(string OctLatWGS, string OctLonWGS) // Data Item I010/041
        {
            //passem a string de bits
            string lat = Convert.ToString(Convert.ToInt32(OctLatWGS, 16), 2);
            string lon = Convert.ToString(Convert.ToInt32(OctLonWGS, 16), 2);

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.LatitudeWGS = this.ComputeComplementoA2(lat) * (180 / Math.Pow(2, 31));
            this.LongitudeWGS = this.ComputeComplementoA2(lon) * (180 / Math.Pow(2, 31));
        }

        public void ComputePositionInPolar(string OctRHO, string OctTheta) // Data Item I010/040
        {
            //passem les coordenades a ints
            int rho = int.Parse(OctRHO, System.Globalization.NumberStyles.HexNumber);
            int theta = int.Parse(OctTheta, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució per passar a doubles
            this.RHO = Convert.ToSingle(rho) * 1;
            this.Theta = Convert.ToSingle(theta) * (360 / Math.Pow(2, 16));
        }

        public void ComputePositionInCartesian(string OctX, string OctY) // Data Item I010/042
        {
            //passem a string de bits:
            string octetx = Convert.ToString(Convert.ToInt32(OctX, 16), 2);
            string octety = Convert.ToString(Convert.ToInt32(OctY, 16), 2);

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.Xcomponent = this.ComputeComplementoA2(octetx) * Math.Pow(2, -14);
            this.Ycomponent = this.ComputeComplementoA2(octety) * (360 / Math.Pow(2, 16));
        }

        public void ComputeTrackVelocityInPolar(string OctGS, string OctTA) // Data Item I010/200
        {
            //passem les coordenades a ints
            int gs = int.Parse(OctGS,System.Globalization.NumberStyles.HexNumber);
            int ta = int.Parse(OctTA,System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució per passar a doubles
            this.GroundSpeed = Convert.ToSingle(gs) * Math.Pow(2, -14);
            this.TrackAngle = Convert.ToSingle(ta) * (360 / Math.Pow(2, 16));
        }

        public void ComputeTrackVelocityInCartesian(string OctVx, string OctVy) // Data Item I010/202
        {
            //passem a string de bits:
            string octetvx = Convert.ToString(Convert.ToInt32(OctVx, 16), 2);
            string octetvy = Convert.ToString(Convert.ToInt32(OctVy, 16), 2);

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.Vx = this.ComputeComplementoA2(octetvx) * 0.25;
            this.Vy = this.ComputeComplementoA2(octetvy) * 0.25;
        }

        public void ComputeTrackNumber(string OctTN) // Data Item I010/161
        {
            //passem a binari
            string tracknum_bin = Convert.ToString(Convert.ToInt32(OctTN, 16), 2).PadLeft(16,'0');

            //tallem els 4 primers bits
            string tracknum_valid = tracknum_bin.Substring(4, 12);

            //passem a int
            this.TrackNumber = Convert.ToInt32(tracknum_valid, 2);
        }

        public int ComputeTrackStatus(string[] paquete, int pos) // Data Item I010/170
        {
            //contador de posicions que ocupa
            int cont = 1;

            //llegim el primer octet
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            //analitzem el octeto 1 (First Part):
            this.CNF = octeto1.Substring(0, 1);
            this.TRE = octeto1.Substring(1, 1);
            this.CST = octeto1.Substring(2, 2);
            this.MAH = octeto1.Substring(4, 1);
            this.TCC = octeto1.Substring(5, 1);
            this.STH = octeto1.Substring(6, 1);


            //mirem el bit FX:
            string FX = Convert.ToString(octeto1[7]);
            while (FX == "1")
            {
                //llegim nou octeto i actualitzem FX:
                string newoctet = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                FX = Convert.ToString(newoctet[7]);

                //analitzem el nou octeto:
                if (cont == 1) //(First Extent):
                {
                    this.TOM = newoctet.Substring(0, 2);
                    this.DOU = newoctet.Substring(2, 3);
                    this.MRS = newoctet.Substring(5, 2);
                }
                else //(Second &+ Extent):
                    this.GHO = newoctet.Substring(0, 1);

                cont++;
            }

            int i = 1;
            string trackstatus = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            while (i <= cont)
            {
                trackstatus = trackstatus + Convert.ToString(Convert.ToInt32(paquete[pos + i], 16), 2).PadLeft(8, '0');
                i++;
            }
            this.TrackStatus = trackstatus;

            return cont;

            //falta decodificar bits
        }

        public void ComputeMode3ACode(string octetos) // Data Item I010/060
        {
            // string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            // V, G & L
            this.V_mode3A = bits.Substring(0, 1);
            this.G_mode3A = bits.Substring(1, 1);
            this.L = bits.Substring(2, 1);

            // Code 3-A 
            int i = 4;
            while(i<bits.Length)
            {
                // bits
                string character_bits = bits.Substring(i, 3);

                // num
                int character_num = Convert.ToInt32(character_bits, 2);

                // afegim al code
                this.Mode3ACode = this.Mode3ACode + Convert.ToString(character_num);

                // següent character
                i=i+3;
            }
        }

        public void ComputeTargetAddress(string octetos) // Data Item I010/220
        {
            this.TargetAddress = Convert.ToString(Convert.ToInt32(octetos, 16), 2);
        }

        public void ComputeTargetIdentification(string octetos) // Data Item I010/245
        {
            //passem a string de bits
            string bits = Convert.ToString(Convert.ToInt64(octetos, 16), 2).PadLeft(56, '0');

            // STI:
            this.STI = bits.Substring(0, 2);

            // ID:
            string targetID = bits.Substring(8, 48);
            int nchar = 0;
            while (nchar < 8)
            {
                string character = "";
                int bit = 0;
                while (bit < 6)
                {
                    character = character + Convert.ToString(targetID[nchar + bit]);
                    bit++;
                }
                this.TargetID = this.TargetID + Convert.ToString(int.Parse(character, System.Globalization.NumberStyles.HexNumber));

                nchar++;
            }
        }

        public int ComputeModeS_MBdata(string[] paquete, int pos) // Data Item I010/250
        {
            //contador dels octets q ocupa
            int cont = 1;

            //el primer octet és el número de missatges
            int REP = int.Parse(paquete[pos], System.Globalization.NumberStyles.HexNumber);

            //creamos los vectores:
            this.MBdata = new string[REP];
            this.BDS1 = new string[REP];
            this.BDS2 = new string[REP];

            //agafem les dades
            int i = 0;
            while (i < REP)
            {
                //MB Data
                string mbdata = "";

                int j = 0;
                while (j < 7) //7 octets
                {
                    mbdata = mbdata + paquete[pos + cont + j];

                    j++;
                }

                this.MBdata[i] = Convert.ToString(Convert.ToInt32(mbdata, 2), 16).PadLeft(8, '0');

                //BDS1 & BDS2
                string octet8 = Convert.ToString(Convert.ToInt32(paquete[pos + cont + 7],16),2).PadLeft(8,'0');

                this.BDS1[i] = octet8.Substring(0, 4);
                this.BDS2[i] = octet8.Substring(4, 4);

                cont = cont + 8;

                i++;
            }

            return cont;
        }

        public void ComputeVehicleFleetID(string octeto) // Data Item I010/300
        {
            //passem a string de bits
            string octeto_stringbits = Convert.ToString(Convert.ToInt32(octeto, 16), 2);

            //passem a num
            this.VehicleFeetID = int.Parse(octeto_stringbits, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeFL(string octetos) // Data Item I010/090
        {
            // string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //llegim v i g
            this.V_FL = bits.Substring(0, 1);
            this.G_FL = bits.Substring(1, 1);

            //llegim la resta (FL)
            string fl_stringbits = bits.Substring(2, 14);

            //fem el complement i multipliquem per la resolució
            this.FL = this.ComputeComplementoA2(fl_stringbits) / 4;
        }

        public void ComputeMesuredHeight(string octetos) // Data Item I010/091
        {
            //passem a string de bits
            string height = Convert.ToString(Convert.ToInt32(octetos, 16), 2);

            //fem el complement A2 i multipliquem per la resolució
            this.Height = this.ComputeComplementoA2(height) * 6.25;
        }

        public int ComputeTargetSizeAndOrientation(string octet1, string octet2, string octet3) // Data Item I010/270
        {
            //contador d'octets:
            int cont = 1;

            //passem a string de bits
            string octL = Convert.ToString(Convert.ToInt32(octet1, 16), 2).PadLeft(8, '0');
            string octO = Convert.ToString(Convert.ToInt32(octet2, 16), 2).PadLeft(8, '0');
            string octW = Convert.ToString(Convert.ToInt32(octet3, 16), 2).PadLeft(8, '0');

            //passem els bits a ints
            int length = Convert.ToInt32(octL.Substring(0, 7), 2);
            this.TargetLength = length * 1;
            if (octL.Substring(7, 1) == "1")
            {
                int orientation = Convert.ToInt32(octO.Substring(0, 7), 2);
                this.TargetOrientation = orientation * (360 / 128);
                cont++;

                if (octO.Substring(7, 1) == "1")
                {
                    int width = Convert.ToInt32(octW.Substring(0, 7), 2);
                    this.TargetWidth = Convert.ToSingle(width) * 1;
                    cont++;
                }
            }

            return cont;
        }

        public void ComputeSystemStatus(string octet) // Data Item I010/550
        {
            //passem a string de bits
            string octet_stringbits = Convert.ToString(Convert.ToInt32(octet, 16), 2).PadLeft(8, '0');

            //separem
            this.NOGO = octet_stringbits.Substring(0, 2);
            this.OVL = octet_stringbits.Substring(2, 1);
            this.TSV = octet_stringbits.Substring(3, 1);
            this.DIV = octet_stringbits.Substring(4, 1);
            this.TTF = octet_stringbits.Substring(5, 1);

            //falta decodificar bits
        }

        public void ComputePreprogrammedMessage(string octeto) // Data Item I010/310
        {
            //passem a string de bits
            string octeto_stringbits = Convert.ToString(Convert.ToInt32(octeto, 16), 2);

            //agafem TRB
            this.TRB = Convert.ToString(octeto_stringbits);

            //agafem MSG
            string msg_bits = octeto_stringbits.Substring(1, 7);

            this.MSG_num = int.Parse(msg_bits, System.Globalization.NumberStyles.HexNumber);

            //decodifiquem bits:
            if (this.MSG_num == 1)
                this.MSG = "Towing aircarft";
            if (this.MSG_num == 2)
                this.MSG = "'Follow me' operation";
            if (this.MSG_num == 3)
                this.MSG = "Runway check";
            if (this.MSG_num == 4)
                this.MSG = "Emergency operation";
            if (this.MSG_num == 5)
                this.MSG = "Work in progress";
        }

        public void ComputeStandardDeviationOfPosition(string octeto1, string octeto2, string octetos34) // Data Item I010/500
        {
            //passem a string de bits
            string octeto1_stringbits = Convert.ToString(Convert.ToInt32(octeto1, 16), 2);
            string octeto2_stringbits = Convert.ToString(Convert.ToInt32(octeto2, 16), 2);
            string octetos34_stringbits = Convert.ToString(Convert.ToInt32(octetos34, 16), 2);

            //passem a ints
            int octeto1_int = int.Parse(octeto1_stringbits, System.Globalization.NumberStyles.HexNumber);
            int octeto2_int = int.Parse(octeto2_stringbits, System.Globalization.NumberStyles.HexNumber);
            int octetos34_int = int.Parse(octetos34_stringbits, System.Globalization.NumberStyles.HexNumber);

            //passem a doubles multiplicant per resoució
            this.DevX = octeto1_int * 0.25;
            this.DevY = octeto2_int * 0.25;
            this.Covariance = octetos34_int * 0.25;
        }

        public int ComputePresence(string[] paquete, int pos) // Data Item I010/280
        {
            //contador
            int cont = 1;

            //el primer octet és el número de diferències
            int REP = int.Parse(paquete[pos], System.Globalization.NumberStyles.HexNumber);

            //creamos los vectores
            this.DRHO = new double[REP];
            this.DTHETA = new double[REP];

            //agafem les dades
            int i = 0;
            while (i < REP)
            {
                //agafem els octets en string de bits:
                string octetRHO = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                string octetoDTHETA = Convert.ToString(Convert.ToInt32(paquete[pos + cont + 1], 16), 2).PadLeft(8, '0');

                //agafem el num fent el complement a2:
                double rho = this.ComputeComplementoA2(octetRHO);
                double dtheta = this.ComputeComplementoA2(octetoDTHETA);

                //multipliquem per la resolució
                this.DRHO[i] = rho * 1;
                this.DTHETA[i] = dtheta * 0.15;

                cont = cont + 2;

                i++;
            }

            return cont;
        }

        public void ComputeAmplitudeOfPrimaryPlot(string octeto) // Data Item I010/131
        {
            //convertimos a int
            int amplitude = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució
            this.PAM = amplitude * 0.255;
        }

        public void ComputeCalculatedAcceleration(string octetoAx, string octetoAy) // Data Item I010/210
        {
            //passem a string de bits
            string ax_string = Convert.ToString(Convert.ToInt32(octetoAx, 16), 2);
            string ay_string = Convert.ToString(Convert.ToInt32(octetoAy, 16), 2);

            //passem a int els bits
            int ax = int.Parse(ax_string, System.Globalization.NumberStyles.HexNumber);
            int ay = int.Parse(ay_string, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució:
            this.Ax = Convert.ToSingle(ax) * 0.25;
            this.Ay = Convert.ToSingle(ay) * 0.25;
        }

        public double ComputeComplementoA2(string bits) //hace el complemento A2 (complemento A1 + 1)
        {
            if (bits == "1")
                return -1;
            if (bits == "0")
                return 0;
            else
            {
                if (Convert.ToString(bits[0]) == "0")
                    return Convert.ToDouble(Convert.ToInt32(bits, 2));
                else
                {
                    //elimino primer bit
                    string bitss = bits.Substring(1, bits.Length - 1);

                    //creo nuevo string cambiando 0s por 1s y viceversa
                    string newbits = "";
                    int i = 0;
                    while (i < bitss.Length)
                    {
                        if (Convert.ToString(bitss[i]) == "1")
                            newbits = newbits + "0";
                        if (Convert.ToString(bitss[i]) == "0")
                            newbits = newbits + "1";
                        i++;
                    }

                    //convertimos a int
                    double num = Convert.ToDouble(Convert.ToInt32(newbits, 2));

                    return -(num + 1);
                }
            }
        }
    }
}
