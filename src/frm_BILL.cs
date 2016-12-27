using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interface
{
    public partial class frm_BILL : Form
    {
        public bool bw = false;
        public ClassTable.PRINT.BILLTYP typ = ClassTable.PRINT.BILLTYP.BAR;
        public ClassTable.Table  t;
        public User us;
        public frm_BILL(ClassTable.Table ta,User user)
        {
            InitializeComponent();
            t = ta;
            us = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            typ = ClassTable.PRINT.BILLTYP.BAR;
            us.BAR += t.TotalM;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                bw = true;
            }
            else
            {
                bw = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            typ = ClassTable.PRINT.BILLTYP.HOTEL;
            us.HOTEL += t.TotalM;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            typ = ClassTable.PRINT.BILLTYP.KREDIT;
            us.CC += t.TotalM;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            typ = ClassTable.PRINT.BILLTYP.EC;
            us.EC += t.TotalM;
            this.Close();
        }
    }
}
