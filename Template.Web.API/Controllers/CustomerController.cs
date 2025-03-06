using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Features.F_Customers.Commands.Create;
using Template.Application.Features.F_Customers.Commands.Delete;
using Template.Application.Features.F_Customers.Commands.Update;
using Template.Application.Features.F_Customers.Queries.GetCustomerById;
using Template.Application.Features.F_Customers.Queries.GetCustomers;

namespace Template.Web.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetCustomerQueryResponse>> GetCustomersAsync()
        {
            var response = await Mediator.Send(new GetCustomerQuery());
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCustomerByIdQueryResponse>> GetCustomerByIdAsync(Guid id)
        {
            var response = await Mediator.Send(new GetCustomerByIdQuery { Id = id });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateCustomerCommandResponse>> CreateCustomerAsync([FromBody] CustomerDto command)
        {
            var result = await Mediator.Send(new CreateCustomerCommand { Customer = command });
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UpdateCustomerCommandResponse>> UpdateCustomerAsync(Guid id, [FromBody] CustomerDto command)
        {
            var result = await Mediator.Send(new UpdateCustomerCommand { Id = id, Customer = command });
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteCustomerCommandResponse>> DeleteCustomerAsync(Guid id)
        {
            var result = await Mediator.Send(new DeleteCustomerCommand { Id = id });
            return Ok(result);
        }
    }
}
