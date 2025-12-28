
using Basket.API.Entities;
using Basket.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    private const string KeyPrefix = "basket:";
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromDays(30);

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var key = GetKey(userName);
        var basketJson = await _redisCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(basketJson))
            return null;

        return JsonSerializer.Deserialize<ShoppingCart>(basketJson);
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var key = GetKey(basket.UserName);
        var basketJson = JsonSerializer.Serialize(basket);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = DefaultExpiration
        };

        await _redisCache.SetStringAsync(key, basketJson, options, cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var key = GetKey(userName);
        await _redisCache.RemoveAsync(key, cancellationToken);
        return true;
    }

    private static string GetKey(string userName) => $"{KeyPrefix}{userName}";
}