using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pannel : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PictureUrl { get; set; }

        public string AnchorIndetifier { get; set; }
    }
}
