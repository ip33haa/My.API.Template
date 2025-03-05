using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_Suppliers.Queries.GetSuppliers
{
    public class GetSupplierQueryResponse : BaseResponse
    {
        public List<SupplierDto> Suppliers { get; set; }
    }
}
