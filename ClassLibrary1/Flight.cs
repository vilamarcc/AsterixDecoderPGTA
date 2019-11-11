using System;
using System.Collections.Generic;
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
    }
}
