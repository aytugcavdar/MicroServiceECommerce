using BuildingBlocks.Core.Requests;
using MediatR;

namespace Notification.Application.Features.Notifications.Queries.GetNotificationHistory;

public class GetNotificationHistoryQuery : IRequest<List<GetNotificationHistoryResponse>>
{
    public string? Email { get; set; }
    public PageRequest PageRequest { get; set; } = new();
}
