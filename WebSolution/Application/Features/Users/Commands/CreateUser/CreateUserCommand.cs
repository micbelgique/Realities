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
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public String NickName { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public String Password { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String SocialMedia { get; set; }
        public String Enterprise { get; set; }
        public String Mission { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CreateUserHandler(IApplicationDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.NickName,
                Name = string.IsNullOrEmpty(request.Name) ? "" : request.Name,
                Surname = string.IsNullOrEmpty(request.Surname) ? "" : request.Surname,
                Email = request.Email, 
                PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? "" : request.PhoneNumber,
                Enterprise = string.IsNullOrEmpty(request.Enterprise) ? "" : request.Enterprise,
                Mission = string.IsNullOrEmpty(request.Mission) ? "" : request.Mission
            };

            if (string.IsNullOrEmpty(request.Password))
                request.Password = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
            }
            else
            {
                string message = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new DuplicateException(message);
            }
            
            return user.Id;
        }
    }
}
