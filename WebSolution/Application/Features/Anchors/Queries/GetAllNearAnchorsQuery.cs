using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Helpers;
using NetTopologySuite.Geometries;
using System.ComponentModel;

namespace Application.Features.Anchors.Queries
{
    public class GetAllNearAnchorsQuery : IRequest<Response<AnchorDTO>>
    {
        [DefaultValue("0")]
        public double Longitude { get; set; }
        [DefaultValue("0")]
        public double Latitude { get; set; }

        [DefaultValue("visitor")]
        public string UserId { get; set; }
    }

    public class GetAllNearAnchorsHandler : IRequestHandler<GetAllNearAnchorsQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllNearAnchorsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<AnchorDTO>> Handle(GetAllNearAnchorsQuery request, CancellationToken cancellationToken)
        {
            var admissions = _context.Admissions.Where(a => a.User.Id == request.UserId && a.Pending == false);
            var userLocation = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

            var anchors = _context.Placements
                       .Where(x => x.Visibility == PlacementVisibility.Public)
                       .Union(_context.Placements.Where(x => x.Visibility == PlacementVisibility.Private && x.User.Id.Equals(request.UserId)))
                       .Union(_context.Placements.Where(x => x.Community != null && x.Visibility == PlacementVisibility.Group)
                        .Where(x => admissions.Select(x => x.Community.Id).ToList().Contains(x.Community.Id)))
                       .OrderByDescending(x => x.Anchor.CreationDate)
                       .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider);


            var anchorList = new List<AnchorDTO>();
            foreach (var anchor in anchors.ToList())
            {
                var anchorPos = new Point(anchor.Location.Longitude, anchor.Location.Latitude) { SRID = 4326 };
                var distanceInMeters = anchorPos.ProjectTo(2855).Distance(userLocation.ProjectTo(2855));
                if (distanceInMeters <= 3000)
                    anchorList.Add(anchor);
            }

            return new Response<AnchorDTO>()
            {
                Data = anchorList
            };
        }
    }
}
