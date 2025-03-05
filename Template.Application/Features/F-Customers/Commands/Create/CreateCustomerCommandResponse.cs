using Template.Application.Responses;

namespace Template.Application.Features.F_Customers.Commands.Create
{
    public class CreateCustomerCommandResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
