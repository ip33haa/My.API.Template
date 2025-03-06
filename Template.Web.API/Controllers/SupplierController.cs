using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Features.F_Suppliers.Commands.Create;
using Template.Application.Features.F_Suppliers.Commands.Delete;
using Template.Application.Features.F_Suppliers.Commands.Update;
using Template.Application.Features.F_Suppliers.Queries.GetSupplierById;
using Template.Application.Features.F_Suppliers.Queries.GetSuppliers;

namespace Template.Web.API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetSupplierQueryResponse>> GetSuppliersAsync()
        {
            var response = await Mediator.Send(new GetSupplierQuery());
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetSupplierByIdQueryResponse>> GetSupplierByIdAsync(Guid id)
        {
            var response = await Mediator.Send(new GetSupplierByIdQuery { Id = id });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateSupplierCommandResponse>> CreateSupplierAsync([FromBody] SupplierDto command)
        {
            var result = await Mediator.Send(new CreateSupplierCommand { Supplier = command });
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UpdateSupplierCommandResponse>> UpdateSupplierAsync(Guid id, [FromBody] SupplierDto command)
        {
            var result = await Mediator.Send(new UpdateSupplierCommand { Id = id, Supplier = command });
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteSupplierCommandResponse>> DeleteSupplierAsync(Guid id)
        {
            var result = await Mediator.Send(new DeleteSupplierCommand { Id = id });
            return Ok(result);
        }
    }
}
