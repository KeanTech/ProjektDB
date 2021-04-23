using Skp_ProjektDB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Models
{
    public class Project
    {
        public int Id { get; set; }
        [DisplayName("Projekt navn")]
        public string Title { get; set; }
        
        [DisplayName("Projekt beskrivelse")]
        public string Description { get; set; }
        
        public List<LogModel> Log { get; set; }
        
        [DisplayName("Start dato")]
        public DateTime StartDate { get; set; }

        [DisplayName("Slut dato")]
        public DateTime EndDate { get; set; }

        public Status Status { get; set; }

        [DisplayName("Projektleder")]
        public string Projectleader { get; set; }

        public List<User> Team { get; set; }

        public Project(string title, string description, List<LogModel> log, DateTime startDate, DateTime endDate, string projectleder, List<User> team)
        {
            Title = title;
            Description = description;
            Log = log;
            StartDate = startDate;
            EndDate = endDate;
            Projectleader = projectleder;
            Team = team;
        }

        public Project()
        {
        }
        
    }
}
