using Application.Features.Users.Commands.AddCommunity;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Common.Queries;
using Application.Features.Users.Commands.RemoveCommunity;

namespace WebApi.Controllers
{
    public class UsersAPIController : ApiControllerBase
    {
        private readonly ILogger<UsersAPIController> _logger;

        public UsersAPIController(ILogger<UsersAPIController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            _logger.Log(LogLevel.Debug, $"user with ID {id} has been requested");
            return Ok(await Mediator.Send(new GetSimpleUserByIdQuery() { UserId = id }));
        }

        [HttpGet("private/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDetailedUserByID(string id)
        {
            _logger.Log(LogLevel.Debug, $"user with ID {id} has been requested");
            return Ok(await Mediator.Send(new GetUserByIdQuery() { UserId = id }));
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserCommand command)
        {
            try
            {
                return await GetUserByID(await Mediator.Send(command));
            }
            catch (Exception ex)
            {

                return UnprocessableEntity(new ErrorResponse() { Code = 422, Message = ex.Message});
            }
            
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody] DeleteUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
