using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ClassPLU;
using System.Data.OleDb;

namespace SaveManager
{
    public partial class frm_Export : Form
    {
        public frm_Export()
        {
            InitializeComponent();
        }
        string SystemValues(string info) { return Main.SystemValues(info); }
        public XmlDocument CreateXmlDocument()
        {
            try
            {
                chpath();                

                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
                doc.InsertBefore(decl, doc.DocumentElement);

                doc.AppendChild(doc.CreateDocumentType("DataSet",
                   null,
                   "gdpdu-01-08-2002.dtd",
                   null));

                XmlNode rootNode = doc.CreateElement("DataSet");
                doc.AppendChild(rootNode);

                XmlNode VersionNode = doc.CreateElement("Version");
                VersionNode.InnerText = "1.0";
                rootNode.AppendChild(VersionNode);

                XmlNode DataSupplierNode = doc.CreateElement("DataSupplier");
                rootNode.AppendChild(DataSupplierNode);

                XmlNode NameNode = doc.CreateNode(((XmlNodeType[])Enum.GetValues(typeof(XmlNodeType)))[1], "Name", doc.NamespaceURI);
                DataSupplierNode.AppendChild(NameNode);

                XmlNode LocNode = doc.CreateNode(((XmlNodeType[])Enum.GetValues(typeof(XmlNodeType)))[1], "Location", doc.NamespaceURI);
                DataSupplierNode.AppendChild(LocNode);

                XmlNode MediaNode = doc.CreateElement("Media");
                rootNode.AppendChild(MediaNode);

                XmlNode MediaNameNode = doc.CreateElement("Name");
                MediaNameNode.InnerText = "USB";
                MediaNode.AppendChild(MediaNameNode);

               FirmaDataGrid(doc, MediaNode);
               StammdataPLU(doc, MediaNode);
               ZAbschlag(doc, MediaNode);
                doc.Save(Application.StartupPath + "/gdpdu-export-v1/index.xml");

                /*XmlTextWriter writer = new XmlTextWriter("test.xml", Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                doc.WriteTo(writer);
                writer.Flush();
                writer.Close();*/

                return doc;
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        void chpath()
        {
            if (Directory.Exists(Application.StartupPath + "/gdpdu-export-v1"))
            { Directory.Delete(Application.StartupPath + "/gdpdu-export-v1",true); }
            
            Directory.CreateDirectory(Application.StartupPath + "/gdpdu-export-v1");
            
        }
        void cxn(string Name,string InnerText,XmlNode Add,XmlDocument doc)
        {
            XmlNode Node = doc.CreateElement(Name);
            Node.InnerText = InnerText;
            Add.AppendChild(Node);            
        }
        XmlNode Numeric(XmlNode Add, XmlDocument doc)
        {
            XmlNode Numeric = doc.CreateElement("Numeric");
            Add.AppendChild(Numeric);
            return Add;
        }
        XmlNode Numeric(string Acc,XmlNode Add, XmlDocument doc)
        {
            XmlNode Numeric = doc.CreateElement("Numeric");
            Add.AppendChild(Numeric);
            XmlNode Accuracy = doc.CreateElement("Accuracy");
            Accuracy.InnerText = Acc;
            Numeric.AppendChild(Accuracy);
            return Add;
        }
        XmlNode VariableColumn(XmlNode Add, XmlDocument doc)
        {
            XmlNode Node = doc.CreateElement("VariableColumn");
            Node.SelectNodes("test");
            Add.AppendChild(Node);
            return Node;
        }
        XmlNode AlphaNumeric(XmlNode Add, XmlDocument doc)
        {
            /*foreach(XmlNodeType xnt in /*(XmlNodeType[])*\/Enum.GetValues(typeof(XmlNodeType)))
            {
                if (xnt != XmlNodeType.None && xnt != XmlNodeType.Entity && xnt != XmlNodeType.Notation && xnt != XmlNodeType.EndElement&& xnt!=XmlNodeType.EndEntity)
                {
                    XmlNode x = doc.CreateNode(xnt, "AlphaNumeric"+xnt.ToString(), doc.NamespaceURI);
                    try { Add.AppendChild(x); }catch(Exception e){}
                }
                
            }*/
            XmlNode x = doc.CreateNode(((XmlNodeType[])Enum.GetValues(typeof(XmlNodeType)))[1], "AlphaNumeric", doc.NamespaceURI);
            Add.AppendChild(x);
            return Add;
        }
        string PreisUmform3(double price)
        {
            return PLU.PreisUmform3(price);
        }
        public void FirmaDataGrid(XmlDocument doc,XmlNode Media)
        {
            //Filename firma_bp.txt
            //char Wall = '|';
            string Firmenname=Main.SystemValues("CompanyName");
            string NettoBruttofirma = Main.SystemValues("CompanyTyp");
            string Str = Main.SystemValues("ConpanyLoc_Str");
            string Postleitzahl = Main.SystemValues("CompanyLoc_Pz");
            string Ort = Main.SystemValues("CompanyLoc_Ort");
            string Telefon = Main.SystemValues("CompanyCom_Tel");
            string Fax = Main.SystemValues("CompanyCom_Fax");
            string Email = Main.SystemValues("CompanyCom_Email");
            string Internet = Main.SystemValues("CompanyCom_Web");
            string Waehrung = Main.SystemValues("CompanyFiBu_Wg");
            string IBANKontoNr = Main.SystemValues("CompanyFiBu_IBAN");
            string StNr = Main.SystemValues("CompanyFiBu_Stn");

            string Content = "Firmenname|Netto-/Bruttofirma|Strasse|Postleitzahl|Ort|Telefon|Fax|Email|Internet|Waehrung|IBAN Konto Nr|Steuernummer\n";
            Content += Firmenname + "|" + NettoBruttofirma + "|" + Str + "|" + Postleitzahl + "|" + Ort + "|" + Telefon + "|" + Fax + "|" + Email + "|" + Internet + "|" + Waehrung + "|" + IBANKontoNr + "|" + StNr;

            StreamWriter sw = new StreamWriter(Application.StartupPath + "/gdpdu-export-v1/firma_bp.txt");
            sw.Write(Content);
            sw.Close();

            XmlNode rootNode_IN = doc.CreateElement("Table");
            Media.AppendChild(rootNode_IN);

            cxn("URL", "firma_bp.txt", rootNode_IN, doc);
            cxn("Name", "firma_bp", rootNode_IN, doc);
            cxn("Description", "Firmenstammblatt", rootNode_IN, doc);

            XmlNode RangeNode_IN = doc.CreateElement("Range");
            rootNode_IN.AppendChild(RangeNode_IN);

            cxn("From","2",RangeNode_IN,doc);
            
            XmlNode VariableLengthNode_IN = doc.CreateElement("VariableLength");
            rootNode_IN.AppendChild(VariableLengthNode_IN);
            
            cxn("ColumnDelimiter","|",VariableLengthNode_IN,doc);            
            cxn("Name","Firmenname",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);           
            cxn("Name","Netto-/Bruttofirma",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","Strasse",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);          
            cxn("Name","Postleitzahl",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","Ort",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);           
            cxn("Name","Telefon",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","Fax",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","Email",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);           
            cxn("Name","Internet",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);           
            cxn("Name","Waehrung",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","IBAN Konto Nr",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);            
            cxn("Name","Steuernummer",AlphaNumeric(VariableColumn(VariableLengthNode_IN,doc),doc),doc);
                    
               
            
        }
        public void StammdataPLU(XmlDocument doc, XmlNode Media)
        {
            //FileName: Artikel.txt
            //char wall = '|';

            string Content = "Artikel-ID|Artikel-Bezeichnung|Getraenk/Essen|Artikel-Einheit|Artikel-Preis-Brutto|Artikel-Preis-Netto|Artikel-Ust-Betrag|Artikel-Ust-Satz";
            
            PLU.PLUDATALoad();
            
            foreach (PLU plu in PLU.ListofPLUs)
            {
                string PLUID = plu.ID.ToString();
                string PLUNAME = plu.BezM;
                string PLUTYP;
                if (plu.Food) { PLUTYP = "Essen"; } else { PLUTYP = "Getraenk"; }
                string PLUEINHEIT = plu.Einheit;
                double Brutto = plu.Price;
                double Netto;
                double Steuerbetrag;
                int vat = (int)plu.Steuer;
                Netto = Brutto / (vat + 100) * 100;
                Steuerbetrag = Brutto / (vat + 100) * vat;
                Content += "\n"+PLUID + "|" + PLUNAME + "|" + PLUTYP + "|" + PLUEINHEIT + "|" + PreisUmform3(Brutto) + "|" + PreisUmform3(Netto) + "|" + PreisUmform3(Steuerbetrag) + "|" + vat.ToString();
            }
            
            StreamWriter sw = new StreamWriter(Application.StartupPath + "/gdpdu-export-v1/artikel.txt");
            sw.Write(Content);
            sw.Close();

            XmlNode rootNode_IN = doc.CreateElement("Table");
            Media.AppendChild(rootNode_IN);

            cxn("URL", "artikel.txt", rootNode_IN, doc);
            cxn("Name", "artikel", rootNode_IN, doc);
            cxn("Description", "Liste aller Artikel", rootNode_IN, doc);

            XmlNode RangeNode_IN = doc.CreateElement("Range");
            rootNode_IN.AppendChild(RangeNode_IN);

            cxn("From", "2", RangeNode_IN, doc);

            XmlNode VariableLengthNode_IN = doc.CreateElement("VariableLength");
            rootNode_IN.AppendChild(VariableLengthNode_IN);

            cxn("ColumnDelimiter", "|", VariableLengthNode_IN, doc);
            cxn("Name", "Artikel-ID", AlphaNumeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Bezeichnung", AlphaNumeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Getraenk/Essen", AlphaNumeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Einheit", AlphaNumeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Preis-Brutto", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Preis-Netto", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Ust-Betrag", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Artikel-Ust-Satz", Numeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            
        }
        public void ZAbschlag(XmlDocument doc, XmlNode Media)
        {
            //FileName: Zabschlag.txt
            //char wall = '|';

            string Content = "DateTime|EssenBrutto|GetraenkBrutto|TotalBrutto|TotalNetto|Mwst-Betrag|Mwst-Satz|TotalStorno";

            PLU.PLUDATALoad();
            OleDbConnection con = new OleDbConnection(Main.constr);           
            con.Open();
            string strSQL = "SELECT * FROM ZAB";
            OleDbCommand cmd = new OleDbCommand(strSQL, con);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string date = dr[1].ToString();
                string fbrutto = dr[2].ToString();
                string bbrutto = dr[3].ToString();
                string tbrutto = dr[4].ToString();
                string tnetto = dr[5].ToString();
                string tmwst = dr[6].ToString();
                string ts = dr[7].ToString();
                Content += "\n" + date + "|" + fbrutto + "|" + bbrutto + "|" + tbrutto + "|" + tnetto + "|" + tmwst + "|19|" + ts;
            }
            dr.Close();
            con.Close();

            StreamWriter sw = new StreamWriter(Application.StartupPath + "/gdpdu-export-v1/zabschlag.txt");
            sw.Write(Content);
            sw.Close();

            XmlNode rootNode_IN = doc.CreateElement("Table");
            Media.AppendChild(rootNode_IN);

            cxn("URL", "zabschlag.txt", rootNode_IN, doc);
            cxn("Name", "zabschlag", rootNode_IN, doc);
            cxn("Description", "Liste von Z-Abschlägen mit Werten TotalStorno entspricht allen Storno addiert", rootNode_IN, doc);

            XmlNode RangeNode_IN = doc.CreateElement("Range");
            rootNode_IN.AppendChild(RangeNode_IN);

            cxn("From", "2", RangeNode_IN, doc);

            XmlNode VariableLengthNode_IN = doc.CreateElement("VariableLength");
            rootNode_IN.AppendChild(VariableLengthNode_IN);

            cxn("ColumnDelimiter", "|", VariableLengthNode_IN, doc);
            cxn("Name", "DateTime", AlphaNumeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "EssenBrutto", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "GetraenkBrutto", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "TotalBrutto", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "TotalNetto", Numeric("2", VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Mwst-Betrag", Numeric("2", VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "Mwst-Satz", Numeric(VariableColumn(VariableLengthNode_IN, doc), doc), doc);
            cxn("Name", "TotalStorno", Numeric("2",VariableColumn(VariableLengthNode_IN, doc), doc), doc);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            var os = System.Environment.OSVersion;
            bool isWindowsXp = (os.Version.Major == 5 && os.Version.Minor == 1);
            if(isWindowsXp ==false)
                new SaveManager.frm_Export().CreateXmlDocument();
        }
        
    }
}
