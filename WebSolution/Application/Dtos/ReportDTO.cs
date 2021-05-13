using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ReportDTO : IMapFrom<Report>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AnchorId { get; set; }
        public string Reason { get; set; }
        public DateTime CreationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Report, ReportDTO>()
                .ForMember(d => d.AnchorId, opt => opt.MapFrom(s => s.Placement.Anchor.Identifier));
        }
    }
}
