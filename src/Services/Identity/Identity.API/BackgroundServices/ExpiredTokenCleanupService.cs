using Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.BackgroundServices;

/// <summary>
/// Expired ve revoke edilmiş refresh token'ları periyodik olarak temizler.
/// Her 6 saatte bir çalışır ve 30 günden eski token'ları siler.
/// </summary>
public class ExpiredTokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredTokenCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval;
    private readonly int _retentionDays;

    public ExpiredTokenCleanupService(
        IServiceProvider serviceProvider,
        ILogger<ExpiredTokenCleanupService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        // appsettings.json'dan oku (varsayılan: 6 saat)
        _cleanupInterval = TimeSpan.FromHours(
            configuration.GetValue<int>("TokenCleanup:IntervalHours", 6));

        // appsettings.json'dan oku (varsayılan: 30 gün)
        _retentionDays = configuration.GetValue<int>("TokenCleanup:RetentionDays", 30);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "🧹 Expired Token Cleanup Service started (Interval: {Interval}, Retention: {Retention} days)",
            _cleanupInterval,
            _retentionDays);

        // İlk çalıştırmayı 1 dakika geciktir (uygulama başlangıcında yük olmaması için)
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredTokensAsync(stoppingToken);

                // Bir sonraki çalıştırmaya kadar bekle
                _logger.LogDebug(
                    "⏰ Next cleanup scheduled in {Interval}",
                    _cleanupInterval);

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Uygulama kapanıyor, normal durum
                _logger.LogInformation("🛑 Token cleanup service is stopping (cancellation requested)");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Critical error in token cleanup service");

                // Hata durumunda 5 dakika bekle ve tekrar dene
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _logger.LogInformation("✅ Expired Token Cleanup Service stopped gracefully");
    }

    private async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🧹 Starting token cleanup process...");

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        try
        {
            // Cutoff tarihi hesapla (örn: 30 gün öncesi)
            var cutoffDate = DateTime.UtcNow.AddDays(-_retentionDays);

            _logger.LogDebug(
                "🔍 Searching for tokens older than {CutoffDate}",
                cutoffDate);

            // Silinecek token'ları bul
            var tokensToDelete = await dbContext.RefreshTokens
                .Where(rt =>
                    // Süresi dolmuş VE eski olanlar
                    rt.ExpiresAt < cutoffDate ||
                    // VEYA revoke edilmiş VE eski olanlar
                    (rt.RevokedAt.HasValue && rt.RevokedAt < cutoffDate))
                .ToListAsync(cancellationToken);

            if (tokensToDelete.Any())
            {
                // Token'ları sil
                dbContext.RefreshTokens.RemoveRange(tokensToDelete);
                int deletedCount = await dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "✅ Successfully cleaned up {Count} expired/revoked refresh tokens",
                    deletedCount);

                // İstatistikleri logla
                var expiredCount = tokensToDelete.Count(t => !t.RevokedAt.HasValue);
                var revokedCount = tokensToDelete.Count(t => t.RevokedAt.HasValue);

                _logger.LogDebug(
                    "📊 Cleanup details - Expired: {ExpiredCount}, Revoked: {RevokedCount}",
                    expiredCount,
                    revokedCount);
            }
            else
            {
                _logger.LogInformation("ℹ️ No expired tokens found to clean up");
            }

            // Toplam aktif token sayısını logla (monitoring için)
            var activeTokenCount = await dbContext.RefreshTokens
                .CountAsync(rt => rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow,
                    cancellationToken);

            _logger.LogDebug(
                "📊 Current active token count: {ActiveCount}",
                activeTokenCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "❌ Failed to cleanup expired tokens. Will retry in next cycle.");
            throw; // Exception'ı yukarı fırlat, ExecuteAsync'te yakalanacak
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛑 Stopping Expired Token Cleanup Service...");
        await base.StopAsync(cancellationToken);
    }
}