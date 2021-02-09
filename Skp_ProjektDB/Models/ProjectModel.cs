using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Models
{
    public class ProjectModel : Project
    {
        public ProjectModel(string title, string description, List<string> log, DateTime startDate, DateTime endDate, User projectleder, List<User> team) : base(title, description, log, startDate, endDate, projectleder, team)
        {
            NameCheckbox = true;
            DescriptionCheckbox = true;
            LogCheckbox = true;
        }

        public bool NameCheckbox { get; set; }

        public bool ProjectLeaderCheckbox { get; set; }

        public bool DescriptionCheckbox { get; set; }

        public bool LogCheckbox { get; set; }

       
    }
}
