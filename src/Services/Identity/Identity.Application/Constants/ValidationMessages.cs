namespace Identity.Application.Constants;

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

    public static class User
    {
        public const string InvalidCredentials = "Invalid username or password";
        public const string EmailAlreadyExists = "Email is already in use";
        public const string UsernameAlreadyExists = "Username is already taken";
        public const string PasswordMismatch = "Passwords do not match";
        public const string WeakPassword = "Password must check complexity rules";
        public const string PasswordUppercase = "Password must contain at least one uppercase letter";
        public const string PasswordLowercase = "Password must contain at least one lowercase letter";
        public const string PasswordNumber = "Password must contain at least one number";
    }
}
