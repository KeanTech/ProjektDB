using Skp_ProjektDB.Types;
using System.Collections.Generic;
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

        public DataSet GetSalt(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection); //call stored procedure here with username as param
            command.Parameters.AddWithValue("", username); // add param name for stored procedure
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetHash(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection); // call stored procedure here with username as param
            command.Parameters.AddWithValue("", username); // ad param name for stored procedure
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetUser(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection); // call stored procedure here with username as param
            command.Parameters.AddWithValue("", username); // add param name for stored procedure
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetAllUsers(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection); // call stored procedure for all users
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public void CreateUser(SqlConnection connection, string name, string competence, string hash, string salt, string username, List<Roles> roles)
        {
            SqlCommand command = new SqlCommand("CreateUser", connection); // call stored procedure
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Competence", competence);
            command.Parameters.AddWithValue("Hash", hash);
            command.Parameters.AddWithValue("Salt", salt);
            command.Parameters.AddWithValue("Login", username);

            command.ExecuteNonQuery();

            // handle roles for the user
            foreach (Roles role in roles)
            {
                command = new SqlCommand("AddRoleToUser", connection); // call procedure for add role
                command.Parameters.AddWithValue("Role", role);
                command.Parameters.AddWithValue("UserName", username);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(SqlConnection connection, string username, string name, List<Roles> roles)
        {
            SqlCommand command = new SqlCommand("DeleteUser", connection); // call stored procedure
            command.Parameters.AddWithValue("UserName", username);
            command.Parameters.AddWithValue("Name", name);
            command.ExecuteNonQuery();

        }

        public void UpdateUser(SqlConnection connection, string name, string competence, string hash, string salt, string username, List<Roles> roles)
        {
            SqlCommand command = new SqlCommand("", connection); // call stored procedure
            command.Parameters.AddWithValue("", name);
            command.Parameters.AddWithValue("", competence);
            command.Parameters.AddWithValue("", hash);
            command.Parameters.AddWithValue("", salt);
            command.Parameters.AddWithValue("", username);

            command.ExecuteNonQuery();

            foreach (Roles role in roles)
            {
                command = new SqlCommand("", connection); // call stored procedure
                command.Parameters.AddWithValue("", role);
                command.ExecuteNonQuery();
            }
        }
    }
}
