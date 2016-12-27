using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interface
{
    public class User
    {
        public string Name;
        public bool Key;
        public Keys LK;
        public string PW;
        public double MINB;
        public double MINF;
        public List<ClassPLU.PLU> STPLU = new List<ClassPLU.PLU>();
        public List<ClassPLU.DIV> STDIV = new List<ClassPLU.DIV>();
        public double BAR = 0;
        public double EC = 0;
        public double HOTEL = 0;
        public double CC = 0;

        public static List<User> ListofUsers = new List<User>();
        public static User ADMIN = new User("Admin", false, Keys.D1, LoadManager.Main.ADMINPW);
        public User(string _Name,bool _Key,Keys _LK,string _PW)
        {
            Name = _Name;
            Key = _Key;
            LK = _LK;
            PW = _PW;
            MINB = 0;
            MINF = 0;
        }

        public static void Create_User()
        {
            
            ListofUsers.Add(ADMIN);
            int AmountU = LoadManager.Main.AmountUser;
            User U1 = new User(LoadManager.Main.UserName(1),true,Keys.Q,"AAAAA");
            ListofUsers.Add(U1);
            if(AmountU >= 2)
            {
                User U2 = new User(LoadManager.Main.UserName(2),true,Keys.W,"BBBBB");
                ListofUsers.Add(U2);
            }
            if (AmountU >= 3)
            {
                User U3 = new User(LoadManager.Main.UserName(3), true, Keys.E, "CCCCC");
                ListofUsers.Add(U3);
            }
            if (AmountU >= 4)
            {
                for (int i = 4; i < AmountU; i++)
                {
                    User UU = new User(LoadManager.Main.UserName(i),false,Keys.D1,LoadManager.Main.UserPW(i));
                    ListofUsers.Add(UU);
                }
            }
            
        }
    }
}
