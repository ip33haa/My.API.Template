using MediatR;

namespace Template.Application.Features.F_Suppliers.Commands.Delete
{
    public class DeleteSupplierCommand : BaseCommandQuery, IRequest<DeleteSupplierCommandResponse>
    {
    }
}
