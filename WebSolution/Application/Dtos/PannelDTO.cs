using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class PannelDTO : IMapFrom<Pannel>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PictureUrl { get; set; }

        public PannelDTO()
        {}
    }
}
