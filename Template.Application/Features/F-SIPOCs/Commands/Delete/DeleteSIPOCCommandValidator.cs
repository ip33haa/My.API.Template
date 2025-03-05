using FluentValidation;

namespace Template.Application.Features.F_SIPOCs.Commands.Delete
{
    public class DeleteSIPOCCommandValidator : AbstractValidator<DeleteSIPOCCommand>
    {
        public DeleteSIPOCCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} should have value.");
        }
    }
}
