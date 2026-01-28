namespace Identity.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
        public const string EmailInvalid = "A valid email address is required";
        public const string PasswordComplexity = "Password must be at least 6 characters and contain uppercase, lowercase, and numbers.";
    }

    public static class ValidationRules
    {
        public const int MinPasswordLength = 6;
        public const int MaxNameLength = 50;
        public const int MaxEmailLength = 100;
    }
}
