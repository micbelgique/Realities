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
    public class PlacementDTO : IMapFrom<Placement>
    {
        public string UserId { get; set; }
        public Nullable<int> CommunityId { get; set; }
        public string Visibility { get; set; }
        public AnchorDTO Anchor { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Placement, PlacementDTO>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
                .ForMember(d => d.CommunityId, opt => opt.MapFrom(s => s.Community != null ? s.Community.Id : -1))
                .ForMember(d => d.Visibility, opt => opt.MapFrom(s => s.Visibility.ToString()));
        }
    }
}
