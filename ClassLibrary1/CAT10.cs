using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AsterixDecoder
{
    public class CAT10
    {
        // ATRIBUTS
        public string FSPEC;

        public string MessageType;

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
        public Boolean SPI;

        public int TimeOfDay;

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

        public string V_mode3A;
        public string G_mode3A;
        public string L;
        public string Mode3ACode;

        public string TargetAddress;

        public string STI;
        public string TargetID;

        public string REP;
        public string MBdata;
        public string BDS1;
        public string BDS2;

        public int VehicleFeetID;

        public string V_FL;
        public string G_FL;
        public int FL;

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
            int pos = longitud - longFSPEC - 1;

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
                this.ComputePositionInCartesian(paquete[pos] + paquete[pos + 1], paquete[pos + 2] + paquete[pos + 3]);
                pos = pos + 4;
            }

            if (FSPEC.Length > 8)
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

                if (FSPEC.Length > 16)
                {
                    // Mode S MB Data
                    if (Convert.ToString(FSPEC[16]) == "1")
                    {
                        this.ComputeModeS_MBdata(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5] + paquete[pos + 6] + paquete[pos + 7] + paquete[pos + 8]);
                        pos = pos + 9;
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

                }
            }
        }

        public void ComputeFSPEC(string[] paquete)
        {
            //llegim el primer octet (posicicó 3 del paquet) en binari
            string octeto1fspect = Convert.ToString(Convert.ToInt32(paquete[3], 16), 2).PadLeft(8,'0');

            //fico aquest octeto al vector de ints que forma el nostre FSPEC
            this.FSPEC = this.FSPEC + octeto1fspect;

            //llegim octets fins que la última posició d'un d'ells sigui "0"
            int pos=4;
            string FX = Convert.ToString(octeto1fspect[octeto1fspect.Length - 1]);
            while (FX == "1" && pos <= 6)
            {
                //llegim nou octeto i actualitzem FX
                string newocteto = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
                FX = Convert.ToString(newocteto[newocteto.Length - 1]);
                
                //fiquem aquest octeto al nostre FSPEC
                this.FSPEC = this.FSPEC + newocteto;

                pos++;
            }
        }

        public void ComputeDataSourceIdentifier(string octetoSAC, string octetoSIC)
        {
            //passem a string les dades a strings del codi binari
            this.SAC = Convert.ToString(Convert.ToInt32(octetoSAC, 16), 2).PadLeft(8, '0');
            this.SIC = Convert.ToString(Convert.ToInt32(octetoSIC, 16), 2).PadLeft(8, '0');
        }

        public void ComputeMessageType(string octetoMT)
        {
            //llegim aquest valor en decimal
            int MT_dec = int.Parse(octetoMT, System.Globalization.NumberStyles.HexNumber);

            //assignem el ripus de missatge (nombre tal qual hi és al manual) segons el valor [diapo19]
            if (MT_dec == 1)
                this.MessageType = "Target Report";
            if (MT_dec == 2)
                this.MessageType = "Start of Update Cycle";
            if (MT_dec == 3)
                this.MessageType = "Periodic Status Message";
            if (MT_dec == 4)
                this.MessageType = "Event-triggered Status Message";
        }

        public int ComputeTargetReportDescriptor(string[] paquete, int pos)
        {
            //contador de posicions que ocupa
            int cont = 1;

            //llegim el primer octet
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            
            //analitzem el octeto 1 (First Part):
            this.TYP=Convert.ToString(octeto1[0] + octeto1[1] + octeto1[2]);
            this.DCR=Convert.ToString(octeto1[3]);
            this.CHN=Convert.ToString(octeto1[4]);
            this.GBS=Convert.ToString(octeto1[5]);
            this.CRT=Convert.ToString(octeto1[6]);

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
                    this.SIM=Convert.ToString(newoctet[0]);
                    this.TST=Convert.ToString(newoctet[1]);
                    this.RAB=Convert.ToString(newoctet[2]);
                    this.LOP=Convert.ToString(newoctet[3] + newoctet[4]);
                    this.TOT=Convert.ToString(newoctet[5] + newoctet[6]);
                }
                else //(Second &+ Extent):
                {
                        //SPI
                    if (Convert.ToString(newoctet[0]) == "0")
                        this.SPI = false;
                    if (Convert.ToString(newoctet[0]) == "1")
                        this.SPI = true;
                }
                
                cont++;
            }

            return cont;
        }

        public void ComputeTimeOfDay(string octetoTimeOfDay)
        {
            //calculem quin minut del dia és
            this.TimeOfDay = int.Parse(octetoTimeOfDay, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputePositionInWGS84(string OctLatWGS, string OctLonWGS)
        {
            //passem les coordenades a doubles
            this.LatitudeWGS = double.Parse(OctLatWGS, System.Globalization.NumberStyles.HexNumber);
            this.LongitudeWGS = double.Parse(OctLonWGS, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputePositionInPolar(string OctRHO, string OctTheta)
        {
            //passem les coordenades a doubles
            this.RHO = double.Parse(OctRHO, System.Globalization.NumberStyles.HexNumber);
            this.Theta = double.Parse(OctTheta, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputePositionInCartesian(string OctX, string OctY)
        {
            //passem les coordenades a doubles
            this.Xcomponent = double.Parse(OctX, System.Globalization.NumberStyles.HexNumber);
            this.Ycomponent = double.Parse(OctY, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeTrackVelocityInPolar(string OctGS, string OctTA)
        {
            //passem les coordenades a doubles
            this.GroundSpeed = double.Parse(OctGS,System.Globalization.NumberStyles.HexNumber);
            this.TrackAngle = double.Parse(OctTA,System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeTrackVelocityInCartesian(string OctVx, string OctVy)
        {
            //passem les coordenades a doubles
            this.Vx = double.Parse(OctVx, System.Globalization.NumberStyles.HexNumber);
            this.Vy = double.Parse(OctVy, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeTrackNumber(string OctTN)
        {
            //passem a int
            this.TrackNumber = int.Parse(OctTN,System.Globalization.NumberStyles.HexNumber);
        }

        public int ComputeTrackStatus(string[] paquete, int pos)
        {
            //contador de posicions que ocupa
            int cont = 1;

            //llegim el primer octet
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            //analitzem el octeto 1 (First Part):
            this.CNF = Convert.ToString(octeto1[0]);
            this.TRE = Convert.ToString(octeto1[1]);
            this.CST = Convert.ToString(octeto1[2] + octeto1[3]);
            this.MAH = Convert.ToString(octeto1[4]);
            this.TCC = Convert.ToString(octeto1[5]);
            this.STH = Convert.ToString(octeto1[6]);

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
                    this.TOM = Convert.ToString(newoctet[0] + newoctet[1]);
                    this.DOU = Convert.ToString(newoctet[2] + newoctet[3] + newoctet[4]);
                    this.MRS = Convert.ToString(newoctet[5] + newoctet[6]);
                }
                else //(Second &+ Extent):
                    this.GHO = Convert.ToString(newoctet[0]);

                cont++;
            }

            return cont;
        }

        public void ComputeMode3ACode(string octetos)
        {
            this.V_mode3A = Convert.ToString(octetos[0]);
            this.G_mode3A = Convert.ToString(octetos[1]);
            this.L = Convert.ToString(octetos[2]);
            this.Mode3ACode = Convert.ToString(octetos[3] + octetos[4] + octetos[5] + octetos[6] + octetos[7] + octetos[8] + octetos[9] + octetos[10] + octetos[11] + octetos[12] + octetos[13] + octetos[14] + octetos[15]);
        }

        public void ComputeTargetAddress(string octetos)
        {
            this.TargetAddress = Convert.ToString(octetos);
        }

        public void ComputeTargetIdentification(string octetos)
        {
            this.STI = Convert.ToString(octetos[0] + octetos[1]);

            int pos = 0;
            while (pos < octetos.Length)
            {
                string character = "";
                int bit = 1;
                while (bit <= 6)
                {
                    character = character + Convert.ToString(octetos[bit]);
                    bit++;
                }
                this.TargetID = this.TargetID + Convert.ToString(int.Parse(character, System.Globalization.NumberStyles.HexNumber));
                pos++;
            }
        }

        public void ComputeModeS_MBdata(string octetos)
        {
            this.REP = Convert.ToString(octetos[0] + octetos[1] + octetos[2] + octetos[3] + octetos[4] + octetos[5] + octetos[6]);
            int cont = 8;
            while (cont <= 63)
            {
                this.MBdata = this.MBdata + Convert.ToString(octetos[cont]);
                cont++;
            }
            this.BDS1 = Convert.ToString(octetos[64] + octetos[65] + octetos[66] + octetos[67]);
            this.BDS2 = Convert.ToString(octetos[68] + octetos[69] + octetos[70] + octetos[71]);
        }

        public void ComputeVehicleFleetID(string octeto)
        {
            this.VehicleFeetID = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeFL(string octetos)
        {
            this.V_FL = Convert.ToString(octetos[0]);
            this.G_FL = Convert.ToString(octetos[1]);

            int cont = 2;
            string FL = "";
            while (cont <= 15)
            {
                FL = FL + Convert.ToString(octetos[cont]);
                cont++;
            }

            this.FL = int.Parse(FL, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
