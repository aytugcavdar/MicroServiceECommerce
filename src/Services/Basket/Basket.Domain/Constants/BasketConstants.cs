namespace Basket.Domain.Constants;

public static class BasketConstants
{
    public static class Cart
    {
        public const int MaxItems = 100;
        public const decimal MaxTotal = 100_000m;
    }

    public static class CartItem
    {
         public const int MinQuantity = 1;
         public const int MaxQuantity = 100;
         public const int ProductNameMaxLength = 200;
    }
}
