using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Features.F_Suppliers.Commands.Create;
using Template.Application.Features.F_Suppliers.Queries.GetSupplierById;
using Template.Application.Features.F_Suppliers.Queries.GetSuppliers;

namespace Template.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ApiControllerBase
    {
        [HttpGet("get-Suppliers")]
        public async Task<ActionResult<GetSupplierQueryResponse>> GetSuppliers()
        {
            var response = await Mediator.Send(new GetSupplierQuery());

            return response;
        }

        [HttpGet("get-Supplier-by-id/{id}")]
        public async Task<ActionResult<GetSupplierByIdQueryResponse>> GetSupplierById(Guid id)
        {
            var response = await Mediator.Send(new GetSupplierByIdQuery() { Id = id });

            return response;
        }

        [HttpPost("create-Supplier")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateSupplierCommandResponse>> CreateSupplier(SupplierDto command)
        {
            var result = await Mediator.Send(new CreateSupplierCommand() { Supplier = command });

            return result;
        }
    }
}
