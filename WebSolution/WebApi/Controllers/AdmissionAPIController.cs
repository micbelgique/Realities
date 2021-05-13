using Application.Common.Queries;
using Application.Features.Admissions.Commands.ValidateAdmission;
using Application.Features.Users.Commands.AddCommunity;
using Application.Features.Users.Commands.RemoveCommunity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AdmissionAPIController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AdmitUser([FromBody] ValidateAdmissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("community")]
        public async Task<IActionResult> AddCommunity([FromBody] AddCommunityCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorResponse() { Code = 400, Message = ex.Message });
            }
        }

        [HttpDelete("community")]
        public async Task<IActionResult> RemoveCommunity([FromBody] RemoveCommunityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
