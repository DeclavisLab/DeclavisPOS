using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace Interface
{    
    class Startup
    {
        public const string Version= "2.4";
        public const bool Debug = true;
        //frm_update Bash Datei
        public const string Server = null;
        const string SVer = "now.v";
                            
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                MsgBox(args[0]);
                if (args[0].Contains("-u"))
                {
                    CVersion(true);
                }
                else { CVersion(false); }
            }catch(Exception e)
            {
                CVersion(false);
            }
        }
               
        
        static void CVersion(bool a)
        {
            /*try
            {
                WebClient wc = new WebClient();
                Stream ws = wc.OpenRead(Server + SVer);
                StreamReader sr = new StreamReader(ws);
                
                string v = sr.ReadToEnd();
                MsgBox(v);
                ws.Close();
                sr.Close();                
                if (Version == v || Debug == true)
                {
                    if (a)
                        Update(v);
                    else
                        Start();
                }
                else
                {
                    Update(v);
                }
            }
            catch { Start(); }*/
            Start();
        }

        public static void MsgBox(string info)
        {
            if(Debug)
                MessageBox.Show(info);
        }
        static void Start()
        {
            Application.Run(new frm_splashscreen());
        }
        static void Update(string ver)
        {
            Application.Run(new frm_update(ver));
        }
    }
}
