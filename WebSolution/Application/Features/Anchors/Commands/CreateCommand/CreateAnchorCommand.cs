using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using System.Net.Http;
using Newtonsoft.Json;
using Domain.GeoFencing;
using Application.Common.Queries;

namespace Application.Features.Anchors.Commands.CreateCommand
{
    public class CreateAnchorCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public Nullable<int> CommunityId {get; set;}
        public PlacementVisibility Visibility { get; set; }

        public DateTime CreationDate { get; set; }

        public string Identifier { get; set; }
        public string Model { get; set; }
        public float Size { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int SRID { get; set; }
    }

    public class CreateAnchorCommandHandler : IRequestHandler<CreateAnchorCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateAnchorCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateAnchorCommand request, CancellationToken cancellationToken)
        {

            request.CreationDate = DateTime.Now; //request.CreationDate == default ? DateTime.Now : request.CreationDate;

            var user = _context.Users.Find(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var community = _context.Communities.Find(request.CommunityId);

            var now = request.CreationDate;

            string adresse = "unkown";
            using (var httpClient = new HttpClient())
            {
                var apiKey = "2e0ba36a1b59732cafdb7f8b4d8decaa";
                var response = await httpClient.GetAsync($"http://api.positionstack.com/v1/reverse?access_key={apiKey}&query={request.Latitude},{request.Longitude}&limit=1");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<Response<Result>>(responseContent);
                    adresse = responseObject.Data.FirstOrDefault().county;
                }
            }

            var placement = new Placement()
            {
                Anchor = new Anchor()
                {
                    Identifier = request.Identifier,
                    Model = request.Model,
                    Address = adresse,
                    Size = request.Size,
                    CreationDate = now,
                    LastUpdateDate = now,
                    Location = new Point(request.Longitude, request.Latitude) { SRID = request.SRID }
                },
                User = user,
                Visibility = request.Visibility
            };   

            switch (request.Visibility)
            {
                case PlacementVisibility.Private:
                    placement.Community = null;
                    break;
                case PlacementVisibility.Group:
                    if(community == null)
                        throw new NotFoundException(nameof(Community), request.CommunityId);

                    placement.Community = community;
                    break;
                case PlacementVisibility.Public:
                    placement.Community = community;
                    break;
                default:
                    throw new Exception($"Visibility not 0:Private | 1:Group | 2:Public");
                    break;
            }

            

            _context.Placements.Add(placement);

            await _context.SaveChangesAsync(cancellationToken);

            return placement.Id;
        }
    }
}
