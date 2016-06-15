using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBuster.Data
{
    public static class CalcDistance
    {
        static public double findDistance(double t1, double n1, double t2, double n2)
        {

            var Rm = 3961; // mean radius of the earth (miles) at 39 degrees from the equator
            var Rk = 6373; // mean radius of the earth (km) at 39 degrees from the equator
            double lat1, lon1, lat2, lon2, dlat, dlon, a, c, dm, dk, mi, km;

            // get values for lat1, lon1, lat2, and lon2

            // convert coordinates to radians
            lat1 = deg2rad(t1);
            lon1 = deg2rad(n1);
            lat2 = deg2rad(t2);
            lon2 = deg2rad(n2);

            // find the differences between the coordinates
            dlat = lat2 - lat1;
            dlon = lon2 - lon1;

            // here's the heavy lifting
            a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
            c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); // great circle distance in radians

            dk = c * Rk; // great circle distance in km

            // round the results down to the nearest 1/1000
            km = round(dk);

            // display the result
            return km;
        }


        // convert degrees to radians
        static double deg2rad(double deg)
        {
            return deg * Math.PI / 180; // radians = degrees * pi/180
            
        }


        // round to the nearest 1/1000
        static double round(double x)
        {
            return Math.Round(x * 1000) / 1000;
        }
    }
}
