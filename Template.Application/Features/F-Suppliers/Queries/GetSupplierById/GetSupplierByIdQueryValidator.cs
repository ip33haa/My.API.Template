using FluentValidation;

namespace Template.Application.Features.F_Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQueryValidator : AbstractValidator<GetSupplierByIdQuery>
    {

        public GetSupplierByIdQueryValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
