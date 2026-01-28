namespace Order.Application.Constants;

public static class ValidationMessages
{
    #region Common Messages
    
    public const string Required = "{PropertyName} is required";
    public const string NotEmpty = "{PropertyName} cannot be empty";
    public const string InvalidFormat = "{PropertyName} has invalid format";
    public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
    public const string MinLength = "{PropertyName} must be at least {MinLength} characters";
    public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
    public const string GreaterThanOrEqual = "{PropertyName} must be greater than or equal to {ComparisonValue}";
    public const string LessThan = "{PropertyName} must be less than {ComparisonValue}";
    public const string LessThanOrEqual = "{PropertyName} must be less than or equal to {ComparisonValue}";
    
    #endregion
    
    public static class Order
    {
        public const string TooManyPendingOrders = "You have {PendingCount} pending orders. Maximum allowed is {MaxAllowed}";
        public const string TotalPriceTooLow = "Order total must be at least {MinPrice:C}";
        public const string TotalPriceTooHigh = "Order total cannot exceed {MaxPrice:C}";
        public const string TooManyItems = "Order cannot contain more than {MaxItems} items";
        public const string DuplicateProducts = "Order contains duplicate products";
    }

    public static class OrderItem
    {
        public const string InvalidPrice = "Product price must be positive";
        public const string InvalidQuantity = "Quantity must be between {Min} and {Max}";
    }
}
