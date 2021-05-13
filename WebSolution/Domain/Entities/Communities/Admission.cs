using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Communities
{
    public class Admission : BaseEntity
    {
        public DateTime JoiningDate { get; set; }
        public User User { get; set; }
        public Community Community { get; set; }
        public bool Pending { get; set; }
        //todo: by who ?
        public Community_roles Roles { get; set; }
    }
}
