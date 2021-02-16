using Microsoft.AspNetCore.Mvc;
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
        public IActionResult UserLogin(string loginName, string password)
        {
            if (loginName == null)
                return BadRequest();
            else
            {

                //Make login here !!

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
        public void CreateUser()
        {
            //Save user in Db
        }

        /// <summary>
        /// This is used to get user from Db
        /// </summary>
        /// <returns></returns>
        public User ReadUser(int id)
        {
            return new User("", "", new List<Types.Roles>());
        }

        /// <summary>
        /// This is used to get all users from Db
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAllUsers() //Made static for testing
        {
            //Setup DbManager here and return the full list of users
            List<User> users = new List<User>() {
                new User("Martin", "mar221", new List<Types.Roles>() { Types.Roles.Udvikler }) { Competence = "H2" },
                new User("Kenneth", "kean513", new List<Types.Roles>(){ Types.Roles.Projektleder, Types.Roles.Udvikler }) { Competence = "H2" },
                new User("Emil", "Emi1213", new List<Types.Roles>(){ Types.Roles.Udvikler }) { Competence = "H2" }
            };

            users[0].Projects.Add((Project)ProjectController.GetProjects()[0]);
            users[1].Projects.Add((Project)ProjectController.GetProjects()[0]);
            users[2].Projects.Add((Project)ProjectController.GetProjects()[0]);

            return users;
        }

        /// <summary>
        /// This is used to update user data
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateUser()
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
