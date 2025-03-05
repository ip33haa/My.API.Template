using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCs
{
    public class GetSIPOCQueryHandler : IRequestHandler<GetSIPOCQuery, GetSIPOCQueryResponse>
    {
        private readonly IRepository<SIPOC> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSIPOCQueryHandler> _logger;

        public GetSIPOCQueryHandler(
            IRepository<SIPOC> repository,
            IMapper mapper,
            ILogger<GetSIPOCQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSIPOCQueryResponse> Handle(GetSIPOCQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSIPOCQueryResponse();
            var validator = new GetSIPOCQueryValidator();

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

                var result = await _repository.GetAllAsync();
                if (result == null || !result.Any())
                {
                    response.Success = false;
                    response.Message = "No SIPOCs found.";
                    _logger.LogWarning("No SIPOCs found in the database.");
                    return response;
                }

                response.SIPOCs = _mapper.Map<List<SIPOCDto>>(result);
                response.Success = true;

                _logger.LogInformation("{Count} SIPOCs retrieved successfully.", response.SIPOCs.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving SIPOCs.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
