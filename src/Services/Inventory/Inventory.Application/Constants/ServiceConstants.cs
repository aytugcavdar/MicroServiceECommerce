namespace Inventory.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string GreaterThanOrEqualTo = "{PropertyName} must be greater than or equal to {ComparisonValue}";
    }

    public static class ValidationRules
    {
        public const int MinStock = 0;
    }
}
