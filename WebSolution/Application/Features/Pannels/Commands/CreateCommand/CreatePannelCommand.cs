using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Pannels.Commands.CreateCommand
{
    public class CreatePannelCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PictureUrl { get; set; }
        public string AnchorIndetifier { get; set; }
    }

    public class CreatePannelHandler : IRequestHandler<CreatePannelCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePannelHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> Handle(CreatePannelCommand request, CancellationToken cancellationToken)
        {
            var anchor = _context.Anchors.FirstOrDefault(x => x.Identifier == request.AnchorIndetifier);
            if (anchor == null)
                throw new NotFoundException(nameof(Anchor), request.AnchorIndetifier);


            var entity = new Pannel()
            {
                AnchorIndetifier = request.AnchorIndetifier,
                Content = request.Content,
                Title = request.Title,
                PictureUrl = request.PictureUrl
            };

            _context.Pannels.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;

        }
    }
}
