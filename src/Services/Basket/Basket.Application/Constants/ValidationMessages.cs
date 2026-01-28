namespace Basket.Application.Constants;

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

    public static class Cart
    {
        public const string MaxItemsExceeded = "Basket cannot contain more than {MaxItems} items";
        public const string MaxTotalExceeded = "Basket total cannot exceed {MaxTotal}";
    }

    public static class CartItem
    {
         public const string QuantityOutOfBounds = "Quantity must be between {Min} and {Max}";
    }
}
