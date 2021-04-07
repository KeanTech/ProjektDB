using Skp_ProjektDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Backend.Interfaces
{
    interface ILogIn
    {
        void LogIn(User user);
        void LogOut(User user);
    }
}
