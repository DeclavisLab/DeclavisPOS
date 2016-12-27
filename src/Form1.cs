using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassTable;
using ClassPLU;
using SaveManager;

namespace Interface
{
    public partial class Form1 : Form
    {
        public User ActivUser;
        public string Multiplikator = "1";        
        Table optab=null;
        public Form1()
        {
            InitializeComponent();
            
            foreach (ClassPLU.WG wg in ClassPLU.WG.ListofWGs)
            {
                comboBox1.Items.Add(wg.Name);
            }
            //DEV CODE

            /*var imageList = new ImageList();
            /*imageList.Images.Add("Plate", Interface.Properties.Resources.folder_blue_coffee);
            // tell your ListView to use the new image list
            //listView1.LargeImageList = imageList;
            listView1.SmallImageList = imageList;
            
            foreach (ListViewItem item in listView1.Items)
            {
                item.ImageKey = "folder_blue_coffee.png";
                
            }*/

            //END DEV CODE
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Reset();
        }

        public void User(User user)
        {
            ActivUser = user;
            this.Text += " Angemeldeter User: " + ActivUser.Name;
            Log.Add("System-Info", "User: " + ActivUser.Name + " loging in.");
            base.ShowDialog();
        }
        void Logout()
        {
            ActivUser = null;
            this.Close();            
        }
        void Reset()
        {
            listBox1.Items.Clear();
            lbl_input.Text = "-";
            lbl_table.Text = "Tisch: ";
            lbl_x.Text = "X: 1";
            lbl_m.Text = "TOTALL: ";
            
        }
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (optab != null && e.Item.Selected == true)
            {
                optab.AddPLU(PLU.PLULoad(e.Item.Text,true).ID,Multiplikator);
                listBox1.Items.Clear();
                optab.GetArticle(listBox1);
                lbl_m.Text = "TOTALL: " + optab.GetTotallMoney();
                Multiplikator = "1";
              
            }
        }
        public void LoadWG()
        {
            listView1.Items.Clear();
            int id = 0;
            string Name ="";
            ListViewGroup wgg = new ListViewGroup();
            foreach (ClassPLU.WG wg in ClassPLU.WG.ListofWGs)
            {
                if (wg.Name == comboBox1.Text)
                {
                    id = wg.Id;
                    Name = wg.Name;
                    break;
                }
            }
            wgg.Name = Name;
            wgg.Header = Name;
            listView1.Groups.Add(wgg);
            foreach (ClassPLU.PLU plu in ClassPLU.PLU.ListofPLUs)
            {
                if(plu.Wg == id)
                    listView1.Items.Add(new ListViewItem(plu.BezB,"folder_blue_coffee.png",wgg));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWG();
        }
        
        private void button17_Click(object sender, EventArgs e)
        {
            if (optab == null)
            {
                if (lbl_input.Text != "-")
                {
                    OPTA(lbl_input.Text);
                    
                    if (optab.ErrorCode == 1001)
                    {
                        Log.Add("System-Error-P1", "The Table "+lbl_input.Text+" did not exsist");
                        optab = null;
                        Reset();
                    }
                    lbl_input.Text = "-";
                }
                else
                {
                    new frm_OT().ShowDialog();
                }
            }
            else
            {
                optab.SaveTable();
                optab = null;
                Reset();
            }
        }
        void OPTA(string number)
        {
            lbl_table.Text += number;
            Table.User = ActivUser.Name;
            optab = Table.LoadTable(int.Parse(number));
            optab.OPTable = true;
            optab.username = ActivUser.Name;
            optab.GetArticle(listBox1);
            lbl_m.Text = "TOTALL: "+optab.GetTotallMoney();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            if (optab != null)
            {
                new frm_DIV(frm_DIV.DIVTYP.Barrage, optab).ShowDialog();
                listBox1.Items.Clear();
                optab.GetArticle(listBox1);
                lbl_m.Text = "TOTALL: " + optab.GetTotallMoney();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")            
                lbl_input.Text += "1";
            else
                lbl_input.Text = "1";
        }
        private void button19_Click(object sender, EventArgs e)
        {
            if (optab != null&&lbl_input.Text != "-")
            {
                optab.AddPLU(int.Parse(lbl_input.Text),Multiplikator);
                listBox1.Items.Clear();
                optab.GetArticle(listBox1);
                lbl_m.Text = "TOTALL: " + optab.GetTotallMoney();
                lbl_input.Text = "-";
                Multiplikator = "1";
           
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "2";
            else
                lbl_input.Text = "2";
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "3";
            else
                lbl_input.Text = "3";
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "4";
            else
                lbl_input.Text = "4";
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "5";
            else
                lbl_input.Text = "5";
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "6";
            else
                lbl_input.Text = "6";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "7";
            else
                lbl_input.Text = "7";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "8";
            else
                lbl_input.Text = "8";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "9";
            else
                lbl_input.Text = "9";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += ",";
            else
                lbl_input.Text = ",";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
                lbl_input.Text += "0";
            else
                lbl_input.Text = "0";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text == "-")
            {
                lbl_x.Text = "X: 1";
                Multiplikator = "1";
            }
            else
                lbl_input.Text = "-";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (optab != null)
            {
                new frm_DIV(frm_DIV.DIVTYP.Food, optab).ShowDialog();
                listBox1.Items.Clear();
                optab.GetArticle(listBox1);
                lbl_m.Text = "TOTALL: " + optab.GetTotallMoney();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if(lbl_input.Text != "-")
            {
                Multiplikator = lbl_input.Text;
                lbl_x.Text ="X: " + Multiplikator;
                lbl_input.Text = "-";
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (optab == null)
            {
                PRINT.PUTDATA(ActivUser.Name, ActivUser.MINF, ActivUser.MINB);
                PRINT.Print(PRINT.BILLTYP.USALDO);
            }
            else
            {
                optab.SaveTable();
                optab.Pay(PRINT.BILLTYP.SALDO, false,ActivUser.Name);
                optab = null;
                Reset();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if(optab != null)
            {
                optab.SaveTable();
                frm_BILL bill = new frm_BILL(optab,ActivUser);
                bill.ShowDialog();
                //User
                double FO = 0;
                double BA = 0;
                foreach (int i in optab.Articles)
                {
                    PLU tplu = PLU.PLULoad(i);
                    if (tplu.Food == true)
                    {
                        //FOOD
                        FO += tplu.Price;
                    }
                    else
                    {
                        //BARRAGE
                        BA += tplu.Price;
                    }
                }
                foreach (DIV div in optab.Divers)
                {
                    if (div.Food == true)
                    {
                        //FOOD
                        FO += div.Price;
                    }
                    else
                    {
                        //BARRAGE
                        BA += div.Price;
                    }
                }

                ActivUser.MINF += FO;
                ActivUser.MINB += BA;
                optab.Pay(bill.typ, bill.bw,ActivUser.Name);
                optab = null;
                Reset();                
            }

        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (optab != null)
            {
                optab.SaveTable();
                new frm_Storno().Storno(optab,ActivUser);
                optab = null;
                Reset();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (lbl_input.Text != "-")
            {
                Table.RETable(Convert.ToInt32(lbl_input.Text),ActivUser.Name);
                foreach (Table tab in Table.ListofTables)
                {
                    if (tab.ID == Convert.ToInt32(lbl_input.Text))
                    {
                        if (tab.username == ActivUser.Name)
                        {
                            //
                            switch (tab.be_typ)
                            {
                                case PRINT.BILLTYP.BAR:
                                    ActivUser.BAR -= tab.OldTotalM;
                                    break;
                                case PRINT.BILLTYP.EC:
                                    ActivUser.EC -= tab.OldTotalM;
                                    break;
                                case PRINT.BILLTYP.KREDIT:
                                    ActivUser.CC -= tab.OldTotalM;
                                    break;
                                case PRINT.BILLTYP.HOTEL:
                                    ActivUser.HOTEL -= tab.OldTotalM;
                                    break;
                            }
                            //Articles und DIV haupt abziehen;
                            foreach (int i in tab.OldArticles)
                            {
                                PLU akt = PLU.PLULoad(i);
                                if (akt.Food == true)
                                {
                                    ActivUser.MINF -= akt.Price;
                                }
                                else
                                {
                                    ActivUser.MINB -= akt.Price;
                                }
                            }
                            foreach(DIV div in tab.OldDivers)
                            {
                                if (div.Food == true)
                                {
                                    ActivUser.MINF -= div.Price;
                                }
                                else
                                {
                                    ActivUser.MINB -= div.Price;
                                }
                            }
                        }
                    }
                }
                lbl_input.Text = "-";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            Logout();
        }
    }
}
