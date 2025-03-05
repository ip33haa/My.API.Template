using FluentValidation;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById
{
    public class GetSIPOCByIdQueryValidator : AbstractValidator<GetSIPOCByIdQuery>
    {
        private readonly IRepository<SIPOC> _repository;

        public GetSIPOCByIdQueryValidator(IRepository<SIPOC> repository)
        {
            _repository = repository;

            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");

            // Custom validation to check if the SIPOC with the provided Id exists
            RuleFor(p => p.Id)
                .MustAsync(async (id, cancellationToken) => await SipocExistsAsync(id, cancellationToken))
                .WithMessage("SIPOC not found with the provided Id.");
        }

        // Custom async method to check if SIPOC exists in the database
        private async Task<bool> SipocExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            var sipoc = await _repository.GetByIdAsync(id);
            return sipoc != null; // If sipoc is null, it means it wasn't found.
        }
    }
}
