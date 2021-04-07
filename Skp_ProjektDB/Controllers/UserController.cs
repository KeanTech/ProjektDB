using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skp_ProjektDB.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private Backend.Managers.Db db = new Backend.Managers.Db();
        private Backend.Managers.Security security = new Backend.Managers.Security();
        private static User logedInUser;

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
            db.SetConnection(configuration.GetConnectionString("Test"));
        }

        // login needs to handle failed logins
        public IActionResult UserLogin(string loginName, string password)
        {
            if (loginName == null)
                return BadRequest("Login oplysningerne var ikke korrekt");
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
                        logedInUser = db.GetUser(loginName);
                        logedInUser = db.GetUserRoles(logedInUser);
                        if (logedInUser.UserRoles.Contains(Skp_ProjektDB.Models.User.Roles.Instruktør))
                            return Redirect("/Project/ProjectOverView");
                        else
                            return Redirect("/User/UserOverView");
                    }
                    else
                    {
                        // password is wrong
                        return BadRequest("Login oplysningerne var ikke korrekt");
                    }
                }
                else
                {
                    // username is incorrect!
                    return BadRequest("Login oplysningerne var ikke korrekt");
                }
            }
        }

        /// <summary>
        /// Makes a detailed view of all users in db
        /// </summary>
        /// <returns></returns>
        public IActionResult UserOverView()
        {
            if (logedInUser != null)
            {
                //returns a list of User models
                List<User> users = db.GetAllUsers();

                users.Where(x => x.Login == logedInUser.Login).Select(x => x.Owner = users.IndexOf(x));
                if (logedInUser.UserRoles.Contains(Skp_ProjektDB.Models.User.Roles.Instruktør))
                {
                    users.Where(x => x.Login == logedInUser.Login).Select(x => x.Admin = true);
                }

                return View(users);
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        public IActionResult SingleUserView(string userName)
        {
            User user = null;
            if (userName != null)
            {
                user = db.GetAllUsers().Where(x => x.Login == userName).FirstOrDefault();
                
                var projects = db.GetAllProjects();
                foreach (var item in projects)
                {
                    item.Team = db.GetTeam(item.Id);
                }

            }

            if (logedInUser != null && !string.IsNullOrEmpty(logedInUser.Login))
            {
                if (user.UserRoles.Contains(Skp_ProjektDB.Models.User.Roles.Instruktør))
                {
                    user.Admin = true;
                    return View(user);
                }
                else if (user.Login == logedInUser.Login)
                {
                    user.Owner = 1;
                    return View(user);
                }
                else
                {
                    return View(user);
                }
            }
            else
            {
                return BadRequest("Du er ikke logget ind");
            }
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
        /// This is used to update user data
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateUser(string userName)
        {
            //This method will only be available to Admin or the user that owns the account 
            var users = db.GetAllUsers();
            if (logedInUser != null)
            {
                foreach (var userInList in users)
                {
                    if (userInList.Login == userName)
                    {
                        userInList.Owner = 1;
                        return View(userInList);
                    }
                    else if (userName == userInList.Login)
                    {
                        return View(db.GetUser(userName));
                    }
                }
                return BadRequest("Fandt ikke noget!");
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        //-------------------------------------------------------------- vv Admin only views vv

        /// <summary>
        /// This is used to save newly created users
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateUser(User user)
        {
            if (logedInUser != null)
            {
                if (user.Name == null)
                {
                    user.Admin = true;
                    return View(user);
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
            else
                return BadRequest("Du er ikke logget ind");
        }

        [HttpGet]
        /// <summary>
        /// This is used to delete users from Db
        /// </summary>
        /// <returns></returns>
        public IActionResult DeleteUser(User user)
        {
            if (logedInUser != null)
            {
                user.Admin = true;
                return View(user);
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        [HttpPost]
        public IActionResult DeleteUser(string username)
        {
            if (logedInUser != null)
            {
                db.DeleteUser(db.GetUser(username));
                return Redirect("/User/UserOverView");
            }
            else
                return BadRequest("Du er ikke loget ind");
        }

        public IActionResult AddRoleToUser(string userName, string role)
        {
            if (logedInUser != null)
            {
                User user = db.GetUser(userName);
                user = db.GetUserRoles(user);
                if (string.IsNullOrEmpty(role))
                {
                    user.Admin = true;
                    return View(user);
                }
                else
                {
                    //Save role to db
                    foreach (User.Roles roles in (User.Roles[])Enum.GetValues(typeof(User.Roles)))
                    {
                        if (role == roles.ToString())
                            user.UserRoles.Add(roles);
                    }

                    db.AddRoleToUser(user);
                    return View("SingleUserView", user);
                }
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        public IActionResult RemoveRoleFromUser() 
        {
            return View();   
        }
    }
}
