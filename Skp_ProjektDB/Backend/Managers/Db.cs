using Skp_ProjektDB.Backend.Db;
using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace Skp_ProjektDB.Backend.Managers
{
    public class Db : IDisposable
    {
        private Connection _dbConnection = new Connection();
        private SqlCommunication _sqlCommands = new SqlCommunication();

        #region --------------------------------------------------------------------------------------------------- vv Project CRUD Methods vv 

        public Project GetProject(int projectId)
        {
            DataSet data = _sqlCommands.GetProject(_dbConnection.GetConnection(), projectId);
            DataRow dataRow = data.Tables[0].Rows[0];
            // set data

            // get team
            int projectid = int.Parse(dataRow[0].ToString());
            DataSet tream = _sqlCommands.GetTeam(_dbConnection.GetConnection(), 0);

            return new Project("", "", new List<string>(), DateTime.Now, DateTime.Now, new User(), new List<User>());
        }

        #endregion --------------------------------------------------------------------------------------------------- ^^ Project CRUD Methods ^^

        #region --------------------------------------------------------------------------------------------------- vv User CRUD Methods vv
        public byte[] GetSalt(string username)
        {
            DataSet data = _sqlCommands.GetSalt(username, _dbConnection.GetConnection());
            DataRow saltRow = data.Tables[0].Rows[0];
            string salt = saltRow[0].ToString();

            byte[] saltBytes = Convert.FromBase64String(salt);

            return saltBytes;
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
            User user = new User() { Name = userRow.ItemArray[0].ToString(), Competence = userRow.ItemArray[1].ToString(), Login = userRow.ItemArray[2].ToString() };
            // fille userRow data into user. (need to know the data placement)
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
                User user = new User(  ) { Name = userRow.ItemArray[0].ToString(), Competence = userRow.ItemArray[1].ToString(), Login = userRow.ItemArray[2].ToString() + "@zbc.dk" };
                users.Add(user);
            }

            return users;
        }

        public void UpdateUser(User user)
        {
            _sqlCommands.UpdateUser(_dbConnection.GetConnection(), user);
        }
        #endregion --------------------------------------------------------------------------------------------------- ^^ User CRUD Methods ^^ 


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
