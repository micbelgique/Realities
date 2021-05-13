using Application.Features.Anchors.Commands.CreateCommand;
using Application.Features.Anchors.Commands.DeleteCommand;
using Application.Features.Anchors.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AnchorsAPIController : ApiControllerBase
    {
        private readonly ILogger<AnchorsAPIController> _logger;

        public AnchorsAPIController(ILogger<AnchorsAPIController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAnchor([FromBody] CreateAnchorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnchors()
        {
            return Ok(await Mediator.Send(new GetAllAnchorsQuery()));
        }

        [HttpGet("near")]
        public async Task<IActionResult> GetAllAnchors([FromQuery] GetAllNearAnchorsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("forUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAnchorsForUser([FromQuery] GetAllAnchorsForUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetAnchor(string identifier)
        {
            return Ok(await Mediator.Send(new GetAnchorByIdentifierQuery(identifier)));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAnchor([FromBody] DeleteAnchorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        
        [HttpGet("byCommunity")]
        public async Task<IActionResult> GetAnchorsByCommunity([FromQuery] GetAnchorsByCommunityQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        
        /*
        [HttpGet("inCommunity")]
        public async Task<IActionResult> GetAnchorsIyCommunity([FromQuery] GetAnchorsInCommunityQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
         */

        [HttpGet("user")]
        public async Task<IActionResult> GetAnchorsByUser([FromQuery] GetAnchorsByUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        
        

    }
}
