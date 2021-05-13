using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetSimpleUserByIdQuery : IRequest<UserDTO>
    {
        public string UserId { get; set; }
    }

    public class GetSimpleUserByIdHandler : IRequestHandler<GetSimpleUserByIdQuery, UserDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetSimpleUserByIdHandler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserDTO> Handle(GetSimpleUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userDto = _context.Users
                .Where(user => user.Id == request.UserId)
                //.Include(user => user.Community)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (userDto == null)
                throw new NotFoundException(nameof(userDto), request.UserId);

            var user = await _userManager.FindByIdAsync(request.UserId);
            userDto.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            return userDto;
        }
    }
}
