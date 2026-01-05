namespace Basket.API.Entities;

public class BasketItem
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public string Color { get; private set; }
    public decimal Price { get; private set; }

    public BasketItem()
    {
        ProductName = string.Empty;
        Color = string.Empty;
    }

    public BasketItem(Guid productId, string productName, int quantity, string color, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Color = color;
        Price = price;
    }

    public decimal TotalPrice => Price * Quantity;

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));

        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));
        if (Quantity - amount < 0)
            throw new InvalidOperationException("Quantity cannot be negative");

        Quantity -= amount;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        Quantity = quantity;
    }
}
