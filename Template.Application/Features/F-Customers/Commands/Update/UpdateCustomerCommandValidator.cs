using FluentValidation;

namespace Template.Application.Features.F_Customers.Commands.Update
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {

            RuleFor(p => p.Customer.CustomerName)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have value.");
        }
    }
}
