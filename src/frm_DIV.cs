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
    public partial class frm_DIV : Form
    {
        bool typs = false;
        Table tab;
        public frm_DIV(DIVTYP typ, Table table)
        {
            InitializeComponent();
            if (typ == DIVTYP.Barrage)
            {
                this.Text += " Getränk";
                typs = false;
            }

            if (typ == DIVTYP.Food)
            {
                this.Text += " Küche";
                typs = true;
            }
            label5.Text += Convert.ToString(DIV.ids + 1);
            tab = table;
            comboBox1.Text = this.Text;
        }

        public enum DIVTYP
        {
            Barrage,Food
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DIV.ids++;
            DIV DIVERS = new DIV(DIV.ids, PLU.PreisUmform2(textBox1.Text), 0.19, comboBox1.Text, typs);
            tab.AddDivers(DIVERS);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += "2";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += "3";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += "4";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += "5";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += "6";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += "7";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += "8";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += "9";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += "0";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text += ",";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }


       
    }
}
