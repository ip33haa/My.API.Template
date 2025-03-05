using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQueryResponse : BaseResponse
    {
        public SupplierDto Supplier { get; set; }
    }
}
