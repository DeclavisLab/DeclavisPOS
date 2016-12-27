using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using ClassPLU;
using System.IO;
using SaveManager;
namespace Interface
{
    public partial class frm_Login : Form
    {
        public static bool closeable = false;
        
        public frm_Login()
        {
            InitializeComponent();
            groupBox2.Enabled = false;
            LoadManager.Main.Intizialize();
            ClassPLU.PLU.PLUDATALoad();
            User.Create_User();
            Log.Start();
            Log.Add("System-Info", "System started");
            AddInsetUSBHandler();
            if (Startup.Debug)
            {
                button15.Visible = true;
                closeable = true;
            }
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void frm_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeable != true)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            groupBox2.Enabled = true;            
        }
        #region Eingabe
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
        #endregion

        private void button11_Click(object sender, EventArgs e)
        {
            User my = null;
            string input = textBox1.Text;
            textBox1.Clear();
            foreach (User user in User.ListofUsers)
            {
                if (user.PW == input)
                {
                    my = user;
                    break;
                }
            }
            if (my != null)
            {
                new Form1().User(my);
                groupBox2.Enabled = false;
            }
        }

        private void frm_Login_KeyUp(object sender, KeyEventArgs e)
        {
            User my = null;
            Keys prin = e.KeyCode;
            foreach (User user in User.ListofUsers)
            {
                if (user.Key == true && user.LK == prin)
                {
                    my = user;
                    break;
                }
            }
            if (my != null)
            {
                new Form1().User(my);
            }
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            User my = null;
            Keys prin = e.KeyCode;
            foreach (User user in User.ListofUsers)
            {
                if (user.Key == true && user.LK == prin)
                {
                    my = user;
                    break;
                }
            }
            if (my != null)
            {
                new Form1().User(my);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Log.Add("System-Info", "System closed");
            closeable = true;
            if(!Startup.Debug)
                Process.Start("shutdown.exe","-s -f -t 100");
            Application.Exit();
        }

        ManagementEventWatcher w = null;


        private void AddInsetUSBHandler()
        {
           /* WqlEventQuery q;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            try
            {
                q = new WqlEventQuery();
                q.EventClassName = "__InstanceCreationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = @"TargetInstance ISA 'Win32_USBHub'";
                w = new ManagementEventWatcher(scope, q);
                w.EventArrived += new EventArrivedEventHandler(USBAdded);
                w.Start();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                if (w != null)
                    w.Stop();
            }*/
        }
        void USBAdded(object sender, EventArgs e)
        {
            Process.Start(@"explorer.exe","/e, ::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == LoadManager.Main.ZPW)
            {
                Log.Add("System-Info","Z-Abschlag started");
                string text = "";
                double TotalMA = 0;
                double fo = 0;
                double ba = 0;
                double st = 0;
                int c = 0;
                foreach (User user in User.ListofUsers)
                {
                    c++;
                    double FOOD = user.MINF;
                    fo += FOOD;
                    double BARRAGE = user.MINB;
                    ba += BARRAGE;
                    double TOTAL = FOOD + BARRAGE;
                    TotalMA += TOTAL;
                    double BAR = user.BAR;
                    double HOTEL = user.HOTEL;
                    double CC = user.CC;
                    double EC = user.EC;
                    double Mwst = Math.Round((TOTAL / 119) * 19, 2);
                    double storno = 0;
                    foreach (DIV div in user.STDIV)
                    {
                        storno += div.Price;
                    }
                    foreach (PLU plu in user.STPLU)
                    {
                        storno += plu.Price;
                    }
                    st += storno;
                    text += "\n\nUser: " + user.Name + "\nKüche: " + PLU.PreisUmform(FOOD.ToString()) + "\nTheke: " + PLU.PreisUmform(BARRAGE.ToString()) + "\nTotal: " + PLU.PreisUmform(TOTAL.ToString()) + "\nStorno: " + PLU.PreisUmform(storno.ToString()) + "\nBAR: " + PLU.PreisUmform(BAR.ToString()) + "\nEC: " + PLU.PreisUmform(EC.ToString()) + "\nCC: " + PLU.PreisUmform(CC.ToString()) + "\nHOTEL-RE: " + PLU.PreisUmform(HOTEL.ToString()) + "\nMwst: (19%) " + PLU.PreisUmform(Mwst.ToString());
                }
                SaveManager.Main.ADDZAB(fo, ba, fo + ba, Math.Round(((fo + ba) / 119) * 100, 2), Math.Round(((fo + ba) / 119) * 19, 2), st);
                ClassTable.PRINT.PUTDATA(User.ADMIN.Name, c, TotalMA, text);
                ClassTable.PRINT.Print(ClassTable.PRINT.BILLTYP.ZAB);
                ClassTable.Table.DeleteTableDBEntry();
                ClassTable.Table.CreateTableDBEntry(); 
                closeable = true;
                Application.Restart();
            }
            else
                textBox1.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            /*Log.Add("System-Info", "Opening Feature DPLN");
            Process.Start(@"C:\DPLN\Dienstplan.exe");*/
        }

        private void button15_Click(object sender, EventArgs e)
        {            
            ClassTable.Table.CreateTableDBEntry();
            MessageBox.Show("");
            ClassTable.Table.DeleteTableDBEntry();
        }

        private void button23_Click(object sender, EventArgs e)
        {    
            if (textBox1.Text == LoadManager.Main.ADMINPW)
            {
                Log.Add("System-Info", "Opened the AdminMode");
                textBox1.Clear();
                new Form2().ShowDialog();
            }
            else
                textBox1.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            new frm_help().ShowDialog();
        }
    }
}
