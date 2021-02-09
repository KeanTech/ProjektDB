using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security;
using System.Text;

namespace SkpDbLib.Db
{
    internal class Connection
    {
        private SqlConnection _connection;

        public void SetConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }
    }
}
