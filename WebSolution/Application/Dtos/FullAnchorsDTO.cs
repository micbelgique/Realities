using Application.Common.Mappings;
using Application.Dtos.GeoLocation;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Dtos
{
    public class FullAnchorsDTO : AnchorDTO
    {
        public List<InteractionDTO> Interactions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Anchor, FullAnchorsDTO>()
                .ForMember(d => d.Interactions, opt => opt.MapFrom(s => s.Interactions));
            //.ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id));

            profile.CreateMap<Placement, FullAnchorsDTO>()
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
                .ForMember(d => d.LastUpdateDate, opt => opt.MapFrom(s => s.Anchor.LastUpdateDate))
                .ForMember(d => d.Interactions, opt => opt.MapFrom(s => s.Anchor.Interactions));
        }
    }
}