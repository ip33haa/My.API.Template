using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Suppliers.Commands.Update
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, UpdateSupplierCommandResponse>
    {
        private readonly IRepository<Supplier> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSupplierCommandHandler> _logger;

        public UpdateSupplierCommandHandler(
            IRepository<Supplier> repository,
            IMapper mapper,
            ILogger<UpdateSupplierCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpdateSupplierCommandResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateSupplierCommandResponse();
            var validator = new UpdateSupplierCommandValidator();

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

                var entity = await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.Id);
                    response.Success = false;
                    response.Message = "Supplier not found.";
                    return response;
                }

                _mapper.Map(request.Supplier, entity);
                await _repository.UpdateAsync(entity);
                response.Success = true;

                _logger.LogInformation("Supplier with ID {SupplierId} updated successfully.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the supplier.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
