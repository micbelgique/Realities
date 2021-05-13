using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Pannels.Queries
{
    public class GetPannelByIdentifierQuery : IRequest<PannelDTO>
    {
        public string Identifier { get; set; }
    }

    public class GetPannelByIdentifierHandler : IRequestHandler<GetPannelByIdentifierQuery, PannelDTO>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetPannelByIdentifierHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PannelDTO> Handle(GetPannelByIdentifierQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Pannels.Where(x => x.AnchorIndetifier.Equals(request.Identifier))
                .ProjectTo<PannelDTO>(_mapper.ConfigurationProvider).FirstOrDefault();

            if (entity == null)
                throw new NotFoundException(nameof(Pannel), request.Identifier);

            return entity;
        }
    }

}
