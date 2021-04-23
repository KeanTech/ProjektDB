using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skp_ProjektDB.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
