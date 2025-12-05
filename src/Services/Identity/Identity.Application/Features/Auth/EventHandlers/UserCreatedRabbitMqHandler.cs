using Identity.Domain.Events;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.EventHandlers;

public class UserCreatedRabbitMqHandler : INotificationHandler<UserCreatedDomainEvent>
{
    // TODO: IMessageBus eklenecek (RabbitMQ client)
    // private readonly IMessageBus _messageBus;
    private readonly Serilog.ILogger _logger;

    public UserCreatedRabbitMqHandler()
    {
        _logger = Log.ForContext<UserCreatedRabbitMqHandler>();
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information(
            "🐰 Publishing UserCreatedIntegrationEvent to RabbitMQ for user: {UserId}",
            notification.UserId);

        try
        {
            // TODO: Integration Event oluştur
            // var integrationEvent = new UserCreatedIntegrationEvent
            // {
            //     UserId = notification.UserId,
            //     Email = notification.Email,
            //     FirstName = notification.FirstName,
            //     LastName = notification.LastName,
            //     UserName = notification.UserName,
            //     OccurredOn = notification.OccurredOn
            // };

            // TODO: RabbitMQ'ya publish et
            // await _messageBus.PublishAsync(
            //     exchange: "user-exchange",
            //     routingKey: "user.created",
            //     message: integrationEvent
            // );

            // Şimdilik sadece log
            _logger.Information(
                "🐰 UserCreatedIntegrationEvent would be published: {UserId} - {Email}",
                notification.UserId,
                notification.Email);

            _logger.Debug(
                "📦 Other services (Basket, Notification, Analytics) would consume this event");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            // RabbitMQ'ya gönderilemese bile kayıt işlemi başarılı olmalı
            // Outbox Pattern sayesinde sonra tekrar denenecek
            _logger.Error(ex,
                "❌ Failed to publish UserCreatedIntegrationEvent: {UserId}",
                notification.UserId);
        }
    }
}
