namespace Skp_ProjektDB.Backend.Db
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
