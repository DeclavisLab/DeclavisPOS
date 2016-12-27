using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Ionic.Zip;
using System.IO;
using System.Runtime.InteropServices;
namespace Interface
{
    public partial class frm_update : Form
    {
        [DllImport("shell32.dll",EntryPoint = "ShellExecute")]
        public static extern long ShellExecute(int hwnd,string cmd,string file,string param1,string param2,int swmode);

        string ver;
        public frm_update(string v)
        {
            InitializeComponent();
            ver = v;
            string url = Startup.Server + "changelog.php?ver="+ v;
            Startup.MsgBox(v);
            Startup.MsgBox(url);
            webBrowser1.Navigate(url);
            if (Application.StartupPath != @"C:\DeclavisCompany\Kassensystem")
                button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();            
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            wc.DownloadFileAsync(new Uri(Startup.Server+"update"+ver+".zip"), Application.StartupPath + "\\update.zip");
            button1.Enabled = false;
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
            using (ZipFile zip = ZipFile.Read(Application.StartupPath + @"\update.zip"))
            {
                // This call to ExtractAll() assumes:
                //   - none of the entries are password-protected.
                //   - want to extract all entries to current working directory
                //   - none of the files in the zip already exist in the directory;
                //     if they do, the method will throw.
                if(!(Directory.Exists(Application.StartupPath + @"\new\")))
                    Directory.CreateDirectory(Application.StartupPath + @"\new\");
                zip.Password = "1003-3774-1514-2116";
                zip.ExtractAll(Application.StartupPath + @"\new\");
            }
            String Com;
            var os = System.Environment.OSVersion;
            bool isWindowsXp = (os.Version.Major == 5 && os.Version.Minor == 1);
            if(isWindowsXp)
                Com = "@echo OFF\nping 127.0.0.1 -n 5>nul\nxcopy /Y C:\\DeclavisCompany\\Kassensystem\\New\\*.* C:\\DeclavisCompany\\Kassensystem\\\ndel C:\\DeclavisCompany\\Kassensystem\\New\\*.* /s /q\nstart C:\\DeclavisCompany\\Kassensystem\\Interface.exe\ndel %0";
            else
                Com = "@echo OFF\ntimeout /T 5 /nobreak\nxcopy /Y C:\\DeclavisCompany\\Kassensystem\\New\\*.* C:\\DeclavisCompany\\Kassensystem\\\ndel C:\\DeclavisCompany\\Kassensystem\\New\\*.* /s /q\nstart C:\\DeclavisCompany\\Kassensystem\\Interface.exe\ndel %0";
            
            File.WriteAllText("ex.bat", Com);
            ShellExecute(0, "open", Application.StartupPath + "/ex.bat", "", "", 5);
            Application.Exit();
        }
    }
}
