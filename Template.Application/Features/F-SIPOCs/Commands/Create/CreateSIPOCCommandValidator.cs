using FluentValidation;

namespace Template.Application.Features.F_SIPOCs.Commands.Create
{
    public class CreateSIPOCCommandValidator : AbstractValidator<CreateSIPOCCommand>
    {
        public CreateSIPOCCommandValidator()
        {
            RuleFor(p => p.SIPOC.SupplierId)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.Input)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.Challenges1)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.Process)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.Challenges2)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.Outputs)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
            RuleFor(p => p.SIPOC.CustomerId)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
