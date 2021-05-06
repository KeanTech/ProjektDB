using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Skp_ProjektDB.Models.User;

namespace Skp_ProjektDB.Models
{
    public class Person
    {
        public int Id { get; set; }
        [DisplayName("Navn")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Hovedforløb")]
        [Required]
        public string Competence { get; set; }

        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Login { get; set; }


        public List<Project> Projects { get; set; }
    }
}
