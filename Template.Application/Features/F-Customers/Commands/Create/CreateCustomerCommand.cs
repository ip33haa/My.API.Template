using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_Customers.Commands.Create
{
    public class CreateCustomerCommand : IRequest<CreateCustomerCommandResponse>
    {
        public CustomerDto Customer { get; set; }
    }
}
