using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Suppliers.Commands.Delete
{
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, DeleteSupplierCommandResponse>
    {
        private readonly IRepository<Supplier> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteSupplierCommandHandler> _logger;

        public DeleteSupplierCommandHandler(
            IRepository<Supplier> repository,
            IMapper mapper,
            ILogger<DeleteSupplierCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DeleteSupplierCommandResponse> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteSupplierCommandResponse();
            var validator = new DeleteSupplierCommandValidator();

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

                await _repository.DeleteAsync(entity);
                response.Success = true;

                _logger.LogInformation("Supplier with ID {SupplierId} deleted successfully.", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the supplier.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
