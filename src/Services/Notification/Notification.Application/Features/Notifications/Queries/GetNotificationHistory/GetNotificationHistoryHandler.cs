using MediatR;
using Notification.Application.Services;

namespace Notification.Application.Features.Notifications.Queries.GetNotificationHistory;

public class GetNotificationHistoryHandler : IRequestHandler<GetNotificationHistoryQuery, List<GetNotificationHistoryResponse>>
{
    private readonly INotificationLogRepository _repository;

    public GetNotificationHistoryHandler(INotificationLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GetNotificationHistoryResponse>> Handle(GetNotificationHistoryQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.GetByEmailAsync(
            request.Email,
            request.PageRequest.PageIndex,
            request.PageRequest.PageSize,
            cancellationToken);

        return notifications.Select(x => new GetNotificationHistoryResponse
        {
            Id = x.Id,
            Type = x.Type,
            RecipientEmail = x.RecipientEmail,
            Subject = x.Subject,
            Status = x.Status,
            SentAt = x.SentAt,
            CreatedDate = x.CreatedDate
        }).ToList();
    }
}
