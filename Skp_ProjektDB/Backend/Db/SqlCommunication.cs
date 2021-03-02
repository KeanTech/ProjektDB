using Skp_ProjektDB.Models;
using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Skp_ProjektDB.Backend.Db
{
    internal class SqlCommunication
    {
        public string CheckUserName(string userName)
        {
            if (userName.Split('@').Length < 2)
                return userName;
            else
                return userName.Split('@')[0];
        }



        public void CreateUser(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("CreateUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Name", user.Name);
            command.Parameters.AddWithValue("Competence", user.Competence);
            command.Parameters.AddWithValue("Hash", user.Hash);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.Parameters.AddWithValue("Login", user.Login);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public DataSet GetAllUsers(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewAllUsers", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetUser(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewUserByUsername", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Login", CheckUserName(username));
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public void DeleteUser(SqlConnection connection, string username)
        {
            SqlCommand command = new SqlCommand("DeleteUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", CheckUserName(username));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateUser(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("UpdateUser", connection);
            command.Parameters.AddWithValue("Username", user.Login);
            command.Parameters.AddWithValue("Hash", user.Hash);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.Parameters.AddWithValue("Name", user.Name);
            command.Parameters.AddWithValue("Competence", user.Competence);

            command.ExecuteNonQuery();
        }

        public void ChangePassword(SqlConnection connection, User user)
        {
            SqlCommand command = new SqlCommand("UpdateUser", connection);
            command.Parameters.AddWithValue("Username", user.Login);
            command.Parameters.AddWithValue("Hash", user.Hash);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.ExecuteNonQuery();
        }

        public DataSet GetSalt(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("GetSalt", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Username", username);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetHash(string username, SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("GetHash", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Username", username);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet SearchForUser(SqlConnection connection, string search)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("SearchForUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Search", search);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        //---

        public void AddRolesToUser(SqlConnection connection, User user)
        {
            foreach (Roles role in user.Roles)
            {
                SqlCommand command = new SqlCommand("AddRoleToUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("UserName", user.Login);
                command.Parameters.AddWithValue("Role", role);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public DataSet ViewUsersRoles(SqlConnection connection, string username)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewUsersRoles", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Username", username);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public void RemoveRoleFromUser(SqlConnection connection, string username, string role)
        {
            SqlCommand command = new SqlCommand("RemoveRoleFromUser", connection);
            command.Parameters.AddWithValue("UserName", username);
            command.Parameters.AddWithValue("Role", role);
            command.ExecuteNonQuery();
        }

        public void UpdateRole(SqlConnection connection, string username, string oldRole, string newRole)
        {
            SqlCommand command = new SqlCommand("UpdateRole", connection);
            command.Parameters.AddWithValue("UserName", username);
            command.Parameters.AddWithValue("OldRole", oldRole);
            command.Parameters.AddWithValue("NewRole", newRole);
            command.ExecuteNonQuery();
        }

        //---

        public void CreateProject(SqlConnection connection, Project project)
        {

            SqlCommand command = new SqlCommand("CreateProject", connection);
            command.Parameters.AddWithValue("Status", project.Status);
            command.Parameters.AddWithValue("Title", project.Title);
            command.Parameters.AddWithValue("Description", project.Description);
            command.Parameters.AddWithValue("StartDate", project.StartDate);
            command.Parameters.AddWithValue("EndDate", project.EndDate);
            command.Parameters.AddWithValue("ProjectLeader", project.Projectleder);
            command.ExecuteNonQuery();
        }

        public DataSet GetAllProjects(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewAllProjects", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }
        public DataSet GetProject(SqlConnection connection, int projectID)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewProject", connection);
            command.Parameters.AddWithValue("ID", projectID);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet GetActiveProjects(SqlConnection connection)
        {
            DataSet data = new DataSet();
            //Select * FROM Projects WHERE Status = 'Aktiv'
            SqlCommand command = new SqlCommand("ViewActiveProjects", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public void UpdateProject(SqlConnection connection, Project project)
        {
            SqlCommand command = new SqlCommand("UpdateProject", connection);
            command.Parameters.AddWithValue("Status", project.Id);
            command.Parameters.AddWithValue("Status", project.Status);
            command.Parameters.AddWithValue("Title", project.Title);
            command.Parameters.AddWithValue("Description", project.Description);
            command.Parameters.AddWithValue("StartDate", project.StartDate);
            command.Parameters.AddWithValue("EndDate", project.EndDate);
            command.Parameters.AddWithValue("ProjectLeader", project.Projectleder);
            command.ExecuteNonQuery();
        }

        public void DeleteProject(SqlConnection connection, Project project)
        {
            SqlCommand command = new SqlCommand("DeleteProject", connection);
            command.Parameters.AddWithValue("Status", project.Id);
            command.ExecuteNonQuery();
        }

        //---
        public void AddUserToTeam(SqlConnection connection, int projectId, string username)
        {
            SqlCommand command = new SqlCommand("AddUserToTeam", connection);
            command.Parameters.AddWithValue("ID", projectId);
            command.Parameters.AddWithValue("Username", username);
            command.ExecuteNonQuery();
        }

        public void RemoveUserFromTeam(SqlConnection connection, int projectId, string username)
        {
            SqlCommand command = new SqlCommand("RemoveUserFromTeam", connection);
            command.Parameters.AddWithValue("ID", projectId);
            command.Parameters.AddWithValue("Username", username);
            command.ExecuteNonQuery();
        }


        public DataSet GetTeam(SqlConnection connection, int projectId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("GetTeam", connection);
            command.Parameters.AddWithValue("ID", projectId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        //---

        public void AddLogToProject(SqlConnection connection, Project project, string username)
        {
            SqlCommand command = new SqlCommand("AddLogToProject", connection);
            command.Parameters.AddWithValue("ID", project.Id);
            command.Parameters.AddWithValue("Log", project.Log); //GETDATE() in database on log
            command.Parameters.AddWithValue("Username", username);
            command.ExecuteNonQuery();
        }

        public void EditLog(SqlConnection connection, int logID, string log, string username)
        {
            SqlCommand command = new SqlCommand("EditLog", connection);
            command.Parameters.AddWithValue("LogID", logID);
            command.Parameters.AddWithValue("Log", log); //GETDATE() in database on log
            command.Parameters.AddWithValue("Username", username);
            command.ExecuteNonQuery();
        }

        public void DeleteLog(SqlConnection connection, int logID)
        {
            SqlCommand command = new SqlCommand("DeleteLog", connection);
            command.Parameters.AddWithValue("LogID", logID);
            command.ExecuteNonQuery();
        }

        public DataSet ViewLastLogFromTeam(SqlConnection connection, int projectId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewLastLogFromTeam", connection);
            command.Parameters.AddWithValue("ID", projectId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet ViewAllLogsFromTeam(SqlConnection connection, int projectId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewAllLogsFromTeam", connection);
            command.Parameters.AddWithValue("ID", projectId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        //---

        public void AddCustomerToProject(SqlConnection connection, int projectId, string name, string email)
        {
            SqlCommand command = new SqlCommand("AddCustomerToProject", connection);
            command.Parameters.AddWithValue("ID", projectId);
            command.Parameters.AddWithValue("Name", name); 
            command.Parameters.AddWithValue("Email", email);
            command.ExecuteNonQuery();
        }

        public void EditCustomer(SqlConnection connection, int customerId, string name, string email)
        {
            SqlCommand command = new SqlCommand("EditCustomer", connection);
            command.Parameters.AddWithValue("CustomerID", customerId);
            command.Parameters.AddWithValue("Name", name); 
            command.Parameters.AddWithValue("Email", email);
            command.ExecuteNonQuery();
        }

        public void DeleteCustomer(SqlConnection connection, int customerId)
        {
            SqlCommand command = new SqlCommand("DeleteCustomer", connection);
            command.Parameters.AddWithValue("CustomerID", customerId);
            command.ExecuteNonQuery();
        }

        public DataSet ViewAllCustomers(SqlConnection connection)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewAllCustomers", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }
        public DataSet ViewALlCustomersOnProject(SqlConnection connection, int projectId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewALlCustomersOnProject", connection);
            command.Parameters.AddWithValue("ID", projectId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataSet ViewCustomersProjects(SqlConnection connection, int customerId)
        {
            DataSet data = new DataSet();
            SqlCommand command = new SqlCommand("ViewCustomersProjects", connection);
            command.Parameters.AddWithValue("CustomerID", customerId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

    }
}
