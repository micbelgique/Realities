using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Report : BaseEntity
    {
        public User User { get; set; }
        public Placement Placement { get; set; }
        public string Reason { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
