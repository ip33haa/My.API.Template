using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_Customers.Queries.GetCustomers
{
    public class GetCustomerQueryResponse : BaseResponse
    {
        public List<CustomerDto> Customers { get; set; }
    }
}
