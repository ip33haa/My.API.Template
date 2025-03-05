using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById
{
    public class GetSIPOCByIdQueryHandler : IRequestHandler<GetSIPOCByIdQuery, GetSIPOCByIdQueryResponse>
    {
        private readonly IRepository<SIPOC> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSIPOCByIdQueryHandler> _logger;

        public GetSIPOCByIdQueryHandler(
            IRepository<SIPOC> repository,
            IMapper mapper,
            ILogger<GetSIPOCByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetSIPOCByIdQueryResponse> Handle(GetSIPOCByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSIPOCByIdQueryResponse();
            var validator = new GetSIPOCByIdQueryValidator(_repository);

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
                    var result = await _repository.GetByIdAsync(request.Id);
                    response.SIPOC = _mapper.Map<SIPOCDto>(result);
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
