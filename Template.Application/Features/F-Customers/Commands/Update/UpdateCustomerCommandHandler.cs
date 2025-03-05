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
        private readonly ILogger _logger;

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
                var validationResult = await validator.ValidateAsync(request, new CancellationToken());
                if (validationResult.Errors.Count > 0)
                {
                    response.Success = false;
                    response.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        response.ValidationErrors.Add(error);
                        _logger.LogError($"validation failed due to error- {error}");
                    }
                }
                else if (response.Success)
                {
                    var entity = await _repository.GetByIdAsync(request.Id);
                    _mapper.Map(request.Customer, entity);
                    await _repository.UpdateAsync(entity);
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
