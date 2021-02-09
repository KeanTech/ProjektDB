using System.Data;
using System.Data.SqlClient;

namespace Skp_ProjektDB.Backend.Db
{
    internal class SqlCommunication
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
