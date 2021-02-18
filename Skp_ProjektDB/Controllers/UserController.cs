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

        public IActionResult UserLogin(string loginName, string password)
        {
            if (loginName == null)
                return BadRequest();
            else
            {
                //Get connection string 
                //configuration.GetConnectionString("SkpDb");

                //Make login here !!
                byte[] salt = db.GetSalt(loginName);
                if (salt != null)
                {
                    string encrypted = security.Encrypt(Encoding.UTF8.GetBytes(password), salt);
                    string hash = security.Hash(Encoding.UTF8.GetBytes(encrypted));

                    if (hash == db.GetHash(loginName)) // if hashes matches == password is correct
                    {
                        // gets the user who logged in
                        User user = db.GetUser(loginName);
                    }
                    else
                    {
                        // password is wrong
                    }
                }
                else
                {
                    // username is incorrect!
                }

                return Redirect("/Project/ProjectOverView");
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
            return View(db.GetUser(userName.Split('@')[0]));
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
                // GENERATE password for created users
                user.Salt = security.GenerateSalt();
                user.Hash = security.Hash(Encoding.UTF8.GetBytes(user.Salt));
                
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
            return View(user = new Models.User() { Name = "Kenneth", Login = "kenn229k", Competence = "H2" });
        }

        [HttpPost]
        public IActionResult DeleteUser(string username)
        {
            db.DeleteUser(db.GetUser(username));
            return Redirect("/User/UserOverView");
        }
    }
}
