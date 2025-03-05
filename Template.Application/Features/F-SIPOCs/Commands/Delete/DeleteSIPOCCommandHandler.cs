using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_SIPOCs.Commands.Delete
{
    public class DeleteSIPOCCommandHandler : IRequestHandler<DeleteSIPOCCommand, DeleteSIPOCCommandResponse>
    {
        private readonly IRepository<SIPOC> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteSIPOCCommandHandler> _logger;

        public DeleteSIPOCCommandHandler(
            IRepository<SIPOC> repository,
            IMapper mapper,
            ILogger<DeleteSIPOCCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DeleteSIPOCCommandResponse> Handle(DeleteSIPOCCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteSIPOCCommandResponse();
            var validator = new DeleteSIPOCCommandValidator();

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

                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "SIPOC not found.";
                    _logger.LogWarning("Attempted to delete SIPOC with Id {Id}, but it was not found.", request.Id);
                    return response;
                }

                await _repository.DeleteAsync(entity);

                response.Success = true;
                _logger.LogInformation("SIPOC with Id {Id} successfully deleted.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting SIPOC with Id {Id}.", request.Id);
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
