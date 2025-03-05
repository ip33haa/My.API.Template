using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Features.F_SIPOCs.Commands.Create;
using Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById;
using Template.Application.Features.F_SIPOCs.Queries.GetSIPOCs;

namespace Template.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SIPOCController : ApiControllerBase
    {
        [HttpGet("get-SIPOCs")]
        public async Task<ActionResult<GetSIPOCQueryResponse>> GetSIPOCs()
        {
            var response = await Mediator.Send(new GetSIPOCQuery());

            return response;
        }

        [HttpGet("get-SIPOC-by-id/{id}")]
        public async Task<ActionResult<GetSIPOCByIdQueryResponse>> GetSIPOCById(Guid id)
        {
            var response = await Mediator.Send(new GetSIPOCByIdQuery() { Id = id });

            return response;
        }

        [HttpPost("create-sipoc")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateSIPOCCommandResponse>> CreateSIPOC(SIPOCDto command)
        {
            var result = await Mediator.Send(new CreateSIPOCCommand() { SIPOC = command });

            return result;
        }
    }
}
