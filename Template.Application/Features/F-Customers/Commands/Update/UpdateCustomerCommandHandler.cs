using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Customers.Commands.Update
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerCommandResponse>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;

        public UpdateCustomerCommandHandler(
            IRepository<Customer> repository,
            IMapper mapper,
            ILogger<UpdateCustomerCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpdateCustomerCommandResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateCustomerCommandResponse();
            var validator = new UpdateCustomerCommandValidator();

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

                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("Customer with ID {Id} not found.", request.Id);
                    response.Success = false;
                    response.Message = "Customer not found.";
                    return response;
                }

                _mapper.Map(request.Customer, entity);
                await _repository.UpdateAsync(entity);

                response.Success = true;
                _logger.LogInformation("Successfully updated customer with ID {Id}.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating customer with ID {Id}.", request.Id);
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
