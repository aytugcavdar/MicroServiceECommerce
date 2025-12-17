using BuildingBlocks.Messaging.Email;
using Identity.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.EventHandlers;

public class UserCreatedEmailHandler:INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly Serilog.ILogger _logger;

    public UserCreatedEmailHandler(IEmailService emailService, Serilog.ILogger logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("📧 Sending email confirmation to: {Email}",
            notification.Email
            );

        try
        {
            if (!string.IsNullOrEmpty(notification.EmailConfirmationToken))
            {
                await _emailService.SendEmailConfirmationAsync(
                    email:notification.Email,
                    firstName:notification.FirstName,
                    confirmationToken: notification.EmailConfirmationToken,
                    cancellationToken: cancellationToken

                    );
                _logger.Information(
                    "✅ Email confirmation sent successfully to: {Email}",
                    notification.Email);
            }
            else
            {
                await _emailService.SendWelcomeEmailAsync(
                   email: notification.Email,
                   firstName: notification.FirstName,
                   cancellationToken: cancellationToken
               );

                _logger.Information(
                    "✅ Welcome email sent successfully to: {Email}",
                    notification.Email);
            }
        }
        catch(Exception ex)
        {
            _logger.Error(ex,
                 "❌ Failed to send email to: {Email}",
                notification.Email);

            throw;
        }
    }
}
