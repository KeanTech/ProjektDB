using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public ProjectController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// This is a single view of a prjects, that a user selects from another view.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IActionResult SingleProjectView(string projectName)
        {
            var project = GetProjects().Where(x => x.Title == projectName).FirstOrDefault();
            project.Team = UserController.GetAllUsers();
            return View(new ProjectModel(project.Title, project.Description, project.Log, project.StartDate, project.EndDate, project.Projectleder, project.Team));
        }

        /// <summary>
        /// This is the view of all the projects in the db
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectOverView()
        {
            var users = UserController.GetAllUsers();
            var projects = GetProjects();
            projects.OrderBy(x => x.Title.ToLower() == "a");

            List<ProjectModel> projectModels = new List<ProjectModel>();
            foreach (var project in projects)
            {
                projectModels.Add(new ProjectModel(project.Title, project.Description, project.Log, project.StartDate, project.EndDate, project.Projectleder, project.Team));
            }

            return View(projectModels);
        }

        [HttpGet]
        /// <summary>
        /// Used to get the AddToLog view.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IActionResult WriteToLog(int projectId)
        {
            return View(GetProjects().Where(x => x.Id == projectId).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult AddToLog(string logString, int projectId)
        {
            //Check authentication

            //Save the log to db
            //add username to logstring 
            var project = GetProjects().Where(x => x.Id == projectId).FirstOrDefault();

            return Redirect("/Project/ProjectOverView");
        }

        //----------------------------------------------------Search methods
  
        public IActionResult ProjectSearch(string projectName, bool nameCheck, bool projectleaderCheck, bool descriptionCheck, bool logCheck)
        {
            if (projectName != null)
            {
                List<ProjectModel> searchedProject = GetProjects().FindAll( x => x.Title.ToLower().Contains(projectName.ToLower() ));
                SortOutSearchCriteria(searchedProject, nameCheck, projectleaderCheck, descriptionCheck, logCheck); 

                if (searchedProject != null)
                    return View("ProjectOverView", searchedProject);
                else
                    return View("ProjectOverView", SortOutSearchCriteria( GetProjects(), nameCheck, projectleaderCheck, descriptionCheck, logCheck ));
            }
            else
            {
                return View("ProjectOverView", SortOutSearchCriteria( GetProjects(), nameCheck, projectleaderCheck, descriptionCheck, logCheck ));
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


        //-----------------------------------------------------CRUD Methods
        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public void CreateProject(Project project)
        {
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("SkpDb"));
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Projects(Status, Title, Description, Log, StartDate, EndDate, ProjectLeader) VALUES (@Status, @Title, @Description, @Log, @StartDate, @EndDate, @ProjectLeader);", connection);

            sqlCommand.Parameters.Add(new SqlParameter("@Status", project.Status));
            sqlCommand.Parameters.Add(new SqlParameter("@Title", project.Title));
            sqlCommand.Parameters.Add(new SqlParameter("@Description", project.Description));
            sqlCommand.Parameters.Add(new SqlParameter("@StartDate", project.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            sqlCommand.Parameters.Add(new SqlParameter("@EndDate", project.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            sqlCommand.Parameters.Add(new SqlParameter("ProjectLeader", project.Projectleder));
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            connection.Close();

            Redirect("/HomePage");
        }

        public Project GetProject()
        {
            return null;
        }

        public static List<ProjectModel> GetProjects() //Made static for testing
        {
            return null;
        }
    }
}
