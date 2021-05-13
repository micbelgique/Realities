using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities.Communities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Communities
{
    public class AdmissionDTO : IMapFrom<Admission>
    {
        public DateTime JoiningDate { get; set; }
        public string UserId { get; set; }
        public string Roles { get; set; }
        public int Community { get; set; }

        public new void Mapping(Profile profile)
        {
            profile.CreateMap<Admission, AdmissionDTO>()
                .ForMember(d => d.UserId,
                    opt => opt.MapFrom(s => s.User.Id))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles.ToString()))
                .ForMember(d => d.Community, opt => opt.MapFrom(s => s.Community.Id));
            //.ForMember(d => d.Users, s => s.MapFrom( m => m))
        }
    }
}
