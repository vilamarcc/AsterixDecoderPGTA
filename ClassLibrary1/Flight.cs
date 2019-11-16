using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AsterixDecoder
{
    public class Flight
    {
        public int SIC, SAC;
        public int CAT;
        public string tracknumber;
        public string messagetype;
        public string callsign;
        public List<int> packages = new List<int>();
        public List<Double> Xs = new List<double>(), Ys = new List<double>();
        public List<Double> lats = new List<double>(), lngs = new List<double>();
        public List<String> fls = new List<string>();
        public List<Double> Vxs = new List<double>(), Vys = new List<double>();
        public List<String> TODs = new List<string>();


        public Flight(CAT20 listcat20, int i)
        {
            this.callsign = listcat20.callsign;
            this.CAT = 20;
            this.tracknumber = listcat20.TrackNum;
            this.messagetype = "MLAT";
            this.SIC = listcat20.SIC;
            this.SAC = listcat20.SAC;

            packages.Add(i);
            Xs.Add(listcat20.X);
            Ys.Add(listcat20.Y);
            fls.Add(listcat20.FL[2]);
            Vxs.Add(listcat20.Vx);
            Vys.Add(listcat20.Vy);
            TODs.Add(listcat20.TOD);
        }

        public void updateFlight(CAT20 listcat20, int i)
        {
            if(this.callsign == null && listcat20.callsign != null) { this.callsign = listcat20.callsign; }
            packages.Add(i);
            Xs.Add(listcat20.X);
            Ys.Add(listcat20.Y);
            fls.Add(listcat20.FL[2]);
            Vxs.Add(listcat20.Vx);
            Vys.Add(listcat20.Vy);
            TODs.Add(listcat20.TOD);
        }

        public Flight(CAT10 listcat10, int i)
        {
            this.callsign = listcat10.TargetID;
            this.CAT = 20;
            this.tracknumber = listcat10.TrackNumber;
            this.messagetype = listcat10.MessageType;
            this.SIC = Convert.ToInt32(listcat10.SICnum);
            this.SAC = Convert.ToInt32(listcat10.SACnum);

            packages.Add(i);
            Xs.Add(Convert.ToDouble(listcat10.Xcomponent));
            Ys.Add(Convert.ToDouble(listcat10.Ycomponent));
            fls.Add(listcat10.FlightLevel);
            Vxs.Add(Convert.ToDouble(listcat10.Vx));
            Vys.Add(Convert.ToDouble(listcat10.Vy));
            TODs.Add(listcat10.TimeOfDay);
        }

        public void updateFlight(CAT10 listcat10, int i)
        {
            if (this.callsign == null && listcat10.TargetID != null) { this.callsign = listcat10.TargetID; }
            packages.Add(i);
            Xs.Add(Convert.ToDouble(listcat10.Xcomponent));
            Ys.Add(Convert.ToDouble(listcat10.Ycomponent));
            fls.Add(listcat10.FlightLevel);
            Vxs.Add(Convert.ToDouble(listcat10.Vx));
            Vys.Add(Convert.ToDouble(listcat10.Vy));
            TODs.Add(listcat10.TimeOfDay);
        }

        public Flight(CAT21 listcat21, int i)
        {
            this.callsign = listcat21.TargetID;
            this.CAT = 20;
            this.tracknumber = Convert.ToString(listcat21.TrackNumber);
            this.messagetype = "ADS-B";
            this.SIC = Convert.ToInt32(listcat21.SIC);
            this.SAC = Convert.ToInt32(listcat21.SAC);

            packages.Add(i);
            lats.Add(listcat21.LatitudeWGS);
            lngs.Add(listcat21.LongitudeWGS);
            fls.Add(listcat21.FlightLevel);
            TODs.Add(listcat21.TOD);
        }

        public void updateFlight(CAT21 listcat21, int i)
        {
            if (this.callsign == null && listcat21.TargetID != null) { this.callsign = listcat21.TargetID; }
            packages.Add(i);
            lats.Add(listcat21.LatitudeWGS);
            lngs.Add(listcat21.LongitudeWGS);
            fls.Add(listcat21.FlightLevel);
            TODs.Add(listcat21.TOD);
        }
    }
}
