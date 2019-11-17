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

        public int version;

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

        public double ROT;
        public string TI;
        public string RateOfTurn;

        public string RP;

        public string ECAT;

        public string DTI;
        public string MDS;
        public string UAT;
        public string VDL;
        public string OTR;
        public string LinkTech;

        public string ATP;
        public string ARC;
        public string RC;
        public string RAB;
        public string DCR;
        public string GBS;
        public string SIM;
        public string TST;
        public string SAA;
        public string SPI;
        public string CL;
        public string LLC;
        public string IPC;
        public string NOGO;
        public string CPR;
        public string LDPJ;
        public string RCF;
        public string TargetReport;

        public double Velocityaccuracy;
        public string velAccuracy; 

        public string AC;
        public string MN;
        public string DC;
        public string FigureOfMerit;
        public string posAccuracy;

        public string Mode3ACode;

        public string TODaccuracy;

        public string TimeOfApplicabilityForPosition_;

        public string TimeOfApplicabilityForVelocity_;

        public string TimeOfMessageReceptionForPosition_;

        public string FSI_Position;
        public string TimeOfMessageReceptionForPosition_HighPrecision_;

        public string TimeOfMessageReceptionForVelocity_;

        public string FSI_Velocity;
        public string TimeOfMessageReceptionForVelocity_HighPrecision_;

        public string TimeOfAsterixReportTransmission_;

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
        public string QualityIndicators;

        public int NAV;
        public int NVB;
        public string[] TCA;
        public string[] NC;
        public int[] TCP;
        public double[] Altitude;
        public double[] Latitude;
        public double[] Longitude;
        public string[] PointType;
        public string[] TD;
        public string[] TRA;
        public string[] TOA;
        public int[] TOV;
        public double[] TTR;
        public string TrajectoryIntentData;

        public double LatitudeWGS;
        public double LongitudeWGS;
        public string positionWGS;

        public double LatitudeWGS_HR;
        public double LongitudeWGS_HR;
        public string HRpositionWGS;

        public double MAM;
        public string MSGampl;

        public double GH;
        public string GeometricHeight;

        public double FL;
        public string FlightLevel;

        public string SAS;
        public string Source;
        public double SelectedAltitude_I;
        public string SelectedAltitude_IS;

        public string MV;
        public string AH;
        public string AM;
        public double SelectedAltitude_F;
        public string SelectedAltitude_FS;

        public string IM;
        public double AirSpeedNum;
        public string AirSpeed;

        public string RE_TrueAirspeed;
        public double TAS;
        public string TrueAirspeed;

        public double MagneticHeadingNum;
        public string MagneticHeading;

        public string RE_BarometricVerticalRate;
        public double BarometricVerticalRateNum;
        public string BarometricVerticalRate;

        public string RE_GeometricVerticalRate;
        public double GeometricVerticalRateNum;
        public string GeometricVerticalRate;

        public string RE_AirborneGroundVector;
        public double GroundSpeed;
        public double TrackAngle;
        public string AirborneGroundVector;

        public string TrackNumber;

        public double TAR;
        public string TrackAngleRate;

        public string TargetID;

        public string ICF;
        public string LNAV;
        public string ME;
        public string PS;
        public string SS;
        public string TargetStatus;

        public string VNS;
        public string VN;
        public string LTT;
        public string MOPS;

        public int WindSpeed;
        public int WindDirection;
        public double Temperature;
        public int Turbulence;
        public string MetReport;

        public double RollAngleNum;
        public string RollAngle;

        public string[] MBdata;
        public string[] BDS1;
        public string[] BDS2;
        public string ModeS;

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
        public string ages;

        public string RID;

        public string TOD;

        public string targetreport;
        public string operationalstatus;
        public string figureofmerit;
        public string ages_;
        public string trajectoryintentdata;
        public string linktech;
        public string targetstatus;
        public string metreport;
        public string qualityindicators;
        public string modes;
        public string mopsv;

        // CONSTRUCTOR:
        public CAT21(string[] paquete, int vers) //decodifica el missatge (paquet)
        {
            this.version = vers;

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

            if (this.version == 23)
            {
                // Target Report Descriptor
                if (Convert.ToString(this.FSPEC[1]) == "1")
                {
                    this.ComputeTargetReportDescriptor23(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // Time Of Day
                if (Convert.ToString(this.FSPEC[2]) == "1")
                {
                    this.ComputeTimeOfDay(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Position in WGS-84
                if (Convert.ToString(this.FSPEC[3]) == "1")
                {
                    this.ComputePositionInWGS84(paquete[pos] + paquete[pos + 1] + paquete[pos + 2], paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5]);
                    pos = pos + 6;
                }

                // Target Address
                if (Convert.ToString(this.FSPEC[4]) == "1")
                {
                    this.ComputeTargetAddress(paquete[pos] + paquete[pos + 1] + paquete[pos + 2]);
                    pos = pos + 3;
                }

                // Geometric Altitude
                if (Convert.ToString(this.FSPEC[5]) == "1")
                {
                    this.ComputeGeometricHeight(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                // Figure of Merit
                if (Convert.ToString(this.FSPEC[6]) == "1")
                {
                    this.ComputeFigureOfMerit(paquete[pos] + paquete[pos + 1]);
                    pos = pos + 2;
                }

                if(longFSPEC>=2)
                {
                    // Link Technology
                    if (Convert.ToString(FSPEC[8]) == "1")
                    {
                        this.ComputeLinkTechnology(paquete[pos]);
                        pos++;
                    }

                    // Roll Angle
                    if (Convert.ToString(FSPEC[9]) == "1")
                    {
                        this.ComputeRollAngle(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Flight Level
                    if (Convert.ToString(FSPEC[10]) == "1")
                    {
                        this.ComputeFL(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Air Speed
                    if (Convert.ToString(FSPEC[11]) == "1")
                    {
                        this.ComputeAirSpeed(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // True Air Speed
                    if (Convert.ToString(FSPEC[12]) == "1")
                    {
                        this.ComputeTrueAirspeed(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Magnetic Heading
                    if (Convert.ToString(FSPEC[13]) == "1")
                    {
                        this.ComputeMagneticHeading(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    // Barometric Vertical Rate
                    if (Convert.ToString(FSPEC[14]) == "1")
                    {
                        this.ComputeBarometricVerticalRate(paquete[pos] + paquete[pos + 1]);
                        pos = pos + 2;
                    }

                    if(longFSPEC>=3)
                    {
                        // Geometric Vertical Rate
                        if (Convert.ToString(FSPEC[16]) == "1")
                        {
                            this.ComputeGeometricVerticalRate(paquete[pos] + paquete[pos + 1]);
                            pos = pos + 2;
                        }

                        // Ground Vector
                        if (Convert.ToString(FSPEC[17]) == "1")
                        {
                            this.ComputeAirborneGroundVector(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3]);
                            pos = pos + 4;
                        }

                        // Rate of Turn
                        if (Convert.ToString(FSPEC[18]) == "1")
                            pos = pos + this.ComputeRateOfAngle(paquete, pos);

                        // Target Identification
                        if (Convert.ToString(FSPEC[19]) == "1")
                        {
                            this.ComputeTargetIdentification(paquete[pos] + paquete[pos + 1] + paquete[pos + 2] + paquete[pos + 3] + paquete[pos + 4] + paquete[pos + 5]);
                            pos = pos + 6;
                        }

                        // Velocity Accuracy
                        if(Convert.ToString(FSPEC[20])=="1")
                        {
                            this.ComputeVelocityAccuracy(paquete[pos]);
                            pos++;
                        }

                        // Time of day accuracy
                        if (Convert.ToString(FSPEC[21]) == "1")
                        {
                            this.ComputeTODaccuracy(paquete[pos]);
                            pos++;
                        }

                        // Target Status
                        if (Convert.ToString(FSPEC[22]) == "1")
                        {
                            this.ComputeTargetStatus23(paquete[pos]);
                            pos++;
                        }

                        if (longFSPEC>=4)
                        {
                            // Emitter Category
                            if (Convert.ToString(FSPEC[24]) == "1")
                            {
                                this.ComputeEmitterCategory(paquete[pos]);
                                pos++;
                            }

                            // Met report
                            if (Convert.ToString(FSPEC[25]) == "1")
                                pos = pos + this.ComputeMetInformation(paquete, pos);

                            // Intermediate State Selected Altitude
                            if (Convert.ToString(FSPEC[26]) == "1")
                            {
                                this.ComputeSelectedAltitude(paquete[pos] + paquete[pos + 1]);
                                pos = pos + 2;
                            }

                            // Final State Selected Altitude
                            if (Convert.ToString(FSPEC[27]) == "1")
                            {
                                this.ComputeFinalStateSelectedAltitude(paquete[pos] + paquete[pos + 1]);
                                pos = pos + 2;
                            }

                            // Trajectory Intent
                            if(Convert.ToString(FSPEC[28])=="1")
                                pos = pos + this.ComputeTrajectoryIntend(paquete, pos);
                            
                            if (longFSPEC>=5)
                            {
                                // Reserved Expansion Field
                                // Special Purpose Field
                            }
                        }
                    }
                }
            }
            if (this.version == 24)
            {
                // Target Report Descriptor
                if (Convert.ToString(this.FSPEC[1]) == "1")
                    pos = pos + this.ComputeTargetReportDescriptor24(paquete, pos);

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
                    pos++;
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
                    if (Convert.ToString(this.FSPEC[13]) == "1")
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
                                this.ComputeTargetStatus24(paquete[pos]);
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

            if (this.TargetReport != null)
                this.targetreport = "Click to expand";
            if (this.OperationalStatus != null)
                this.operationalstatus = "Click to expand";
            if (this.FigureOfMerit != null)
                this.figureofmerit = "Click to expand";
            if (this.ages != null)
                this.ages_ = "Click to expand";
            if (this.TrajectoryIntentData != null)
                this.trajectoryintentdata = "Click to expand";
            if (this.LinkTech != null)
                this.linktech = "Click to expand";
            if (this.TargetStatus != null)
                this.targetstatus = "Click to expand";
            if (this.MetReport != null)
                this.metreport = "Click to expand";
            if (this.QualityIndicators != null)
                this.qualityindicators = "Click to expand";
            if (this.ModeS != null)
                this.modes = "Click to expand";
            if (this.MOPS != null)
                this.mopsv = "Click to expand";
        }

        // MÈTODES:

        public void ComputeFSPEC(string[] paquete)
        {
            //llegim octets fins que la última posició d'un d'ells sigui "0"
            int pos = 3;
            bool continua = true;
            while (continua==true)
            {
                //llegim nou octeto
                string newocteto = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

                //fiquem aquest octeto al nostre FSPEC
                this.FSPEC = this.FSPEC + newocteto;

                if (newocteto.Substring(7, 1) == "1") //seguim
                    pos++;
                else //sortim del bucle
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
            else
                this.DataSourceID = "SAC: " + this.SAC.ToString() + ", SIC: " + this.SIC.ToString();
        }

        public void ComputeFigureOfMerit(string octetos) // Data Item I021/090
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //separem
            string AC = bits.Substring(0, 2);
            if (AC == "00")
                this.AC = "ACAS operability: Unknown";
            if (AC == "01")
                this.AC = "ACAS not operational";
            if (AC == "10")
                this.AC = "ACAS operational";
            if (AC == "11")
                this.AC = "ACAS operability: Invalid";

            string MN = bits.Substring(2, 2);
            if (MN == "00")
                this.MN = "Multiple aids operating: unknown";
            if (MN == "01")
                this.MN = "Multiple navigational aids not operating";
            if (MN == "10")
                this.MN = "Multiple navigational aids operating";
            if (MN == "11")
                this.MN = "Multiple aids operating: invalid";

            string DC = bits.Substring(4, 2);
            if (DC == "00")
                this.DC = "Differential correction: unknown";
            if (DC == "01")
                this.DC = "Differential correction";
            if (DC == "10")
                this.DC = "No differential correction";
            if (DC == "11")
                this.DC = "Differential correction: invalid";

            this.FigureOfMerit = " - " + this.AC + "\n - " + this.MN + "\n - " + this.DC;

            int PA = Convert.ToInt32(bits.Substring(12, 4), 2);
            this.posAccuracy = Convert.ToString(PA);
        }

        public void ComputeServiceIdentification(string octeto) // Data Item I021/015
        {
            //passem a string de bits
            int servID = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);

            this.serviceID = Convert.ToString(servID);
        }

        public void ComputeTimeOfDay(string octetoTimeOfDay) // Data Item I021/030
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetoTimeOfDay, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            this.TOD = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputeServiceManagement(string octeto) // Data Item I021/016
        {
            //passem a int
            int rp_int=int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);

            //multipliquem per la resolució
            this.RP = Convert.ToString(rp_int * 0.5) + " s";
        }

        public void ComputeEmitterCategory(string octeto) // Data Item I021/020
        {
            //llegim el número
            int ecat = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);
            if (ecat == 1)
                this.ECAT = "Light aircraft";
            if (ecat == 3)
                this.ECAT = "Medium aircraft";
            if (ecat == 5)
                this.ECAT = "Heavy aircraft";
            if (ecat == 6)
                this.ECAT = "Highly manoeuvrable and high speed";
            if (ecat >= 7 && ecat <= 9)
                this.ECAT = "Reserved";
            if (ecat == 10)
                this.ECAT = "Rotocraft";
            if (ecat == 11)
                this.ECAT = "Glider/Sailplane";
            if (ecat == 12)
                this.ECAT = "Lighter than air aircraft";
            if (ecat == 13)
                this.ECAT = "Unmanned aerial vehicle";
            if (ecat == 14)
                this.ECAT = "Space/Transatmospheric vehicle";
            if (ecat == 15)
                this.ECAT = "Ultralight/Handglider/Paraglider";
            if (ecat == 16)
                this.ECAT = "Parachutis/Skydriver";
            if (ecat >= 17 && ecat <= 19)
                this.ECAT = "Reserved";
            if (ecat == 20)
                this.ECAT = "Surface emergency vehicle";
            if (ecat == 21)
                this.ECAT = "Surface service vehicle";
            if (ecat == 22)
                this.ECAT = "Fixed grounf or tethered obstruction";
            if (this.version == 24)
            {
                if (ecat == 0)
                    this.ECAT = "No information";
                if (ecat == 2)
                    this.ECAT = "Small aircraft";
                if (ecat == 4)
                    this.ECAT = "High Vortex Large";
                if (ecat == 23)
                    this.ECAT = "Cluster obstacle";
                if (ecat == 24)
                    this.ECAT = "Line obstacle";
            }
            if (this.version == 23 && (ecat == 2 || ecat == 4 || ecat==23 || ecat==24))
                this.ECAT = "Reserved";
        }

        public int ComputeRateOfAngle(string[] paquete, int pos) // Data Item I021/165
        {
            //octeto 1
            int cont = 1;
            string octeto1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            string TI = octeto1.Substring(0, 2);
            if (TI == "00")
                this.TI = "";
            if (TI == "01")
                this.TI = "to the left";
            if (TI == "10")
                this.TI = "to the rigth";
            if (TI == "11")
                this.TI = "straight";
            if(octeto1.Substring(7,1)=="1")
            {
                string octeto2= Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                cont++;
                this.ROT = (1 / 4) * this.ComputeComplementoA2(octeto2.Substring(0, 7));
                this.RateOfTurn = this.ROT.ToString() + " º/s " + this.TI;
            }
            return cont;
        }

        public void ComputeTargetReportDescriptor23(string octetos) // Data Item I021/040
        {
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            int DCR = int.Parse(bits.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
            if (DCR == 0)
                this.DCR = "No differential correction (ADS-B)";
            if (DCR == 1)
                this.DCR = "Differential correction (ADS-B)";

            int GBS = int.Parse(bits.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
            if (GBS == 0)
                this.GBS = "Ground Bit not set";
            if (GBS == 1)
                this.GBS = "Ground Bit set";

            int SIM = int.Parse(bits.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
            if (SIM == 0)
                this.SIM = "Actual target report";
            if (SIM == 1)
                this.SIM = "Simulated target report";

            int TST = int.Parse(bits.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
            if (TST == 0)
                this.TST = "Test Target: Default";
            if (TST == 1)
                this.TST = "Test Target";

            int RAB = int.Parse(bits.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
            if (RAB == 0)
                this.RAB = "Report from target transponder";
            if (RAB == 1)
                this.RAB = "Report from field monitor (fixed transponder)";

            int SAA = int.Parse(bits.Substring(5, 1), System.Globalization.NumberStyles.HexNumber);
            if (SAA == 0)
                this.SAA = "Equipment capable to provide Selected Altitude";
            if (SAA == 1)
                this.SAA = "Equipment not capable to provide Selected Altitude";

            int SPI = int.Parse(bits.Substring(6, 1), System.Globalization.NumberStyles.HexNumber);
            if (SPI == 0)
                this.SPI = "Absence of SPI (Special Position Identification)";
            if (SPI == 1)
                this.SPI = "Special Position Identification";

            int ATP = int.Parse(bits.Substring(8, 3), System.Globalization.NumberStyles.HexNumber);
            if (ATP == 0)
                this.ATP = "Non unique address";
            if (ATP == 1)
                this.ATP = "24-Bit ICAO address";
            if (ATP == 2)
                this.ATP = "Surface vehicle address";
            if (ATP == 3)
                this.ATP = "Anonymous address";
            if (ATP >= 4)
                this.ATP = "Address reserved for future use";

            int ARC = int.Parse(bits.Substring(11, 2), System.Globalization.NumberStyles.HexNumber);
            if (ARC == 1)
                this.ARC = "Altitude Reporting Capability: 25ft";
            if (ARC == 2)
                this.ARC = "Altitude Reporting Capability: 100ft";
            if (ARC == 0)
                this.ARC = "Altitude Reporting Capability: Unknown";

            this.TargetReport = " - " + this.DCR + "\n - " + this.GBS + "\n - " + this.SIM + "\n - " + this.TST + "\n - " + this.RAB + "\n - " + this.SAA + "\n - " + this.SPI + "\n - " + this.ATP + "\n - " + this.ARC;
        }

        public int ComputeTargetReportDescriptor24(string[] paquete, int pos) // Data Item I021/040
        {
            //contador de octetos
            int cont = 1;

            //llegim primer octet
            string octet1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');
            
            int ATP = int.Parse(octet1.Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
            if (ATP == 0)
                this.ATP = "24-Bit ICAO address";
            if (ATP == 1)
                this.ATP = "Duplicate address";
            if (ATP == 2)
                this.ATP = "Surface vehicle address";
            if (ATP == 3)
                this.ATP = "Anonymous address";
            if (ATP >= 4)
                this.ATP = "Address reserved for future use";

            int ARC = int.Parse(octet1.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            if (ARC == 0)
                this.ARC = "Altitude Reporting Capability: 25ft";
            if (ARC == 1)
                this.ARC = "Altitude Reporting Capability: 100ft";
            if (ARC == 2)
                this.ARC = "Altitude Reporting Capability: Unknown";
            if (ARC == 3)
                this.ARC = "Altitude Reporting Capability: Invalid";

            int RC = int.Parse(octet1.Substring(5, 1), System.Globalization.NumberStyles.HexNumber);
            if (RC == 0)
                this.RC = "Range Check: Default";
            if (RC == 1)
                this.RC = "Range Check passed, CPR Validation pending";

            int RAB = int.Parse(octet1.Substring(6, 1), System.Globalization.NumberStyles.HexNumber);
            if (RAB == 0)
                this.RAB = "Report from target transponder";
            if (RAB == 1)
                this.RAB = "Report from field monitor (fixed transponder)";

            this.TargetReport = " - " + this.ATP + "\n - " + this.ARC + "\n - " + this.RC + "\n - " + this.RAB;

            if (octet1.Substring(7,1) == "1") // First Extension
            {
                string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                
                int DCR = int.Parse(octet2.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                if (DCR == 0)
                    this.DCR = "No differential correction (ADS-B)";
                if (DCR == 1)
                    this.DCR = "Differential correction (ADS-B)";

                int GBS = int.Parse(octet2.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                if (GBS == 0)
                    this.GBS = "Ground Bit not set";
                if (GBS == 1)
                    this.GBS = "Ground Bit set";

                int SIM = int.Parse(octet2.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                if (SIM == 0)
                    this.SIM = "Actual target report";
                if (SIM == 1)
                    this.SIM = "Simulated target report";

                int TST = int.Parse(octet2.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                if (TST == 0)
                    this.TST = "Test Target: Default";
                if (TST == 1)
                    this.TST = "Test Target";

                int SAA = int.Parse(octet2.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
                if (SAA == 0)
                    this.SAA = "Equipment capable to provide Selected Altitude";
                if (SAA == 1)
                    this.SAA = "Equipment not capable to provide Selected Altitude";

                int CL = int.Parse(octet2.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                if (CL == 0)
                    this.CL = "Report valid";
                if (CL == 1)
                    this.CL = "Report suspect";
                if (CL == 2)
                    this.CL = "No information about Confidence Level";
                if (CL == 3)
                    this.CL = "Reserved for future use";

                cont++;

                this.TargetReport = this.TargetReport + "\n - " + this.DCR + "\n - " + this.GBS + "\n - " + this.SIM + "\n - " + this.TST + "\n - " + this.SAA + "\n - " + this.CL;

                if (octet2.Substring(7, 1) == "1") // Second Extension: Error Conditions
                {
                    string octet3 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');
                    
                    int LLC = int.Parse(octet3.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    if (LLC == 0)
                        this.LLC = "List Lookup Check: Default";
                    if (LLC == 1)
                        this.LLC = "List Lookup Check failed";

                    int IPC = int.Parse(octet3.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    if (IPC == 0)
                        this.IPC = "Independent Position Check: Default";
                    if (IPC == 1)
                        this.IPC = "Independent Position Check failed";

                    int NOGO = int.Parse(octet3.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                    if (NOGO == 0)
                        this.NOGO = "NOGO bit not set";
                    if (NOGO == 1)
                        this.NOGO = "NOGO bit set";

                    int CPR = int.Parse(octet3.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
                    if (CPR == 0)
                        this.CPR = "CPR Validation correct";
                    if (CPR == 1)
                        this.CPR = "CPR Validation failed";

                    int LDPJ = int.Parse(octet3.Substring(5, 1), System.Globalization.NumberStyles.HexNumber);
                    if (LDPJ == 0)
                        this.LDPJ = "LDPJ not detected";
                    if (LDPJ == 1)
                        this.LDPJ = "LDPJ detected";

                    int RCF = int.Parse(octet3.Substring(6, 1), System.Globalization.NumberStyles.HexNumber);
                    if (RCF == 0)
                        this.RCF = "Range Check: Default";
                    if (RCF == 1)
                        this.RCF = "Range Chack failed";

                    cont++;

                    this.TargetReport = this.TargetReport + "\n - " + this.LLC + "\n - " + this.IPC + "\n - " + this.NOGO + "\n - " + this.CPR + "\n - " + this.LDPJ + "\n - " + this.RCF;
                }
            }

            return cont;
        }

        public void ComputeMode3ACode(string octetos) // Data Item I021/070
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            this.Mode3ACode = bits.Substring(4, 12);
        }

        public void ComputeTimeOfApplicabilityForPosition(string octetos) // Data Item I021/071
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            this.TimeOfApplicabilityForPosition_ = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputeTimeOfApplicabilityForVelocity(string octetos) // Data Item I021/072
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            this.TimeOfApplicabilityForVelocity_ = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputeTimeOfMessageReceptionForPosition(string octetos) // Data Item I021/073
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            this.TimeOfMessageReceptionForPosition_ = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputeTimeOfMessageReceptionForPosition_HighPrecision(string octetos) // Data Item I021/074
        {
            //passem a string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(32, '0');

            //agafem FSI
            this.FSI_Position = octetos_bits.Substring(0, 2);

            //agafem els segons
            string time_bits = octetos_bits.Substring(2, 30);

            //calculem quin segon del dia és
            int seg = Convert.ToInt32(time_bits, 2);//resolució
            double segundos = seg * Math.Pow(2, -30);

            //afegim el FSI:
            if (this.FSI_Position == "10")
                segundos = segundos - 1;
            if (this.FSI_Position == "01")
                segundos = segundos + 1;

            double segundos_red = Math.Round(10000 * segundos) / 10000;

            this.TimeOfMessageReceptionForPosition_HighPrecision_ = segundos_red.ToString() + " s";
        }

        public void ComputeTimeOfMessageReceptionForVelocity(string octetos) // Data Item I021/075
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            this.TimeOfMessageReceptionForVelocity_ = time.ToString(@"hh\:mm\:ss\:fff");
        }

        public void ComputeTimeOfMessageReceptionForVelocity_HighPrecision(string octetos) // Data Item I021/076
        {
            //passem a string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(32, '0');

            //agafem FSI
            this.FSI_Velocity = octetos_bits.Substring(0, 2);

            //agafem els segons
            string time_bits = octetos_bits.Substring(2, 30);

            //calculem quin segon del dia és
            int seg = Convert.ToInt32(time_bits, 2);
            double segundos = seg * Math.Pow(2, -30);

            //afegim el FSI:
            if (this.FSI_Velocity == "10")
                segundos = segundos - 1;
            if (this.FSI_Velocity == "01")
                segundos = segundos + 1;

            double segundos_red = Math.Round(10000 * segundos) / 10000;

            this.TimeOfMessageReceptionForVelocity_HighPrecision_ = segundos_red.ToString() + " s";
        }

        public void ComputeTimeOfAsterixReportTransmission(string octetos) // Data Item I021/077
        {
            //calculem quin segon del dia és
            int seg = int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);
            double segundos = Convert.ToSingle(seg) / 128; //resolució

            TimeSpan time = TimeSpan.FromSeconds(segundos);

            this.TimeOfAsterixReportTransmission_ = time.ToString(@"hh\:mm\:ss\:fff");
            this.TOD = this.TimeOfAsterixReportTransmission_;
        }

        public void ComputeTargetAddress(string octetos) // Data Item I021/080
        {
            this.TargetAddress = octetos;
        }

        public int ComputeQualityIndicators(string[] paquete, int pos) // Data Item I021/090
        {
            //contador de octetos
            int cont = 1;

            //llegim primer octet: Primary Subfield
            string octet1 = Convert.ToString(Convert.ToInt32(paquete[pos], 16), 2).PadLeft(8, '0');

            this.NUCr_NACv = int.Parse(octet1.Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
            this.NUCp_NIC = int.Parse(octet1.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);

            this.QualityIndicators = " - Navigation Uncertainty Category for Velocity: " + this.NUCr_NACv.ToString() + "\n - Navigation Uncertainty category for Position: " + this.NUCp_NIC.ToString();

            if (octet1.Substring(7, 1) == "1") // First Extension: Navigation Accuracy Category for Position
            {
                string octet2 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                this.NICbaro = int.Parse(octet2.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                this.SIL = int.Parse(octet2.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                this.NACp = int.Parse(octet2.Substring(3, 4), System.Globalization.NumberStyles.HexNumber);

                this.QualityIndicators = this.QualityIndicators + "\n - Navigation Integrity Category for Barometric Altitude: " + this.NICbaro.ToString() + "\n - Surveillance (MOPS version 1) or Source (MOPS version 2) Integrity Level: " + this.SIL.ToString() + "\n - Navigation Accuracy Category for Position: " + this.NACp.ToString();

                cont++;

                if (octet2.Substring(7, 1) == "1") // Second Extension: Position Quality Indicators
                {
                    string octet3 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                    this.SILS = int.Parse(octet3.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    if (this.SILS == 0)
                        this.QualityIndicators = this.QualityIndicators + "\n - SIL-Supplement measured per flight-hour";
                    if (this.SILS == 1)
                        this.QualityIndicators = this.QualityIndicators + "\n - SIL-Supplement measured per sample";
                    this.SDA = int.Parse(octet3.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                    this.GVA = int.Parse(octet3.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                    this.QualityIndicators = this.QualityIndicators + "\n - Horizontal Position System Design Assurance Level: " + this.SDA.ToString() + "\n - Geometric Altitude Acuracy: " + this.GVA;

                    cont++;

                    if (octet3.Substring(7, 1) == "1") // Third Extension: Position Quality Indicators
                    {
                        string octet4 = Convert.ToString(Convert.ToInt32(paquete[pos + cont], 16), 2).PadLeft(8, '0');

                        this.PIC = int.Parse(octet4.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                        this.QualityIndicators = this.QualityIndicators + "\n - Position Integrity Category: " + this.PIC;

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
                this.TCP = new int[rep];
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
                    this.TrajectoryIntentData = "\n\tPOINT " + Convert.ToString(i + 1) + ":";

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
                    this.TCP[i] = Convert.ToInt32(octet2.Substring(2, 6), 2);
                    int pointtype = Convert.ToInt32(octet11.Substring(0, 4), 2);
                    if (pointtype ==0)
                        this.PointType[i] = "Unknown";
                    if (pointtype == 2)
                        this.PointType[i] = "Fly over waypoint";
                    if (pointtype ==1)
                        this.PointType[i] = "Fly by waypoint";
                    if (pointtype ==3)
                        this.PointType[i] = "Hold pattern";
                    if (pointtype ==4)
                        this.PointType[i] = "Procedure hold";
                    if (pointtype ==5)
                        this.PointType[i] = "Procedure hold";
                    if (pointtype ==6)
                        this.PointType[i] = "RF leg";
                    if (pointtype ==7)
                        this.PointType[i] = "Top of climb";
                    if (pointtype ==8)
                        this.PointType[i] = "Top of descent";
                    if (pointtype ==9)
                        this.PointType[i] = "Start of level";
                    if (pointtype ==10)
                        this.PointType[i] = "Cross-over altitude";
                    if (pointtype ==11)
                        this.PointType[i] = "Transition altitude";
                    if (pointtype ==11)
                        this.PointType[i] = "";
                    string td = octet11.Substring(4, 2);
                    if (td == "00")
                        this.TD[i] = "N/A";
                    if (td == "01")
                        this.TD[i] = "Turn right";
                    if (td == "10")
                        this.TD[i] = "Turn left";
                    if (td == "11")
                        this.TD[i] = "No turn";
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

                    if (TCA[i] == "0")
                        this.TrajectoryIntentData = this.TrajectoryIntentData + "\n - TCP: " + this.TCP[i].ToString();
                    this.TrajectoryIntentData = this.TrajectoryIntentData + "\n - Altitude: " + this.Altitude[i].ToString() + " ft\n - Latitude: " + this.Latitude[i].ToString() + "º\n - Longitude: " + this.Longitude[i].ToString() + "º\n - Point type: " + this.PointType[i] + "\n - TD: " + this.TD[i];
                    if (TOA[i] == "0")
                        this.TrajectoryIntentData = this.TrajectoryIntentData + "\n - Time Over Point: " + this.TOV[i] + " s";
                    if (this.TRA[i] == "1")
                        this.TrajectoryIntentData = this.TrajectoryIntentData + "\n - TCP Turn Radius: " + this.TTR[i] + " Nm";

                    i++;
                }

                cont = cont + 1 + (15 * rep);
            }

            return cont;
        }

        public void ComputePositionInWGS84(string OctLatWGS, string OctLonWGS) // Data Item I021/130
        {
            //passem a string de bits
            string lat = Convert.ToString(Convert.ToInt32(OctLatWGS, 16), 2).PadLeft(24, '0');
            string lon = Convert.ToString(Convert.ToInt32(OctLonWGS, 16), 2).PadLeft(24, '0');

            //fem el complement a2 que ens torna els bit en doubles, multipliquem per la resolució i arrodonim
            this.LatitudeWGS = this.ComputeComplementoA2(lat) * (180 / Math.Pow(2, 23));
            this.LongitudeWGS = this.ComputeComplementoA2(lon) * (180 / Math.Pow(2, 23));

            double lat_red= Math.Round(1000 * this.LatitudeWGS) / 1000;
            double lon_red = Math.Round(1000 * this.LongitudeWGS) / 1000;

            this.positionWGS = "[" + lat_red.ToString() + "º, " + lon_red.ToString() + "º]";
        }

        public void ComputeHighResolutionPositionInWGS84(string OctLatWGS, string OctLonWGS) // Data Item I021/131
        {
            //passem a string de bits
            string lat = Convert.ToString(Convert.ToInt32(OctLatWGS, 16), 2).PadLeft(32, '0');
            string lon = Convert.ToString(Convert.ToInt32(OctLonWGS, 16), 2).PadLeft(32, '0');

            //fem el complement a2 que ens torna els bit en doubles i multipliquem per la resolució
            this.LatitudeWGS_HR = Math.Round(1000 * this.ComputeComplementoA2(lat) * (180 / Math.Pow(2, 31))) / 1000;
            this.LongitudeWGS_HR = Math.Round(1000 * this.ComputeComplementoA2(lon) * (180 / Math.Pow(2, 31))) / 1000;

            this.HRpositionWGS = "[" + this.LatitudeWGS_HR.ToString() + "º, " + this.LongitudeWGS_HR.ToString() + "º]";
        }

        public void ComputeMessageAmplitude(string octeto) // Data Item I021/132
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //complement a2 + resolució
            this.MAM = 1 * this.ComputeComplementoA2(bits);

            this.MSGampl = this.MAM.ToString() + " dBm";
        }

        public void ComputeGeometricHeight(string octetos) // Data Item I021/140
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //complemento a2 + resolució
            this.GH = 6.25 * this.ComputeComplementoA2(bits);

            this.GeometricHeight = this.GH.ToString() + " ft";
        }

        public void ComputeFL(string octetos) // Data Item I021/145
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16,'0');

            //complemento a2 + resolució
            this.FL = 0.25 * this.ComputeComplementoA2(bits);

            this.FlightLevel = "FL" + this.FL.ToString();
        }

        public void ComputeSelectedAltitude(string octetos) // Data Item I021/146
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //separem SAS, Source i l'altitud en string de bits
            this.SAS = octetos_bits.Substring(0, 1);
            string s = octetos_bits.Substring(1, 2);
            if (s == "00")
                this.Source = "Unknown source";
            if (s == "01")
                this.Source = "Aircraft altitude";
            if (s == "10")
                this.Source = "MCP/FCU selected altitude";
            if (s == "11")
                this.Source = "FMS selected altitude";
            string altitude_bits = octetos_bits.Substring(3, 13);

            //complement a2 y resolució a l'altitud
            this.SelectedAltitude_I = 25 * this.ComputeComplementoA2(altitude_bits);

            this.SelectedAltitude_IS = this.Source + ": " + this.SelectedAltitude_I + " ft";
        }

        public void ComputeFinalStateSelectedAltitude(string octetos) // Data Item I021/148
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //separem 
            this.MV = octetos_bits.Substring(0, 1);
            this.AH = octetos_bits.Substring(1, 1);
            this.AM = octetos_bits.Substring(2, 1);
            string altitude_bits = octetos_bits.Substring(3, 13);

            //complement a2 y resolució
            this.SelectedAltitude_F = 25 * this.ComputeComplementoA2(altitude_bits);

            this.SelectedAltitude_FS = this.SelectedAltitude_F + " ft";
        }

        public void ComputeAirSpeed(string octetos) // Data Item I021/150
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //separem 
            this.IM = octetos_bits.Substring(0, 1);
            string airspeed_bits = octetos_bits.Substring(1, 15);

            //passem a int
            double airspeed_num = Convert.ToInt32(airspeed_bits, 2);

            //resolució
            if (this.IM == "0")
            { 
                this.AirSpeedNum = Math.Pow(2, -14) * airspeed_num;

                this.AirSpeed = "IAS: " + this.AirSpeedNum + " NM/s";
            }
            if (this.IM == "1")
            {
                this.AirSpeedNum = 0.001 * airspeed_num;

                this.AirSpeed = "Mach: " + this.AirSpeedNum;
            }
        }

        public void ComputeTrueAirspeed(string octetos) // Data Item I021/151
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            string airspeed_bits = "0";
            if (this.version == 24)
            {
                //separem 
                this.RE_TrueAirspeed = octetos_bits.Substring(0, 1);
                airspeed_bits = octetos_bits.Substring(1, 15);
            }
            if (this.version == 23)
                airspeed_bits = octetos_bits.Substring(0, 16);

            //passem a int i resolució
            this.TAS = Convert.ToInt32(airspeed_bits, 2);

            this.TrueAirspeed = this.TAS + " knots";
        }

        public void ComputeMagneticHeading(string octetos) // Data Item I021/152
        {
            //passem a int i multipliquem per la resolució
            this.MagneticHeadingNum = (360 / Math.Pow(2, 16)) * int.Parse(octetos, System.Globalization.NumberStyles.HexNumber);

            this.MagneticHeading = this.MagneticHeadingNum + "º";
        }

        public void ComputeLinkTechnology(string octeto) // Data Item I021/210
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //separem
            string dti = bits.Substring(3, 1);
            if (dti == "0")
                this.DTI = "Cockpit Display of Traffic Information: unknown";
            if (dti == "1")
                this.DTI = "Cockpit Display of Traffic Information: aircraft equiped with CDTI";

            string mds = bits.Substring(4, 1);
            if (mds == "0")
                this.MDS = "Mode-S Extended Squitter: not used";
            if (mds == "1")
                this.MDS = "Mode-S Extended Squitter: used";

            string uat = bits.Substring(5, 1);
            if (uat == "0")
                this.UAT = "UAT: not used";
            if (uat == "1")
                this.UAT = "UAT: used";

            string vdl = bits.Substring(6, 1);
            if (vdl == "0")
                this.VDL = "VDL Mode 4: not used";
            if (vdl == "1")
                this.VDL = "VDL Mode 4: used";

            string otr = bits.Substring(7, 1);
            if (otr == "0")
                this.OTR = "Other Technology: not used";
            if (otr == "1")
                this.OTR = "Other Technology: used";

            this.LinkTech = " - " + this.DTI + "\n - " + this.MDS + "\n - " + this.UAT + "\n - " + this.VDL + "\n - " + this.OTR;
        }

        public void ComputeBarometricVerticalRate(string octetos) // Data Item I021/155
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            string BarometricVR_bits = "0";
            if (this.version == 24)
            { 
                this.RE_BarometricVerticalRate = octetos_bits.Substring(0, 1);
                BarometricVR_bits = octetos_bits.Substring(1, 15);
            }
            if(this.version==23)
                BarometricVR_bits = octetos_bits.Substring(0, 16);

            //passem a int i resolució
            this.BarometricVerticalRateNum = 6.25 * this.ComputeComplementoA2(BarometricVR_bits);

            this.BarometricVerticalRate = this.BarometricVerticalRateNum + " feet/minute";
        }

        public void ComputeGeometricVerticalRate(string octetos) // Data Item I021/157
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            string GeometricVR_bits = "0";
            if (this.version==23)
                GeometricVR_bits = octetos_bits.Substring(0, 16);
            if (this.version == 24)
            {
                this.RE_GeometricVerticalRate = octetos_bits.Substring(0, 1);
                GeometricVR_bits = octetos_bits.Substring(1, 15);
            }

            //complement a2 i resolució
            this.GeometricVerticalRateNum = 6.25 * this.ComputeComplementoA2(GeometricVR_bits);

            this.GeometricVerticalRate = this.GeometricVerticalRateNum + " feet/minute";
        }

        public void ComputeAirborneGroundVector(string octetos) // Data Item I021/160
        {
            //string de bits
            string octetos_bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(32, '0');

            //separem
            if (this.version == 24)
            {
                this.RE_AirborneGroundVector = octetos_bits.Substring(0, 1);
                this.GroundSpeed = Math.Pow(2, -14) * Convert.ToInt32(octetos_bits.Substring(1, 15), 2);
                this.TrackAngle = (360 / Math.Pow(2, 16)) * Convert.ToInt32(octetos_bits.Substring(16, 16), 2);
            }
            if(this.version==23)
            {
                this.GroundSpeed = Math.Pow(2, -14) * this.ComputeComplementoA2(octetos_bits.Substring(0, 16));
                this.TrackAngle = (360 / Math.Pow(2, 16)) * Convert.ToInt32(octetos_bits.Substring(16, 16), 2);
            }

            double gs_red = Math.Round(1000000 * this.GroundSpeed) / 1000000;
            double ta_red = Math.Round(1000 * this.TrackAngle) / 1000;

            string gs = Convert.ToString(gs_red);
            string ta = Convert.ToString(ta_red);

            this.AirborneGroundVector = "[" + gs + " NM/s, " + ta + "º]";
        }

        public void ComputeTrackNumber(string octetos) // Data Item I021/161
        {
            //passem a binari
            string tracknum_bin = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(16, '0');

            //tallem els 4 primers bits
            string tracknum_valid = tracknum_bin.Substring(4, 12);

            //passem a int
            this.TrackNumber = Convert.ToString(Convert.ToInt32(tracknum_valid, 2));
        }

        public void ComputeTrackAngleRate(string octetos) // Data Item I021/165
        {
            //passem a binari
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //tallem els 6 primers bits
            string bits_valid = bits.Substring(6, 10);

            //complement a2 i resolució
            this.TAR = (1 / 32) * this.ComputeComplementoA2(bits_valid);

            this.TrackAngleRate = this.TAR.ToString() + " º/s";
        }

        public void ComputeTargetIdentification(string octetos) // Data Item I021/170
        {
            //string de bits
            string targetID = Convert.ToString(Convert.ToInt64(octetos, 16), 2).PadLeft(48, '0');

            //llegim
            int nchar = 0;
            while (nchar < targetID.Length)
            {
                string character = targetID.Substring(nchar, 6);

                this.TargetID = this.TargetID + this.GetCode(character);

                nchar = nchar + 6;
            }
        }

        public string GetCode(string character_string) // decodifica els caràcters del target ID (Callsign)
        {
            //número que representa el string
            int character_int = Convert.ToInt32(character_string, 2);

            //lista per decodificar 
            List<string> code = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            if (character_int == 0)
                return "";
            else
                return code[character_int - 1];
        }

        public void ComputeTargetStatus24(string octeto) // Data Item I021/200
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            //llegim
            string ICF = bits.Substring(0, 1);
            if (ICF == "0")
                this.ICF = "Intent Change Flag: no intent active";
            if (ICF == "1")
                this.ICF = "Intent Change Flag raised";

            string LNAV = bits.Substring(1, 1);
            if (LNAV == "0")
                this.LNAV = "LNAV Mode engaged";
            if (LNAV == "1")
                this.LNAV = "LNAV Mode not engaged";

            string ME = bits.Substring(2, 1);
            if (ME == "0")
                this.ME = "No military emergency";
            if (ME == "1")
                this.ME = "Military emergency";

            int PS = Convert.ToInt32(bits.Substring(3, 3), 2);
            if (PS == 0)
                this.PS = "No emergency/not reported emergency";
            if (PS == 1)
                this.PS = "General emergency";
            if (PS == 2)
                this.PS = "Lifeguard/medical emergency";
            if (PS == 3)
                this.PS = "Minimum fuel";
            if (PS == 4)
                this.PS = "No communications";
            if (PS == 5)
                this.PS = "Unlawful interference";
            if (PS == 6)
                this.PS = "'Downed' aircraft";

            int SS = Convert.ToInt32(bits.Substring(6, 2), 2);
            if (SS == 0)
                this.SS = "Surveillance Status: No condition reported";
            if (SS == 1)
                this.SS = "Surveillance Status: Permanent Alert (Emergency condition)";
            if (SS == 2)
                this.SS = "Surveillance Status: Temporary Alert (change in Mode 3/A Code other than emergency)";
            if (SS == 3)
                this.SS = "Surveillance Status: SPI set";

            this.TargetStatus = " - " + this.ICF + "\n - " + this.LNAV + "\n - " + this.ME + "\n - " + this.PS + "\n - " + this.SS;
        }

        public void ComputeTargetStatus23(string octeto) // Data Item I021/200
        {
            int targetstatus = int.Parse(octeto, System.Globalization.NumberStyles.HexNumber);
            if (targetstatus == 0)
                this.TargetStatus = "No emergency/no reported emergency";
            if (targetstatus == 1)
                this.TargetStatus = "General emergency";
            if (targetstatus == 2)
                this.TargetStatus = "Lifeguard/medical";
            if (targetstatus == 3)
                this.TargetStatus = "Minimum fuel";
            if (targetstatus == 4)
                this.TargetStatus = "No communications";
            if (targetstatus == 5)
                this.TargetStatus = "Unlawful interference";
        }

        public void ComputeVelocityAccuracy(string octeto) // Data Item I021/095
        {
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            this.velAccuracy = Convert.ToString(Convert.ToInt32(bits, 2));
        }

        public void ComputeTODaccuracy(string octeto) // Data Item I021/032
        {
            string bits = Convert.ToString(Convert.ToInt32(octeto, 16), 2).PadLeft(8, '0');

            double todacc = (1 / 128) * Convert.ToInt32(bits, 2);

            this.TODaccuracy = todacc.ToString() + " s";
        }

        public void ComputeMOPSversion(string octetos) // Data Item I021/210
        {
            //string de bits
            string bits = Convert.ToString(Convert.ToInt32(octetos, 16), 2).PadLeft(8, '0');

            //llegim
            string VNS = bits.Substring(1, 1);
            if (VNS == "0")
                this.VNS = "The MOPS Version is supported by the GS";
            if (VNS == "1")
                this.VNS = "The MOPS Version is not supported by the GS";
            int VN = int.Parse(bits.Substring(2, 3), System.Globalization.NumberStyles.HexNumber);
            int LTT = int.Parse(bits.Substring(5, 3), System.Globalization.NumberStyles.HexNumber);
            if (LTT == 0)
                this.LTT = "Link Technology Type: other";
            if (LTT == 1)
                this.LTT = "Link Technology Type: UAT";
            if (LTT == 2)
                this.LTT = "Link Technology Type: 1090 ES";
            if (LTT == 3)
                this.LTT = "Link Technology Type: VDL 4";
            if (LTT >= 4)
                this.LTT = "Link Technology Type: not assigned";
            if (LTT == 2)
            {
                if (VN == 0)
                    this.VN = "ED102/DO-260";
                if (VN == 1)
                    this.VN = "DO-260A";
                if (VN == 2)
                    this.VN = "ED102A/DO-260B";
            }
            else
                this.VN = Convert.ToString(VN);

            this.MOPS = " - " + this.VNS + "\n - " + this.VN + "\n - " + this.LTT;
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
                string octwindspeed = paquete[pos + cont] + paquete[pos + cont + 1];
                this.WindSpeed = int.Parse(octwindspeed, System.Globalization.NumberStyles.HexNumber);
                cont = cont + 2;
                this.MetReport = " - Wind Speed: " + this.WindSpeed.ToString() + " knots";
            }
            if(bits.Substring(1, 1) == "1")
            {
                string octwinddirect = paquete[pos + cont] + paquete[pos + cont + 1];
                this.WindDirection = int.Parse(octwinddirect, System.Globalization.NumberStyles.HexNumber);
                cont = cont + 2;
                this.MetReport = this.MetReport + " - Wind Direction: " + this.WindDirection + "º";
            }
            if(bits.Substring(2, 1) == "1")
            {
                string octetoTMP = paquete[pos + cont] + paquete[pos + cont + 1];
                string bitsTMP = Convert.ToString(Convert.ToInt32(octetoTMP, 16), 2);
                this.Temperature = 0.25 * this.ComputeComplementoA2(bitsTMP);
                cont = cont + 2;
                this.MetReport = this.MetReport + " - Temperature: " + this.Temperature + "ºC";
            }
            if(bits.Substring(3, 1) == "1")
            {
                this.Turbulence = int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.MetReport = this.MetReport + " - Turbulence: " + this.Turbulence;
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
            this.RollAngleNum = rollangle * 0.01;

            this.RollAngle = this.RollAngleNum.ToString() + "º";
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

                this.MBdata[i] = Convert.ToString(Convert.ToInt32(mbdata, 2), 16).PadLeft(8, '0');

                //BDS1 & BDS2
                string octet8 = Convert.ToString(Convert.ToInt32(paquete[pos + cont + 7], 16), 2).PadLeft(8, '0');

                this.BDS1[i] = octet8.Substring(0, 4);
                this.BDS2[i] = octet8.Substring(4, 4);

                this.ModeS = "\nMessage: " + this.MBdata[i] + ", Address 1: " + this.BDS1[i] + ", Address 2: " + this.BDS2[i];

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

                longitud++;
                
                if (octeto.Substring(7, 1) != "1")
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
                this.ages = " - Aircraft Operational Status: " + this.AOS_age;
            }
            if (octeto1.Substring(1, 1) == "1") // Subfield #2
            {
                this.TRD_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Target Report Descriptor: " + this.TRD_age;
            }
            if (octeto1.Substring(2, 1) == "1") // Subfield #3
            {
                this.M3A_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Mode 3/A Code: " + this.M3A_age;
            }
            if (octeto1.Substring(3, 1) == "1") // Subfield #4
            {
                this.QI_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Quality Indicators: " + this.QI_age;
            }
            if (octeto1.Substring(4, 1) == "1") // Subfield #5
            {
                this.TI_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Trajectory Intent: " + this.TI_age;
            }
            if (octeto1.Substring(5, 1) == "1") // Subfield #6
            {
                this.MAM_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Message Amplitude: " + this.MAM_age;
            }
            if (octeto1.Substring(6, 1) == "1") // Subfield #7
            {
                this.GH_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                cont++;
                this.ages = this.ages + "\n - Geometric Height: " + this.GH_age;
            }
            if (longitud >= 2)
            {
                string octeto2 = Convert.ToString(Convert.ToInt32(paquete[pos + 1], 16), 2).PadLeft(8, '0');
                if (octeto2.Substring(0, 1) == "1") // Subfield #8
                {
                    this.FL_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Flight Level: " + this.FL_age;
                }
                if (octeto2.Substring(1, 1) == "1") // Subfield #8
                {
                    this.ISA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Intermediate State Selected Altitude: " + this.ISA_age;
                }
                if (octeto2.Substring(2, 1) == "1") // Subfield #9
                {
                    this.FSA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Final State Selected Altitude: " + this.FSA_age;
                }
                if (octeto2.Substring(3, 1) == "1") // Subfield #10
                {
                    this.AS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Air Speed: " + this.AS_age;
                }
                if (octeto2.Substring(4, 1) == "1") // Subfield #11
                {
                    this.TAS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - True Air Speed: " + this.TAS_age;
                }
                if (octeto2.Substring(5, 1) == "1") // Subfield #12
                {
                    this.MH_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Magnetic Heading: " + this.MH_age;
                }
                if (octeto2.Substring(6, 1) == "1") // Subfield #13
                {
                    this.BVR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                    cont++;
                    this.ages = this.ages + "\n - Barometric Vertical Rate: " + this.BVR_age;
                }
                if (longitud >= 3)
                {
                    string octeto3 = Convert.ToString(Convert.ToInt32(paquete[pos + 2], 16), 2).PadLeft(8, '0');
                    if (octeto3.Substring(0, 1) == "1") // Subfield #14
                    {
                        this.GVR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Geometric Vertical Rate: " + this.GVR_age;
                    }
                    if (octeto3.Substring(1, 1) == "1") // Subfield #15
                    {
                        this.GV_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Ground Vector: " + this.GV_age;
                    }
                    if (octeto3.Substring(2, 1) == "1") // Subfield #16
                    {
                        this.TAR_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Track Angle Rate:" + this.TAR_age;
                    }
                    if (octeto3.Substring(3, 1) == "1") // Subfield #17
                    {
                        this.TID_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Target Identification: " + this.TID_age;
                    }
                    if (octeto3.Substring(4, 1) == "1") // Subfield #18
                    {
                        this.TS_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Target Status: " + this.TS_age;
                    }
                    if (octeto3.Substring(5, 1) == "1") // Subfield #19
                    {
                        this.MET_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Met Information: " + this.MET_age;
                    }
                    if (octeto3.Substring(6, 1) == "1") // Subfield #20
                    {
                        this.ROA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                        cont++;
                        this.ages = this.ages + "\n - Roll Angle: " + this.ROA_age;
                    }
                    if (longitud == 4)
                    {
                        string octeto4 = Convert.ToString(Convert.ToInt32(paquete[pos + 3], 16), 2).PadLeft(8, '0');
                        if (octeto4.Substring(0, 1) == "1") // Subfield #21
                        {
                            this.ARA_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                            cont++;
                            this.ages = this.ages + "\n - ACAS Resolution Advisory: " + this.ARA_age;
                        }
                        if (octeto4.Substring(1, 1) == "1") // Subfield #22
                        {
                            this.SCC_age = 0.1 * int.Parse(paquete[pos + cont], System.Globalization.NumberStyles.HexNumber);
                            cont++;
                            this.ages = this.ages + "\n - Surface Capabilities and Characteristics: " + this.SCC_age;
                        }
                    }
                }
            }

            return cont;
        }

        public void ComputeReceiverID(string octeto) // Data Item I021/400
        {
            this.RID = Convert.ToString(Convert.ToInt32(octeto, 16));
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
                {
                    int num = Convert.ToInt32(bits, 2);
                    return Convert.ToSingle(num);
                }
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
                    double num = Convert.ToInt32(newbits, 2);

                    return -(num + 1);
                }
            }
        }
    }
}
