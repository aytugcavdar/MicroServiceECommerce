namespace Order.Domain.Constants;

public static class OrderConstants
{
    public static class Order
    {
        public const int StockReservationTimeoutMinutes = 5;
        public const int MaxSagaRetryCount = 3;
        public const int MaxPendingOrdersPerUser = 5;
        public const int MaxOrderItemsCount = 50;
        public const decimal MaxOrderTotalPrice = 1_000_000m;
        public const decimal MinOrderTotalPrice = 10m;
    }

    public static class OrderItem
    {
        public const int MaxQuantityPerItem = 1000;
        public const int MinQuantityPerItem = 1;
        public const int ProductNameMaxLength = 200;
    }

    public static class Address
    {
        public const int StreetMaxLength = 100;
        public const int CityMaxLength = 50;
        public const int StateMaxLength = 50;
        public const int CountryMaxLength = 50;
        public const int ZipCodeMaxLength = 20;
    }
}
