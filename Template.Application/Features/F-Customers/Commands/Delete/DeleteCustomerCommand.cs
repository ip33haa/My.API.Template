using MediatR;

namespace Template.Application.Features.F_Customers.Commands.Delete
{
    public class DeleteCustomerCommand : BaseCommandQuery, IRequest<DeleteCustomerCommandResponse>
    {
    }
}
