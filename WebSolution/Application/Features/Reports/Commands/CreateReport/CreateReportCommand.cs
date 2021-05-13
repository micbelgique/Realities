using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
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

namespace Application.Features.Reports.Commands.CreateReport
{
    public class CreateReportCommand : IRequest<ReportDTO>
    {
        public string UserId { get; set; }
        public string AnchorIdentifier { get; set; }
        public string Reason { get; set; }
    }

    public class CreateReportHandler : IRequestHandler<CreateReportCommand, ReportDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateReportHandler(IApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReportDTO> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var alreadyRport = _context.Reports.FirstOrDefault(r => r.User.Id == request.UserId && r.Placement.Anchor.Identifier.Equals(request.AnchorIdentifier));
            if(alreadyRport !=  null)
                return _mapper.Map<ReportDTO>(alreadyRport);

            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(request.UserId));
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var placement = _context.Placements.Include(x => x.Anchor).FirstOrDefault(p => p.Anchor.Identifier.Equals(request.AnchorIdentifier));
            if (placement == null)
                throw new NotFoundException(nameof(Anchor), request.AnchorIdentifier);

            if (String.IsNullOrEmpty(request.Reason))
                throw new Exception($"Plase insert a valid reason");

            var report = new Report()
            {
                Placement = placement,
                User = user,
                Reason = request.Reason,
                CreationDate = DateTime.Now
            };

            _context.Reports.Add(report);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReportDTO>(report);

        }
    }
}
