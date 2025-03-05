using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Features.F_Customers.Commands.Create;
using Template.Application.Features.F_Customers.Commands.Delete;
using Template.Application.Features.F_Customers.Commands.Update;
using Template.Application.Features.F_Customers.Queries.GetCustomerById;
using Template.Application.Features.F_Customers.Queries.GetCustomers;

namespace Template.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ApiControllerBase
    {
        [HttpGet("get-customers")]
        public async Task<ActionResult<GetCustomerQueryResponse>> GetCustomers()
        {
            var response = await Mediator.Send(new GetCustomerQuery());

            return response;
        }

        [HttpGet("get-customer-by-id/{id}")]
        public async Task<ActionResult<GetCustomerByIdQueryResponse>> GetCustomerById(Guid id)
        {
            var response = await Mediator.Send(new GetCustomerByIdQuery() { Id = id });

            return response;
        }

        [HttpPost("create-customer")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateCustomerCommandResponse>> CreateCustomer(CustomerDto command)
        {
            var result = await Mediator.Send(new CreateCustomerCommand() { Customer = command });

            return result;
        }

        [HttpPut("update-customer/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UpdateCustomerCommandResponse>> UpdateCustomer(Guid id, CustomerDto command)
        {
            var result = await Mediator.Send(new UpdateCustomerCommand() { Id = id, Customer = command });

            return result;
        }

        [HttpDelete("delete-customer/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteCustomerCommandResponse>> DeleteCustomer(Guid id)
        {
            var result = await Mediator.Send(new DeleteCustomerCommand() { Id = id });

            return result;
        }
    }
}
