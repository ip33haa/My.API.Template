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
        private readonly ILogger _logger;

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
                    var entity = _mapper.Map<Customer>(request.Customer);
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
