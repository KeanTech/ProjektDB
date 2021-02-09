using System.Data;

namespace Skp_ProjektDB.Backend.Db
{
    internal class SqlCommands
    {
        public DataSet GetData(string sqlCommand, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand(sqlCommand, connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }
    }
}
