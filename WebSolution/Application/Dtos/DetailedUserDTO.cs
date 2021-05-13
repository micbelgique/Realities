using Application.Common.Mappings;
using Application.Dtos.Communities;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class DetailedUserDTO: UserDTO, IMapFrom<User>
    {
        public ICollection<AdmissionDTO> Admissions { get; set; }
        public ICollection<AdmissionDTO> PendingAdmissions { get; set; }


        public ICollection<PlacementDTO> Placements { get; set; }

        #region MAPPING
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, DetailedUserDTO>()
                .ForMember(d => d.NickName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.Admissions, opt => opt.MapFrom(s => s.Admissions.Where(x => x.Pending == false)))
                .ForMember(d => d.PendingAdmissions, opt => opt.MapFrom(s => s.Admissions.Where(x => x.Pending == true)));
        }
        #endregion
    }
}
