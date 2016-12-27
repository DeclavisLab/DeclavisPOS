using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassPLU
{
    public class DIV
    {
        double Steuer;
        public double Price;
        public string Bez;
        int id;
        public bool Food;
        //public static List<DIV> ListofDIV = new List<DIV>();
        public static int ids = 0;
        public DIV(int _id, double _P,double _S,string _Bez,bool typ)
        {
            id = _id;
            Steuer = _S;
            Bez = _Bez;
            Price = _P;
            Food = typ;
        }
        public override string ToString()
        {
            string ret = id + "|" + Price + "|" + Steuer + "|" + Bez + "|" + Food.ToString();
            return ret;
        }
    }
}
