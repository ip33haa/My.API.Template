using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Customers.Commands.Delete
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerCommandResponse>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(
            IRepository<Customer> repository,
            IMapper mapper,
            ILogger<DeleteCustomerCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DeleteCustomerCommandResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteCustomerCommandResponse();
            var validator = new DeleteCustomerCommandValidator();

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

                await _repository.DeleteAsync(entity);

                response.Success = true;
                _logger.LogInformation("Successfully deleted customer with ID {Id}.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting customer with ID {Id}.", request.Id);
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
