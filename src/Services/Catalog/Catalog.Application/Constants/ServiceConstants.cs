namespace Catalog.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
        public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
        public const string GreaterThanOrEqualTo = "{PropertyName} cannot be negative";
        public const string LessThanOrEqualTo = "{PropertyName} cannot exceed {ComparisonValue}";
    }

    public static class ValidationRules
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
        public const int PictureFileNameMaxLength = 255;
        public const decimal MaxPrice = 1000000;
        public const int MaxStock = 100000;
    }
}
