using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Skp_ProjektDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
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

                return Redirect("/Project/ProjectOverView");
            }
        }

        public static List<User> GetAllUsers()
        {
            return new List<User>();
        }

        /// <summary>
        /// Makes a detailed view of all users in db
        /// </summary>
        /// <returns></returns>
        public IActionResult UserOverView()
        {
            //returns a list of User models
            return View(GetAllUsers());
        }

        public IActionResult SingleUserView(string userName)
        {
            return View(GetAllUsers().Where(x => x.Name == userName).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult UserSearch(string searchWord)
        {
            List<User> users;
            if (searchWord == null)
            {
                users = GetAllUsers();
            }
            else
            {
                users = GetAllUsers().FindAll(x => x.Name.ToLower().Contains(searchWord.ToLower()));
            }
            return View("UserOverView", users);
        }

        //------------------------------------------------------------CRUD methods
        
        /// <summary>
        /// This is used to save newly created users
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateUser(User user)
        {
            if(user.Name == null)
            {
                return View();
            }
            else
            {
                return Redirect("/User/UserOverView");
            }
        }


        /// <summary>
        /// This is used to update user data
        /// </summary>
        /// <returns></returns>
        public IActionResult EditUser()
        {
            return View();
        }

        /// <summary>
        /// This is used to delete users from Db
        /// </summary>
        /// <returns></returns>
        public IActionResult DeleteUser()
        {
            return View();
        }
    }
}
