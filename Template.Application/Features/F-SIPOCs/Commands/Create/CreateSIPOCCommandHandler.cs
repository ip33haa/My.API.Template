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
        private readonly ILogger _logger;

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
                var validationResult = await validator.ValidateAsync(request, new CancellationToken());
                if (validationResult.Errors.Count > 0)
                {
                    response.Success = false;
                    response.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        response.ValidationErrors.Add(error);
                        _logger.LogError($"validation failed due to error- {error}.");
                    }
                }
                else if (response.Success)
                {
                    var entity = _mapper.Map<SIPOC>(request.SIPOC);
                    var result = await _repository.AddAsync(entity);
                    response.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while due to error- {ex.Message}");
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
