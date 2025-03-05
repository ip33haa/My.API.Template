using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_Customers.Commands.Update
{
    public class UpdateCustomerCommand : BaseCommandQuery, IRequest<UpdateCustomerCommandResponse>
    {
        public CustomerDto Customer { get; set; }
    }
}
