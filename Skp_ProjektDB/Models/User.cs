using Skp_ProjektDB.Types;
using System.Collections.Generic;
using System.ComponentModel;

namespace Skp_ProjektDB.Models
{
    public class User
    {
        public User(string name, string login, List<Roles> roles)
        {
            Name = name;
            Login = login;
            UserRoles = roles;
            Projects = new List<Project>();
        }

        public enum Roles
        {
            Instruktør,
            ProjektLeder,
            Udvikler,
            DevOps,
            Support
        }

        [DisplayName("Navn")]
        public string Name { get; set; }

        [DisplayName("Hovedforløb")]
        public string Competence { get; set; }

        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Login { get; set; }
        
        [DisplayName("Roller")]
        public List<Roles> UserRoles { get; set; }

        //This is for view only dont add this to db (User is part of Project)
        public int Id { get; set; }
        [DisplayName("Projekter")]
        public List<Project> Projects { get; set; }
        public bool Admin { get; set; }
        public bool ProjectLeader { get; set; }
        public int Owner { get; set; }
        public User()
        {
            Projects = new List<Project>();
            UserRoles = new List<Roles>();
        }

    }
}
