using Application.Common.Queries;
using Application.Features.Reports.Commands.CancelReport;
using Application.Features.Reports.Commands.CreateReport;
using Application.Features.Reports.Commands.ValidateReport;
using Application.Features.Reports.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ReportsAPIController : ApiControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super_Admin")]
        public async Task<IActionResult> GetAllReports()
        {
            return Ok(await Mediator.Send(new GetAllReportsQuery()));
        }

        [HttpDelete("resolve")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super_Admin")]
        public async Task<IActionResult> ValidateReport([FromBody] ValidateReportCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("resolve")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Super_Admin")]
        public async Task<IActionResult> CancelReport([FromBody] CancelReportCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message, Code = 500 });
            }
        }
    }
}
