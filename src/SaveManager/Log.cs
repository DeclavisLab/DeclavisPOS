using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SaveManager
{
    public class Log
    {
        static bool Started = false;
        static string file = "log/T[" + DateTime.Today.Date.ToString("dd.MM.yyyy") + "].log";
        public static void Start()
        {
            if (!(Directory.Exists(Path.Combine(Application.StartupPath, "log"))))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "log"));
            }
            Started = true;
        }
        public static void Add(string user,string INFO)
        {
            if (Started)
            {
                string text = "{" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "}[" + user + "]> " + INFO;
                Save(text);
            }
        }
        static void Save(string content)
        {
            if (!(File.Exists(Path.Combine(Application.StartupPath, file))))
                content = "\n" + content;
            StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath,file), true);
            sw.WriteLine(content);
            sw.Close();
        }
    }
}
