using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Skp_ProjektDB.Backend.Managers;
using Skp_ProjektDB.DataClasses;
using Skp_ProjektDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IConfiguration configuration;
        private Db db = new Db();
        public UserLogin logedInUser { get; set; } = UserController.logedInUser;

        public ProjectController(IConfiguration configuration)
        {
            this.configuration = configuration;
            db.SetConnection(configuration.GetConnectionString("SkpDb"));
        }

        /// <summary>
        /// This is a single view of a prjects, that a user selects from another view.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IActionResult SingleProjectView(string projectName)
        {
            if (logedInUser != null)
            {
                if (logedInUser.User.Login != null)
                {
                    var project = db.GetAllProjects().Where(x => x.Title == projectName).FirstOrDefault();
                    project.Team = db.GetTeam(project.Id);
                    project.Log = db.ViewAllLogsFromTeam(project.Id);
                    foreach (var item in project.Team)
                    {
                        db.GetUserRoles(item);
                    }
                    if (logedInUser.User.UserRoles.Contains(Models.User.Roles.Instruktør))
                    {
                        project.Admin = true;
                    }
                    return View(project);
                }
                else
                    return BadRequest("Du er ikke logget ind");

            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        /// <summary>
        /// This is the view of all the projects in the db
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectOverView()
        {
            if (logedInUser != null)
            {
                if (logedInUser.User.Login != null)
                {
                    var projects = db.GetAllProjects();
                    projects.OrderBy(x => x.Title.ToLower() == "a");
                    if (logedInUser.User.UserRoles.Contains(Models.User.Roles.Instruktør))
                    {
                        projects[0].Admin = true;
                    }
                    return View(projects);
                }
                else
                    return BadRequest("Du er ikke logget ind");
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        [HttpGet]
        /// <summary>
        /// Used to get the AddToLog view.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// 

        //Log vv
        public IActionResult WriteToLog(int projectId)
        {
            if (logedInUser.User.Login != null)
                return View(db.GetAllProjects().Where(x => x.Id == projectId).FirstOrDefault());
            else
                return BadRequest("Du er ikke logget ind");
        }

        [HttpPost]
        public IActionResult AddToLog(string logString, int projectId, string username)
        {
            //Check authentication

            //Save the log to db
            //add username to logstring 
            if (logedInUser.User.Login != null)
            {
                db.AddLogToProject(projectId, logString, username);

                var project = db.GetAllProjects().Where(x => x.Id == projectId).FirstOrDefault();
                if (logedInUser.User.UserRoles.Contains(Models.User.Roles.Instruktør))
                {
                    project.Admin = true;
                }
                return Redirect("/Project/ProjectOverView");
            }
            else
                return BadRequest("Du er ikke logget ind");

        }

        public void UpdateLog(int projectId, string logString, string username)
        {
            db.UpdateLog(projectId, logString, username);
        }

        public void DeleteLog(int logId)
        {
            db.DeleteLog(logId);
        }

        public void ViewLastLogFromTeam(int projectId)
        {
            db.ViewLastLogFromTeam(projectId);
        }

        public void ViewAllLogsFromTeam(int projectId)
        {
            db.ViewAllLogsFromTeam(projectId);
        }

        public void ViewLogWithID(int logId)
        {
            db.ViewLogWithID(logId);
        }
        //Log ^^

        //----------------------------------------------------Search methods

        public IActionResult ProjectSearch(string projectName, bool projectleaderCheck, bool descriptionCheck, bool logCheck)
        {
            if (projectName != null)
            {
                List<ProjectModel> searchedProject = db.GetAllProjects().FindAll(x => x.Title.ToLower().Contains(projectName.ToLower()));
                SortOutSearchCriteria(searchedProject, projectleaderCheck, descriptionCheck, logCheck);

                if (searchedProject != null)
                    return View("ProjectOverView", searchedProject);
                else
                    return View("ProjectOverView", SortOutSearchCriteria(db.GetAllProjects(), projectleaderCheck, descriptionCheck, logCheck));
            }
            else
            {
                return View("ProjectOverView", SortOutSearchCriteria(db.GetAllProjects(), projectleaderCheck, descriptionCheck, logCheck));
            }
        }

        private List<ProjectModel> SortOutSearchCriteria(List<ProjectModel> projectModels, bool projectleaderCheck, bool descriptionCheck, bool logCheck)
        {
            foreach (var model in projectModels)
            {
                model.ProjectLeaderCheckbox = projectleaderCheck;
                model.DescriptionCheckbox = descriptionCheck;
                model.LogCheckbox = logCheck;
            }
            return projectModels;
        }

        //-----------------------------------------------------CRUD Project Methods
        [HttpGet]
        public IActionResult CreateProject()
        {
            if (logedInUser.User.Login != null)
            {
                return View(new ProjectModel() { Users = db.GetAllUsers(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date });
            }
            else
            {
                return BadRequest("Du er ikke logget ind");
            }
        }

        [HttpPost]
        public IActionResult CreateProject(ProjectModel model)
        {
            if (logedInUser.User.Login != null)
            {
                Project project = (Project)model;

                db.CreateProject(project);
                return Redirect("/Project/ProjectOverView");
            }
            else
                return BadRequest("Du er ikke logget ind");
        }

        public IActionResult DeleteProject(int projectId)
        {
            if (logedInUser.User.Login != null)
            {
                db.DeleteProject(projectId);
                return Redirect("/Project/ProjectOverView");
            }
            else
            {
                return BadRequest("Du er ikke logget ind");
            }
        }

        //-----------------------------------------------------CRUD Team Methods

        public void RemoveUserFromTeam(string userName, int projectId)
        {
            if (logedInUser.User.Login != null)
            {
                db.RemoveUserFromTeam(userName, projectId);
                Redirect("/Project/ProjectOverView");
            }
        }

        public void AddUserToTeam(string userName, int projectId)
        {
            if (logedInUser.User.Login != null)
            {
                db.AddUserToTeam(projectId, userName);
                Redirect("/Project/ProjectOverView");
            }
            else
                BadRequest("Du er ikke logget ind");
        }

        //-----------------------------------------------------CRUD Role Methods

        public void RemoveRoleFromUser(User user, User.Roles role)
        {
            if (logedInUser.User.Login != null)
            {
                db.RemoveRoleFromUser(user.Login, role);
            }
            else
                BadRequest("Du er ikke logget ind");
            
        }

        public void AddRoleToUser(User user)
        {

        }
    }
}
