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
        public ProjectController(IConfiguration configuration)
        {
            this.configuration = configuration;
            db.SetConnection(configuration.GetConnectionString("Test"));
        }

        /// <summary>
        /// This is a single view of a prjects, that a user selects from another view.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IActionResult SingleProjectView(string projectName)
        {
            var project = db.GetAllProjects().Where(x => x.Title == projectName).FirstOrDefault();
            project.Team = db.GetTeam(project.Id);
            return View(project);
        }

        /// <summary>
        /// This is the view of all the projects in the db
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectOverView()
        {
            var projects = db.GetAllProjects();
            projects.OrderBy(x => x.Title.ToLower() == "a");

            return View(projects);
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
            return View(db.GetAllProjects().Where(x => x.Id == projectId).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult AddToLog(string logString, int projectId, string username)
        {
            //Check authentication

            db.AddLogToProject(projectId, logString, username); 

            var project = db.GetAllProjects().Where(x => x.Id == projectId).FirstOrDefault();

            return Redirect("/Project/ProjectOverView");
        }
        public void UpdateLog(int projectId, string logString, string username)
        {
            db.UpdateLog(projectId, logString, username);
        }
        public void DeleteLog(int logId)
        {
            db.DeleteLog(logId);
        }
        public void LastViewLastLogFromTeam(int projectId)
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

        public IActionResult ProjectSearch(string projectName, bool nameCheck, bool projectleaderCheck, bool descriptionCheck, bool logCheck)
        {
            if (projectName != null)
            {
                List<ProjectModel> searchedProject = db.GetAllProjects().FindAll(x => x.Title.ToLower().Contains(projectName.ToLower()));
                SortOutSearchCriteria(searchedProject, nameCheck, projectleaderCheck, descriptionCheck, logCheck);

                if (searchedProject != null)
                    return View("ProjectOverView", searchedProject);
                else
                    return View("ProjectOverView", SortOutSearchCriteria(db.GetAllProjects(), nameCheck, projectleaderCheck, descriptionCheck, logCheck));
            }
            else
            {
                return View("ProjectOverView", SortOutSearchCriteria(db.GetAllProjects(), nameCheck, projectleaderCheck, descriptionCheck, logCheck));
            }
        }

        private List<ProjectModel> SortOutSearchCriteria(List<ProjectModel> projectModels, bool nameCheck, bool projectleaderCheck, bool descriptionCheck, bool logCheck)
        {
            foreach (var model in projectModels)
            {
                model.NameCheckbox = nameCheck;
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
            return View(new ProjectModel() { Users = db.GetAllUsers(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date });
        }

        [HttpPost]
        public void CreateProject(ProjectModel model)
        {
            Project project = (Project)model;

            db.CreateProject(project);
            Redirect("/Project/ProjectOverView");
        }

        public void DeleteProject(int projectId)
        {
            db.DeleteProject(projectId);
        }

        //-----------------------------------------------------CRUD Team Methods

        public void RemoveUserFromTeam(string userName, int projectId)
        {
            db.RemoveUserFromTeam(userName, projectId);
            Redirect("/Project/ProjectOverView");
        }

        public void AddUserToTeam(string userName, int projectId)
        {
            db.AddUserToTeam(projectId, userName);
            Redirect("/Project/ProjectOverView");
        }

        //-----------------------------------------------------CRUD Role Methods

        public void RemoveRoleFromUser(User user, User.Roles role) 
        {
            db.RemoveRoleFromUser(user.Login, role);
        }

        public void AddRoleToUser(User user)
        {

        }
    }
}
