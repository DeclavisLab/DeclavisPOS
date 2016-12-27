using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace ClassPLU
{
    public class PLU
    {
        public static string constr = LoadManager.Main.constr;
        public double Price;
        public string BezM;
        public string BezB;
        public int ID;
        public double Steuer;
        public int Wg;
        public bool Food;
        public string Einheit;

        public static List<PLU> ListofPLUs = new List<PLU>();

        public PLU(int _ID, double _PR, string _Bez, double _Steuer, int _WG, bool _ka, string _Ein)
        {
            constr = LoadManager.Main.constr;
            ID = _ID;
            Price = _PR;
            BezM = _Bez;
            Steuer = _Steuer;
            Wg = _WG;
            Food = _ka;
            Einheit = _Ein;
            BezB = BezM + " " + _Ein;
        }

        public static PLU PLULoad(int _ID)
        {
            foreach (PLU PL in ListofPLUs)
            {
                if (PL.ID == _ID)
                {
                    return PL;
                }
            }
            return new PLU(0,0,"Error",0,0,false,"");
        }
        public static string PreisUmform(string Preis)
        {
            if (Preis.Contains(',') == true)
            {
                string[] Preiss = Preis.Split(new char[] { ',' });
                if (Preiss[1].Length == 1)
                    return Preis + "0 €";
                else
                    return Preis + " €";
            }
            else
                return Preis + ",00 €";
        }
        public static double PreisUmform2(string Preis)
        {
            double ret = 0;
            if (Preis.Contains(',') == true)
            {
                string[] Prices = Preis.Split(new char[] { ',' });
                int a = Convert.ToInt32(Prices[0]);
                float c = (float)a;
                int b = Convert.ToInt32(Prices[1]);
                float d = (float)b;
                if (Prices[1].Length == 1)
                    c += (d / 10);
                else
                    c += (d / 100);

                ret += c;
            }
            else
            {
                int M = Convert.ToInt32(Preis);
                ret += (float)M;
            }
            ret = Math.Round(ret, 2);
            return ret;
        }
        public static string PreisUmform3(double price)
        {
            price = Math.Round(price, 2);
            string Preis = Convert.ToString(price);
            if (Preis.Contains(',') == true)
            {
                string[] Preiss = Preis.Split(new char[] { ',' });
                if (Preiss[1].Length == 1)
                    return Preis + "0";
                else
                    return Preis + "";
            }
            else
                return Preis + ",00";
        }
        public static PLU PLULoad(string _Bez,bool both)
        {
            foreach (PLU PL in ListofPLUs)
            {
                if (both)
                {
                    if (PL.BezB == _Bez)
                    { return PL; }
                }
                else
                {
                    if (PL.BezM == _Bez)
                    {
                        return PL;
                    }
                }
            }
            return new PLU(0, 0, "Error", 0, 0, false,"-");
        }

        
        public static void PLUDATALoad()
        {
            ListofPLUs.Clear();
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();

            string strSQL = "SELECT * FROM PLU";

            OleDbCommand cmd = new OleDbCommand(strSQL, con);
            OleDbDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int ID = Convert.ToInt32(dr[1].ToString());
                double PR = Convert.ToDouble(dr[3].ToString());
                string Name = dr[2].ToString();
                double steuer = Convert.ToDouble(dr[4].ToString());
                int wg = Convert.ToInt32(dr[5].ToString());
                bool kate = Convert.ToBoolean(dr[6].ToString());
                string ein = dr[7].ToString();
                ListofPLUs.Add(new PLU(ID, PR, Name, steuer, wg, kate,ein));
            }
            dr.Close();
            con.Close();
        }

        public static void PLUDelete(string id)
        {            
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();
         
            string strSQL = "DELETE FROM PLU WHERE `ArticleID` Like '" +id+ "'";

            OleDbCommand cmd = new OleDbCommand(strSQL, con);
            try { cmd.ExecuteNonQuery(); }
            catch (OleDbException e) { e.ToString(); }
            //cmd.ExecuteNonQuery();*/
            con.Close();           
        }

        public static void PLUUpdate(PLU neu,string idold)
        {           
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();

            string strSQL = "UPDATE PLU SET `ArticleID` = '"+neu.ID+"', `Name` = '"+neu.BezM+"', `Preis` = '"+neu.Price+"', `Vat` = '"+neu.Steuer+"', `Warengruppe` = '"+neu.Wg+"', `Food` = "+neu.Food+", `Einheit` = '"+neu.Einheit+"' WHERE `ArticleID` Like '" + idold + "'";

            OleDbCommand cmd = new OleDbCommand(strSQL, con);
            try { cmd.ExecuteNonQuery(); }
            catch (OleDbException e) { e.ToString(); }
            con.Close();
        }

        public static void PLUInsert(PLU neu)
        {
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();

            string strSQL = "INSERT INTO PLU (`ArticleID`, `Name`, `Preis`, `Vat`, `Warengruppe`, `Food`, `Einheit`) VALUES ('"+neu.ID+"','"+neu.BezM+"','"+neu.Price.ToString()+"','"+neu.Steuer+"','"+neu.Wg+"',"+neu.Food.ToString()+",'"+neu.Einheit+"');";

            OleDbCommand cmd = new OleDbCommand(strSQL, con);
            try { cmd.ExecuteNonQuery(); }
            catch (OleDbException e) { e.ToString(); }
            //cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
