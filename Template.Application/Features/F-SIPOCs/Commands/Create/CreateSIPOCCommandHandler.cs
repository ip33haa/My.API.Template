using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_SIPOCs.Commands.Create
{
    public class CreateSIPOCCommandHandler : IRequestHandler<CreateSIPOCCommand, CreateSIPOCCommandResponse>
    {
        private readonly IRepository<SIPOC> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSIPOCCommandHandler> _logger;

        public CreateSIPOCCommandHandler(
            IRepository<SIPOC> repository,
            IMapper mapper,
            ILogger<CreateSIPOCCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateSIPOCCommandResponse> Handle(CreateSIPOCCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateSIPOCCommandResponse();
            var validator = new CreateSIPOCCommandValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Any())
                {
                    response.Success = false;
                    response.ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    foreach (var error in response.ValidationErrors)
                    {
                        _logger.LogError("Validation failed: {Error}", error);
                    }

                    return response; // Return early to prevent unnecessary execution
                }

                var entity = _mapper.Map<SIPOC>(request.SIPOC);
                var result = await _repository.AddAsync(entity);

                response.Success = true;
                response.Id = result.Id;

                _logger.LogInformation("SIPOC {Id} successfully created.", result.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating SIPOC.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
