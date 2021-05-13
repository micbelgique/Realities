using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Anchors.Queries
{
    public class GetAllAnchorsForUserQuery: IRequest<Response<AnchorDTO>>
    {
        [DefaultValue("visitor")]
        public string UserId { get; set; }
    }

    public class GetAllAnchorsForUserHandler : IRequestHandler<GetAllAnchorsForUserQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllAnchorsForUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<AnchorDTO>> Handle(GetAllAnchorsForUserQuery request, CancellationToken cancellationToken)
        {
            IQueryable<AnchorDTO> anchors;
            var admissions = _context.Admissions.Where(a => a.User.Id == request.UserId && a.Pending == false);

            anchors = _context.Placements
                       .Where(x => x.Visibility == PlacementVisibility.Public)
                       .Union(_context.Placements.Where(x => x.Visibility == PlacementVisibility.Private && x.User.Id.Equals(request.UserId)))
                       .Union(_context.Placements.Where(x => x.Community != null && x.Visibility == PlacementVisibility.Group)
                        .Where(x => admissions.Select(x => x.Community.Id).ToList().Contains(x.Community.Id)))
                       .OrderByDescending(x => x.Anchor.CreationDate)
                       .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider);

            return new Response<AnchorDTO>()
            {
                Data = anchors.ToList()
            };
        }

    }
}
