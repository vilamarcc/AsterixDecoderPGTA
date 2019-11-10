using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixDecoder
{
    class Flight
    {
        public int SIC, SAC;
        public int CAT;
        public string callsign;
        public List<int> packages = new List<int>();
        public List<Double> Xs, Ys = new List<double>();
        public List<Double> lats, lngs = new List<double>();
        public List<String> fls = new List<string>();
        public List<Double> Vxs, Vys = new List<double>();
        public List<String> TODs = new List<string>();


        public Flight(List<CAT20> listcat20, string id)
        {
            this.callsign = id;
            this.CAT = 20;

            for (int i = 0; i < listcat20.Count; i++)
            {
                if(listcat20[i].callsign == id)
                {
                    packages.Add(i);
                    Xs.Add(listcat20[i].X);
                    Ys.Add(listcat20[i].Y);
                    fls.Add(listcat20[i].FL[1]);
                    Vxs.Add(listcat20[i].Vx);
                    Vys.Add(listcat20[i].Vy);
                    TODs.Add(listcat20[i].TOD);
                }
            }
        }
    }
}
