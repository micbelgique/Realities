using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.RemoveCommunity
{
    public class RemoveCommunityCommand : IRequest
    {
        public int CommunityId { get; set; }
        public string UserId { get; set; }
    }

    public class RemoveCommunityHandler : IRequestHandler<RemoveCommunityCommand>
    {
        private readonly IApplicationDbContext _context;

        public RemoveCommunityHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(RemoveCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = _context.Communities.Include(x => x.Admissions).FirstOrDefault(community => community.Id == request.CommunityId);
            if (community == null)
                throw new NotFoundException(nameof(Community), request.CommunityId);

            var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var admission = _context.Admissions.FirstOrDefault(x => x.User.Id == user.Id && x.Community.Id == community.Id);
            if(admission == null)
                throw new Exception($"User: {user.NickName} is not part of {community.Name}");

            _context.Admissions.Remove(admission);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}