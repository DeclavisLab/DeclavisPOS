using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassPLU
{
    public class WG
    {
        public int Id;
        public string Name;

        public static List<WG> ListofWGs = new List<WG>();

        public WG(int _Id, string _Name)
        {
            Id=_Id;
            Name = _Name;
        }
    }
}
