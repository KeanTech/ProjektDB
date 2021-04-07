using Skp_ProjektDB.Backend.Db;
using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Skp_ProjektDB.Backend.Managers
{
    public class Db : IDisposable
    {
        private Connection _dbConnection = new Connection();
        private SqlCommunication _sqlCommands = new SqlCommunication();
        private Security security = new Security();
        private Message message = new Message();
        private TimeSpan timeLogOut = new TimeSpan(1, 0, 0);


        #region vv Team CRUD Methods vv

        public void AddUserToTeam(int projectID, string userName)
        {
            _sqlCommands.AddUserToTeam(_dbConnection.GetConnection(), projectID, userName);
        }

        public List<User> GetTeam(int projectID)
        {
            DataSet data = _sqlCommands.GetTeam(_dbConnection.GetConnection(), projectID);
            List<User> teamNames = new List<User>();

            foreach (DataRow row in data.Tables[0].Rows)
            {
                teamNames.Add(GetUser(row.ItemArray[0].ToString()));
            }
            return teamNames;
        }

        public void RemoveUserFromTeam(string userName, int projectId)
        {
            _sqlCommands.RemoveUserFromTeam(_dbConnection.GetConnection(), projectId, userName);
        }

        #endregion

        #region ------------------------------------------------------------------- vv Project CRUD Methods vv 

        public void CreateProject(Project project)
        {
            _sqlCommands.CreateProject(_dbConnection.GetConnection(), project);
        }

        public Project GetProject(int projectId)
        {
            DataSet data = _sqlCommands.GetProject(_dbConnection.GetConnection(), projectId);
            ProjectModel project = new ProjectModel();
            project.Id = (int)data.Tables[0].Rows[0].ItemArray[0];
            project.Status = (Status) Convert.ToInt32(data.Tables[0].Rows[0].ItemArray[1].ToString());
            project.Title = data.Tables[0].Rows[0].ItemArray[2].ToString();
            project.Description = data.Tables[0].Rows[0].ItemArray[3].ToString();
            project.StartDate = (DateTime)data.Tables[0].Rows[0].ItemArray[4];
            project.EndDate = (DateTime)data.Tables[0].Rows[0].ItemArray[5];
            project.Projectleader = data.Tables[0].Rows[0].ItemArray[6].ToString();
            return project;
        }

        public List<ProjectModel> GetAllProjects()
        {
            DataSet data = _sqlCommands.GetAllProjects(_dbConnection.GetConnection());
            List<ProjectModel> projects = new List<ProjectModel>();
            foreach (DataRow dataRow in data.Tables[0].Rows)
            {
                ProjectModel project = new ProjectModel();
                project.Id = (int)dataRow.ItemArray[0];
                project.Status = (Status)Convert.ToInt32(dataRow.ItemArray[1]);
                project.Title = dataRow.ItemArray[2].ToString();
                project.Description = dataRow.ItemArray[3].ToString();
                project.StartDate = (DateTime)dataRow.ItemArray[4];
                project.EndDate = (DateTime)dataRow.ItemArray[5];
                project.Projectleader = dataRow.ItemArray[6].ToString();
                projects.Add(project);
            }

            return projects;
        }

        public void DeleteProject(int projectId)
        {
            _sqlCommands.DeleteProject(_dbConnection.GetConnection(), projectId);
        }

        public void UpdateProject(ProjectModel project)
        {
            _sqlCommands.UpdateProject(_dbConnection.GetConnection(), project);
        }

        #endregion --------------------------------------------------------------------------------------------------- ^^ Project CRUD Methods ^^

        #region -------------------------------------------------------------- vv User CRUD Methods vv
        public string GetSalt(string username)
        {
            DataSet data = _sqlCommands.GetSalt(username, _dbConnection.GetConnection());
            DataRow saltRow = data.Tables[0].Rows[0];
            string salt = saltRow[0].ToString();

            return salt;
        }

        public string GetHash(string username)
        {
            DataSet data = _sqlCommands.GetHash(username, _dbConnection.GetConnection());
            DataRow hashRow = data.Tables[0].Rows[0];
            string hash = hashRow[0].ToString();
            return hash;
        }

        public void CreateUser(User user)
        {
            user.Salt = security.GenerateSalt();
            string pass = "Kode1234";
            string encrypted = security.Encrypt(Encoding.UTF8.GetBytes(pass), Convert.FromBase64String(user.Salt));
            user.Hash = security.Hash(Convert.FromBase64String(encrypted));
            _sqlCommands.CreateUser(_dbConnection.GetConnection(), user);
        }

        public void DeleteUser(User user)
        {
            _sqlCommands.DeleteUser(_dbConnection.GetConnection(), user.Login);
        }

        public User GetUser(string username)
        {
            DataSet data = _sqlCommands.GetUser(username, _dbConnection.GetConnection());
            DataRow userRow = data.Tables[0].Rows[0];
            User user = new User() { Id = Convert.ToInt32(userRow.ItemArray[0]), Name = userRow.ItemArray[1].ToString(), Competence = userRow.ItemArray[2].ToString(), Login = userRow.ItemArray[3].ToString() };
            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            DataSet data = _sqlCommands.GetAllUsers(_dbConnection.GetConnection());
            DataRowCollection userRows = data.Tables[0].Rows;

            foreach (DataRow userRow in userRows)
            {
                // fill user with correct data (need to know data placement)
                User user = new User() { Id = Convert.ToInt32(userRow.ItemArray[0]), Name = userRow.ItemArray[1].ToString(), Competence = userRow.ItemArray[2].ToString(), Login = userRow.ItemArray[3].ToString() };
                GetUserRoles(user);
                users.Add(user);
            }
            return users;
        }

        public void UpdateUser(User user)
        {
            _sqlCommands.UpdateUser(_dbConnection.GetConnection(), user);
        }


        #endregion --------------------------------------------------------------------------------------------------- ^^ User CRUD Methods ^^ 

        #region -------------------------------------------------------------------------------------------- vv Role CRUD Methods vv
        public void AddRoleToUser(User user)
        {
            _sqlCommands.AddRolesToUser(_dbConnection.GetConnection(), user);
        }

        public User GetUserRoles(User user)
        {
            DataSet data = _sqlCommands.ViewUsersRoles(_dbConnection.GetConnection(), user.Login);
            DataRowCollection userRows = data.Tables[0].Rows;
            if (user.UserRoles == null)
            {
                user.UserRoles = new List<User.Roles>();
            }
            foreach (DataRow userRow in userRows)
            {
                user.UserRoles.Add((User.Roles)Convert.ToInt32(userRow.ItemArray[0]));
            }
            return user;
        }

        public void RemoveRoleFromUser(string userName, User.Roles role)
        {
            _sqlCommands.RemoveRoleFromUser(_dbConnection.GetConnection(), userName, role.ToString());
        }

        #endregion

        #region LogInAndOutAuthentication

        public void UserLogIn(string userName)
        {
            _sqlCommands.LoginAuthentication(_dbConnection.GetConnection(), userName);
        }

        public void UserLogOut(string userName)
        {
            _sqlCommands.LogoutAuthentication(_dbConnection.GetConnection(), userName);
        }

        public bool IsUserLogedIn(string userName)
        {
            DataSet data = _sqlCommands.LastLoginTime(_dbConnection.GetConnection(), userName);
            
            if (DateTime.Now - ((DateTime)data.Tables[0].Rows[0].ItemArray[0]) < timeLogOut)
            {
                return true;
            }
            else
                return false;
        }

        public bool IsUserLogedOut(string userName)
        {
            DateTime logOutDate = (DateTime)_sqlCommands.LastLogoutTime(_dbConnection.GetConnection(), userName).Tables[0].Rows[0].ItemArray[0];

            if (DateTime.Now - logOutDate > timeLogOut)
            {
                return true;
            }
            else
                return false;
        }

        #endregion


        public void SetConnection(string connectionString)
        {
            _dbConnection.SetConnection(connectionString);
        }

        public void Dispose()
        {
            _dbConnection = null;
            _sqlCommands = null;
        }
    }
}
