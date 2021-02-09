using Skp_ProjektDB.Backend.Db;
using System;
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
