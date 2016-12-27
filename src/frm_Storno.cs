using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassPLU;
using ClassTable;

namespace Interface
{
    public partial class frm_Storno : Form
    {
        Table tab;
        List<DIV> lDIV = new List<DIV>();
        List<PLU> lPLU = new List<PLU>();
        User Acc;
        public frm_Storno()
        {
            InitializeComponent();
        }

        public void Storno(Table table,User user)
        {
            this.Text = "Storno: " + table.ID;
            tab = table;
            Acc = user;
            base.ShowDialog();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (radioButton1.Checked == true)
            {
                foreach (DIV div in tab.Divers)
                {
                    listBox1.Items.Add(div.Bez + " - " + PLU.PreisUmform(div.Price.ToString()));
                    lDIV.Add(div);
                }
            }
            else
            {
                foreach (int i in tab.Articles)
                {
                    PLU plu = PLU.PLULoad(i);
                    listBox1.Items.Add(plu.BezB+" - "+PLU.PreisUmform(plu.Price.ToString()));
                    lPLU.Add(plu);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (radioButton1.Checked == true)
                {
                    DIV tdiv = null;
                    foreach (DIV div in tab.Divers)
                    {
                        if (div.Bez == lDIV[listBox1.SelectedIndex].Bez)
                        {
                            tdiv = div;
                        }
                    }
                    if (tdiv != null)
                    {
                        Acc.STDIV.Add(tdiv);
                        tab.StornoDivers(tdiv);
                        tab.SaveTable();
                    }
                }
                else
                {
                    Acc.STPLU.Add(PLU.PLULoad(lPLU[listBox1.SelectedIndex].ID));
                    tab.StornoPLU(PLU.PLULoad(lPLU[listBox1.SelectedIndex].ID).ID);
                    tab.SaveTable();
                }
                this.Close();
            }
        }
    }
}
