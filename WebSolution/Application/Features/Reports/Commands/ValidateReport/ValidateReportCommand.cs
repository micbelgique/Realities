using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Anchors.Commands.DeleteCommand;
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

namespace Application.Features.Reports.Commands.ValidateReport
{
    public class ValidateReportCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class ValidateReportHandler : IRequestHandler<ValidateReportCommand>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public ValidateReportHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(ValidateReportCommand request, CancellationToken cancellationToken)
        {
            var report = _context.Reports.Include(x=> x.Placement).Include(x => x.Placement.Anchor).FirstOrDefault(x => x.Id == request.Id);
            if (report == null)
                throw new NotFoundException(nameof(Report), request.Id);

            var result = await _mediator.Send(new DeleteAnchorCommand() { Identifier = report.Placement.Anchor.Identifier });

            //_context.Reports.Remove(report);

            return Unit.Value;
        }
    }
}
