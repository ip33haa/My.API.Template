using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Features.F_Notifications.Commands.Create
{
    public class NotificationHandler : IRequestHandler<NotificationCommand, NotificationResponse>
    {
        private readonly IRepository<Notification> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationHandler> _logger;

        public NotificationHandler(IRepository<Notification> repository, IMapper mapper, ILogger<NotificationHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NotificationResponse> Handle(NotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new NotificationResponse();
            var validator = new NotificationValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Any())
                {
                    response.Success = false;
                    response.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                    foreach (var error in response.ValidationErrors)
                    {
                        _logger.LogError("Validation failed: {Error}", error);
                    }

                    return response; // Return early to prevent further execution
                }

                var entity = _mapper.Map<Notification>(request.NotificationDTO);
                entity.IsSent = true;
                entity.DateSent = DateTime.UtcNow.AddHours(8);

                var result = await _repository.AddAsync(entity);
                response.Success = true;
                response.Id = result.Id;

                _logger.LogInformation("Notification {Id} successfully created and sent.", result.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the notification.");
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
            }

            return response;
        }
    }
}
