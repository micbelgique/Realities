using System.Collections.Generic;
using System.Linq;
using Application.Dtos.Communities;
using Applications.Dtos.GeoLocation;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos
{
    public class FullCommunityDTO : CommunityDTO
    {
        public ICollection<AdmissionDTO> Admissions { get; set; }
        public ICollection<AdmissionDTO> PendingAdmissions { get; set; }

        public new void Mapping(Profile profile)
        {
            profile.CreateMap<Community, FullCommunityDTO>()
                .ForMember(d => d.EpiCenter, 
                    opt => opt.MapFrom(s => new EpicenterDTO(s.EpiCenter.Y, s.EpiCenter.X, s.EpiCenterRadius)))
                .ForMember(d => d.Admissions, opt => opt.MapFrom(s => s.Admissions.Where(x => x.Pending == false)))
                .ForMember(d => d.PendingAdmissions, opt => opt.MapFrom(s => s.Admissions.Where(x => x.Pending == true)));
            //.ForMember(d => d.Users, s => s.MapFrom( m => m))
        }
    }
}