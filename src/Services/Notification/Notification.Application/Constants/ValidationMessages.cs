namespace Notification.Application.Constants;

public static class ValidationMessages
{
    #region Common Messages
    
    public const string Required = "{PropertyName} is required";
    public const string NotEmpty = "{PropertyName} cannot be empty";
    public const string MaxLength = "{PropertyName} cannot exceed {MaxLength} characters";
    public const string MinLength = "{PropertyName} must be at least {MinLength} characters";
    
    #endregion
}
