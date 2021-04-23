using Skp_ProjektDB.Backend.Interfaces;
using Skp_ProjektDB.Models;
using System;

namespace Skp_ProjektDB.Backend.Managers
{

    // This class is experimental needs some testing !
    public class UserLogin : ILogIn
    {
        private readonly Db db;

        public UserLogin(Db db)
        {
            this.db = db;
            this.User = new User();
        }

        public User User { get; set; }
        public bool LogedIn { get; set; }
        public string Identity { get; set; }

        public void LogIn(User user, string identity)
        {
            User = user;
            LogedIn = true;
            db.UserLogIn(user.Login, identity);
        }

        public void LogOut(User user, string Identity)
        {
            LogedIn = false;
            db.UserLogOut(user.Login, Identity);            
        }

        //Remember to test the login, so that a user only can be logged in on one device at a time 
        public void CheckLogIn(User user, string identity)
        {
            
            if (identity == "GetFromDB")
            {
                
            }
        }
    }
}
