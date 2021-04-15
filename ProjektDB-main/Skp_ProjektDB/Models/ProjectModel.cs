using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Models
{
    public class ProjectModel : Project
    {
        public ProjectModel()
        {
            Log = new List<string>();
            NameCheckbox = true;
            DescriptionCheckbox = true;
        }


        public List<User> Users { get; set; }

        public bool NameCheckbox { get; set; }

        public bool ProjectLeaderCheckbox { get; set; }

        public bool DescriptionCheckbox { get; set; }

        public bool LogCheckbox { get; set; }

    }
}
