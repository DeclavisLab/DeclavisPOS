using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ClassPLU;
namespace Interface
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadListBox();
            button3.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad",LoadManager.Main.SYSTEMPATH+"Config.ini");
        }
        void LoadListBox()
        {
            listBox1.Items.Clear();
            PLU.PLUDATALoad();
            foreach (PLU plu in PLU.ListofPLUs)
            {
                listBox1.Items.Add(plu.ID + ": " + plu.BezM + " ->" + PLU.PreisUmform3(plu.Price) + " pro " + plu.Einheit);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string[] data = listBox1.SelectedItem.ToString().Split(new char[] { ':' });
                PLU.PLUDelete(data[0]);
                LoadListBox();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string[] data = listBox1.SelectedItem.ToString().Split(new char[] { ':' });
                button2.Enabled = false;
                button3.Enabled = true;
                PLU chplu = PLU.PLULoad(int.Parse(data[0]));
                textBox1.Text = chplu.ID.ToString(); ;
                textBox2.Text = chplu.BezM;
                textBox3.Text = PLU.PreisUmform3(chplu.Price);
                textBox4.Text = chplu.Wg.ToString();
                textBox5.Text = chplu.Einheit;
                if (chplu.Food)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;

            }
        }

        void ClearEdit()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            
            PLU chplu = new PLU(int.Parse(textBox1.Text),double.Parse(textBox3.Text),textBox2.Text,19,int.Parse(textBox4.Text),radioButton1.Checked,textBox5.Text);
            string[] data = listBox1.SelectedItem.ToString().Split(new char[] { ':' });
            PLU.PLUUpdate(chplu,data[0]);
            LoadListBox();
            ClearEdit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PLU nwplu = new PLU(int.Parse(textBox1.Text),double.Parse(textBox3.Text),textBox2.Text,19,int.Parse(textBox4.Text),radioButton1.Checked,textBox5.Text);
            if(PLU.PLULoad(nwplu.ID).BezM != "Error")
            {
                //Error
                Startup.MsgBox("Error");
            }
            else { PLU.PLUInsert(nwplu);}
            LoadListBox();
            ClearEdit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new SaveManager.frm_Export().ShowDialog();
        }
    }
}
