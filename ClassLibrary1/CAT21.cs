using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixDecoder
{
    public class CAT21
    {
        // ATRIBUTS:

        public string FSPEC;

        public string RA;
        public string TC;
        public string TS;
        public string ARV;
        public string CDTI;
        public string notTCAS;
        public string SA;
        public string OperationalStatus;

        public string SAC;
        public string SIC;
        public string DataSourceID;

        public string serviceID;

        public double RP;

        public int ECAT;

        public int ATP;
        public int ARC;
        public int RC;
        public int RAB;
        public int DCR;
        public int GBS;
        public int SIM;
        public int TST;
        public int SAA;
        public int CL;
        public int LLC;
        public int IPC;
        public int NOGO;
        public int CPR;
        public int LDPJ;
        public int RCF;

        public string Mode3ACode;

        public double TimeOfApplicabilityForPosition;

        public double TimeOfApplicabilityForVelocity;

        public double TimeOfMessageReceptionForPosition;

        public string FSI_Position;
        public double TimeOfMessageReceptionForPosition_HighPrecision;

        public double TimeOfMessageReceptionForVelocity;

        public string FSI_Velocity;
        public double TimeOfMessageReceptionForVelocity_HighPrecision;

        public double TimeOfAsterixReportTransmission;

        public string TargetAddress;

        public int NUCr_NACv;
        public int NUCp_NIC;
        public int NICbaro;
        public int SIL;
        public int NACp;
        public int SILS;
        public int SDA;
        public int GVA;
        public int PIC;

        public int NAV;
        public int NVB;
        public string[] TCA;
        public string[] NC;
        public string[] TCP;
        public double[] Altitude;
        public double[] Latitude;
        public double[] Longitude;
        public string[] PointType;
        public string[] TD;
        public string[] TRA;
        public string[] TOA;
        public int[] TOV;
        public double[] TTR;

        public double LatitudeWGS;
        public double LongitudeWGS;

        public double LatitudeWGS_HR;
        public double LongitudeWGS_HR;

        public double MAM;

        public double GH;

        public double FL;

        public string SAS;
        public string Source;
        public double SelectedAltitude;

        public string MV;
        public string AH;
        public string AM;
        public double SelectedAltitude_FS;

        public string IM;
        public double AirSpeed;

        public string RE_TrueAirspeed;
        public double TrueAirspeed;

        public double MagneticHeading;

        public string RE_BarometricVerticalRate;
        public double BarometricVerticalRate;

        public string RE_GeometricVerticalRate;
        public double GeometricVerticalRate;

        public string RE_AirborneGroundVector;
        public double GroundSpeed;
        public double TrackAngle;

        public int TrackNumber;

        public double TAR;

        public string TargetID;

        public string ICF;
        public string LNAV;
        public string ME;
        public int PS;
        public int SS;

        public string VNS;
        public int VN;
        public int LTT;

        public int WindSpeed;
        public int WindDirection;
        public double Temperature;
        public int Turbulence;

        public double RollAngle;

        public string[] MBdata;
        public string[] BDS1;
        public string[] BDS2;

        public string TYP;
        public string STYP;
        public string ARA;
        public string RAC;
        public string RAT;
        public string MTE;
        public string TTI;
        public string TID;

        public string POA;
        public string CDTI_S;
        public string B2low;
        public string RAS;
        public string IDENT;
        public int LmasW;

        public double AOS_age;
        public double TRD_age;
        public double M3A_age;
        public double QI_age;
        public double TI_age;
        public double MAM_age;
        public double GH_age;
        public double FL_age;
        public double ISA_age;
        public double FSA_age;
        public double AS_age;
        public double TAS_age;
        public double MH_age;
        public double BVR_age;
        public double GVR_age;
        public double GV_age;
        public double TAR_age;
        public double TID_age;
        public double TS_age;
        public double MET_age;
        public double ROA_age;
        public double ARA_age;
        public double SCC_age;

        public string RID;


        // CONSTRUCTOR:
        public CAT21(string[] paquete) //decodifica el missatge (paquet)
        {
            //el primer octet (posició 0) és la categoria --> ja l'hem llegida

            //els segon i tercet octet (posicions 1 i 2) és la longitud del missatge = nombre d'octets
            int longitud = int.Parse(paquete[1] + paquete[2], System.Globalization.NumberStyles.HexNumber);
            //sense comptar aquests tres primers octets:
            longitud = longitud - 3;

            //llegim el FSPEC
            this.ComputeFSPEC(paquete);
            int longFSPEC = this.FSPEC.Length / 8;

            //posició on comencen les dades
            int pos = 0 + 1 + 2 + longFSPEC;

            // Data Source Identification
            if (Convert.ToString(this.FSPEC[0]) == "1")
            {
                this.ComputeDataSourceIdentification(paquete[pos], paquete[pos + 1]);
                pos = pos + 2;
            }

            // Target Report Descriptor
            if (Convert.ToString(this.FSPEC[1]) == "1")
                pos = pos + this.ComputeTargetReportDescriptor(paquete, pos);

            // Track Number
            if (Convert.ToString(this.FSPEC[2]) == "1")
            {
                this.ComputeTrackNumber(paquete[pos] + paquete[pos + 1]);
                pos = pos + 2;
            }

            // Service Identification
            if (Convert.ToString(this.FSPEC[3]) == "1")
            {
                this.ComputeServiceIdentification(paquete[pos]);
            }

            // Time of Applicability for Position
            if (Convert.ToString(this.FSPEC[4]) == "1")
            {
                this.ComputeTimeOfApplicabilityForPosition(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                pos = pos + 3;
            }

            // Position in WGS-84 coordinates
            if (Convert.ToString(this.FSPEC[5]) == "1")
            {
                this.ComputePositionInWGS84(paquete[pos] + paquete[pos + 1] + paquete[pos + 2], paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5]);
                pos = pos + 6;
            }

            // Position in WGS-84 coordinates - High Resolution
            if (Convert.ToString(this.FSPEC[6]) == "1")
            {
                this.ComputeHighResolutionPositionInWGS84(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3], paquete[pos + 4] + paquete[pos + 5] + paquete[pos + 6] + paquete[pos + 7]);
                pos = pos + 8;
            }

            if (longFSPEC >= 2)
            {
                // Time of Applicability for Velocity
                if (Convert.ToString(this.FSPEC[8]) == "1")
                {
                    this.ComputeTimeOfApplicabilityForVelocity(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Air Speed
                if (Convert.ToString(this.FSPEC[9]) == "1")
                {
                    this.ComputeAirSpeed(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // True Air Speed
                if (Convert.ToString(this.FSPEC[10]) == "1")
                {
                    this.ComputeTrueAirspeed(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // Target Address
                if (Convert.ToString(this.FSPEC[11]) == "1")
                {
                    this.ComputeTargetAddress(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Time of Message Reception of Position
                if (Convert.ToString(this.FSPEC[12]) == "1")
                {
                    this.ComputeTimeOfMessageReceptionForPosition(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Time of Message Reception of Position - High Precision
                if(Convert.ToString(this.FSPEC[13])=="1")
                {
                    this.ComputeTimeOfMessageReceptionForPosition_HighPrecision(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3]);
                    pos = pos + 4;
                }

                // Time of Message Reception of Velocity
                if (Convert.ToString(this.FSPEC[14]) == "1")
                {
                    this.ComputeTimeOfMessageReceptionForVelocity(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                if (longFSPEC >= 3)
                {
                    // Time of Message Reception of Velocity - High Precision
                    if (Convert.ToString(this.FSPEC[16]) == "1")
                    {
                        this.ComputeTimeOfMessageReceptionForVelocity_HighPrecision(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3]);
                        pos = pos + 4;
                    }

                    // Geometric Height
                    if (Convert.ToString(this.FSPEC[17]) == "1")
                    {
                        this.ComputeGeometricHeight(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Quality Indicators
                    if (Convert.ToString(this.FSPEC[18]) == "1")
                        pos = pos + this.ComputeQualityIndicators(paquete, pos);

                    // MOPS Version
                    if (Convert.ToString(this.FSPEC[19]) == "1")
                    {
                        this.ComputeMOPSversion(paquete[pos]);
                        pos++;
                    }

                    // Mode 3/A Code
                    if (Convert.ToString(this.FSPEC[20]) == "1")
                    {
                        this.ComputeMode3ACode(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Roll Angle 
                    if (Convert.ToString(this.FSPEC[21]) == "1")
                    {
                        this.ComputeRollAngle(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Flight Level
                    if (Convert.ToString(this.FSPEC[22]) == "1")
                    {
                        this.ComputeFL(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    if (longFSPEC >= 4)
                    {
                        // Magnetic Heading
                        if (Convert.ToString(this.FSPEC[24]) == "1")
                        {
                            this.ComputeMagneticHeading(paquete[pos] + paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Target Status
                        if (Convert.ToString(this.FSPEC[25]) == "1")
                        {
                            this.ComputeTargetStatus(paquete[pos]);
                            pos++;
                        }

                        // Barometric Vertical Rate
                        if (Convert.ToString(this.FSPEC[26]) == "1")
                        {
                            this.ComputeBarometricVerticalRate(paquete[pos] + paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Geometric Vertical Rate
                        if (Convert.ToString(this.FSPEC[27]) == "1")
                        {
                            this.ComputeGeometricVerticalRate(paquete[pos] + paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Airborne Ground Vector
                        if (Convert.ToString(this.FSPEC[28]) == "1")
                        {
                            this.ComputeAirborneGroundVector(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3]);
                            pos = pos + 4;
                        }

                        // Track Angle Rate
                        if (Convert.ToString(this.FSPEC[29]) == "1")
                        {
                            this.ComputeTrackAngleRate(paquete[pos] + paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Time of Report Transmission
                        if (Convert.ToString(this.FSPEC[30]) == "1")
                        {
                            this.ComputeTimeOfAsterixReportTransmission(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                            pos = pos + 3;
                        }

                        if (longFSPEC >= 5)
                        {
                            // Target Identification
                            if (Convert.ToString(this.FSPEC[32]) == "1")
                            {
                                this.ComputeTargetIdentification(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5]);
                                pos = pos + 6;
                            }

                            // Emitter Category
                            if (Convert.ToString(this.FSPEC[33]) == "1")
                            {
                                this.ComputeEmitterCategory(paquete[pos]);
                                pos++;
                            }

                            // Met Information
                            if (Convert.ToString(this.FSPEC[34]) == "1")
                                pos = pos + this.ComputeMetInformation(paquete, pos);

                            // Selected Altitude
                            if (Convert.ToString(this.FSPEC[35]) == "1")
                            {
                                this.ComputeSelectedAltitude(paquete[pos] + paquete[pos + 1]);
                                pos = pos + 2;
                            }

                            // Final State Selected Altitude
                            if (Convert.ToString(this.FSPEC[36]) == "1")
                            {
                                this.ComputeFinalStateSelectedAltitude(paquete[pos] + paquete[pos + 1]);
                                pos = pos + 2;
                            }

                            // Trajectory Intend
                            if (Convert.ToString(this.FSPEC[37]) == "1")
                                pos = pos + this.ComputeTrajectoryIntend(paquete, pos);

                            // Service Management
                            if (Convert.ToString(this.FSPEC[38]) == "1")
                            {
                                this.ComputeServiceManagement(paquete[pos]);
                                pos++;
                            }

                            if (longFSPEC >= 6)
                            {
                                // Aircraft Operational Status
                                if (Convert.ToString(this.FSPEC[40]) == "1")
                                {
                                    this.ComputeAircraftOperationalStatus(paquete[pos]);
                                    pos++;
                                }

                                // Surface Capabilities and Characteristics
                                if (Convert.ToString(this.FSPEC[41]) == "1")
                                    pos = pos + this.ComputeSurfaceCapabilitiesAndCharacteristics(paquete, pos);

                                // Message Amplitude
                                if (Convert.ToString(this.FSPEC[42]) == "1")
                                {
                                    this.ComputeMessageAmplitude(paquete[pos]);
                                    pos++;
                                }

                                // Mode S MB Data
                                if (Convert.ToString(this.FSPEC[43]) == "1")
                                    pos = pos + this.ComputeModeS_MBdata(paquete, pos);

                                // ACAS Resolution Advisory Report
                                if (Convert.ToString(this.FSPEC[44]) == "1")
                                {
                                    this.ComputeACAS_RAreport(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5] + paquete[pos + 6]);
                                    pos = pos + 7;
                                }

                                // Receiver ID
                                if (Convert.ToString(this.FSPEC[45]) == "1")
                                {
                                    this.ComputeReceiverID(paquete[pos]);
                                    pos++;
                                }

                                // Data Ages
                                if (Convert.ToString(this.FSPEC[46]) == "1")
                                    pos = pos + this.ComputeDataAges(paquete, pos);

                                if (longFSPEC == 7)
                                {
                                    //nidea
                                }
                            }
                        }
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
            while (continua==true && pos <= 9)
            {
                //llegim nou octeto
                string newocteto = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

                if (newocteto.Substring(7, 1) == "1") //fiquem aquest octeto al nostre FSPEC
                {
                    this.FSPEC = this.FSPEC + newocteto;

                    pos++;
                }
                else
                    continua = false;
            }
        }

        public void ComputeAircraftOperationalStatus(string octeto) // Data Item I021/008
        {
            //passem a string de bits
            string octeto_bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //llegim bits
            string RA = octeto_bits.Substring(0, 1);
            if (RA == "1")
                this.RA = "TCAS RA active";
            if (RA == "0")
                this.RA = "TCAS II or ACAS RA not active";

            int TC = Convert.ToInt32(octeto_bits.Substring(1, 2), 2);
            if (TC == 0)
                this.TC = "No capability for Trajectory Change Reports";
            if (TC == 1)
                this.TC = "Support for TC+0 reports only";
            if (TC == 2)
                this.TC = "Support for multiple TC reports";
            if (TC == 3)
                this.TC = "Target Trajectory Change Report Capability: reserved";

            string TS = octeto_bits.Substring(3, 1);
            if (TS == "0")
                this.TS = "No capability to support Target State Reports";
            if (TS == "1")
                this.TS = "Capable of supporting Target State Reports";

            string ARV = octeto_bits.Substring(4, 1);
            if (ARV == "0")
                this.ARV = "No capability to generate ARV-reports";
            if (ARV == "1")
                this.ARV = "Capable of generate ARV-reports";

            string CDTI = octeto_bits.Substring(5, 1);
            if (CDTI == "0")
                this.CDTI = "CDTI not operational";
            if (CDTI == "1")
                this.CDTI = "CDTI operational";

            string notTCAS = octeto_bits.Substring(6, 1);
            if (notTCAS == "0")
                this.notTCAS = "TCAS operational";
            if (notTCAS == "1")
                this.notTCAS = "TCAS not operational";

            string SA = octeto_bits.Substring(7, 1);
            if (SA == "1")
                this.SA = "Single antenna only";
            if (SA == "0")
                this.SA = "Antenna Diversity";

            this.OperationalStatus = " - " + this.RA + "\n - " + this.TC + "\n - " + this.TS + "\n - " + this.ARV + "\n - " + this.CDTI + "\n - " + this.notTCAS + "\n - " + this.SA;
        }

        public void ComputeDataSourceIdentification(string octetoSAC, string octetoSIC) // Data Item I021/010
        {
            //passem a string de bits
            this.SAC = Convert.ToString(Convert.ToInt32(octetoSAC, 16));
            this.SIC = Convert.ToString(Convert.ToInt32(octetoSIC, 16));

            if (this.SAC == "0" && this.SIC == "107")
                this.DataSourceID = "Data flow local to the airport: Barcelona - LEBL";
        }

        public void ComputeServiceIdentification(string octeto) // Data Item I021/015
        {
            //passem a string de bits
            this.serviceID = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');
        }

        public void ComputeServiceManagement(string octeto) // Data Item I021/016
        {
            //passem a int
            int rp_int=int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució
            this.RP = rp_int * 0.5;
        }

        public void ComputeEmitterCategory(string octeto) // Data Item I021/020
        {
            //llegim el número
            this.ECAT = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);

            // falta decodificar bits
        }

        public int ComputeTargetReportDescriptor(string[] paquete, int pos) // Data Item I021/040
        {
            //contador de octetos
            int cont = 1;

            //llegim primer octet
            string octet1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            
            this.ATP = int.Parse(octet1.Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
            this.ARC = int.Parse(octet1.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            this.RC = int.Parse(octet1.Substring(5, 1), System.Globalization.NumberStyles.HexNumber);
            this.RAB = int.Parse(octet1.Substring(6, 1), System.Globalization.NumberStyles.HexNumber);

            if(octet1.Substring(7,1) == "1") // First Extension
            {
                string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                
                this.DCR = int.Parse(octet2.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                this.GBS = int.Parse(octet2.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                this.SIM = int.Parse(octet2.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                this.TST = int.Parse(octet2.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                this.SAA = int.Parse(octet2.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
                this.CL = int.Parse(octet2.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                cont++;

                if (octet2.Substring(7, 1) == "1") // Second Extension: Error Conditions
                {
                    string octet3 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                    
                    this.LLC = int.Parse(octet3.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    this.IPC = int.Parse(octet3.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    this.NOGO = int.Parse(octet3.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                    this.CPR = int.Parse(octet3.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
                    this.LDPJ = int.Parse(octet3.Substring(5, 1), System.Globalization.NumberStyles.HexNumber);
                    this.RCF = int.Parse(octet3.Substring(6, 1), System.Globalization.NumberStyles.HexNumber);

                    cont++;
                }
            }

            return cont;
        }

        public void ComputeMode3ACode(string octetos) // Data Item I021/070
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            this.Mode3ACode = bits.Substring(4, 12);
        }

        public void ComputeTimeOfApplicabilityForPosition(string octetos) // Data Item I021/071
        {
            //passem a int
            int time = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per a la resolució
            this.TimeOfApplicabilityForPosition = time / 128;
        }

        public void ComputeTimeOfApplicabilityForVelocity(string octetos) // Data Item I021/072
        {
            //passem a int
            int time = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per a la resolució
            this.TimeOfApplicabilityForVelocity = time / 128;
        }

        public void ComputeTimeOfMessageReceptionForPosition(string octetos) // Data Item I021/073
        {
            //passem a int
            int time = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per a la resolució
            this.TimeOfMessageReceptionForPosition = time / 128;
        }

        public void ComputeTimeOfMessageReceptionForPosition_HighPrecision(string octetos) // Data Item I021/074
        {
            //passem a string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(32, '0');

            //agafem FSI
            this.FSI_Position = octetos_bits.Substring(0, 2);

            //agafem els segons
            string time_bits = octetos_bits.Substring(2, 30);

            //passem a int
            int time = Convert.ToInt32(time_bits, 2);

            //multipliquem per la resolució
            this.TimeOfMessageReceptionForPosition_HighPrecision = time * Math.Pow(2, -30);

            //afegim el FSI:
            if (this.FSI_Position == "10")
                this.TimeOfMessageReceptionForPosition_HighPrecision = this.TimeOfMessageReceptionForPosition_HighPrecision - 1;
            if (this.FSI_Position == "01")
                this.TimeOfMessageReceptionForPosition_HighPrecision = this.TimeOfMessageReceptionForPosition_HighPrecision + 1;
        }

        public void ComputeTimeOfMessageReceptionForVelocity(string octetos) // Data Item I021/075
        {
            //passem a int
            int time = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per a la resolució
            this.TimeOfMessageReceptionForVelocity = time / 128;
        }

        public void ComputeTimeOfMessageReceptionForVelocity_HighPrecision(string octetos) // Data Item I021/076
        {
            //passem a string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(32, '0');

            //agafem FSI
            this.FSI_Velocity = octetos_bits.Substring(0, 2);

            //agafem els segons
            string time_bits = octetos_bits.Substring(2, 30);

            //passem a int
            int time = Convert.ToInt32(time_bits, 2);

            //multipliquem per la resolució
            this.TimeOfMessageReceptionForVelocity_HighPrecision = time * Math.Pow(2, -30);

            //afegim el FSI:
            if (this.FSI_Velocity == "10")
                this.TimeOfMessageReceptionForVelocity_HighPrecision = this.TimeOfMessageReceptionForVelocity_HighPrecision - 1;
            if (this.FSI_Velocity == "01")
                this.TimeOfMessageReceptionForVelocity_HighPrecision = this.TimeOfMessageReceptionForVelocity_HighPrecision + 1;
        }

        public void ComputeTimeOfAsterixReportTransmission(string octetos) // Data Item I021/077
        {
            //passem a int
            int time = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per a la resolució
            this.TimeOfAsterixReportTransmission = time / 128;
        }

        public void ComputeTargetAddress(string octetos) // Data Item I021/080
        {
            this.TargetAddress = Convert.ToString(Convert.ToInt32(octetos, 16), 2);
        }

        public int ComputeQualityIndicators(string[] paquete, int pos) // Data Item I021/090
        {
            //contador de octetos
            int cont = 1;

            //llegim primer octet: Primary Subfield
            string octet1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            this.NUCr_NACv = int.Parse(octet1.Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
            this.NUCp_NIC = int.Parse(octet1.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);

            if (octet1.Substring(7, 1) == "1") // First Extension: Navigation Accuracy Category for Position
            {
                string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                this.NICbaro = int.Parse(octet2.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                this.SIL = int.Parse(octet2.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                this.NACp = int.Parse(octet2.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);

                cont++;

                if (octet2.Substring(7, 1) == "1") // Second Extension: Position Quality Indicators
                {
                    string octet3 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                    this.SILS = int.Parse(octet3.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    this.SDA = int.Parse(octet3.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                    this.GVA = int.Parse(octet3.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                    cont++;

                    if (octet3.Substring(7, 1) == "1") // Third Extension: Position Quality Indicators
                    {
                        string octet4 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                        this.PIC = int.Parse(octet4.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                        cont++;
                    }
                }
            }

            return cont;
        }

        public int ComputeTrajectoryIntend(string[] paquete, int pos) // Data Item I021/110
        {
            //contador de octetos
            int cont = 1;

            //llegim primer octet: Primary Subfield
            string octet1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            int TIS = int.Parse(octet1.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
            int TID = int.Parse(octet1.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);

            if (TIS == 1) // Subfield #1 - Trajectory Intent Status
            {
                string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                this.NAV = int.Parse(octet2.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                this.NVB = int.Parse(octet2.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);

                cont++;
            }

            if (TID == 1)// Subfield #2 - Trajectory Intent Data
            {
                //llegim num de repeticions
                int rep = int.Parse(paquete[pos], System.Globalization.NumberStyles.HexNumber);

                //creamos vectores
                this.TCA = new string[rep];
                this.NC = new string[rep];
                this.TCP = new string[rep];
                this.Altitude = new double[rep];
                this.Latitude = new double[rep];
                this.Longitude = new double[rep];
                this.PointType = new string[rep];
                this.TD = new string[rep];
                this.TRA = new string[rep];
                this.TOA = new string[rep];
                this.TOV = new int[rep];
                this.TTR = new double[rep];

                //agafem les dades
                int i = 0;
                while (i < rep)
                {
                    //agafem octets en string de bits
                    string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 1], 16), 2).PadLeft(8, '0');
                    string octets_Altitude = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 2], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 3], 16), 2).PadLeft(8, '0');
                    string octets_Latitude = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 4], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 5], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 6], 16), 2).PadLeft(8, '0');
                    string octets_Longitude = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 7], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 8], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 9], 16), 2).PadLeft(8, '0');
                    string octet11 = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 10], 16), 2).PadLeft(8, '0');
                    string octets_TOV = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 11], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 12], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 13], 16), 2).PadLeft(8, '0');
                    string octets_TTR = Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 14], 16), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(paquete[pos + (i * 15) + 15], 16), 2).PadLeft(8, '0');

                    //llegim els que són strings
                    this.TCA[i] = octet2.Substring(0, 1);
                    this.NC[i] = octet2.Substring(1, 1);
                    this.TCP[i] = octet2.Substring(2, 6);
                    this.PointType[i] = octet11.Substring(0, 4);
                    this.TD[i] = octet11.Substring(4, 2);
                    this.TRA[i] = octet11.Substring(6, 1);
                    this.TOA[i] = octet11.Substring(7, 1);

                    //fem complement A2 dels que calgui + multipliquem per resolució
                    this.Altitude[i] = 10 * this.ComputeComplementoA2(octets_Altitude);
                    this.Latitude[i] = (180 / Math.Pow(2, 23)) * this.ComputeComplementoA2(octets_Latitude);
                    this.Longitude[i] = (180 / Math.Pow(2, 23)) * this.ComputeComplementoA2(octets_Longitude);

                    //passem a int el TOV
                    this.TOV[i] = Convert.ToInt32(octets_TOV);

                    //passem a double el TTR multiplicant int per resolució
                    this.TTR[i] = 0.01 * Convert.ToInt32(octets_TTR, 2);

                    i++;
                }

                cont = cont + 1 + (15 * rep);
            }

            return cont;
        }

        public void ComputePositionInWGS84(string OctLatWGS, string OctLonWGS) // Data Item I021/130
        {
            //passem a string de bits
            string lat = Convert.ToString(Convert.ToInt32(OctLatWGS, 16), 2);
            string lon = Convert.ToString(Convert.ToInt32(OctLonWGS, 16), 2);

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.LatitudeWGS = this.ComputeComplementoA2(lat) * (180 / Math.Pow(2, 31));
            this.LongitudeWGS = this.ComputeComplementoA2(lon) * (180 / Math.Pow(2, 31));
        }

        public void ComputeHighResolutionPositionInWGS84(string OctLatWGS, string OctLonWGS) // Data Item I021/131
        {
            //passem a string de bits
            string lat = Convert.ToString(Convert.ToInt32(OctLatWGS, 16), 2);
            string lon = Convert.ToString(Convert.ToInt32(OctLonWGS, 16), 2);

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.LatitudeWGS_HR = this.ComputeComplementoA2(lat) * (180 / Math.Pow(2, 31));
            this.LongitudeWGS_HR = this.ComputeComplementoA2(lon) * (180 / Math.Pow(2, 31));
        }

        public void ComputeMessageAmplitude(string octeto) // Data Item I021/132
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2);

            //complement a2 + resolució
            this.MAM = 1 * this.ComputeComplementoA2(bits);
        }

        public void ComputeGeometricHeight(string octetos) // Data Item I021/140
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2);

            //complemento a2 + resolució
            this.GH = 6.25 * this.ComputeComplementoA2(bits);
        }

        public void ComputeFL(string octetos) // Data Item I021/145
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2);

            //complemento a2 + resolució
            this.FL = (1 / 4) * this.ComputeComplementoA2(bits);
        }

        public void ComputeSelectedAltitude(string octetos) // Data Item I021/146
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem SAS, Source i l'altitud en string de bits
            this.SAS = octetos_bits.Substring(0, 1);
            this.Source = octetos_bits.Substring(1, 2);
            string altitude_bits = octetos_bits.Substring(3, 13);

            //complement a2 y resolució a l'altitud
            this.SelectedAltitude = 25 * this.ComputeComplementoA2(altitude_bits);
        }

        public void ComputeFinalStateSelectedAltitude(string octetos) // Data Item I021/148
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem 
            this.MV = octetos_bits.Substring(0, 1);
            this.AH = octetos_bits.Substring(1, 1);
            this.AM = octetos_bits.Substring(2, 1);
            string altitude_bits = octetos_bits.Substring(3, 13);

            //complement a2 y resolució
            this.SelectedAltitude_FS = 25 * this.ComputeComplementoA2(altitude_bits);
        }

        public void ComputeAirSpeed(string octetos) // Data Item I021/150
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem 
            this.IM = octetos_bits.Substring(0, 1);
            string airspeed_bits = octetos_bits.Substring(1, 15);

            //passem a int
            double airspeed_num = Convert.ToInt32(airspeed_bits, 2);

            //resolució
            if (this.IM == "0")
                this.AirSpeed = Math.Pow(2, -14) * airspeed_num;
            if (this.IM == "1")
                this.AirSpeed = 0.001 * airspeed_num;
        }

        public void ComputeTrueAirspeed(string octetos) // Data Item I021/151
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem 
            this.RE_TrueAirspeed = octetos_bits.Substring(0, 1);
            string airspeed_bits = octetos_bits.Substring(1, 15);

            //passem a int i resolució
            this.TrueAirspeed = Convert.ToInt32(airspeed_bits, 2);
        }

        public void ComputeMagneticHeading(string octetos) // Data Item I021/152
        {
            //passem a int i multipliquem per la resolució
            this.MagneticHeading = (360 / Math.Pow(2, 16)) * int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeBarometricVerticalRate(string octetos) // Data Item I021/155
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //separem 
            this.RE_BarometricVerticalRate = octetos_bits.Substring(0, 1);
            string BarometricVR_bits = octetos_bits.Substring(1, 15);

            //passem a int i resolució
            this.BarometricVerticalRate = 6.25 * Convert.ToInt32(BarometricVR_bits);
        }

        public void ComputeGeometricVerticalRate(string octetos) // Data Item I021/157
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem 
            this.RE_GeometricVerticalRate = octetos_bits.Substring(0, 1);
            string GeometricVR_bits = octetos_bits.Substring(1, 15);

            //complement a2 i resolució
            this.GeometricVerticalRate = 6.25 * this.ComputeComplementoA2(GeometricVR_bits);
        }

        public void ComputeAirborneGroundVector(string octetos) // Data Item I021/160
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem 
            this.RE_AirborneGroundVector = octetos_bits.Substring(0, 1);
            this.GroundSpeed = Math.Pow(2, -14) * Convert.ToInt32(octetos_bits.Substring(1, 15), 2);
            this.TrackAngle = (360 / Math.Pow(2, 16)) * Convert.ToInt32(octetos_bits.Substring(16, 16), 2);
        }

        public void ComputeTrackNumber(string octetos) // Data Item I021/161
        {
            //passem a binari
            string tracknum_bin = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //tallem els 4 primers bits
            string tracknum_valid = tracknum_bin.Substring(4, 12);

            //passem a int
            this.TrackNumber = Convert.ToInt32(tracknum_valid, 2);
        }

        public void ComputeTrackAngleRate(string octetos) // Data Item I021/165
        {
            //passem a binari
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //tallem els 6 primers bits
            string bits_valid = bits.Substring(6, 10);

            //complement a2 i resolució
            this.TAR = (1 / 32) * this.ComputeComplementoA2(bits_valid);
        }

        public void ComputeTargetIdentification(string octetos) // Data Item I021/170
        {
            //string de bits
            string targetID = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');
            
            //llegim
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

        public void ComputeTargetStatus(string octeto) // Data Item I021/200
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //llegim
            this.ICF = bits.Substring(0, 1);
            this.LNAV = bits.Substring(1, 1);
            this.ME = bits.Substring(2, 1);
            this.PS = int.Parse(bits.Substring(3, 3), System.Globalization.NumberStyles.HexNumber);
            this.SS = int.Parse(bits.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }

        public void ComputeMOPSversion(string octetos) // Data Item I021/210
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //llegim
            this.VNS = bits.Substring(1, 1);
            this.VN = int.Parse(bits.Substring(2, 3), System.Globalization.NumberStyles.HexNumber);
            this.LTT = int.Parse(bits.Substring(5, 3), System.Globalization.NumberStyles.HexNumber);
        }

        public int ComputeMetInformation(string[] paquete, int pos) // Data Item I021/220
        {
            //contador d'octets
            int cont = 1;

            //agafem primer octeto i el passem a string de bits
            string octeto = paquete[pos];
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //llegim quins camps hi ha
            if(bits.Substring(0, 1) == "1")
            {
                this.WindSpeed = int.Parse(paquete[pos + cont] + paquete[pos + cont + 1], System.Globalization.NumberStyles.HexNumber);
                cont = cont + 2;
            }
            if(bits.Substring(1, 1) == "1")
            {
                this.WindDirection = int.Parse(paquete[pos + cont] + paquete[pos + cont + 1], System.Globalization.NumberStyles.HexNumber);
                cont = cont + 2;
            }
            if(bits.Substring(2, 1) == "1")
            {
                string octetoTMP = paquete[pos + cont] + paquete[pos + cont + 1];
                string bitsTMP = Convert.ToString(Convert.ToInt32(octetoTMP, 16), 2);
                this.Temperature = 0.25 * this.ComputeComplementoA2(bitsTMP);
                cont = cont + 2;
            }
            if(bits.Substring(3, 1) == "1")
            {
                this.Turbulence = int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }

            return cont;
        }

        public void ComputeRollAngle(string octetos) // Data Item I021/230
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2);

            //complement a2
            double rollangle = this.ComputeComplementoA2(bits);

            //resolució
            this.RollAngle = rollangle * 0.01;
        }

        public int ComputeModeS_MBdata(string[] paquete, int pos) // Data Item I021/250
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

                this.MBdata[i] = Convert.ToString(Convert.ToInt32(mbdata, 16), 2).PadLeft(8, '0');

                //BDS1 & BDS2
                string octet8 = Convert.ToString(Convert.ToInt32(paquete[pos + cont + 7], 16), 2).PadLeft(8, '0');

                this.BDS1[i] = octet8.Substring(0, 4);
                this.BDS2[i] = octet8.Substring(4, 4);

                cont = cont + 8;

                i++;
            }

            return cont;
        }

        public void ComputeACAS_RAreport(string octetos) // Data Item I021/260
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //llegim
            this.TYP = bits.Substring(0, 5);
            this.STYP = bits.Substring(5, 3);
            this.ARA = bits.Substring(8, 14);
            this.RAC = bits.Substring(22, 4);
            this.RAT = bits.Substring(26, 1);
            this.MTE = bits.Substring(27, 1);
            this.TTI = bits.Substring(28, 2);
            this.TID = bits.Substring(30, 26);
        }

        public int ComputeSurfaceCapabilitiesAndCharacteristics(string[] paquete, int pos) // Data Item I021/271
        {
            //contador d'octets
            int cont = 1;

            //octeto 1 -hexadecimal
            string octeto1 = paquete[pos];

            //octeto 1 -binari
            string bits1 = Convert.ToString(Convert.ToInt32(octeto1, 16), 2).PadLeft(8, '0');

            //llegim octeto 1 - Primary Subfield
            this.POA = bits1.Substring(2, 1);
            this.CDTI_S = bits1.Substring(3, 1);
            this.B2low = bits1.Substring(4, 1);
            this.RAS = bits1.Substring(5, 1);
            this.IDENT = bits1.Substring(6, 1);

            // First Extension
            if(bits1.Substring(7,1)=="1")
            {
                //octeto 2 -hexadecimal
                string octeto2 = paquete[pos + cont];

                //octeto 2 -binari
                string bits2 = Convert.ToString(Convert.ToInt32(octeto2, 16), 2).PadLeft(8, '0');

                //llegim octeto 2 - First Extension
                this.LmasW = Convert.ToInt32(bits2.Substring(0, 4), 2);
            }

            return cont;
        }

        public int ComputeDataAges(string[] paquete, int pos) // Data Item I021/295
        {
            //contem length Primary Subfield
            int longitud = 0;
            bool continua = true;
            while (longitud < 4 && continua == true)
            {
                string octeto = Convert.ToString(Convert.ToInt32(paquete[pos + longitud], 16), 2).PadLeft(8, '0');

                if (octeto.Substring(7, 1) == "1")
                    longitud++;
                else
                    continua = false;
            }

            // contador d'octetos:
            int cont = longitud;

            //llegim dades
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            if (octeto1.Substring(0, 1) == "1") // Subfield #1
            {
                this.AOS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(1, 1) == "1") // Subfield #2
            {
                this.TRD_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(2, 1) == "1") // Subfield #3
            {
                this.M3A_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(3, 1) == "1") // Subfield #4
            {
                this.QI_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(4, 1) == "1") // Subfield #5
            {
                this.TI_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(5, 1) == "1") // Subfield #6
            {
                this.MAM_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (octeto1.Substring(6, 1) == "1") // Subfield #7
            {
                this.GH_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
            }
            if (longitud >= 2)
            {
                string octeto2 = Convert.ToString(Convert.ToInt32(paquete[pos + 1], 16), 2).PadLeft(8, '0');
                if (octeto2.Substring(0, 1) == "1") // Subfield #8
                {
                    this.FL_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(1, 1) == "1") // Subfield #8
                {
                    this.ISA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(2, 1) == "1") // Subfield #9
                {
                    this.FSA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(3, 1) == "1") // Subfield #10
                {
                    this.AS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(4, 1) == "1") // Subfield #11
                {
                    this.TAS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(5, 1) == "1") // Subfield #12
                {
                    this.MH_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (octeto2.Substring(6, 1) == "1") // Subfield #13
                {
                    this.BVR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                }
                if (longitud >= 3)
                {
                    string octeto3 = Convert.ToString(Convert.ToInt32(paquete[pos + 2], 2), 16).PadLeft(8, '0');
                    if (octeto3.Substring(0, 1) == "1") // Subfield #14
                    {
                        this.GVR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(1, 1) == "1") // Subfield #15
                    {
                        this.GV_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(2, 1) == "1") // Subfield #16
                    {
                        this.TAR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(3, 1) == "1") // Subfield #17
                    {
                        this.TID_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(4, 1) == "1") // Subfield #18
                    {
                        this.TS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(5, 1) == "1") // Subfield #19
                    {
                        this.MET_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (octeto3.Substring(6, 1) == "1") // Subfield #20
                    {
                        this.ROA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                    }
                    if (longitud == 4)
                    {
                        string octeto4 = Convert.ToString(Convert.ToInt32(paquete[pos + 3], 16), 2).PadLeft(8, '0');
                        if (octeto4.Substring(0, 1) == "1") // Subfield #21
                        {
                            this.ARA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                            cont++;
                        }
                        if (octeto4.Substring(1, 1) == "1") // Subfield #22
                        {
                            this.SCC_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                            cont++;
                        }
                    }
                }
            }

            return cont;
        }

        public void ComputeReceiverID(string octeto) // Data Item I021/400
        {
            this.RID = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');
        }

        public double ComputeComplementoA2(string bits) //hace el complemento A2
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
