using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SkpDbLib.Db
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
