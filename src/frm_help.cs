using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LoadManager;
namespace Interface
{
    public partial class frm_help : Form
    {
        public frm_help()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            webBrowser1.Navigate(LoadManager.Main.SYSTEMPATH +"help\\index.htm");
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //Startup.MsgBox(e.Url.ToString());
            if (e.Url.ToString().Contains("close.form"))
            {
                this.Close();
            }
        }
    }
}
