using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Suppliers.Queries.GetSuppliers
{
    public class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, GetSupplierQueryResponse>
    {
        private readonly IRepository<Supplier> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSupplierQueryHandler> _logger;

        public GetSupplierQueryHandler(
            IRepository<Supplier> repository,
            IMapper mapper,
            ILogger<GetSupplierQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetSupplierQueryResponse> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSupplierQueryResponse();
            var validator = new GetSupplierQueryValidator();

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
                    var result = await _repository.GetAllAsync();
                    response.Suppliers = _mapper.Map<List<SupplierDto>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while due to error- {ex.Message}.");
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
