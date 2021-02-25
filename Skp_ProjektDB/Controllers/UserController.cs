using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private Backend.Managers.Db db = new Backend.Managers.Db();
        private Backend.Managers.Security security = new Backend.Managers.Security();

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
            db.SetConnection(configuration.GetConnectionString("SkpDb"));
        }

        // login needs to handle failed logins
        public IActionResult UserLogin(string loginName, string password)
        {
            if (loginName == null)
                return BadRequest();
            else
            {
                // gets salt if username exists
                string salt = db.GetSalt(loginName);
                if (salt != null)
                {
                    // gets the hash value of typed password
                    string encrypted = security.Encrypt(Encoding.UTF8.GetBytes(password), Convert.FromBase64String(salt));
                    string hash = security.Hash(Convert.FromBase64String(encrypted));

                    if (hash == db.GetHash(loginName)) // if hashes matches == password is correct
                    {
                        // gets the user who logged in
                        User user = db.GetUser(loginName);
                        if (user.Roles.Contains(Roles.Instruktør))
                            return Redirect("/Project/AdminUserSingleView");
                        else
                            return Redirect("/Project/");
                    }
                    else
                    {
                        // password is wrong
                        return BadRequest("Forkert Kode");
                    }
                }
                else
                {
                    // username is incorrect!
                    return BadRequest("Login navn findes ikke");
                }

            }
        }

        /// <summary>
        /// Makes a detailed view of all users in db
        /// </summary>
        /// <returns></returns>
        public IActionResult UserOverView()
        {
            //returns a list of User models
            return View(db.GetAllUsers());
        }

        public IActionResult SingleUserView(string userName)
        {
            return View(db.GetUser(userName));
        }

        [HttpPost]
        public IActionResult UserSearch(string searchWord)
        {
            List<User> users;
            if (searchWord == null)
            {
                users = db.GetAllUsers();
            }
            else
            {
                users = db.GetAllUsers().FindAll(x => x.Name.ToLower().Contains(searchWord.ToLower()));
            }
            return View("UserOverView", users);
        }

        //------------------------------------------------------------ vv CRUD Views vv



        /// <summary>
        /// This is used to get user from Db
        /// </summary>
        /// <returns></returns>
        public User ReadUser(string username)
        {
            return db.GetUser(username);
        }

        /// <summary>
        /// This is used to update user data
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateUser(User user)
        {
            var users = db.GetAllUsers();

            foreach (var userInList in users)
            {
                if (userInList == user)
                {
                    return View(user);
                }
                else
                {
                    return View(db.GetUser(user.Login));
                }
            }

            return View();
        }


        //-------------------------------------------------------------- vv Admin only views vv

        /// <summary>
        /// This is used to save newly created users
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateUser(User user)
        {
            if (user.Name == null)
            {
                return View();
            }
            else
            {
                // create salt for user
                user.Salt = security.GenerateSalt();

                // create random password for user
                Random random = new Random();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    sb.Append(random.Next(0, 10));
                }
                user.Hash = security.Hash(Convert.FromBase64String(security.Encrypt(Encoding.UTF8.GetBytes(sb.ToString()), Convert.FromBase64String(user.Salt))));

                // create user on database
                db.CreateUser(user);
                return Redirect("/User/UserOverView");
            }
        }

        [HttpGet]
        /// <summary>
        /// This is used to delete users from Db
        /// </summary>
        /// <returns></returns>
        public IActionResult DeleteUser(User user)
        {

            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(string username)
        {
            db.DeleteUser(db.GetUser(username));
            return Redirect("/User/UserOverView");
        }
    }
}
