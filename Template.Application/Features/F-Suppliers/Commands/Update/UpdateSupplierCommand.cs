using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_Suppliers.Commands.Update
{
    public class UpdateSupplierCommand : BaseCommandQuery, IRequest<UpdateSupplierCommandResponse>
    {
        public SupplierDto Supplier { get; set; }
    }
}
