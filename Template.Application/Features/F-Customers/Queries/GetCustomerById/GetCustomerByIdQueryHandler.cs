using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResponse>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

        public GetCustomerByIdQueryHandler(
            IRepository<Customer> repository,
            IMapper mapper,
            ILogger<GetCustomerByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCustomerByIdQueryResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetCustomerByIdQueryResponse();
            var validator = new GetCustomerByIdQueryValidator();

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

                var customer = await _repository.GetByIdAsync(request.Id);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with ID {Id} not found.", request.Id);
                    response.Success = false;
                    response.Message = "Customer not found.";
                    return response;
                }

                response.Success = true;
                response.Customer = _mapper.Map<CustomerDto>(customer);
                _logger.LogInformation("Successfully retrieved customer with ID {Id}.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customer with ID {Id}.", request.Id);
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
