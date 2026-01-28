namespace Identity.Domain.Constants;

public static class IdentityConstants
{
    public static class User
    {
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 50;
        public const int PasswordMinLength = 6;
        public const int EmailMaxLength = 100;
        public const int FullNameMaxLength = 100;
    }
}
