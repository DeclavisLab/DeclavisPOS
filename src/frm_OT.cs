using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassTable;

namespace Interface
{
    public partial class frm_OT : Form
    {
        public frm_OT()
        {
            InitializeComponent();
            LoadTable();
        }

        void LoadTable()
        {
            foreach (Table tb in Table.ListofTables)
            {
                if (tb.OPTable == true)
                {
                    listBox1.Items.Add("Tisch: "+ tb.ID + " - "+ ClassPLU.PLU.PreisUmform(tb.TotalM.ToString()));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
