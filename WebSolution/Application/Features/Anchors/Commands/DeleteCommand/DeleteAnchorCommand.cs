using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Anchors.Commands.DeleteCommand
{
    public class DeleteAnchorCommand : IRequest
    {
        public string Identifier { get; set; }
    }

    public class DeleteAnchorHandler : IRequestHandler<DeleteAnchorCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAnchorHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAnchorCommand request, CancellationToken cancellationToken)
        {

            var entity = _context.Placements.Include(p => p.Anchor).Where(placement => placement.Anchor.Identifier == request.Identifier).FirstOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Identifier);
            }

            _context.Reports.RemoveRange(_context.Reports.Where(r => r.Placement.Id == entity.Id).ToList());
            _context.Interactions.RemoveRange(_context.Interactions.Where(i => i.Anchor.Id == entity.Anchor.Id).ToList());

            _context.Placements.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;            
        }
    }
}
