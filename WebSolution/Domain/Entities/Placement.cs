using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Placement : BaseEntity
    {
        public User User { get; set; }
        public Community Community { get; set; }
        public PlacementVisibility Visibility { get; set; }
        public Anchor Anchor { get; set; }
    }
}
