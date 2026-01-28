using BuildingBlocks.Messaging.Email;
using MassTransit;
using BuildingBlocks.Messaging.IntegrationEvents;
using Notification.Application.Services;
using Notification.Application.Constants;
using Notification.Domain.Constants;
using Notification.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedIntegrationEvent>
{
    private readonly IEmailService _emailService;
    private readonly INotificationLogRepository _notificationLogRepository;
    private readonly ILogger _logger;

    public UserCreatedConsumer(
        IEmailService emailService,
        INotificationLogRepository notificationLogRepository)
    {
        _emailService = emailService;
        _notificationLogRepository = notificationLogRepository;
        _logger = Log.ForContext<UserCreatedConsumer>();
    }

    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.Information(
            "📬 Received UserCreatedIntegrationEvent: UserId={UserId}, Email={Email}",
            message.UserId,
            message.Email);

        // Log oluştur
        var notificationLog = new NotificationLog(
            type: NotificationType.Email,
            recipientEmail: message.Email,
            subject: NotificationConstants.Templates.Subjects.Welcome,
            content: $"Welcome email for {message.FirstName} {message.LastName}",
            templateName: !string.IsNullOrEmpty(message.EmailConfirmationToken)
                ? NotificationConstants.Templates.EmailConfirmation
                : NotificationConstants.Templates.Welcome,
            eventType: NotificationConstants.Events.UserCreated,
            relatedEntityId: message.UserId
        );

        await _notificationLogRepository.AddAsync(notificationLog);

        try
        {
            bool emailSent;

            // Token varsa confirmation email, yoksa welcome email
            if (!string.IsNullOrEmpty(message.EmailConfirmationToken))
            {
                emailSent = await _emailService.SendEmailConfirmationAsync(
                    email: message.Email,
                    firstName: message.FirstName,
                    confirmationToken: message.EmailConfirmationToken,
                    cancellationToken: context.CancellationToken
                );
            }
            else
            {
                emailSent = await _emailService.SendWelcomeEmailAsync(
                    email: message.Email,
                    firstName: message.FirstName,
                    cancellationToken: context.CancellationToken
                );
            }

            if (emailSent)
            {
                notificationLog.MarkAsSent();
                _logger.Information(
                    "✅ Email sent successfully to {Email}",
                    message.Email);
            }
            else
            {
                notificationLog.MarkAsFailed("Email service returned false");
                _logger.Warning(
                    "⚠️ Email sending failed for {Email}",
                    message.Email);
            }
        }
        catch (Exception ex)
        {
            notificationLog.MarkAsFailed(ex.Message);
            _logger.Error(ex, "❌ Error sending email to {Email}", message.Email);

            // Hata durumunda retry mekanizması
            if (notificationLog.CanRetry())
            {
                throw;
            }
        }
        finally
        {
            await _notificationLogRepository.SaveChangesAsync(context.CancellationToken);
        }
    }
}
