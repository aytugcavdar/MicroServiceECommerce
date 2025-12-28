namespace Basket.API.Constants;

public static class BasketCacheKeys
{
    private const string Prefix = "basket";

    public static string GetBasketKey(string userName)
        => $"{Prefix}:{userName}";

    public static TimeSpan DefaultExpiration
        => TimeSpan.FromHours(24);
}