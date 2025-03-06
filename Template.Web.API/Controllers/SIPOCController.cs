using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Features.F_SIPOCs.Commands.Create;
using Template.Application.Features.F_SIPOCs.Commands.Delete;
using Template.Application.Features.F_SIPOCs.Commands.Update;
using Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById;
using Template.Application.Features.F_SIPOCs.Queries.GetSIPOCs;

namespace Template.Web.API.Controllers
{
    [Route("api/sipocs")]
    [ApiController]
    public class SIPOCController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetSIPOCQueryResponse>> GetSIPOCsAsync()
        {
            var response = await Mediator.Send(new GetSIPOCQuery());
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetSIPOCByIdQueryResponse>> GetSIPOCByIdAsync(Guid id)
        {
            var response = await Mediator.Send(new GetSIPOCByIdQuery { Id = id });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateSIPOCCommandResponse>> CreateSIPOCAsync([FromBody] SIPOCDto command)
        {
            var result = await Mediator.Send(new CreateSIPOCCommand { SIPOC = command });
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UpdateSIPOCCommandResponse>> UpdateSIPOCAsync(Guid id, [FromBody] SIPOCDto command)
        {
            var result = await Mediator.Send(new UpdateSIPOCCommand { Id = id, SIPOC = command });
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteSIPOCCommandResponse>> DeleteSIPOCAsync(Guid id)
        {
            var result = await Mediator.Send(new DeleteSIPOCCommand { Id = id });
            return Ok(result);
        }
    }
}
