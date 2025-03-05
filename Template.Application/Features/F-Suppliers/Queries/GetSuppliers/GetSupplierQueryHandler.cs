using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Suppliers.Queries.GetSuppliers
{
    public class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, GetSupplierQueryResponse>
    {
        private readonly IRepository<Supplier> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSupplierQueryHandler> _logger;

        public GetSupplierQueryHandler(
            IRepository<Supplier> repository,
            IMapper mapper,
            ILogger<GetSupplierQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSupplierQueryResponse> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSupplierQueryResponse();
            var validator = new GetSupplierQueryValidator();

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

                    return response; // Return early if validation fails
                }

                var result = await _repository.GetAllAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No suppliers found.");
                    response.Success = false;
                    response.Message = "No suppliers found.";
                    return response;
                }

                response.Suppliers = _mapper.Map<List<SupplierDto>>(result);
                response.Success = true;

                _logger.LogInformation("Retrieved {Count} suppliers successfully.", result.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving suppliers.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
