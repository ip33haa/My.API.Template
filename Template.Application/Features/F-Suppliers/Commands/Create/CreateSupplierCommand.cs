using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_Suppliers.Commands.Create
{
    public class CreateSupplierCommand : IRequest<CreateSupplierCommandResponse>
    {
        public SupplierDto Supplier { get; set; }
    }
}
