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
    public partial class frm_splashscreen : Form
    {
        public frm_splashscreen()
        {
            InitializeComponent();
            ReadVersion();
        }

        void ReadVersion()
        {
            string info;
            info = "A PC cash register developed by the Declavis Company."+ "\r\n"+"Support under Github: DeclavisLab";
            info += "\r\n\r\nRegistered for:\r\n\r\nGithub\r\n\r\n\r\n\r\n";
            info += "Version:\r\n\r\nMain Program: " + Startup.Version.ToString();

            textBox1.Text = info;
            button1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frm_Login().Show();
        }
    }
}
