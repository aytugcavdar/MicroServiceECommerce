namespace Inventory.Application.Constants;

public static class ValidationMessages
{
    #region Common Messages
    
    public const string Required = "{PropertyName} is required";
    public const string NotEmpty = "{PropertyName} cannot be empty";
    public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
    public const string GreaterThanOrEqual = "{PropertyName} must be greater than or equal to {ComparisonValue}";
    public const string LessThan = "{PropertyName} must be less than {ComparisonValue}";
    public const string LessThanOrEqual = "{PropertyName} must be less than or equal to {ComparisonValue}";
    
    #endregion

    public static class Product // Renamed from Stock to match usage in consumer
    {
        public const string NotFound = "Product not found in inventory";
        public const string InsufficientStock = "Insufficient stock for product. Available: {AvailableStock}, Requested: {RequestedQuantity}";
    }
}
