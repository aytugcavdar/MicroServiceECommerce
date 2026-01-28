namespace Catalog.Application.Constants;

/// <summary>
/// FluentValidation hata mesajları
/// NOT: {PropertyName}, {MaxLength} gibi placeholderlar FluentValidation tarafından otomatik doldurulur
/// </summary>
public static class ValidationMessages
{
    #region Common Messages
    
    public const string Required = "{PropertyName} is required";
    public const string NotEmpty = "{PropertyName} cannot be empty";
    public const string InvalidFormat = "{PropertyName} has invalid format";
    
    #endregion

    #region Length Messages
    
    public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
    public const string MinLength = "{PropertyName} must be at least {MinLength} characters";
    public const string ExactLength = "{PropertyName} must be exactly {Length} characters";
    
    #endregion

    #region Comparison Messages
    
    public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
    public const string GreaterThanOrEqual = "{PropertyName} must be greater than or equal to {ComparisonValue}";
    public const string LessThan = "{PropertyName} must be less than {ComparisonValue}";
    public const string LessThanOrEqual = "{PropertyName} must be less than or equal to {ComparisonValue}";
    public const string Range = "{PropertyName} must be between {From} and {To}";
    
    #endregion

    #region Business Specific Messages
    
    public static class Product
    {
        public const string NameAlreadyExists = "A product with this name already exists in the category";
        public const string CategoryNotFound = "The specified category does not exist";
        public const string InsufficientStock = "Insufficient stock. Available: {AvailableStock}, Requested: {RequestedQuantity}";
        public const string InvalidPrice = "Price must be a positive value";
        public const string NegativeStock = "Stock quantity cannot be negative";
    }

    public static class Category
    {
        public const string NameAlreadyExists = "A category with this name already exists";
        public const string HasProducts = "Cannot delete category because it contains {ProductCount} product(s)";
        public const string MaxDepthExceeded = "Category hierarchy cannot exceed {MaxDepth} levels";
        public const string CircularReference = "A category cannot be its own parent";
    }
    
    #endregion
}
