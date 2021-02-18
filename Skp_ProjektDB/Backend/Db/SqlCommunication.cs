using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Skp_ProjektDB.Backend.Db
{
    internal class SqlCommunication
    {
        public DataSet GetTeam(SqlConnection connection, int projectId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection);
            command.Parameters.AddWithValue("", projectId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetProject(SqlConnection connection, string projectName)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection);
            command.Parameters.AddWithValue("", projectName);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetActiveProjects(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetAllProjects(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("", connection);
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

        public void CreateUser(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("CreateUser", connection); // call stored procedure
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Competence", user.Competence);
            command.Parameters.AddWithValue("Hash", user.Hash);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.Parameters.AddWithValue("Login", user.Login);

            command.ExecuteNonQuery();

            // handle roles for the user
            foreach (Roles role in user.Roles)
            {
                command = new SqlCommand("AddRoleToUser", connection); // call procedure for add role
                command.Parameters.AddWithValue("Role", role);
                command.Parameters.AddWithValue("UserName", user.Login);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("DeleteUser", connection); // call stored procedure
            command.Parameters.AddWithValue("UserName", user.Login);
            command.Parameters.AddWithValue("Name", user.Name);
            command.ExecuteNonQuery();

        }

        public void UpdateUser(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("", connection); // call stored procedure
            command.Parameters.AddWithValue("", user.Name);
            command.Parameters.AddWithValue("", user.Competence);
            command.Parameters.AddWithValue("", user.Hash);
            command.Parameters.AddWithValue("", user.Salt);
            command.Parameters.AddWithValue("", user.Login);

            command.ExecuteNonQuery();

            foreach (Roles role in user.Roles)
            {
                command = new SqlCommand("", connection); // call stored procedure
                command.Parameters.AddWithValue("", role);
                command.ExecuteNonQuery();
            }
        }
    }
}
