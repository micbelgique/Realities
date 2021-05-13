using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Anchors.Queries
{
    public class GetAnchorsByCommunityQuery : IRequest<Response<AnchorDTO>>
    {
        public int CommunityId { get; set; }
    }

    public class GetAnchorsByCommunityHandler : IRequestHandler<GetAnchorsByCommunityQuery, Response<AnchorDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAnchorsByCommunityHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Response<AnchorDTO>> Handle(GetAnchorsByCommunityQuery request, CancellationToken cancellationToken)
        {
            var anchorsIn = _context.Placements.Where(p => p.Community.Id == request.CommunityId)
                .OrderByDescending(x => x.Anchor.CreationDate)
                .ProjectTo<AnchorDTO>(_mapper.ConfigurationProvider);

            return new Response<AnchorDTO>()
            {
                Data = anchorsIn.ToList()
            };
        }
    }
}