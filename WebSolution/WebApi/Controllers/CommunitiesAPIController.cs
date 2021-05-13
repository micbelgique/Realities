using Application.Dtos;
using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Commands.DeleteCommunity;
using Application.Features.Communities.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class CommunitiesAPIController : ApiControllerBase
    {
        private readonly ILogger<CommunitiesAPIController> _logger;
        private readonly UserManager<User> _userManager;

        public CommunitiesAPIController(ILogger<CommunitiesAPIController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCommunitiesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunity(int id, Boolean full = false)
        {
            if (full)
            {
                var result = await Mediator.Send(new GetFullCommunityQuery() { Id = id });
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
            }
            else
            {
                var result = await Mediator.Send(new GetCommunityQuery() { Id = id });
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
            }
            

            
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] CreateCommunityCommand command, string userId)
        {
            command.UserId = userId;
            //command.UserId = _userManager.GetUserId(User); ;

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCommunityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
