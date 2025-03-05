using MediatR;

namespace Template.Application.Features.F_Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : BaseCommandQuery, IRequest<GetCustomerByIdQueryResponse>
    {
    }
}
