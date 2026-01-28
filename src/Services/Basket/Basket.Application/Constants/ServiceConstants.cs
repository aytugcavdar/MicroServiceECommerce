namespace Basket.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
        public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
    }

    public static class ValidationRules
    {
        // Add specific rules here as needed, e.g. MaxItemQuantity
        public const int MaxItemQuantity = 100;
    }
}
