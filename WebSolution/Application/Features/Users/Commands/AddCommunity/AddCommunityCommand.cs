using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Communities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.AddCommunity
{
    public class AddCommunityCommand : IRequest
    {
        public int CommunityId { get; set; }
        public string UserEmail { get; set; }
    }

    public class AddCommunityHandler : IRequestHandler<AddCommunityCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddCommunityHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(AddCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = _context.Communities.Include(x => x.Admissions).FirstOrDefault(community => community.Id == request.CommunityId);
            if (community == null)
                throw new NotFoundException(nameof(Community), request.CommunityId);

            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(request.UserEmail));
            if(user == null)
                throw new NotFoundException(nameof(User), request.UserEmail);


            if (_context.Admissions.Count(x=> x.User.Id.Equals(user.Id) && x.Community.Id == community.Id) > 0)
                throw new Exception($"user {user.NickName} is already part of {community.Name}");

            
            var admission = new Admission()
            {
                JoiningDate = DateTime.Now,
                User = user,
                Community = community,
                Roles = Community_roles.Member,
                Pending = true
            };

            _context.Admissions.Add(admission);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

