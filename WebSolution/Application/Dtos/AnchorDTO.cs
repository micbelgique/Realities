using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using Application.Dtos.GeoLocation;

namespace Application.Dtos
{
    public class AnchorDTO : IMapFrom<Anchor>, IMapFrom<Placement>
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Address { get; set; }

        public float Size { get; set; }

        public string PictureUrl { get; set; }

        //public UserDTO User { get; set; }
        public string UserId { get; set; }

        public CoordinateDTO Location { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public string Visibility { get; set; }
        

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Anchor, AnchorDTO>()
                .ForMember(d => d.Location, opt => opt.MapFrom(s => new CoordinateDTO(s.Location.Y, s.Location.X) { SRID = s.Location.SRID }));
            //.ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id));

            profile.CreateMap<Placement, AnchorDTO>()
                .ForMember(d => d.Location, opt => opt.MapFrom(s => new CoordinateDTO(s.Anchor.Location.Y, s.Anchor.Location.X) { SRID = s.Anchor.Location.SRID }))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Anchor.Id))
                .ForMember(d => d.Identifier, opt => opt.MapFrom(s => s.Anchor.Identifier))
                .ForMember(d => d.Model, opt => opt.MapFrom(s => s.Anchor.Model))
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Anchor.Address))
                .ForMember(d => d.Size, opt => opt.MapFrom(s => s.Anchor.Size))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(s => s.Anchor.PictureUrl))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
                .ForMember(d => d.CreationDate, opt => opt.MapFrom(s => s.Anchor.CreationDate))
                .ForMember(d => d.Visibility, opt => opt.MapFrom(s => s.Visibility.ToString()))
                .ForMember(d => d.LastUpdateDate, opt => opt.MapFrom(s => s.Anchor.LastUpdateDate));
        }
    }
}
