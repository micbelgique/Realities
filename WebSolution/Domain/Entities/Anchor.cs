using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Domain.Entities
{
    public class Anchor : BaseEntity
    {
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Address { get; set; }

        public float Size { get; set; }

        public string PictureUrl { get; set; }

        public Point Location { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public virtual List<Interaction> Interactions { get; set; }
    }
}
