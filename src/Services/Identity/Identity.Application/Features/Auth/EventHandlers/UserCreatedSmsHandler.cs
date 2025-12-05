using Identity.Domain.Events;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.EventHandlers;

public class UserCreatedSmsHandler : INotificationHandler<UserCreatedDomainEvent>
{
    // TODO: ISmsService eklenecek
    // private readonly ISmsService _smsService;
    private readonly Serilog.ILogger _logger;

    public UserCreatedSmsHandler()
    {
        _logger = Log.ForContext<UserCreatedSmsHandler>();
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information(
            "📱 Sending welcome SMS to user: {UserId}",
            notification.UserId);

        try
        {
            // TODO: Gerçek SMS servisi eklenecek
            // await _smsService.SendWelcomeSmsAsync(
            //     phoneNumber: notification.PhoneNumber,
            //     firstName: notification.FirstName
            // );

            // Şimdilik sadece log
            _logger.Information(
                "📱 Welcome SMS would be sent to user: {FirstName} {LastName}",
                notification.FirstName,
                notification.LastName);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            // SMS gönderilemese bile kayıt işlemi başarılı olmalı
            _logger.Error(ex,
                "❌ Failed to send welcome SMS to user: {UserId}",
                notification.UserId);
        }
    }
}