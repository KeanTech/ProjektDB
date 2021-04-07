using Skp_ProjektDB.Backend.Interfaces;
using Skp_ProjektDB.Models;
using System;

namespace Skp_ProjektDB.Backend.Managers
{

    // This class is experimental needs some testing !
    public class LogIn : ILogIn
    {
        private readonly Db db;

        public LogIn(Db db)
        {
            this.db = db;
        }

        public int UserID { get; set; }
        public bool LogedIn { get; set; }
        public string Identity { get; set; }

        void ILogIn.LogIn(User user)
        {
            UserID = user.Id;
            LogedIn = true;
            db.UserLogIn(user.Login);
        }

        public void LogOut(User user)
        {
            LogedIn = false;
            db.UserLogOut(user.Login);            
        }

        //Remember to test the login, so that a user only can be logged in on one device at a time 
        public void CheckLogIn(string identity, User user)
        {
            
            if (identity == "GetFromDB")
            {
                
            }
        }
    }
}
