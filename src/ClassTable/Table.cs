using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassPLU;
using SaveManager;
using System.Data.OleDb;
namespace ClassTable
{
    public class Table
    {

        public int ID;
        public List<int> Articles = new List<int>();
        public List<int> OldArticles = new List<int>();
        public double TotalM;
        public double OldTotalM = 0;
        public List<DIV> Divers = new List<DIV>();
        public List<DIV> OldDivers = new List<DIV>();
        public int ErrorCode;
        public bool OPTable = false;
        public string username;
        public PRINT.BILLTYP be_typ = PRINT.BILLTYP.NONE;

        public List<PLU> NewPLU = new List<PLU>();
        public List<DIV> NewDIV = new List<DIV>();
        public List<PLU> LStornoPLU = new List<PLU>();
        public List<DIV> LStornoDIV = new List<DIV>();
        //Nicht zur Table Klasse
        public static List<Table> ListofTables = new List<Table>();
        public static string User;
        public static int Anzahl;
        //END

        public Table(int _ID, int _ErrorCode)
        {
            ID = _ID;
            ErrorCode = _ErrorCode;
            TotalM = 0;
        }

        public Table(int _ID, string _A, string _OA, double _TM, double _OTM, string _div, string _odiv, bool _opt, string _us, PRINT.BILLTYP _bt)
        {
            ID = _ID;
            if(_A != null && _A != "")
            {
                string[] A = _A.Split(new char[] { ',' });
                foreach (string s in A)
                {
                    Articles.Add(int.Parse(s));
                }
            }
            if (_OA != null && _OA != "")
            {
                string[] OA = _OA.Split(new char[] { ',' });
                foreach (string s in OA)
                {
                    OldArticles.Add(int.Parse(s));
                }
            }
            TotalM = _TM;
            OldTotalM = _OTM;
            if (_div != null && _div != "")
            {
                string[] Tempdiv = _div.Split(new char[] { ',' });
                foreach (string s in Tempdiv)
                {
                    string[] d = s.Split(new char[] { '|' });
                    DIV TDIV = new DIV(int.Parse(d[0]), Convert.ToDouble(d[1]), Convert.ToDouble(d[2]), d[3], Convert.ToBoolean(d[4]));
                    Divers.Add(TDIV);
                }
            }
            if (_odiv != null && _odiv != "")
            {
                string[] Tempodiv = _odiv.Split(new char[] { ',' });
                foreach (string s in Tempodiv)
                {
                    string[] d = s.Split(new char[] { '|' });
                    DIV TDIV = new DIV(int.Parse(d[0]), Convert.ToDouble(d[1]), Convert.ToDouble(d[2]), d[3], Convert.ToBoolean(d[4]));
                    OldDivers.Add(TDIV);
                }
            }
            OPTable = _opt;
            username = _us;
            be_typ = _bt;
        }

        public static void CreateTable(int AMOUNT)
        {
            Anzahl = AMOUNT;
            /*for(int i= 1;i< (AMOUNT + 1);i++)
            {
                ListofTables.Add(new Table(i, 0));
            }*/
            try
            {                
                OleDbConnection con = new OleDbConnection(SaveManager.Main.constr);
                con.Open();

                string strSQL = "SELECT * FROM STa";

                OleDbCommand cmd = new OleDbCommand(strSQL, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int ID = Convert.ToInt32(dr[0].ToString());
                    string TempArt = dr[1].ToString();
                    string TempOldArt = dr[2].ToString();
                    double TempTM = Convert.ToDouble(dr[3].ToString());
                    double TempOldTM = Convert.ToDouble(dr[4].ToString());
                    string TempDIV = dr[5].ToString();
                    string TempOldDIV = dr[6].ToString();
                    bool Tempopt = Convert.ToBoolean(dr[7].ToString());
                    string Tempus = dr[8].ToString();
                    PRINT.BILLTYP Tempbt = PRINT.BILLTYP.NONE;
                    foreach(PRINT.BILLTYP bt in (PRINT.BILLTYP[])Enum.GetValues(typeof(PRINT.BILLTYP)))
                    {
                        if (bt.ToString() == dr[9].ToString())
                        { Tempbt = bt; break; }
                    }
                    ListofTables.Add(new Table(ID, TempArt, TempOldArt , TempTM, TempOldTM, TempDIV , TempOldDIV, Tempopt,Tempus,Tempbt));
                }
                dr.Close();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public static Table LoadTable(int _ID)
        {
            foreach (Table Ta in ListofTables)
            {
                if (Ta.ID == _ID)
                {
                    return Ta;
                }                
            }
            return new Table(0, 1001);            
        }

        public string GetTotallMoney()
        {
            return PLU.PreisUmform(TotalM.ToString());
        }
        public void GetArticle(ListBox lb)
        {
            List<int> list = new List<int>();
            foreach (int i in Articles)
            {
                int count = 0;
                if (!(list.Contains(i)))
                {
                    foreach (int b in Articles)
                    {
                        if (b == i)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        PLU myplu = PLU.PLULoad(i);
                        lb.Items.Insert(0, count + "x " + myplu.BezB + " - " + PLU.PreisUmform(myplu.Price.ToString()));
                    }
                    else
                    {
                        PLU myplu = PLU.PLULoad(i);
                        lb.Items.Insert(0, myplu.BezB + " - " + PLU.PreisUmform(myplu.Price.ToString()));
                    }
                    list.Add(i);
                }
            }
            foreach (DIV div in Divers)
            {
                lb.Items.Insert(0,div.Bez + " - " + PLU.PreisUmform(div.Price.ToString()));
            }
        }
        public void AddPLU(int _id, string X)
        {
            if (X == "1")
            {
                Articles.Add(_id);
                NewPLU.Add(PLU.PLULoad(_id));
                TotalM += PLU.PLULoad(_id).Price;
                TotalM = Math.Round(TotalM, 2);
            }
            else
            {
                for (int i = 0; i < Convert.ToInt32(X); i++)
                {
                    Articles.Add(_id);
                    NewPLU.Add(PLU.PLULoad(_id));
                    TotalM += PLU.PLULoad(_id).Price;
                    TotalM = Math.Round(TotalM, 2);
                }
            }
        }
        public void AddDivers(DIV div)
        {
            Divers.Add(div);
            NewDIV.Add(div);
            TotalM += div.Price;
            TotalM = Math.Round(TotalM, 2);
        }
        public void StornoPLU(int _id)
        {
            if (Articles.Contains(_id))
            {
                Articles.Remove(_id);
                LStornoPLU.Add(PLU.PLULoad(_id));
                TotalM -= PLU.PLULoad(_id).Price;
                TotalM = Math.Round(TotalM,2);
            }
        }
        public void StornoDivers(DIV div)
        {
            if (Divers.Contains(div))
            {
                Divers.Remove(div);
                LStornoDIV.Add(div);
                TotalM -= div.Price;
                TotalM = Math.Round(TotalM, 2);
            }
        }
        public void Pay(PRINT.BILLTYP billtyp,bool bwplan,string us)
        {
            be_typ = billtyp;
            username = us;
            Log.Add("System-Info","Tisch: "+ ID + " zahlt "+PLU.PreisUmform(TotalM.ToString())+" in " + billtyp.ToString());
            PRINT.PUTDATA(User, this,bwplan);
            PRINT.Print(billtyp);
            if (billtyp != PRINT.BILLTYP.SALDO)
            {
                OldTotalM=TotalM;
                OldArticles.AddRange(Articles);
                OldDivers.AddRange(Divers);
                TotalM = 0;
                Articles.Clear();
                Divers.Clear();
                OPTable = false;
            }
        }
        public static void RETable(int id,string us)
        {
            Table retable = LoadTable(id);
            if (us == retable.username)
            {                
                if (retable.OldTotalM != 0)
                    if (retable.OldDivers.Count != 0 || retable.OldArticles.Count != 0)
                    {
                        Log.Add("System-Info", "Tischreaktivierung: "+ retable.ID + " Money at the Table: "+ retable.OldTotalM + "von User: "+ us);
                        retable.OPTable = true;
                        retable.Articles.AddRange(retable.OldArticles);
                        retable.Divers.AddRange(retable.OldDivers);
                        retable.TotalM = retable.OldTotalM;
                        retable.OldTotalM = 0;
                        retable.OldArticles.Clear();
                        retable.OldDivers.Clear();
                        PRINT.PUTDATA(User, retable);
                        PRINT.Print(PRINT.BILLTYP.TRE);
                    }
            }
        }

        public void TableToTable()
        {

        }

        public void Splitt()
        {

        }

        public void SaveTable()
        {
            SaveTableToDB();
            //PRINT NEW ARTICLE
            if (NewPLU.Count != 0 || NewDIV.Count != 0)
            {
                PRINT.PUTDATA(User, this);
                PRINT.Print(PRINT.BILLTYP.NEWARTICLE);
                NewPLU.Clear();
                NewDIV.Clear();
            }
            //PRINT STORNO ARTICLE
            if (LStornoDIV.Count != 0 || LStornoPLU.Count != 0)
            {
                PRINT.PUTDATA(User, this);
                PRINT.Print(PRINT.BILLTYP.STORNO);
                LStornoDIV.Clear();
                LStornoPLU.Clear();
            }            
        }

        private void SaveTableToDB()
        {
            try
            {
                OleDbConnection con = new OleDbConnection(SaveManager.Main.constr);
                
                string TempArt = "";
                bool TempArtB = false;
                foreach (int Ti in Articles)
                {
                    if (TempArtB == false)
                    { TempArt += Ti.ToString(); TempArtB = true; }
                    else
                        TempArt += "," + Ti;
                }

                string TempOldArt = "";
                bool TempOldArtB = false;
                foreach (int Ti in OldArticles)
                {
                    if (TempOldArtB == false)
                    { TempOldArt += Ti.ToString(); TempOldArtB = true; }
                    else
                        TempOldArt += "," + Ti;
                }

                string TempDIV = "";
                bool TempDIVB = false;
                foreach (DIV div in Divers)
                {
                    if (TempDIVB == false)
                    { TempDIV += div.ToString(); TempDIVB = true; }
                    else
                        TempDIV += "," + div.ToString();
                }

                string TempOldDIV = "";
                bool TempOldDIVB = false;
                foreach (DIV div in OldDivers)
                {
                    if (TempOldDIVB == false)
                    { TempOldDIV += div.ToString(); TempOldDIVB = true; }
                    else
                        TempOldDIV += "," + div.ToString();
                }                

                con.Open();
                string SQL = "UPDATE STa SET";
                if(TempArt != "")
                    SQL += " `Articles` = '" + TempArt + "',";
                if (TempOldArt != "")
                    SQL += " `OldArticles` = '" + TempOldArt + "',";
                SQL += " `TotalM` = '" + TotalM + "'," +
                    " `OldTotalM` = '" + OldTotalM + "',";
                if (TempDIV != "")
                    SQL += " `Divers` = '" + TempDIV + "',";
                if (TempOldDIV != "")
                    SQL += " `OldDivers` = '" + TempOldDIV + "',";
                SQL +=" `OPTable` = " + OPTable.ToString() + "," +
                    " `username` = '" + username + "'," +
                    " `be_typ` = '" + be_typ.ToString() + "'" +
                    " WHERE `ID` = " + this.ID;
                OleDbCommand cmd = new OleDbCommand(SQL, con);
                int time = cmd.ExecuteNonQuery();
                con.Close();

                //return nowvalue.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //return "Error: RBONO()";
            }
        }
        public static void CreateTableDBEntry()
        {
            try
            {
                OleDbConnection con = new OleDbConnection(SaveManager.Main.constr);                
                con.Open();
                for(int i = 1; i <= Anzahl; i++)
                {
                    string SQL = "INSERT INTO STa (`ID`) VALUES ('"+i.ToString()+"')";
                    OleDbCommand cmd = new OleDbCommand(SQL, con);
                    int time = cmd.ExecuteNonQuery();
                }
                con.Close();

                //return nowvalue.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //return "Error: RBONO()";
            }
        }
        public static void DeleteTableDBEntry()
        {
            try
            {
                OleDbConnection con = new OleDbConnection(SaveManager.Main.constr);
                con.Open();
                string SQL = "DELETE FROM STa";
                OleDbCommand cmd = new OleDbCommand(SQL, con);
                int time = cmd.ExecuteNonQuery();
                con.Close();

                //return nowvalue.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                //return "Error: RBONO()";
            }
        }
    }
}
