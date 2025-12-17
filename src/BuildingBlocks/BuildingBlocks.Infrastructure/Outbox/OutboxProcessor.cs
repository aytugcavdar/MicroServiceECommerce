using BuildingBlocks.Core.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// Outbox mesajlarını işleyen background worker
/// Her 10 saniyede bir çalışır ve işlenmemiş mesajları publish eder
/// </summary>
/// <typeparam name="TDbContext">DbContext tipi</typeparam>
public class OutboxProcessor<TDbContext> : BackgroundService
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor<TDbContext>> _logger;
    private readonly TimeSpan _interval;
    private readonly int _batchSize;
    private readonly int _maxRetryCount;

    public OutboxProcessor(
        IServiceProvider serviceProvider,
        ILogger<OutboxProcessor<TDbContext>> logger,
        TimeSpan? interval = null,
        int batchSize = 10,
        int maxRetryCount = 3)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _interval = interval ?? TimeSpan.FromSeconds(10);
        _batchSize = batchSize;
        _maxRetryCount = maxRetryCount;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "📦 OutboxProcessor started for {DbContextName} - Interval: {Interval}s, BatchSize: {BatchSize}",
            typeof(TDbContext).Name,
            _interval.TotalSeconds,
            _batchSize);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in OutboxProcessor");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("📦 OutboxProcessor stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // İşlenmemiş mesajları al
        var messages = await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOn == null && m.RetryCount < _maxRetryCount)
            .OrderBy(m => m.OccurredOn)
            .Take(_batchSize)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
            return;

        _logger.LogInformation(
            "📦 Processing {Count} outbox messages",
            messages.Count);

        var successCount = 0;
        var failCount = 0;

        foreach (var message in messages)
        {
            try
            {
                // Event tipini bul
                var eventType = Type.GetType(message.Type);
                if (eventType == null)
                {
                    message.MarkAsFailed($"Event type not found: {message.Type}");
                    failCount++;

                    _logger.LogWarning(
                        "⚠️ Event type not found: {Type} - MessageId: {MessageId}",
                        message.Type,
                        message.Id);
                    continue;
                }

                // JSON'ı event'e çevir
                _logger.LogInformation("RAW JSON CONTENT: {Content}", message.Content);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };
                var domainEvent = JsonSerializer.Deserialize(message.Content, eventType, options);
                if (domainEvent == null)
                {
                    message.MarkAsFailed("Failed to deserialize event");
                    failCount++;
                    continue;
                }

                // MediatR ile publish et
                await mediator.Publish(domainEvent, cancellationToken);

                // Başarılı olarak işaretle
                message.MarkAsProcessed();
                successCount++;

                _logger.LogInformation(
                    "✅ Processed outbox message: {MessageId} - {EventType}",
                    message.Id,
                    eventType.Name);
            }
            catch (Exception ex)
            {
                message.MarkAsFailed(ex.Message);
                failCount++;

                _logger.LogError(ex,
                    "❌ Failed to process outbox message: {MessageId} - Retry: {RetryCount}/{MaxRetry}",
                    message.Id,
                    message.RetryCount,
                    _maxRetryCount);
            }
        }

        // Değişiklikleri kaydet
        await dbContext.SaveChangesAsync(cancellationToken);

        if (successCount > 0 || failCount > 0)
        {
            _logger.LogInformation(
                "📊 Outbox processing completed - Success: {SuccessCount}, Failed: {FailCount}",
                successCount,
                failCount);
        }
    }
}
