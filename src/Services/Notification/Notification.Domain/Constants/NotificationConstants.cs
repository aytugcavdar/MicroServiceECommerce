namespace Notification.Domain.Constants;

public static class NotificationConstants
{
    public static class Templates
    {
        public const string Welcome = "Welcome";
        public const string EmailConfirmation = "EmailConfirmation";
        
        public static class Subjects
        {
            public const string Welcome = "Welcome to MicroECommerce!";
            public const string EmailConfirmation = "Confirm your email";
        }
    }

    public static class Events
    {
        public const string UserCreated = "UserCreated";
        public const string OrderCreated = "OrderCreated";
    }
}
