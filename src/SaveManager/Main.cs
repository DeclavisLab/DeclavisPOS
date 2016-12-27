using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SaveManager
{
    public class Main
    {

        public static string constr = LoadManager.Main.constr;
        public static void ADDZAB(double KücheBR,double ThekeBR,double TotalBR,double TotalNe,double TotalMwst,double Storno)
        {
            try
            {
                OleDbConnection con = new OleDbConnection(constr);
                con.Open();
                DateTime today = DateTime.Now;
                string strSQL = "INSERT INTO ZAB (`Datum`, `Küche Brutto`, `Theke Brutto`, `Total Brutto`, `Total Netto`, `Total Mwst 19%`, `Total Storno`) VALUES ('" + today.ToString("dd.MM.yyyy HH:mm:ss").ToString() + "','" + KücheBR + "','" + ThekeBR + "','" + TotalBR + "','" + TotalNe + "','" + TotalMwst + "','" + Storno + "');";

                OleDbCommand cmd = new OleDbCommand(strSQL, con);
                int time = cmd.ExecuteNonQuery();
                con.Close();
                
                //Reset Rechnung Nummern
                con.Open();
                string SQL = "UPDATE System SET `Value` = '0' WHERE `Systemvalue` LIKE 'Rnummer'";
                cmd = new OleDbCommand(SQL, con);
                time = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public static string ZABNO()
        {
            try
            {
                OleDbConnection con = new OleDbConnection(constr);
                int nowvalue = 0;
                con.Open();
                string strSQL = "SELECT * FROM System WHERE `Systemvalue` LIKE 'Znummer'";
                OleDbCommand cmd = new OleDbCommand(strSQL, con);
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nowvalue = Convert.ToInt32(dr[1].ToString());                    
                }
                dr.Close();
                con.Close();

                nowvalue++;

                con.Open();
                string SQL = "UPDATE System SET `Value` = '" + nowvalue.ToString() + "' WHERE `Systemvalue` LIKE 'Znummer'";
                cmd = new OleDbCommand(SQL, con);
                int time = cmd.ExecuteNonQuery();
                con.Close();

                return nowvalue.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return "Error: ZABNO()";
            }
        }
        public static string RBONO()
        { //Rechnungs - Bon Nummer
            try
            {
                OleDbConnection con = new OleDbConnection(constr);
                int nowvalue = 0;
                con.Open();
                string strSQL = "SELECT * FROM System WHERE `Systemvalue` LIKE 'Rnummer'";
                OleDbCommand cmd = new OleDbCommand(strSQL, con);
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nowvalue = Convert.ToInt32(dr[1].ToString());
                }
                dr.Close();
                con.Close();

                nowvalue++;

                con.Open();
                string SQL = "UPDATE System SET `Value` = '" + nowvalue.ToString() + "' WHERE `Systemvalue` LIKE 'Rnummer'";
                cmd = new OleDbCommand(SQL, con);
                int time = cmd.ExecuteNonQuery();
                con.Close();

                return nowvalue.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return "Error: RBONO()";
            }
        }
        public static string SystemValues(string SystemValue)
        {
            try
            {
                string ret = "";
                OleDbConnection con = new OleDbConnection(constr);
                con.Open();
                string strSQL = "SELECT * FROM System WHERE `Systemvalue` LIKE '"+SystemValue+"'";
                OleDbCommand cmd = new OleDbCommand(strSQL, con);
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ret = dr[1].ToString();
                }
                dr.Close();
                con.Close();               
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return "Error: SystemValues()";
            }
        }
        
    }
}
