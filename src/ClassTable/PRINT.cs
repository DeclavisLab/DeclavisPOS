using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using ClassPLU;
using System.IO;
using SaveManager;
using System.Windows.Forms;

namespace ClassTable
{
    public class PRINT
    {
        public enum BILLTYP { SALDO, NEWARTICLE, STORNO, BAR, KREDIT, HOTEL, EC, USALDO, TRE , ZAB, NONE}
        static string _User;
        public static string MPath = LoadManager.Main.SYSTEMPATH;
        public static int printer = Convert.ToInt32(LoadManager.Main.Config["PRINT"]["PR"]);
        static int z = 5;
        static bool bwplan;
        static double mf;
        static double mb;
        static int c;
        static double bru;
        static string val;
        //NEW ARTICLE
        static Table AktTab = Table.LoadTable(1);
        //END
        public static void PUTDATA(string User,Table thTab)
        {
            AktTab = thTab;
            _User = User;
        }
        public static void PUTDATA(string User,int co, double brut ,string value)
        {
            _User = User;
            c = co;
            bru = brut;
            val = value;
        }
        public static void PUTDATA(string User, Table thTab, bool bw)
        {
            AktTab = thTab;
            _User = User;
            bwplan = bw;
        }
        public static void PUTDATA(string User, double f, double b)
        {
            _User = User;
            mf = f;
            mb = b;
        }
        static string RechnungsNumber()
        {
            Log.Add("System-PRINT()", "Rechnungsnummer++");
            return SaveManager.Main.RBONO();
        }
        static string ABN()
        {
            Log.Add("System-PRINT()", "Z-Abschlagnummer++");
            return SaveManager.Main.ZABNO();
        }
        
        public static void Print(BILLTYP billtyp)
        {
            PrintDocument PrintDoc = new PrintDocument();

            List<string> Printers = new List<string>();
            foreach (string p in PrinterSettings.InstalledPrinters)
            Printers.Add(p);
            // CONFIG DRUCKER !!
            PrintDoc.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[printer];
            switch(billtyp)
            {
                case BILLTYP.BAR:
                    PrintDoc.PrintPage += new PrintPageEventHandler(BILL_BAR);
                    break;
                case BILLTYP.KREDIT:
                    PrintDoc.PrintPage += new PrintPageEventHandler(BILL_KREDIT);
                    break;
                case BILLTYP.EC:
                    PrintDoc.PrintPage += new PrintPageEventHandler(BILL_EC);
                    break;
                case BILLTYP.HOTEL:
                    PrintDoc.PrintPage += new PrintPageEventHandler(BILL_HOTEL);
                    break;
                case BILLTYP.NEWARTICLE:
                    PrintDoc.PrintPage += new PrintPageEventHandler(NEWARTICLE);
                    break;
                case BILLTYP.SALDO:
                    PrintDoc.PrintPage += new PrintPageEventHandler(BILL_SALDO);
                    break;
                case BILLTYP.STORNO:
                    PrintDoc.PrintPage += new PrintPageEventHandler(STORNO);
                    break;
                case BILLTYP.USALDO:
                    PrintDoc.PrintPage += new PrintPageEventHandler(USER_SALDO);
                    break;
                case BILLTYP.TRE:
                    PrintDoc.PrintPage += new PrintPageEventHandler(TRE);
                    break;
                case BILLTYP.ZAB:
                    PrintDoc.PrintPage += new PrintPageEventHandler(ZAB);
                    break;
            }  
            if(!Interface.Startup.Debug)
                PrintDoc.Print();
        }
        static void ZAB(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort";
            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100-10));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 165-40), new Point(200, 165-40));
            e.Graphics.DrawString("Z-Abschlag", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 170-40));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 195-40), new Point(200, 195-40));
            
            //Insgesammt
            double Brutto = bru;
            double Netto = Math.Round((Brutto/119)*100,2);
            double Mwst = Math.Round((Brutto/119)*19,2);
            e.Graphics.DrawString("Insgesammt:\nBrutto: " + PLU.PreisUmform(Brutto.ToString()) + "\nNetto: " + PLU.PreisUmform(Netto.ToString()) + "\nMwst (19 %): " + PLU.PreisUmform(Mwst.ToString()), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 200-40));
            int x = (228*(c-1))+200-20-40;
            e.Graphics.DrawString(val , new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 255-40));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 69+x), new Point(200, 69+x));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(today.ToString("dd.MM.yyyy HH:mm:ss").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 58+x));
            e.Graphics.DrawString("Z-Abschlag: "+ ABN()+"\n"+_User, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 72+x));
                
    }
        static void TRE(object sender, PrintPageEventArgs e)
        {
            Log.Add("System-PRINT()", "Tischreaktivierung: " + AktTab.ID + _User);
            string Text = "Tischreaktivierung: " + AktTab.ID + "\n";
            e.Graphics.DrawString(Text, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, -4));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 30), new Point(200, 30));
            Text = "Tisch: " + AktTab.ID;
            int x = 40 + 1 * 16;
            e.Graphics.DrawString(Text, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 35));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, x), new Point(200, x));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(today.ToString("dd.MM.yyyy HH:mm:ss").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 10));
            e.Graphics.DrawString(_User, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 25));
        }

        static void USER_SALDO(object sender, PrintPageEventArgs e)
        {
            Log.Add("System-PRINT()", "User-Saldo: " + _User + " Getränke: " + PLU.PreisUmform(mb.ToString()) + " Essen: " + PLU.PreisUmform(mf.ToString()));
            string Text = "User SALDO: " + _User + "\n";
            e.Graphics.DrawString(Text, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, -4));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 30), new Point(200, 30));
            Text = "Getränke: "+ PLU.PreisUmform(mb.ToString()) + "\nEssen: "+ PLU.PreisUmform(mf.ToString());
            int x = 40 + 2 * 16;
            e.Graphics.DrawString(Text, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 35));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, x), new Point(200, x));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(today.ToString("dd.MM.yyyy HH:mm:ss").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 10));
            e.Graphics.DrawString(_User, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 25));
        }

        static void BILL_SALDO(object sender, PrintPageEventArgs e)
        {
            Log.Add("System-PRINT()", "Bill-Saldo");
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort\nwww.HotelMuster.com\nTelefon: 01234 56789";

            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 185), new Point(200, 185));
            e.Graphics.DrawString("SALDO", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 200));
            e.Graphics.DrawString("Tisch: " + AktTab.ID, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 220));
            string Articels = "";
            int y = 1;
            List<PLU> MYPLU = new List<PLU>();
            List<PLU> list = new List<PLU>();
            foreach (DIV div in AktTab.Divers)
            {
                y++;
                Articels += div.Bez + "   " + PLU.PreisUmform(div.Price.ToString()) + "\n";
            }
            foreach (int i in AktTab.Articles)
            {
                MYPLU.Add(PLU.PLULoad(i));
            }
            foreach (PLU plu in MYPLU)
            {
                if (!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in MYPLU)
                    {
                        if (plu == plua)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        y += 3;
                        Articels += count + "x   " + PLU.PreisUmform(plu.Price.ToString()) + "\n" + plu.BezB + "   " + PLU.PreisUmform(Math.Round(count * plu.Price, 2).ToString()) + "\n";
                    }
                    else
                    {
                        y++;
                        Articels += plu.BezB + "  " + PLU.PreisUmform(plu.Price.ToString()) + "\n";
                    }
                    list.Add(plu);
                }
            }
            e.Graphics.DrawString(Articels, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 240));
            int t = 240 + (y * 10);
            e.Graphics.DrawString("SALDO: " + PLU.PreisUmform(AktTab.TotalM.ToString()), new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 1));
            string mwst = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 19, 2).ToString());
            string netto = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 100, 2).ToString());
            e.Graphics.DrawString("Mwst 19%: " + mwst, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 2));
            e.Graphics.DrawString("Netto:         " + netto, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 3));
            e.Graphics.DrawString("Rechnungs-Nummer: " + RechnungsNumber(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 4));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(_User + "     " + today.ToString("dd.MM.yyyy").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 5));
            string Foot = "Wir würden uns freuen, Sie bald\nwieder begrüßen zu dürfen.\nSt.Nr. 0000/0000/0000";
            e.Graphics.DrawString(Foot, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 6));
        }

        static void NEWARTICLE(object sender, PrintPageEventArgs e)
        {
                      
            int y = 0;
            string Text = "Tisch: " + AktTab.ID + "\n";
            e.Graphics.DrawString(Text, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, -4));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 30), new Point(200, 30));
            string Articles = "";
            List<PLU> list = new List<PLU>();
            foreach (PLU plu in AktTab.NewPLU)
            {
                if(!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in AktTab.NewPLU)
                    {
                        if (plua == plu)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        Articles += count+"x "+plu.BezB + " [" + PLU.PreisUmform(plu.Price.ToString()) + "]\n";
                        y++;
                    }
                    else
                    {
                        Articles += plu.BezB + " [" + PLU.PreisUmform(plu.Price.ToString()) + "]\n";
                        y++;
                    }
                    list.Add(plu);
                }
            }
            foreach (DIV div in AktTab.NewDIV)
            {
                Articles += div.Bez + " [" + PLU.PreisUmform(div.Price.ToString()) + "]\n";
                y++;
            }
            int x = 40 + y * 16;
            e.Graphics.DrawString(Articles, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 35));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, x), new Point(200, x));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(today.ToString("dd.MM.yyyy HH:mm:ss").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 10));
            e.Graphics.DrawString(_User, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 25));
        }

        static void STORNO(object sender, PrintPageEventArgs e)
        {
            Log.Add("System-PRINT(Storno)", "Storno: von" + _User+ " -See next Entry-");
            string Text = "STORNO TISCH: " + AktTab.ID + "\n";
            e.Graphics.DrawString(Text, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, -4));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 30), new Point(200, 30));
            Text = "";
            int y = 0;
            
                foreach (DIV div in AktTab.LStornoDIV)
                {
                    Text += div.Bez + " "+ div.Price +"\n";                    
                    y++;
                }
           
                foreach (PLU plu in AktTab.LStornoPLU)
                {
                    Text += plu.BezB + " " + plu.Price + "\n";                    
                    y++;
                }
                Log.Add("System-PRINT()", Text);
            int x = 40 + y * 16;
            e.Graphics.DrawString(Text, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 35));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, x), new Point(200, x));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(today.ToString("dd.MM.yyyy HH:mm:ss").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 10));
            e.Graphics.DrawString(_User, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, x + 25));
        }

        static void BILL_BAR(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort\nwww.HotelMuster.com\nTelefon: 01234 56789";
            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 185), new Point(200, 185));
            e.Graphics.DrawString("Rechnung", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 200));
            e.Graphics.DrawString("Tisch: " + AktTab.ID, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 220));
            string Articels = "";
            int y = 1;
            List<PLU> MYPLU = new List<PLU>();
            List<PLU> list = new List<PLU>();
            foreach (DIV div in AktTab.Divers)
            {
                y++;
                Articels += div.Bez + "   " + PLU.PreisUmform(div.Price.ToString())+"\n";
            }
            foreach (int i in AktTab.Articles)
            {
                MYPLU.Add(PLU.PLULoad(i));
            }
            foreach (PLU plu in MYPLU)
            {
                if (!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in MYPLU)
                    {
                        if(plu == plua)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        y += 3;
                        Articels += count + "x   " + PLU.PreisUmform(plu.Price.ToString()) + "\n"+plu.BezB + "   " + PLU.PreisUmform(Math.Round(count*plu.Price,2).ToString()) + "\n";
                    }
                    else
                    {
                        y++;
                        Articels += plu.BezB + "  " + PLU.PreisUmform(plu.Price.ToString()) + "\n";
                    }
                    list.Add(plu);
                }
            }
                e.Graphics.DrawString(Articels, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 240));
                int t = 240 + (y * 10);
                e.Graphics.DrawString("Total-BAR: " + PLU.PreisUmform(AktTab.TotalM.ToString()), new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 1));
                string mwst = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119)*19,2).ToString());
                string netto = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 100, 2).ToString());
                e.Graphics.DrawString("Mwst 19%: " + mwst, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 2));
                e.Graphics.DrawString("Netto:         "+netto, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 3));
                e.Graphics.DrawString("Rechnungs-Nummer: "+RechnungsNumber(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 4));
                DateTime today = DateTime.Now;
                e.Graphics.DrawString(_User + "     " + today.ToString("dd.MM.yyyy").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 5));
                string Foot = "Wir würden uns freuen, Sie bald\nwieder begrüßen zu dürfen.\nSt.Nr. 0000/0000/0000";
                e.Graphics.DrawString(Foot, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 6));
                if (bwplan == true)
                {
                    string Msg = "-----------------------------------------\nAngaben zum Nachweiß von\nBetriebsaufwendungen nach\n(Par.4 Abs.5 Ziff.2 EStG)\n-----------------------------------------\nBewirtete Person(en):\n\n\n\n\nAnlass der Bewirtung:\n\nHöhe der Aufwendungen:\n_ _ _ _ _ _ _ _ €\n(Bewirtung in Gaststätte)\n\n_ _ _ _ _ _ _ _ €\n(in anderen Fällen)\n\n.  .  .  .  .  .  .  .  .  .\nOrt               Datum\n\n.  .  .  .  .  .  .  .  .  .\nUnterschrift";
                    e.Graphics.DrawString(Msg, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 11));

                }
            

        }

        static void BILL_KREDIT(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort\nwww.HotelMuster.com\nTelefon: 01234 56789";

            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 185), new Point(200, 185));
            e.Graphics.DrawString("Rechnung", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 200));
            e.Graphics.DrawString("Tisch: " + AktTab.ID, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 220));
            string Articels = "";
            int y = 1;
            List<PLU> MYPLU = new List<PLU>();
            List<PLU> list = new List<PLU>();
            foreach (DIV div in AktTab.Divers)
            {
                y++;
                Articels += div.Bez + "   " + PLU.PreisUmform(div.Price.ToString()) + "\n";
            }
            foreach (int i in AktTab.Articles)
            {
                MYPLU.Add(PLU.PLULoad(i));
            }
            foreach (PLU plu in MYPLU)
            {
                if (!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in MYPLU)
                    {
                        if (plu == plua)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        y += 3;
                        Articels += count + "x   " + PLU.PreisUmform(plu.Price.ToString()) + "\n" + plu.BezB + "   " + PLU.PreisUmform(Math.Round(count * plu.Price, 2).ToString()) + "\n";
                    }
                    else
                    {
                        y++;
                        Articels += plu.BezB + "  " + PLU.PreisUmform(plu.Price.ToString()) + "\n";
                    }
                    list.Add(plu);
                }
            }
            e.Graphics.DrawString(Articels, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 240));
            int t = 240 + (y * 10);
            e.Graphics.DrawString("Total-CC: " + PLU.PreisUmform(AktTab.TotalM.ToString()), new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 1));
            string mwst = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 19, 2).ToString());
            string netto = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 100, 2).ToString());
            e.Graphics.DrawString("Mwst 19%: " + mwst, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 2));
            e.Graphics.DrawString("Netto:         " + netto, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 3));
            e.Graphics.DrawString("Rechnungs-Nummer: " + RechnungsNumber(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 4));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(_User + "     " + today.ToString("dd.MM.yyyy").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 5));
            string Foot = "Wir würden uns freuen, Sie bald\nwieder begrüßen zu dürfen.\nSt.Nr. 0000/0000/0000";
            e.Graphics.DrawString(Foot, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 6));
            if (bwplan == true)
            {
                string Msg = "-----------------------------------------\nAngaben zum Nachweiß von\nBetriebsaufwendungen nach\n(Par.4 Abs.5 Ziff.2 EStG)\n-----------------------------------------\nBewirtete Person(en):\n\n\n\n\nAnlass der Bewirtung:\n\nHöhe der Aufwendungen:\n_ _ _ _ _ _ _ _ €\n(Bewirtung in Gaststätte)\n\n_ _ _ _ _ _ _ _ €\n(in anderen Fällen)\n\n.  .  .  .  .  .  .  .  .  .\nOrt               Datum\n\n.  .  .  .  .  .  .  .  .  .\nUnterschrift";
                e.Graphics.DrawString(Msg, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 11));

            }
        }

        static void BILL_HOTEL(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort\nwww.HotelMuster.com\nTelefon: 01234 56789";
            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 185), new Point(200, 185));
            e.Graphics.DrawString("Rechnung", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 200));
            e.Graphics.DrawString("Tisch: " + AktTab.ID, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 220));
            string Articels = "";
            int y = 1;
            List<PLU> MYPLU = new List<PLU>();
            List<PLU> list = new List<PLU>();
            foreach (DIV div in AktTab.Divers)
            {
                y++;
                Articels += div.Bez + "   " + PLU.PreisUmform(div.Price.ToString()) + "\n";
            }
            foreach (int i in AktTab.Articles)
            {
                MYPLU.Add(PLU.PLULoad(i));
            }
            foreach (PLU plu in MYPLU)
            {
                if (!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in MYPLU)
                    {
                        if (plu == plua)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        y += 3;
                        Articels += count + "x   " + PLU.PreisUmform(plu.Price.ToString()) + "\n" + plu.BezB + "   " + PLU.PreisUmform(Math.Round(count * plu.Price, 2).ToString()) + "\n";
                    }
                    else
                    {
                        y++;
                        Articels += plu.BezB + "  " + PLU.PreisUmform(plu.Price.ToString()) + "\n";
                    }
                    list.Add(plu);
                }
            }
            e.Graphics.DrawString(Articels, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 240));
            int t = 240 + (y * 10);
            e.Graphics.DrawString("Total-HOTEL: " + PLU.PreisUmform(AktTab.TotalM.ToString()), new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 1));
            string mwst = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 19, 2).ToString());
            string netto = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 100, 2).ToString());
            e.Graphics.DrawString("Mwst 19%: " + mwst, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 2));
            e.Graphics.DrawString("Netto:         " + netto, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 3));
            e.Graphics.DrawString("Rechnungs-Nummer: " + RechnungsNumber(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 4));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(_User + "     " + today.ToString("dd.MM.yyyy").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 5));
            string Foot = "Wir würden uns freuen, Sie bald\nwieder begrüßen zu dürfen.\nSt.Nr. 0000/0000/0000";
            e.Graphics.DrawString(Foot, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 6));
            if (bwplan == true)
            {
                string Msg = "-----------------------------------------\nAngaben zum Nachweiß von\nBetriebsaufwendungen nach\n(Par.4 Abs.5 Ziff.2 EStG)\n-----------------------------------------\nBewirtete Person(en):\n\n\n\n\nAnlass der Bewirtung:\n\nHöhe der Aufwendungen:\n_ _ _ _ _ _ _ _ €\n(Bewirtung in Gaststätte)\n\n_ _ _ _ _ _ _ _ €\n(in anderen Fällen)\n\n.  .  .  .  .  .  .  .  .  .\nOrt               Datum\n\n.  .  .  .  .  .  .  .  .  .\nUnterschrift";
                e.Graphics.DrawString(Msg, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 11));

            }
        }

        static void BILL_EC(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(Interface.Properties.Resources.Logo, new Rectangle(0, 10, 250 - (125 / 2), 75));
            string Adresse = "Musterstr. 1 \n12345 Ort\nwww.HotelMuster.com\nTelefon: 01234 56789";
            e.Graphics.DrawString(Adresse, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(z - 5, 185), new Point(200, 185));
            e.Graphics.DrawString("Rechnung", new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 200));
            e.Graphics.DrawString("Tisch: " + AktTab.ID, new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, 220));
            string Articels = "";
            int y = 1;
            List<PLU> MYPLU = new List<PLU>();
            List<PLU> list = new List<PLU>();
            foreach (DIV div in AktTab.Divers)
            {
                y++;
                Articels += div.Bez + "   " + PLU.PreisUmform(div.Price.ToString()) + "\n";
            }
            foreach (int i in AktTab.Articles)
            {
                MYPLU.Add(PLU.PLULoad(i));
            }
            foreach (PLU plu in MYPLU)
            {
                if (!(list.Contains(plu)))
                {
                    int count = 0;
                    foreach (PLU plua in MYPLU)
                    {
                        if (plu == plua)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        y += 3;
                        Articels += count + "x   " + PLU.PreisUmform(plu.Price.ToString()) + "\n" + plu.BezB + "   " + PLU.PreisUmform(Math.Round(count * plu.Price, 2).ToString()) + "\n";
                    }
                    else
                    {
                        y++;
                        Articels += plu.BezB + "  " + PLU.PreisUmform(plu.Price.ToString()) + "\n";
                    }
                    list.Add(plu);
                }
            }
            e.Graphics.DrawString(Articels, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, 240));
            int t = 240 + (y * 10);
            e.Graphics.DrawString("Total-EC: " + PLU.PreisUmform(AktTab.TotalM.ToString()), new Font("Arial", 12), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 1));
            string mwst = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 19, 2).ToString());
            string netto = PLU.PreisUmform(Math.Round((AktTab.TotalM / 119) * 100, 2).ToString());
            e.Graphics.DrawString("Mwst 19%: " + mwst, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 2));
            e.Graphics.DrawString("Netto:         " + netto, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 3));
            e.Graphics.DrawString("Rechnungs-Nummer: " + RechnungsNumber(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 4));
            DateTime today = DateTime.Now;
            e.Graphics.DrawString(_User + "     " + today.ToString("dd.MM.yyyy").ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 5));
            string Foot = "Wir würden uns freuen, Sie bald\nwieder begrüßen zu dürfen.\nSt.Nr. 0000/0000/0000";
            e.Graphics.DrawString(Foot, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 6));
            if (bwplan == true)
            {
                string Msg = "-----------------------------------------\nAngaben zum Nachweiß von\nBetriebsaufwendungen nach\n(Par.4 Abs.5 Ziff.2 EStG)\n-----------------------------------------\nBewirtete Person(en):\n\n\n\n\nAnlass der Bewirtung:\n\nHöhe der Aufwendungen:\n_ _ _ _ _ _ _ _ €\n(Bewirtung in Gaststätte)\n\n_ _ _ _ _ _ _ _ €\n(in anderen Fällen)\n\n.  .  .  .  .  .  .  .  .  .\nOrt               Datum\n\n.  .  .  .  .  .  .  .  .  .\nUnterschrift";
                e.Graphics.DrawString(Msg, new Font("Arial", 10), new SolidBrush(Color.Black), new Point(z - 4, t + 20 * 11));

            }
        }

    }
}
