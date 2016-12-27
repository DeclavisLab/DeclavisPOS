using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.OleDb;

namespace LoadManager
{
    public class Main
    {
        public const string SYSTEMPATH = @"C:\DeclavisCompany\Kassensystem\";
        public static string ADMINPW;
        public static string ZPW;
        private static int AMOUNTWG;
        private static string PLUDATAPATH;
        public static IniFile Config = new IniFile(SYSTEMPATH+ "config.ini");
        public static int AmountUser;
        public static string Printtyp;
        public static string constr;

        public static void Intizialize()
        {
            if (File.Exists(SYSTEMPATH + "config.ini"))
            {
                Load();
            }
            else
            {
              
            }
        }

        public static string UserName(int id)
        {
            return Config["USER"]["U"+ id];
        }

        public static string UserPW(int id)
        {
            return Config["USER"]["P" + id];
        }

        private static void Load()
        {
            //ClassTable.PRINT.MPath = SYSTEMPATH;
            //ClassTable.PRINT.printer = Convert.ToInt32(Config["PRINT"]["PR"]);
            AmountUser = Convert.ToInt32(Config["USER"]["U"]);
            ADMINPW = Config["ADMIN"]["PW"];
            //ClassLog.Log.LOGPATH = SYSTEMPATH + Config["PATH"]["LOG1"];
            //ClassLog.Log.LOGPATH2 =SYSTEMPATH + Config["PATH"]["LOG2"];
            AMOUNTWG = Convert.ToInt32(Config["WARENGRUPPE"]["WGs"]);
            for (int i = 1; i < AMOUNTWG; i++)
            {
                ClassPLU.WG.ListofWGs.Add(new ClassPLU.WG(i, Config["WARENGRUPPE"]["WG"+i]));
            }
            PLUDATAPATH = Config["PATH"]["PLU"];            
            Printtyp = Config["PRINT"]["ROLL"];
            constr = (@"Provider=Microsoft.Jet.OLEDB.4.0;
Data Source=" + SYSTEMPATH + PLUDATAPATH + ";Jet OLEDB:Database Password=8765-5357-3550-3026;");
            //ClassPLU.PLU.DATAPATH = SYSTEMPATH + PLUDATAPATH;
            //ClassPLU.PLU.constr = constr;
            //SaveManager.Main.DATAPATH = SYSTEMPATH + PLUDATAPATH;
            //SaveManager.Main.constr = constr;
            ClassTable.Table.CreateTable(Convert.ToInt32(Config["TABLE"]["AMOUNT"]));
            ZPW = Config["ADMIN"]["Z"];
        }

        public static void LoadLog()
        {
            //ClassLog.Log.LOGPATH = SYSTEMPATH + Config["PATH"]["LOG1"];
            //ClassLog.Log.LOGPATH2 = SYSTEMPATH + Config["PATH"]["LOG2"];
        }


    }
}
