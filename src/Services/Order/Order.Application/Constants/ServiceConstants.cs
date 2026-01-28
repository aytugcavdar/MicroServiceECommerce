namespace Order.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
        public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
    }

    public static class ValidationRules
    {
        public const int MaxAddressLength = 250;
    }
}
