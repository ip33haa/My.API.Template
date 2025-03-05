using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Customers.Queries.GetCustomers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerQueryResponse>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerQueryHandler> _logger;

        public GetCustomerQueryHandler(
            IRepository<Customer> repository,
            IMapper mapper,
            ILogger<GetCustomerQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCustomerQueryResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var response = new GetCustomerQueryResponse();
            var validator = new GetCustomerQueryValidator();

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

                    return response;
                }

                var customers = await _repository.GetAllAsync();
                if (customers == null || !customers.Any())
                {
                    _logger.LogWarning("No customers found.");
                    response.Success = true;
                    response.Customers = new List<CustomerDto>();
                    return response;
                }

                response.Success = true;
                response.Customers = _mapper.Map<List<CustomerDto>>(customers);
                _logger.LogInformation("Successfully retrieved {Count} customers.", customers.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customers.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
