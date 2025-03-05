using FluentValidation;

namespace Template.Application.Features.F_Suppliers.Commands.Update
{
    public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierCommandValidator()
        {
            RuleFor(p => p.Supplier).NotEmpty().NotNull().WithMessage("Supplier is required.");
        }
    }
}
