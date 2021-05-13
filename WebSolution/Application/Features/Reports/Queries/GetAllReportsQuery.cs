using Application.Common.Interfaces;
using Application.Common.Queries;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reports.Queries
{
    public class GetAllReportsQuery : IRequest<Response<ReportDTO>>
    {

    }

    public class GetAllReportsHandler : IRequestHandler<GetAllReportsQuery, Response<ReportDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllReportsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ReportDTO>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            var reports = _context.Reports.OrderBy(r => r.CreationDate).ProjectTo<ReportDTO>(_mapper.ConfigurationProvider);
            return new Response<ReportDTO>
            {
                Data = reports.ToList()
            };
        }
    }
}
