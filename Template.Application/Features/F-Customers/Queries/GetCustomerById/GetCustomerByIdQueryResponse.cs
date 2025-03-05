using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryResponse : BaseResponse
    {
        public CustomerDto Customer { get; set; }
    }
}
