using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.CancelReport
{
    public class CancelReportCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class CancleReportHandler : IRequestHandler<CancelReportCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CancleReportHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(CancelReportCommand request, CancellationToken cancellationToken)
        {
            var report = _context.Reports.Include(x => x.Placement).Include(x => x.Placement.Anchor).FirstOrDefault(x => x.Id == request.Id);
            if (report == null)
                throw new NotFoundException(nameof(Report), request.Id);

            var reports = _context.Reports.Where(x => x.Placement.Id == report.Placement.Id);
            _context.Reports.RemoveRange(reports.ToList());

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
