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
        private readonly ILogger _logger;

        public NotificationHandler(IRepository<Notification> repository, IMapper mapper, ILogger<NotificationHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NotificationResponse> Handle(NotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationResponse = new NotificationResponse();
            var validator = new NotificationValidator();

            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Count > 0)
                {
                    notificationResponse.Success = true;
                    notificationResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors.Select(x => x.ErrorMessage))
                    {
                        notificationResponse.ValidationErrors.Add(error);
                        _logger.LogError($"validation failed due to error- {error}.");
                    }
                }
                else if (notificationResponse.Success)
                {
                    var entity = _mapper.Map<Notification>(request.NotificationDTO);
                    entity.IsSent = true;
                    entity.DateSent = DateTime.UtcNow.AddHours(8);
                    var result = await _repository.AddAsync(entity);
                    notificationResponse.Id = result.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"validation failed due to error- {ex.Message}.");
                notificationResponse.Success = false;
            }

            return notificationResponse;
        }
    }
}
