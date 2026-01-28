namespace Notification.Application.Constants;

public static class ServiceConstants
{
    public static class ValidationMessages
    {
        public const string Required = "{PropertyName} is required";
        public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
    }

    public static class ValidationRules
    {
        public const int MaxTitleLength = 200;
        public const int MaxBodyLength = 1000;
    }
}
