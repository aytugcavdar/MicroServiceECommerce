using BuildingBlocks.Core.Domain;

namespace Basket.API.Entities;

public class ShoppingCart : Entity<string>, IAggregateRoot
{
    public string UserName { get; private set; }
    public List<BasketItem> Items { get; private set; }

    public ShoppingCart()
    {
        UserName = string.Empty;
        Items = new List<BasketItem>();
    }

    public ShoppingCart(string userName)
    {
        Id = userName;
        UserName = userName;
        Items = new List<BasketItem>();
        CreatedDate = DateTime.UtcNow;
    }

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    // Domain Methods
    public void AddItem(BasketItem item)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(item.Quantity);
        }
        else
        {
            Items.Add(item);
        }
        UpdatedDate = DateTime.UtcNow;
    }

    public void RemoveItem(int productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
            UpdatedDate = DateTime.UtcNow;
        }
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.SetQuantity(quantity);
            UpdatedDate = DateTime.UtcNow;
        }
    }

    public void Clear()
    {
        Items.Clear();
        UpdatedDate = DateTime.UtcNow;
    }

    public bool IsEmpty => !Items.Any();

    public bool HasItem(int productId) => Items.Any(i => i.ProductId == productId);
}