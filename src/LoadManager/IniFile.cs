using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LoadManager
{
    public class IniFile
    {
        private string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);

        
        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public Section this[string Section]
        {
            get { return new Section(Section, this); }
        }

        public class Section
        {
            private string Name;
            private IniFile File;

            public Section(string name, IniFile file)
            {
                Name = name;
                File = file;
            }

            public string this[string Key]
            {
                get { return File.IniReadValue(Name, Key); }
                set { if (value != this[Key]) File.IniWriteValue(Name, Key, value); }
            }
        }

        protected void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        protected string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }
}