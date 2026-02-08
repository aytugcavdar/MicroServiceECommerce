namespace Payment.Domain.Constants;

public static class PaymentConstants
{
    public const decimal MinimumPaymentAmount = 0.01m;
    public const decimal MaximumPaymentAmount = 100000.00m;
    
    public static class ErrorMessages
    {
        public const string InsufficientFunds = "Insufficient funds in the account";
        public const string CardDeclined = "Card was declined by the issuer";
        public const string InvalidCard = "Invalid card information provided";
        public const string ExpiredCard = "Card has expired";
        public const string ProcessingError = "Payment processing error occurred";
        public const string NetworkError = "Network error during payment processing";
    }
}
