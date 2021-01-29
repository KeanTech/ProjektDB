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

        public IActionResult SingleProjectView(string projectName)
        {
            var project = GetProjects().Where(x => x.Title == projectName).FirstOrDefault();
            project.Team = UserController.GetAllUsers();
            return View(project);
        }

        public IActionResult ProjectOverView()
        {
            var users = UserController.GetAllUsers();
            var projects = GetProjects();
            projects[0].Team = users;

            return View(projects);
        }

        public IActionResult ProjectSearch(string projectName)
        {
            return View(GetProjects().Where(x => x.Title == projectName).FirstOrDefault());
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
                    "Kenneth: Jeg er igang med frontend, User overview er done",
                    DateTime.Now.Date,
                    DateTime.Now.Date.AddDays(60),
                    new Models.User("Kenneth","kean513",new List<Types.Roles>() { Types.Roles.Projektleder, Types.Roles.Udvikler }),
                    new List<User>()
                        ),
                new Project(
                    "SkpUdLån",
                    "Hvis bare",
                    "Line: Ønske om mere info!",
                    DateTime.Now.Date,
                    DateTime.Now.Date,
                    new User("Jesper", "jkd431", new List<Types.Roles>(){ Types.Roles.Projektleder }),
                    new List<User>()
                    )
            };
        }
    }
}
