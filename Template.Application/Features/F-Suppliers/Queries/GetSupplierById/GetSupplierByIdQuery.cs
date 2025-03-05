using MediatR;

namespace Template.Application.Features.F_Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQuery : BaseCommandQuery, IRequest<GetSupplierByIdQueryResponse>
    {
    }
}
