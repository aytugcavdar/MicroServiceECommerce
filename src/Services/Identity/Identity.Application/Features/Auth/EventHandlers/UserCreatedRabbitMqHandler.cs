using BuildingBlocks.Messaging.IntegrationEvents;
using Identity.Domain.Events;
using MassTransit;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.EventHandlers;

public class UserCreatedRabbitMqHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public UserCreatedRabbitMqHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _logger = Log.ForContext<UserCreatedRabbitMqHandler>();
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information(
            "🐰 UserCreatedIntegrationEvent hazırlanıyor: {UserId}",
            notification.UserId);

        try
        {
            var integrationEvent = new UserCreatedIntegrationEvent
            {
                UserId = notification.UserId,
                Email = notification.Email,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                UserName = notification.UserName,
                EmailConfirmationToken = notification.EmailConfirmationToken, 
                OccurredOn = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);

            _logger.Information(
                "✅ UserCreatedIntegrationEvent başarıyla RabbitMQ'ya gönderildi: {UserId}",
                notification.UserId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "❌ RabbitMQ mesaj gönderimi başarısız: {UserId}",
                notification.UserId);
            throw;
        }
    }
}