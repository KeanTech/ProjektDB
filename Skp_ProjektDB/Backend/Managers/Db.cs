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

        // add sql commands here => stored procedures prefered
        public DataSet GetData(string sqlCommand)
        {
            return _sqlCommands.GetData(sqlCommand, _dbConnection.GetConnection());
        }


        #region --------------------------------------------------------------------------------------------------- vv Project CRUD Methods vv 

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

        public void CreateUser(string name, string competence, string hash, string salt, string username, List<Roles> roles)
        {
            _sqlCommands.CreateUser(_dbConnection.GetConnection(), name, competence, hash, salt, username, roles);
        }

        public void DeleteUser(string username)
        {
            _sqlCommands.DeleteUser(_dbConnection.GetConnection(), username);
        }

        public User GetUser(string username)
        {
            DataSet data = _sqlCommands.GetUser(username, _dbConnection.GetConnection());
            DataRow userRow = data.Tables[0].Rows[0];
            User user = new User("", "", new System.Collections.Generic.List<Types.Roles>());
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
                User user = new User("", "", new List<Types.Roles>());
                // fill user with correct data (need to know data placement)
            }

            return users;
        }

        public void UpdateUser(string name, string competence, string hash, string salt, string username, List<Roles> roles)
        {
            _sqlCommands.UpdateUser(_dbConnection.GetConnection(), name, competence, hash, salt, username, roles);
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
