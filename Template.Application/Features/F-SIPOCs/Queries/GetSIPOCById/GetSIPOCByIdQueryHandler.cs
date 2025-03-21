﻿using AutoMapper;
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
                    _logger.LogWarning("SIPOC with Id {Id} not found.", request.Id);
                    return response;
                }

                response.SIPOC = _mapper.Map<SIPOCDto>(entity);
                response.Success = true;

                _logger.LogInformation("SIPOC with Id {Id} retrieved successfully.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving SIPOC with Id {Id}.", request.Id);
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
