using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Customers.Commands.Create
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(
            IRepository<Customer> repository,
            IMapper mapper,
            ILogger<CreateCustomerCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCustomerCommandResponse();
            var validator = new CreateCustomerCommandValidator();

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

                var entity = _mapper.Map<Customer>(request.Customer);
                var result = await _repository.AddAsync(entity);

                response.Success = true;
                response.Id = result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a customer.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
