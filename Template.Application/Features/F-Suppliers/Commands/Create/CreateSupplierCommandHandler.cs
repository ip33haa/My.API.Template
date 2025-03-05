using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Suppliers.Commands.Create
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, CreateSupplierCommandResponse>
    {
        private readonly IRepository<Supplier> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSupplierCommandHandler> _logger;

        public CreateSupplierCommandHandler(
            IRepository<Supplier> repository,
            IMapper mapper,
            ILogger<CreateSupplierCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateSupplierCommandResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateSupplierCommandResponse();
            var validator = new CreateSupplierCommandValidator();

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

                var entity = _mapper.Map<Supplier>(request.Supplier);
                var result = await _repository.AddAsync(entity);
                response.Id = result.Id;
                response.Success = true;

                _logger.LogInformation("Supplier {SupplierId} created successfully.", response.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the supplier.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
