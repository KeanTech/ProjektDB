using Microsoft.AspNetCore.Mvc;
using Skp_ProjektDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Controllers
{
    public class ProjectController : Controller
    {
        /// <summary>
        /// This is a single view of a prjects, that a user selects from another view.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IActionResult SingleProjectView(string projectName)
        {
            var project = GetProjects().Where(x => x.Title == projectName).FirstOrDefault();
            project.Team = UserController.GetAllUsers();
            return View(project);
        }

        /// <summary>
        /// This is the view of all the projects in the db
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectOverView()
        {
            var users = UserController.GetAllUsers();
            var projects = GetProjects();
            projects[0].Team = users;

            return View(projects);
        }

        /// <summary>
        /// Used to get the AddToLog view.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IActionResult WriteToLog(int projectId)
        {
            return View(GetProjects().Where(x => x.Id == projectId).FirstOrDefault());
        }

        public IActionResult AddToLog(string logString, int projectId)
        {
            var x = User.Identity.IsAuthenticated;
            //Save the log to db
            //add username to logstring 
            var project = GetProjects().Where(x => x.Id == projectId).FirstOrDefault();
            project.Log.Add(logString);
            return Redirect("/Project/ProjectOverView");
        }

        public IActionResult ProjectSearch(string projectName)
        {
            if (projectName != null)
            {
                List<Project> searchedProject = GetProjects().FindAll(x => x.Title.ToLower().Contains(projectName.ToLower()));

                if (searchedProject != null)
                    return View("ProjectOverView", searchedProject);
                else
                    return View("ProjectOverView", GetProjects());
            }
            else
            {
                return View("ProjectOverView", GetProjects());
            }


        }

        //-----------------------------------------------------CRUD Methods

        public void CreateProject(Project project)
        {
            Redirect("/HomePage");
        }

        public Project GetProject()
        {
            return null;
        }

        public static List<Project> GetProjects() //Made static for testing
        {
            return new List<Project>() {
                new Project(
                    "SkpProjektDB",
                    "Dette er en test på en projektbeskrivelse",
                    new List<string>(){ "Kenneth: Jeg er igang med frontend, User overview er done", "Martin: Jeg er i gang med backend all good" },
                    DateTime.Now.Date,
                    DateTime.Now.Date.AddDays(60),
                    new Models.User("Kenneth","kean513",new List<Types.Roles>() { Types.Roles.Projektleder, Types.Roles.Udvikler }),
                    new List<User>()
                        ),
                new Project(
                    "SkpUdLån",
                    "Hvis bare",
                    new List<string>(){ "Line: Ønske om mere info!" },
                    DateTime.Now.Date,
                    DateTime.Now.Date,
                    new User("Jesper", "jkd431", new List<Types.Roles>(){ Types.Roles.Projektleder }),
                    new List<User>()
                    )
            };
        }
    }
}
