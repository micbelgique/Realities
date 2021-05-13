using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Applications.Dtos.GeoLocation;
using Domain.Entities;
using Domain.Entities.Communities;
using MediatR;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands.CreateCommunity
{
    public class CreateCommunityCommand : IRequest<int>
    {
        public String Name { get; set; }
        public String PictureUrl { get; set; }
        public EpicenterDTO EpiCenter { get; set; }
        public String InfoUrl { get; set; }
        public String Address { get; set; }
        public bool IsLocated { get; set; }

        public string UserId { get; set; }

    }

    public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCommunityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var entity = new Community()
            {
                Name = request.Name,
                PictureUrl = request.PictureUrl,
                Address = request.Address,
                EpiCenter = new Point(request.EpiCenter.Longitude, request.EpiCenter.Latitude)
                { SRID = request.EpiCenter.SRID },
                IsLocated = request.IsLocated,
                EpiCenterRadius = request.EpiCenter.Radius,
                InfoUrl = request.InfoUrl
            };

            _context.Communities.Add(entity);

            var admission = new Admission() { Community = entity, Pending = false, JoiningDate = DateTime.Now, Roles = Community_roles.Creator, User = user };
            _context.Admissions.Add(admission);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
