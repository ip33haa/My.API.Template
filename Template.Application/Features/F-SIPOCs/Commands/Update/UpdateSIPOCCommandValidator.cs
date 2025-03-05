using FluentValidation;

namespace Template.Application.Features.F_SIPOCs.Commands.Update
{
    public class UpdateSIPOCCommandValidator : AbstractValidator<UpdateSIPOCCommand>
    {
        public UpdateSIPOCCommandValidator()
        {
            RuleFor(p => p.SIPOC).NotEmpty().NotNull().WithMessage("SIPOC is required.");
        }
    }
}
