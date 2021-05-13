using Application.Common.Exceptions;
using Application.Common.Queries;
using Application.Features.Pannels.Commands.CreateCommand;
using Application.Features.Pannels.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PannelsAPIController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostPannel([FromBody] CreatePannelCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getPannel(string id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetPannelByIdentifierQuery() { Identifier = id }));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorResponse() { Code = 404, Message = ex.Message });
            }
        }
    }
}
