using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Entities;

public class InventoryItem : Entity<Guid>
{
    public Guid ProductId { get; set; } 
    public int Stock { get; set; }      

    public InventoryItem()
    {
    }

    public InventoryItem(Guid id, Guid productId, int stock)
    {
        Id = id;
        ProductId = productId;
        Stock = stock;
    }

   
    public bool HasStock(int quantity)
    {
        return Stock >= quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (!HasStock(quantity))
        {
            throw new Exception("Stok yetersiz!"); 
        }
        Stock -= quantity;
    }
}